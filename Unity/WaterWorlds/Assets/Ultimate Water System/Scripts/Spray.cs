namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;
    using Random = UnityEngine.Random;
    using Internal;

    [RequireComponent(typeof(Water))]
    [AddComponentMenu("Ultimate Water/Spray", 1)]
    public sealed class Spray : MonoBehaviour, IOverlaysRenderer
    {
        #region Public Types
        public struct Particle
        {
            #region Public Variables
            public Vector3 Position;
            public Vector3 Velocity;
            public Vector2 Lifetime;
            public float Offset;
            public float MaxIntensity;
            #endregion Public Variables

            #region Public Methods
            public Particle(Vector3 position, Vector3 velocity, float lifetime, float offset, float maxIntensity)
            {
                Position = position;
                Velocity = velocity;
                Lifetime = new Vector2(lifetime, lifetime);
                Offset = offset;
                MaxIntensity = maxIntensity;
            }
            #endregion Public Methods
        }
        #endregion Public Types
        #region Public Variables
        public int MaxParticles
        {
            get { return _MaxParticles; }
        }

        public int SpawnedParticles
        {
            get
            {
                if (_ParticlesA != null)
                {
                    ComputeBuffer.CopyCount(_ParticlesA, _ParticlesInfo, 0);
                    _ParticlesInfo.GetData(_CountBuffer);
                    return _CountBuffer[0];
                }

                return 0;
            }
        }

        public ComputeBuffer ParticlesBuffer
        {
            get { return _ParticlesA; }
        }
        #endregion Public Variables
        #region Public Methods
        public void SpawnCustomParticle(Particle particle)
        {
            if (!enabled)
                return;

            if (_ParticlesToSpawn.Length <= _NumParticlesToSpawn)
                System.Array.Resize(ref _ParticlesToSpawn, _ParticlesToSpawn.Length << 1);

            _ParticlesToSpawn[_NumParticlesToSpawn] = particle;
            ++_NumParticlesToSpawn;
        }

        public void SpawnCustomParticles(Particle[] particles, int numParticles)
        {
            if (!enabled)
                return;

            CheckResources();

            if (_SpawnBuffer == null || _SpawnBuffer.count < particles.Length)
            {
                if (_SpawnBuffer != null)
                    _SpawnBuffer.Release();

                _SpawnBuffer = new ComputeBuffer(particles.Length, 40);
            }

            _SpawnBuffer.SetData(particles);

            _SprayControllerShader.SetFloat("particleCount", numParticles);
            _SprayControllerShader.SetBuffer(2, "SourceParticles", _SpawnBuffer);
            _SprayControllerShader.SetBuffer(2, "TargetParticles", _ParticlesA);
            _SprayControllerShader.Dispatch(2, 1, 1, 1);
        }
        public void RenderOverlays(DynamicWaterCameraData overlays)
        {
        }

        public void RenderFoam(DynamicWaterCameraData overlays)
        {
            if (!enabled)
                return;

            CheckResources();
            //SpawnWindWavesParticlesLocal(overlays);

            if (_SprayToFoam)
                GenerateLocalFoam(overlays);
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector] [SerializeField, FormerlySerializedAs("sprayTiledGeneratorShader")] private Shader _SprayTiledGeneratorShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("sprayLocalGeneratorShader")] private Shader _SprayLocalGeneratorShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("sprayToFoamShader")] private Shader _SprayToFoamShader;
        [HideInInspector] [SerializeField, FormerlySerializedAs("sprayControllerShader")] private ComputeShader _SprayControllerShader;

        [SerializeField, FormerlySerializedAs("sprayMaterial")]
        private Material _SprayMaterial;

        [Range(16, 327675)]
        [SerializeField, FormerlySerializedAs("maxParticles")]
        private int _MaxParticles = 65535;

        [SerializeField, FormerlySerializedAs("sprayToFoam")]
        private bool _SprayToFoam = true;
        #endregion Inspector Variables
        #region Unity Methods

        private void Start()
        {
            _Water = GetComponent<Water>();
            _WindWaves = _Water.WindWaves;
            _Overlays = _Water.DynamicWater;

            _WindWaves.ResolutionChanged.AddListener(OnResolutionChanged);
            _Supported = CheckSupport();

            _LastSurfaceOffset = _Water.SurfaceOffset;

            if (!_Supported)
                enabled = false;
        }

        private void OnEnable()
        {
            _Water = GetComponent<Water>();
            _Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            OnProfilesChanged(_Water);

            // ReSharper disable once DelegateSubtraction
            Camera.onPreCull -= OnSomeCameraPreCull;
            Camera.onPreCull += OnSomeCameraPreCull;
        }

        private void OnDisable()
        {
            if (_Water != null)
                _Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);

            // ReSharper disable once DelegateSubtraction
            Camera.onPreCull -= OnSomeCameraPreCull;
            Dispose();
        }

        private void LateUpdate()
        {
            if (Time.frameCount < 10)
                return;

            if (!_ResourcesReady)
                CheckResources();

            SwapParticleBuffers();
            ClearParticles();
            UpdateParticles();

            if (/*overlays == null && */Camera.main != null)
                SpawnWindWavesParticlesTiled(Camera.main.transform);

            if (_NumParticlesToSpawn != 0)
            {
                SpawnCustomParticles(_ParticlesToSpawn, _NumParticlesToSpawn);
                _NumParticlesToSpawn = 0;
            }
        }
        private void OnValidate()
        {
            _MaxParticles = Mathf.RoundToInt(_MaxParticles / 65535.0f) * 65535;

            if (_SprayTiledGeneratorShader == null)
                _SprayTiledGeneratorShader = Shader.Find("UltimateWater/Spray/Generator (Tiles)");

            if (_SprayLocalGeneratorShader == null)
                _SprayLocalGeneratorShader = Shader.Find("UltimateWater/Spray/Generator (Local)");

            if (_SprayToFoamShader == null)
                _SprayToFoamShader = Shader.Find("UltimateWater/Spray/Spray To Foam");

#if UNITY_EDITOR
            if (_SprayControllerShader == null)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("\"SprayController\" t:ComputeShader");

                if (guids.Length != 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _SprayControllerShader = (ComputeShader)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(ComputeShader));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }

            if (_SprayMaterial == null)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("\"Spray\" t:Material");

                if (guids.Length != 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _SprayMaterial = (Material)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
#endif

            UpdatePrecomputedParams();
        }
        #endregion Unity Methods

        #region Private Variables
        private float _SpawnThreshold = 1.0f;
        private float _SpawnSkipRatio = 0.9f;
        private float _Scale = 1.0f;

        private Water _Water;
        private WindWaves _WindWaves;
        private DynamicWater _Overlays;
        private Material _SprayTiledGeneratorMaterial;
        private Material _SprayLocalGeneratorMaterial;
        private Material _SprayToFoamMaterial;
        private Transform _ProbeAnchor;

        private RenderTexture _BlankOutput;
        private Texture2D _BlankWhiteTex;
        private ComputeBuffer _ParticlesA;
        private ComputeBuffer _ParticlesB;
        private ComputeBuffer _ParticlesInfo;
        private ComputeBuffer _SpawnBuffer;
        private int _Resolution;
        private Mesh _Mesh;
        private bool _Supported;
        private bool _ResourcesReady;
        private Vector2 _LastSurfaceOffset;
        private readonly int[] _CountBuffer = new int[4];
        private float _SkipRatioPrecomp;
        private Particle[] _ParticlesToSpawn = new Particle[10];
        private int _NumParticlesToSpawn;
        private MaterialPropertyBlock[] _PropertyBlocks;
        #endregion Private Variables
        #region Private Methods
        private void OnSomeCameraPreCull(Camera cameraComponent)
        {
            if (!_ResourcesReady)
                return;

            var waterCamera = WaterCamera.GetWaterCamera(cameraComponent);

            if (waterCamera != null && waterCamera.Type == WaterCamera.CameraType.Normal)
            {
                _SprayMaterial.SetBuffer("_Particles", _ParticlesA);
                _SprayMaterial.SetVector("_CameraUp", cameraComponent.transform.up);
                _SprayMaterial.SetVector("_WrapSubsurfaceScatteringPack", _Water.Renderer.PropertyBlock.GetVector("_WrapSubsurfaceScatteringPack"));
                //sprayMaterial.SetTexture("_SubtractiveMask", waterCamera.SubtractiveMask);
                _SprayMaterial.SetFloat("_UniformWaterScale", _Water.UniformWaterScale);

                if (_ProbeAnchor == null)
                {
                    var probeAnchorGo = new GameObject("Spray Probe Anchor") { hideFlags = HideFlags.HideAndDontSave };
                    _ProbeAnchor = probeAnchorGo.transform;
                }

                _ProbeAnchor.position = cameraComponent.transform.position;

                int numMeshes = _PropertyBlocks.Length;

                for (int i = 0; i < numMeshes; ++i)
                    Graphics.DrawMesh(_Mesh, Matrix4x4.identity, _SprayMaterial, 0, cameraComponent, 0, _PropertyBlocks[i], UnityEngine.Rendering.ShadowCastingMode.Off, false, _ProbeAnchor);
            }
        }
        private void SpawnWindWavesParticlesTiled(Transform origin)
        {
            Vector3 originPosition = origin.position;
            float pixelSize = 400.0f / _BlankOutput.width;

            _SprayTiledGeneratorMaterial.CopyPropertiesFromMaterial(_Water.Materials.SurfaceMaterial);
            _SprayTiledGeneratorMaterial.SetVector("_SurfaceOffset", new Vector3(_Water.SurfaceOffset.x, _Water.transform.position.y, _Water.SurfaceOffset.y));
            _SprayTiledGeneratorMaterial.SetVector("_Params", new Vector4(_SpawnThreshold * 0.25835f, _SkipRatioPrecomp, 0.0f, _Scale * 0.455f));
            _SprayTiledGeneratorMaterial.SetVector("_Coordinates", new Vector4(originPosition.x - 200.0f + Random.value * pixelSize, originPosition.z - 200.0f + Random.value * pixelSize, 400.0f, 400.0f));

            if (_Overlays == null)
                _SprayTiledGeneratorMaterial.SetTexture("_LocalNormalMap", GetBlankWhiteTex());

            Graphics.SetRandomWriteTarget(1, _ParticlesA);
            GraphicsUtilities.Blit(null, _BlankOutput, _SprayTiledGeneratorMaterial, 0, _Water.Renderer.PropertyBlock);
            Graphics.ClearRandomWriteTargets();
        }

        private void GenerateLocalFoam(DynamicWaterCameraData data)
        {
            var temp = RenderTexture.GetTemporary(512, 512, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
            Graphics.SetRenderTarget(temp);
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            _SprayToFoamMaterial.SetBuffer("_Particles", _ParticlesA);
            _SprayToFoamMaterial.SetVector("_LocalMapsCoords", data.Camera.LocalMapsShaderCoords);
            _SprayToFoamMaterial.SetFloat("_UniformWaterScale", 50.0f * _Water.UniformWaterScale / data.Camera.LocalMapsRect.width);

            Vector4 particleParams = _SprayMaterial.GetVector("_ParticleParams");
            particleParams.x *= 8.0f;
            particleParams.z = 1.0f;
            _SprayToFoamMaterial.SetVector("_ParticleParams", particleParams);

            int numMeshes = _PropertyBlocks.Length;

            for (int i = 0; i < numMeshes; ++i)
            {
                _SprayToFoamMaterial.SetFloat("_ParticleOffset", i * 65535);
                if (_SprayToFoamMaterial.SetPass(0))
                {
                    Graphics.DrawMeshNow(_Mesh, Matrix4x4.identity, 0);
                }
            }

            var planeProjectorCamera = data.Camera.PlaneProjectorCamera;

            var localMapsRect = data.Camera.LocalMapsRect;
            var localMapsCenter = localMapsRect.center;
            float scale = localMapsRect.width * 0.5f;

            Matrix4x4 matrix = new Matrix4x4
            {
                m03 = localMapsCenter.x,
                m13 = _Water.transform.position.y,
                m23 = localMapsCenter.y,
                m00 = scale,
                m11 = scale,
                m22 = scale,
                m33 = 1.0f
            };

            GL.PushMatrix();
            GL.modelview = planeProjectorCamera.worldToCameraMatrix;
            GL.LoadProjectionMatrix(planeProjectorCamera.projectionMatrix);

            Graphics.SetRenderTarget(data.FoamMap);

            _SprayToFoamMaterial.mainTexture = temp;

            if (_SprayToFoamMaterial.SetPass(1))
                Graphics.DrawMeshNow(Quads.BipolarXZ, matrix, 0);

            GL.PopMatrix();

            RenderTexture.ReleaseTemporary(temp);
        }
        private void UpdateParticles()
        {
            Vector2 windSpeed = _WindWaves.WindSpeed * 0.0008f;
            Vector3 gravity = Physics.gravity;
            float deltaTime = Time.deltaTime;

            if (_Overlays != null)
            {
                var overlaysData = _Overlays.GetCameraOverlaysData(Camera.main, false);

                if (overlaysData != null)
                {
                    _SprayControllerShader.SetTexture(0, "TotalDisplacementMap", overlaysData.TotalDisplacementMap);

                    var mainWaterCamera = WaterCamera.GetWaterCamera(Camera.main);

                    if (mainWaterCamera != null)
                        _SprayControllerShader.SetVector("localMapsCoords", mainWaterCamera.LocalMapsShaderCoords);
                }
                else
                    _SprayControllerShader.SetTexture(0, "TotalDisplacementMap", GetBlankWhiteTex());
            }
            else
                _SprayControllerShader.SetTexture(0, "TotalDisplacementMap", GetBlankWhiteTex());

            Vector2 surfaceOffset = _Water.SurfaceOffset;

            _SprayControllerShader.SetVector("deltaTime", new Vector4(deltaTime, 1.0f - deltaTime * 0.2f, 0.0f, 0.0f));
            _SprayControllerShader.SetVector("externalForces", new Vector3((windSpeed.x + gravity.x) * deltaTime, gravity.y * deltaTime, (windSpeed.y + gravity.z) * deltaTime));
            _SprayControllerShader.SetVector("surfaceOffsetDelta", new Vector3(_LastSurfaceOffset.x - surfaceOffset.x, 0.0f, _LastSurfaceOffset.y - surfaceOffset.y));
            _SprayControllerShader.SetFloat("surfaceOffsetY", transform.position.y);
            _SprayControllerShader.SetVector("waterTileSizesInv", _WindWaves.TileSizesInv);
            _SprayControllerShader.SetBuffer(0, "SourceParticles", _ParticlesB);
            _SprayControllerShader.SetBuffer(0, "TargetParticles", _ParticlesA);
            _SprayControllerShader.Dispatch(0, _MaxParticles / 256, 1, 1);

            _LastSurfaceOffset = surfaceOffset;
        }
        private Texture2D GetBlankWhiteTex()
        {
            if (_BlankWhiteTex == null)
            {
                _BlankWhiteTex = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);

                for (int x = 0; x < 2; ++x)
                    for (int y = 0; y < 2; ++y)
                        _BlankWhiteTex.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 1.0f));

                _BlankWhiteTex.Apply(false, true);
            }

            return _BlankWhiteTex;
        }
        private void ClearParticles()
        {
            _SprayControllerShader.SetBuffer(1, "TargetParticlesFlat", _ParticlesA);
            _SprayControllerShader.Dispatch(1, _MaxParticles / 256, 1, 1);
        }
        private void SwapParticleBuffers()
        {
            var t = _ParticlesB;
            _ParticlesB = _ParticlesA;
            _ParticlesA = t;
        }
        private void OnResolutionChanged(WindWaves windWaves)
        {
            if (_BlankOutput != null)
            {
                Destroy(_BlankOutput);
                _BlankOutput = null;
            }

            _ResourcesReady = false;
        }
        private void OnProfilesChanged(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;

            _SpawnThreshold = 0.0f;
            _SpawnSkipRatio = 0.0f;
            _Scale = 0.0f;

            if (profiles != null)
            {
                for (int i = 0; i < profiles.Length; ++i)
                {
                    var weightedProfile = profiles[i];
                    var profile = weightedProfile.Profile;
                    float weight = weightedProfile.Weight;

                    _SpawnThreshold += profile.SprayThreshold * weight;
                    _SpawnSkipRatio += profile.SpraySkipRatio * weight;
                    _Scale += profile.SpraySize * weight;
                }
            }
        }
        private void UpdatePrecomputedParams()
        {
            if (_Water != null)
                _Resolution = _WindWaves.FinalResolution;

            _SkipRatioPrecomp = Mathf.Pow(_SpawnSkipRatio, 1024.0f / _Resolution);
        }
        private bool CheckSupport()
        {
            return SystemInfo.supportsComputeShaders && _SprayTiledGeneratorShader != null && _SprayTiledGeneratorShader.isSupported;
        }

        private void CheckResources()
        {
            if (_SprayTiledGeneratorMaterial == null)
                _SprayTiledGeneratorMaterial = new Material(_SprayTiledGeneratorShader) { hideFlags = HideFlags.DontSave };

            if (_SprayLocalGeneratorMaterial == null)
                _SprayLocalGeneratorMaterial = new Material(_SprayLocalGeneratorShader) { hideFlags = HideFlags.DontSave };

            if (_SprayToFoamMaterial == null)
                _SprayToFoamMaterial = new Material(_SprayToFoamShader) { hideFlags = HideFlags.DontSave };

            if (_BlankOutput == null)
            {
                UpdatePrecomputedParams();

                _BlankOutput = new RenderTexture(_Resolution, _Resolution, 0,
                    SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8)
                        ? RenderTextureFormat.R8
                        : RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
                { name = "[UWS] Spray - WaterSpray Blank Output Texture", filterMode = FilterMode.Point };
                _BlankOutput.Create();
            }

            if (_Mesh == null)
            {
                int vertexCount = Mathf.Min(_MaxParticles, 65535);

                _Mesh = new Mesh
                {
                    name = "Spray",
                    hideFlags = HideFlags.DontSave,
                    vertices = new Vector3[vertexCount]
                };

                var indices = new int[vertexCount];

                for (int i = 0; i < vertexCount; ++i)
                    indices[i] = i;

                _Mesh.SetIndices(indices, MeshTopology.Points, 0);
                _Mesh.bounds = new Bounds(Vector3.zero, new Vector3(10000000.0f, 10000000.0f, 10000000.0f));
            }

            if (_PropertyBlocks == null)
            {
                int numMeshes = Mathf.CeilToInt(_MaxParticles / 65535.0f);

                _PropertyBlocks = new MaterialPropertyBlock[numMeshes];

                for (int i = 0; i < numMeshes; ++i)
                {
                    var block = _PropertyBlocks[i] = new MaterialPropertyBlock();
                    block.SetFloat("_ParticleOffset", i * 65535);
                }
            }

            if (_ParticlesA == null)
                _ParticlesA = new ComputeBuffer(_MaxParticles, 40, ComputeBufferType.Append);

            if (_ParticlesB == null)
                _ParticlesB = new ComputeBuffer(_MaxParticles, 40, ComputeBufferType.Append);

            if (_ParticlesInfo == null)
            {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                particlesInfo = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
#else
                _ParticlesInfo = new ComputeBuffer(1, 16, ComputeBufferType.IndirectArguments);
#endif
                _ParticlesInfo.SetData(new[] { 0, 1, 0, 0 });
            }

            _ResourcesReady = true;
        }

        private void Dispose()
        {
            if (_BlankOutput != null)
            {
                Destroy(_BlankOutput);
                _BlankOutput = null;
            }

            if (_ParticlesA != null)
            {
                _ParticlesA.Dispose();
                _ParticlesA = null;
            }

            if (_ParticlesB != null)
            {
                _ParticlesB.Dispose();
                _ParticlesB = null;
            }

            if (_ParticlesInfo != null)
            {
                _ParticlesInfo.Release();
                _ParticlesInfo = null;
            }

            if (_Mesh != null)
            {
                Destroy(_Mesh);
                _Mesh = null;
            }

            if (_ProbeAnchor != null)
            {
                Destroy(_ProbeAnchor.gameObject);
                _ProbeAnchor = null;
            }

            if (_SpawnBuffer != null)
            {
                _SpawnBuffer.Release();
                _SpawnBuffer = null;
            }

            _ResourcesReady = false;
        }
        #endregion Private Methods
    }
}