namespace UltimateWater
{
    using Internal;
    using UnityEngine;

    public static class TransferShader
    {
        #region Public Types
        public enum KernelType
        {
            Horizontal = 0,
            Vertical = 1,
            Unknown
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

        public static RenderTexture HorizontalSourceA
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _SourceAName, value);
            }
        }
        public static RenderTexture HorizontalSourceB
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _SourceBName, value);
            }
        }
        public static RenderTexture HorizontalDestinationA
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _DestinationAName, value);
            }
        }
        public static RenderTexture HorizontalDestinationB
        {
            set
            {
                Shader.SetTexture(HorizontalKernel, _DestinationBName, value);
            }
        }

        public static RenderTexture VerticalSourceA
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _SourceAName, value);
            }
        }
        public static RenderTexture VerticalSourceB
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _SourceBName, value);
            }
        }
        public static RenderTexture VerticalDestinationA
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _DestinationAName, value);
            }
        }
        public static RenderTexture VerticalDestinationB
        {
            set
            {
                Shader.SetTexture(VerticalKernel, _DestinationBName, value);
            }
        }

        public static int From
        {
            set
            {
                Shader.SetInt(_FromName, value);
            }
        }
        public static int To
        {
            set
            {
                Shader.SetInt(_ToName, value);
            }
        }

        public static float Width
        {
            set
            {
                Shader.SetFloat(_InvWidthName, 1.0f / value);
            }
        }
        public static float Height
        {
            set
            {
                Shader.SetFloat(_InvHeightName, 1.0f / value);
            }
        }

        public static ComputeShader Shader
        {
            get
            {
                return ShaderUtility.Instance.Get(ComputeShaderList.Transfer);
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

        public static void DispatchVertical(int height)
        {
            Dispatch(KernelType.Vertical, height, 1);
        }
        public static void DistpachHorizontal(int width)
        {
            Dispatch(KernelType.Horizontal, 1, width);
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly string[] _KernelName = { "VerticalTransfer", "HorizontalTransfer" };

        private static readonly int[] _ThreadGroupX = new int[2];
        private static readonly int[] _ThreadGroupY = new int[2];
        private static readonly int[] _ThreadGroupZ = new int[2];

        private const string _SourceAName = "SourceA";
        private const string _SourceBName = "SourceB";
        private const string _DestinationAName = "DestinationA";
        private const string _DestinationBName = "DestinationB";

        private const string _InvWidthName = "InvWidth";
        private const string _InvHeightName = "InvHeight";

        private const string _FromName = "From";
        private const string _ToName = "To";

        private static readonly int[] _Kernel = { -1, -1 };
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