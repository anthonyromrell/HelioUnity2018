namespace UltimateWater
{
    using Internal;

    /// <summary>
    ///     Implement this interface to create components that render into water local maps.
    /// </summary>
    public interface IOverlaysRenderer
    {
        void RenderOverlays(DynamicWaterCameraData overlays);
        void RenderFoam(DynamicWaterCameraData overlays);
    }
}