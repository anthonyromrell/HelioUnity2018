namespace UltimateWater.Utils
{
    using UnityEngine;

    public class MinMaxRangeAttribute : PropertyAttribute
    {
        #region Public Variables
        public float MinValue;
        public float MaxValue;
        #endregion Public Variables

        #region Ctors/Dtors
        public MinMaxRangeAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
        #endregion Ctors/Dtors
    }
}