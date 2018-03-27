namespace UltimateWater.Utils
{
    [System.Serializable]
    public class MinMaxRange
    {
        #region Public Variables
        public float MinValue;
        public float MaxValue;
        #endregion Public Variables

        #region Public Methods
        public float Random()
        {
            return UnityEngine.Random.Range(MinValue, MaxValue);
        }
        #endregion Public Methods
    }
}