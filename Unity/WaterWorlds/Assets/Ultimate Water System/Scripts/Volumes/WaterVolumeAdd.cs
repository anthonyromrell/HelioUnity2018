using UnityEngine;

namespace UltimateWater
{
    /// <summary>
    /// Extends water volume.
    /// </summary>
    [System.Serializable]
    [AddComponentMenu("Ultimate Water/Water Volume Add")]
    public sealed class WaterVolumeAdd : WaterVolumeBase
    {
        #region Private Methods
        protected override void Register(Water water)
        {
            if (water != null)
            {
                water.Volume.AddVolume(this);
            }
        }

        protected override void Unregister(Water water)
        {
            if (water != null)
            {
                water.Volume.RemoveVolume(this);
            }
        }
        #endregion Private Methods
    }
}