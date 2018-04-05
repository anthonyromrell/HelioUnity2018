using UnityEngine.Serialization;

namespace UltimateWater
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Internal;

    public sealed class StaticWaterInteraction : MonoBehaviour, IWaterShore, ILocalDisplacementMaskRenderer
    {
        #region Public Types
        public enum UnderwaterAreasMode
        {
            Generate,
            UseExisting
        }
        #endregion Public Types

        #region Public Variables
        [FormerlySerializedAs("staticWaterInteractions")]
        public static List<StaticWaterInteraction> StaticWaterInteractions = new List<StaticWaterInteraction>();

        public Bounds Bounds
        {
            get { return _TotalBounds; }
        }

        public RenderTexture IntensityMask
        {
            get
            {
                if (_IntensityMask == null)
                {
                    _IntensityMask = new RenderTexture(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat,
                        RenderTextureReadWrite.Linear)
                    {
                        name = "[UWS] StaticWaterInteraction - IntensityMask",
                        hideFlags = HideFlags.DontSave,
                        filterMode = FilterMode.Bilinear,
                    };
                }

                return _IntensityMask;
            }
        }

        public Renderer InteractionRenderer
        {
            get { return _InteractionMaskRenderer; }
        }
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Call it after changes are made to the associated renderers to update internal data.
        /// </summary>
        [ContextMenu("Refresh Intensity Mask (Runtime Only)")]
        public void Refresh()
        {
            if (_InteractionMaskRenderer == null)
                return;			// it hadn't started, it will refresh anyway

            RenderShorelineIntensityMask();

            Vector3 maskLocalScale = _TotalBounds.size * 0.5f;
            maskLocalScale.x /= transform.localScale.x;
            maskLocalScale.y /= transform.localScale.y;
            maskLocalScale.z /= transform.localScale.z;
            _InteractionMaskRenderer.gameObject.transform.localScale = maskLocalScale;
        }
        public void SetUniformDepth(float depth, float boundsSize)
        {
            OnValidate();
            OnDestroy();

            _TotalBounds = new Bounds(transform.position + new Vector3(boundsSize * 0.5f, 0.0f, boundsSize * 0.5f), new Vector3(boundsSize, 1.0f, boundsSize));

            var intensityMask = IntensityMask;

            float f = Mathf.Sqrt((float)Math.Tanh(depth * -0.01));
            Graphics.SetRenderTarget(intensityMask);
            GL.Clear(true, true, new Color(f, f, f, f));
            Graphics.SetRenderTarget(null);

            if (_InteractionMaskRenderer == null)
                CreateMaskRenderer();
        }
        public float GetDepthAt(float x, float z)
        {
            x = (x + _OffsetX) * _ScaleX;
            z = (z + _OffsetZ) * _ScaleZ;

            int ix = (int)x; if (ix > x) --ix;       // inlined FastMath.FloorToInt(x);
            int iz = (int)z; if (iz > z) --iz;       // inlined FastMath.FloorToInt(z);

            if (ix >= _Width || ix < 0 || iz >= _Height || iz < 0)
                return 100.0f;

            x -= ix;
            z -= iz;

            int index = iz * _Width + ix;

            float a = _HeightMapData[index] * (1.0f - x) + _HeightMapData[index + 1] * x;
            float b = _HeightMapData[index + _Width] * (1.0f - x) + _HeightMapData[index + _Width + 1] * x;

            return a * (1.0f - z) + b * z;
        }
        public static float GetTotalDepthAt(float x, float z)
        {
            float minDepth = 100.0f;
            int numInteractions = StaticWaterInteractions.Count;

            for (int i = 0; i < numInteractions; ++i)
            {
                float depth = StaticWaterInteractions[i].GetDepthAt(x, z);

                if (minDepth > depth)
                    minDepth = depth;
            }

            return minDepth;
        }
        public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
            Vector3 pos = _InteractionMaskRenderer.transform.position;
            pos.y = overlays.DynamicWater.Water.transform.position.y;
            _InteractionMaskRenderer.transform.position = pos;

            commandBuffer.DrawMesh(_InteractionMaskRenderer.GetComponent<MeshFilter>().sharedMesh, _InteractionMaskRenderer.transform.localToWorldMatrix, _InteractionMaskMaterial, 0, 0);
        }
        public void Enable()
        {
            throw new NotImplementedException();
        }
        public void Disable()
        {
            throw new NotImplementedException();
        }
        public static StaticWaterInteraction AttachTo(GameObject target, float shoreSmoothness, bool hasBottomFaces, UnderwaterAreasMode underwaterAreasMode, int mapResolution, float waveDampingThreshold = 4.0f, float depthScale = 1.0f)
        {
            var instance = target.AddComponent<StaticWaterInteraction>();
            instance._ShoreSmoothness = shoreSmoothness;
            instance._HasBottomFaces = hasBottomFaces;
            instance._UnderwaterAreasMode = underwaterAreasMode;
            instance._MapResolution = mapResolution;
            instance._WaveDampingThreshold = waveDampingThreshold;
            instance._DepthScale = depthScale;
            return instance;
        }
        #endregion Public Methods

        #region Inspector Variables
        [Tooltip("Specifies a distance from the shore over which a water gets one meter deeper (value of 50 means that water has a depth of 1m at a distance of 50m from the shore).")]
        [Range(0.001f, 80.0f)]
        [SerializeField, FormerlySerializedAs("shoreSmoothness")]
        private float _ShoreSmoothness = 50.0f;

        [Tooltip("If set to true, geometry that floats above water is correctly ignored.\n\nUse for objects that are closed and have faces at the bottom like basic primitives and most custom meshes, but not terrain.")]
        [SerializeField, FormerlySerializedAs("hasBottomFaces")]
        private bool _HasBottomFaces;

        [SerializeField, FormerlySerializedAs("underwaterAreasMode")]
        private UnderwaterAreasMode _UnderwaterAreasMode;

        [Resolution(1024, 128, 256, 512, 1024, 2048)]
        [SerializeField, FormerlySerializedAs("mapResolution")]
        private int _MapResolution = 1024;

        [Tooltip("All waves bigger than this (in scene units) will be dampened near the shore.")]
        [SerializeField, FormerlySerializedAs("waveDampingThreshold")]
        private float _WaveDampingThreshold = 4.0f;

        [SerializeField, FormerlySerializedAs("depthScale")]
        private float _DepthScale = 1.0f;

        [HideInInspector] [SerializeField, FormerlySerializedAs("maskGenerateShader")] private Shader _MaskGenerateShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("maskDisplayShader")] private Shader _MaskDisplayShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("heightMapperShader")] private Shader _HeightMapperShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("heightMapperShaderAlt")] private Shader _HeightMapperShaderAlt;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnValidate()
        {
            if (_MaskGenerateShader == null)
                _MaskGenerateShader = Shader.Find("UltimateWater/Utility/ShorelineMaskGenerate");

            if (_MaskDisplayShader == null)
                _MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRender");

            if (_HeightMapperShader == null)
                _HeightMapperShader = Shader.Find("UltimateWater/Utility/HeightMapper");

            if (_HeightMapperShaderAlt == null)
                _HeightMapperShaderAlt = Shader.Find("UltimateWater/Utility/HeightMapperAlt");

            if (_InteractionMaskMaterial != null)
                _InteractionMaskMaterial.SetFloat("_WaveDampingThreshold", _WaveDampingThreshold);

            if (_InteractionMaskMaterial != null)
            {
                _InteractionMaskMaterial.SetFloat("_WaveDampingThreshold", _WaveDampingThreshold);
            }
        }
        private void OnEnable()
        {
            StaticWaterInteractions.Add(this);
            DynamicWater.AddRenderer((ILocalDisplacementMaskRenderer)this);
        }
        private void OnDisable()
        {
            DynamicWater.RemoveRenderer((ILocalDisplacementMaskRenderer)this);
            StaticWaterInteractions.Remove(this);
        }
        private void OnDestroy()
        {
            if (_IntensityMask != null)
            {
                _IntensityMask.Destroy();
                _IntensityMask = null;
            }

            if (_InteractionMaskMaterial != null)
            {
                _InteractionMaskMaterial.Destroy();
                _InteractionMaskMaterial = null;
            }

            if (_InteractionMaskRenderer != null)
            {
                _InteractionMaskRenderer.Destroy();
                _InteractionMaskRenderer = null;
            }
        }
        private void Start()
        {
            OnValidate();

            if (_IntensityMask == null)
                RenderShorelineIntensityMask();

            if (_InteractionMaskRenderer == null)
                CreateMaskRenderer();
        }
        #endregion Unity Methods

        #region Private Variables
        private GameObject[] _GameObjects;
        private Terrain[] _Terrains;
        private int[] _OriginalRendererLayers;
        private float[] _OriginalTerrainPixelErrors;
        private bool[] _OriginalDrawTrees;

        private RenderTexture _IntensityMask;
        private MeshRenderer _InteractionMaskRenderer;
        private Material _InteractionMaskMaterial;
        private Bounds _Bounds;
        private Bounds _TotalBounds;

        private float[] _HeightMapData;
        private float _OffsetX, _OffsetZ, _ScaleX, _ScaleZ;
        private int _Width, _Height;
        #endregion Private Variables

        #region Private Methods
        private void RenderShorelineIntensityMask()
        {
            try
            {
                PrepareRenderers();

                float shoreSteepness = 1.0f / _ShoreSmoothness;
                _TotalBounds = _Bounds;

                if (_UnderwaterAreasMode == UnderwaterAreasMode.Generate)
                {
                    float distanceToFullSeaInMeters = 80.0f / shoreSteepness;
                    _TotalBounds.Expand(new Vector3(distanceToFullSeaInMeters, 0.0f, distanceToFullSeaInMeters));
                }

                float heightOffset = transform.position.y;
                var heightMap = RenderHeightMap(_MapResolution, _MapResolution);

                var intensityMask = IntensityMask;

                _OffsetX = -_TotalBounds.min.x;
                _OffsetZ = -_TotalBounds.min.z;
                _ScaleX = _MapResolution / _TotalBounds.size.x;
                _ScaleZ = _MapResolution / _TotalBounds.size.z;
                _Width = _MapResolution;
                _Height = _MapResolution;

                var temp1 = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                var temp2 = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);

                var material = new Material(_MaskGenerateShader);
                material.SetVector("_ShorelineExtendRange", new Vector2(_TotalBounds.size.x / _Bounds.size.x - 1.0f, _TotalBounds.size.z / _Bounds.size.z - 1.0f));
                material.SetFloat("_TerrainMinPoint", heightOffset);
                material.SetFloat("_Steepness", Mathf.Max(_TotalBounds.size.x, _TotalBounds.size.z) * shoreSteepness);
                material.SetFloat("_Offset1", 1.0f / _MapResolution);
                material.SetFloat("_Offset2", 1.41421356f / _MapResolution);

                RenderTexture distanceMapB = null;

                if (_UnderwaterAreasMode == UnderwaterAreasMode.Generate)
                {
                    // distance map
                    var distanceMapA = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
                    distanceMapB = RenderTexture.GetTemporary(_MapResolution, _MapResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
                    Graphics.Blit(heightMap, distanceMapA, material, 2);
                    ComputeDistanceMap(material, distanceMapA, distanceMapB);
                    RenderTexture.ReleaseTemporary(distanceMapA);

                    distanceMapB.filterMode = FilterMode.Bilinear;

                    // create filtered height map
                    material.SetTexture("_DistanceMap", distanceMapB);
                    material.SetFloat("_GenerateUnderwaterAreas", 1.0f);
                }
                else
                    material.SetFloat("_GenerateUnderwaterAreas", 0.0f);

                material.SetFloat("_DepthScale", _DepthScale);

                Graphics.Blit(heightMap, temp1, material, 0);
                RenderTexture.ReleaseTemporary(heightMap);

                if (distanceMapB != null)
                    RenderTexture.ReleaseTemporary(distanceMapB);

                Graphics.Blit(temp1, temp2);
                ReadBackHeightMap(temp1);

                // create intensity mask
                Graphics.Blit(temp1, intensityMask, material, 1);

                RenderTexture.ReleaseTemporary(temp2);
                RenderTexture.ReleaseTemporary(temp1);
                Destroy(material);
            }
            finally
            {
                RestoreRenderers();
            }
        }
        private void PrepareRenderers()
        {
            bool hasBounds = false;
            _Bounds = new Bounds();

            var gameObjectsList = new List<GameObject>();
            var renderers = GetComponentsInChildren<Renderer>(false);

            for (int i = 0; i < renderers.Length; ++i)
            {
                if (renderers[i].name == "Shoreline Mask")
                    continue;

                var swi = renderers[i].GetComponent<StaticWaterInteraction>();

                if (swi == null || swi == this)
                {
                    gameObjectsList.Add(renderers[i].gameObject);

                    if (hasBounds)
                        _Bounds.Encapsulate(renderers[i].bounds);
                    else
                    {
                        _Bounds = renderers[i].bounds;
                        hasBounds = true;
                    }
                }
            }

            _Terrains = GetComponentsInChildren<Terrain>(false);
            _OriginalTerrainPixelErrors = new float[_Terrains.Length];
            _OriginalDrawTrees = new bool[_Terrains.Length];

            for (int i = 0; i < _Terrains.Length; ++i)
            {
                _OriginalTerrainPixelErrors[i] = _Terrains[i].heightmapPixelError;
                _OriginalDrawTrees[i] = _Terrains[i].drawTreesAndFoliage;

                var swi = _Terrains[i].GetComponent<StaticWaterInteraction>();

                if (swi == null || swi == this)
                {
                    gameObjectsList.Add(_Terrains[i].gameObject);
                    _Terrains[i].heightmapPixelError = 1.0f;
                    _Terrains[i].drawTreesAndFoliage = false;

                    if (hasBounds)
                    {
                        _Bounds.Encapsulate(_Terrains[i].transform.position);
                        _Bounds.Encapsulate(_Terrains[i].transform.position + _Terrains[i].terrainData.size);
                    }
                    else
                    {
                        _Bounds = new Bounds(_Terrains[i].transform.position + _Terrains[i].terrainData.size * 0.5f, _Terrains[i].terrainData.size);
                        hasBounds = true;
                    }
                }
            }

            _GameObjects = gameObjectsList.ToArray();
            _OriginalRendererLayers = new int[_GameObjects.Length];

            for (int i = 0; i < _GameObjects.Length; ++i)
            {
                _OriginalRendererLayers[i] = _GameObjects[i].layer;
                _GameObjects[i].layer = WaterProjectSettings.Instance.WaterTempLayer;
            }
        }
        private void RestoreRenderers()
        {
            if (_Terrains != null)
            {
                for (int i = 0; i < _Terrains.Length; ++i)
                {
                    _Terrains[i].heightmapPixelError = _OriginalTerrainPixelErrors[i];
                    _Terrains[i].drawTreesAndFoliage = _OriginalDrawTrees[i];
                }
            }

            if (_GameObjects != null)
            {
                for (int i = _GameObjects.Length - 1; i >= 0; --i)
                    _GameObjects[i].layer = _OriginalRendererLayers[i];
            }
        }
        private RenderTexture RenderHeightMap(int width, int height)
        {
            var heightMap = RenderTexture.GetTemporary(width, height, 32, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
            heightMap.wrapMode = TextureWrapMode.Clamp;

            RenderTexture.active = heightMap;
            GL.Clear(true, true, new Color(-4000.0f, -4000.0f, -4000.0f, -4000.0f), 1000000.0f);
            RenderTexture.active = null;

            var cameraGo = new GameObject();
            var cameraComponent = cameraGo.AddComponent<Camera>();
            cameraComponent.enabled = false;
            cameraComponent.clearFlags = CameraClearFlags.Nothing;
            cameraComponent.depthTextureMode = DepthTextureMode.None;
            cameraComponent.orthographic = true;
            cameraComponent.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
            cameraComponent.nearClipPlane = 0.95f;
            cameraComponent.farClipPlane = _Bounds.size.y + 2.0f;
            cameraComponent.orthographicSize = _Bounds.size.z * 0.5f;
            cameraComponent.aspect = _Bounds.size.x / _Bounds.size.z;

            Vector3 cameraPosition = _Bounds.center;
            cameraPosition.y = _Bounds.max.y + 1.0f;

            cameraComponent.transform.position = cameraPosition;
            cameraComponent.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);

            cameraComponent.targetTexture = heightMap;
            cameraComponent.RenderWithShader(_HasBottomFaces ? _HeightMapperShaderAlt : _HeightMapperShader, "RenderType");
            cameraComponent.targetTexture = null;

            Destroy(cameraGo);

            return heightMap;
        }

        private static void ComputeDistanceMap(Material material, RenderTexture sa, RenderTexture sb)
        {
            sa.filterMode = FilterMode.Point;
            sb.filterMode = FilterMode.Point;

            var a = sa;
            var b = sb;
            int w = (int)(Mathf.Max(sa.width, sa.height) * 0.7f);

            for (int i = 0; i < w; ++i)
            {
                Graphics.Blit(a, b, material, 3);

                var t = a;
                a = b;
                b = t;
            }

            // ensure that result is in b tex
            if (a != sb)
                Graphics.Blit(a, sb, material, 3);
        }

        private void ReadBackHeightMap(RenderTexture source)
        {
            int width = _IntensityMask.width;
            int height = _IntensityMask.height;

            _HeightMapData = new float[width * height + width + 1];

            RenderTexture.active = source;
            var gpuDownloadTex = new Texture2D(_IntensityMask.width, _IntensityMask.height, TextureFormat.RGBAFloat, false, true);
            gpuDownloadTex.ReadPixels(new Rect(0, 0, _IntensityMask.width, _IntensityMask.height), 0, 0);
            gpuDownloadTex.Apply();
            RenderTexture.active = null;

            int index = 0;

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    float h = gpuDownloadTex.GetPixel(x, y).r;

                    if (h > 0.0f && h < 1.0f)
                        h = Mathf.Sqrt(h);

                    _HeightMapData[index++] = h;
                }
            }

            Destroy(gpuDownloadTex);
        }

        private void CreateMaskRenderer()
        {
            var go = new GameObject("Shoreline Mask")
            {
                hideFlags = HideFlags.DontSave,
                layer = WaterProjectSettings.Instance.WaterTempLayer
            };

            var mf = go.AddComponent<MeshFilter>();
            mf.sharedMesh = Quads.BipolarXZ;

            _InteractionMaskMaterial = new Material(_MaskDisplayShader) { hideFlags = HideFlags.DontSave };
            _InteractionMaskMaterial.SetTexture("_MainTex", _IntensityMask);

            OnValidate();

            _InteractionMaskRenderer = go.AddComponent<MeshRenderer>();
            _InteractionMaskRenderer.sharedMaterial = _InteractionMaskMaterial;
            _InteractionMaskRenderer.enabled = false;

            go.transform.SetParent(transform);
            go.transform.position = new Vector3(_TotalBounds.center.x, 0.0f, _TotalBounds.center.z);
            go.transform.localRotation = Quaternion.identity;

            Vector3 maskLocalScale = _TotalBounds.size * 0.5f;
            maskLocalScale.x /= transform.localScale.x;
            maskLocalScale.y /= transform.localScale.y;
            maskLocalScale.z /= transform.localScale.z;
            go.transform.localScale = maskLocalScale;
        }
        #endregion Private Methods
    }
}