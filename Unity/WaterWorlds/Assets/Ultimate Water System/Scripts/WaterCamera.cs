using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using Internal;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Rendering;

    /// <summary>
    ///     Each camera supposed to see water needs this component attached. Renders all camera-specific maps for the water:
    ///     <list type="bullet">
    ///         <item>Depth Maps</item>
    ///         <item>Displaced water info map</item>
    ///         <item>Volume maps</item>
    ///     </list>
    /// </summary>
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    [AddComponentMenu("Ultimate Water/Water Camera", -1)]
    [ExecuteInEditMode]
    public class WaterCamera : MonoBehaviour
    {
        #region Public Types
        public enum CameraType
        {
            /// <summary>
            /// It's a camera that has attached WaterCamera component.
            /// </summary>
            Normal,

            /// <summary>
            /// It's a camera used for depth rendering etc.
            /// </summary>
            Effect,

            /// <summary>
            /// It's a camera used for rendering water before displaying it in image effect render modes.
            /// </summary>
            RenderHelper
        }

        [System.Serializable]
        public class WaterCameraEvent : UnityEvent<WaterCamera> { }
        #endregion Public Types

        #region Public Variables
        [HideInInspector] public Camera ReflectionCamera;

        public bool RenderWaterDepth
        {
            get { return _RenderWaterDepth; }
            set { _RenderWaterDepth = value; }
        }

        public bool RenderVolumes
        {
            get { return _RenderVolumes; }
            set { _RenderVolumes = value; }
        }

        public float BaseEffectsQuality
        {
            get { return _BaseEffectsQuality; }
            set { _BaseEffectsQuality = value; }
        }

        public CameraType Type
        {
            get { return _WaterCameraType; }
            set { _WaterCameraType = value; }
        }

        public WaterGeometryType GeometryType
        {
            get { return _GeometryType; }
            set { _GeometryType = value; }
        }

        public Rect LocalMapsRect
        {
            get { return _LocalMapsRect; }
        }

        public WaterRenderMode RenderMode
        {
            get { return _RenderMode; }
            set
            {
                _RenderMode = value;
                OnDisable();
                OnEnable();
            }
        }

        public Rect LocalMapsRectPrevious
        {
            get { return _LocalMapsRectPrevious; }
        }

        public Vector4 LocalMapsShaderCoords
        {
            get
            {
                float invWidth = 1.0f / _LocalMapsRect.width;
                return new Vector4(-_LocalMapsRect.xMin * invWidth, -_LocalMapsRect.yMin * invWidth, invWidth, _LocalMapsRect.width);
            }
        }

        public int ForcedVertexCount
        {
            get { return _ForcedVertexCount; }
            set { _ForcedVertexCount = value; }
        }

        public Water ContainingWater
        {
            get { return _BaseCamera == null ? (_SubmersionState != SubmersionState.None ? _ContainingWater : null) : _BaseCamera.ContainingWater; }
        }

        public float WaterLevel
        {
            get { return _WaterLevel; }
        }

        public SubmersionState SubmersionState
        {
            get { return _SubmersionState; }
        }

        public Camera MainCamera
        {
            get { return _MainCamera; }
        }

        public Camera CameraComponent
        {
            get { return _CameraComponent; }
        }
        public Water MainWater
        {
            get
            {
                if (_MainWater != null)
                {
                    return _MainWater;
                }
                else
                {
                    var boundlessWaters = WaterSystem.Instance.BoundlessWaters;

                    if (boundlessWaters.Count != 0)
                        return boundlessWaters[0];

                    var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

                    if (waters.Count != 0)
                        return waters[0];

                    return null;
                }
            }
        }

        public static List<WaterCamera> EnabledWaterCameras
        {
            get { return _EnabledWaterCameras; }
        }

        public int BaseEffectWidth { get { return _PixelWidth; } }
        public int BaseEffectHeight { get { return _PixelHeight; } }

        public bool RenderFlatMasks { get { return _RenderFlatMasks; } }
        public bool IsInsideSubtractiveVolume { get { return _IsInsideSubtractiveVolume; } }

        /// <summary>
        /// Ready to render alternative camera for effects.
        /// </summary>
        public Camera EffectsCamera
        {
            get
            {
                if (_WaterCameraType == CameraType.Normal && _EffectCamera == null)
                    _EffectCamera = CreateEffectsCamera(CameraType.Effect);

                return _EffectCamera;
            }
        }

        public Camera PlaneProjectorCamera
        {
            get
            {
                if (_WaterCameraType == CameraType.Normal && _PlaneProjectorCamera == null)
                    _PlaneProjectorCamera = CreateEffectsCamera(CameraType.Effect);

                return _PlaneProjectorCamera;
            }
        }

        public WaterCameraEvent SubmersionStateChanged
        {
            get { return _SubmersionStateChanged != null ? _SubmersionStateChanged : (_SubmersionStateChanged = new WaterCameraEvent()); }
        }
        public bool IsInsideAdditiveVolume
        {
            get { return _IsInsideAdditiveVolume; }
        }

        public LightWaterEffects EffectsLight
        {
            get { return _EffectsLight; }
            set { _EffectsLight = value; }
        }

        public WaterCameraSubmersion CameraSubmersion
        {
            get { return _CameraSubmersion; }
        }
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Use this method to set a custom list of waters that should be rendered by this WaterCamera. Pass null to revert back to the default behaviour.
        /// </summary>
        /// <param name="waters"></param>
        public void SetCustomWaterRenderList(List<Water> waters)
        {
            _CustomWaterRenderList = waters;
        }

        /// <summary>
        /// Fast and allocation free way to get a WaterCamera component attached to camera.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="forceAdd"></param>
        /// <returns></returns>
        public static WaterCamera GetWaterCamera(Camera camera, bool forceAdd = false)
        {
            WaterCamera waterCamera;

            if (!_WaterCamerasCache.TryGetValue(camera, out waterCamera))
            {
                waterCamera = camera.GetComponent<WaterCamera>();

                if (waterCamera != null)
                    _WaterCamerasCache[camera] = waterCamera;
                else if (forceAdd)
                    _WaterCamerasCache[camera] = camera.gameObject.AddComponent<WaterCamera>();
                else
                    // ReSharper disable once RedundantAssignment
                    _WaterCamerasCache[camera] = waterCamera = null;         // force null reference (Unity uses custom null checks)
            }

            return waterCamera;
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector, SerializeField, FormerlySerializedAs("depthBlitCopyShader")]
        private Shader _DepthBlitCopyShader;

        [HideInInspector, SerializeField, FormerlySerializedAs("shadowEnforcerShader")]
        private Shader _ShadowEnforcerShader;

        [HideInInspector, SerializeField, FormerlySerializedAs("gbuffer0MixShader")]
        private Shader _Gbuffer0MixShader;

        [HideInInspector, SerializeField, FormerlySerializedAs("gbuffer123MixShader")]
        private Shader _Gbuffer123MixShader;

        [HideInInspector, SerializeField, FormerlySerializedAs("finalColorMixShader")]
        private Shader _FinalColorMixShader;

        [HideInInspector, SerializeField, FormerlySerializedAs("deferredReflections")]
        private Shader _DeferredReflections;

        [HideInInspector, SerializeField, FormerlySerializedAs("deferredShading")]
        private Shader _DeferredShading;

        [HideInInspector, SerializeField, FormerlySerializedAs("mergeDisplacementsShader")]
        private Shader _MergeDisplacementsShader;

        [SerializeField, FormerlySerializedAs("renderMode")]
        private WaterRenderMode _RenderMode;

        [SerializeField, FormerlySerializedAs("geometryType")]
        private WaterGeometryType _GeometryType = WaterGeometryType.Auto;

        [SerializeField, FormerlySerializedAs("renderWaterDepth")]
        private bool _RenderWaterDepth = true;

        [Tooltip("Water has a pretty smooth shape so it's often safe to render it's depth in a lower resolution than the rest of the scene. Although the default value is 1.0, you may probably safely use 0.5 and gain some minor performance boost. If you will encounter any artifacts in masking or image effects, set it back to 1.0.")]
        [Range(0.2f, 1.0f), SerializeField, FormerlySerializedAs("baseEffectsQuality")]
        private float _BaseEffectsQuality = 1.0f;

        [SerializeField, FormerlySerializedAs("superSampling")]
        private float _SuperSampling = 1.0f;

        [SerializeField, FormerlySerializedAs("renderVolumes")]
        private bool _RenderVolumes = true;

        [SerializeField, FormerlySerializedAs("renderFlatMasks")]
        private bool _RenderFlatMasks = true;

        [SerializeField, FormerlySerializedAs("forcedVertexCount")]
        private int _ForcedVertexCount;

        [SerializeField, FormerlySerializedAs("submersionStateChanged")]
        private WaterCameraEvent _SubmersionStateChanged;

        [Tooltip("Optional. Deferred rendering mode will try to match profile parameters of this water object as well as possible. It affects only some minor parameters and you may generally ignore this setting. May be removed in the future.")]
        [SerializeField, FormerlySerializedAs("mainWater")]
        private Water _MainWater;

        [SerializeField, FormerlySerializedAs("effectsLight")]
        private LightWaterEffects _EffectsLight;
        #endregion Inspector Variables

        #region Private Variables
        protected Material _DepthBlitCopyMaterial
        {
            get
            {
                return _DepthBlitCopyMaterialCache != null ? _DepthBlitCopyMaterialCache :
                       (_DepthBlitCopyMaterialCache = new Material(_DepthBlitCopyShader) { hideFlags = HideFlags.DontSave });
            }
        }
        private static CommandBuffer _UtilityCommandBuffer
        {
            get
            {
                return _UtilityCommandBufferCache != null ? _UtilityCommandBufferCache : (_UtilityCommandBufferCache =
                    new CommandBuffer { name = "[PW Water] WaterCamera Utility" });
            }
        }
        internal Camera _WaterRenderCamera
        {
            get { return _WaterRenderCameraCache != null ? _WaterRenderCameraCache : (_WaterRenderCameraCache = CreateEffectsCamera(CameraType.RenderHelper)); }
        }
        private RenderTexture _Gbuffer0Tex, _DepthTex2;
        private WaterCamera _BaseCamera;
        private Camera _EffectCamera;
        private Camera _MainCamera;
        private Camera _PlaneProjectorCamera;
        protected Camera _CameraComponent;
        private Material _DepthBlitCopyMaterialCache;
        private RenderTextureFormat _BlendedDepthTexturesFormat;
        private CameraType _WaterCameraType;
        private bool _EffectsEnabled;
        private IWaterImageEffect[] _ImageEffects;
        private Rect _LocalMapsRect;
        private Rect _LocalMapsRectPrevious;
        private Rect _ShadowedWaterRect;
        private int _PixelWidth, _PixelHeight;
        private Mesh _ShadowsEnforcerMesh;
        private Material _ShadowsEnforcerMaterial;
        internal Water _ContainingWater;

        private float _WaterLevel;
        private SubmersionState _SubmersionState;
        private bool _IsInsideSubtractiveVolume;
        private bool _IsInsideAdditiveVolume;
        private Matrix4x4 _LastPlaneProjectorMatrix;
        private WaterCameraIME _WaterCameraIME;
        private List<Water> _CustomWaterRenderList;

        public static event System.Action<WaterCamera> OnGlobalPreCull;

        public static event System.Action<WaterCamera> OnGlobalPostRender;

        public event System.Action<WaterCamera> RenderTargetResized;

        public event System.Action<WaterCamera> Destroyed;

        public event System.Action<WaterCamera> Disabled;

        private static CommandBuffer _UtilityCommandBufferCache;
        private static readonly Dictionary<Camera, WaterCamera> _WaterCamerasCache = new Dictionary<Camera, WaterCamera>();
        private static readonly List<WaterCamera> _EnabledWaterCameras = new List<WaterCamera>();
        private static readonly RenderTargetIdentifier[] _DeferredTargets = { BuiltinRenderTextureType.GBuffer1, BuiltinRenderTextureType.GBuffer2, BuiltinRenderTextureType.Reflections };

        private Camera _WaterRenderCameraCache;
        private CommandBuffer _ImageEffectCommands;
        private RenderTexture _DeferredRenderTarget;
        private Material _Gbuffer0MixMaterial, _Gbuffer123MixMaterial, _FinalColorMixMaterial;

        private readonly MaskModule _MaskModule = new MaskModule();
        private readonly DepthModule _DepthModule = new DepthModule();
        private readonly ForwardModule _ForwardModule = new ForwardModule();

        [SerializeField]
        private WaterCameraSubmersion _CameraSubmersion = new WaterCameraSubmersion();

        public bool _EditSubmersion;
        #endregion Private Variables

        #region Unity Methods
        protected void Awake()
        {
            OnValidate();

            if (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) && _DepthBlitCopyMaterial.passCount > 3)
            {
                _BlendedDepthTexturesFormat = RenderTextureFormat.Depth;
            }
            else
            {
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat) && _BaseEffectsQuality > 0.2f)
                    _BlendedDepthTexturesFormat = RenderTextureFormat.RFloat;
                else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf))
                    _BlendedDepthTexturesFormat = RenderTextureFormat.RHalf;
                else
                    _BlendedDepthTexturesFormat = RenderTextureFormat.R8;
            }

            _Gbuffer0MixMaterial = new Material(_Gbuffer0MixShader) { hideFlags = HideFlags.DontSave };
            _Gbuffer123MixMaterial = new Material(_Gbuffer123MixShader) { hideFlags = HideFlags.DontSave };
            _FinalColorMixMaterial = new Material(_FinalColorMixShader) { hideFlags = HideFlags.DontSave };
        }

        protected void OnEnable()
        {
            _CameraComponent = GetComponent<Camera>();
            _WaterCamerasCache[_CameraComponent] = this;

            if (_WaterCameraType == CameraType.Normal)
            {
                _EnabledWaterCameras.Add(this);
                _ImageEffects = GetComponents<IWaterImageEffect>();

                foreach (var imageEffect in _ImageEffects)
                    imageEffect.OnWaterCameraEnabled();
            }

            RemoveUtilityCommands();
            AddUtilityCommands();

            _MaskModule.OnEnable(this);
            _DepthModule.OnEnable(this);
            _ForwardModule.OnEnable(this);

            _CameraSubmersion.OnEnable(this);
        }

        protected void OnDisable()
        {
            if (_WaterCameraType == CameraType.Normal)
                _EnabledWaterCameras.Remove(this);

            ReleaseImageEffectTemporaryTextures();
            ReleaseTemporaryTextures();

            RemoveUtilityCommands();
            DisableEffects();

            if (_EffectCamera != null)
            {
                _EffectCamera.gameObject.Destroy();
                _EffectCamera = null;
            }

            if (_PlaneProjectorCamera != null)
            {
                _PlaneProjectorCamera.gameObject.Destroy();
                _PlaneProjectorCamera = null;
            }

            if (_DepthBlitCopyMaterialCache != null)
            {
                _DepthBlitCopyMaterialCache.Destroy();
                _DepthBlitCopyMaterialCache = null;
            }

            _ContainingWater = null;

            _MaskModule.OnDisable(this);
            _DepthModule.OnDisable(this);
            _ForwardModule.OnDisable(this);

            _CameraSubmersion.OnDisable();

            if (Disabled != null)
                Disabled(this);
        }

        protected void OnPreCull()
        {
            if (!ShouldRender()) { return; }

#if UNITY_EDITOR
            if (WaterUtilities.IsSceneViewCamera(_CameraComponent))
            {
                // ReSharper disable once LocalVariableHidesMember
                var camera = _CameraComponent.GetComponent<WaterCamera>();
                camera._RenderMode = WaterRenderMode.DefaultQueue;
            }
#endif

            if (OnGlobalPreCull != null)
                OnGlobalPreCull(this);

            if (_WaterCameraType == CameraType.RenderHelper)
            {
                // ReSharper disable once LocalVariableHidesMember
                var camera = _MainCamera.GetComponent<WaterCamera>();
                if (camera != null)
                {
                    camera.RenderWaterDirect();
                }
                return;
            }

            if (_WaterCameraType == CameraType.Normal)
            {
                // on some unity versions, rendered effects are inverted,
                // so we need do compensate by inverting projection matrix
                bool flip = WaterUtilities.IsSceneViewCamera(_CameraComponent) ||
                          (VersionCompatibility.Version >= 560 && _RenderMode == WaterRenderMode.DefaultQueue &&
#if UNITY_5_6_OR_NEWER
                                 _CameraComponent.allowHDR
#else
                                 _CameraComponent.hdr
#endif
                            );

                SetPlaneProjectorMatrix(_RenderMode != WaterRenderMode.DefaultQueue, flip);

                ToggleEffects();

                PrepareToRender();
                SetFallbackTextures();
            }

            if (_EffectsEnabled)
                SetLocalMapCoordinates();

            RenderWaterEffects();

            if (_EffectsLight != null)
                _EffectsLight.PrepareRenderingOnCamera(this);

            if (_RenderMode == WaterRenderMode.DefaultQueue)
                RenderWaterDirect();

            if (!_EffectsEnabled)
            {
                Shader.DisableKeyword("WATER_BUFFERS_ENABLED");
                return;
            }

            Shader.EnableKeyword("WATER_BUFFERS_ENABLED");

            if (_RenderVolumes)
            {
                _MaskModule.Process(this);
            }

            if (_RenderWaterDepth || _RenderMode == WaterRenderMode.ImageEffectDeferred)
            {
                _DepthModule.Process(this);
            }

            if (_ImageEffects != null && Application.isPlaying)
            {
                for (int i = 0; i < _ImageEffects.Length; ++i)
                    _ImageEffects[i].OnWaterCameraPreCull();
            }

            if (_ShadowedWaterRect.xMin < _ShadowedWaterRect.xMax)
                RenderShadowEnforcers();

            if (_RenderMode != WaterRenderMode.DefaultQueue)
            {
                var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

                for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                    waters[waterIndex].Volume.DisableRenderers();

                WaterMaterials.ValidateGlobalWaterDataLookupTex();
            }
        }

        protected void OnPostRender()
        {
            if (!ShouldRender()) { return; }

            ReleaseTemporaryTextures();

            var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

            for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                waters[waterIndex].Renderer.PostRender(this);

            if ((object)_EffectsLight != null)
                _EffectsLight.CleanRenderingOnCamera();

            if (OnGlobalPostRender != null)
                OnGlobalPostRender(this);
        }

        protected void Update()
        {
            if (!ShouldRender()) { return; }
            switch (_RenderMode)
            {
                case WaterRenderMode.ImageEffectDeferred:
                    {
                        ReleaseImageEffectTemporaryTextures();
                        break;
                    }
            }
        }

        private void OnDestroy()
        {
            _WaterCamerasCache.Remove(GetComponent<Camera>());

            if (Destroyed != null)
            {
                Destroyed(this);
                Destroyed = null;
            }
        }

        private void OnValidate()
        {
            _CameraSubmersion.OnValidate();

            _MaskModule.OnValidate(this);
            _DepthModule.OnValidate(this);
            _ForwardModule.OnValidate(this);

            if (_DepthBlitCopyShader == null)
                _DepthBlitCopyShader = Shader.Find("UltimateWater/Depth/Depth Copy");

            if (_ShadowEnforcerShader == null)
                _ShadowEnforcerShader = Shader.Find("UltimateWater/Utility/ShadowEnforcer");

            if (_Gbuffer0MixShader == null)
                _Gbuffer0MixShader = Shader.Find("UltimateWater/Deferred/GBuffer0Mix");

            if (_Gbuffer123MixShader == null)
                _Gbuffer123MixShader = Shader.Find("UltimateWater/Deferred/GBuffer123Mix");

            if (_FinalColorMixShader == null)
                _FinalColorMixShader = Shader.Find("UltimateWater/Deferred/FinalColorMix");

            if (_DeferredReflections == null)
                _DeferredReflections = Shader.Find("Hidden/UltimateWater-Internal-DeferredReflections");

            if (_DeferredShading == null)
                _DeferredShading = Shader.Find("Hidden/UltimateWater-Internal-DeferredShading");

            if (_MergeDisplacementsShader == null)
                _MergeDisplacementsShader = Shader.Find("UltimateWater/Utility/MergeDisplacements");

            if (_WaterCameraIME == null)
            {
                _WaterCameraIME = GetComponent<WaterCameraIME>();
                if (_WaterCameraIME == null)
                {
                    _WaterCameraIME = gameObject.AddComponent<WaterCameraIME>();
                }
            }

            if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
                _RenderWaterDepth = true;

            _CameraComponent = GetComponent<Camera>();

#if UNITY_EDITOR
            ReorderWaterCameraIME();
#endif

            RemoveUtilityCommands();

            if (enabled)
                AddUtilityCommands();

            _WaterCameraIME.enabled = enabled && (_RenderMode == WaterRenderMode.ImageEffectDeferred || _RenderMode == WaterRenderMode.ImageEffectForward);
        }

        private void OnDrawGizmosSelected()
        {
            if (_EditSubmersion)
            {
                _CameraSubmersion.OnDrawGizmos();
            }
        }

        #endregion Unity Methods

        #region Private Methods
