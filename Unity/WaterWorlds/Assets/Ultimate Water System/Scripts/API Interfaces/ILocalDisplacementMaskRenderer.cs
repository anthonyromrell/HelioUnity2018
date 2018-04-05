namespace UltimateWater
{
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    /// Implement this interface to use the attached Renderer for rendering displacement mask
    ///
    /// [important] you must add/remove the renderer to/from rendering queue,
    /// to do it, use static functions from DynamicWater script:
    ///
    /// - DynamicWater.AddRenderer
    /// - DynamicWater.RemoveRenderer
    ///
    /// </summary>
    public interface ILocalDisplacementMaskRenderer : IDynamicWaterEffects
    {
        /// <summary>
        /// This method should render meshes with displacement mask. Displacement mask render target and projection matrices are already set.
        /// </summary>
        void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays);
    }
}