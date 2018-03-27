namespace UltimateWater
{
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    /// Implement this interface to use the attached Renderer for rendering displacement
    ///
    /// [important] you must add/remove the renderer to/from rendering queue,
    /// to do it, use static functions from DynamicWater script:
    ///
    /// - DynamicWater.AddRenderer
    /// - DynamicWater.RemoveRenderer
    ///
    /// </summary>
    public interface ILocalDisplacementRenderer : IDynamicWaterEffects
    {
        /// <summary>
        /// This method should render meshes with displacement and normal. Displacement and normal render targets and projection matrices are already set.
        /// </summary>
        void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays);
    }
}