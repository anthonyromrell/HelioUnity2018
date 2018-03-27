namespace UltimateWater
{
    using Internal;
    using UnityEngine.Rendering;

    /// <summary>
    /// Implement this interface to use the attached Renderer for rendering diffuse color
    ///
    /// [important] you must add/remove the renderer to/from rendering queue,
    /// to do it, use static functions from DynamicWater script:
    ///
    /// - DynamicWater.AddRenderer
    /// - DynamicWater.RemoveRenderer
    ///
    /// </summary>
    public interface ILocalDiffuseRenderer : IDynamicWaterEffects
    {
        /// <summary>
        /// This method should render meshes diffuse color.
        /// </summary>
        void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays);
    }
}