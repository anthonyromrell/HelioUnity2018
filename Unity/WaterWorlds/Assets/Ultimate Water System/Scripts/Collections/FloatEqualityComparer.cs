namespace UltimateWater.Internal
{
    using System.Collections.Generic;

    public class FloatEqualityComparer : IEqualityComparer<float>
    {
        #region Public Methods
        public bool Equals(float x, float y)
        {
            return x == y;
        }

        public int GetHashCode(float obj)
        {
            return (int)System.BitConverter.DoubleToInt64Bits(obj);
        }
        #endregion Public Methods
    }
}