#if UNITY_EDITOR
        private void ReorderWaterCameraIME()
        {
            var components = GetComponents<Component>();

            int index = components.Length - 1;

            while (!(components[index] is WaterCameraIME))
            {
                if (components[index] is WaterCamera)
                {
                    while (!(components[index--] is WaterCameraIME))
                        UnityEditorInternal.ComponentUtility.MoveComponentDown(_WaterCameraIME);

                    return;
                }

                --index;
            }

            --index;

            while (!(components[index--] is WaterCamera))
                UnityEditorInternal.ComponentUtility.MoveComponentUp(_WaterCameraIME);
        }
#endif

        internal void ReportShadowedWaterMinMaxRect(Vector2 min, Vector2 max)
        {
            if (_ShadowedWaterRect.xMin > min.x)
                _ShadowedWaterRect.xMin = min.x;

            if (_ShadowedWaterRect.yMin > min.y)
                _ShadowedWaterRect.yMin = min.y;

            if (_ShadowedWaterRect.xMax < max.x)
                _ShadowedWaterRect.xMax = max.x;

            if (_ShadowedWaterRect.yMax < max.y)
                _ShadowedWaterRect.yMax = max.y;
        }

        internal void RenderWaterWithShader(string commandName, RenderTexture target, Shader shader, Water water)
        {
            var commandBuffer = _UtilityCommandBuffer;
            commandBuffer.Clear();
#if UNITY_EDITOR
            commandBuffer.name = commandName;
#endif
            commandBuffer.SetRenderTarget(target);

            water.Renderer.Render(_CameraComponent, _GeometryType, commandBuffer, shader);

            GL.PushMatrix();
            GL.modelview = _CameraComponent.worldToCameraMatrix;
            GL.LoadProjectionMatrix(_CameraComponent.projectionMatrix);

            Graphics.ExecuteCommandBuffer(commandBuffer);

            GL.PopMatrix();
        }

        internal void RenderWaterWithShader(string commandName, RenderTexture target, Shader shader, bool surfaces, bool volumes, bool volumesTwoPass)
        {
            var commandBuffer = _UtilityCommandBuffer;
            commandBuffer.Clear();
#if UNITY_EDITOR
            commandBuffer.name = commandName;
#endif
            commandBuffer.SetRenderTarget(target);

            AddWaterRenderCommands(commandBuffer, shader, surfaces, volumes, volumesTwoPass);

            GL.PushMatrix();
            GL.modelview = _CameraComponent.worldToCameraMatrix;
            GL.LoadProjectionMatrix(_CameraComponent.projectionMatrix);

            Graphics.ExecuteCommandBuffer(commandBuffer);

            GL.PopMatrix();
        }

        internal void AddWaterRenderCommands(CommandBuffer commandBuffer, Shader shader, bool surfaces, bool volumes, bool volumesTwoPass)
        {
            var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

            if (volumes)
            {
                for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                    waters[waterIndex].Renderer.RenderVolumes(commandBuffer, shader, volumesTwoPass);
            }

            if (surfaces)
            {
                for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                    waters[waterIndex].Renderer.Render(_CameraComponent, _GeometryType, commandBuffer, shader);
            }
        }

        internal void AddWaterMasksRenderCommands(CommandBuffer commandBuffer)
        {
            var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

            for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                waters[waterIndex].Renderer.RenderMasks(commandBuffer);
        }

        private void ReleaseTemporaryTextures()
        {
            if (_Gbuffer0Tex != null)
            {
                RenderTexture.ReleaseTemporary(_Gbuffer0Tex);
                _Gbuffer0Tex = null;
            }

            if (_DepthTex2 != null)
            {
                RenderTexture.ReleaseTemporary(_DepthTex2);
                _DepthTex2 = null;
            }
        }

        private void CopyFrom(WaterCamera waterCamera)
        {
            _LocalMapsRect = waterCamera._LocalMapsRect;
            _LocalMapsRectPrevious = waterCamera._LocalMapsRectPrevious;
            _GeometryType = waterCamera._GeometryType;
        }

        private void RenderWaterDirect()
        {
            var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

            for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                waters[waterIndex].Renderer.Render(_CameraComponent, _GeometryType);
        }

        private void RenderWaterEffects()
        {
            var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

            for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                waters[waterIndex].Renderer.RenderEffects(this);
        }

        #region ImageEffectRenderModes

        internal void OnRenderImageCallback(RenderTexture source, RenderTexture destination)
        {
            if (!ShouldRender())
            {
                Graphics.Blit(source, destination);
                return;
            }

            if (_RenderMode != WaterRenderMode.DefaultQueue)
            {
                var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

                for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                    waters[waterIndex].Volume.EnableRenderers();
            }

            switch (_RenderMode)
            {
                case WaterRenderMode.ImageEffectForward:
                    {
                        _ForwardModule.Render(this, source, destination);
                        break;
                    }

                case WaterRenderMode.ImageEffectDeferred:
                    {
#if UNITY_EDITOR && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && !UNITY_5_3 && !UNITY_5_4
                        // that takes care of this warning: "OnRenderImage() possibly didn't write anything to the destination texture!"
                        Graphics.SetRenderTarget(destination);
                        GL.Clear(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
#endif

                        var deferredRenderTarget = RenderTexture.GetTemporary(Mathf.RoundToInt(_CameraComponent.pixelWidth * _SuperSampling) + 1, Mathf.RoundToInt(_CameraComponent.pixelHeight * _SuperSampling), 32, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);      // get a buffer that is slightly larger to ensure that this camera buffers won't be used later this frame
                        deferredRenderTarget.filterMode = FilterMode.Point;
                        source.filterMode = FilterMode.Point;
                        Shader.SetGlobalTexture(ShaderVariables.RefractTex, source);
                        RenderWaterDeferred(deferredRenderTarget, destination);
                        RenderTexture.ReleaseTemporary(deferredRenderTarget);
                        break;
                    }

                default:
                    {
                        Graphics.Blit(source, destination);
                        break;
                    }
            }

            if (_RenderMode != WaterRenderMode.DefaultQueue)
            {
                var waters = _CustomWaterRenderList != null ? _CustomWaterRenderList : WaterSystem.Instance.Waters;

                for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
                    waters[waterIndex].Volume.DisableRenderers();
            }
        }

        private void ReleaseImageEffectTemporaryTextures()
        {
            if (_DeferredRenderTarget != null)
            {
                RenderTexture.ReleaseTemporary(_DeferredRenderTarget);
                _DeferredRenderTarget = null;
            }
        }

        private void AddUtilityCommands()
        {
            if (_ImageEffectCommands == null && _RenderMode == WaterRenderMode.ImageEffectDeferred)
            {
                _ImageEffectCommands = new CommandBuffer { name = "[PW Water] Set Buffers" };
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer0, BuiltinRenderTextureType.GBuffer0);
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer1, BuiltinRenderTextureType.GBuffer1);
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer2, BuiltinRenderTextureType.GBuffer2);
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.Gbuffer3, BuiltinRenderTextureType.Reflections);
#if UNITY_5_6_OR_NEWER
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.WaterlessDepthTexture, BuiltinRenderTextureType.ResolvedDepth);
#else
                _ImageEffectCommands.SetGlobalTexture(ShaderVariables.WaterlessDepthTexture, BuiltinRenderTextureType.Depth);
