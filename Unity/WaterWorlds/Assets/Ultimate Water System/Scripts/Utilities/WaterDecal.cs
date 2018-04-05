using UltimateWater.Utils;

namespace UltimateWater
{
    using UnityEngine;
    using Internal;
    using UnityEngine.Rendering;

    /// <summary>
    /// Local water effects
    /// </summary>
    public class WaterDecal : MonoBehaviour,
        ILocalDisplacementRenderer, ILocalFoamRenderer, ILocalDiffuseRenderer
    {
        #region Public Variables
        public Material DisplacementMaterial
        {
            get { return _DisplacementMaterial; }
            set
            {
                var previous = _DisplacementMaterial;
                _DisplacementMaterial = value;

                // added displacement support
                if (previous == null && _DisplacementMaterial != null)
                {
                    DynamicWater.AddRenderer<ILocalDisplacementRenderer>(this);
                }

                // removed displacement support
                if (previous != null && _DisplacementMaterial == null)
                {
                    DynamicWater.RemoveRenderer<ILocalDisplacementRenderer>(this);
                }
            }
        }
        public Material FoamMaterial
        {
            get { return _FoamMaterial; }
            set
            {
                var previous = _FoamMaterial;
                _FoamMaterial = value;

                // added foam support
                if (previous == null && _FoamMaterial != null)
                {
                    DynamicWater.AddRenderer<ILocalFoamRenderer>(this);
                }

                // removed foam support
                if (previous != null && _FoamMaterial == null)
                {
                    DynamicWater.RemoveRenderer<ILocalFoamRenderer>(this);
                }
            }
        }

        public bool RenderDiffuse
        {
            get { return _RenderDiffuse; }
            set
            {
                var previous = _RenderDiffuse;
                _RenderDiffuse = value;

                // added foam support
                if (!previous && _RenderDiffuse)
                {
                    DynamicWater.AddRenderer<ILocalFoamRenderer>(this);
                }

                // removed foam support
                if (previous && !_RenderDiffuse)
                {
                    DynamicWater.RemoveRenderer<ILocalFoamRenderer>(this);
                }
            }
        }
        #endregion Public Variables

        #region Inspector Variables
        [Tooltip("Used for rendering water displacements")]
        [SerializeField]
        private Material _DisplacementMaterial;
        [Tooltip("Used for rendering foam")]
        [SerializeField]
        private Material _FoamMaterial;

        [SerializeField] private bool _RenderDiffuse;

        [SerializeField] private bool _UseChildrenRenderers;
        #endregion Inspector Variables

        #region Public Methods
        public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            for (int i = 0; i < _Renderers.Length; ++i)
            {
                commandBuffer.DrawRenderer(_Renderers[i], _DisplacementMaterial);
            }
        }
        public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            for (int i = 0; i < _Renderers.Length; ++i)
            {
                commandBuffer.DrawRenderer(_Renderers[i], _FoamMaterial);
            }
        }
        public void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            for (int i = 0; i < _Renderers.Length; ++i)
            {
                commandBuffer.DrawRenderer(_Renderers[i], _Renderers[i].material);
            }
        }

        public void Enable()
        {
            for (int i = 0; i < _Renderers.Length; ++i)
            {
                _Renderers[i].enabled = true;
            }
        }
        public void Disable()
        {
            for (int i = 0; i < _Renderers.Length; ++i)
            {
                _Renderers[i].enabled = false;
            }
        }
        #endregion Public Methods

        #region Private Variables
        private Renderer[] _Renderers;
        #endregion Private Variables

        #region Unity Messages
        private void Awake()
        {
            _Renderers = _UseChildrenRenderers ? GetComponentsInChildren<Renderer>() : new[] { GetComponent<Renderer>() };

            for (int i = 0; i < _Renderers.Length; ++i)
            {
                _Renderers[i].enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_DisplacementMaterial != null)
            {
                DynamicWater.AddRenderer((ILocalDisplacementRenderer)this);
            }
            if (_FoamMaterial != null)
            {
                DynamicWater.AddRenderer((ILocalFoamRenderer)this);
            }
            if (_RenderDiffuse)
            {
                DynamicWater.AddRenderer((ILocalDiffuseRenderer)this);
            }
        }
        private void OnDisable()
        {
            if (_DisplacementMaterial != null)
            {
                DynamicWater.RemoveRenderer((ILocalDisplacementRenderer)this);
            }
            if (_FoamMaterial != null)
            {
                DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
            }
            if (_RenderDiffuse)
            {
                DynamicWater.RemoveRenderer((ILocalDiffuseRenderer)this);
            }
        }

        private void OnValidate()
        {
            // force-call setters to disable/enable rendering
            DisplacementMaterial = _DisplacementMaterial;
            FoamMaterial = _FoamMaterial;
            RenderDiffuse = _RenderDiffuse;
        }

        private void OnDrawGizmos()
        {
            var filter = GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null)
            {
                return;
            }

            Gizmos.color = new Color(0.5f, 0.2f, 1.0f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireMesh(filter.sharedMesh);
        }

        #endregion Unity Messages

        #region Validation
        [InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Error)]
        [SerializeField]
        private int _Validation;

        // ReSharper disable once UnusedMember.Local
        private string Validation()
        {
            if (_DisplacementMaterial == null && _FoamMaterial == null && !_RenderDiffuse)
            {
                return "Warnings:\n- Set at least one material";
            }
            return string.Empty;
        }
        #endregion Validation
    }

    // todo : check shader in Displacement/Foam Material setters
}