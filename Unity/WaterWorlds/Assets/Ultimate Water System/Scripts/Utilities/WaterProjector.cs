using UltimateWater.Utils;

namespace UltimateWater
{
    using UnityEngine;
    using Internal;
    using UnityEngine.Rendering;

#if UNITY_EDITOR

    using UnityEditor;

#endif

    /// <summary>
    /// Projects renderers to water surface
    /// </summary>
    public class WaterProjector : MonoBehaviour, ILocalDiffuseRenderer, ILocalDisplacementRenderer
    {
        #region Public Types
        public enum Type
        {
            Diffuse,
            Displacement,
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField] private Type _Type = Type.Diffuse;
        [SerializeField] private Material _Displacement;
        [SerializeField] private bool _UseChildrenRenderers;
        #endregion Inspector Variables

        #region Public Methods
        public void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            if (_Type != Type.Diffuse)
            {
                return;
            }

            for (int i = 0; i < _Renderers.Length; ++i)
            {
                commandBuffer.DrawRenderer(_Renderers[i], _Renderers[i].sharedMaterial);
            }
        }
        public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            if (_Type != Type.Displacement || _Displacement == null)
            {
                return;
            }

            for (int i = 0; i < _Renderers.Length; ++i)
            {
                commandBuffer.DrawRenderer(_Renderers[i], _Displacement);
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
            if (_UseChildrenRenderers)
            {
                _Renderers = GetComponentsInChildren<Renderer>();
            }
            else
            {
                var rendererComponent = GetComponent<Renderer>();
                if (rendererComponent != null)
                {
                    _Renderers = new[] { rendererComponent };
                }
            }

            if (_Renderers == null)
            {
                Debug.LogError("[WaterProjector] : no renderers found");
                enabled = false;
                return;
            }

            for (int i = 0; i < _Renderers.Length; ++i)
            {
                _Renderers[i].enabled = false;
            }

            if (_Type == Type.Displacement && _Displacement == null)
            {
                _Displacement = Resources.Load<Material>("Materials/Overlay (Displacements)");
            }
        }

        private void OnEnable()
        {
            Unregister();
            Register();
        }
        private void OnDisable()
        {
            Unregister();
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

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                if (_Type == Type.Displacement && _Displacement == null)
                {
                    _Displacement = Resources.Load<Material>("Materials/Overlay (Displacements)");
                }

                return;
            }

            Unregister();
            Register();
        }
        #endregion Unity Messages

        #region Private Methods
        private void Register()
        {
            DynamicWater.AddRenderer(this);
        }
        private void Unregister()
        {
            DynamicWater.RemoveRenderer(this);
        }
        #endregion Private Methods

        #region Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/Water/Projector", false, 10)]
        private static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Plane);

            var collider = obj.GetComponent<MeshCollider>();
            if (collider != null)
            {
                collider.Destroy();
            }

            obj.AddComponent<WaterProjector>();

            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }
#endif
        #endregion Editor

        #region Validation
        [InspectorWarning("Warning", InspectorWarningAttribute.InfoType.Error)]
        [SerializeField]
        private string _Warning;

        // It's used by Editor, by reflecting on it's name, provided in [InspectorWarning] parameter
        // ReSharper disable once UnusedMember.Local
        private string Warning()
        {
            bool valid = true;
            if (_UseChildrenRenderers)
            {
                var renderers = GetComponentsInChildren<Renderer>();
                if (renderers == null || renderers.Length == 0)
                {
                    valid = false;
                }
            }
            else
            {
                if (GetComponent<Renderer>() == null)
                {
                    valid = false;
                }
            }

            return valid ? string.Empty : "error: no renderers found,\nadd renderer to GameObject or its children";
        }
        #endregion Validation
    }
}