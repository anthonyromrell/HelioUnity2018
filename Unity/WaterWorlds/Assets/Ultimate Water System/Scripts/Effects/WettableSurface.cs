using UnityEngine.Serialization;

namespace UltimateWater
{
    using UnityEngine;
    using System.Linq;

    public sealed class WettableSurface : MonoBehaviour
    {
        #region Public Types
        public enum Mode
        {
            TextureSpace,
            NearCamera
        }
        #endregion Public Types

        #region Public Variables
        public WaterCamera MainCamera
        {
            get { return _MainCamera; }
            set { _MainCamera = value; }
        }
        #endregion Public Variables

        #region Inspector Variables
        [HideInInspector]
        [SerializeField, FormerlySerializedAs("wettableUtilShader")]
        private Shader _WettableUtilShader;

        [HideInInspector]
        [SerializeField, FormerlySerializedAs("wettableUtilNearShader")]
        private Shader _WettableUtilNearShader;

        [SerializeField, FormerlySerializedAs("water")]
        private Water _Water;

        [Tooltip("Surface wetting near this camera will be more precise.")]
        [SerializeField, FormerlySerializedAs("mainCamera")]
        private WaterCamera _MainCamera;

        [Tooltip("Texture space is good for small objects, especially convex ones.\nNear camera mode is better for terrains and big meshes that are static and don't have geometry at the bottom.")]
        [SerializeField, FormerlySerializedAs("mode")]
        private Mode _Mode;

        [SerializeField, FormerlySerializedAs("resolution")]
        private int _Resolution = 512;

        [Header("Direct references (Optional)")]
        [SerializeField, FormerlySerializedAs("meshRenderers")]
        private MeshRenderer[] _MeshRenderers;

        [SerializeField, FormerlySerializedAs("terrains")]
        private Terrain[] _Terrains;
        #endregion Inspector Variables

        #region Unity Methods
        private void LateUpdate()
        {
            if (!_MainCamera.enabled)
                return;

            switch (_Mode)
            {
                case Mode.TextureSpace:
                    {
                        Graphics.SetRenderTarget(_WetnessMapA);
                        _WettableUtilMaterial.SetPass(0);
                        _WettableUtilMaterial.SetTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
                        _WettableUtilMaterial.SetVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);

                        for (int i = 0; i < _MeshFilters.Length; ++i)
                            Graphics.DrawMeshNow(_MeshFilters[i].sharedMesh, _MeshFilters[i].transform.localToWorldMatrix);

                        if (_Terrains.Length != 0)
                        {
                            int waterTempLayer = WaterProjectSettings.Instance.WaterTempLayer;

                            for (int i = 0; i < _Terrains.Length; ++i)
                            {
                                var go = _Terrains[i].gameObject;
                                _TerrainLayers[i] = go.layer;
                                go.layer = waterTempLayer;
                            }

                            Shader.SetGlobalTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
                            Shader.SetGlobalVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);

                            var wettingCamera = GetWettingCamera();
                            wettingCamera.transform.position = _Terrains[0].transform.position + _Terrains[0].terrainData.size * 0.5f;
                            wettingCamera.RenderWithShader(_WettableUtilShader, "CustomType");

                            for (int i = 0; i < _Terrains.Length; ++i)
                                _Terrains[i].gameObject.layer = _TerrainLayers[i];
                        }

                        break;
                    }

                case Mode.NearCamera:
                    {
                        int waterTempLayer = WaterProjectSettings.Instance.WaterTempLayer;

                        for (int i = 0; i < _Terrains.Length; ++i)
                        {
                            _TerrainDrawTrees[i] = _Terrains[i].drawTreesAndFoliage;
                            _Terrains[i].drawTreesAndFoliage = false;

                            var go = _Terrains[i].gameObject;
                            _TerrainLayers[i] = go.layer;
                            go.layer = waterTempLayer;
                        }

                        Shader.SetGlobalTexture("_WetnessMapPrevious", _WetnessMapB);
                        Shader.SetGlobalTexture("_TotalDisplacementMap", _Water.DynamicWater.GetCameraOverlaysData(_MainCamera.GetComponent<Camera>()).GetTotalDisplacementMap());
                        Shader.SetGlobalVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);

                        var localMapsRectPrevious = _MainCamera.LocalMapsRectPrevious;
                        Shader.SetGlobalVector("_LocalMapsCoordsPrevious", new Vector4(-localMapsRectPrevious.xMin / localMapsRectPrevious.width, -localMapsRectPrevious.yMin / localMapsRectPrevious.width, 1.0f / localMapsRectPrevious.width, localMapsRectPrevious.width));

                        Rect localMapsRect = _MainCamera.LocalMapsRect;
                        var wettingCamera = GetWettingCamera();
                        wettingCamera.transform.position = new Vector3(localMapsRect.center.x, _Terrains[0].transform.position.y + _Terrains[0].terrainData.size.y + 1.0f, localMapsRect.center.y);
                        wettingCamera.orthographicSize = localMapsRect.width * 0.5f;
                        wettingCamera.nearClipPlane = 1.0f;
                        wettingCamera.farClipPlane = _Terrains[0].terrainData.size.y * 2.0f;
                        wettingCamera.targetTexture = _WetnessMapA;
                        wettingCamera.clearFlags = CameraClearFlags.Color;
                        wettingCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                        wettingCamera.RenderWithShader(_WettableUtilNearShader, "CustomType");

                        for (int i = 0; i < _Materials.Length; ++i)
                            _Materials[i].SetTexture("_WetnessMap", _WetnessMapA);

