using System.Threading;

namespace UltimateWater.Internal
{
    using System.Linq;
    using UnityEngine;

    public abstract class WaterWavesSpectrumDataBase
    {
        #region Public Variables

        public WaterWave[] CpuWaves { get; private set; }

        public WaterWave[] ShorelineCandidates { get; private set; }

        // [scale index][spectrum data]
        public Vector3[][] SpectrumValues { get; private set; }

        public Texture2D Texture
        {
            get
            {
                if (_Texture == null)
                    CreateSpectrumTexture();

                return _Texture;
            }
        }

        public float Weight { get; set; }

        public float Gravity
        {
            get { return _Gravity; }
        }

        public Vector2 WeatherSystemOffset { get; set; }

        public float WeatherSystemRadius { get; set; }

        /// <summary>
        /// Applies only to non-local weather systems.
        /// </summary>
        public Vector2 WindDirection
        {
            get { return _WindDirection; }
            set { _WindDirection = value; }
        }

        #endregion Public Variables

        #region Public Methods

        public static int GetMipIndex(int i)
        {
            if (i == 0) return 0;

            int mip = (int)Mathf.Log(i, 2) - 3;

            return mip >= 0 ? mip : 0;
        }

        public float GetStandardDeviation()
        {
            return _StdDev;
        }

        public float GetStandardDeviation(int scaleIndex, int mipLevel)
        {
            var data = _StandardDeviationData[scaleIndex];
            return mipLevel < data.Length ? data[mipLevel] : 0.0f;
        }

        public void SetCpuWavesDirty()
        {
            _CpuWavesDirty = true;
        }

        public void ValidateSpectrumData()
        {
            if (CpuWaves != null)
                return;

            lock (this)
            {
                if (CpuWaves != null)
                    return;

                if (SpectrumValues == null)
                {
                    SpectrumValues = new Vector3[4][];
                    _StandardDeviationData = new float[4][];
                }

                int resolution = _WindWaves.FinalResolution;
                int resolutionSquared = resolution * resolution;
                int currentResolutionIndex = Mathf.RoundToInt(Mathf.Log(resolution, 2)) - 4;

                if (SpectrumValues[0] == null || SpectrumValues[0].Length != resolutionSquared)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        SpectrumValues[i] = new Vector3[resolutionSquared];
                        _StandardDeviationData[i] = new float[currentResolutionIndex + 1];
                    }
                }

