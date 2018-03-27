namespace UltimateWater
{
    using System;

    public class OverlayRendererOrderAttribute : Attribute
    {
        #region Public Variables
        public int Priority
        {
            get { return _Priority; }
        }
        #endregion Public Variables

        #region Public Methods
        public OverlayRendererOrderAttribute(int priority)
        {
            _Priority = priority;
        }
        #endregion Public Methods

        #region Private Variables
        private readonly int _Priority;
        #endregion Private Variables
    }
}