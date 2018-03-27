namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    ///     Displays water spectrum using a few Gerstner waves directly in vertex shader. Works on all platforms.
    /// </summary>
    public sealed class WavesRendererGerstner
    {
        #region Public Types
        [System.Serializable]
        public class Data
        {
            [Range(0, 20)]
            public int NumGerstners = 20;
        }
        #endregion Public Types

        #region Public Variables
        public bool Enabled
        {
            get { return _Enabled; }
        }
        #endregion Public Variables

        #region Public Methods
        public WavesRendererGerstner(Water water, WindWaves windWaves, Data data)
        {
            _Water = water;
            _WindWaves = windWaves;
            _Data = data;
        }

        public void OnWaterRender(Camera camera)
        {
            if (!Application.isPlaying || !_Enabled) return;

            UpdateWaves();
        }

        public void OnWaterPostRender(Camera camera)
        {
        }

        public void BuildShaderVariant(ShaderVariant variant, Water water, WindWaves windWaves, WaterQualityLevel qualityLevel)
        {
            variant.SetUnityKeyword("_WAVES_GERSTNER", _Enabled);
        }

        private void UpdateWaves()
        {
            int frameCount = Time.frameCount;

            if (_LastUpdateFrame == frameCount)
                return;         // it's already done

            _LastUpdateFrame = frameCount;

            var block = _Water.Renderer.PropertyBlock;
            float t = Time.time;

            for (int index = 0; index < _GerstnerFours.Length; ++index)
            {
                var gerstner4 = _GerstnerFours[index];

                Vector4 offset;
                offset.x = gerstner4.Wave0.Offset + gerstner4.Wave0.Speed * t;
                offset.y = gerstner4.Wave1.Offset + gerstner4.Wave1.Speed * t;
                offset.z = gerstner4.Wave2.Offset + gerstner4.Wave2.Speed * t;
                offset.w = gerstner4.Wave3.Offset + gerstner4.Wave3.Speed * t;

                block.SetVector("_GrOff" + index, offset);
            }
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        private readonly Data _Data;

        private Gerstner4[] _GerstnerFours;
        private int _LastUpdateFrame;
        private bool _Enabled;
        #endregion Private Variables

        #region Private Methods
        internal void Enable()
        {
            if (_Enabled) return;

            _Enabled = true;

            if (Application.isPlaying)
            {
                _Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
                FindMostMeaningfulWaves();
            }
        }

        internal void Disable()
        {
            if (!_Enabled) return;

            _Enabled = false;
        }

        internal void OnValidate(WindWaves windWaves)
        {
            if (_Enabled)
                FindMostMeaningfulWaves();
        }

        private void FindMostMeaningfulWaves()
        {
            _WindWaves.SpectrumResolver.SetDirectWaveEvaluationMode(_Data.NumGerstners);
            var directWaves = _WindWaves.SpectrumResolver.DirectWaves;

            int index = 0;
            int numFours = (directWaves.Length >> 2);
            _GerstnerFours = new Gerstner4[numFours];

            // compute texture offsets from the FFT shader to match Gerstner waves to FFT
            var offsets = new Vector2[4];

            for (int i = 0; i < 4; ++i)
            {
                float tileSize = _WindWaves.TileSizes[i];

                offsets[i].x = tileSize + (0.5f / _WindWaves.FinalResolution) * tileSize;
                offsets[i].y = -tileSize + (0.5f / _WindWaves.FinalResolution) * tileSize;
            }

            for (int i = 0; i < numFours; ++i)
            {
                var wave0 = index < directWaves.Length ? new GerstnerWave(directWaves[index++], offsets) : new GerstnerWave();
                var wave1 = index < directWaves.Length ? new GerstnerWave(directWaves[index++], offsets) : new GerstnerWave();
                var wave2 = index < directWaves.Length ? new GerstnerWave(directWaves[index++], offsets) : new GerstnerWave();
                var wave3 = index < directWaves.Length ? new GerstnerWave(directWaves[index++], offsets) : new GerstnerWave();

                _GerstnerFours[i] = new Gerstner4(wave0, wave1, wave2, wave3);
            }

            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            var block = _Water.Renderer.PropertyBlock;
            //block.SetVector("_GerstnerOrigin", new Vector4(water.TileSize + (0.5f / water.SpectraRenderer.FinalResolution) * water.TileSize, -water.TileSize + (0.5f / water.SpectraRenderer.FinalResolution) * water.TileSize, 0.0f, 0.0f));

            for (int index = 0; index < _GerstnerFours.Length; ++index)
            {
                var gerstner4 = _GerstnerFours[index];

                Vector4 amplitude, directionAb, directionCd, frequencies;

                amplitude.x = gerstner4.Wave0.Amplitude;
                frequencies.x = gerstner4.Wave0.Frequency;
                directionAb.x = gerstner4.Wave0.Direction.x;
                directionAb.y = gerstner4.Wave0.Direction.y;

                amplitude.y = gerstner4.Wave1.Amplitude;
                frequencies.y = gerstner4.Wave1.Frequency;
                directionAb.z = gerstner4.Wave1.Direction.x;
                directionAb.w = gerstner4.Wave1.Direction.y;

                amplitude.z = gerstner4.Wave2.Amplitude;
                frequencies.z = gerstner4.Wave2.Frequency;
                directionCd.x = gerstner4.Wave2.Direction.x;
                directionCd.y = gerstner4.Wave2.Direction.y;

                amplitude.w = gerstner4.Wave3.Amplitude;
                frequencies.w = gerstner4.Wave3.Frequency;
                directionCd.z = gerstner4.Wave3.Direction.x;
                directionCd.w = gerstner4.Wave3.Direction.y;

                block.SetVector("_GrAB" + index, directionAb);
                block.SetVector("_GrCD" + index, directionCd);
                block.SetVector("_GrAmp" + index, amplitude);
                block.SetVector("_GrFrq" + index, frequencies);
            }

            // zero unused waves
            for (int index = _GerstnerFours.Length; index < 5; ++index)
                block.SetVector("_GrAmp" + index, Vector4.zero);
        }

        private void OnProfilesChanged(Water water)
        {
            FindMostMeaningfulWaves();
        }
        #endregion Private Methods
    }

    public class Gerstner4
    {
        #region Public Variables
        public GerstnerWave Wave0;
        public GerstnerWave Wave1;
        public GerstnerWave Wave2;
        public GerstnerWave Wave3;
        #endregion Public Variables

        #region Public Methods
        public Gerstner4(GerstnerWave wave0, GerstnerWave wave1, GerstnerWave wave2, GerstnerWave wave3)
        {
            Wave0 = wave0;
            Wave1 = wave1;
            Wave2 = wave2;
            Wave3 = wave3;
        }
        #endregion Public Methods
    }

    public class GerstnerWave
    {
        #region Public Variables
        public Vector2 Direction;
        public float Amplitude;
        public float Offset;
        public float Frequency;
        public float Speed;
        #endregion Public Variables

        #region Public Methods
        public GerstnerWave()
        {
            Direction = new Vector2(0, 1);
            Frequency = 1;
        }

        public GerstnerWave(WaterWave wave, Vector2[] scaleOffsets)
        {
            float speed = wave._W;
            float mapOffset = (scaleOffsets[wave._ScaleIndex].x * wave._Nkx + scaleOffsets[wave._ScaleIndex].y * wave._Nky) * wave.K;       // match Gerstner to FFT map equivalent

            Direction = new Vector2(wave._Nkx, wave._Nky);
            Amplitude = wave._Amplitude;
            Offset = mapOffset + wave._Offset;
            Frequency = wave.K;
            Speed = speed;
        }

        public GerstnerWave(Vector2 direction, float amplitude, float offset, float frequency, float speed)
        {
            Direction = direction;
            Amplitude = amplitude;
            Offset = offset;
            Frequency = frequency;
            Speed = speed;
        }
        #endregion Public Methods
    }
}