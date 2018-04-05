namespace UltimateWater
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    /// Adds caustics effect to a directional light.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public sealed class LightWaterEffects : MonoBehaviour
    {
        #region Public Types
        public enum CausticsMode
        {
            None,
            ProjectedTexture,
            Raymarching
        }
        #endregion Public Types

        #region Public Variables
        public Light UnityLight
        {
            get { return _LocalLight; }
        }
        public CausticsMode Mode
        {
            get { return _CausticsMode; }
            set
            {
                _CausticsMode = value;
                if (_CausticsMode == CausticsMode.None)
                {
                    ResetCausticMaps();
                    // ReSharper disable once DelegateSubtraction
                    Camera.onPreCull -= OnSomeCameraGlobalPreCull;
                }
                else
                {
                    if (_RenderCamera == null)
                    {
                        CreateCausticsCamera();
                    }

                    if (Camera.onPreCull != null) Camera.onPreCull -= OnSomeCameraGlobalPreCull;
                    Camera.onPreCull += OnSomeCameraGlobalPreCull;
                }
            }
        }

        [Range(0.0f, 3.0f)]
        public float Intensity = 1.0f;
        public bool CastShadows = true;
        public Texture2D ProjectedTexture;

        [SerializeField]
        public float UvScale = 1.0f;

        [Range(0.0f, 0.25f)]
        public float ScrollSpeed = 0.01f;

        [Range(0.0f, 8.0f)]
        public float Distortions1 = 1.0f;

        [Range(0.0f, 8.0f)]
        public float Distortions2 = 1.0f;

        [Tooltip("Optional.")]
        [SerializeField]
        public Transform ScrollDirectionPointer;
        #endregion Public Variables

        #region Private Variables
        [HideInInspector] [SerializeField] private Shader _WorldPosShader;
        [HideInInspector] [SerializeField] private Shader _CausticsMapShader;
        [HideInInspector] [SerializeField] private Shader _NormalMapperShader;
        [HideInInspector] [SerializeField] private Shader _CausticUtilShader;

        [SerializeField] private CausticsMode _CausticsMode = CausticsMode.ProjectedTexture;

        [SerializeField]
        private LayerMask _CausticReceiversMask = int.MaxValue;

        [SerializeField]
        private Blur _Blur;

        [Tooltip("Causes minor allocation per frame (no way around it), but makes caustics rendering a lot faster. Disable it, if you don't use terrains.")]
        [SerializeField]
        private bool _SkipTerrainTrees = true;

        private Camera _RenderCamera;
        private WaterCamera _WaterCamera;
        private Material _CausticUtilMat;
        private Light _LocalLight;
        private Vector2 _Offset;
        private Vector2 _Scroll;
        private bool _RenderingPrepared;
        private bool[] _TerrainSettingTemp;
        private int _Id;
        private CommandBuffer _CopyShadowmap;

        private RenderTexture _WorldPosMap;
        private RenderTexture _CausticsMap;

        public static readonly List<LightWaterEffects> Lights = new List<LightWaterEffects>();

        private static int _ShadowmapId;
        #endregion Private Variables

        #region Unity Messages

        private void Awake()
        {
            _ShadowmapId = ShaderVariables.WaterShadowmap;
            _TerrainSettingTemp = new bool[32];

            _LocalLight = GetComponent<Light>();

            _CausticUtilMat = new Material(_CausticUtilShader) { hideFlags = HideFlags.DontSave };
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            Lights.Add(this);
            OnValidate();

            if (_CausticsMode != CausticsMode.None)
            {
                CreateCausticsCamera();
            }
            else
            {
                ResetCausticMaps();
            }

            _Id = Lights.Count - 1;

            // ReSharper disable once DelegateSubtraction
            Camera.onPreCull -= OnSomeCameraGlobalPreCull;
            Camera.onPreCull += OnSomeCameraGlobalPreCull;
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            // ReSharper disable once DelegateSubtraction
            Camera.onPreCull -= OnSomeCameraGlobalPreCull;

            ResetCausticMaps();

            Lights.Remove(this);

            TextureUtility.Release(ref _WorldPosMap);
            TextureUtility.Release(ref _CausticsMap);

            if (_RenderCamera != null)
            {
                _RenderCamera.gameObject.Destroy();
                _RenderCamera = null;
            }
            if (_WaterCamera != null)
            {
                _WaterCamera.gameObject.Destroy();
                _WaterCamera = null;
            }
        }

        private void Update()
        {
            if (_RenderCamera == null)
            {
                CreateCausticsCamera();
            }
            if (ScrollDirectionPointer != null)
            {
                Vector3 forward = ScrollDirectionPointer.forward;
                float t = ScrollSpeed * UvScale * Time.deltaTime;
                _Scroll.x += forward.x * t;
                _Scroll.y += forward.z * t;
            }
            else
            {
                float t = 0.7f * ScrollSpeed * UvScale * Time.deltaTime;
                _Scroll.x += t;
                _Scroll.y += t;
            }
        }

        private void OnValidate()
        {
            if (_WorldPosShader == null)
                _WorldPosShader = Shader.Find("UltimateWater/Caustics/WorldPos");

            if (_CausticsMapShader == null)
                _CausticsMapShader = Shader.Find("UltimateWater/Caustics/Map");

            if (_NormalMapperShader == null)
                _NormalMapperShader = Shader.Find("UltimateWater/Caustics/NormalMapper");

            if (_CausticUtilShader == null)
                _CausticUtilShader = Shader.Find("UltimateWater/Caustics/Utility");

            _Blur.Validate();

            if (_CausticsMode == CausticsMode.None)
            {
                ResetCausticMaps();
            }
        }
        #endregion Unity Messages

        #region Public Methods
        public void PrepareRenderingOnCamera(WaterCamera targetCamera)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (_RenderingPrepared || !isActiveAndEnabled)
                return;

            _RenderingPrepared = true;

            if (CastShadows)
                PrepareShadows(targetCamera.CameraComponent);

            PrepareCaustics(targetCamera);
        }

        public void CleanRenderingOnCamera()
        {
            if (!_RenderingPrepared)
                return;

            _RenderingPrepared = false;

            if (_CopyShadowmap != null)
                _LocalLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
        }

        public void AddWorldSpaceOffset(Vector3 offset)
        {
            if (!isActiveAndEnabled)
                return;

            _Offset.x += Vector3.Dot(offset, _RenderCamera.transform.right) * UvScale / (_RenderCamera.orthographicSize * 2.0f);
            _Offset.y += Vector3.Dot(offset, _RenderCamera.transform.up) * UvScale / (_RenderCamera.orthographicSize * 2.0f);
        }
        #endregion Public Methods

        #region Private Methods
        private void OnSomeCameraGlobalPreCull(Camera cameraComponent)
        {
            if (_RenderingPrepared)
            {
                transform.position = new Vector3(_Id, 6.137f, 0.0f);
            }
        }

        private void CreateCausticsCamera()
        {
            if (_CausticsMode == CausticsMode.ProjectedTexture)
            {
                _CausticsMap = new RenderTexture(256, 256, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
                {
                    hideFlags = HideFlags.DontSave,
                    wrapMode = TextureWrapMode.Repeat,
                    name = "[UWS] LightWaterEffects - Caustics Map"
                };
            }
            else
            {
                _WorldPosMap = new RenderTexture(256, 256, 32, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear)
                {
                    hideFlags = HideFlags.DontSave,
                    wrapMode = TextureWrapMode.Clamp,
                    name = "[UWS] LightWaterEffects - WorldPosMap"
                };

                _CausticsMap = new RenderTexture(512, 512, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
                {
                    hideFlags = HideFlags.DontSave,
                    wrapMode = TextureWrapMode.Clamp,
                    name = "[UWS] LightWaterEffects - CausticsMap"
                };
            }

            var renderCameraGo = new GameObject("Caustic Camera") { hideFlags = HideFlags.DontSave };

            renderCameraGo.transform.position = transform.position;
            renderCameraGo.transform.rotation = transform.rotation;

            _RenderCamera = renderCameraGo.AddComponent<Camera>();
            _RenderCamera.enabled = false;
            _RenderCamera.orthographic = true;

            _RenderCamera.orthographicSize = 85;
            _RenderCamera.farClipPlane = 5000;
            _RenderCamera.depthTextureMode = DepthTextureMode.None;

#if UNITY_5_6_OR_NEWER
            _RenderCamera.allowHDR = true;
#else
            _RenderCamera.hdr = true;
#endif
            _RenderCamera.useOcclusionCulling = false;
            _RenderCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            _RenderCamera.renderingPath = RenderingPath.VertexLit;

            _WaterCamera = renderCameraGo.AddComponent<WaterCamera>();
            _WaterCamera.RenderWaterDepth = false;

            _WaterCamera.RenderVolumes = false;
            _WaterCamera.Type = WaterCamera.CameraType.Effect;
            _WaterCamera.GeometryType = WaterGeometryType.UniformGrid;

            renderCameraGo.hideFlags |= HideFlags.HideInHierarchy;
        }

        private void PrepareShadows(Camera cameraComponent)
        {
            if (_CopyShadowmap == null)
            {
                _CopyShadowmap = new CommandBuffer { name = "[UWS] LightWaterEffects._CopyShadowmap" };
            }

            Shader.SetGlobalTexture(_ShadowmapId, DefaultTextures.Get(Color.white));

            _CopyShadowmap.Clear();
            _CopyShadowmap.GetTemporaryRT(_ShadowmapId, cameraComponent.pixelWidth, cameraComponent.pixelHeight, 32, FilterMode.Point,
                RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            _CopyShadowmap.Blit(BuiltinRenderTextureType.CurrentActive, _ShadowmapId);
            _CopyShadowmap.ReleaseTemporaryRT(_ShadowmapId);

            _LocalLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
            _LocalLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, _CopyShadowmap);
        }

        private void PrepareCaustics(WaterCamera waterCamera)
        {
            switch (_CausticsMode)
            {
                case CausticsMode.ProjectedTexture:
                    UpdateCausticsCameraPosition(waterCamera);
                    RenderProjectedTextureCaustics();
                    break;

                case CausticsMode.Raymarching:
                    UpdateCausticsCameraPosition(waterCamera);
                    RenderRaymarchedCaustics(waterCamera);
                    break;
            }
        }

        private void UpdateCausticsCameraPosition(WaterCamera waterCamera)
        {
            Vector2 center = waterCamera.LocalMapsRect.center;
            _RenderCamera.transform.position = new Vector3(center.x, 0.0f, center.y) - transform.forward * Mathf.Max(Mathf.Abs(waterCamera.transform.position.y * 2.2f), 300.0f);
            _RenderCamera.transform.rotation = transform.rotation;
        }

        private void RenderProjectedTextureCaustics()
        {
            _RenderCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
            _WaterCamera.RenderWaterWithShader("[PW Water] Caustics Normal Map", _CausticsMap, _NormalMapperShader, true, false, false);

            Vector3 pos = _RenderCamera.transform.position;
            float x = Vector3.Dot(pos, _RenderCamera.transform.right) * UvScale / (_RenderCamera.orthographicSize * 3.5f);
            float y = Vector3.Dot(pos, _RenderCamera.transform.up) * UvScale / (_RenderCamera.orthographicSize * 2.0f);

            Shader.SetGlobalTexture("_CausticsMap", ProjectedTexture);
            Shader.SetGlobalTexture("_CausticsDistortionMap", _CausticsMap);
            Shader.SetGlobalFloat("_CausticsMultiplier", Intensity * 5.0f);
            Shader.SetGlobalVector("_CausticsOffsetScale", new Vector4(_Offset.x + _Scroll.x + x, _Offset.y + _Scroll.y + y, UvScale, Distortions1 * 0.02f));
            Shader.SetGlobalVector("_CausticsOffsetScale2", new Vector4(_Offset.x - _Scroll.x + x + 0.5f, _Offset.y - _Scroll.y + y, UvScale, Distortions2 * 0.02f));
            Shader.SetGlobalMatrix("_CausticsMapProj", GL.GetGPUProjectionMatrix(_RenderCamera.projectionMatrix, true) * _RenderCamera.worldToCameraMatrix);
        }

        private void RenderRaymarchedCaustics(WaterCamera waterCamera)
        {
            _CausticUtilMat.SetMatrix("_InvProjMatrix", Matrix4x4.Inverse(_RenderCamera.projectionMatrix * _RenderCamera.worldToCameraMatrix));

            Graphics.SetRenderTarget(_WorldPosMap);
            GL.Clear(true, true, new Color(1.0f, 0.0f, 0.0f, 0.0f), 1.0f);

            Terrain[] terrains = null;

            if (_SkipTerrainTrees)
            {
                terrains = Terrain.activeTerrains;

                if (_TerrainSettingTemp.Length < terrains.Length)
                    System.Array.Resize(ref _TerrainSettingTemp, terrains.Length * 2);

                for (int i = 0; i < terrains.Length; ++i)
                {
                    _TerrainSettingTemp[i] = terrains[i].drawTreesAndFoliage;
                    terrains[i].drawTreesAndFoliage = false;
                }
            }

            _WaterCamera.enabled = false;
            _RenderCamera.orthographicSize = waterCamera.LocalMapsRect.width * 0.6f;
            _RenderCamera.clearFlags = CameraClearFlags.Depth;
            _RenderCamera.cullingMask = _CausticReceiversMask;
            _RenderCamera.targetTexture = _WorldPosMap;
            _RenderCamera.RenderWithShader(_WorldPosShader, "RenderType");

            if (_SkipTerrainTrees)
            {
                // ReSharper disable once PossibleNullReferenceException
                for (int i = 0; i < terrains.Length; ++i)
                    terrains[i].drawTreesAndFoliage = _TerrainSettingTemp[i];
            }

            Shader.SetGlobalTexture("_WorldPosMap", _WorldPosMap);
            Shader.SetGlobalVector("_CausticLightDir", transform.forward);
            Shader.SetGlobalFloat("_CausticLightIntensity", _LocalLight.intensity * Intensity * 1.5f);

            _WaterCamera.enabled = true;
            _RenderCamera.clearFlags = CameraClearFlags.Color;
            _RenderCamera.cullingMask = (1 << WaterProjectSettings.Instance.WaterLayer);
            _RenderCamera.targetTexture = _CausticsMap;
            _RenderCamera.RenderWithShader(_CausticsMapShader, "CustomType");

            //blur.TotalSize = baseBlurSize / waterCamera.LocalMapsRect.width;
            _Blur.Apply(_CausticsMap);

            Graphics.Blit(null, _CausticsMap, _CausticUtilMat, 1);

            Shader.SetGlobalTexture("_CausticsMap", _CausticsMap);
            Shader.SetGlobalFloat("_CausticsMultiplier", 1.0f);
            Shader.SetGlobalMatrix("_CausticsMapProj", GL.GetGPUProjectionMatrix(_RenderCamera.projectionMatrix, true) * _RenderCamera.worldToCameraMatrix);
        }
        private void ResetCausticMaps()
        {
            Shader.SetGlobalFloat("_CausticsMultiplier", 0.0f);
        }
        #endregion Private Methods
    }
}