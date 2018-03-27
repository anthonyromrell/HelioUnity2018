namespace UltimateWater
{
    using Internal;
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    /// Underwater image effect.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(WaterCamera))]
    [AddComponentMenu("Ultimate Water/Underwater IME")]
    public sealed class UnderwaterIME : MonoBehaviour, IWaterImageEffect
    {
        #region Public Variables
        public float Intensity
        {
            get { return _Intensity; }
        }

        public bool EffectEnabled
        {
            get { return _EffectEnabled; }
            set { _EffectEnabled = value; }
        }

        public Water WaterOverride
        {
            get { return _WaterOverride; }
            set
            {
                _WaterOverride = value;
                _HasWaterOverride = value != null;
                OnSubmersionStateChanged(_LocalWaterCamera);
            }
        }
        #endregion Public Variables

        #region Public Methods
        // Called by WaterCamera.cs
        public void OnWaterCameraEnabled()
        {
            var waterCamera = GetComponent<WaterCamera>();
            waterCamera.SubmersionStateChanged.AddListener(OnSubmersionStateChanged);
        }

        // Called by WaterCamera.cs, to update this effect when it's disabled
        public void OnWaterCameraPreCull()
        {
            if (!_EffectEnabled)
            {
                enabled = false;
                return;
            }

            if (_HasWaterOverride)
            {
                enabled = true;
                _RenderUnderwaterMask = true;
                return;
            }

            switch (_LocalWaterCamera.SubmersionState)
            {
                case SubmersionState.None:
                    {
                        enabled = false;
                        break;
                    }

                case SubmersionState.Partial:
                    {
                        enabled = true;
                        _RenderUnderwaterMask = true;
                        break;
                    }

                case SubmersionState.Full:
                    {
                        enabled = true;
                        _RenderUnderwaterMask = false;
                        break;
                    }
            }

            float nearPlaneSizeY = _LocalCamera.nearClipPlane * Mathf.Tan(_LocalCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float verticalDistance = transform.position.y - _LocalWaterCamera.WaterLevel;
            float intensity = (-verticalDistance + nearPlaneSizeY) * 0.25f;
            SetEffectsIntensity(intensity);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField]
        private Blur _Blur;

        [SerializeField]
        private bool _UnderwaterAudio = true;

        [Tooltip("Individual camera blur scale. It's recommended to modify blur scale through water profiles. Use this one, only if some of your cameras need a clear view and some don't.")]
        [Range(0.0f, 4.0f)]
        [SerializeField]
        private float _CameraBlurScale = 1.0f;

        [Range(0.1f, 1.0f)]
        [SerializeField]
        private float _MaskResolution = 0.5f;

        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _LocalCamera = GetComponent<Camera>();
            _LocalWaterCamera = GetComponent<WaterCamera>();

            OnValidate();

            _MaskMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.ScreenSpaceMask, HideFlags.DontSave);
            _ImeMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.BaseIME, HideFlags.DontSave);
            _NoiseMaterial = ShaderUtility.Instance.CreateMaterial(ShaderList.Noise, HideFlags.DontSave);
            _ComposeUnderwaterMaskMaterial =
                ShaderUtility.Instance.CreateMaterial(ShaderList.ComposeUnderWaterMask, HideFlags.DontSave);

            _ReverbFilter = GetComponent<AudioReverbFilter>();
            if (_ReverbFilter == null && _UnderwaterAudio)
            {
                _ReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
            }
        }

        private void OnDisable()
        {
            TextureUtility.Release(ref _UnderwaterMask);

            if (_MaskCommandBuffer != null)
            {
                _MaskCommandBuffer.Clear();
            }
        }

        private void OnDestroy()
        {
            if (_MaskCommandBuffer != null)
            {
                _MaskCommandBuffer.Dispose();
                _MaskCommandBuffer = null;
            }

            if (_Blur != null)
            {
                _Blur.Dispose();
                _Blur = null;
            }

            _MaskMaterial.Destroy();
            _ImeMaterial.Destroy();
        }

        private void OnValidate()
        {
            ShaderUtility.Instance.Use(ShaderList.ScreenSpaceMask);
            ShaderUtility.Instance.Use(ShaderList.BaseIME);
            ShaderUtility.Instance.Use(ShaderList.Noise);
            ShaderUtility.Instance.Use(ShaderList.ComposeUnderWaterMask);

            if (_Blur != null)
                _Blur.Validate("UltimateWater/Utilities/Blur (Underwater)");
        }

        private void OnPreCull()
        {
            RenderUnderwaterMask();
        }

        //[ImageEffectOpaque] // it will be an opaque effect in the future
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Graphics.Blit(source, destination);
                return;
            }
