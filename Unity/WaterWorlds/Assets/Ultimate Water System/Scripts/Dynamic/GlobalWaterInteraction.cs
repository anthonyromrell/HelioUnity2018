namespace UltimateWater.Internal
{
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.Rendering;

    public class GlobalWaterInteraction : MonoBehaviour, ILocalDisplacementMaskRenderer
    {
        #region Public Variables
        public Vector2 WorldUnitsOffset
        {
            get { return _WorldUnitsOffset; }
            set { _WorldUnitsOffset = value; }
        }

        public Vector2 WorldUnitsSize
        {
            get { return _WorldUnitsSize; }
            set { _WorldUnitsSize = value; }
        }
        #endregion Public Variables

        #region Public Methods
        public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            float farClipPlane = overlays.Camera.CameraComponent.farClipPlane;

            Vector3 pos = overlays.Camera.transform.position;
            pos.y = overlays.DynamicWater.Water.transform.position.y;
            _InteractionMaskRenderer.transform.position = pos;
            _InteractionMaskRenderer.transform.localScale = new Vector3(farClipPlane, farClipPlane, farClipPlane);

            _InteractionMaskMaterial.SetVector("_OffsetScale", new Vector4(0.5f + _WorldUnitsOffset.x / _WorldUnitsSize.x, 0.5f + _WorldUnitsOffset.y / _WorldUnitsSize.y, 1.0f / _WorldUnitsSize.x, 1.0f / _WorldUnitsSize.y));

            commandBuffer.DrawMesh(_InteractionMaskRenderer.GetComponent<MeshFilter>().sharedMesh, _InteractionMaskRenderer.transform.localToWorldMatrix, _InteractionMaskMaterial, 0, 0);
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
        [HideInInspector] [SerializeField, FormerlySerializedAs("maskDisplayShader")] private Shader _MaskDisplayShader;

        [SerializeField, FormerlySerializedAs("intensityMask")]
        private Texture2D _IntensityMask;

        [SerializeField, FormerlySerializedAs("worldUnitsOffset")]
        private Vector2 _WorldUnitsOffset;

        [SerializeField, FormerlySerializedAs("worldUnitsSize")]
        private Vector2 _WorldUnitsSize;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            OnValidate();
            CreateMaskRenderer();

            gameObject.layer = WaterProjectSettings.Instance.WaterTempLayer;
        }

        private void OnEnable()
        {
            DynamicWater.AddRenderer(this);
        }

        private void OnDisable()
        {
            DynamicWater.RemoveRenderer(this);
        }

        private void OnValidate()
        {
            if (_MaskDisplayShader == null)
                _MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRenderSimple");
        }
        #endregion Unity Methods

        #region Private Variables
        private MeshRenderer _InteractionMaskRenderer;
        private Material _InteractionMaskMaterial;
        #endregion Private Variables

        #region Private Methods
        private void CreateMaskRenderer()
        {
            var mf = gameObject.AddComponent<MeshFilter>();
            mf.sharedMesh = Quads.BipolarXZ;

            _InteractionMaskMaterial = new Material(_MaskDisplayShader) { hideFlags = HideFlags.DontSave };
            _InteractionMaskMaterial.SetTexture("_MainTex", _IntensityMask);

            _InteractionMaskRenderer = gameObject.AddComponent<MeshRenderer>();
            _InteractionMaskRenderer.sharedMaterial = _InteractionMaskMaterial;
            _InteractionMaskRenderer.enabled = false;

            transform.localRotation = Quaternion.identity;
        }
        #endregion Private Methods
    }
}