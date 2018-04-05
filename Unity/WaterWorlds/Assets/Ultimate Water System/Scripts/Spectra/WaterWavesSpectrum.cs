namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    ///     Base class for oceanic omnidirectional spectrum generators.
    /// </summary>
    public abstract class WaterWavesSpectrum
    {
        #region Public Variables
        public float TileSize
        {
            get { return _TileSize * WaterQualitySettings.Instance.TileSizeScale; }
        }
        public float Gravity
        {
            get { return _Gravity; }
        }
        #endregion Public Variables

        #region Public Methods
        public abstract void ComputeSpectrum(Vector3[] spectrum, float tileSizeMultiplier, int maxResolution, System.Random random);
        #endregion Public Methods

        #region Private Variables
        protected float _TileSize;
        protected float _Gravity;
        protected float _WindSpeed;
        protected float _Amplitude;
        #endregion Private Variables

        #region Private Methods
        protected WaterWavesSpectrum(float tileSize, float gravity, float windSpeed, float amplitude)
        {
            _TileSize = tileSize;
            _Gravity = gravity;
            _WindSpeed = windSpeed;
            _Amplitude = amplitude;
        }
        #endregion Private Methods
    }
}