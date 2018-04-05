namespace UltimateWater
{
    using UnityEngine.Rendering;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// Inflicts force on Water Simulation Areas using attached Renderer object.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    [AddComponentMenu("Ultimate Water/Dynamic/Water Interactive")]
    public sealed class WaterInteractive : MonoBehaviour, IWavesInteractive
    {
        #region Public Variables
        [Tooltip("How much velocity modifies wave amplitude")]
        public float Multiplier = 1.0f;
        #endregion Public Variables

        #region Public Methods
        public void Render(CommandBuffer commandBuffer)
        {
            if (!_Renderer.enabled) { return; }

            commandBuffer.DrawRenderer(_Renderer, _Material);
        }

        public void Enable()
        {
        }  // unused interface method
        public void Disable()
        {
        } // unused interface method
        #endregion Public Methods

        #region Private Variables
        private Matrix4x4 _Previous;

        private Renderer _Renderer;
        private Material _Material;
        #endregion Private Variables

        #region Unity Messages
        private void Awake()
        {
            _Material = ShaderUtility.Instance.CreateMaterial(ShaderList.Velocity);
            if (_Material.IsNullReference(this)) return;

            _Renderer = GetComponent<Renderer>();
            if (_Renderer.IsNullReference(this)) return;

            _Previous = transform.localToWorldMatrix;
        }

        private void OnEnable()
        {
            DynamicWater.AddRenderer(this);
        }
        private void OnDisable()
        {
            DynamicWater.RemoveRenderer(this);
        }

        private void FixedUpdate()
        {
            var current = transform.localToWorldMatrix;

            _Material.SetMatrix("_PreviousWorld", _Previous);
            _Material.SetMatrix("_CurrentWorld", current);
            _Material.SetFloat("_Data", Multiplier / Time.fixedDeltaTime);

            _Previous = current;
        }
        #endregion Unity Messages
    }
}