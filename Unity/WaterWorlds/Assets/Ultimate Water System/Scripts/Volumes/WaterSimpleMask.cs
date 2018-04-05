namespace UltimateWater.Internal
{
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <inheritdoc />
    /// <summary>
    ///     Attach this to objects supposed to mask water in screen-space. It will mask both water surface and camera's
    ///     underwater image effect. Great for sections etc.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public sealed class WaterSimpleMask : MonoBehaviour
    {
        #region Public Variables
        public Renderer Renderer { get; private set; }

        public int RenderQueuePriority
        {
            get { return _RenderQueuePriority; }
        }

        public Water Water
        {
            get { return _Water; }
            set
            {
                if (_Water == value)
                    return;

                enabled = false;
                _Water = value;
                enabled = true;
            }
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("water")] private Water _Water;
        [SerializeField, FormerlySerializedAs("renderQueuePriority")] private int _RenderQueuePriority;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnValidate()
        {
            SetObjectLayer();
        }
        private void OnEnable()
        {
            Renderer = GetComponent<Renderer>();
            Renderer.enabled = false;
            Renderer.material.SetFloat(_WaterIdName, 1 << _Water.WaterId);

            SetObjectLayer();

            if (Renderer == null)
            {
                throw new System.InvalidOperationException("WaterSimpleMask is attached to an object without any renderer.");
            }

            _Water.Renderer.AddMask(this);
            _Water.WaterIdChanged += OnWaterIdChanged;
        }

        private void OnDisable()
        {
            _Water.WaterIdChanged -= OnWaterIdChanged;
            _Water.Renderer.RemoveMask(this);
        }
        #endregion Unity Methods

        #region Private Variables
        private const string _WaterIdName = "_WaterId";
        #endregion Private Variables

        #region Private Methods
        private void SetObjectLayer()
        {
            if (gameObject.layer != WaterProjectSettings.Instance.WaterTempLayer)
            {
                gameObject.layer = WaterProjectSettings.Instance.WaterTempLayer;
            }
        }
        private void OnWaterIdChanged()
        {
            var rendererComponent = GetComponent<Renderer>();
            rendererComponent.material.SetFloat(_WaterIdName, 1 << _Water.WaterId);
        }
        #endregion Private Methods
    }
}