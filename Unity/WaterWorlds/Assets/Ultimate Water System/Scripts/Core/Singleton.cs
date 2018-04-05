namespace UltimateWater.Internal
{
    /// <summary>
    /// Simple non-unity types Singleton
    /// </summary>
    public class Singleton<T> where T : new()
    {
        #region Public Variables
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
        }
        #endregion Public Variables

        #region Private Variables
        private static T _Instance;
        #endregion Private Variables
    }
}