#if WATER_SIMD
using vector4 = Mono.Simd.Vector4f;
#else

using vector4 = UnityEngine.Vector4;

#endif

namespace UltimateWater
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Internal;

    public class SpectrumResolverCPU
    {
        #region Public Variables
        public WaterWave[] DirectWaves
        {
            get
            {
                lock (this)
                {
                    return GetValidatedDirectWavesList();
                }
            }
        }

        public float MaxVerticalDisplacement { get; private set; }

        public float MaxHorizontalDisplacement { get; private set; }

        public Vector2 WindDirection { get; private set; }

        public float LastFrameTime { get; private set; }

        public float StdDev { get; private set; }

        public WaterTileSpectrum GetTile(int index)
        {
            return _TileSpectra[index];
        }
        #endregion Public Variables

        #region Public Methods
        public SpectrumResolverCPU(Water water, WindWaves windWaves, int numScales)
        {
            _Water = water;
            _WindWaves = windWaves;

            _SpectraDataCache = new Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>();
            _SpectraDataList = new List<WaterWavesSpectrumDataBase>();
            _OverlayedSpectra = new List<WaterWavesSpectrumDataBase>();
            _NumTiles = numScales;
            _CachedSeed = water.Seed;

            CreateSpectraLevels();
        }

        public List<WaterWavesSpectrumDataBase> GetOverlayedSpectraDirect()
        {
            return _OverlayedSpectra;
        }

        public void DisposeCachedSpectra()
        {
            lock (_SpectraDataCache)
            {
                var kv = _SpectraDataCache.GetEnumerator();
                while (kv.MoveNext())
                {
                    kv.Current.Value.Dispose(false);
                }
                kv.Dispose();
            }

            SetDirectionalSpectrumDirty();
        }

        public WaterWavesSpectrumData GetSpectrumData(WaterWavesSpectrum spectrum)
        {
            WaterWavesSpectrumData spectrumData;

            if (!_SpectraDataCache.TryGetValue(spectrum, out spectrumData))
            {
                lock (_SpectraDataCache)
                {
                    _SpectraDataCache[spectrum] = spectrumData = new WaterWavesSpectrumData(_Water, _WindWaves, spectrum);
                }

                spectrumData.ValidateSpectrumData();
                _CpuWavesDirty = true;

                lock (_SpectraDataList)
                {
                    _SpectraDataList.Add(spectrumData);
                }
            }

            return spectrumData;
        }

        /// <summary>
        /// Add additional spectrum into the mix that doesn't come from used water profiles. It may be some distant storm spectrum or a custom overlay.
        /// </summary>
        /// <param name="spectrum"></param>
        public virtual void AddSpectrum(WaterWavesSpectrumDataBase spectrum)
        {
            lock (_OverlayedSpectra)
            {
                _OverlayedSpectra.Add(spectrum);
            }
        }

        /// <summary>
        /// Remove additional spectrum into the mix that doesn't come from used water profiles. It may be some distant storm spectrum or a custom overlay.
        /// </summary>
        /// <param name="spectrum"></param>
        public virtual void RemoveSpectrum(WaterWavesSpectrumDataBase spectrum)
        {
            lock (_OverlayedSpectra)
            {
                _OverlayedSpectra.Remove(spectrum);
            }
        }

        public bool ContainsSpectrum(WaterWavesSpectrumDataBase spectrum)
        {
            return _OverlayedSpectra.Contains(spectrum);
        }

        public void CacheSpectrum(WaterWavesSpectrum spectrum)
        {
            GetSpectrumData(spectrum);
        }

        public Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData> GetCachedSpectraDirect()
        {
            return _SpectraDataCache;
        }

        public Vector3 GetDisplacementAt(float x, float z, float time)
        {
            Vector3 result = new Vector3();
            x = -(x + _SurfaceOffset.x);
            z = -(z + _SurfaceOffset.y);

            if (_TargetDirectWavesCount == -1)
            {
                // sample FFT results
                for (int scaleIndex = _NumTiles - 1; scaleIndex >= 0; --scaleIndex)
                {
                    if (_TileSpectra[scaleIndex].ResolveByFFT)
                    {
                        float fx, invFx, fy, invFy, t; int index0, index1, index2, index3;
                        Vector2[] da, db; vector4[] fa, fb;
                        InterpolationParams(x, z, scaleIndex, _WindWaves.TileSizes[scaleIndex], out fx, out invFx, out fy, out invFy, out index0, out index1, out index2, out index3);
                        _TileSpectra[scaleIndex].GetResults(time, out da, out db, out fa, out fb, out t);

                        Vector2 subResult = FastMath.Interpolate(
                            ref da[index0], ref da[index1], ref da[index2], ref da[index3],
                            ref db[index0], ref db[index1], ref db[index2], ref db[index3],
                            fx, invFx, fy, invFy, t
                        );

                        result.x -= subResult.x;
                        result.z -= subResult.y;

#if WATER_SIMD
                        result.y += FastMath.Interpolate(
                            fa[index0].W, fa[index1].W, fa[index2].W, fa[index3].W,
                            fb[index0].W, fb[index1].W, fb[index2].W, fb[index3].W,
                            fx, invFx, fy, invFy, t
                        );
#else
                        result.y += FastMath.Interpolate(
                            fa[index0].w, fa[index1].w, fa[index2].w, fa[index3].w,
                            fb[index0].w, fb[index1].w, fb[index2].w, fb[index3].w,
                            fx, invFx, fy, invFy, t
                        );
#endif
                    }
                }
            }
            else
            {
                // sample waves directly
                lock (this)
                {
                    var waves = GetValidatedDirectWavesList();

                    if (waves.Length != 0)
                    {
                        Vector3 subResult = new Vector3();

                        for (int i = 0; i < waves.Length; ++i)
                            subResult += waves[i].GetDisplacementAt(x, z, time);

                        result += subResult;
                    }
                }
            }

            float horizontalScale = -_Water.Materials.HorizontalDisplacementScale * _UniformWaterScale;
            result.x = result.x * horizontalScale;
            result.y *= _UniformWaterScale;
            result.z = result.z * horizontalScale;

            return result;
        }

        public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
        {
            Vector2 result = new Vector2();
            x = -(x + _SurfaceOffset.x);
            z = -(z + _SurfaceOffset.y);

            // sample FFT results
            if (_TargetDirectWavesCount == -1)
            {
                for (int scaleIndex = _NumTiles - 1; scaleIndex >= 0; --scaleIndex)
                {
                    if (_TileSpectra[scaleIndex].ResolveByFFT)
                    {
                        float fx, invFx, fy, invFy, t; int index0, index1, index2, index3;
                        Vector2[] da, db;
                        InterpolationParams(x, z, scaleIndex, _WindWaves.TileSizes[scaleIndex], out fx, out invFx, out fy, out invFy, out index0, out index1, out index2, out index3);

                        vector4[] fa, fb;
                        _TileSpectra[scaleIndex].GetResults(time, out da, out db, out fa, out fb, out t);

                        result -= FastMath.Interpolate(
                            ref da[index0], ref da[index1], ref da[index2], ref da[index3],
                            ref db[index0], ref db[index1], ref db[index2], ref db[index3],
                            fx, invFx, fy, invFy, t
                        );
                    }
                }
            }
            else
            {
                // sample waves directly
                lock (this)
                {
                    var waves = GetValidatedDirectWavesList();

                    if (waves.Length != 0)
                    {
                        Vector2 subResult = new Vector2();

                        for (int i = 0; i < waves.Length; ++i)
                            subResult += waves[i].GetRawHorizontalDisplacementAt(x, z, time);

                        result += subResult;
                    }
                }
            }

            float horizontalScale = -_Water.Materials.HorizontalDisplacementScale * _UniformWaterScale;
            result.x = result.x * horizontalScale;
            result.y = result.y * horizontalScale;

            return result;
        }

        public Vector4 GetForceAndHeightAt(float x, float z, float time)
        {
            vector4 result = new vector4();
            x = -(x + _SurfaceOffset.x);
            z = -(z + _SurfaceOffset.y);

            // sample FFT results
            if (_TargetDirectWavesCount == -1)
            {
                for (int scaleIndex = _NumTiles - 1; scaleIndex >= 0; --scaleIndex)
                {
                    var tileSpectrum = _TileSpectra[scaleIndex];

                    if (tileSpectrum.ResolveByFFT)
                    {
                        float fx, invFx, fy, invFy, t; int index0, index1, index2, index3;
                        vector4[] fa, fb;
                        InterpolationParams(x, z, scaleIndex, _WindWaves.TileSizes[scaleIndex], out fx, out invFx, out fy, out invFy, out index0, out index1, out index2, out index3);

                        Vector2[] da, db;
                        tileSpectrum.GetResults(time, out da, out db, out fa, out fb, out t);

                        result += FastMath.Interpolate(
                            fa[index0], fa[index1], fa[index2], fa[index3],
                            fb[index0], fb[index1], fb[index2], fb[index3],
                            fx, invFx, fy, invFy, t
                        );
                    }
                }
            }
            else
            {
                // sample waves directly
                lock (this)
                {
                    var waves = GetValidatedDirectWavesList();

                    if (waves.Length != 0)
                    {
                        Vector4 subResult = new Vector4();

                        for (int i = 0; i < waves.Length; ++i)
                            waves[i].GetForceAndHeightAt(x, z, time, ref subResult);

#if WATER_SIMD
                        result += new vector4(subResult.x, subResult.y, subResult.z, subResult.w);
#else
                        result += subResult;
#endif
                    }
                }
            }

            float horizontalScale = -_Water.Materials.HorizontalDisplacementScale * _UniformWaterScale;

#if WATER_SIMD
            return new Vector4(result.X * horizontalScale, result.Y * 0.5f, result.Z * horizontalScale, result.W);
#else
            result.x = result.x * horizontalScale;
            result.z = result.z * horizontalScale;
            result.y *= 0.5f * _UniformWaterScale;
            result.w *= _UniformWaterScale;              // not 100% sure about this

            return result;
#endif
        }

        public float GetHeightAt(float x, float z, float time)
        {
            float result = 0.0f;
            x = -(x + _SurfaceOffset.x);
            z = -(z + _SurfaceOffset.y);

            if (_TargetDirectWavesCount == -1)
            {
                // sample FFT results
                for (int scaleIndex = _NumTiles - 1; scaleIndex >= 0; --scaleIndex)
                {
                    if (_TileSpectra[scaleIndex].ResolveByFFT)
                    {
                        float fx, invFx, fy, invFy, t; int index0, index1, index2, index3;
                        vector4[] fa, fb;
                        InterpolationParams(x, z, scaleIndex, _WindWaves.TileSizes[scaleIndex], out fx, out invFx, out fy, out invFy, out index0, out index1, out index2, out index3);

                        Vector2[] da, db;
                        _TileSpectra[scaleIndex].GetResults(time, out da, out db, out fa, out fb, out t);

#if WATER_SIMD
                        result += FastMath.Interpolate(
                            fa[index0].W, fa[index1].W, fa[index2].W, fa[index3].W,
                            fb[index0].W, fb[index1].W, fb[index2].W, fb[index3].W,
                            fx, invFx, fy, invFy, t
                        );
#else
                        float a0 = fa[index0].w * fx + fa[index1].w * invFx;
                        float a1 = fa[index2].w * fx + fa[index3].w * invFx;
                        float a = a0 * fy + a1 * invFy;

                        float b0 = fb[index0].w * fx + fb[index1].w * invFx;
                        float b1 = fb[index2].w * fx + fb[index3].w * invFx;
                        float b = b0 * fy + b1 * invFy;

                        result += a * (1.0f - t) + b * t;
#endif
                    }
                }
            }
            else
            {
                // sample waves directly
                lock (this)
                {
                    var waves = GetValidatedDirectWavesList();

                    if (waves.Length != 0)
                    {
                        float subResult = 0.0f;

                        for (int i = 0; i < waves.Length; ++i)
                            subResult += waves[i].GetHeightAt(x, z, time);

                        result += subResult;
                    }
                }
            }

            return result * _UniformWaterScale;
        }

        public void SetDirectWaveEvaluationMode(int waveCount)
        {
            lock (this)
            {
                if (_DirectWaves == null)
                    _DirectWaves = new WaterWave[0];

                _TargetDirectWavesCount = waveCount;
                _CpuWavesDirty = true;
            }
        }

        public void SetFFTEvaluationMode()
        {
            lock (this)
            {
                _DirectWaves = null;
                _TargetDirectWavesCount = -1;
            }
        }

        public WaterWave[] FindMostMeaningfulWaves(int waveCount)
        {
            var waves = new Heap<WaterWave>();

            for (int spectrumDataIndex = _SpectraDataList.Count - 1; spectrumDataIndex >= 0; --spectrumDataIndex)
            {
                var spectrumData = _SpectraDataList[spectrumDataIndex];
                spectrumData.UpdateSpectralValues(WindDirection, _WindWaves.SpectrumDirectionality);

                lock (spectrumData)
                {
                    float weight = spectrumData.Weight;
                    var cpuWaves = spectrumData.CpuWaves;

                    for (int i = 0; i < cpuWaves.Length; ++i)
                    {
                        var wave = cpuWaves[i];
                        wave._Amplitude *= weight;
                        wave._CPUPriority *= weight;
                        waves.Insert(wave);

                        if (waves.Count > waveCount)
                            waves.ExtractMax();
                    }
                }
            }

            return waves.ToArray();
        }

        public virtual void SetDirectionalSpectrumDirty()
        {
            _CpuWavesDirty = true;

            for (int i = _SpectraDataList.Count - 1; i >= 0; --i)
                _SpectraDataList[i].SetCpuWavesDirty();

            for (int scaleIndex = 0; scaleIndex < _NumTiles; ++scaleIndex)
                _TileSpectra[scaleIndex].SetDirty();
        }
        #endregion Public Methods

        #region Private Variables
        private WaterTileSpectrum[] _TileSpectra;
        private Vector2 _SurfaceOffset;
        private float _UniformWaterScale;
        private WaterWave[] _DirectWaves;
        private int _TargetDirectWavesCount = -1;
        private int _CachedSeed;
        private bool _CpuWavesDirty;

        // statistical data
        private readonly int _NumTiles;
        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        private readonly List<WaterWavesSpectrumDataBase> _SpectraDataList;
        protected readonly List<WaterWavesSpectrumDataBase> _OverlayedSpectra;
        protected Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData> _SpectraDataCache;
        #endregion Private Variables

        #region Private Methods
        internal void Update()
        {
            _SurfaceOffset = _Water.SurfaceOffset;

            float timeToSet = _Water.Time;

            if (_WindWaves.LoopDuration != 0.0f)
            {
                timeToSet = timeToSet % _WindWaves.LoopDuration;
            }

            LastFrameTime = timeToSet;
            _UniformWaterScale = _Water.UniformWaterScale;

            UpdateCachedSeed();

            bool allowFFT = WaterProjectSettings.Instance.AllowCpuFFT;

            for (int scaleIndex = 0; scaleIndex < _NumTiles; ++scaleIndex)
            {
                int fftResolution = 16;
                int mipLevel = 0;

                while (true)
                {
                    float totalStdDev = 0;

                    for (int spectrumIndex = _SpectraDataList.Count - 1; spectrumIndex >= 0; --spectrumIndex)
                    {
                        var spectrum = _SpectraDataList[spectrumIndex];
                        spectrum.ValidateSpectrumData();

                        var stdDev = spectrum.GetStandardDeviation(scaleIndex, mipLevel);
                        totalStdDev += stdDev * spectrum.Weight;
                    }

                    for (int spectrumIndex = _OverlayedSpectra.Count - 1; spectrumIndex >= 0; --spectrumIndex)
                    {
                        var spectrum = _OverlayedSpectra[spectrumIndex];
                        spectrum.ValidateSpectrumData();

                        var stdDev = spectrum.GetStandardDeviation(scaleIndex, mipLevel);
                        totalStdDev += stdDev * spectrum.Weight;
                    }

                    if (totalStdDev < _WindWaves.CpuDesiredStandardError * 0.25f ||
                        fftResolution >= _WindWaves.FinalResolution
                    ) // desired error is multiplied by 0.25 because there are 4 scales that cumulate the standard error
                    {
                        break;
                    }

                    fftResolution <<= 1;
                    ++mipLevel;
                }

                if (fftResolution > _WindWaves.FinalResolution)
                {
                    fftResolution = _WindWaves.FinalResolution;
                }

                var level = _TileSpectra[scaleIndex];
                if (level.SetResolveMode(fftResolution >= 16 && allowFFT, fftResolution))
                {
                    _CpuWavesDirty = true;
                }
            }

#if WATER_DEBUG
            DebugUpdate();
#endif
        }

        internal void SetWindDirection(Vector2 windDirection)
        {
            WindDirection = windDirection;
            SetDirectionalSpectrumDirty();
        }

        private void InterpolationParams(float x, float z, int scaleIndex, float tileSize, out float fx, out float invFx, out float fy, out float invFy, out int index0, out int index1, out int index2, out int index3)
        {
            int resolution = _TileSpectra[scaleIndex].ResolutionFFT;
            float offset = 0.5f / _WindWaves.FinalResolution * tileSize;
            x += offset;
            z += offset;

            float multiplier = resolution / tileSize;
            fx = x * multiplier;
            fy = z * multiplier;
            int indexX = (int)fx; if (indexX > fx) { --indexX; }
            int indexY = (int)fy; if (indexY > fy) { --indexY; }
            fx -= indexX;
            fy -= indexY;

            indexX = indexX % resolution;
            indexY = indexY % resolution;

            if (indexX < 0) { indexX += resolution; }
            if (indexY < 0) { indexY += resolution; }

            indexX = resolution - indexX - 1;
            indexY = resolution - indexY - 1;

            int indexX2 = indexX + 1;
            int indexY2 = indexY + 1;

            if (indexX2 == resolution) indexX2 = 0;
            if (indexY2 == resolution) indexY2 = 0;

            indexY *= resolution;
            indexY2 *= resolution;

            index0 = indexY + indexX;
            index1 = indexY + indexX2;
            index2 = indexY2 + indexX;
            index3 = indexY2 + indexX2;

            invFx = 1.0f - fx;
            invFy = 1.0f - fy;
        }

        private WaterWave[] GetValidatedDirectWavesList()
        {
            if (_CpuWavesDirty)
            {
                _CpuWavesDirty = false;
                var waves = FindMostMeaningfulWaves(_TargetDirectWavesCount);
                _DirectWaves = waves.ToArray();
            }

            return _DirectWaves;
        }

        public GerstnerWave[] SelectShorelineWaves(int waveCount, float angle, float coincidenceRange)
        {
            var waves = new Heap<WaterWave>();

            for (int spectrumDataIndex = _SpectraDataList.Count - 1; spectrumDataIndex >= 0; --spectrumDataIndex)
            {
                var spectrumData = _SpectraDataList[spectrumDataIndex];
                spectrumData.UpdateSpectralValues(WindDirection, _WindWaves.SpectrumDirectionality);

                lock (spectrumData)
                {
                    float weight = spectrumData.Weight;
                    var shorelineCandidates = spectrumData.ShorelineCandidates;

                    for (int i = 0; i < shorelineCandidates.Length; ++i)
                    {
                        var wave = shorelineCandidates[i];
                        wave._Amplitude *= weight;
                        wave._CPUPriority *= weight;

                        float waveAngle = Mathf.Atan2(wave._Nkx, wave._Nky) * Mathf.Rad2Deg;

                        if (Mathf.Abs(Mathf.DeltaAngle(waveAngle, angle)) < coincidenceRange && wave._Amplitude > 0.025f)
                        {
                            waves.Insert(wave);

                            if (waves.Count > waveCount)
                                waves.ExtractMax();
                        }
                    }
                }
            }

            var offsets = new Vector2[4];

            for (int i = 0; i < 4; ++i)
            {
                float tileSize = _WindWaves.TileSizes[i];

                offsets[i].x = tileSize + (0.5f / _WindWaves.FinalResolution) * tileSize;
                offsets[i].y = -tileSize + (0.5f / _WindWaves.FinalResolution) * tileSize;
            }

            var wavesArray = waves.ToArray();
            int c = Mathf.Min(waves.Count, waveCount);
            var gerstners = new GerstnerWave[c];

            for (int i = 0; i < c; ++i)
                gerstners[i] = new GerstnerWave(wavesArray[waves.Count - i - 1], offsets);            // shoreline waves have a reversed order here...

            return gerstners;
        }

        private void UpdateCachedSeed()
        {
            if (_CachedSeed == _Water.Seed) { return; }

            _CachedSeed = _Water.Seed;
            DisposeCachedSpectra();
            OnProfilesChanged();
        }

        internal virtual void OnProfilesChanged()
        {
            var profiles = _Water.ProfilesManager.Profiles;

            var kv = _SpectraDataCache.GetEnumerator();
            while (kv.MoveNext())
            {
                kv.Current.Value.Weight = 0.0f;
            }
            kv.Dispose();

            for (int i = 0; i < profiles.Length; ++i)
            {
                var weightedProfile = profiles[i];
                if (weightedProfile.Weight <= 0.0001f) { continue; }

                var spectrum = weightedProfile.Profile.Spectrum;

                WaterWavesSpectrumData spectrumData;

                if (!_SpectraDataCache.TryGetValue(spectrum, out spectrumData))
                    spectrumData = GetSpectrumData(spectrum);

                spectrumData.Weight = weightedProfile.Weight;
            }

            SetDirectionalSpectrumDirty();

            StdDev = 0.0f;
            float worstCasesFactor = 0.0f;

            kv = _SpectraDataCache.GetEnumerator();
            while (kv.MoveNext())
            {
                var spectrumData = kv.Current.Value;
                spectrumData.ValidateSpectrumData();

                StdDev += spectrumData.GetStandardDeviation() * spectrumData.Weight;

                if (spectrumData.CpuWaves.Length != 0)
                    worstCasesFactor += spectrumData.CpuWaves[0]._Amplitude * spectrumData.Weight;
            }
            kv.Dispose();

            for (int i = _OverlayedSpectra.Count - 1; i >= 0; --i)
            {
                var spectrumData = _OverlayedSpectra[i];
                spectrumData.ValidateSpectrumData();

                StdDev += spectrumData.GetStandardDeviation() * spectrumData.Weight;

                if (spectrumData.CpuWaves.Length != 0)
                    worstCasesFactor += spectrumData.CpuWaves[0]._Amplitude * spectrumData.Weight;
            }

            MaxVerticalDisplacement = StdDev * 1.6f + worstCasesFactor;
            MaxHorizontalDisplacement = MaxVerticalDisplacement * _Water.Materials.HorizontalDisplacementScale;
        }

        private void CreateSpectraLevels()
        {
            _TileSpectra = new WaterTileSpectrum[_NumTiles];
            for (int scaleIndex = 0; scaleIndex < _NumTiles; ++scaleIndex)
            {
                _TileSpectra[scaleIndex] = new WaterTileSpectrum(_Water, _WindWaves, scaleIndex);
            }
        }

#if WATER_DEBUG
        private void DebugUpdate()
        {
            if(Input.GetKeyDown(KeyCode.F10))
            {
                float scale = 0.1f;

                for(int i = 0; i < 4; ++i)
                {
                    if(!tileSpectra[i].IsResolvedByFFT) continue;

                    int resolution = tileSpectra[i].ResolutionFFT;

                    lock (tileSpectra[i])
                    {
                        var tex = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false, true);
                        for(int y = 0; y < resolution; ++y)
                        {
                            for(int x = 0; x < resolution; ++x)
                            {
                                tex.SetPixel(x, y, new Color(tileSpectra[i].directionalSpectrum[y * resolution + x].x, tileSpectra[i].directionalSpectrum[y * resolution + x].y, 0.0f, 1.0f));
                            }
                        }

                        tex.Apply();
                        var bytes = tex.EncodeToPNG();
                        System.IO.File.WriteAllBytes("CPU Dir " + i + ".png", bytes);

                        tex.Destroy();
                    }
                }

                for(int i = 0; i < 4; ++i)
                {
                    if(!tileSpectra[i].IsResolvedByFFT) continue;

                    int resolution = tileSpectra[i].ResolutionFFT;

                    var tex = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false, true);
                    for(int y = 0; y < resolution; ++y)
                    {
                        for(int x = 0; x < resolution; ++x)
                        {
                            tex.SetPixel(x, y, new Color(tileSpectra[i].displacements[1][y * resolution + x].x * water.HorizontalDisplacementScale * scale, tileSpectra[i].forceAndHeight[1][y * resolution + x][3] * scale, tileSpectra[i].displacements[1][y * resolution + x].y * water.HorizontalDisplacementScale * scale, 1.0f));
                        }
                    }

                    tex.Apply();
                    var bytes = tex.EncodeToPNG();
                    System.IO.File.WriteAllBytes("CPU FFT " + i + ".png", bytes);

                    tex.Destroy();
                }

                for(int i = 0; i < 4; ++i)
                {
                    if(!tileSpectra[i].IsResolvedByFFT) continue;

                    int resolution = tileSpectra[i].ResolutionFFT;

                    var tex = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false, true);
                    for(int y = 0; y < resolution; ++y)
                    {
                        for(int x = 0; x < resolution; ++x)
                        {
                            Vector2 displacement = GetHorizontalDisplacementAt((float)(x + 0.5f) / resolution * windWaves.TileSizes[i], (float)(y + 0.5f) / resolution * windWaves.TileSizes[i], 0.0f, 1.0f, Time.time);
                            float height = GetHeightAt((float)(x + 0.5f) / resolution * windWaves.TileSizes[i], (float)(y + 0.5f) / resolution * windWaves.TileSizes[i], 0.0f, 1.0f, Time.time);
                            tex.SetPixel(x, y, new Color(displacement.x * scale, height * scale, displacement.y * scale, 1.0f));
                        }
                    }

                    tex.Apply();
                    var bytes = tex.EncodeToPNG();
                    System.IO.File.WriteAllBytes("CPU FFT Sampled " + i + ".png", bytes);

                    tex.Destroy();
                }
            }
        }
#endif

        internal virtual void OnMapsFormatChanged(bool resolution)
        {
            if (_SpectraDataCache != null)
            {
                var kv = _SpectraDataCache.GetEnumerator();
                while (kv.MoveNext())
                {
                    kv.Current.Value.Dispose(!resolution);
                }
                kv.Dispose();
            }

            if (resolution)
            {
                for (int i = _OverlayedSpectra.Count - 1; i >= 0; --i)
                {
                    _OverlayedSpectra[i].Dispose(false);
                }
            }

            SetDirectionalSpectrumDirty();
        }

        internal virtual void OnDestroy()
        {
            OnMapsFormatChanged(true);
            _SpectraDataCache = null;

            for (int i = _OverlayedSpectra.Count - 1; i >= 0; --i)
            {
                _OverlayedSpectra[i].Dispose(false);
            }
            _OverlayedSpectra.Clear();

            lock (_SpectraDataList)
            {
                _SpectraDataList.Clear();
            }
        }
        #endregion Private Methods
    }
}