namespace UltimateWater
{
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    /// Implement this interface to use the attached Renderer for rendering foam
    ///
    /// [important] you must add/remove the renderer to/from rendering queue,
    /// to do it, use static functions from DynamicWater script:
    ///
    /// - DynamicWater.AddRenderer
    /// - DynamicWater.RemoveRenderer
    ///
    /// </summary>
    public interface ILocalFoamRenderer : IDynamicWaterEffects
    {
        /// <summary>
        /// This method should render meshes with foam. Foam render target and projection matrices are already set.
        /// </summary>
        void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays);
    }
}