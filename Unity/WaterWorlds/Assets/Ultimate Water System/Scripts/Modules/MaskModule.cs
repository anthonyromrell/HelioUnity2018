namespace UltimateWater
{
    using UnityEngine;
    using Internal;
    using UnityEngine.Rendering;

    public class MaskModule : IRenderModule
    {
        #region Public Methods
        public void OnEnable(WaterCamera waterCamera)
        {
            _Commands = new CommandBuffer { name = "[Water] : Render Volumes and Masks" };

            _SubtractiveMaskId = ShaderVariables.SubtractiveMask;
            _AdditiveMaskId = ShaderVariables.AdditiveMask;

            _VolumeFrontFastShader = ShaderUtility.Instance.Get(ShaderList.VolumesFrontSimple);
            _VolumeFrontShader = ShaderUtility.Instance.Get(ShaderList.VolumesFront);
            _VolumeBackShader = ShaderUtility.Instance.Get(ShaderList.VolumesBack);
        }

        public void OnDisable(WaterCamera waterCamera)
        {
            if (_Commands != null)
            {
                var camera = waterCamera.CameraComponent;

                var evt = GetEvent(waterCamera);
                camera.RemoveCommandBuffer(evt, _Commands);

                _Commands.Release();
                _Commands = null;
            }
        }
        public void OnValidate(WaterCamera waterCamera)
        {
            var shaders = ShaderUtility.Instance;

            shaders.Use(ShaderList.VolumesFrontSimple);
            shaders.Use(ShaderList.VolumesFront);
            shaders.Use(ShaderList.VolumesBack);
        }

        public void Process(WaterCamera waterCamera)
        {
            if (!waterCamera.RenderVolumes)
            {
                return;
            }

            bool hasSubtractiveVolumes = false;
            bool hasAdditiveVolumes = false;
            bool hasFlatMasks = false;

            var waters = WaterSystem.Instance.Waters;
            for (int i = waters.Count - 1; i >= 0; --i)
            {
                waters[i].Renderer.OnSharedSubtractiveMaskRender(ref hasSubtractiveVolumes, ref hasAdditiveVolumes, ref hasFlatMasks);
            }

            SetupCamera(waterCamera);

            _Commands.Clear();

            SubtractiveMask(waterCamera, hasSubtractiveVolumes, hasFlatMasks);
            AdditiveMask(waterCamera, hasAdditiveVolumes);

            if (_Commands.sizeInBytes != 0)
            {
                var camera = waterCamera.CameraComponent;

                var evt = GetEvent(waterCamera);
                camera.RemoveCommandBuffer(evt, _Commands);
                camera.AddCommandBuffer(evt, _Commands);
            }

            for (int i = waters.Count - 1; i >= 0; --i)
            {
                waters[i].Renderer.OnSharedMaskPostRender();
            }
        }

        public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
        {
        }
        #endregion Public Methods

        #region Private Variables
        private CommandBuffer _Commands;

        private int _SubtractiveMaskId;
        private int _AdditiveMaskId;

        private Shader _VolumeFrontFastShader;
        private Shader _VolumeFrontShader;
        private Shader _VolumeBackShader;
        #endregion Private Variables

        #region Private Methods
        private void SubtractiveMask(WaterCamera waterCamera, bool hasSubtractiveVolumes, bool hasFlatMasks)
        {
            if (hasSubtractiveVolumes || hasFlatMasks)
            {
                RenderSubtractivePass(waterCamera, hasSubtractiveVolumes, hasFlatMasks);
            }
            else
            {
                Shader.SetGlobalTexture(_SubtractiveMaskId, DefaultTextures.Get(Color.clear));
            }
        }
        private void AdditiveMask(WaterCamera waterCamera, bool hasAdditiveVolumes)
        {
            if (hasAdditiveVolumes)
            {
                RenderAdditivePass(waterCamera);
            }
            else
            {
                Shader.SetGlobalTexture(_AdditiveMaskId, DefaultTextures.Get(Color.clear));
            }
        }

        private void RenderSubtractivePass(WaterCamera waterCamera, bool hasSubtractiveVolumes, bool hasFlatMasks)
        {
            int width = waterCamera.BaseEffectWidth;
            int height = waterCamera.BaseEffectHeight;
            var effectFilter = waterCamera.BaseEffectsQuality > 0.98f ? FilterMode.Point : FilterMode.Bilinear;

            var effectCamera = waterCamera.EffectsCamera;
            var effectWaterCamera = effectCamera.GetComponent<WaterCamera>();

            _Commands.GetTemporaryRT(_SubtractiveMaskId, width, height, 24, effectFilter, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _Commands.SetRenderTarget(_SubtractiveMaskId);
            _Commands.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            if (hasSubtractiveVolumes)
            {
                var frontSubtractive = waterCamera.IsInsideSubtractiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader;

                effectCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
                effectWaterCamera.AddWaterRenderCommands(_Commands, frontSubtractive, false, true, waterCamera.IsInsideSubtractiveVolume);
                effectWaterCamera.AddWaterRenderCommands(_Commands, _VolumeBackShader, false, true, false);
            }

            if (hasFlatMasks && waterCamera.RenderFlatMasks)
            {
                effectCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
                effectWaterCamera.AddWaterMasksRenderCommands(_Commands);
            }
        }
        private void RenderAdditivePass(WaterCamera waterCamera)
        {
            int width = waterCamera.BaseEffectWidth;
            int height = waterCamera.BaseEffectHeight;

            var waters = WaterSystem.Instance.Waters;
            var effectCamera = waterCamera.EffectsCamera;
            var effectWaterCamera = effectCamera.GetComponent<WaterCamera>();

            var frontAdditive = waterCamera.IsInsideAdditiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader;
            var effectFilter = waterCamera.BaseEffectsQuality > 0.98f ? FilterMode.Point : FilterMode.Bilinear;

            for (int i = waters.Count - 1; i >= 0; --i)
            {
                waters[i].Renderer.OnSharedMaskAdditiveRender();
            }

            effectCamera.cullingMask = (1 << WaterProjectSettings.Instance.WaterLayer);
            _Commands.GetTemporaryRT(_AdditiveMaskId, width, height, 24, effectFilter, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

            _Commands.SetRenderTarget(_AdditiveMaskId);
            _Commands.ClearRenderTarget(true, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            effectWaterCamera.AddWaterRenderCommands(_Commands, frontAdditive, false, true, waterCamera.IsInsideAdditiveVolume);
            effectWaterCamera.AddWaterRenderCommands(_Commands, _VolumeBackShader, false, true, false);
        }

        private static void SetupCamera(WaterCamera waterCamera)
        {
            var effectCamera = waterCamera.EffectsCamera;
            effectCamera.transform.position = waterCamera.transform.position;
            effectCamera.transform.rotation = waterCamera.transform.rotation;
            effectCamera.projectionMatrix = waterCamera.CameraComponent.projectionMatrix;
        }
        private static CameraEvent GetEvent(WaterCamera waterCamera)
        {
            var camera = waterCamera.CameraComponent;
            return camera.actualRenderingPath == RenderingPath.Forward ? CameraEvent.BeforeForwardOpaque : CameraEvent.BeforeGBuffer;
        }
        #endregion Private Methods
    }
}