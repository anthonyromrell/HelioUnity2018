namespace UltimateWater
{
    using UnityEngine;

    public class ResolutionAttribute : PropertyAttribute
    {
        #region Public Variables
        public int RecommendedResolution
        {
            get { return _RecommendedResolution; }
        }
        public int[] Resolutions
        {
            get { return _Resolutions; }
        }
        #endregion Public Variables

        #region Public Methods
        public ResolutionAttribute(int recommendedResolution, params int[] resolutions)
        {
            _RecommendedResolution = recommendedResolution;
            _Resolutions = resolutions;
        }
        #endregion Public Methods

        #region Private Variables
        private readonly int _RecommendedResolution;
        private readonly int[] _Resolutions;
        #endregion Private Variables
    }
}