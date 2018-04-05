namespace UltimateWater
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    /// Simulates wave propagation in defined space
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [AddComponentMenu("Ultimate Water/Dynamic/Water Simulation Area")]
    public class WaterSimulationArea : MonoBehaviour, ILocalDisplacementRenderer
    {
        #region Public Variables
        /// <summary>
        /// Simulation settings profile
        /// </summary>
        public WaterRipplesProfile Profile
        {
            get { return _Profile; }
            set
            {
                _Profile = value;
                UpdateShaderVariables();
            }
        }

        /// <summary>
        /// Simulation texture resolution in pixels
        /// </summary>
        public Vector2 Resolution
        {
            get
            {
                var result = Size * _PixelsPerUnit;
                return new Vector2((int)result.x, (int)result.y);
            }
        }

        /// <summary>
        /// Simulation size in Unity units
        /// </summary>
        public Vector2 Size
        {
            // note: Simulation Area uses a Unity Plane as render surface,
            // and the default dimensions of the plane are (10, 0, 10) unity units, with scale equal to (1, 1, 1)

            get
            {
                const float planeSize = 10.0f;
                return new Vector2(transform.localScale.x * planeSize, transform.localScale.z * planeSize);
            }
            set
            {
                const float planeSizeInv = 0.1f;
                transform.localScale = new Vector3(value.x * planeSizeInv, 1.0f, value.y * planeSizeInv);
            }
        }

        /// <summary>
        /// The pixel resolution of a depth texture
        /// </summary>
        public Vector2 DepthResolution
        {
            get
            {
                return Resolution * _DepthScale;
            }
        }
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Transforms global position in Unity units into pixel coordinates on simulation matrices
        /// </summary>
        /// <returns>Simulation area local pixel coordinates of the point</returns>
        public Vector2 GetLocalPixelPosition(Vector3 globalPosition)
        {
            // Get the 2d position in the <X, Z> plane
            var position = new Vector2(transform.position.x, transform.position.z);

            // Translate to the local left-bottom corner
            var result = (position - new Vector2(globalPosition.x, globalPosition.z)) + Size * 0.5f;

            // Scale from global to pixel units
            result.x *= _PixelsPerUnit;
            result.y *= _PixelsPerUnit;
            return result;
        }

        /// <summary>
        /// Adds force to simulation matrix
        /// </summary>
        public void AddForce(List<WaterForce.Data> data, float radius = 1.0f)
        {
            var resolution = Resolution;
            int selected = 0;

            for (int i = 0; i < data.Count; ++i)
            {
                var position = GetLocalPixelPosition(data[i].Position);
                if (!ContainsLocalRaw(position, resolution)) { continue; }
                if (data[i].Force <= 0.0f) { continue; }

                const float forceBaseMultiplier = 500.0f;

                _Array[selected * 4] = position.x;
                _Array[selected * 4 + 1] = position.y;
                _Array[selected * 4 + 2] = data[i].Force * forceBaseMultiplier;
                _Array[selected * 4 + 3] = 0.0f;

                selected++;

                // when we fill all the _Array elements,
                // we need to flush it
                if (selected == _MaxArrayElements)
                {
                    DispatchAddForce(selected);
                    selected = 0;
                }
            }

            DispatchAddForce(selected);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Water _Water;
        [SerializeField] private WaterRipplesProfile _Profile;

        [Header("Settings")]
        [Tooltip("How many simulation pixels per one unit are used"), SerializeField, Range(1, 32)]
        private int _PixelsPerUnit = 16;

        [Tooltip("What resolution is used for dynamic depth rendering"), SerializeField, Range(0.125f, 2.0f)]
        private float _DepthScale = 0.5f;

        [Tooltip("Does the water can be  stopped by Blocking objects"), SerializeField]
        private bool _EnableStaticCalculations;

        [Tooltip("What resolution is used for static depth information"), SerializeField, Range(0.125f, 2.0f)]
        private float _StaticDepthScale = 1.0f;

        [Header("Edge fade")]
        [SerializeField]
        private bool _Fade;
        [SerializeField] private float _FadePower = 0.5f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _MeshFilter = GetComponent<MeshFilter>();
            _TranslateMaterial = new Material(ShaderUtility.Instance.Get(ShaderList.Translate));

            if (_Water == null) { _Water = Utilities.GetWaterReference(); }

            if (_DisplacementMaterial == null)
            {
                _DisplacementMaterial = new Material(Resources.Load<Material>("Materials/Overlay (Displacements)"));
            }
        }

        private void OnEnable()
        {
            // cache transform
            _Position = transform.position;
            _Scale = transform.localScale;

            _CommandBuffer = new CommandBuffer { name = "[Ultimate Water]: Water Simulation Area" };

            CreateDepthCamera();
            CreateTextures();

            UpdateShaderVariables();
            RenderStaticDepthTexture();

            DynamicWater.AddRenderer(this);
            WaterRipples.Register(this);
        }

        private void OnDisable()
        {
            WaterRipples.Unregister(this);
            DynamicWater.RemoveRenderer(this);

            ReleaseDepthCamera();
            ReleaseTextures();

            if (_CommandBuffer != null)
            {
                _CommandBuffer.Release();
                _CommandBuffer = null;
            }
        }
        private void OnDestroy()
        {
            OnDisable();
        }

        private void Update()
        {
            // reset transform
            transform.position = new Vector3(transform.position.x, _Water.transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(_Scale.x, 1.0f, _Scale.z);

            RenderDepth();
        }

        private void OnValidate()
        {
            ShaderUtility.Instance.Use(ShaderList.Simulation);
            ShaderUtility.Instance.Use(ShaderList.Translate);
            ShaderUtility.Instance.Use(ShaderList.Velocity);
            ShaderUtility.Instance.Use(ShaderList.Depth);
        }

        private void Reset()
        {
            //Assign Plane mesh to the mesh filter
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);

            var filter = GetComponent<MeshFilter>();
            filter.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
            filter.hideFlags = HideFlags.HideInInspector;

            DestroyImmediate(go);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            // Draw area gizmo
            if (Camera.main == null || _Water == null) { return; }

            var color = Color.cyan;

            // Draw cube representation
            Gizmos.color = color * new Color(1.0f, 1.0f, 1.0f, 0.6f);
            Gizmos.DrawWireCube(
                new Vector3(transform.position.x, _Water.transform.position.y, transform.position.z),
                new Vector3(Size.x, 1.0f, Size.y));

            Gizmos.color = color * new Color(1.0f, 1.0f, 1.0f, 0.05f);
            Gizmos.DrawCube(
                new Vector3(transform.position.x, _Water.transform.position.y, transform.position.z),
                new Vector3(Size.x, 1.0f, Size.y));
        }

        // Used by CustomEditor to refresh data
        public void Refresh()
        {
            if (!Application.isPlaying || !Application.isEditor) return;

            OnDisable();
            OnEnable();
        }

#endif
        #endregion Unity Methods

        #region Private Variables
        private Camera _DepthCamera;

        private readonly RenderTexture[] _Buffers = new RenderTexture[3];

        private RenderTexture _PreviousDepth;
        private RenderTexture _Depth;
        private RenderTexture _StaticDepth;

        private Material _RipplesMaterialCache;

        private MeshFilter _MeshFilter;

        [SerializeField]
        [HideInInspector]
        private Material _DisplacementMaterial;

        private Material _RipplesMaterial
        {
            get
            {
                if (_RipplesMaterialCache != null) { return _RipplesMaterialCache; }

                var shader = ShaderUtility.Instance.Get(ShaderList.Simulation);
                _RipplesMaterialCache = new Material(shader);

                return _RipplesMaterialCache;
            }
        }

        private int _Width
        {
            get { return (int)Resolution.x; }
        }

        private int _Height
        {
            get { return (int)Resolution.y; }
        }

        private int _DepthWidth
        {
            get { return (int)(Resolution.x * _DepthScale); }
        }

        private int _DepthHeight
        {
            get { return (int)(Resolution.y * _DepthScale); }
        }

        private int _StaticWidth
        {
            get { return (int)(Resolution.x * _StaticDepthScale); }
        }

        private int _StaticHeight
        {
            get { return (int)(Resolution.y * _StaticDepthScale); }
        }

        private Vector3 _Position;
        private Vector3 _Scale;

        private Material _TranslateMaterial;
        private CommandBuffer _CommandBuffer;

        /// <summary>
        /// Max values allowed by setup shader [512 * float4]
        /// </summary>
        private static readonly float[] _Array = new float[_MaxArrayElements * 4];
        private const int _MaxArrayElements = 512;
        #endregion Private Variables

        #region Private Methods
        // Progresses simulation by one tick
        internal void Simulate()
        {
            // update dynamic data
            Shader.SetGlobalFloat("_WaterHeight", _Water.transform.position.y);
            if (transform.position.x != _Position.x || transform.position.z != _Position.z)
            {
                RenderStaticDepthTexture();
                MoveSimulation();

                _Position = transform.position;
            }

            // we need to clear both buffers even if only one RT was recreated
            if (_Buffers[0].Verify(false) || _Buffers[1].Verify(false))
            {
                _Buffers[0].Clear(Color.clear);
                _Buffers[1].Clear(Color.clear);
            }
            RipplesShader.SetPrimary(_Buffers[0], _RipplesMaterial);
            RipplesShader.SetSecondary(_Buffers[1], _RipplesMaterial);

            // Based on that, simulate in pixel or compute shader
            var mode = WaterQualitySettings.Instance.Ripples.ShaderMode;
            switch (mode)
            {
                case WaterRipplesData.ShaderModes.ComputeShader:
                    {
                        SimulateComputeShader();
                        break;
                    }
                case WaterRipplesData.ShaderModes.PixelShader:
                    {
                        SimulatePixelShader();
                        break;
                    }
            }
        }

        internal void Smooth()
        {
            if (Profile.Sigma <= 0.1f) { return; }

            // Get temporary render texture
            var temp = RenderTexturesCache.GetTemporary(_Width, _Height, 0, WaterQualitySettings.Instance.Ripples.SimulationFormat,
                true, true);

            // Ensure that temp is created
            TextureUtility.Verify(temp);

            // Blur simulation matrix
            {
                // Vertical Pass
                GaussianShader.VerticalInput = _Buffers[1];
                GaussianShader.VerticalOutput = temp;
                GaussianShader.Dispatch(GaussianShader.KernelType.Vertical, _Width, _Height);

                // Horizontal pass
                GaussianShader.HorizontalInput = temp;
                GaussianShader.HorizontalOutput = _Buffers[1];
                GaussianShader.Dispatch(GaussianShader.KernelType.Horizontal, _Width, _Height);
            }

            // Release temporary
            temp.Dispose();
        }

        internal void Swap()
        {
            TextureUtility.Swap(ref _Buffers[0], ref _Buffers[1]);
        }

        // Send shader variables from WaterRipples to shaders
        internal void UpdateShaderVariables()
        {
            if (Profile == null) { return; }
            if (_DisplacementMaterial.IsNullReference(this)) { return; }

            float scale = _PixelsPerUnit / 32.0f;
            RipplesShader.SetPropagation(Profile.Propagation * scale, _RipplesMaterial);
            RipplesShader.SetStaticDepth(DefaultTextures.Get(Color.clear), _RipplesMaterial);
            RipplesShader.SetDamping(Profile.Damping / 32.0f, _RipplesMaterial);
            RipplesShader.SetGain(Profile.Gain, _RipplesMaterial);
            RipplesShader.SetHeightGain(Profile.HeightGain, _RipplesMaterial);
            RipplesShader.SetHeightOffset(Profile.HeightOffset, _RipplesMaterial);

            var terms = Utils.Math.GaussianTerms(Profile.Sigma);
            GaussianShader.Term0 = terms[0];
            GaussianShader.Term1 = terms[1];
            GaussianShader.Term2 = terms[2];

            _DisplacementMaterial.SetFloat("_Amplitude", Profile.Amplitude * 0.05f);
            _DisplacementMaterial.SetFloat("_Spread", Profile.Spread);
            _DisplacementMaterial.SetFloat("_Multiplier", Profile.Multiplier);
            _DisplacementMaterial.SetFloat("_Fadeout", _Fade ? 1.0f : 0.0f);
            _DisplacementMaterial.SetFloat("_FadePower", _FadePower);
        }

        private void DispatchAddForce(int count)
        {
            if (count <= 0) { return; }

            var shader = SetupShader.Shader;

            shader.SetTexture(SetupShader.Multi, SetupShader.PrimaryName, _Buffers[0]);
            shader.SetTexture(SetupShader.Multi, SetupShader.SecondaryName, _Buffers[1]);

            shader.SetFloats("data", _Array);

            shader.Dispatch(SetupShader.Multi, count, 1, 1);
        }

        #region Simulation
        private void SimulateComputeShader()
        {
            // Set simulation parameters
            RipplesShader.Size = Resolution;

            // Tick simulation
            RipplesShader.Dispatch(_Width, _Height);
        }

        private void SimulatePixelShader()
        {
            var temporary = _Buffers[1].CreateTemporary();
            {
                temporary.Clear(Color.clear);

                Graphics.Blit(null, temporary, _RipplesMaterial);
                Graphics.Blit(temporary, _Buffers[1]);
            }
            temporary.ReleaseTemporary();
        }
        #endregion Simulation

        #region Depth
        private void CreateDepthCamera()
        {
            if (Resolution.x <= 0 || Resolution.y <= 0)
            {
                Debug.LogError("WaterSimulationArea: invalid resolution");
            }

            var go = new GameObject("Depth");
            go.transform.SetParent(transform);

            _DepthCamera = go.AddComponent<Camera>();
            _DepthCamera.enabled = false;
            _DepthCamera.backgroundColor = Color.clear;
            _DepthCamera.clearFlags = CameraClearFlags.Color;
            _DepthCamera.orthographic = true;
            _DepthCamera.orthographicSize = Size.y * 0.5f;
            _DepthCamera.aspect = Resolution.x / Resolution.y;
            _DepthCamera.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 180.0f, 0.0f));
            _DepthCamera.transform.localScale = Vector3.one;
            _DepthCamera.depthTextureMode = DepthTextureMode.Depth;

            SetDepthCameraParameters(ref _DepthCamera);
        }
        private void ReleaseDepthCamera()
        {
            if (_DepthCamera != null)
            {
                Destroy(_DepthCamera.gameObject);
            }
            _DepthCamera = null;
        }

        private void RenderDepth()
        {
            if (_DepthCamera.IsNullReference(this)) { return; }

            TextureUtility.Swap(ref _Depth, ref _PreviousDepth);

            GL.PushMatrix();
            GL.modelview = _DepthCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(_DepthCamera.projectionMatrix);

            _CommandBuffer.Clear();

            _CommandBuffer.SetRenderTarget(_Depth);
            _CommandBuffer.ClearRenderTarget(true, true, Color.clear);

            var interactive = DynamicWater.Interactions;
            for (int i = 0; i < interactive.Count; ++i)
            {
                interactive[i].Render(_CommandBuffer);
            }

            Graphics.ExecuteCommandBuffer(_CommandBuffer);
            GL.PopMatrix();

            // send depth data
            RipplesShader.SetDepth(_Depth, _RipplesMaterial);
            RipplesShader.SetPreviousDepth(_PreviousDepth, _RipplesMaterial);
        }
        #endregion Depth

        #region Dynamic

        private void MoveSimulation()
        {
            var delta = _Position - transform.position;
            float x = delta.x * _PixelsPerUnit;
            float z = delta.z * _PixelsPerUnit;

            _TranslateMaterial.SetVector("_Offset", new Vector4(x, z, 0.0f, 0.0f));
            for (int i = 0; i < 2; ++i)
            {
                Graphics.Blit(_Buffers[i], _Buffers[2], _TranslateMaterial);
                TextureUtility.Swap(ref _Buffers[i], ref _Buffers[2]);
            }
            SendSimulationMatrix(_Buffers[1]);
        }
        #endregion Dynamic

        private static bool ContainsLocalRaw(Vector2 localPosition, Vector2 resolution)
        {
            return localPosition.x >= 0 && localPosition.x < resolution.x &&
                   localPosition.y >= 0 && localPosition.y < resolution.y;
        }

        private void RenderStaticDepthTexture()
        {
            if (_EnableStaticCalculations == false)
            {
                if (_StaticDepth != null)
                {
                    _StaticDepth.Release();
                    _StaticDepth = null;
                }
                return;
            }

            if (_DepthCamera.IsNullReference(this)) { return; }

            // create
            if (_StaticDepth == null)
            {
                var staticDepthDesc = new TextureUtility.RenderTextureDesc("[UWS] - Static Depth")
                {
                    Width = _StaticWidth,
                    Height = _StaticHeight,
                    Depth = 24,
                    Format = RenderTextureFormat.RFloat,
                    Filter = FilterMode.Point
                };
                _StaticDepth = staticDepthDesc.CreateRenderTexture();
                _StaticDepth.Clear(Color.clear);
            }

            // render
            var height = _DepthCamera.transform.localPosition.y;
            var near = _DepthCamera.nearClipPlane;
            var far = _DepthCamera.farClipPlane;

            SetDepthCameraParameters(ref _DepthCamera, 40.0f, 0.0f, 80.0f);

            _DepthCamera.cullingMask = WaterQualitySettings.Instance.Ripples.StaticDepthMask;
            _DepthCamera.targetTexture = _StaticDepth;
            _DepthCamera.SetReplacementShader(ShaderUtility.Instance.Get(ShaderList.Depth), "");

            _DepthCamera.Render();

            SetDepthCameraParameters(ref _DepthCamera, height, near, far);

            // send depth data
            RipplesShader.SetStaticDepth(_StaticDepth, _RipplesMaterial);
        }

        private void CreateTextures()
        {
            ReleaseTextures();

            // Create simulation textures
            var simulationDesc = new TextureUtility.RenderTextureDesc("[UWS] WaterSimulationArea - Simulation")
            {
                Width = _Width,
                Height = _Height,
                Format = WaterQualitySettings.Instance.Ripples.SimulationFormat,
                Filter = FilterMode.Bilinear,
                EnableRandomWrite = true,
            };

            for (int i = 0; i < 3; ++i)
            {
                _Buffers[i] = simulationDesc.CreateRenderTexture();
                _Buffers[i].name = "[UWS] WaterSimulationArea - Buffer[" + i + "]";
                _Buffers[i].Clear(Color.clear);
            }
            SendSimulationMatrix(_Buffers[1]);

            // Create depth textures
            var depthDesc = new TextureUtility.RenderTextureDesc("[UWS] WaterSimulationArea - Depth")
            {
                Width = _DepthWidth,
                Height = _DepthHeight,
                Depth = 24,
                Format = RenderTextureFormat.ARGBHalf,
                Filter = FilterMode.Point
            };
            _Depth = depthDesc.CreateRenderTexture();
            _PreviousDepth = depthDesc.CreateRenderTexture();

            _Depth.Clear(Color.clear);
            _PreviousDepth.Clear(Color.clear);
        }

        private void ReleaseTextures()
        {
            // Release depth textures
            TextureUtility.Release(ref _Depth);
            TextureUtility.Release(ref _PreviousDepth);
            TextureUtility.Release(ref _StaticDepth);

            // Release simulation matrices
            TextureUtility.Release(ref _Buffers[0]);
            TextureUtility.Release(ref _Buffers[1]);
            TextureUtility.Release(ref _Buffers[2]);
        }

        private void SendSimulationMatrix(Texture texture)
        {
            // Bind the simulation matrix for screen-space rendering
            _DisplacementMaterial.SetTexture("_SimulationMatrix", texture);
        }

        private static void SetDepthCameraParameters(ref Camera camera,
            float height = 10.0f, float near = 0.0f, float far = 20.0f)
        {
            camera.nearClipPlane = near;
            camera.farClipPlane = far;
            camera.transform.localPosition = new Vector3(0.0f, height, 0.0f);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Water/SimulationArea", false, 10)]
        private static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var obj = new GameObject("Simulation Area");
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<WaterSimulationArea>();

            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }
#endif

        #endregion Private Methods

        #region Helper Methods
        public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            commandBuffer.DrawMesh(_MeshFilter.sharedMesh, transform.localToWorldMatrix, _DisplacementMaterial);
        }

        public void Enable()
        {
        } // unused interface method
        public void Disable()
        {
        } // unused interface method
        #endregion Helper Methods
    }
}