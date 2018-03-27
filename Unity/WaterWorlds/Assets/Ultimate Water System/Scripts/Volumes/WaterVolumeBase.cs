namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Utils;

    #region Public Types
    public enum WaterVolumeRenderMode
    {
        None,
        Basic
    }
    #endregion Public Types

    [ExecuteInEditMode]
    public abstract class WaterVolumeBase : MonoBehaviour
    {
        #region Public Variables
        public Water Water
        {
            get { return _Water; }
        }

        public bool EnablePhysics
        {
            get { return _AffectPhysics; }
        }

        public WaterVolumeRenderMode RenderMode
        {
            get { return _RenderMode; }
        }

        public MeshRenderer[] VolumeRenderers { get; private set; }
        public MeshFilter[] VolumeFilters { get; private set; }

        protected virtual CullMode _CullMode
        {
            get { return CullMode.Back; }
        }
        #endregion Public Variables

        #region Public Methods

        public void AssignTo(Water water)
        {
            if (_Water == water || water == null)
            {
                return;
            }

            DisposeRenderers();
            Unregister(water);
            _Water = water;
            Register(water);

            if (_RenderMode != WaterVolumeRenderMode.None && Application.isPlaying)
            {
                CreateRenderers();
            }
        }

        public void EnableRenderers(bool forBorderRendering)
        {
            if (VolumeRenderers == null)
            {
                return;
            }

            bool enable = (!forBorderRendering /*|| _RenderMode == WaterVolumeRenderMode.Full*/) && _Water.enabled;
            for (int i = 0; i < VolumeRenderers.Length; ++i)
            {
                VolumeRenderers[i].enabled = enable;
            }
        }

        public void DisableRenderers()
        {
            if (VolumeRenderers == null)
            {
                return;
            }

            for (int i = 0; i < VolumeRenderers.Length; ++i)
            {
                VolumeRenderers[i].enabled = false;
            }
        }

        public bool IsPointInside(Vector3 point)
        {
            for (int i = 0; i < _Colliders.Length; ++i)
            {
                if (_Colliders[i].IsPointInside(point))
                {
                    return true;
                }
            }

            return false;
        }

        public static WaterVolumeBase GetWaterVolume<T>(Collider collider) where T : WaterVolumeBase
        {
            return GetWaterVolume(collider) as T;
        }
        public static WaterVolumeBase GetWaterVolume(Collider collider)
        {
            WaterVolumeBase volume;

            if (!_ColliderToVolumeCache.TryGetValue(collider, out volume))
            {
                volume = collider.GetComponent<WaterVolumeBase>();

                if (volume != null)
                {
                    _ColliderToVolumeCache[collider] = volume;
                }
                else
                {
                    // force null reference (Unity uses custom null)
                    // ReSharper disable once RedundantAssignment
                    _ColliderToVolumeCache[collider] = volume = null;
                }
            }

            return volume;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField]
        private Water _Water;

        [SerializeField]
        private WaterVolumeRenderMode _RenderMode = WaterVolumeRenderMode.Basic;

        [SerializeField]
        private bool _AffectPhysics = true;
        #endregion Inspector Variables

        #region Unity Methods
        protected void OnEnable()
        {
            _Colliders = GetComponents<Collider>();
            gameObject.layer = WaterProjectSettings.Instance.WaterCollidersLayer;

            Register(_Water);

            if (_RenderMode != WaterVolumeRenderMode.None && _Water != null && Application.isPlaying)
                CreateRenderers();
        }
        protected void OnDisable()
        {
            DisposeRenderers();
            Unregister(_Water);
        }

        private void Update()
        {
            if (VolumeRenderers == null)
            {
                return;
            }

            for (int i = 0; i < VolumeRenderers.Length; ++i)
            {
                VolumeRenderers[i].SetPropertyBlock(_Water.Renderer.PropertyBlock);
            }
        }

        protected void OnValidate()
        {
            _Colliders = GetComponents<Collider>();

            for (int i = 0; i < _Colliders.Length; ++i)
            {
                if (!_Colliders[i].isTrigger)
                {
                    _Colliders[i].isTrigger = true;
                }
            }

            if (_Water == null)
            {
                _Water = GetComponentInChildren<Water>();
            }
        }
        protected void Reset()
        {
            if (_Water == null)
            {
                _Water = Utilities.GetWaterReference();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Collider[] _Colliders;

        private static readonly Dictionary<Collider, WaterVolumeBase> _ColliderToVolumeCache = new Dictionary<Collider, WaterVolumeBase>();
        #endregion Private Variables

        #region Private Methods

        protected abstract void Register(Water water);
        protected abstract void Unregister(Water water);

        internal void SetLayer(int layer)
        {
            if (VolumeRenderers == null)
            {
                return;
            }

            for (int i = 0; i < VolumeRenderers.Length; ++i)
            {
                VolumeRenderers[i].gameObject.layer = layer;
            }
        }

        private void DisposeRenderers()
        {
            if (VolumeRenderers == null)
            {
                return;
            }

            for (int i = 0; i < VolumeRenderers.Length; ++i)
            {
                if (VolumeRenderers[i] != null)
                {
                    Destroy(VolumeRenderers[i].gameObject);
                }
            }

            VolumeRenderers = null;
            VolumeFilters = null;
        }

        protected virtual void CreateRenderers()
        {
            int numVolumes = _Colliders.Length;
            VolumeRenderers = new MeshRenderer[numVolumes];
            VolumeFilters = new MeshFilter[numVolumes];

            var material = _CullMode == CullMode.Back ? _Water.Materials.VolumeMaterial : _Water.Materials.VolumeBackMaterial;

            for (int i = 0; i < numVolumes; ++i)
            {
                var colliderComponent = _Colliders[i];

                GameObject rendererGo;

                BoxCollider boxCollider;
                MeshCollider meshCollider;
                SphereCollider sphereCollider;
                CapsuleCollider capsuleCollider;

                if ((boxCollider = colliderComponent as BoxCollider) != null)
                {
                    HandleBoxCollider(out rendererGo, boxCollider);
                }
                else if ((sphereCollider = colliderComponent as SphereCollider) != null)
                {
                    HandleSphereCollider(out rendererGo, sphereCollider);
                }
                else if ((meshCollider = colliderComponent as MeshCollider) != null)
                {
                    HandleMeshCollider(out rendererGo, meshCollider);
                }
                else if ((capsuleCollider = colliderComponent as CapsuleCollider) != null)
                {
                    HandleCapsuleCollider(out rendererGo, capsuleCollider);
                }
                else
                {
                    throw new System.InvalidOperationException("Unsupported collider type.");
                }

                rendererGo.hideFlags = HideFlags.DontSave;
                rendererGo.name = "Volume Renderer";
                rendererGo.layer = WaterProjectSettings.Instance.WaterLayer;
                rendererGo.transform.SetParent(transform, false);

                Destroy(rendererGo.GetComponent<Collider>());

                var rendererComponent = rendererGo.GetComponent<MeshRenderer>();
                rendererComponent.sharedMaterial = material;
                rendererComponent.shadowCastingMode = ShadowCastingMode.Off;
                rendererComponent.receiveShadows = false;
#if UNITY_5_4_OR_NEWER
                rendererComponent.lightProbeUsage = LightProbeUsage.Off;
#else
                renderer.useLightProbes = false;
#endif
                rendererComponent.enabled = true;
                rendererComponent.SetPropertyBlock(_Water.Renderer.PropertyBlock);

                VolumeRenderers[i] = rendererComponent;
                VolumeFilters[i] = rendererComponent.GetComponent<MeshFilter>();
            }
        }

        private static void HandleBoxCollider(out GameObject obj, BoxCollider boxCollider)
        {
            obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = boxCollider.size;
            obj.transform.localPosition = boxCollider.center;
        }

        private void HandleMeshCollider(out GameObject obj, MeshCollider meshCollider)
        {
            var sharedMesh = meshCollider.sharedMesh;
            if (sharedMesh == null)
            {
                throw new System.InvalidOperationException("MeshCollider used to mask water doesn't have a mesh assigned.");
            }

            obj = new GameObject { hideFlags = HideFlags.DontSave };

            var mf = obj.AddComponent<MeshFilter>();
            mf.sharedMesh = sharedMesh;

            obj.AddComponent<MeshRenderer>();
        }

        private static void HandleSphereCollider(out GameObject obj, SphereCollider sphereCollider)
        {
            float d = sphereCollider.radius * 2;

            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.localScale = new Vector3(d, d, d);
            obj.transform.localPosition = sphereCollider.center;
        }

        private static void HandleCapsuleCollider(out GameObject obj, CapsuleCollider capsuleCollider)
        {
            float height = capsuleCollider.height * 0.5f;
            float radius = capsuleCollider.radius * 2.0f;

            obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.transform.localPosition = capsuleCollider.center;

            switch (capsuleCollider.direction)
            {
                case 0:
                {
                    obj.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                    obj.transform.localScale = new Vector3(height, radius, radius);
                    break;
                }

                case 1:
                {
                    obj.transform.localScale = new Vector3(radius, height, radius);
                    break;
                }

                case 2:
                {
                    obj.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                    obj.transform.localScale = new Vector3(radius, radius, height);
                    break;
                }
            }
        }
        #endregion Private Methods

        #region Validation
        [InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
        [SerializeField]
        private string _Validation;
        protected string Validation()
        {
            string info = string.Empty;
            if (_Water == null)
            {
                info += "warning: assign Water reference";
            }

            return info;
        }
        #endregion Validation
    }
}