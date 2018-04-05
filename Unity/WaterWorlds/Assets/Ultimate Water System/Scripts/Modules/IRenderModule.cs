namespace UltimateWater
{
    using UnityEngine;

    public interface IRenderModule
    {
        void OnEnable(WaterCamera camera);
        void OnDisable(WaterCamera camera);

        void OnValidate(WaterCamera camera);

        // called in WaterCamera.OnPreCull
        void Process(WaterCamera waterCamera);

        // called in WaterCamera.OnRenderImageCallback
        void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination);
    }
}