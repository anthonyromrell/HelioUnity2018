namespace UltimateWater
{
    using Internal;
    using UnityEngine;

    public static class GaussianShader
    {
        #region Public Types
        public enum KernelType
        {
            Horizontal,
            Vertical
        }
        #endregion Public Types

        #region Public Variables
        public static int VerticalKernel
        {
            get
            {
                Assign(KernelType.Vertical);
                return _Kernel[(int)KernelType.Vertical];
            }
        }
        public static int HorizontalKernel
        {
            get
            {
                Assign(KernelType.Horizontal);
                return _Kernel[(int)KernelType.Horizontal];
            }
        }

        public static float Term0
        {
            set
            {
                Shader.SetFloat(_K0Name, value);
            }
        }
        public static float Term1
        {
            set
            {
                Shader.SetFloat(_K1Name, value);
            }
        }
        public static float Term2
        {
            set
            {
                Shader.SetFloat(_K2Name, value);
            }
        }

        public static RenderTexture VerticalInput
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _InputName, value);
            }
        }
        public static RenderTexture HorizontalInput
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _InputName, value);
            }
        }

        public static RenderTexture VerticalOutput
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _OutputName, value);
            }
        }
        public static RenderTexture HorizontalOutput
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _OutputName, value);
            }
        }

        public static ComputeShader Shader
        {
            get
            {
                return ShaderUtility.Instance.Get(ComputeShaderList.Gauss);
            }
        }
        #endregion Public Variables

        #region Public Methods
        public static void Dispatch(KernelType type, int width, int height)
        {
            Shader.Dispatch(_Kernel[(int)type],
                width / _ThreadGroupX[(int)type],
                height / _ThreadGroupY[(int)type],
                1 / _ThreadGroupZ[(int)type]);
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly string[] _KernelName = { "Gaussian_Horizontal", "Gaussian_Vertical" };

        private static readonly int[] _Kernel = { -1, -1 };

        private static readonly int[] _ThreadGroupX = new int[2];
        private static readonly int[] _ThreadGroupY = new int[2];
        private static readonly int[] _ThreadGroupZ = new int[2];

        private static readonly string _InputName = "Input";
        private static readonly string _OutputName = "Output";

        private static readonly string _K0Name = "k0";
        private static readonly string _K1Name = "k1";
        private static readonly string _K2Name = "k2";
        #endregion Private Variables

        #region Private Methods
        private static void Assign(KernelType type)
        {
            var value = (int)type;
            if (_Kernel[value] == -1)
            {
                _Kernel[value] = Shader.FindKernel(_KernelName[value]);

                uint x, y, z;
                Shader.GetKernelThreadGroupSizes(_Kernel[value], out x, out y, out z);

                _ThreadGroupX[value] = (int)x;
                _ThreadGroupY[value] = (int)y;
                _ThreadGroupZ[value] = (int)z;
            }
        }
        #endregion Private Methods
    }
}