namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Rendering;

    public sealed class WaterShadowCastingLight : MonoBehaviour
    {
        #region Unity Methods
        private void Start()
        {
            int shadowmapId = ShaderVariables.WaterShadowmap;

            _CommandBuffer = new CommandBuffer { name = "Water: Copy Shadowmap" };
            _CommandBuffer.GetTemporaryRT(shadowmapId, Screen.width, Screen.height, 32, FilterMode.Point,
                RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            _CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, shadowmapId);

            var lightComponent = GetComponent<Light>();
            lightComponent.AddCommandBuffer(LightEvent.AfterScreenspaceMask, _CommandBuffer);
        }
        #endregion Unity Methods

        #region Private Variables
        private CommandBuffer _CommandBuffer;
        #endregion Private Variables
    }
}