#endif

                _CameraComponent.RemoveCommandBuffer(CameraEvent.AfterLighting, _ImageEffectCommands);
                _CameraComponent.AddCommandBuffer(CameraEvent.AfterLighting, _ImageEffectCommands);
            }
        }

        private void RemoveUtilityCommands()
        {
            RemoveCommandBuffer(CameraEvent.AfterLighting, "[PW Water] Set Buffers");

            if (_ImageEffectCommands != null)
            {
                _ImageEffectCommands.Dispose();
                _ImageEffectCommands = null;
            }
        }

        /// <summary>
        /// Removes a command buffer by name. It's the most reliable way to remove a command buffer as references may be cleared in the editor.
        /// </summary>
        /// <param name="cameraEvent"></param>
        /// <param name="bufferName"></param>
        private void RemoveCommandBuffer(CameraEvent cameraEvent, string bufferName)
        {
            var buffers = _CameraComponent.GetCommandBuffers(cameraEvent);

            for (int i = buffers.Length - 1; i >= 0; --i)
            {
                if (buffers[i].name == bufferName)
                {
                    _CameraComponent.RemoveCommandBuffer(cameraEvent, buffers[i]);
                    return;
                }
            }
        }

        private void RenderWaterDeferred(RenderTexture temp, RenderTexture target)
        {
            var waterRenderCamera = _WaterRenderCamera;
            waterRenderCamera.CopyFrom(_CameraComponent);
            var mainWater = MainWater;

            if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
            {
                _FinalColorMixMaterial.SetMatrix(ShaderVariables.UnityMatrixVPInverse, Matrix4x4.Inverse(GL.GetGPUProjectionMatrix(_CameraComponent.projectionMatrix, true) * _CameraComponent.worldToCameraMatrix));
                _Gbuffer123MixMaterial.SetFloat(ShaderVariables.DepthClipMultiplier, -1.0f);

                if (_Gbuffer0Tex == null)
                {
                    _Gbuffer0Tex = RenderTexture.GetTemporary(_CameraComponent.pixelWidth + 1, _CameraComponent.pixelHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                    _Gbuffer0Tex.filterMode = FilterMode.Point;
                }

                if (_DepthTex2 == null)
                {
                    _DepthTex2 = RenderTexture.GetTemporary(_CameraComponent.pixelWidth + 1, _CameraComponent.pixelHeight, _BlendedDepthTexturesFormat == RenderTextureFormat.Depth ? 32 : 0, _BlendedDepthTexturesFormat, RenderTextureReadWrite.Linear);
                    _DepthTex2.filterMode = FilterMode.Point;
                }

                var utilityCommandBuffer = _UtilityCommandBuffer;
                utilityCommandBuffer.Clear();
                utilityCommandBuffer.name = "[PW Water] Blend Deferred Results";

                // depth
                var depthBlitCopyMaterial = _DepthBlitCopyMaterial;
                utilityCommandBuffer.SetRenderTarget(_DepthTex2);
                utilityCommandBuffer.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, depthBlitCopyMaterial, 0, _BlendedDepthTexturesFormat == RenderTextureFormat.Depth ? 5 : 2);

                // gbuffer 0, 1, 2, 3
                utilityCommandBuffer.Blit(BuiltinRenderTextureType.GBuffer0, _Gbuffer0Tex, _Gbuffer0MixMaterial, 0);
                utilityCommandBuffer.SetRenderTarget(_DeferredTargets, BuiltinRenderTextureType.Reflections);
                utilityCommandBuffer.DrawMesh(Quads.BipolarXY, Matrix4x4.identity, _Gbuffer123MixMaterial, 0);

                // final color
                utilityCommandBuffer.SetRenderTarget(target);
                utilityCommandBuffer.SetGlobalTexture("_WaterColorTex", BuiltinRenderTextureType.CameraTarget);
                utilityCommandBuffer.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, _FinalColorMixMaterial, 0, 0, mainWater != null ? mainWater.Renderer.PropertyBlock : null);

                utilityCommandBuffer.SetGlobalTexture("_CameraDepthTexture", _DepthTex2);
                utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture0", _Gbuffer0Tex);
                utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture1", BuiltinRenderTextureType.GBuffer1);
                utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture2", BuiltinRenderTextureType.GBuffer2);
                utilityCommandBuffer.SetGlobalTexture("_CameraGBufferTexture3", BuiltinRenderTextureType.Reflections);
                utilityCommandBuffer.SetGlobalTexture("_CameraReflectionsTexture", BuiltinRenderTextureType.Reflections);

                waterRenderCamera.AddCommandBuffer(CameraEvent.AfterEverything, utilityCommandBuffer);
            }

            var originalDeferredReflections = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredReflections);
            var originalDeferredShading = GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredShading);
            var originalShaderModeReflections = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections);
            var originalShaderModeShading = GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredShading);

            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, _DeferredReflections);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, _DeferredShading);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, BuiltinShaderMode.UseCustom);

            if (mainWater != null)
                Shader.SetGlobalVector("_MainWaterWrapSubsurfaceScatteringPack", mainWater.Renderer.PropertyBlock.GetVector("_WrapSubsurfaceScatteringPack"));

            var effectWaterCamera = waterRenderCamera.GetComponent<WaterCamera>();
            effectWaterCamera.CopyFrom(this);

            waterRenderCamera.enabled = false;
            waterRenderCamera.clearFlags = CameraClearFlags.Color;
            waterRenderCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            waterRenderCamera.depthTextureMode = DepthTextureMode.Depth;
            waterRenderCamera.renderingPath = RenderingPath.DeferredShading;
