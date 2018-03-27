namespace UltimateWater
{
    using UnityEngine.Rendering;

    /// <summary>
    /// Implement this interface to use the attached Renderer for rendering dynamic waves
    ///
    /// [important] you must add/remove the renderer to/from rendering queue,
    /// to do it, use static functions from DynamicWater script:
    ///
    /// - DynamicWater.AddRenderer
    /// - DynamicWater.RemoveRenderer
    ///
    /// </summary>
    public interface IWavesInteractive : IDynamicWaterEffects
    {
        void Render(CommandBuffer commandBuffer);
    }
}