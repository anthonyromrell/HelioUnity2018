namespace UltimateWater
{
    using UnityEngine;
    using Internal;

    /// <summary>
    ///     Displays water spectrum using Fast Fourier Transform. Uses vertex shader texture fetch available on platforms with
    ///     Shader Model 3.0+.
    /// </summary>
    public sealed class WavesRendererFFT
    {
        #region Public Types
        [System.Serializable]
        public sealed class Data
        {
            [Tooltip("Determines if GPU partial derivatives or Fast Fourier Transform (high quality) should be used to compute normal map (Recommended: on). Works only if displacement map rendering is enabled.")]
            public bool HighQualityNormalMaps = true;

#pragma warning disable 0414
            [Tooltip("Check this option, if your water is flat or game crashes instantly on a DX11 GPU (in editor or build). Compute shaders are very fast, so use this as a last resort.")]
            public bool ForcePixelShader;
#pragma warning restore 0414

            [Tooltip("Fixes crest artifacts during storms, but lowers overall quality. Enabled by default when used with additive water volumes as it is actually needed and disabled in all other cases.")]
            public FlattenMode FlattenMode = FlattenMode.Auto;

            [Tooltip("Sea state will be cached in the specified frame count for extra performance, if LoopLength on WindWaves is set to a value greater than zero.")]
            public int CachedFrameCount = 180;
        }

        public enum SpectrumType
        {
            Phillips,
            Unified
        }

        [System.Flags]
        public enum MapType
        {
            Displacement = 1,
            Normal = 2
        }

        public enum FlattenMode
        {
            Auto,
            ForcedOn,
            ForcedOff
        }
        #endregion Public Types

        #region Public Variables
        public MapType RenderedMaps
        {
            get { return _RenderedMaps; }
            set
            {
                _RenderedMaps = value;

                if (Enabled && Application.isPlaying)
                {
                    Dispose(false);
                    ValidateResources();
                }
            }
        }
        public bool Enabled { get; private set; }
        public RenderTexture[] NormalMaps
        {
            get { return _NormalMaps; }
        }
        public event System.Action ResourcesChanged;

        #endregion Public Variables

        #region Public Methods
        public void OnCopyModeChanged()
        {
            _CopyModeDirty = true;

            if (_LastCopyFrom != null)
                _LastCopyFrom.WindWaves.WaterWavesFFT.ResourcesChanged -= ValidateResources;

            if (_WindWaves.CopyFrom != null)
                _WindWaves.CopyFrom.WindWaves.WaterWavesFFT.ResourcesChanged += ValidateResources;

            _LastCopyFrom = _WindWaves.CopyFrom;

            Dispose(false);
        }
        public Texture GetDisplacementMap(int index)
        {
            return _DisplacementMaps != null ? _DisplacementMaps[index] : null;
        }
        public Texture GetNormalMap(int index)
        {
            return _NormalMaps[index];
        }
        public void OnWaterRender(Camera camera)
        {
            if (_FFTUtilitiesMaterial == null) return;

            ValidateWaveMaps();
        }
        public WavesRendererFFT(Water water, WindWaves windWaves, Data data)
        {
            _Water = water;
            _WindWaves = windWaves;
            _Data = data;

            if (windWaves.LoopDuration != 0.0f)
            {
                _NormalMapsCache = new RenderTexture[data.CachedFrameCount][];
                _DisplacementMapsCache = new RenderTexture[data.CachedFrameCount][];
                _IsCachedFrameValid = new bool[data.CachedFrameCount];

                water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            }

            Validate();
        }
        #endregion Public Methods

        #region Private Variables
        private Shader _FFTShader;
        private Shader _FFTUtilitiesShader;

        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        private readonly Data _Data;

        private readonly RenderTexture[][] _NormalMapsCache;
        private readonly RenderTexture[][] _DisplacementMapsCache;
        private readonly bool[] _IsCachedFrameValid;

        private RenderTexture[] _NormalMaps;
        private RenderTexture[] _DisplacementMaps;

        private RenderTexturesCache _SingleTargetCache;
        private RenderTexturesCache _DoubleTargetCache;

        private GpuFFT _HeightFFT;
        private GpuFFT _NormalFFT;
        private GpuFFT _DisplacementFFT;
        private Material _FFTUtilitiesMaterial;
        private ComputeShader _Dx11FFT;

        private MapType _RenderedMaps;
        private bool _FinalHighQualityNormalMaps;
        private bool _CopyModeDirty;
        private int _WaveMapsFrame;
        private Water _LastCopyFrom;

        private static readonly Vector4[] _Offsets = { new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.5f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.5f, 0.0f, 0.0f), new Vector4(0.5f, 0.5f, 0.0f, 0.0f) };
        private static readonly Vector4[] _OffsetsDual = { new Vector4(0.0f, 0.0f, 0.5f, 0.0f), new Vector4(0.0f, 0.5f, 0.5f, 0.5f) };
        #endregion Private Variables

        #region Private Methods
        private void RenderSpectra(float time, out Texture heightSpectrum, out Texture normalSpectrum, out Texture displacementSpectrum)
        {
            if (_RenderedMaps == MapType.Normal)
            {
                heightSpectrum = null;
                displacementSpectrum = null;
                normalSpectrum = _WindWaves.SpectrumResolver.RenderNormalsSpectrumAt(time);
            }
            else if ((_RenderedMaps & MapType.Normal) == 0 || !_FinalHighQualityNormalMaps)
            {
                normalSpectrum = null;
                _WindWaves.SpectrumResolver.RenderDisplacementsSpectraAt(time, out heightSpectrum, out displacementSpectrum);
            }
            else
                _WindWaves.SpectrumResolver.RenderCompleteSpectraAt(time, out heightSpectrum, out normalSpectrum, out displacementSpectrum);
        }
        private void RenderMaps(float time, RenderTexture[] displacementMaps, RenderTexture[] normalMaps)
        {
            // render needed spectra
            Texture heightSpectrum, normalSpectrum, displacementSpectrum;
            RenderSpectra(time, out heightSpectrum, out normalSpectrum, out displacementSpectrum);

            // displacement
            if ((_RenderedMaps & MapType.Displacement) != 0)
            {
                TemporaryRenderTexture packedHeightMaps = _SingleTargetCache.GetTemporary();
                TemporaryRenderTexture packedHorizontalDisplacementMaps = _DoubleTargetCache.GetTemporary();

                _HeightFFT.ComputeFFT(heightSpectrum, packedHeightMaps);
                _DisplacementFFT.ComputeFFT(displacementSpectrum, packedHorizontalDisplacementMaps);

                _FFTUtilitiesMaterial.SetTexture(ShaderVariables.HeightTex, packedHeightMaps);
                _FFTUtilitiesMaterial.SetTexture(ShaderVariables.DisplacementTex, packedHorizontalDisplacementMaps);
                _FFTUtilitiesMaterial.SetFloat(ShaderVariables.HorizontalDisplacementScale, _Water.Materials.HorizontalDisplacementScale);

                for (int scaleIndex = 0; scaleIndex < 4; ++scaleIndex)
                {
                    _FFTUtilitiesMaterial.SetFloat(ShaderVariables.JacobianScale, _Water.Materials.HorizontalDisplacementScale * 0.1f * displacementMaps[scaleIndex].width / _WindWaves.TileSizes[scaleIndex]);     // * 220.0f * displacementMaps[scaleIndex].width / (2048.0f * water.SpectraRenderer.TileSizes[scaleIndex])
                    _FFTUtilitiesMaterial.SetVector(ShaderVariables.Offset, _Offsets[scaleIndex]);

                    Graphics.Blit(null, displacementMaps[scaleIndex], _FFTUtilitiesMaterial, 1);
                }

                packedHeightMaps.Dispose();
                packedHorizontalDisplacementMaps.Dispose();
            }

            // normals
            if ((_RenderedMaps & MapType.Normal) != 0)
            {
                if (!_FinalHighQualityNormalMaps)
                {
                    for (int scalesIndex = 0; scalesIndex < 2; ++scalesIndex)
                    {
                        int resolution = _WindWaves.FinalResolution;

                        _FFTUtilitiesMaterial.SetFloat("_Intensity1", 0.58f * resolution / _WindWaves.TileSizes[scalesIndex * 2]);
                        _FFTUtilitiesMaterial.SetFloat("_Intensity2", 0.58f * resolution / _WindWaves.TileSizes[scalesIndex * 2 + 1]);
                        _FFTUtilitiesMaterial.SetTexture("_MainTex", displacementMaps[scalesIndex << 1]);
                        _FFTUtilitiesMaterial.SetTexture("_SecondTex", displacementMaps[(scalesIndex << 1) + 1]);
                        _FFTUtilitiesMaterial.SetFloat("_MainTex_Texel_Size", 1.0f / displacementMaps[scalesIndex << 1].width);
                        Graphics.Blit(null, normalMaps[scalesIndex], _FFTUtilitiesMaterial, 0);
                    }
                }
                else
                {
                    TemporaryRenderTexture packedNormalMaps = _DoubleTargetCache.GetTemporary();
                    _NormalFFT.ComputeFFT(normalSpectrum, packedNormalMaps);

                    for (int scalesIndex = 0; scalesIndex < 2; ++scalesIndex)
                    {
                        _FFTUtilitiesMaterial.SetVector(ShaderVariables.Offset, _OffsetsDual[scalesIndex]);
                        Graphics.Blit(packedNormalMaps, normalMaps[scalesIndex], _FFTUtilitiesMaterial, 3);
                    }

                    packedNormalMaps.Dispose();
                }
            }
        }
        private void RetrieveCachedFrame(int frameIndex, out RenderTexture[] displacementMaps, out RenderTexture[] normalMaps)
        {
            float frameTime = (float)frameIndex / _Data.CachedFrameCount * _WindWaves.LoopDuration;

            if (!_IsCachedFrameValid[frameIndex])
            {
                _IsCachedFrameValid[frameIndex] = true;

                if ((_RenderedMaps & MapType.Displacement) != 0 && _DisplacementMapsCache[frameIndex] == null)
                    CreateRenderTextures(ref _DisplacementMapsCache[frameIndex], "[UWS] WavesRendererFFT - Water Displacement Map", RenderTextureFormat.ARGBHalf, 4, true);

                if ((_RenderedMaps & MapType.Normal) != 0 && _NormalMapsCache[frameIndex] == null)
                {
                    CreateRenderTextures(ref _NormalMapsCache[frameIndex], "[UWS] WavesRendererFFT - Water Normal Map", RenderTextureFormat.ARGBHalf, 2, true);
                }

                RenderMaps(frameTime, _DisplacementMapsCache[frameIndex], _NormalMapsCache[frameIndex]);
            }

            displacementMaps = _DisplacementMapsCache[frameIndex];
            normalMaps = _NormalMapsCache[frameIndex];
        }
        private void RenderMapsFromCache(float time, RenderTexture[] displacementMaps, RenderTexture[] normalMaps)
        {
            float frameIndexFloat = _Data.CachedFrameCount * FastMath.FracAdditive(time / _WindWaves.LoopDuration);
            int frameIndex = (int)frameIndexFloat;
            float blendFactor = frameIndexFloat - frameIndex;

            RenderTexture[] displacementMaps1;
            RenderTexture[] normalMaps1;
            RetrieveCachedFrame(frameIndex, out displacementMaps1, out normalMaps1);

            int nextFrameIndex = frameIndex + 1;

            if (nextFrameIndex >= _Data.CachedFrameCount)
                nextFrameIndex = 0;

            RenderTexture[] displacementMaps2;
            RenderTexture[] normalMaps2;
            RetrieveCachedFrame(nextFrameIndex, out displacementMaps2, out normalMaps2);

            _FFTUtilitiesMaterial.SetFloat("_BlendFactor", blendFactor);

            for (int scaleIndex = 0; scaleIndex < 4; ++scaleIndex)
            {
                _FFTUtilitiesMaterial.SetTexture("_Texture1", displacementMaps1[scaleIndex]);
                _FFTUtilitiesMaterial.SetTexture("_Texture2", displacementMaps2[scaleIndex]);
                Graphics.Blit(null, displacementMaps[scaleIndex], _FFTUtilitiesMaterial, 6);
            }

            for (int scalesIndex = 0; scalesIndex < 2; ++scalesIndex)
            {
                _FFTUtilitiesMaterial.SetTexture("_Texture1", normalMaps1[scalesIndex]);
                _FFTUtilitiesMaterial.SetTexture("_Texture2", normalMaps2[scalesIndex]);
                Graphics.Blit(null, normalMaps[scalesIndex], _FFTUtilitiesMaterial, 6);
            }
        }
        private void ValidateWaveMaps()
        {
            int frameCount = Time.frameCount;

            if (_WaveMapsFrame == frameCount || !Application.isPlaying)
                return;         // it's already done

            if (_LastCopyFrom != null)
            {
                if (_CopyModeDirty)
                {
                    _CopyModeDirty = false;
                    ValidateResources();
                }

                return;
            }

            _WaveMapsFrame = frameCount;

            if (_WindWaves.LoopDuration == 0.0f)
            {
                RenderMaps(_Water.Time, _DisplacementMaps, _NormalMaps);
            }
            else
            {
                RenderMapsFromCache(_Water.Time, _DisplacementMaps, _NormalMaps);
            }
        }
        private void OnResolutionChanged(WindWaves windWaves)
        {
            Dispose(false);
            ValidateResources();
        }
        private void Dispose(bool total)
        {
            _WaveMapsFrame = -1;

            if (_HeightFFT != null)
            {
                _HeightFFT.Dispose();
                _HeightFFT = null;
            }

            if (_NormalFFT != null)
            {
                _NormalFFT.Dispose();
                _NormalFFT = null;
            }

            if (_DisplacementFFT != null)
            {
                _DisplacementFFT.Dispose();
                _DisplacementFFT = null;
            }

            if (_NormalMaps != null)
            {
                foreach (var normalMap in _NormalMaps)
                    normalMap.Destroy();

                _NormalMaps = null;
            }

            if (_DisplacementMaps != null)
            {
                foreach (var displacementMap in _DisplacementMaps)
                    displacementMap.Destroy();

                _DisplacementMaps = null;
            }

            if (_NormalMapsCache != null)
            {
                for (int i = _NormalMapsCache.Length - 1; i >= 0; --i)
                {
                    var normalMapsCacheFrame = _NormalMapsCache[i];

                    if (normalMapsCacheFrame != null)
                    {
                        for (int ii = normalMapsCacheFrame.Length - 1; ii >= 0; --ii)
                            normalMapsCacheFrame[ii].Destroy();

                        _NormalMapsCache[i] = null;
                    }
                }
            }

            if (_DisplacementMapsCache != null)
            {
                for (int i = _DisplacementMapsCache.Length - 1; i >= 0; --i)
                {
                    var displacementMapsCacheFrame = _DisplacementMapsCache[i];

                    if (displacementMapsCacheFrame != null)
                    {
                        for (int ii = displacementMapsCacheFrame.Length - 1; ii >= 0; --ii)
                            displacementMapsCacheFrame[ii].Destroy();

                        _DisplacementMapsCache[i] = null;
                    }
                }
            }

            if (_IsCachedFrameValid != null)
            {
                for (int i = _IsCachedFrameValid.Length - 1; i >= 0; --i)
                    _IsCachedFrameValid[i] = false;
            }

            if (total)
            {
                if (_FFTUtilitiesMaterial != null)
                {
                    _FFTUtilitiesMaterial.Destroy();
                    _FFTUtilitiesMaterial = null;
                }
            }
        }
        private void OnProfilesChanged(Water water)
        {
            for (int i = _IsCachedFrameValid.Length - 1; i >= 0; --i)
                _IsCachedFrameValid[i] = false;
        }
        internal void Validate()
        {
            _Dx11FFT = _Water.ShaderSet.GetComputeShader("DX11 FFT");

            if (_FFTShader == null)
                _FFTShader = Shader.Find("UltimateWater/Base/FFT");

            if (_FFTUtilitiesShader == null)
                _FFTUtilitiesShader = Shader.Find("UltimateWater/Utilities/FFT Utilities");

            if (Application.isPlaying && Enabled)
                ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);
        }
        internal void ResolveFinalSettings(WaterQualityLevel qualityLevel)
        {
            _FinalHighQualityNormalMaps = _Data.HighQualityNormalMaps;

            if (!qualityLevel.AllowHighQualityNormalMaps)
                _FinalHighQualityNormalMaps = false;

            if ((_RenderedMaps & MapType.Displacement) == 0)           // if heightmap is not rendered, only high-quality normal map is possible
                _FinalHighQualityNormalMaps = true;
        }
        private GpuFFT ChooseBestFFTAlgorithm(bool twoChannels)
        {
            GpuFFT fft;

            int resolution = _WindWaves.FinalResolution;

#if !UNITY_IOS && !UNITY_ANDROID && !UNITY_PS3 && !UNITY_PS4 && !UNITY_BLACKBERRY && !UNITY_TIZEN && !UNITY_WEBGL && !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_LINUX && !UNITY_EDITOR_OSX
            if (!_Data.ForcePixelShader && _Dx11FFT != null && SystemInfo.supportsComputeShaders && /*resolution >= 128 && */resolution <= 512)
                fft = new Dx11FFT(_Dx11FFT, resolution, _WindWaves.FinalHighPrecision || resolution >= 2048, twoChannels);
            else
#endif
                fft = new PixelShaderFFT(_FFTShader, resolution, _WindWaves.FinalHighPrecision || resolution >= 2048, twoChannels);

            fft.SetupMaterials();

            return fft;
        }
        private void ValidateFFT(ref GpuFFT fft, bool present, bool twoChannels)
        {
            if (present)
            {
                if (fft == null)
                    fft = ChooseBestFFTAlgorithm(twoChannels);
            }
            else if (fft != null)
            {
                fft.Dispose();
                fft = null;
            }
        }
        private RenderTexture CreateRenderTexture(string name, RenderTextureFormat format, bool mipMaps)
        {
            var texture = new RenderTexture(_WindWaves.FinalResolution, _WindWaves.FinalResolution, 0, format, RenderTextureReadWrite.Linear)
            {
                name = name,
                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Repeat
            };

            if (mipMaps && WaterProjectSettings.Instance.AllowFloatingPointMipMaps)
            {
                texture.filterMode = FilterMode.Trilinear;
                texture.useMipMap = true;
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4
                texture.generateMips = true;
#else
                texture.autoGenerateMips = true;
#endif
            }
            else
                texture.filterMode = FilterMode.Bilinear;

            return texture;
        }
        // ReSharper disable once RedundantAssignment
        private void CreateRenderTextures(ref RenderTexture[] renderTextures, string name, RenderTextureFormat format, int count, bool mipMaps)
        {
            renderTextures = new RenderTexture[count];

            for (int i = 0; i < count; ++i)
                renderTextures[i] = CreateRenderTexture(name, format, mipMaps);
        }
        private void ValidateResources()
        {
            if (_WindWaves.CopyFrom == null)
            {
                ValidateFFT(ref _HeightFFT, (_RenderedMaps & MapType.Displacement) != 0, false);
                ValidateFFT(ref _DisplacementFFT, (_RenderedMaps & MapType.Displacement) != 0, true);
                ValidateFFT(ref _NormalFFT, (_RenderedMaps & MapType.Normal) != 0, true);
            }

            if (_DisplacementMaps == null || _NormalMaps == null)
            {
                RenderTexture[] usedDisplacementMaps, usedNormalMaps;

                if (_WindWaves.CopyFrom == null)
                {
                    int resolution = _WindWaves.FinalResolution;
                    int packResolution = resolution << 1;
                    _SingleTargetCache = RenderTexturesCache.GetCache(packResolution, packResolution, 0, RenderTextureFormat.RHalf, true, _HeightFFT is Dx11FFT);
                    _DoubleTargetCache = RenderTexturesCache.GetCache(packResolution, packResolution, 0, RenderTextureFormat.RGHalf, true, _DisplacementFFT is Dx11FFT);

                    if (_DisplacementMaps == null && (_RenderedMaps & MapType.Displacement) != 0)
                        CreateRenderTextures(ref _DisplacementMaps, "[UWS] WavesRendererFFT - Water Displacement Map", RenderTextureFormat.ARGBHalf, 4, true);

                    if (_NormalMaps == null && (_RenderedMaps & MapType.Normal) != 0)
                        CreateRenderTextures(ref _NormalMaps, "[UWS] WavesRendererFFT - Water Normal Map", RenderTextureFormat.ARGBHalf, 2, true);

                    usedDisplacementMaps = _DisplacementMaps;
                    usedNormalMaps = _NormalMaps;
                }
                else
                {
                    var copyFrom = _WindWaves.CopyFrom;

                    if (copyFrom.WindWaves.WaterWavesFFT._WindWaves == null)
                        copyFrom.WindWaves.ResolveFinalSettings(WaterQualitySettings.Instance.CurrentQualityLevel);

                    copyFrom.WindWaves.WaterWavesFFT.ValidateResources();

                    usedDisplacementMaps = copyFrom.WindWaves.WaterWavesFFT._DisplacementMaps;
                    usedNormalMaps = copyFrom.WindWaves.WaterWavesFFT._NormalMaps;
                }

                for (int scaleIndex = 0; scaleIndex < 4; ++scaleIndex)
                {
                    string suffix = scaleIndex != 0 ? scaleIndex.ToString() : "";

                    if (usedDisplacementMaps != null)
                    {
                        string texName = "_GlobalDisplacementMap" + suffix;
                        _Water.Renderer.PropertyBlock.SetTexture(texName, usedDisplacementMaps[scaleIndex]);
                    }

                    if (scaleIndex < 2 && usedNormalMaps != null)
                    {
                        string texName = "_GlobalNormalMap" + suffix;
                        _Water.Renderer.PropertyBlock.SetTexture(texName, usedNormalMaps[scaleIndex]);
                    }
                }

                if (ResourcesChanged != null)
                    ResourcesChanged();
            }
        }
        internal void Enable()
        {
            if (Enabled) return;

            Enabled = true;

            OnCopyModeChanged();

            if (Application.isPlaying)
            {
                if (_LastCopyFrom == null)
                    ValidateResources();

                _WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);
            }

            _FFTUtilitiesMaterial = new Material(_FFTUtilitiesShader) { hideFlags = HideFlags.DontSave };
        }

        internal void Disable()
        {
            if (!Enabled) return;

            Enabled = false;

            Dispose(false);
        }

        internal void OnDestroy()
        {
            Dispose(true);
        }
        #endregion Private Methods
    }
}