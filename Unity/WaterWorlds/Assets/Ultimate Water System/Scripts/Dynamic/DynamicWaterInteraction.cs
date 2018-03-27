namespace UltimateWater.Internal
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Serialization;

    public sealed class DynamicWaterInteraction : MonoBehaviour, ILocalFoamRenderer
    {
        #region Public Methods
        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
        {
#if UNITY_5_6_OR_NEWER
            if(_DetectContactArea)
            {
                var bounds = _MeshFilters[0].GetComponent<MeshRenderer>().bounds;

                for (int i = _MeshFilters.Length - 1; i > 0; --i)
                    bounds.Encapsulate(_MeshFilters[i].GetComponent<MeshRenderer>().bounds);

                Vector3 center = bounds.center;
                Vector3 extents = bounds.extents;
                extents.x += _FoamRange;
                extents.y += _FoamRange;
                extents.z += _FoamRange;
                center.y -= extents.y + 1.0f;

                commandBuffer.GetTemporaryRT(_OcclusionMap2Hash, _FoamOcclusionMapResolution, _FoamOcclusionMapResolution, 0, FilterMode.Point, RenderTextureFormat.ARGB32,
                    RenderTextureReadWrite.Linear);
                commandBuffer.SetRenderTarget(_OcclusionMap2Hash);
                commandBuffer.ClearRenderTarget(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

                var cam = overlays.Camera.EffectsCamera;
                cam.transform.position = center;
                cam.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
                cam.orthographic = true;
                cam.orthographicSize = Mathf.Max(extents.x, extents.z);
                cam.nearClipPlane = 1.0f;
                cam.farClipPlane = extents.y*2.0f + 10.0f;
                //Matrix4x4 matrix = Matrix4x4.Ortho(-extents.x, extents.x, -extents.z, extents.z, 1.0f, extents.y * 2.0f + 10.0f) * Matrix4x4.TRS(center, Quaternion.LookRotation(Vector3.up, Vector3.forward), Vector3.one).inverse;
                Matrix4x4 matrix = cam.projectionMatrix*cam.worldToCameraMatrix;
                commandBuffer.SetGlobalMatrix(_OcclusionMapProjectionMatrixHash, matrix);
                commandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, matrix);

                for (int i = _MeshFilters.Length - 1; i >= 0; --i)
                    commandBuffer.DrawMesh(_MeshFilters[i].sharedMesh, _MeshFilters[i].transform.localToWorldMatrix, _Material, 0, 1);

                commandBuffer.GetTemporaryRT(_OcclusionMapHash, _FoamOcclusionMapResolution, _FoamOcclusionMapResolution, 0, FilterMode.Bilinear, RenderTextureFormat.R8,
                    RenderTextureReadWrite.Linear);
                commandBuffer.Blit(_OcclusionMap2Hash, _OcclusionMapHash, _Material, 2);
                commandBuffer.ReleaseTemporaryRT(_OcclusionMap2Hash);

                commandBuffer.SetRenderTarget(overlays.FoamMap);
                var effectsCamera = overlays.Camera.PlaneProjectorCamera;
                commandBuffer.SetViewProjectionMatrices(effectsCamera.worldToCameraMatrix, effectsCamera.projectionMatrix);
            }
#else
            bool detectContactArea = false;
#endif

            for (int i = _MeshFilters.Length - 1; i >= 0; --i)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                commandBuffer.DrawMesh(_MeshFilters[i].sharedMesh, _MeshFilters[i].transform.localToWorldMatrix, _Material, 0, _DetectContactArea ? 0 : 3, overlays.DynamicWater.Water.Renderer.PropertyBlock);
            }

            // ReSharper disable HeuristicUnreachableCode
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_DetectContactArea)
            {
                commandBuffer.ReleaseTemporaryRT(_OcclusionMapHash);
            }
            // ReSharper restore HeuristicUnreachableCode

            if (_Waves && _WaveEmissionFrequency != 0.0f)
            {
                _MatrixTemp[0] = transform.localToWorldMatrix;
                _ObjectToWorld.SetData(_MatrixTemp);

                _ColliderInteractionShader.SetVector("_LocalMapsCoords", overlays.Camera.LocalMapsShaderCoords);
                _ColliderInteractionShader.SetTexture(0, "TotalDisplacementMap", overlays.GetTotalDisplacementMap());
                _ColliderInteractionShader.SetBuffer(0, "Vertices", _ColliderVerticesBuffer);
                _ColliderInteractionShader.SetBuffer(0, "Particles", _Water.ParticlesBuffer);
                _ColliderInteractionShader.SetBuffer(0, "ObjectToWorld", _ObjectToWorld);
                _ColliderInteractionShader.Dispatch(0, Mathf.CeilToInt((_ColliderVerticesBuffer.count >> 1) / 256.0f), 1, 1);
            }
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector, SerializeField, FormerlySerializedAs("colliderInteractionShader")] private ComputeShader _ColliderInteractionShader;
        [HideInInspector, SerializeField, FormerlySerializedAs("maskDisplayShader")] private Shader _MaskDisplayShader;
        [HideInInspector, SerializeField, FormerlySerializedAs("baseShader")] private Shader _BaseShader;

        [Header("Contact Foam")]
        [SerializeField, FormerlySerializedAs("foam")]
        private bool _Foam = true;
        [SerializeField, FormerlySerializedAs("foamPatternTiling")] private float _FoamPatternTiling = 1.0f;
        [SerializeField, FormerlySerializedAs("foamRange")] private float _FoamRange = 1.6f;
        [SerializeField, FormerlySerializedAs("uniformFoamAmount")] private float _UniformFoamAmount = 30.5f;
        [SerializeField, FormerlySerializedAs("noisyFoamAmount")] private float _NoisyFoamAmount = 30.5f;
        [Range(0.0f, 1.0f)] [SerializeField, FormerlySerializedAs("foamIntensity")] private float _FoamIntensity = 0.45f;

        //#if UNITY_5_6_OR_NEWER
        [SerializeField, FormerlySerializedAs("detectContactArea")] private bool _DetectContactArea = false;
        [SerializeField, FormerlySerializedAs("foamOcclusionMapResolution")] private int _FoamOcclusionMapResolution = 128;
        private int _OcclusionMap2Hash;
        private int _OcclusionMapProjectionMatrixHash;
        //#endif

        [SerializeField, FormerlySerializedAs("meshFilters")] private MeshFilter[] _MeshFilters;

        [Header("Waves")]
        [SerializeField, FormerlySerializedAs("waves")]
        private bool _Waves = true;
        [SerializeField, FormerlySerializedAs("water")] private WaveParticlesSystemGPU _Water;
        [Range(0.0f, 4.0f)] [SerializeField, FormerlySerializedAs("waveEmissionFrequency")] private float _WaveEmissionFrequency = 1.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            OnValidate();

            if (_MeshFilters == null || _MeshFilters.Length == 0)
                _MeshFilters = GetComponentsInChildren<MeshFilter>(true);

            _Material = new Material(_BaseShader) { hideFlags = HideFlags.DontSave };
            _OcclusionMapHash = ShaderVariables.OcclusionMap;

