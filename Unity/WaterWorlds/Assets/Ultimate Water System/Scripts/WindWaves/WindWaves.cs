namespace UltimateWater
{
    using System;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Events;

    using Internal;

    #region Public Types
    public enum WaveSpectrumRenderMode
    {
        FullFFT,
        GerstnerAndFFTNormals,
        Gerstner
    }
    #endregion Public Types

    /// <summary>
    ///     Renders wind waves on water surface and also resolves them on CPU for physics etc.
    /// </summary>
    public sealed class WindWaves : WaterModule
    {
        #region Public Types
        [Serializable]
        public class WindWavesEvent : UnityEvent<WindWaves> { };
        [Serializable]
        public sealed class Data
        {
            public Transform WindDirectionPointer;

            [Tooltip("Higher values increase quality, but also decrease performance. Directly controls quality of waves, foam and spray.")]
            [SerializeField]
            public int Resolution = 256;

            [Tooltip("Determines if 32-bit precision buffers should be used for computations (Default: off). Not supported on most mobile devices. This setting has impact on performance, even on PCs.\n\nTips:\n 1024 and higher - The difference is clearly visible, use this option on higher quality settings.\n 512 or lower - Keep it disabled.")]
            [SerializeField]
            public bool HighPrecision = true;

            [Tooltip("What error in world units is acceptable for elevation sampling used by physics and custom scripts? Lower values mean better precision, but higher CPU usage.")]
            public float CpuDesiredStandardError = 0.12f;

            [Tooltip("Copying wave spectrum from other fluid will make this instance a lot faster.")]
            public Water CopyFrom;

            [Tooltip("Setting this property to any value greater than zero will loop the water spectra in that time. A good value is 10 seconds. Set to 0 to resolve sea state at each frame without any looping (best quality).")]
            public float LoopDuration;

            public WindWavesEvent WindDirectionChanged;
            public WindWavesEvent ResolutionChanged;
            public float MipBias = 0.0f;

            public WavesRendererFFT.Data WavesRendererFFTData;
            public WavesRendererGerstner.Data WavesRendererGerstnerData;
        }
        #endregion Public Types

        #region Public Variables
        /// <summary>
        /// Copying wave spectrum from other fluid will make this instance a lot faster.
        /// </summary>
        public Water CopyFrom
        {
            get { return _RuntimeCopyFrom; }
            set
            {
                if (_Data.CopyFrom != value || _RuntimeCopyFrom != value)
                {
                    _Data.CopyFrom = value;
                    _RuntimeCopyFrom = value;
                    _IsClone = value != null;

                    _DynamicSmoothness.OnCopyModeChanged();
                    _WaterWavesFFT.OnCopyModeChanged();
                }
            }
        }

        public SpectrumResolver SpectrumResolver
        {
            get { return _Data.CopyFrom == null ? _SpectrumResolver : _Data.CopyFrom.WindWaves._SpectrumResolver; }
        }

        public WavesRendererFFT WaterWavesFFT
        {
            get { return _WaterWavesFFT; }
        }

        public WavesRendererGerstner WaterWavesGerstner
        {
            get { return _WaterWavesGerstner; }
        }

        public DynamicSmoothness DynamicSmoothness
        {
            get { return _DynamicSmoothness; }
        }

        public WaveSpectrumRenderMode FinalRenderMode
        {
            get { return _FinalRenderMode; }
        }

        public Vector4 TileSizes
        {
            get { return _TileSizes; }
        }

        public Vector4 TileSizesInv
        {
            get { return _TileSizesInv; }
        }

        public Vector4 UnscaledTileSizes
        {
            get { return _UnscaledTileSizes; }
        }

        /// <summary>
        /// Current wind speed as resolved from the currently set profiles.
        /// </summary>
        public Vector2 WindSpeed
        {
            get { return _WindSpeed; }
        }

        public bool WindSpeedChanged
        {
            get { return _WindSpeedChanged; }
        }

        /// <summary>
        /// Current wind direction. It's controlled by the WindDirectionPointer.
        /// </summary>
        public Vector2 WindDirection
        {
            get { return _WindDirection; }
        }

        public Transform WindDirectionPointer
        {
            get { return _Data.WindDirectionPointer; }
        }

        /// <summary>
        /// Event invoked when wind direction changes.
        /// </summary>
        public WindWavesEvent WindDirectionChanged
        {
            get { return _Data.WindDirectionChanged; }
        }

        /// <summary>
        /// Event invoked when wind spectrum resolution changes.
        /// </summary>
        public WindWavesEvent ResolutionChanged
        {
            get { return _Data.ResolutionChanged != null ? _Data.ResolutionChanged : (_Data.ResolutionChanged = new WindWavesEvent()); }
        }

        public int Resolution
        {
            get { return _Data.Resolution; }
            set
            {
                if (_Data.Resolution == value)
                    return;

                _Data.Resolution = value;
                ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
            }
        }

        public int FinalResolution
        {
            get { return _FinalResolution; }
        }

        public bool FinalHighPrecision
        {
            get { return _FinalHighPrecision; }
        }

        public bool HighPrecision
        {
            get { return _Data.HighPrecision; }
        }

        public float CpuDesiredStandardError
        {
            get { return _Data.CpuDesiredStandardError; }
        }

        public float LoopDuration
        {
            get { return _Data.LoopDuration; }
        }

        public Vector4 TileSizeScales
        {
            get { return _TileSizeScales; }
        }

        public float MaxVerticalDisplacement
        {
            get { return _SpectrumResolver.MaxVerticalDisplacement; }
        }

        public float MaxHorizontalDisplacement
        {
            get { return _SpectrumResolver.MaxHorizontalDisplacement; }
        }

        public float SpectrumDirectionality
        {
            get { return _SpectrumDirectionality; }
        }
        #endregion Public Variables

        #region Public Methods
        public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
        {
            return _SpectrumResolver.GetHorizontalDisplacementAt(x, z, time);
        }
        public float GetHeightAt(float x, float z, float time)
        {
            return _SpectrumResolver.GetHeightAt(x, z, time);
        }
        public Vector4 GetForceAndHeightAt(float x, float z, float time)
        {
            return _SpectrumResolver.GetForceAndHeightAt(x, z, time);
        }
        public Vector3 GetDisplacementAt(float x, float z, float time)
        {
            return _SpectrumResolver.GetDisplacementAt(x, z, time);
        }
        public WindWaves(Water water, Data data)
        {
            _Water = water;
            _Data = data;

            _RuntimeCopyFrom = data.CopyFrom;
            _IsClone = _RuntimeCopyFrom != null;

            CheckSupport();

            Validate();

            var spectrumShader = Shader.Find("UltimateWater/Spectrum/Water Spectrum");

            if (_SpectrumResolver == null) _SpectrumResolver = new SpectrumResolver(water, this, spectrumShader);
            if (data.WindDirectionChanged == null) data.WindDirectionChanged = new WindWavesEvent();

            CreateObjects();

            ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);

            if (!Application.isPlaying)
                return;

            water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            OnProfilesChanged(water);
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Water _Water;
        private readonly Data _Data;

        // I didn't found any practical reason for now to adjust these scales in inspector
        private Vector4 _TileSizeScales = new Vector4(0.79241f, 0.163151f, 3.175131f, 13.7315131f);

        private int _FinalResolution;
        private bool _FinalHighPrecision;
        private float _WindSpeedMagnitude;
        private float _SpectrumDirectionality;
        private float _TileSize;
        private float _LastTileSize = float.NaN;
        private float _LastUniformWaterScale = float.NaN;
        private Vector4 _TileSizes;
        private Vector4 _TileSizesInv;
        private Vector4 _UnscaledTileSizes;
        private Vector2 _WindDirection;
        private Vector2 _WindSpeed;
        private WaveSpectrumRenderMode _FinalRenderMode;
        private SpectrumResolver _SpectrumResolver;
        private Water _RuntimeCopyFrom;
        private bool _IsClone;
        private bool _WindSpeedChanged;
        private bool _HasWindDirectionPointer;

        private WavesRendererFFT _WaterWavesFFT;
        private WavesRendererGerstner _WaterWavesGerstner;
        private DynamicSmoothness _DynamicSmoothness;
        #endregion Private Variables

        #region Private Methods
        internal override void Validate()
        {
            if (Application.isPlaying)
                CopyFrom = _Data.CopyFrom;

#if UNITY_EDITOR
            if (_Data.CopyFrom != null && !Application.isPlaying)
            {
                _Data.CopyFrom.ForceStartup();

                var copiedWindWaves = _Data.CopyFrom.WindWaves;

                Assert.IsNotNull(copiedWindWaves);

                _FinalRenderMode = copiedWindWaves._FinalRenderMode;
                _Data.Resolution = copiedWindWaves._Data.Resolution;
                _Data.HighPrecision = copiedWindWaves._Data.HighPrecision;
                _Data.CpuDesiredStandardError = copiedWindWaves._Data.CpuDesiredStandardError;
            }
#endif

            if (_Data.CpuDesiredStandardError < 0.00001f)
                _Data.CpuDesiredStandardError = 0.00001f;

            _HasWindDirectionPointer = (_Data.WindDirectionPointer != null);

            if (_SpectrumResolver != null)
            {
                ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);

                _WaterWavesFFT.Validate();
                _WaterWavesGerstner.OnValidate(this);
            }

            if (_Water != null && _TileSize != 0.0f)
                UpdateShaderParams();

            if (_WaterWavesFFT != null && _WaterWavesFFT.NormalMaps != null && _WaterWavesFFT.NormalMaps.Length != 0)
            {
                _WaterWavesFFT.GetNormalMap(0).mipMapBias = _Data.MipBias;
                _WaterWavesFFT.GetNormalMap(1).mipMapBias = _Data.MipBias;
            }
        }
        internal override void Update()
        {
            UpdateWind();

            if (_IsClone)
            {
                _TileSize = _RuntimeCopyFrom.WindWaves._TileSize;
                UpdateShaderParams();
                return;
            }

            if (!Application.isPlaying)
                return;

            _SpectrumResolver.Update();
            _DynamicSmoothness.Update();
            UpdateShaderParams();
        }
        /// <summary>
        /// Resolves final component settings based on the desired values, quality settings and hardware limitations.
        /// </summary>
        internal void ResolveFinalSettings(WaterQualityLevel quality)
        {
            CreateObjects();

            var wavesMode = quality.WavesMode;

            if (wavesMode == WaterWavesMode.DisallowAll)
            {
                _WaterWavesFFT.Disable();
                _WaterWavesGerstner.Disable();
                return;
            }

            bool supportsFloats = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);

            int finalResolution = Mathf.Min(_Data.Resolution, quality.MaxSpectrumResolution, SystemInfo.maxTextureSize);
            bool finalHighPrecision = _Data.HighPrecision && quality.AllowHighPrecisionTextures && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat);

            var windWavesMode = _Water.ShaderSet.WindWavesMode;

            if (windWavesMode == WindWavesRenderMode.FullFFT && wavesMode == WaterWavesMode.AllowAll && supportsFloats)
                _FinalRenderMode = WaveSpectrumRenderMode.FullFFT;
            else if (windWavesMode <= WindWavesRenderMode.GerstnerAndFFTNormals && wavesMode <= WaterWavesMode.AllowNormalFFT && supportsFloats)
                _FinalRenderMode = WaveSpectrumRenderMode.GerstnerAndFFTNormals;
            else
                _FinalRenderMode = WaveSpectrumRenderMode.Gerstner;

            if (_FinalResolution != finalResolution)
            {
                lock (this)
                {
                    _FinalResolution = finalResolution;
                    _FinalHighPrecision = finalHighPrecision;

                    if (_SpectrumResolver != null)
                        _SpectrumResolver.OnMapsFormatChanged(true);

                    if (ResolutionChanged != null)
                        ResolutionChanged.Invoke(this);
                }
            }
            else if (_FinalHighPrecision != finalHighPrecision)
            {
                lock (this)
                {
                    _FinalHighPrecision = finalHighPrecision;

                    if (_SpectrumResolver != null)
                        _SpectrumResolver.OnMapsFormatChanged(false);
                }
            }

            switch (_FinalRenderMode)
            {
                case WaveSpectrumRenderMode.FullFFT:
                {
                    _WaterWavesFFT.RenderedMaps = WavesRendererFFT.MapType.Displacement | WavesRendererFFT.MapType.Normal;
                    _WaterWavesFFT.Enable();

                    _WaterWavesGerstner.Disable();
                    break;
                }

                case WaveSpectrumRenderMode.GerstnerAndFFTNormals:
                {
                    _WaterWavesFFT.RenderedMaps = WavesRendererFFT.MapType.Normal;
                    _WaterWavesFFT.Enable();

                    _WaterWavesGerstner.Enable();
                    break;
                }

                case WaveSpectrumRenderMode.Gerstner:
                {
                    _WaterWavesFFT.Disable();
                    _WaterWavesGerstner.Enable();
                    break;
                }
            }
        }
        private void UpdateShaderParams()
        {
            float uniformWaterScale = _Water.UniformWaterScale;

            if (_LastTileSize == _TileSize && _LastUniformWaterScale == uniformWaterScale)
                return;

            var block = _Water.Renderer.PropertyBlock;

            float scaledTileSize = _TileSize * uniformWaterScale;
            _TileSizes.x = scaledTileSize * _TileSizeScales.x;
            _TileSizes.y = scaledTileSize * _TileSizeScales.y;
            _TileSizes.z = scaledTileSize * _TileSizeScales.z;
            _TileSizes.w = scaledTileSize * _TileSizeScales.w;
            block.SetVector(ShaderVariables.WaterTileSize, _TileSizes);

            _TileSizesInv.x = 1.0f / _TileSizes.x;
            _TileSizesInv.y = 1.0f / _TileSizes.y;
            _TileSizesInv.z = 1.0f / _TileSizes.z;
            _TileSizesInv.w = 1.0f / _TileSizes.w;
            block.SetVector(ShaderVariables.WaterTileSizeInv, _TileSizesInv);

            _LastUniformWaterScale = uniformWaterScale;
            _LastTileSize = _TileSize;
        }
        private void OnProfilesChanged(Water water)
        {
            _TileSize = 0.0f;
            _WindSpeedMagnitude = 0.0f;
            _SpectrumDirectionality = 0.0f;

            var profiles = water.ProfilesManager.Profiles;

            for (int i = 0; i < profiles.Length; ++i)
            {
                var profile = profiles[i].Profile;
                float weight = profiles[i].Weight;

                _TileSize += profile.TileSize * profile.TileScale * weight;
                _WindSpeedMagnitude += profile.WindSpeed * weight;
                _SpectrumDirectionality += profile.Directionality * weight;
            }

            // scale by quality settings
            var waterQualitySettings = WaterQualitySettings.Instance;
            _TileSize *= waterQualitySettings.TileSizeScale;
            _UnscaledTileSizes = _TileSize * _TileSizeScales;
            UpdateShaderParams();

            var block = water.Renderer.PropertyBlock;
            block.SetVector(ShaderVariables.WaterTileSizeScales,
                new Vector4(_TileSizeScales.x / _TileSizeScales.y,
                _TileSizeScales.x / _TileSizeScales.z,
                _TileSizeScales.x / _TileSizeScales.w,
                0.0f));

            _SpectrumResolver.OnProfilesChanged();

            block.SetFloat(ShaderVariables.MaxDisplacement, _SpectrumResolver.MaxHorizontalDisplacement);
        }
        internal override void Destroy()
        {
            if (_SpectrumResolver != null)
            {
                _SpectrumResolver.OnDestroy();
                _SpectrumResolver = null;
            }

            if (_WaterWavesFFT != null) _WaterWavesFFT.OnDestroy();
        }
        private void UpdateWind()
        {
            Vector2 newWindDirection;

            if (_HasWindDirectionPointer)				// used bool var to avoid calling Unity's overloaded == operator and gain some performance
            {
                Vector3 forward = _Data.WindDirectionPointer.forward;
                float len = Mathf.Sqrt(forward.x * forward.x + forward.z * forward.z);
                newWindDirection = new Vector2(forward.x / len, forward.z / len);
            }
            else
                newWindDirection = new Vector2(1.0f, 0.0f);

            Vector2 newWindSpeed = new Vector2(
                newWindDirection.x * _WindSpeedMagnitude,
                newWindDirection.y * _WindSpeedMagnitude
            );

            if (_WindSpeed.x != newWindSpeed.x || _WindSpeed.y != newWindSpeed.y)
            {
                _WindDirection = newWindDirection;
                _WindSpeed = newWindSpeed;
                _WindSpeedChanged = true;

                _SpectrumResolver.SetWindDirection(_WindDirection);
            }
            else
                _WindSpeedChanged = false;
        }
        private void CreateObjects()
        {
            if (_WaterWavesFFT == null) _WaterWavesFFT = new WavesRendererFFT(_Water, this, _Data.WavesRendererFFTData);
            if (_WaterWavesGerstner == null) _WaterWavesGerstner = new WavesRendererGerstner(_Water, this, _Data.WavesRendererGerstnerData);
            if (_DynamicSmoothness == null) _DynamicSmoothness = new DynamicSmoothness(_Water, this);
        }
        private void CheckSupport()
        {
            if (_Data.HighPrecision && (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat) || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat)))
                _FinalHighPrecision = false;

            if (!_Data.HighPrecision && (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)))
            {
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
                    _FinalHighPrecision = true;
                else if (_Water.ShaderSet.WindWavesMode == WindWavesRenderMode.FullFFT)
                {
#if UNITY_EDITOR
                    Debug.LogError("Your hardware doesn't support floating point render textures. FFT water waves won't work in editor.");
#endif

                    _FinalRenderMode = WaveSpectrumRenderMode.Gerstner;
                }
            }
        }
        internal override void OnWaterRender(WaterCamera waterCamera)
        {
            if (!Application.isPlaying) return;

            var camera = waterCamera.CameraComponent;
            if (_WaterWavesFFT.Enabled)
                _WaterWavesFFT.OnWaterRender(camera);

            if (_WaterWavesGerstner.Enabled)
                _WaterWavesGerstner.OnWaterRender(camera);
        }
        internal override void Enable()
        {
            UpdateWind();

            ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
        }

        internal override void Disable()
        {
            if (_WaterWavesFFT != null) _WaterWavesFFT.Disable();
            if (_WaterWavesGerstner != null) _WaterWavesGerstner.Disable();
            if (_DynamicSmoothness != null) _DynamicSmoothness.FreeResources();
        }
        #endregion Private Methods
    }
}