namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Computes FFT on shader model 2.0 pixel shaders. The only considerable requirement is the support for at least half
    ///     precision fp render textures.
    /// </summary>
    public sealed class PixelShaderFFT : GpuFFT
    {
        #region Public Methods
        public PixelShaderFFT(Shader fftShader, int resolution, bool highPrecision, bool twoChannels) : base(resolution, highPrecision, twoChannels, false)
        {
            _Material = new Material(fftShader) { hideFlags = HideFlags.DontSave };
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_Material == null)
                Object.Destroy(_Material);
        }

        public override void SetupMaterials()
        {
            _Material.SetTexture(ShaderVariables.ButterflyTex, Butterfly);
        }

        public override void ComputeFFT(Texture tex, RenderTexture target)
        {
            using (_RT1 = _RenderTexturesSet.GetTemporary())
            using (_RT2 = _RenderTexturesSet.GetTemporary())
            {
                ComputeFFT(tex, null, _TwoChannels ? 2 : 0);
                ComputeFFT(_RT1, target, _TwoChannels ? 3 : 1);
            }
        }
        #endregion Public Methods

        #region Private Variables
        private TemporaryRenderTexture _RT1;
        private TemporaryRenderTexture _RT2;

        private readonly Material _Material;
        #endregion Private Variables

        #region Private Methods
        private void ComputeFFT(Texture tex, RenderTexture target, int passIndex)
        {
            _Material.SetFloat(ShaderVariables.ButterflyPass, 0.5f / _NumButterfliesPow2);
            Graphics.Blit(tex, _RT2, _Material, passIndex);

            SwapRT();

            for (int i = 1; i < _NumButterflies; ++i)
            {
                if (target != null && i == _NumButterflies - 1)
                {
                    _Material.SetFloat(ShaderVariables.ButterflyPass, (i + 0.5f) / _NumButterfliesPow2);
                    Graphics.Blit(_RT1, target, _Material, passIndex == 1 ? 4 : 5);
                }
                else
                {
                    _Material.SetFloat(ShaderVariables.ButterflyPass, (i + 0.5f) / _NumButterfliesPow2);
                    Graphics.Blit(_RT1, _RT2, _Material, passIndex);
                }

                SwapRT();
            }
        }

        private void SwapRT()
        {
            var t = _RT1;
            _RT1 = _RT2;
            _RT2 = t;
        }
        #endregion Private Methods
    }
}