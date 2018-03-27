namespace UltimateWater
{
    using UnityEngine;

    #region Public Types
    public enum WaterRenderMode
    {
        DefaultQueue,
        ImageEffectForward,
        ImageEffectDeferred
    }
    #endregion Public Types

    /// <summary>
    /// Renders water just after all opaque objects. Works fine with fog effects etc.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class WaterCameraIME : MonoBehaviour
    {
        #region Private Variables
        private WaterCamera _WaterCamera;
        #endregion Private Variables

        #region Unity Messages
        private void Awake()
        {
            _WaterCamera = GetComponent<WaterCamera>();
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_WaterCamera == null)
            {
                Graphics.Blit(source, destination);

                if (Application.isPlaying)
                {
                    Destroy(this);
                }
                return;
            }

            _WaterCamera.OnRenderImageCallback(source, destination);
        }
        #endregion Unity Messages
    }
}