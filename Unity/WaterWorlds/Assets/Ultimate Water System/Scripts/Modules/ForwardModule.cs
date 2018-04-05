namespace UltimateWater
{
    using UnityEngine;

    public class ForwardModule : IRenderModule
    {
        #region Public Methods
        public void OnEnable(WaterCamera camera)
        {
        }
        public void OnDisable(WaterCamera camera)
        {
        }

        public void OnValidate(WaterCamera camera)
        {
        }

        public void Process(WaterCamera waterCamera)
        {
        }

        public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
        {
            RenderWater(waterCamera, source);
            Graphics.Blit(source, destination);
        }
        #endregion Public Methods

        #region Private Methods
        private void RenderWater(WaterCamera waterCamera, RenderTexture source)
        {
            var camera = waterCamera._WaterRenderCamera;
            camera.CopyFrom(waterCamera.CameraComponent);

            camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            camera.enabled = false;
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.depthTextureMode = DepthTextureMode.None;
            camera.renderingPath = RenderingPath.Forward;

#if UNITY_5_6_OR_NEWER
            camera.allowHDR = true;
#else
            camera.hdr = true;
#endif
            camera.targetTexture = source;
            camera.cullingMask = (1 << WaterProjectSettings.Instance.WaterLayer);
            camera.Render();
            camera.targetTexture = null;
        }
        #endregion Private Methods
    }
}
