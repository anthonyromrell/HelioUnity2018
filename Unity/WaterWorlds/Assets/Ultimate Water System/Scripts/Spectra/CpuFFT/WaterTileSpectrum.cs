namespace UltimateWater.Internal
{
    using UnityEngine;

#if WATER_SIMD
    using vector4 = Mono.Simd.Vector4f;
#else

    using vector4 = UnityEngine.Vector4;

#endif

    /// <summary>
    ///     Holds data for a spectrum of one of water tiles.
    /// </summary>
    public class WaterTileSpectrum
    {
        #region Public Variables
        public bool IsResolvedByFFT
        {
            get { return ResolveByFFT; }
        }
        public int ResolutionFFT
        {
            get { return _ResolutionFFT; }
        }
        public int MipIndexFFT
        {
            get { return _MipIndexFFT; }
        }

        public readonly Water Water;
        public readonly WindWaves WindWaves;
        public readonly int TileIndex;

        // work-time data
        public Vector2[] DirectionalSpectrum;

        // results
        public Vector2[][] Displacements;
        public vector4[][] ForceAndHeight;
        public float[] ResultsTiming;
        public int RecentResultIndex;

        // state and context
        public bool ResolveByFFT;
        public int DirectionalSpectrumDirty;
        #endregion Public Variables

        #region Public Methods
        public void SetDirty()
        {
            DirectionalSpectrumDirty = 2;
        }
        public bool SetResolveMode(bool resolveByFFT, int resolution)
        {
            if (ResolveByFFT != resolveByFFT || (ResolveByFFT && _ResolutionFFT != resolution))
            {
                if (resolveByFFT)
                {
                    lock (this)
                    {
                        bool looping = WindWaves.LoopDuration != 0.0f;
                        int cachedFrames = looping ? (int)(WindWaves.LoopDuration * 5.0f + 0.6f) : 4;

                        _ResolutionFFT = resolution;
                        _MipIndexFFT = WaterWavesSpectrumDataBase.GetMipIndex(resolution);
                        int resolutionSquared = resolution * resolution;
                        DirectionalSpectrum = new Vector2[resolutionSquared];
                        Displacements = new Vector2[cachedFrames][];
                        ForceAndHeight = new vector4[cachedFrames][];
                        ResultsTiming = new float[cachedFrames];
                        SetDirty();
                        _CachedTime = float.NegativeInfinity;

                        for (int i = 0; i < cachedFrames; ++i)
                        {
                            Displacements[i] = new Vector2[resolutionSquared];
                            ForceAndHeight[i] = new vector4[resolutionSquared];
                        }

                        if (ResolveByFFT == false)
                        {
                            WaterAsynchronousTasks.Instance.AddFFTComputations(this);
                            ResolveByFFT = true;
                        }
                    }
                }
                else
                {
                    WaterAsynchronousTasks.Instance.RemoveFFTComputations(this);
                    ResolveByFFT = false;
                }

                return true;
            }

            return false;
        }
        public void GetResults(float time, out Vector2[] da, out Vector2[] db, out vector4[] fa, out vector4[] fb, out float p)
        {
            if (WindWaves.LoopDuration != 0.0f)
                time = time % WindWaves.LoopDuration;

            lock (this)
            {
                if (time == _CachedTime)
                {
                    da = _CachedDisplacementsA;
                    db = _CachedDisplacementsB;
                    fa = _CachedForceAndHeightA;
                    fb = _CachedForceAndHeightB;
                    p = _CachedTimeProp;

                    return;
                }

                int recentResultIndex = RecentResultIndex;

                for (int i = recentResultIndex - 1; i >= 0; --i)
                {
                    if (ResultsTiming[i] <= time)
                    {
                        int nextIndex = i + 1;

                        da = Displacements[i];
                        db = Displacements[nextIndex];
                        fa = ForceAndHeight[i];
                        fb = ForceAndHeight[nextIndex];

                        float duration = ResultsTiming[nextIndex] - ResultsTiming[i];

                        if (duration != 0.0f)
                            p = (time - ResultsTiming[i]) / duration;
                        else
                            p = 0.0f;

                        if (time > _CachedTime)
                        {
                            _CachedDisplacementsA = da;
                            _CachedDisplacementsB = db;
                            _CachedForceAndHeightA = fa;
                            _CachedForceAndHeightB = fb;
                            _CachedTimeProp = p;
                            _CachedTime = time;
                        }

                        return;
                    }
                }

                for (int i = ResultsTiming.Length - 1; i > recentResultIndex; --i)
                {
                    if (ResultsTiming[i] <= time)
                    {
                        int nextIndex = i != Displacements.Length - 1 ? i + 1 : 0;

                        da = Displacements[i];
                        db = Displacements[nextIndex];
                        fa = ForceAndHeight[i];
                        fb = ForceAndHeight[nextIndex];

                        float duration = ResultsTiming[nextIndex] - ResultsTiming[i];

                        if (duration != 0.0f)
                            p = (time - ResultsTiming[i]) / duration;
                        else
                            p = 0.0f;

                        return;
                    }
                }

                da = Displacements[recentResultIndex];
                db = Displacements[recentResultIndex];
                fa = ForceAndHeight[recentResultIndex];
                fb = ForceAndHeight[recentResultIndex];
                p = 0.0f;
            }
        }
        public WaterTileSpectrum(Water water, WindWaves windWaves, int index)
        {
            Water = water;
            WindWaves = windWaves;
            TileIndex = index;
        }
        #endregion Public Methods

        #region Private Variables
        private int _ResolutionFFT;
        private int _MipIndexFFT;

        private float _CachedTime = float.NegativeInfinity;
        private float _CachedTimeProp;
        private Vector2[] _CachedDisplacementsA, _CachedDisplacementsB;
        private vector4[] _CachedForceAndHeightA, _CachedForceAndHeightB;
        #endregion Private Variables
    }
}