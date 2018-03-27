namespace UltimateWater
{
    public interface IWaterImageEffect
    {
        /// <summary>
        /// Called by WaterCamera.cs
        /// </summary>
        void OnWaterCameraEnabled();

        /// <summary>
        /// Called by WaterCamera.cs, to update this effect when it's disabled
        /// </summary>
        void OnWaterCameraPreCull();
    }
}