#if UNITY_5_6_OR_NEWER
            waterRenderCamera.allowHDR = true;
#else
            waterRenderCamera.hdr = true;
#endif
            waterRenderCamera.targetTexture = temp;
            waterRenderCamera.cullingMask = (1 << WaterProjectSettings.Instance.WaterLayer);
            waterRenderCamera.Render();
            waterRenderCamera.targetTexture = null;

            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredReflections, originalDeferredReflections);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, originalDeferredShading);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredReflections, originalShaderModeReflections);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, originalShaderModeShading);

            Shader.SetGlobalTexture("_CameraDepthTexture", _DepthTex2);

            if (_RenderMode == WaterRenderMode.ImageEffectDeferred)
                waterRenderCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, _UtilityCommandBufferCache);
        }

        #endregion ImageEffectRenderModes

        private void EnableEffects()
        {
            if (_WaterCameraType != CameraType.Normal)
                return;

            _PixelWidth = Mathf.RoundToInt(_CameraComponent.pixelWidth);
            _PixelHeight = Mathf.RoundToInt(_CameraComponent.pixelHeight);

            if (WaterProjectSettings.Instance.SinglePassStereoRendering)
                _PixelWidth = Mathf.CeilToInt((_PixelWidth << 1) / 256.0f) * 256;

            _EffectsEnabled = true;

            if (_RenderWaterDepth || _RenderVolumes)
                _CameraComponent.depthTextureMode |= DepthTextureMode.Depth;
        }

        private void DisableEffects()
        {
            _EffectsEnabled = false;
        }

        protected Camera CreateEffectsCamera(CameraType type)
        {
            var effectCameraGo = new GameObject(name + " Water Effects Camera") { hideFlags = HideFlags.HideAndDontSave };

            var effectCamera = effectCameraGo.AddComponent<Camera>();
            effectCamera.enabled = false;
            effectCamera.useOcclusionCulling = false;

            var effectWaterCamera = effectCameraGo.AddComponent<WaterCamera>();
            effectWaterCamera._WaterCameraType = type;
            effectWaterCamera._MainCamera = _CameraComponent;
            effectWaterCamera._BaseCamera = this;

            _EnabledWaterCameras.Remove(effectWaterCamera);

            return effectCamera;
        }

        private void RenderShadowEnforcers()
        {
            if (_ShadowsEnforcerMesh == null)
            {
                _ShadowsEnforcerMesh = new Mesh
                {
                    name = "Water Shadow Enforcer",
                    hideFlags = HideFlags.DontSave,
                    vertices = new Vector3[4]
                };
                _ShadowsEnforcerMesh.SetIndices(new[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
                _ShadowsEnforcerMesh.UploadMeshData(true);

                _ShadowsEnforcerMaterial = new Material(_ShadowEnforcerShader) { hideFlags = HideFlags.DontSave };
            }

            var bounds = new Bounds();

            float distance = QualitySettings.shadowDistance;
            Vector3 a = _CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMin, _ShadowedWaterRect.yMin, 1.0f)).GetPoint(distance * 1.5f);
            Vector3 b = _CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMax, _ShadowedWaterRect.yMax, 1.0f)).GetPoint(distance * 1.5f);
            SetBoundsMinMaxComponentWise(ref bounds, a, b);
            bounds.Encapsulate(_CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMin, _ShadowedWaterRect.yMax, 1.0f)).GetPoint(distance * 0.3f));
            bounds.Encapsulate(_CameraComponent.ViewportPointToRay(new Vector3(_ShadowedWaterRect.xMax, _ShadowedWaterRect.yMin, 1.0f)).GetPoint(distance * 0.3f));
            _ShadowsEnforcerMesh.bounds = bounds;

            Graphics.DrawMesh(_ShadowsEnforcerMesh, Matrix4x4.identity, _ShadowsEnforcerMaterial, 0);
        }

        private void PrepareToRender()
        {
            // reset shadowed water rect
            _ShadowedWaterRect = new Rect(1.0f, 1.0f, -1.0f, -1.0f);

            // find water
            var waterEnterTolerance = 4.0f * _CameraComponent.nearClipPlane * Mathf.Tan(0.5f * _CameraComponent.fieldOfView * Mathf.Deg2Rad);
            var newWater = Water.FindWater(transform.position,
                waterEnterTolerance,
                _CustomWaterRenderList,
                out _IsInsideSubtractiveVolume,
                out _IsInsideAdditiveVolume);

            // if the water changed
            if (newWater != _ContainingWater)
            {
                if (_ContainingWater != null && _SubmersionState != SubmersionState.None)
                {
                    _SubmersionState = SubmersionState.None;
                    SubmersionStateChanged.Invoke(this);
                }

                _ContainingWater = newWater;
                _SubmersionState = SubmersionState.None;

                if (newWater != null && newWater.Volume.Boundless)
                {
                    _CameraSubmersion.Create();
                }
            }
            else
            {
                // find new submersion state
                var state = _CameraSubmersion.State;
                if (state != _SubmersionState)
                {
                    _SubmersionState = state;
                    SubmersionStateChanged.Invoke(this);
                }
            }
        }

        private static void SetFallbackTextures()
        {
            Shader.SetGlobalTexture(ShaderVariables.UnderwaterMask, DefaultTextures.Get(Color.clear));
            Shader.SetGlobalTexture(ShaderVariables.DisplacementsMask, DefaultTextures.Get(Color.white));
        }

        private void ToggleEffects()
        {
            var containsWater = WaterSystem.IsWaterPossiblyVisible();

            if (!_EffectsEnabled)
            {
                if (containsWater)
                {
                    EnableEffects();
                }
            }
            else if (!containsWater)
            {
                DisableEffects();
            }

            int pixelWidth = Mathf.RoundToInt(_CameraComponent.pixelWidth);
            int pixelHeight = Mathf.RoundToInt(_CameraComponent.pixelHeight);

            if (WaterProjectSettings.Instance.SinglePassStereoRendering)
                pixelWidth = Mathf.CeilToInt((pixelWidth << 1) / 256.0f) * 256;

            if (_EffectsEnabled && (pixelWidth != _PixelWidth || pixelHeight != _PixelHeight))
            {
                DisableEffects();
                EnableEffects();

                if (RenderTargetResized != null)
                    RenderTargetResized(this);
            }
        }

        private void SetPlaneProjectorMatrix(bool isRenderTarget, bool flipY)
        {
            var planeProjectorCamera = PlaneProjectorCamera;

            Shader.SetGlobalMatrix("_WaterProjectorPreviousVP", _LastPlaneProjectorMatrix);

            planeProjectorCamera.CopyFrom(_CameraComponent);
            planeProjectorCamera.renderingPath = RenderingPath.Forward;
            planeProjectorCamera.ResetProjectionMatrix();

            planeProjectorCamera.projectionMatrix = _CameraComponent.projectionMatrix;

            var projection = flipY ? _CameraComponent.projectionMatrix * Matrix4x4.Scale(new Vector3(1.0f, -1.0f, 1.0f)) : _CameraComponent.projectionMatrix;
            _LastPlaneProjectorMatrix = GL.GetGPUProjectionMatrix(projection, isRenderTarget) * _CameraComponent.worldToCameraMatrix;

            Shader.SetGlobalMatrix("_WaterProjectorVP", _LastPlaneProjectorMatrix);
        }

        private void SetLocalMapCoordinates()
        {
            int resolution = Mathf.NextPowerOfTwo((_CameraComponent.pixelWidth + _CameraComponent.pixelHeight) >> 1);
            float maxHeight = 0.0f;
            float maxWaterLevel = 0.0f;

            var waters = WaterSystem.Instance.Waters;

            for (int waterIndex = waters.Count - 1; waterIndex >= 0; --waterIndex)
            {
                var water = waters[waterIndex];
                maxHeight += water.MaxVerticalDisplacement;

                float posY = water.transform.position.y;
                if (maxWaterLevel < posY)
                    maxWaterLevel = posY;
            }

            // place camera
            Vector3 thisCameraPosition = _CameraComponent.transform.position;
            Vector3 thisCameraForward = _CameraComponent.transform.forward;
            float forwardFactor = Mathf.Min(1.0f, thisCameraForward.y + 1.0f);

            float size1 = Mathf.Abs(thisCameraPosition.y) * (1.0f + 7.0f * Mathf.Sqrt(forwardFactor));
            float size2 = maxHeight * 2.5f;
            float size = size1 > size2 ? size1 : size2;

            if (size < 20.0f)
                size = 20.0f;

            Vector3 effectCameraPosition = new Vector3(thisCameraPosition.x + thisCameraForward.x * size * 0.4f, 0.0f, thisCameraPosition.z + thisCameraForward.z * size * 0.4f);

            _LocalMapsRectPrevious = _LocalMapsRect;

            float halfPixelSize = size / resolution;
            _LocalMapsRect = new Rect((effectCameraPosition.x - size) + halfPixelSize, (effectCameraPosition.z - size) + halfPixelSize, 2.0f * size, 2.0f * size);

            float invWidthPrevious = 1.0f / _LocalMapsRectPrevious.width;
            Shader.SetGlobalVector(ShaderVariables.LocalMapsCoordsPrevious, new Vector4(-_LocalMapsRectPrevious.xMin * invWidthPrevious, -_LocalMapsRectPrevious.yMin * invWidthPrevious, invWidthPrevious, _LocalMapsRectPrevious.width));

            float invWidth = 1.0f / _LocalMapsRect.width;
            Shader.SetGlobalVector(ShaderVariables.LocalMapsCoords, new Vector4(-_LocalMapsRect.xMin * invWidth, -_LocalMapsRect.yMin * invWidth, invWidth, _LocalMapsRect.width));
        }
        #endregion Private Methods

        #region Helper Methods
        private bool ShouldRender()
        {
            var shouldRender = true;

            shouldRender &= enabled && Application.isPlaying;
            shouldRender &= WaterSystem.IsWaterPossiblyVisible();
            shouldRender &= !WaterUtilities.IsSceneViewCamera(_CameraComponent) || WaterProjectSettings.Instance.RenderInSceneView;

            return shouldRender;
        }

        private static void SetBoundsMinMaxComponentWise(ref Bounds bounds, Vector3 a, Vector3 b)
        {
            if (a.x > b.x)
            {
                float t = b.x;
                b.x = a.x;
                a.x = t;
            }

            if (a.y > b.y)
            {
                float t = b.y;
                b.y = a.y;
                a.y = t;
            }

            if (a.z > b.z)
            {
                float t = b.z;
                b.z = a.z;
                a.z = t;
            }

            bounds.SetMinMax(a, b);
        }
        #endregion Helper Methods
    }
}