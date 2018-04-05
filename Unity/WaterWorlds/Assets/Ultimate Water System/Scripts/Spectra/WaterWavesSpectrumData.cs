namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Resolves spectrum data in the context of a specific water object.
    /// </summary>
    public sealed class WaterWavesSpectrumData : WaterWavesSpectrumDataBase
    {
        #region Public Methods
        public WaterWavesSpectrumData(Water water, WindWaves windWaves, WaterWavesSpectrum spectrum) : base(water, windWaves, spectrum.TileSize, spectrum.Gravity)
        {
            _Spectrum = spectrum;
        }
        #endregion Public Methods

        #region Private Variables
        private readonly WaterWavesSpectrum _Spectrum;
        #endregion Private Variables

        #region Private Methods
        protected override void GenerateContents(Vector3[][] spectrumValues)
        {
            int resolution = _WindWaves.FinalResolution;
            Vector4 tileSizeScales = _WindWaves.TileSizeScales;
            int seed = _Water.Seed != 0 ? _Water.Seed : Random.Range(0, 1000000);

            var qualityLevels = WaterQualitySettings.Instance.GetQualityLevelsDirect();
            int maxResolution = qualityLevels[qualityLevels.Length - 1].MaxSpectrumResolution;

            if (resolution > maxResolution)
                Debug.LogWarningFormat(
                    "In water quality settings spectrum resolution of {0} is max, but at runtime a spectrum with resolution of {1} is generated. That may generate some unexpected behaviour. Make sure that the last water quality level has the highest resolution and don't alter it at runtime.",
                    maxResolution, resolution);

            for (byte scaleIndex = 0; scaleIndex < 4; ++scaleIndex)
            {
#if UNITY_5_4_OR_NEWER
                var randomState = Random.state;
                Random.InitState(seed + scaleIndex);
#else
                Random.seed = seed + scaleIndex;
#endif

                _Spectrum.ComputeSpectrum(spectrumValues[scaleIndex], tileSizeScales[scaleIndex], maxResolution, null);

#if UNITY_5_4_OR_NEWER
                // restore random number generator state to not affect game randomness
                Random.state = randomState;
#endif
            }
        }
        #endregion Private Methods
    }
}