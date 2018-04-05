namespace UltimateWater.Internal
{
    using UnityEngine;
    using UnityEngine.Serialization;

    [System.Serializable]
    public sealed class BlurSSS : Blur
    {
        #region Public Methods
        public override void Validate(string shaderName, string computeShaderName = null, int kernelIndex = 0)
        {
            base.Validate(shaderName, computeShaderName, kernelIndex);

            if (!_InitializedDefaults)
            {
                Iterations = 5;
                _InitializedDefaults = true;
            }
        }
        public void Apply(RenderTexture source, RenderTexture target, Color absorptionColor, float worldSpaceSize, float lightFractionToIgnore)
        {
            //if(SystemInfo.supportsComputeShaders)
            //	ApplyComputeShader(target, absorptionColor, worldSpaceSize, lightFractionToIgnore);
            //else
            ApplyPixelShader(source, target, absorptionColor, worldSpaceSize, lightFractionToIgnore);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("initializedDefaults")]
        private bool _InitializedDefaults;
        #endregion Inspector Variables

        #region Private Methods
        private void ApplyPixelShader(RenderTexture source, RenderTexture target, Color absorptionColor, float worldSpaceSize, float lightFractionToIgnore)
        {
            Color absorptionColorPerPixel = absorptionColor * (-2.5f * worldSpaceSize);

            // it's multiplied by -1, so min is actually max
            float minAbsorption = absorptionColorPerPixel.r > absorptionColorPerPixel.g ? absorptionColorPerPixel.r : absorptionColorPerPixel.g;

            if (absorptionColorPerPixel.b > minAbsorption)
                minAbsorption = absorptionColorPerPixel.b;

            float maxDistance = Mathf.Log(lightFractionToIgnore) / minAbsorption;

            var originalFilterMode = source.filterMode;
            source.filterMode = FilterMode.Bilinear;

            var blurMaterial = BlurMaterial;
            blurMaterial.SetColor(ShaderVariables.AbsorptionColorPerPixel, absorptionColorPerPixel);
            blurMaterial.SetFloat(ShaderVariables.MaxDistance, maxDistance);
            Graphics.Blit(source, target, blurMaterial, 0);

            source.filterMode = originalFilterMode;
        }
        #endregion Private Methods
    }
}