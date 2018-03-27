namespace UltimateWater
{
    using Internal;
    using UnityEngine;

    public static class RipplesShader
    {
        #region Public Variables
        public static int Kernel
        {
            get
            {
                if (_Kernel == -1)
                {
                    _Kernel = Shader.FindKernel(_KernelName);

                    uint x, y, z;
                    Shader.GetKernelThreadGroupSizes(Kernel, out x, out y, out z);

                    _ThreadGroupX = (int)x;
                    _ThreadGroupY = (int)y;
                    _ThreadGroupZ = (int)z;
                }
                return _Kernel;
            }
        }

        public static Vector2 Size
        {
            set
            {
                Shader.SetVector(_SizeInvName, new Vector2(1.0f / value.x, 1.0f / value.y));
                Shader.SetInts(_SizeName, (int)value.x, (int)value.y);
            }
        }

        public static ComputeShader Shader
        {
            get
            {
                return ShaderUtility.Instance.Get(ComputeShaderList.Simulation);
            }
        }

        public static void SetDepth(Texture value, Material material)
        {
            SetTextureShaderVariable(_DepthName, value, material);
        }

        public static void SetPreviousDepth(Texture value, Material material)
        {
            SetTextureShaderVariable(_PreviousDepthName, value, material);
        }
        public static void SetStaticDepth(Texture value, Material material)
        {
            SetTextureShaderVariable(_StaticDepthName, value, material);
        }

        public static void SetPrimary(Texture value, Material material)
        {
            SetTextureShaderVariable(_PrimaryName, value, material);
        }
        public static void SetSecondary(Texture value, Material material)
        {
            SetTextureShaderVariable(_SecondaryName, value, material);
        }

        public static void SetDamping(float value, Material material)
        {
            SetFloatShaderVariable(_DampingName, value, material);
        }
        public static void SetPropagation(float value, Material material)
        {
            SetFloatShaderVariable(_PropagationName, value, material);
        }

        public static void SetGain(float value, Material material)
        {
            SetFloatShaderVariable(_GainName, value, material);
        }

        public static void SetHeightGain(float value, Material material)
        {
            SetFloatShaderVariable(_HeightGainName, value, material);
        }
        public static void SetHeightOffset(float value, Material material)
        {
            SetFloatShaderVariable(_HeightOffsetName, value, material);
        }
        #endregion Public Variables

        #region Public Methods
        public static void Dispatch(int width, int height)
        {
            Shader.Dispatch(Kernel, width / _ThreadGroupX, height / _ThreadGroupY, 1 / _ThreadGroupZ);
        }
        #endregion Public Methods

        #region Private Variables
        private static int _ThreadGroupX;
        private static int _ThreadGroupY;
        private static int _ThreadGroupZ;

        private const string _KernelName = "Simulation";
        private const string _DepthName = "Depth";
        private const string _PreviousDepthName = "DepthT1";
        private const string _StaticDepthName = "StaticDepth";

        private const string _SizeInvName = "SizeInv";
        private const string _SizeName = "Size";

        private const string _PrimaryName = "MatrixT1";
        private const string _SecondaryName = "MatrixT2";

        private const string _PropagationName = "Propagation";
        private const string _DampingName = "Damping";

        private const string _GainName = "Gain";
        private const string _HeightGainName = "HeightGain";
        private const string _HeightOffsetName = "HeightOffset";

        private static int _Kernel = -1;
        #endregion Private Variables

        #region Private Methods
        private static void SetFloatShaderVariable(string name, float value, Material material)
        {
            var mode = WaterQualitySettings.Instance.Ripples.ShaderMode;
            switch (mode)
            {
                case WaterRipplesData.ShaderModes.ComputeShader:
                {
                    Shader.SetFloat(name, value);
                    break;
                }

                case WaterRipplesData.ShaderModes.PixelShader:
                {
                    material.SetFloat(name, value);
                    break;
                }
            }
        }

        private static void SetTextureShaderVariable(string name, Texture value, Material material)
        {
            var mode = WaterQualitySettings.Instance.Ripples.ShaderMode;
            switch (mode)
            {
                case WaterRipplesData.ShaderModes.ComputeShader:
                {
                    Shader.SetTexture(Kernel, name, value);
                    break;
                }

                case WaterRipplesData.ShaderModes.PixelShader:
                {
                    material.SetTexture(name, value);
                    break;
                }
            }
        }
        #endregion Private Methods
    }
}