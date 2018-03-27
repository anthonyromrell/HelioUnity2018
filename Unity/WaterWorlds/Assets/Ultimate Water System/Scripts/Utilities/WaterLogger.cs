namespace UltimateWater.Internal
{
    using UnityEngine;

    public static class WaterLogger
    {
        #region Public Methods
        public static void Info(string script, string method, string text)
        {
            Debug.Log(_Prefix + text);
        }
        public static void Warning(string script, string method, string text)
        {
            Debug.LogWarning(_Prefix + text);
        }
        public static void Error(string script, string method, string text)
        {
            Debug.LogError(_Prefix + text);
        }
        #endregion Public Methods

        #region Private Variables
        private const string _Prefix = "[Ultimate Water System] : ";
        #endregion Private Variables
    }
}