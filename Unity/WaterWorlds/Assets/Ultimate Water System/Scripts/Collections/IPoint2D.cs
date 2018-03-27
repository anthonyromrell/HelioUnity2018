namespace UltimateWater.Internal
{
    using UnityEngine;

    public interface IPoint2D
    {
        #region Variables
        Vector2 Position { get; }
        #endregion Variables

        #region Methods
        void Destroy();
        #endregion Methods
    }
}