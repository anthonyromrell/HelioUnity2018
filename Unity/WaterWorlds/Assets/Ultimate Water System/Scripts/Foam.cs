namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Foam : WaterModule
    {
        #region Public Types
        [System.Serializable]
        public class Data
        {
            [Tooltip("Foam map supersampling in relation to the waves simulator resolution. Has to be a power of two (0.25, 0.5, 1, 2, etc.)")]
            public float Supersampling = 1.0f;
        }
        #endregion Public Types

        #region Public Variables
        public float FoamIntensity
        {
            get { return _FoamIntensity; }
            set
            {
                if (float.IsNaN(value))
                {
                    _FoamIntensityOverriden = false;
                    OnProfilesChanged(_Water);
                }
                else
                {
                    _FoamIntensityOverriden = true;
                    _FoamIntensity = value;

                    if (_GlobalFoamSimulationMaterial != null)
                    {
                        float tl = _FoamThreshold * _Resolution / 2048.0f * 0.5f;
                        _GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, tl, 0.0f, _FoamFadingFactor));
                    }

                    var block = _Water.Renderer.PropertyBlock;
                    float t = _FoamThreshold * _Resolution / 2048.0f * 0.5f;
                    block.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, t, 150.0f / (_FoamShoreExtent * _FoamShoreExtent), _FoamFadingFactor));
                }
            }
        }

        public Texture FoamMap
        {
            get { return _FoamMapA; }
        }
        #endregion Public Variables

        #region Public Methods
        public Foam(Water water, Data data)
        {
            _Water = water;
            _WindWaves = water.WindWaves;
            _Overlays = water.DynamicWater;
            _Data = data;

            Validate();

            _WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);

            _Resolution = Mathf.RoundToInt(_WindWaves.FinalResolution * data.Supersampling);
            _GlobalFoamSimulationMaterial = new Material(_GlobalFoamSimulationShader) { hideFlags = HideFlags.DontSave };

            _FirstFrame = true;
        }

        public void RenderOverlays(DynamicWaterCameraData overlays)
        {
            if (!Application.isPlaying || !CheckPreresquisites()) { return; }

            var waterCamera = overlays.Camera;
            if (waterCamera.Type != WaterCamera.CameraType.Normal) { return; }

            int layer = _Water.gameObject.layer;
            CameraRenderData cameraRenderData;

            if (!_LayerUpdateFrames.TryGetValue(waterCamera, out cameraRenderData))
            {
                _LayerUpdateFrames[waterCamera] = cameraRenderData = new CameraRenderData();
                waterCamera.Destroyed += OnCameraDestroyed;
            }

            int frameCount = Time.frameCount;

            if (cameraRenderData.RenderFramePerLayer[layer] < frameCount)
            {
                cameraRenderData.RenderFramePerLayer[layer] = frameCount;

                if (_Water.WindWaves.FinalRenderMode == WaveSpectrumRenderMode.FullFFT)
                {
                    var displacementDeltaMaps = GetDisplacementDeltaMaps();

                    float t = _FoamThreshold * _Resolution / 2048.0f * 0.5f;
                    _GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, t, 0.0f, _FoamFadingFactor));

                    for (int i = 0; i < 4; ++i)
                    {
                        var displacementMap = _Water.WindWaves.WaterWavesFFT.GetDisplacementMap(i);
                        var displacementDeltaMap = displacementDeltaMaps[i];

                        _GlobalFoamSimulationMaterial.SetFloat(ShaderVariables.WaterTileSizeInvSrt, _Water.WindWaves.TileSizesInv[i]);
                        Graphics.Blit(displacementMap, displacementDeltaMap, _GlobalFoamSimulationMaterial, 1);
                    }

                    Shader.SetGlobalTexture("_FoamMapPrevious", overlays.FoamMapPrevious);
                    Shader.SetGlobalVector("_WaterOffsetDelta", _Water.SurfaceOffset - cameraRenderData.LastSurfaceOffset);
                    cameraRenderData.LastSurfaceOffset = _Water.SurfaceOffset;

                    var projectorCamera = waterCamera.PlaneProjectorCamera;
                    projectorCamera.cullingMask = 1 << layer;

                    projectorCamera.GetComponent<WaterCamera>()
                        .RenderWaterWithShader("[PW Water] Foam", overlays.FoamMap, _LocalFoamSimulationShader, _Water);
                }
            }

            _Water.Renderer.PropertyBlock.SetTexture("_FoamMap", overlays.FoamMap);
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        private readonly Data _Data;

        private float _FoamIntensity = 1.0f;
        private float _FoamThreshold = 1.0f;
        private float _FoamFadingFactor = 0.85f;
        private float _FoamShoreExtent;
        private bool _FoamIntensityOverriden;

        private Shader _LocalFoamSimulationShader;
        private Shader _GlobalFoamSimulationShader;

        private RenderTexture _FoamMapA;
        private RenderTexture _FoamMapB;
        private RenderTexture[] _DisplacementDeltaMaps;
        private int _Resolution;
        private bool _FirstFrame;

        private readonly DynamicWater _Overlays;
        private readonly Material _GlobalFoamSimulationMaterial;

        private static readonly Dictionary<WaterCamera, CameraRenderData> _LayerUpdateFrames = new Dictionary<WaterCamera, CameraRenderData>();
        #endregion Private Variables

        #region Private Methods
        internal override void Enable()
        {
            _Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            OnProfilesChanged(_Water);
        }

        internal override void Disable()
        {
            _Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);
        }

        private void SetupFoamMaterials()
        {
            if (_GlobalFoamSimulationMaterial != null)
            {
                float tl = _FoamThreshold * _Resolution / 2048.0f * 0.5f;
                float tg = tl * 220.0f;
                _GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamParameters, new Vector4(_FoamIntensity * 0.6f, tl, 0.0f, _FoamFadingFactor));
                _GlobalFoamSimulationMaterial.SetVector(ShaderVariables.FoamIntensity, new Vector4(tg / _WindWaves.TileSizes.x, tg / _WindWaves.TileSizes.y, tg / _WindWaves.TileSizes.z, tg / _WindWaves.TileSizes.w));
            }
        }

        internal override void Validate()
        {
            if (_GlobalFoamSimulationShader == null)
                _GlobalFoamSimulationShader = Shader.Find("UltimateWater/Foam/Global");

            if (_LocalFoamSimulationShader == null)
                _LocalFoamSimulationShader = Shader.Find("UltimateWater/Foam/Local");

            _Data.Supersampling = Mathf.ClosestPowerOfTwo(Mathf.RoundToInt(_Data.Supersampling * 4096)) / 4096.0f;
        }

        internal override void Destroy()
        {
            if (_FoamMapA != null)
            {
                _FoamMapA.Destroy();
                _FoamMapB.Destroy();

                _FoamMapA = null;
                _FoamMapB = null;
            }

            if (_DisplacementDeltaMaps != null)
            {
                for (int i = 0; i < _DisplacementDeltaMaps.Length; ++i)
                {
                    _DisplacementDeltaMaps[i].Destroy();
                }

                _DisplacementDeltaMaps = null;
            }
        }

        internal override void Update()
        {
            if (!_FirstFrame && _Overlays == null)
                UpdateFoamTiled();
            else
                _FirstFrame = false;
        }

        private void CheckTilesFoamResources()
        {
            if (_FoamMapA == null)
            {
                _FoamMapA = CreateRt(0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, FilterMode.Trilinear, TextureWrapMode.Repeat);
                _FoamMapA.name = "[UWS] Foam - Map A";
                _FoamMapB = CreateRt(0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, FilterMode.Trilinear, TextureWrapMode.Repeat);
                _FoamMapB.name = "[UWS] Foam - Map B";

                RenderTexture.active = null;
            }
        }

        private RenderTexture CreateRt(int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite, FilterMode filterMode, TextureWrapMode wrapMode)
        {
            bool allowFloatingPointMipMaps = WaterProjectSettings.Instance.AllowFloatingPointMipMaps;

            var renderTexture = new RenderTexture(_Resolution, _Resolution, depth, format, readWrite)
            {
                name = "[UWS] Foam",
                hideFlags = HideFlags.DontSave,
                filterMode = filterMode,
                wrapMode = wrapMode,
                useMipMap = allowFloatingPointMipMaps,
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4
                generateMips = allowFloatingPointMipMaps
#else
                autoGenerateMips = allowFloatingPointMipMaps
#endif
            };

            RenderTexture.active = renderTexture;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            return renderTexture;
        }

        private void UpdateFoamTiled()
        {
            if (!CheckPreresquisites())
                return;

            CheckTilesFoamResources();
            SetupFoamMaterials();

            var waterWavesFft = _WindWaves.WaterWavesFFT;
            _GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap0", waterWavesFft.GetDisplacementMap(0));
            _GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap1", waterWavesFft.GetDisplacementMap(1));
            _GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap2", waterWavesFft.GetDisplacementMap(2));
            _GlobalFoamSimulationMaterial.SetTexture("_DisplacementMap3", waterWavesFft.GetDisplacementMap(3));
            Graphics.Blit(_FoamMapA, _FoamMapB, _GlobalFoamSimulationMaterial, 0);

            _Water.Renderer.PropertyBlock.SetTexture("_FoamMap", _FoamMapB);

            SwapRenderTargets();
        }

        private void OnResolutionChanged(WindWaves windWaves)
        {
            _Resolution = Mathf.RoundToInt(windWaves.FinalResolution * _Data.Supersampling);

            Destroy();
        }

        private bool CheckPreresquisites()
        {
            return _WindWaves != null && _WindWaves.FinalRenderMode == WaveSpectrumRenderMode.FullFFT;
        }

        private void OnProfilesChanged(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;

            float foamIntensity = 0.0f;
            _FoamThreshold = 0.0f;
            _FoamFadingFactor = 0.0f;
            _FoamShoreExtent = 0.0f;
            float foamShoreIntensity = 0.0f;
            float foamNormalScale = 0.0f;

            if (profiles != null)
            {
                for (int i = profiles.Length - 1; i >= 0; --i)
                {
                    var weightedProfile = profiles[i];
                    var profile = weightedProfile.Profile;
                    float weight = weightedProfile.Weight;

                    foamIntensity += profile.FoamIntensity * weight;
                    _FoamThreshold += profile.FoamThreshold * weight;
                    _FoamFadingFactor += profile.FoamFadingFactor * weight;
                    _FoamShoreExtent += profile.FoamShoreExtent * weight;
                    foamShoreIntensity += profile.FoamShoreIntensity * weight;
                    foamNormalScale += profile.FoamNormalScale * weight;
                }
            }

            if (!_FoamIntensityOverriden) { _FoamIntensity = foamIntensity; }

            var block = water.Renderer.PropertyBlock;
            block.SetFloat("_FoamNormalScale", foamNormalScale);

            if (_FoamShoreExtent < 0.001f)
                _FoamShoreExtent = 0.001f;

            float t = _FoamThreshold * _Resolution / 2048.0f * 0.5f;
            block.SetVector(ShaderVariables.FoamParameters, new Vector4(foamIntensity * 0.6f, t, 150.0f / (_FoamShoreExtent * _FoamShoreExtent), _FoamFadingFactor));
            block.SetFloat(ShaderVariables.FoamShoreIntensity, foamShoreIntensity);
        }

        private void SwapRenderTargets()
        {
            var t = _FoamMapA;
            _FoamMapA = _FoamMapB;
            _FoamMapB = t;
        }

        private RenderTexture[] GetDisplacementDeltaMaps()
        {
            if (_DisplacementDeltaMaps == null)
            {
                _DisplacementDeltaMaps = new RenderTexture[4];
                bool allowFloatingPointMipMaps = WaterProjectSettings.Instance.AllowFloatingPointMipMaps;

                for (int i = 0; i < 4; ++i)
                {
                    _DisplacementDeltaMaps[i] = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
                    {
                        name = "[UWS] Foam - Displacement Delta Map [" + i + "]",
                        useMipMap = allowFloatingPointMipMaps,
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4
                        generateMips = allowFloatingPointMipMaps,
#else
                        autoGenerateMips = allowFloatingPointMipMaps,
#endif
                        wrapMode = TextureWrapMode.Repeat,
                        filterMode = allowFloatingPointMipMaps ? FilterMode.Trilinear : FilterMode.Bilinear
                    };

                    _Water.Renderer.PropertyBlock.SetTexture(ShaderVariables.DisplacementDeltaMaps[i], _DisplacementDeltaMaps[i]);
                }
            }

            return _DisplacementDeltaMaps;
        }

        private static void OnCameraDestroyed(WaterCamera waterCamera)
        {
            _LayerUpdateFrames.Remove(waterCamera);
        }

        private class CameraRenderData
        {
            public readonly int[] RenderFramePerLayer = new int[32];
            public Vector2 LastSurfaceOffset;
        }
        #endregion Private Methods
    }
}