using UltimateWater.Internal;

namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    ///     Precomputes spectra variance used by shader microfacet model. Currently works only on platforms with compute
    ///     shaders. General SM 3.0 support will be added later.
    ///     <seealso cref="WindWaves.DynamicSmoothness" />
    /// </summary>
    [System.Serializable]
    public class DynamicSmoothness
    {
        #region Public Variables
        public bool Enabled
        {
            get { return _Water.ShaderSet.SmoothnessMode == DynamicSmoothnessMode.Physical; }
        }

        public Texture VarianceTexture
        {
            get { return _VarianceTexture; }
        }
        /// <summary>
        /// You need to set this in your script, when instantiating WindWaves manually as compute shaders need to be directly referenced in Unity.
        /// </summary>
        public ComputeShader ComputeShader
        {
            get { return _VarianceShader; }
            set { _VarianceShader = value; }
        }
        #endregion Public Variables

        #region Public Methods
        public DynamicSmoothness(Water water, WindWaves windWaves)
        {
            _Water = water;
            _WindWaves = windWaves;
            _Supported = CheckSupport();

            _VarianceShader = water.ShaderSet.GetComputeShader("Spectral Variances");

            OnCopyModeChanged();
        }

        public void FreeResources()
        {
            if (_VarianceTexture != null)
            {
                _VarianceTexture.Destroy();
                _VarianceTexture = null;
            }
        }

        public void OnCopyModeChanged()
        {
            if (_WindWaves == null || _WindWaves.CopyFrom == null) { return; }

            _WindWaves.CopyFrom.ForceStartup();

            Assert.IsNotNull(_WindWaves.CopyFrom.WindWaves);

            FreeResources();

            var copyFromWindWaves = _WindWaves.CopyFrom.WindWaves;
            copyFromWindWaves.DynamicSmoothness.ValidateVarianceTextures();
            _Water.Renderer.PropertyBlock.SetTexture("_SlopeVariance", copyFromWindWaves.DynamicSmoothness._VarianceTexture);
        }

        public static bool CheckSupport()
        {
            var format = Compatibility.GetFormat(RenderTextureFormat.RGHalf, new[]
            {
                RenderTextureFormat.RGFloat,
            });

            var result = SystemInfo.supportsComputeShaders && SystemInfo.supports3DTextures && format.HasValue;
            if (!result)
            {
                WaterLogger.Warning("Dynamic Smoothness", "Check Support", "Dynamic Smoothness not supported");
                if (!SystemInfo.supportsComputeShaders)
                {
                    WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - compute shaders not supported");
                }
                if (!SystemInfo.supports3DTextures)
                {
                    WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - 3D textures not supported");
                }
                if (!format.HasValue)
                {
                    WaterLogger.Warning("Dynamic Smoothness", "Check Support", " - necessary RenderTexture formats not found");
                }
                return false;
            }

            _Format = format.Value;
            return true;
        }

        public void Update()
        {
            if (_Water.ShaderSet.SmoothnessMode != DynamicSmoothnessMode.Physical || !_Supported) { return; }

            if (!_Initialized) { InitializeVariance(); }

            ValidateVarianceTextures();

            if (!_Finished)
            {
                RenderNextPixel();
            }
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        private readonly bool _Supported;
        private static RenderTextureFormat _Format;

        // variance
        private ComputeShader _VarianceShader;
        private RenderTexture _VarianceTexture;
        private int _LastResetIndex;
        private int _CurrentIndex;
        private bool _Finished;
        private bool _Initialized;
        private float _DynamicSmoothnessIntensity;
        #endregion Private Variables

        #region Private Methods

        private void InitializeVariance()
        {
            _Initialized = true;

            _Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            _WindWaves.WindDirectionChanged.AddListener(OnWindDirectionChanged);

            OnProfilesChanged(_Water);
        }

        private void ValidateVarianceTextures()
        {
            if (_VarianceTexture == null)
            {
                _VarianceTexture = CreateVarianceTexture(_Format);
                ResetComputations();
            }

            if (!_VarianceTexture.IsCreated())
            {
                _VarianceTexture.Create();
                _Water.Renderer.PropertyBlock.SetTexture("_SlopeVariance", _VarianceTexture);

                _LastResetIndex = 0;
                _CurrentIndex = 0;
            }
        }

        private void RenderNextPixel()
        {
            _VarianceShader.SetInt("_FFTSize", _WindWaves.FinalResolution);
            _VarianceShader.SetInt("_FFTSizeHalf", _WindWaves.FinalResolution >> 1);
            _VarianceShader.SetFloat("_VariancesSize", _VarianceTexture.width);
            _VarianceShader.SetFloat("_IntensityScale", _DynamicSmoothnessIntensity);
            _VarianceShader.SetVector("_TileSizes", _WindWaves.TileSizes);
            _VarianceShader.SetVector("_Coordinates", new Vector4(_CurrentIndex % 4, (_CurrentIndex >> 2) % 4, _CurrentIndex >> 4, 0));
            _VarianceShader.SetTexture(0, "_Spectrum", _WindWaves.SpectrumResolver.GetRawDirectionalSpectrum());
            _VarianceShader.SetTexture(0, "_Variance", _VarianceTexture);
            _VarianceShader.Dispatch(0, 1, 1, 1);

            ++_CurrentIndex;

            if (_CurrentIndex >= 64)
                _CurrentIndex = 0;

            if (_CurrentIndex == _LastResetIndex)
                _Finished = true;
        }

        private void ResetComputations()
        {
            _LastResetIndex = _CurrentIndex;
            _Finished = false;
        }

        private void OnProfilesChanged(Water water)
        {
            ResetComputations();

            _DynamicSmoothnessIntensity = 0.0f;

            var profiles = water.ProfilesManager.Profiles;

            for (int i = profiles.Length - 1; i >= 0; --i)
                _DynamicSmoothnessIntensity += profiles[i].Profile.DynamicSmoothnessIntensity * profiles[i].Weight;
        }

        private void OnWindDirectionChanged(WindWaves windWaves)
        {
            ResetComputations();
        }

        private static RenderTexture CreateVarianceTexture(RenderTextureFormat format)
        {
            var variancesTexture = new RenderTexture(4, 4, 0, format, RenderTextureReadWrite.Linear)
            {
                name = "[UWS] DynamicSmoothness - Variance Tex",
                hideFlags = HideFlags.DontSave,
                volumeDepth = 4,
                enableRandomWrite = true,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,

#if UNITY_5_5_OR_NEWER
                autoGenerateMips = false,
#else
                generateMips = false,
#endif

                useMipMap = false,
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
                dimension = UnityEngine.Rendering.TextureDimension.Tex3D
#else
                isVolume = true
#endif
            };

            return variancesTexture;
        }
        #endregion Private Methods
    }
}