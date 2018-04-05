namespace UltimateWater
{
    using Internal;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public sealed class WaterSubsurfaceScattering : WaterModule
    {
        #region Public Types
        [System.Serializable]
        public enum SubsurfaceScatteringMode
        {
            Disabled,
            TextureSpace
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField]
        private SubsurfaceScatteringMode _Mode = SubsurfaceScatteringMode.TextureSpace;

        [SerializeField]
        private BlurSSS _SubsurfaceScatteringBlur;

        [Range(0.0f, 0.9f)]
        [SerializeField]
        private float _IgnoredLightFraction = 0.15f;

        [Resolution(128, 64, 128, 256, 512)]
        [SerializeField]
        private int _AmbientResolution = 128;

        [Range(-1, 6)]
        [SerializeField]
        private int _LightCount = -1;

        [SerializeField]
        private int _LightingLayer = 22;
        #endregion Inspector Variables

        #region Public Variables
        public float IsotropicScatteringIntensity
        {
            get { return _ShaderParams.x; }
            set { _ShaderParams.x = value; }
        }

        public float SubsurfaceScatteringContrast
        {
            get { return _ShaderParams.y; }
            set { _ShaderParams.y = value; }
        }
        #endregion Public Variables

        #region Private Variables
        private RenderTexture _ScatteringTex;
        private Vector4 _ShaderParams;				// x = intensity, y = contrast
        private Water _Water;

        private static readonly List<Water> _CachedRenderList;
        #endregion Private Variables

        #region Private Methods
        static WaterSubsurfaceScattering()
        {
            _CachedRenderList = new List<Water> { null };
        }
        internal override void OnWaterRender(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;
            var rect = waterCamera.LocalMapsRect;

            if (rect.width == 0.0f || !Application.isPlaying || _Mode == SubsurfaceScatteringMode.Disabled)
            {
                return;
            }

            var temp1 = RenderTexture.GetTemporary(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
            temp1.filterMode = FilterMode.Bilinear;

            var effectsCamera = waterCamera.EffectsCamera;
            var effectsWaterCamera = effectsCamera.GetComponent<WaterCamera>();
            effectsWaterCamera.enabled = true;
            effectsWaterCamera.GeometryType = WaterGeometryType.UniformGrid;

            _CachedRenderList[0] = _Water;
            effectsWaterCamera.SetCustomWaterRenderList(_CachedRenderList);

            effectsCamera.stereoTargetEye = StereoTargetEyeMask.None;
            effectsCamera.enabled = false;
            effectsCamera.depthTextureMode = DepthTextureMode.None;
            effectsCamera.renderingPath = RenderingPath.Forward;
            effectsCamera.orthographic = true;
            effectsCamera.orthographicSize = rect.width * 0.5f;
            effectsCamera.cullingMask = 1 << _LightingLayer;
            effectsCamera.farClipPlane = 2000.0f;
            effectsCamera.ResetProjectionMatrix();
            effectsCamera.clearFlags = CameraClearFlags.Nothing;

#if UNITY_5_6_OR_NEWER
            effectsCamera.allowHDR = true;
#else
            effectsCamera.hdr = true;
#endif
            effectsCamera.transform.position = new Vector3(rect.center.x, 1000.0f, rect.center.y);
            effectsCamera.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f));

            effectsCamera.targetTexture = temp1;

            Shader.SetGlobalVector("_ScatteringParams", _ShaderParams);
            Shader.SetGlobalVector("_WorldSpaceOriginalCameraPos", camera.transform.position);

            int originalPixelLightCount = 3;

            if (_LightCount >= 0)
            {
                originalPixelLightCount = QualitySettings.pixelLightCount;
                QualitySettings.pixelLightCount = _LightCount;
            }

            var collectLight = ShaderUtility.Instance.Get(ShaderList.CollectLight);

            _Water.gameObject.layer = _LightingLayer;
            effectsCamera.RenderWithShader(collectLight, "CustomType");
            _Water.gameObject.layer = WaterProjectSettings.Instance.WaterLayer;

            if (_LightCount >= 0)
                QualitySettings.pixelLightCount = originalPixelLightCount;

            effectsWaterCamera.GeometryType = WaterGeometryType.Auto;
            effectsWaterCamera.SetCustomWaterRenderList(null);

            var temp2 = RenderTexture.GetTemporary(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
            temp2.filterMode = FilterMode.Point;

            var absorptionColor = _Water.Materials.GetParameterValue(WaterMaterials.ColorParameter.AbsorptionColor);
            _SubsurfaceScatteringBlur.BlurMaterial.SetVector("_ScatteringParams", _ShaderParams);
            _SubsurfaceScatteringBlur.Apply(temp1, temp2, absorptionColor, waterCamera.LocalMapsRect.width, _IgnoredLightFraction);

            RenderTexture.ReleaseTemporary(temp1);

            Graphics.Blit(temp2, _ScatteringTex, _SubsurfaceScatteringBlur.BlurMaterial, 1);
            RenderTexture.ReleaseTemporary(temp2);

            _Water.Renderer.PropertyBlock.SetTexture("_SubsurfaceScattering", _ScatteringTex);

            // clear render target
            Graphics.SetRenderTarget(null);
        }

        internal override void Start(Water water)
        {
            _Water = water;
            water.ProfilesManager.Changed.AddListener(ResolveProfileData);
        }

        internal override void Enable()
        {
            Validate();

            if (Application.isPlaying && _Mode == SubsurfaceScatteringMode.TextureSpace)
            {
                _ScatteringTex = new RenderTexture(_AmbientResolution, _AmbientResolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] WaterSubsurfaceScattering - Scattering Tex",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Bilinear,
                    useMipMap = WaterProjectSettings.Instance.AllowFloatingPointMipMaps,

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4
                    generateMips = WaterProjectSettings.Instance.AllowFloatingPointMipMaps
#else
                    autoGenerateMips = WaterProjectSettings.Instance.AllowFloatingPointMipMaps
#endif
                };
            }
        }

        internal override void Disable()
        {
            if (_ScatteringTex != null)
            {
                _ScatteringTex.Destroy();
                _ScatteringTex = null;
            }

            if (_Water != null)
            {
                var texture = DefaultTextures.Get(Color.white);
                if (texture != null)
                {
                    _Water.Renderer.PropertyBlock.SetTexture("_SubsurfaceScattering", texture);
                }
            }
        }

        internal override void Destroy()
        {
            if (_Water == null)
            {
                return;
            }

            _Water.ProfilesManager.Changed.RemoveListener(ResolveProfileData);
        }

        private void ResolveProfileData(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;
            _ShaderParams.x = 0.0f;
            _ShaderParams.y = 0.0f;

            for (int i = 0; i < profiles.Length; ++i)
            {
                var profile = profiles[i].Profile;
                float weight = profiles[i].Weight;

                _ShaderParams.x += profile.IsotropicScatteringIntensity * weight;
                _ShaderParams.y += profile.SubsurfaceScatteringContrast * weight;
            }

            _ShaderParams.x *= 1.0f + _ShaderParams.y;
        }

        internal override void Validate()
        {
            if (_SubsurfaceScatteringBlur == null)
            {
                _SubsurfaceScatteringBlur = new BlurSSS();
            }
            _SubsurfaceScatteringBlur.Validate("UltimateWater/Utilities/Blur (Subsurface Scattering)", "Shaders/Blurs", 6);
        }
        #endregion Private Methods
    }
}