namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Small utility class used to display Unity's builtin gradient inspector.
    /// </summary>
    public class GradientContainer : ScriptableObject
    {
        #region Public Variables
        [FormerlySerializedAs("gradient")]
        public Gradient Gradient;
        #endregion Public Variables
    }
}