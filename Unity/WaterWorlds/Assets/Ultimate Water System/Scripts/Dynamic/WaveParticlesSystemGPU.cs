namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;
    using Internal;

    public class WaveParticlesSystemGPU : MonoBehaviour, IOverlaysRenderer
    {
        #region Public Types
        public struct ParticleData
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Wavelength;
            public float Amplitude;
            public float InitialLifetime;
            public float Lifetime;
            public float UvOffsetPack;
            public float Foam;
            public float TrailCalming;
            public float TrailFoam;
        }
        #endregion Public Types

        #region Public Variables
        public ComputeBuffer ParticlesBuffer
        {
            get { return _ParticlesA; }
        }
        public int FoamAtlasWidth
        {
            get { return _FoamAtlasWidth; }
        }
        public int FoamAtlasHeight
        {
            get { return _FoamAtlasHeight; }
        }
        #endregion Public Variables

        #region Public Methods
        public void EmitParticle(ParticleData particleData)
        {
            if (_ParticlesToSpawnCount == _ParticlesToSpawn.Length)
                return;

            _ParticlesToSpawn[_ParticlesToSpawnCount++] = particleData;
        }

        public void RenderOverlays(DynamicWaterCameraData overlays)
        {
            var spray = GetComponent<Spray>();

            if (spray != null && spray.ParticlesBuffer != null)
                Graphics.SetRandomWriteTarget(3, spray.ParticlesBuffer);

            _RenderBuffers[0] = overlays.DynamicDisplacementMap.colorBuffer;
            _RenderBuffers[1] = overlays.NormalMap.colorBuffer;

            _ParticlesRenderMaterial.SetBuffer("_Particles", _ParticlesA);
            _ParticlesRenderMaterial.SetMatrix("_ParticlesVP", GL.GetGPUProjectionMatrix(overlays.Camera.PlaneProjectorCamera.projectionMatrix, true) * overlays.Camera.PlaneProjectorCamera.worldToCameraMatrix);

            // displacement and normals
            if (_ParticlesRenderMaterial.SetPass(0))
            {
                Graphics.SetRenderTarget(_RenderBuffers, overlays.DynamicDisplacementMap.depthBuffer);
                Graphics.DrawProceduralIndirect(MeshTopology.Points, _ParticlesRenderInfo);
                Graphics.ClearRandomWriteTargets();
            }

            // trails
            if (_ParticlesRenderMaterial.SetPass(2))
            {
                Graphics.SetRenderTarget(overlays.DisplacementsMask);
                Graphics.DrawProceduralIndirect(MeshTopology.Points, _ParticlesRenderInfo);
            }

            Graphics.SetRenderTarget(null);
        }

        public void RenderFoam(DynamicWaterCameraData overlays)
        {
            // foam
            if (_ParticlesRenderMaterial.SetPass(1))
            {
                Graphics.SetRenderTarget(overlays.FoamMap);
                Graphics.DrawProceduralIndirect(MeshTopology.Points, _ParticlesRenderInfo);
            }

            // foam trails
            if (_ParticlesRenderMaterial.SetPass(3))
            {
                Graphics.DrawProceduralIndirect(MeshTopology.Points, _ParticlesRenderInfo);
            }
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("maxParticles")] private int _MaxParticles = 80000;
        [SerializeField, FormerlySerializedAs("controllerShader")] private ComputeShader _ControllerShader;
        [SerializeField, FormerlySerializedAs("particlesRenderShader")] private Shader _ParticlesRenderShader;
        [SerializeField, FormerlySerializedAs("foamTexture")] private Texture _FoamTexture;
        [SerializeField, FormerlySerializedAs("foamOverlayTexture")] private Texture _FoamOverlayTexture;
        [SerializeField, FormerlySerializedAs("foamAtlasWidth")] private int _FoamAtlasWidth = 8;
        [SerializeField, FormerlySerializedAs("foamAtlasHeight")] private int _FoamAtlasHeight = 4;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnValidate()
        {
            if (_ParticlesRenderShader == null)
                _ParticlesRenderShader = Shader.Find("UltimateWater/Particles/GPU_Render");

            if (_ParticlesRenderMaterial != null)
                SetMaterialProperties();

#if UNITY_EDITOR
            if (_ControllerShader == null)
            {
                var guids = UnityEditor.AssetDatabase.FindAssets("\"WaveParticlesGPU\" t:ComputeShader");

                if (guids.Length != 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _ControllerShader = (ComputeShader)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(ComputeShader));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
#endif
        }
        private void Update()
        {
            CheckResources();
            UpdateParticles();
            SpawnParticles();
            SwapBuffers();

            ComputeBuffer.CopyCount(_ParticlesA, _ParticlesRenderInfo, 0);
        }
        private void Start()
        {
            _Water = GetComponentInParent<Water>();

            OnValidate();

            _ParticlesRenderMaterial = new Material(_ParticlesRenderShader) { hideFlags = HideFlags.DontSave };
            SetMaterialProperties();

            _LastSurfaceOffset = _Water.SurfaceOffset;
        }

        private void OnDestroy()
        {
            if (_ParticlesA != null)
            {
                _ParticlesA.Release();
                _ParticlesA = null;
            }

            if (_ParticlesB != null)
            {
                _ParticlesB.Release();
                _ParticlesB = null;
            }

            if (_ParticlesRenderInfo != null)
            {
                _ParticlesRenderInfo.Release();
                _ParticlesRenderInfo = null;
            }

            if (_ParticlesUpdateInfo != null)
            {
                _ParticlesUpdateInfo.Release();
                _ParticlesUpdateInfo = null;
            }

            if (_SpawnBuffer != null)
            {
                _SpawnBuffer.Release();
                _SpawnBuffer = null;
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Material _ParticlesRenderMaterial;
        private ComputeBuffer _ParticlesA;
        private ComputeBuffer _ParticlesB;
        private ComputeBuffer _SpawnBuffer;
        private ComputeBuffer _ParticlesRenderInfo;
        private ComputeBuffer _ParticlesUpdateInfo;
        private Vector2 _LastSurfaceOffset;
        private uint _ParticlesToSpawnCount;
        private Water _Water;

        private readonly ParticleData[] _ParticlesToSpawn = new ParticleData[16];
        private readonly RenderBuffer[] _RenderBuffers = new RenderBuffer[2];
        #endregion Private Variables

        #region Private Methods
        private void UpdateParticles()
        {
            _ParticlesB.SetCounterValue(0);

            Vector2 surfaceOffset = _Water.SurfaceOffset;
            ComputeBuffer.CopyCount(_ParticlesA, _ParticlesUpdateInfo, 0);

            _ControllerShader.SetFloat("deltaTime", Time.deltaTime);
            _ControllerShader.SetVector("surfaceOffsetDelta", new Vector4(_LastSurfaceOffset.x - surfaceOffset.x, _LastSurfaceOffset.y - surfaceOffset.y, 0.0f, 0.0f));
            _ControllerShader.SetBuffer(0, "Particles", _ParticlesB);
            _ControllerShader.SetBuffer(0, "SourceParticles", _ParticlesA);
            _ControllerShader.DispatchIndirect(0, _ParticlesUpdateInfo);

            _LastSurfaceOffset = surfaceOffset;
        }
        private void SpawnParticles()
        {
            if (_ParticlesToSpawnCount == 0)
                return;

            _SpawnBuffer.SetData(_ParticlesToSpawn);

            _ControllerShader.SetBuffer(1, "Particles", _ParticlesB);
            _ControllerShader.SetBuffer(1, "SpawnedParticles", _SpawnBuffer);
            _ControllerShader.Dispatch(1, 1, 1, 1);

            for (int i = 0; i < _ParticlesToSpawnCount; ++i)
                _ParticlesToSpawn[i].Lifetime = 0.0f;

            _ParticlesToSpawnCount = 0;
        }

        private void SwapBuffers()
        {
            var t = _ParticlesA;
            _ParticlesA = _ParticlesB;
            _ParticlesB = t;
        }

        private void CheckResources()
        {
            if (_ParticlesA == null)
            {
                _ParticlesA = new ComputeBuffer(_MaxParticles, 48, ComputeBufferType.Append);
                _ParticlesA.SetCounterValue(0);

                _ParticlesB = new ComputeBuffer(_MaxParticles, 48, ComputeBufferType.Append);
                _ParticlesB.SetCounterValue(0);

                _SpawnBuffer = new ComputeBuffer(16, 48, ComputeBufferType.Default);
            }

            if (_ParticlesRenderInfo == null)
            {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                particlesRenderInfo = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
#else
                _ParticlesRenderInfo = new ComputeBuffer(1, 16, ComputeBufferType.IndirectArguments);
#endif
                _ParticlesRenderInfo.SetData(new[] { 0, 1, 0, 0 });
            }

            if (_ParticlesUpdateInfo == null)
            {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                particlesUpdateInfo = new ComputeBuffer(1, 12, ComputeBufferType.DrawIndirect);
#else
                _ParticlesUpdateInfo = new ComputeBuffer(1, 12, ComputeBufferType.IndirectArguments);
#endif
                _ParticlesUpdateInfo.SetData(new[] { 0, 1, 1 });
            }
        }
        private void SetMaterialProperties()
        {
            _ParticlesRenderMaterial.SetVector("_FoamAtlasParams", new Vector4(1.0f / _FoamAtlasWidth, 1.0f / _FoamAtlasHeight, 0.0f, 0.0f));
            _ParticlesRenderMaterial.SetTexture("_FoamAtlas", _FoamTexture);
            _ParticlesRenderMaterial.SetTexture("_FoamOverlayTexture", _FoamOverlayTexture);
        }
        #endregion Private Methods
    }
}