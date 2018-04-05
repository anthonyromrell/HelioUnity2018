namespace UltimateWater.Internal
{
    using System.Collections.Generic;

    public class UInt64EqualityComparer : IEqualityComparer<ulong>
    {
        #region Public Variables
        public static UInt64EqualityComparer Default
        {
            get { return _DefaultInstance != null ? _DefaultInstance : (_DefaultInstance = new UInt64EqualityComparer()); }
        }
        #endregion Public Variables

        #region Public Methods
        public bool Equals(ulong x, ulong y)
        {
            return x == y;
        }

        public int GetHashCode(ulong obj)
        {
            return (int)(obj ^ (obj >> 32));
        }
        #endregion Public Methods

        #region Private Variables
        private static UInt64EqualityComparer _DefaultInstance;
        #endregion Private Variables
    }
}