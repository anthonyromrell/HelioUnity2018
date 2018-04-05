namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Performs FFT with compute shaders (fast). The in/out resolution cannot exceed 1024.
    /// </summary>
    public sealed class Dx11FFT : GpuFFT
    {
        #region Public Methods
        public Dx11FFT(ComputeShader shader, int resolution, bool highPrecision, bool twoChannels) : base(resolution, highPrecision, twoChannels, true)
        {
            _Shader = shader;

            _KernelIndex = (_NumButterflies - 5) << 1;

            if (twoChannels)
                _KernelIndex += 10;
        }

        public override void SetupMaterials()
        {
            // nothing to do
        }

        public override void ComputeFFT(Texture tex, RenderTexture target)
        {
            var rt1 = _RenderTexturesSet.GetTemporary();

            if (!target.IsCreated())
            {
                target.enableRandomWrite = true;
                target.Create();
            }

            _Shader.SetTexture(_KernelIndex, "_ButterflyTex", Butterfly);
            _Shader.SetTexture(_KernelIndex, "_SourceTex", tex);
            _Shader.SetTexture(_KernelIndex, "_TargetTex", rt1);
            _Shader.Dispatch(_KernelIndex, 1, tex.height, 1);

            _Shader.SetTexture(_KernelIndex + 1, "_ButterflyTex", Butterfly);
            _Shader.SetTexture(_KernelIndex + 1, "_SourceTex", rt1);
            _Shader.SetTexture(_KernelIndex + 1, "_TargetTex", target);
            _Shader.Dispatch(_KernelIndex + 1, 1, tex.height, 1);

            rt1.Dispose();
        }
        #endregion Public Methods

        #region Private Variables
        private readonly ComputeShader _Shader;
        private readonly int _KernelIndex;
        #endregion Private Variables

        #region Private Methods
        protected override void FillButterflyTexture(Texture2D butterfly, int[][] indices, Vector2[][] weights)
        {
            for (int row = 0; row < _NumButterflies; ++row)
            {
                for (int scaleIndex = 0; scaleIndex < 2; ++scaleIndex)
                {
                    int offset = scaleIndex == 0 ? 0 : _Resolution;

                    for (int col = 0; col < _Resolution; ++col)
                    {
                        Color c;

                        int indexX = _NumButterflies - row - 1;
                        int indexY = (col << 1);

                        c.r = indices[indexX][indexY] + offset;
                        c.g = indices[indexX][indexY + 1] + offset;

                        c.b = weights[row][col].x;
                        c.a = weights[row][col].y;

                        butterfly.SetPixel(offset + col, row, c);
                    }
                }
            }
        }
        #endregion Private Methods
    }
}