#if UNITY_5_6_OR_NEWER
            _OcclusionMap2Hash = ShaderVariables.OcclusionMap2;
            _OcclusionMapProjectionMatrixHash = ShaderVariables.OcclusionMapProjection;
#endif

            OnValidate();

            if (_Waves && _WaveEmissionFrequency != 0.0f)
                CreateComputeBuffers();
        }

        private void OnEnable()
        {
            if (_Foam)
                DynamicWater.AddRenderer(this);
        }

        private void OnDisable()
        {
            DynamicWater.RemoveRenderer(this);
        }

        private void OnDestroy()
        {
            if (_ColliderVerticesBuffer != null)
            {
                _ColliderVerticesBuffer.Release();
                _ColliderVerticesBuffer = null;
            }

            if (_ObjectToWorld != null)
            {
                _ObjectToWorld.Release();
                _ObjectToWorld = null;
            }
        }

        private void OnValidate()
        {
            if (_MaskDisplayShader == null)
                _MaskDisplayShader = Shader.Find("UltimateWater/Utility/ShorelineMaskRender");

            if (_BaseShader == null)
                _BaseShader = Shader.Find("UltimateWater/Utility/DynamicWaterInteraction");

#if UNITY_EDITOR
            if (_ColliderInteractionShader == null)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("\"ColliderInteraction\" t:ComputeShader");

                if (guids.Length != 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _ColliderInteractionShader = (ComputeShader)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(ComputeShader));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
#endif

            if (_Material != null)
            {
                _Material.SetVector("_FoamIntensity", new Vector4(_UniformFoamAmount, _NoisyFoamAmount, _FoamIntensity, 0.0f));
                _Material.SetFloat("_FoamRange", _FoamRange);
                _Material.SetFloat("_FoamIntensityMaskTiling", _FoamPatternTiling);
                //material.SetTexture("_FoamIntensityMask", customFoamMask);
            }
        }
        #endregion Unity Methods

        #region Private Types
        private struct VertexData
        {
#pragma warning disable 414
            public Vector3 Position;
            public Vector3 Normal;
#pragma warning restore 414
        }
        #endregion Private Types

        #region Private Variables
        private Material _Material;
        private int _OcclusionMapHash;

        private ComputeBuffer _ColliderVerticesBuffer;
        private ComputeBuffer _ObjectToWorld;

        private static readonly Matrix4x4[] _MatrixTemp = new Matrix4x4[1];
        #endregion Private Variables

        #region Private Methods
        private void CreateComputeBuffers()
        {
            var meshCollider = GetComponent<MeshCollider>();
            var colliderMesh = meshCollider.sharedMesh;
            var vertices = colliderMesh.vertices;
            var normals = colliderMesh.normals;
            var indices = colliderMesh.GetIndices(0);

            _ColliderVerticesBuffer = new ComputeBuffer(indices.Length * 2, 24, ComputeBufferType.Default);
            _ObjectToWorld = new ComputeBuffer(1, 64, ComputeBufferType.Default);

            var colliderVerticesRaw = new VertexData[indices.Length * 2];
            int index = 0;

            for (int i = 0; i < indices.Length; ++i)
            {
                int currentIndex = indices[i];
                int previousIndex = indices[i % 3 == 0 ? i + 2 : i - 1];

                colliderVerticesRaw[index++] = new VertexData()
                {
                    Position = vertices[previousIndex],
                    Normal = normals[previousIndex]
                };

                colliderVerticesRaw[index++] = new VertexData()
                {
                    Position = vertices[currentIndex],
                    Normal = normals[currentIndex]
                };
            }

            _ColliderVerticesBuffer.SetData(colliderVerticesRaw);
        }
        #endregion Private Methods
    }
}