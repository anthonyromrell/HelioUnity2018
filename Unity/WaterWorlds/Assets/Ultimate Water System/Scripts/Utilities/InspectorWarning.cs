namespace UltimateWater.Utils
{
    using UnityEngine;

    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class InspectorWarningAttribute : PropertyAttribute
    {
        #region Public Types
        public enum InfoType
        {
            Note,
            Warning,
            Error
        }
        #endregion Public Types

        #region Public Variables
        public readonly string MethodName;
        public readonly InfoType Type;
        #endregion Public Variables

        #region Public Methods
        public InspectorWarningAttribute(string methodName, InfoType type)
        {
            MethodName = methodName;
            Type = type;
        }
        #endregion Public Methods
    }
}