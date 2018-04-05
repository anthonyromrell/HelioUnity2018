namespace UltimateWater.Internal
{
    using System.Collections.Generic;

    public class Int32EqualityComparer : IEqualityComparer<int>
    {
        #region Public Variables
        public static Int32EqualityComparer Default
        {
            get { return _DefaultInstance != null ? _DefaultInstance : (_DefaultInstance = new Int32EqualityComparer()); }
        }
        #endregion Public Variables

        #region Public Methods
        public bool Equals(int x, int y)
        {
            return x == y;
        }

        public int GetHashCode(int obj)
        {
            return obj;
        }
        #endregion Public Methods

        #region Private Variables
        private static Int32EqualityComparer _DefaultInstance;
        #endregion Private Variables
    }
}