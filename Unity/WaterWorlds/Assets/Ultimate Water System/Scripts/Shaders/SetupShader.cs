namespace UltimateWater
{
    using Internal;
    using UnityEngine;

    public static class SetupShader
    {
        #region Public Variables
        public const string KernelName = "Setup";
        public const string MultiName = "MultiSetup";

        public const string PrimaryName = "Previous";
        public const string SecondaryName = "Current";

        public const string PositionName = "Position";
        public const string ForceName = "Force";

        public const string ScaleName = "_Scale";

        public static int Kernel
        {
            get
            {
                if (_Kernel == -1)
                {
                    _Kernel = Shader.FindKernel(KernelName);
                }
                return _Kernel;
            }
        }

        public static int Multi
        {
            get
            {
                if (_Multi == -1)
                {
                    _Multi = Shader.FindKernel(MultiName);
                }
                return _Multi;
            }
        }

        public static RenderTexture Primary
        {
            set
            {
                Shader.SetTexture(Kernel, PrimaryName, value);
            }
        }
        public static RenderTexture Secondary
        {
            set
            {
                Shader.SetTexture(Kernel, SecondaryName, value);
            }
        }

        public static Vector2 Position
        {
            set
            {
                Shader.SetVector(PositionName, value);
            }
        }

        public static float Force
        {
            set
            {
                Shader.SetFloat(ForceName, value);
            }
        }
        public static int Scale
        {
            set
            {
                Shader.SetInt(ScaleName, value);
            }
        }

        public static ComputeShader Shader
        {
            get
            {
                return ShaderUtility.Instance.Get(ComputeShaderList.Setup);
            }
        }

        #endregion Public Variables

        #region Public Methods
        public static void Dispatch()
        {
            Shader.Dispatch(Kernel, 1, 1, 1);
        }
        #endregion Public Methods

        #region Private Variables
        private static int _Kernel = -1;
        private static int _Multi = -1;

        #endregion Private Variables
    }
}