                GenerateContents(SpectrumValues);
                AnalyzeSpectrum();
            }
        }

        public void UpdateSpectralValues(Vector2 windDirection, float directionality)
        {
            ValidateSpectrumData();

            if (_CpuWavesDirty)
            {
                lock (this)
                {
                    if (CpuWaves == null || !_CpuWavesDirty) return;

                    lock (CpuWaves)
                    {
                        _CpuWavesDirty = false;

                        float directionalityInv = 1.0f - directionality;
                        float horizontalDisplacementScale = _Water.Materials.HorizontalDisplacementScale;
                        int resolution = _WindWaves.FinalResolution;
                        bool mostlySorted = Vector2.Dot(_LastWindDirection, windDirection) >= 0.97f;

                        var cpuWavesLocal = CpuWaves;

                        for (int i = 0; i < cpuWavesLocal.Length; ++i)
                            cpuWavesLocal[i].UpdateSpectralValues(SpectrumValues, windDirection, directionalityInv,
                                resolution, horizontalDisplacementScale);

                        SortCpuWaves(cpuWavesLocal, mostlySorted);

                        var shorelineCandidatesLocal = ShorelineCandidates;

                        for (int i = 0; i < shorelineCandidatesLocal.Length; ++i)
                            shorelineCandidatesLocal[i].UpdateSpectralValues(SpectrumValues, windDirection,
                                directionalityInv, resolution, horizontalDisplacementScale);

                        _LastWindDirection = windDirection;
                    }
                }
            }
        }

        public static void SortCpuWaves(WaterWave[] windWaves, bool mostlySorted)
        {
            if (!mostlySorted)
            {
                System.Array.Sort(windWaves, (a, b) =>
                {
                    if (a._CPUPriority > b._CPUPriority)
                        return -1;
                    else
                        return a._CPUPriority == b._CPUPriority ? 0 : 1;
                });
            }
            else
            {
                // bubble sort
                int numCpuWaves = windWaves.Length;
                int prevIndex = 0;

                for (int index = 1; index < numCpuWaves; ++index)
                {
                    if (windWaves[prevIndex]._CPUPriority < windWaves[index]._CPUPriority)
                    {
                        var t = windWaves[prevIndex];
                        windWaves[prevIndex] = windWaves[index];
                        windWaves[index] = t;

                        if (index != 1) index -= 2;
                    }

                    prevIndex = index;
                }
            }
        }

        public void Dispose(bool onlyTexture)
        {
            if (_Texture != null)
            {
                _Texture.Destroy();
                _Texture = null;
            }

            if (!onlyTexture)
            {
                lock (this)
                {
                    SpectrumValues = null;
                    CpuWaves = null;
                    _CpuWavesDirty = true;
                }
            }
        }

        #endregion Public Methods

        #region Private Variables

        // [scale index][standard deviation for each mip level]
        private float[][] _StandardDeviationData;

        private bool _CpuWavesDirty = true;
        private Vector2 _LastWindDirection;
        private float _StdDev;
        private Vector2 _WindDirection = new Vector2(1.0f, 0.0f);
        private Texture2D _Texture;

        private readonly float _TileSize;
        private readonly float _Gravity;

        protected readonly Water _Water;
        protected readonly WindWaves _WindWaves;

        private static Color[] _Colors = new Color[512 * 512];

        #endregion Private Variables

        #region Private Methods

        protected WaterWavesSpectrumDataBase(Water water, WindWaves windWaves, float tileSize, float gravity)
        {
            _Water = water;
            _WindWaves = windWaves;
            _TileSize = tileSize;
            _Gravity = gravity;
        }

        private void CreateSpectrumTexture()
        {
            ValidateSpectrumData();

            int resolution = _WindWaves.FinalResolution;

            int width = resolution << 1;
            int height = resolution << 1;

            if (_Colors.Length < (width * height))
            {
                _Colors = new Color[width * height];
            }

            // start worker threads
            var threads = new Thread[4];
            for (int i = 0; i < 4; ++i)
            {
                int index = i;
                threads[i] = new Thread(() => { Calculate(index, resolution, width); });
            }
            for (int i = 0; i < 4; ++i)
            {
                threads[i].Start();
            }

            // create texture
            _Texture = new Texture2D(width, height, TextureFormat.RGBAFloat, false, true)
            {
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Repeat
            };

            // join threads
            for (int i = 0; i < 4; ++i)
            {
                threads[i].Join();
            }

            // update texture
            _Texture.SetPixels(0, 0, width, height, _Colors);
            _Texture.Apply(false, true);
        }

        private void Calculate(int scaleIndex, int resolution, int width)
        {
            var data = SpectrumValues[scaleIndex];

            // x axis offset into texture data
            int uOffset = (scaleIndex & 1) == 0 ? 0 : resolution;
            // y axis offset into texture data
            int vOffset = (scaleIndex & 2) == 0 ? 0 : resolution;

            // calculate offset to the quarter we fill
            int uvOffset = vOffset * width + uOffset;

            for (int y = 0; y < resolution; ++y)
            {
                // calculate y offset for spectrum
                int ySpectrum = y * resolution;

                // calculate y offset for texture
                int yIndex = y * width + uvOffset;

                for (int x = 0; x < resolution; ++x)
                {
                    var spectrum = data[ySpectrum + x];
                    int index = yIndex + x;

                    // assign the spectrum color
                    _Colors[index].r = spectrum.x;
                    _Colors[index].g = spectrum.y;
                    _Colors[index].b = spectrum.z;
                }
            }
        }

        private void AnalyzeSpectrum()
        {
            int resolution = _WindWaves.FinalResolution;
            int halfResolution = resolution >> 1;
            int currentResolutionIndex = Mathf.RoundToInt(Mathf.Log(resolution >> 1, 2)) - 4;
            var shorelineCandidatesHeap = new Heap<WaterWave>();
            var importantWavesHeap = new Heap<WaterWave>();
            _StdDev = 0.0f;

            for (byte scaleIndex = 0; scaleIndex < 4; ++scaleIndex)
            {
                var data = SpectrumValues[scaleIndex];
                var scaleStandardDeviationData = _StandardDeviationData[scaleIndex] =
                    new float[currentResolutionIndex + 1];

                float frequencyScale = 2.0f * Mathf.PI / _TileSize;
                float offsetX = _TileSize + (0.5f / resolution) * _TileSize;
                float offsetZ = -_TileSize + (0.5f / resolution) * _TileSize;

                for (int x = 0; x < resolution; ++x)
                {
                    float kx = frequencyScale * (x - halfResolution);
                    ushort u = (ushort)((x + halfResolution) % resolution);
                    ushort offset = (ushort)(u * resolution);

                    for (int y = 0; y < resolution; ++y)
                    {
                        float ky = frequencyScale * (y - halfResolution);
                        ushort v = (ushort)((y + halfResolution) % resolution);

                        Vector3 s = data[offset + v];
                        float ls = s.x * s.x + s.y * s.y;
                        float amplitude = Mathf.Sqrt(ls);
                        float k = Mathf.Sqrt(kx * kx + ky * ky);
                        float w = Mathf.Sqrt(_Gravity * k);

                        // collect important waves
                        if (amplitude >= 0.0025f)
                        {
                            importantWavesHeap.Insert(new WaterWave(scaleIndex, offsetX, offsetZ, u, v, kx, ky, k, w,
                                amplitude));

                            if (importantWavesHeap.Count > 100)
                                importantWavesHeap.ExtractMax();
                        }

                        // collect shoreline candidates
                        if (amplitude > 0.025f)
                        {
                            shorelineCandidatesHeap.Insert(new WaterWave(scaleIndex, offsetX, offsetZ, u, v, kx, ky, k,
                                w, amplitude));

                            if (shorelineCandidatesHeap.Count > 200)
                                shorelineCandidatesHeap.ExtractMax();
                        }

                        // compute total (halved) elevation variance per mip level
                        int mipIndex = GetMipIndex(Mathf.Max(Mathf.Min(u, resolution - u - 1),
                            Mathf.Min(v, resolution - v - 1)));
                        scaleStandardDeviationData[mipIndex] += ls;
                    }
                }

                // half of variance to standard deviation
                for (int i = 0; i < scaleStandardDeviationData.Length; ++i)
                {
                    scaleStandardDeviationData[i] = Mathf.Sqrt(2.0f * scaleStandardDeviationData[i]);
                    _StdDev += scaleStandardDeviationData[i];
                }
            }

            CpuWaves = importantWavesHeap.ToArray();
            SortCpuWaves(CpuWaves, false);

            ShorelineCandidates = shorelineCandidatesHeap.ToArray();
            System.Array.Sort(ShorelineCandidates);
        }

        protected abstract void GenerateContents(Vector3[][] spectrumValues);

        #endregion Private Methods
    }
}