                        for (int i = 0; i < _Terrains.Length; ++i)
                        {
                            _Terrains[i].drawTreesAndFoliage = _TerrainDrawTrees[i];
                            _Terrains[i].gameObject.layer = _TerrainLayers[i];
                        }

                        var t = _WetnessMapB;
                        _WetnessMapB = _WetnessMapA;
                        _WetnessMapA = t;

                        for (int i = 0; i < _Materials.Length; ++i)
                            _Materials[i].SetVector("_LocalMapsCoords", _MainCamera.LocalMapsShaderCoords);

                        break;
                    }
            }
        }
        private void OnValidate()
        {
            if (_WettableUtilShader == null)
                _WettableUtilShader = Shader.Find("UltimateWater/Utility/Wetness Update");

            if (_WettableUtilNearShader == null)
                _WettableUtilNearShader = Shader.Find("UltimateWater/Utility/Wetness Update (Near Camera)");

            if (_MeshRenderers == null || _MeshRenderers.Length == 0)
                _MeshRenderers = GetComponentsInChildren<MeshRenderer>(true);

            if (_Terrains == null || _Terrains.Length == 0)
                _Terrains = GetComponentsInChildren<Terrain>(true);

            var materials = _Materials;

            if (!Application.isPlaying)
            {
                if (materials == null)
                {
                    materials = _MeshRenderers
                        .SelectMany(mr => mr.sharedMaterials)
                        .Concat(_Terrains.Select(t => t.materialTemplate))
                        .ToArray();
                }

                if (_Mode == Mode.NearCamera)
                {
                    for (int i = 0; i < materials.Length; ++i)
                        materials[i].EnableKeyword("_WET_NEAR_CAMERA");
                }
                else
                {
                    for (int i = 0; i < materials.Length; ++i)
                        materials[i].DisableKeyword("_WET_NEAR_CAMERA");
                }
            }
        }
        private void Awake()
        {
            if (_Water == null || _Water.DynamicWater == null)
            {
                enabled = false;
                return;
            }

            OnValidate();

            _TerrainLayers = new int[_Terrains.Length];
            _TerrainDrawTrees = new bool[_Terrains.Length];
            _MeshFilters = new MeshFilter[_MeshRenderers.Length];

            for (int i = 0; i < _MeshFilters.Length; ++i)
                _MeshFilters[i] = _MeshRenderers[i].GetComponent<MeshFilter>();

            _WetnessMapA = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
            {
                name = "[UWS] WettableSurface - Wetness Map",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear
            };

            if (_Mode == Mode.NearCamera)
            {
                _WetnessMapB = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] WettableSurface - Wetness Map",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Bilinear
                };
            }

            var originalMaterials = _MeshRenderers
                .SelectMany(mr => mr.sharedMaterials)
                .Concat(_Terrains.Select(t => t.materialTemplate))
                .Distinct()
                .ToArray();

            _Materials = InstantiateMaterials(originalMaterials);

            OnValidate();

            _WettableUtilMaterial = new Material(_Mode == Mode.TextureSpace ? _WettableUtilShader : _WettableUtilNearShader)
            {
                hideFlags = HideFlags.DontSave
            };
        }
        #endregion Unity Methods

        #region Private Variables
        private MeshFilter[] _MeshFilters;
        private Material _WettableUtilMaterial;
        private RenderTexture _WetnessMapA;
        private RenderTexture _WetnessMapB;
        private Camera _WettingCamera;
        private Material[] _Materials;
        private bool[] _TerrainDrawTrees;
        private int[] _TerrainLayers;
        #endregion Private Variables

        #region Private Methods
        private Material[] InstantiateMaterials(Material[] materials)
        {
            var instantiatedMaterials = new Material[materials.Length];

            for (int i = 0; i < instantiatedMaterials.Length; ++i)
                instantiatedMaterials[i] = Instantiate(materials[i]);

            for (int i = 0; i < _MeshRenderers.Length; ++i)
            {
                var mr = _MeshRenderers[i];
                var sharedMaterials = mr.sharedMaterials;

                for (int ii = 0; ii < sharedMaterials.Length; ++ii)
                {
                    int index = System.Array.IndexOf(materials, sharedMaterials[ii]);
                    sharedMaterials[ii] = instantiatedMaterials[index];
                }

                mr.sharedMaterials = sharedMaterials;
            }

            for (int i = 0; i < _Terrains.Length; ++i)
            {
                var terrain = _Terrains[i];
                int index = System.Array.IndexOf(materials, terrain.materialTemplate);
                terrain.materialTemplate = instantiatedMaterials[index];
            }

            for (int i = 0; i < instantiatedMaterials.Length; ++i)
                instantiatedMaterials[i].SetTexture("_WetnessMap", _WetnessMapA);

            return instantiatedMaterials;
        }

        private Camera GetWettingCamera()
        {
            if (_WettingCamera == null)
            {
                var wettingCameraGo = new GameObject("Wetting Camera") { hideFlags = HideFlags.DontSave };
                wettingCameraGo.transform.position = new Vector3(0, 100000, 0);
                wettingCameraGo.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

                _WettingCamera = wettingCameraGo.AddComponent<Camera>();
                _WettingCamera.enabled = false;
                _WettingCamera.orthographic = true;
                _WettingCamera.orthographicSize = 1000000;
                _WettingCamera.nearClipPlane = 10;
                _WettingCamera.farClipPlane = 1000000;
                _WettingCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
                _WettingCamera.renderingPath = RenderingPath.VertexLit;
                _WettingCamera.clearFlags = CameraClearFlags.Nothing;
                _WettingCamera.targetTexture = _WetnessMapA;
            }

            return _WettingCamera;
        }
        #endregion Private Methods
    }
}