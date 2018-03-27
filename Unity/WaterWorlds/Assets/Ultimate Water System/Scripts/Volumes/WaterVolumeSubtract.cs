namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    ///     Removes water from the attached colliders volumes. No water will be rendered inside them, objects inside won't be
    ///     affected by physics and cameras won't use underwater image effect.
    /// </summary>
    [AddComponentMenu("Ultimate Water/Water Volume Subtract")]
    public sealed class WaterVolumeSubtract : WaterVolumeBase
    {
        #region Private Variables
        protected override CullMode _CullMode
        {
            get { return CullMode.Front; }
        }
        #endregion Private Variables

        #region Private Methods
        protected override void Register(Water water)
        {
            if (water != null)
            {
                water.Volume.AddSubtractor(this);
            }
        }

        protected override void Unregister(Water water)
        {
            if (water != null)
            {
                water.Volume.RemoveSubtractor(this);
            }
        }
        #endregion Private Methods
    }
}