using UnityEngine.Serialization;

namespace UltimateWater.Internal
{
    using UnityEngine;

    [System.Serializable]
    public class Blur
    {
        #region Public Variables
        public int Iterations
        {
            get { return _Iterations; }
            set
            {
                // preserve total blur size
                float totalSize = TotalSize;
                _Iterations = value;
                TotalSize = totalSize;
            }
        }

        public float Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        public float TotalSize
        {
            get { return _Size * _Iterations; }
            set { _Size = value / _Iterations; }
        }

        public Material BlurMaterial
        {
            get
            {
                if (_BlurMaterial == null)
                {
                    if (_BlurShader == null)
                        Validate();

                    _BlurMaterial = new Material(_BlurShader) { hideFlags = HideFlags.DontSave };
                }

                return _BlurMaterial;
            }

            set
            {
                _BlurMaterial = value;
            }
        }

        public int PassIndex
        {
            get { return _PassIndex; }
            set { _PassIndex = value; }
        }
        #endregion Public Variables

        #region Public Methods
        public virtual void Validate(string shaderName, string computeShaderName = null, int kernelIndex = 0)
        {
            _BlurShader = Shader.Find(shaderName);

            if (computeShaderName != null)
            {
                _BlurComputeShader = Resources.Load<ComputeShader>(computeShaderName);
                _ComputeShaderKernelIndex = kernelIndex;
            }
            else
                _BlurComputeShader = null;
        }
        public void Dispose()
        {
            if (_BlurMaterial != null)
                Object.DestroyImmediate(_BlurMaterial);

            if (_ShaderWeights != null)
            {
                _ShaderWeights.Release();
                _ShaderWeights = null;
            }
        }
        public void Validate()
        {
            Validate("UltimateWater/Utilities/Blur", "Shaders/Blurs");
        }
        public void Apply(RenderTexture target)
        {
            if (_Iterations == 0)
                return;

            //if (SystemInfo.supportsComputeShaders)
            //	ApplyComputeShader(target);
            //else
            ApplyPixelShader(target);
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector]
        [SerializeField, FormerlySerializedAs("blurComputeShader")]
        protected ComputeShader _BlurComputeShader;

        [HideInInspector]
        [SerializeField, FormerlySerializedAs("blurShader")]
        private Shader _BlurShader;

        [HideInInspector]
        [SerializeField, FormerlySerializedAs("computeShaderKernelIndex")]
        private int _ComputeShaderKernelIndex;

        [Range(0, 10)]
        [SerializeField, FormerlySerializedAs("iterations")]
        private int _Iterations = 1;

        [SerializeField, FormerlySerializedAs("size")]
        private float _Size = 0.005f;
        #endregion Inspector Variables

        #region Private Variables
        private Material _BlurMaterial;

        private int _PassIndex;
        protected ComputeBuffer _ShaderWeights;
        #endregion Private Variables

        #region Private Methods
        protected void ApplyComputeShader(RenderTexture target)
        {
            if (_ShaderWeights == null)
            {
                _ShaderWeights = new ComputeBuffer(4, 16);
                _ShaderWeights.SetData(new[]
                {
                    new Color(0.324f, 0.324f, 0.324f, 1.0f),
                    new Color(0.232f, 0.232f, 0.232f, 1.0f),
                    new Color(0.0855f, 0.0855f, 0.0855f, 1.0f),
                    new Color(0.0205f, 0.0205f, 0.0205f, 1.0f)
                });
            }

            int w = target.width;
            int kernelIndex = w == 128 ? _ComputeShaderKernelIndex : (w == 256 ? _ComputeShaderKernelIndex + 2 : _ComputeShaderKernelIndex + 4);

            var temp = RenderTexturesCache.GetTemporary(target.width, target.height, 0, target.format, true, true);

            _BlurComputeShader.SetBuffer(kernelIndex, "weights", _ShaderWeights);
            _BlurComputeShader.SetTexture(kernelIndex, "_MainTex", target);
            _BlurComputeShader.SetTexture(kernelIndex, "_Output", temp);
            _BlurComputeShader.Dispatch(kernelIndex, 1, target.height, 1);

            ++kernelIndex;

            _BlurComputeShader.SetBuffer(kernelIndex, "weights", _ShaderWeights);
            _BlurComputeShader.SetTexture(kernelIndex, "_MainTex", temp);
            _BlurComputeShader.SetTexture(kernelIndex, "_Output", target);
            _BlurComputeShader.Dispatch(kernelIndex, target.width, 1, 1);

            temp.Dispose();
        }

        protected void ApplyPixelShader(RenderTexture target)
        {
            var blurMaterial = BlurMaterial;

            var originalFilterMode = target.filterMode;
            target.filterMode = FilterMode.Bilinear;

            var temp = RenderTexture.GetTemporary(target.width, target.height, 0, target.format);
            temp.filterMode = FilterMode.Bilinear;
            temp.name = "[UWS] Blur - ApplyPixelShader temporary";

            for (int i = 0; i < _Iterations; ++i)
            {
                float blurSize = _Size * (1.0f + i * 0.5f);

                blurMaterial.SetVector(ShaderVariables.Offset, new Vector4(blurSize, 0.0f, 0.0f, 0.0f));
                Graphics.Blit(target, temp, blurMaterial, _PassIndex);

                blurMaterial.SetVector(ShaderVariables.Offset, new Vector4(0.0f, blurSize, 0.0f, 0.0f));
                Graphics.Blit(temp, target, blurMaterial, _PassIndex);
            }

            target.filterMode = originalFilterMode;

            RenderTexture.ReleaseTemporary(temp);
        }
        #endregion Private Methods
    }
}