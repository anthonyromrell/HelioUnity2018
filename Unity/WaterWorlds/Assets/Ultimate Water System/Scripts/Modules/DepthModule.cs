namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using Internal;

    public class DepthModule : IRenderModule
    {
        #region Public Methods

        public void OnEnable(WaterCamera waterCamera)
        {
            if (!Application.isPlaying) { return; }

            var depthFormat = Compatibility.GetFormat(RenderTextureFormat.Depth,
                new[] { RenderTextureFormat.RFloat, RenderTextureFormat.RHalf, RenderTextureFormat.R8 });

            var blendFormat = Compatibility.GetFormat(RenderTextureFormat.RFloat,
                new[] { RenderTextureFormat.RHalf });

            if (!depthFormat.HasValue || !blendFormat.HasValue) { return; }

            _Commands = new CommandBuffer { name = "[UWS] DepthModule - Render Depth" };

            _CameraDepthTextureId = ShaderVariables.CameraDepthTexture2;
            _WaterDepthTextureId = ShaderVariables.WaterDepthTexture;
            _WaterlessDepthId = ShaderVariables.WaterlessDepthTexture;

            _DepthFormat = depthFormat.Value;
            _BlendedDepthFormat = depthFormat.Value;

            // only >= 4.0 shader targets can copy depth textures
            if (SystemInfo.graphicsShaderLevel < 50)
            {
                // get closest supported format for blended depth
                _BlendedDepthFormat = blendFormat.Value;

                // if supported format is precise (RFloat) but effect quality is low, decrease precision
                if (_BlendedDepthFormat == RenderTextureFormat.RFloat && waterCamera.BaseEffectsQuality < 0.2f)
                {
                    var lowBlend = Compatibility.GetFormat(RenderTextureFormat.RHalf);
                    if (!lowBlend.HasValue) { return; }

                    _BlendedDepthFormat = lowBlend.Value;
                }
            }
        }
        public void OnDisable(WaterCamera waterCamera)
        {
            if (!Application.isPlaying) { return; }

            Unbind(waterCamera);

            if (_DepthBlitCache != null)
            {
                _DepthBlitCache.Destroy();
                _DepthBlitCache = null;
            }

            if (_Commands != null)
            {
                _Commands.Release();
                _Commands = null;
            }
        }

        public void OnValidate(WaterCamera waterCamera)
        {
            ShaderUtility.Instance.Use(ShaderList.WaterDepth);
        }

        public void Process(WaterCamera waterCamera)
        {
            if (!Application.isPlaying) { return; }

            // check, if depth rendering is needed
            if (!waterCamera.RenderWaterDepth && (waterCamera.RenderMode != WaterRenderMode.ImageEffectDeferred))
            {
                return;
            }

            int width = waterCamera.BaseEffectWidth;
            int height = waterCamera.BaseEffectHeight;

            var filter = waterCamera.BaseEffectsQuality > 0.98f ? FilterMode.Point : FilterMode.Bilinear;

            var depthBlitCopyMaterial = _DepthBlit;

            if (_Commands == null)
            {
                _Commands = new CommandBuffer { name = "[Ultimate Water] Render Depth" };
            }
            _Commands.Clear();

            // water depth texture
            _Commands.GetTemporaryRT(_WaterDepthTextureId, width, height, _DepthFormat == RenderTextureFormat.Depth ? 32 : 16, filter, _DepthFormat, RenderTextureReadWrite.Linear);
            _Commands.SetRenderTarget(_WaterDepthTextureId);
            _Commands.ClearRenderTarget(true, true, Color.white);

            waterCamera.AddWaterRenderCommands(_Commands, ShaderUtility.Instance.Get(ShaderList.WaterDepth), true, true, false);

            // waterless depth texture
            _Commands.GetTemporaryRT(_WaterlessDepthId, width, height,
                _BlendedDepthFormat == RenderTextureFormat.Depth ? 32 : 0, FilterMode.Point, _BlendedDepthFormat,
                RenderTextureReadWrite.Linear);

            _Commands.SetRenderTarget(_WaterlessDepthId);
            _Commands.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, depthBlitCopyMaterial, 0, _BlendedDepthFormat == RenderTextureFormat.Depth ? 3 : 0);

            // camera depth texture
            _Commands.GetTemporaryRT(_CameraDepthTextureId, width, height,
                _BlendedDepthFormat == RenderTextureFormat.Depth ? 32 : 0, FilterMode.Point, _BlendedDepthFormat,
                RenderTextureReadWrite.Linear);

            _Commands.SetRenderTarget(_CameraDepthTextureId);
            _Commands.ClearRenderTarget(true, true, Color.white);
            _Commands.DrawMesh(Quads.BipolarXInversedY, Matrix4x4.identity, depthBlitCopyMaterial, 0, _BlendedDepthFormat == RenderTextureFormat.Depth ? 4 : 1);
            _Commands.SetGlobalTexture("_CameraDepthTexture", _CameraDepthTextureId);

            Unbind(waterCamera);
            Bind(waterCamera);
        }
        public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
        {
        }
        #endregion Public Methods

        #region Private Variables
        private CommandBuffer _Commands;

        private RenderTextureFormat _DepthFormat;
        private RenderTextureFormat _BlendedDepthFormat;

        private Material _DepthBlit
        {
            get
            {
                return _DepthBlitCache != null ? _DepthBlitCache : (_DepthBlitCache = ShaderUtility.Instance.CreateMaterial(ShaderList.DepthCopy, HideFlags.DontSave));
            }
        }
        private Material _DepthBlitCache;

        private int _WaterDepthTextureId;
        private int _WaterlessDepthId;
        private int _CameraDepthTextureId;
        #endregion Private Variables

        #region Private Methods
        private void Bind(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;

            bool singlePassStereoRendering = WaterProjectSettings.Instance.SinglePassStereoRendering;
            camera.AddCommandBuffer(camera.actualRenderingPath == RenderingPath.Forward ? (singlePassStereoRendering ? CameraEvent.BeforeForwardOpaque : CameraEvent.AfterDepthTexture) : CameraEvent.BeforeLighting, _Commands);
        }
        private void Unbind(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;

            bool singlePassStereoRendering = WaterProjectSettings.Instance.SinglePassStereoRendering;
            camera.RemoveCommandBuffer(singlePassStereoRendering ? CameraEvent.BeforeForwardOpaque : CameraEvent.AfterDepthTexture, _Commands);
            camera.RemoveCommandBuffer(CameraEvent.BeforeLighting, _Commands);
        }
        #endregion Private Methods
    }
}