#endif

            var containingWater = _HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater;

            if (!_LocalWaterCamera.enabled || containingWater == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            source.filterMode = FilterMode.Bilinear;

            var temporary = RenderTexturesCache.GetTemporary(source.width, source.height, 0, destination != null ? destination.format : source.format, true, false);
            temporary.Texture.filterMode = FilterMode.Bilinear;
            temporary.Texture.wrapMode = TextureWrapMode.Clamp;

            RenderDepthScatter(source, temporary);

            _Blur.TotalSize = containingWater.Materials.UnderwaterBlurSize * _CameraBlurScale;
            _Blur.Apply(temporary);

            RenderDistortions(temporary, destination);
            temporary.Dispose();
        }
        #endregion Unity Methods

        #region Private Variables
        private Material _MaskMaterial;
        private Material _ImeMaterial;
        private Material _NoiseMaterial;
        private Material _ComposeUnderwaterMaskMaterial;

        private Camera _LocalCamera;
        private WaterCamera _LocalWaterCamera;
        private AudioReverbFilter _ReverbFilter;
        private CommandBuffer _MaskCommandBuffer;

        private float _Intensity = float.NaN;
        private bool _RenderUnderwaterMask;
        private Water _WaterOverride;
        private bool _HasWaterOverride;
        private bool _EffectEnabled = true;

        private RenderTexture _UnderwaterMask;
        #endregion Private Variables

        #region Private Methods
        private void RenderUnderwaterMask()
        {
            if (_MaskCommandBuffer == null)
                return;

            _MaskCommandBuffer.Clear();

            var containingWater = _HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater;
            var currentCamera = Camera.current;

            int underwaterId = ShaderVariables.UnderwaterMask;
            int underwater2Id = ShaderVariables.UnderwaterMask2;

            if (_UnderwaterMask == null)
            {
                int w = Mathf.RoundToInt(currentCamera.pixelWidth * _MaskResolution);
                int h = Mathf.RoundToInt(currentCamera.pixelHeight * _MaskResolution);

                _UnderwaterMask = new RenderTexture(w, h, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear)
                {
                    filterMode = FilterMode.Bilinear,
                    name = "[UWS] UnderwaterIME - Mask"
                };
                _UnderwaterMask.Create();
            }

            if (_RenderUnderwaterMask || (containingWater != null && containingWater.Renderer.MaskCount > 0))
            {
                int w = Mathf.RoundToInt(currentCamera.pixelWidth * _MaskResolution);
                int h = Mathf.RoundToInt(currentCamera.pixelHeight * _MaskResolution);
                _MaskCommandBuffer.GetTemporaryRT(underwaterId, w, h, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);
                _MaskCommandBuffer.GetTemporaryRT(underwater2Id, w, h, 0, FilterMode.Point, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);
            }
            else
                _MaskCommandBuffer.GetTemporaryRT(underwaterId, 4, 4, 0, FilterMode.Point, RenderTextureFormat.R8, RenderTextureReadWrite.Linear, 1);

            if (_RenderUnderwaterMask && containingWater != null)
            {
                _MaskMaterial.CopyPropertiesFromMaterial(containingWater.Materials.SurfaceMaterial);

                _MaskCommandBuffer.SetRenderTarget(underwater2Id);
                _MaskCommandBuffer.ClearRenderTarget(false, true, Color.black);

                Matrix4x4 matrix;
                var geometry = containingWater.Geometry;
                var meshes = geometry.GetTransformedMeshes(_LocalCamera, out matrix, geometry.GeometryType == WaterGeometry.Type.ProjectionGrid ? WaterGeometryType.RadialGrid : WaterGeometryType.Auto, true, geometry.ComputeVertexCountForCamera(currentCamera));

                for (int i = meshes.Length - 1; i >= 0; --i)
                {
                    _MaskCommandBuffer.DrawMesh(meshes[i], matrix, _MaskMaterial, 0, 0, containingWater.Renderer.PropertyBlock);
                }

                _MaskCommandBuffer.SetRenderTarget(underwaterId);
                _MaskCommandBuffer.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, _ImeMaterial, 0, 3, containingWater.Renderer.PropertyBlock);
                _MaskCommandBuffer.ReleaseTemporaryRT(underwater2Id);
            }
            else
            {
                _MaskCommandBuffer.SetRenderTarget(underwaterId);
                _MaskCommandBuffer.ClearRenderTarget(false, true, Color.white);
            }

            _MaskCommandBuffer.Blit(underwaterId, _UnderwaterMask);
            Shader.SetGlobalTexture(underwaterId, _UnderwaterMask);

            if (containingWater != null && containingWater.Renderer.MaskCount != 0)
            {
                if (_LocalWaterCamera.RenderVolumes)
                    _MaskCommandBuffer.Blit("_SubtractiveMask", underwaterId, _ComposeUnderwaterMaskMaterial, 0);
            }

            var evt = _LocalCamera.actualRenderingPath == RenderingPath.Forward ?
                (WaterProjectSettings.Instance.SinglePassStereoRendering ? CameraEvent.BeforeForwardOpaque : CameraEvent.AfterDepthTexture)
                : CameraEvent.BeforeLighting;

            _LocalCamera.RemoveCommandBuffer(evt, _MaskCommandBuffer);
            _LocalCamera.AddCommandBuffer(evt, _MaskCommandBuffer);
        }

        private void RenderDepthScatter(Texture source, RenderTexture target)
        {
            var containingWater = _HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater;
            _ImeMaterial.CopyPropertiesFromMaterial(containingWater.Materials.SurfaceMaterial);
            _ImeMaterial.SetTexture("_UnderwaterAbsorptionGradient", containingWater.Materials.UnderwaterAbsorptionColorByDepth);
            _ImeMaterial.SetFloat("_UnderwaterLightFadeScale", containingWater.Materials.UnderwaterLightFadeScale);
            _ImeMaterial.SetMatrix("UNITY_MATRIX_VP_INVERSE", Matrix4x4.Inverse(_LocalCamera.projectionMatrix * _LocalCamera.worldToCameraMatrix));

            var block = containingWater.Renderer.PropertyBlock;
            GraphicsUtilities.Blit(source, target, _ImeMaterial, 1, block);
        }

        private void RenderDistortions(Texture source, RenderTexture target)
        {
            var containingWater = _HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater;
            float distortionIntensity = containingWater.Materials.UnderwaterDistortionsIntensity;

            if (distortionIntensity > 0.0f)
            {
                int w = Camera.current.pixelWidth >> 2;
                int h = Camera.current.pixelHeight >> 2;
                var distortionTex = RenderTexturesCache.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32, true, false);

                RenderDistortionMap(distortionTex);

                distortionTex.Texture.filterMode = FilterMode.Bilinear;
                _ImeMaterial.SetTexture("_DistortionTex", distortionTex);
                _ImeMaterial.SetFloat("_DistortionIntensity", distortionIntensity);
                GraphicsUtilities.Blit(source, target, _ImeMaterial, 2, containingWater.Renderer.PropertyBlock);

                distortionTex.Dispose();
            }
            else
                Graphics.Blit(source, target);
        }

        private void RenderDistortionMap(RenderTexture target)
        {
            var containingWater = _HasWaterOverride ? _WaterOverride : _LocalWaterCamera.ContainingWater;
            _NoiseMaterial.SetVector("_Offset", new Vector4(0.0f, 0.0f, Time.time * containingWater.Materials.UnderwaterDistortionAnimationSpeed, 0.0f));
            _NoiseMaterial.SetVector("_Period", new Vector4(4, 4, 4, 4));
            Graphics.Blit(null, target, _NoiseMaterial, 3);
        }

        private void OnSubmersionStateChanged(WaterCamera waterCamera)
        {
            if (waterCamera.SubmersionState != SubmersionState.None || _HasWaterOverride)
            {
                if (_MaskCommandBuffer == null)
                {
                    _MaskCommandBuffer = new CommandBuffer { name = "[UWS] UnderwaterIME - Render Underwater Mask" };
                }
            }
            else
            {
                if (_MaskCommandBuffer == null)
                {
                    return;
                }

                var cameraComponent = GetComponent<Camera>();
                cameraComponent.RemoveCommandBuffer(WaterProjectSettings.Instance.SinglePassStereoRendering ? CameraEvent.BeforeForwardOpaque : CameraEvent.AfterDepthTexture, _MaskCommandBuffer);
                cameraComponent.RemoveCommandBuffer(CameraEvent.AfterLighting, _MaskCommandBuffer);
            }
        }

        private void SetEffectsIntensity(float intensity)
        {
            // start wasn't called yet
            if (_LocalCamera == null)
            {
                return;
            }

            intensity = Mathf.Clamp01(intensity);
            if (_Intensity == intensity)
            {
                return;
            }

            _Intensity = intensity;
            if (_ReverbFilter == null || !_UnderwaterAudio)
            {
                return;
            }

            float reverbIntensity = intensity > 0.05f ? Mathf.Clamp01(intensity + 0.7f) : intensity;

            _ReverbFilter.dryLevel = -2000 * reverbIntensity;
            _ReverbFilter.room = -10000 * (1.0f - reverbIntensity);
            _ReverbFilter.roomHF = Mathf.Lerp(-10000, -4000, reverbIntensity);
            _ReverbFilter.decayTime = 1.6f * reverbIntensity;
            _ReverbFilter.decayHFRatio = 0.1f * reverbIntensity;
            _ReverbFilter.reflectionsLevel = -449.0f * reverbIntensity;
            _ReverbFilter.reverbLevel = 1500.0f * reverbIntensity;
            _ReverbFilter.reverbDelay = 0.0259f * reverbIntensity;
        }
        #endregion Private Methods
    }
}