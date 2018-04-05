namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Serialization;
    using Internal;

    [RequireComponent(typeof(Renderer))]
    public class WaterSurfaceOverlayRenderer : MonoBehaviour, ILocalDisplacementRenderer, ILocalDisplacementMaskRenderer, ILocalFoamRenderer
    {
        #region Public Variables
        public Material DisplacementAndNormalMaterial
        {
            get { return _DisplacementAndNormalMaterial; }
        }

        public Material DisplacementMaskMaterial
        {
            get { return _DisplacementMaskMaterial; }
        }

        public Material FoamMaterial
        {
            get { return _FoamMaterial; }
        }

        public Renderer Renderer
        {
            get { return _RendererComponent; }
        }
        #endregion Public Variables

        #region Public Methods
        public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            commandBuffer.DrawRenderer(_RendererComponent, _DisplacementAndNormalMaterial);
        }

        public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            commandBuffer.DrawRenderer(_RendererComponent, _DisplacementMaskMaterial);
        }

        public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            commandBuffer.DrawRenderer(_RendererComponent, _FoamMaterial);
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("displacementAndNormalMaterial")]
        private Material _DisplacementAndNormalMaterial;
        [SerializeField, FormerlySerializedAs("displacementMaskMaterial")]
        private Material _DisplacementMaskMaterial;
        [SerializeField, FormerlySerializedAs("foamMaterial")]
        private Material _FoamMaterial;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _RendererComponent = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            if (_DisplacementAndNormalMaterial != null)
                DynamicWater.AddRenderer((ILocalDisplacementRenderer)this);

            if (_DisplacementMaskMaterial != null)
                DynamicWater.AddRenderer((ILocalDisplacementMaskRenderer)this);

            if (_FoamMaterial != null)
                DynamicWater.AddRenderer((ILocalFoamRenderer)this);
        }

        private void OnDisable()
        {
            DynamicWater.RemoveRenderer((ILocalDisplacementRenderer)this);
            DynamicWater.RemoveRenderer((ILocalDisplacementMaskRenderer)this);
            DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
        }
        #endregion Unity Methods

        #region Private Variables
        private Renderer _RendererComponent;
        #endregion Private Variables
    }
}