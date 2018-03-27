using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    /// <summary>
    ///     Simulates wave particles on the water surface.
    /// </summary>
    [RequireComponent(typeof(DynamicWater))]
    [AddComponentMenu("Water/Waves Particle System", 1)]
    public sealed class WaveParticleSystem : MonoBehaviour, IOverlaysRenderer
    {
        #region Public Variables
        public int ParticleCount
        {
            get { return _Particles.Count; }
        }

        public float SimulationTime
        {
            get { return _SimulationTime; }
        }
        #endregion Public Variables

        #region Public Methods
        public WaveParticleSystem()
        {
            _Plugins = new List<IWavesParticleSystemPlugin>();
        }
        public bool Spawn(WaveParticle particle, int clones, float waveShapeIrregularity, float centerElevation = 2.0f, float edgesElevation = 0.35f)
        {
            if (particle == null || _Particles.FreeSpace < clones * 2 + 1)
                return false;

            particle.Group = new WaveParticlesGroup(_SimulationTime);
            particle.BaseAmplitude *= _Water.UniformWaterScale;
            particle.BaseFrequency /= _Water.UniformWaterScale;

            WaveParticle previousParticle = null;

            float minAmplitude = 1.0f / waveShapeIrregularity;

            for (int i = -clones; i <= clones; ++i)
            {
                var p = particle.Clone(particle.Position + new Vector2(particle.Direction.y, -particle.Direction.x) * (i * 1.48f / particle.BaseFrequency));

                if (p == null)
                    continue;

                p.AmplitudeModifiers2 = Random.Range(minAmplitude, 1.0f) * (edgesElevation + (0.5f + Mathf.Cos(Mathf.PI * i / clones) * 0.5f) * (centerElevation - edgesElevation));
                p.LeftNeighbour = previousParticle;

                if (previousParticle != null)
                {
                    previousParticle.RightNeighbour = p;

                    if (i == clones)
                        p.DisallowSubdivision = true;           // it's a last particle of the group
                }
                else
                {
                    p.Group.LeftParticle = p;               // it's a first particle of the group
                    p.DisallowSubdivision = true;
                }

                if (!_Particles.AddElement(p))
                    return previousParticle != null;

                previousParticle = p;
            }

            return true;
        }
        public void RenderOverlays(DynamicWaterCameraData overlays)
        {
        }
        public void RenderFoam(DynamicWaterCameraData overlays)
        {
            if (enabled)
                RenderParticles(overlays);
        }
        public void RegisterPlugin(IWavesParticleSystemPlugin plugin)
        {
            if (!_Plugins.Contains(plugin))
                _Plugins.Add(plugin);
        }
        public void UnregisterPlugin(IWavesParticleSystemPlugin plugin)
        {
            _Plugins.Remove(plugin);
        }
        public bool AddParticle(WaveParticle particle)
        {
            if (particle != null)
            {
                if (particle.Group == null)
                    throw new System.ArgumentException("Particle has no group");

                return _Particles.AddElement(particle);
            }

            return false;
        }
        #endregion Public Methods

        #region Inspector Variables
        [HideInInspector]
        [SerializeField, FormerlySerializedAs("waterWavesParticlesShader")]
        private Shader _WaterWavesParticlesShader;

        [SerializeField, FormerlySerializedAs("maxParticles")]
        private int _MaxParticles = 50000;

        [SerializeField, FormerlySerializedAs("maxParticlesPerTile")]
        private int _MaxParticlesPerTile = 2000;

        [SerializeField, FormerlySerializedAs("prewarmTime")]
        private float _PrewarmTime = 40.0f;

        [Tooltip("Allowed execution time per frame.")]
        [SerializeField, FormerlySerializedAs("timePerFrame")]
        private float _TimePerFrame = 0.8f;
        #endregion Inspector Variables

        #region Unity Methods
        private void LateUpdate()
        {
            if (!_Prewarmed)
                Prewarm();

            UpdateSimulation(Time.deltaTime);
        }
        private void OnValidate()
        {
            _TimePerFrameExp = Mathf.Exp(_TimePerFrame * 0.5f);

            if (_WaterWavesParticlesShader == null)
                _WaterWavesParticlesShader = Shader.Find("UltimateWater/Particles/Particles");

            if (_Particles != null)
                _Particles.DebugMode = _Water.ShaderSet.LocalEffectsDebug;
        }
        private void Awake()
        {
            _Water = GetComponent<Water>();
            OnValidate();
        }

        private void OnEnable()
        {
            CheckResources();
        }

        private void OnDisable()
        {
            FreeResources();
        }
        #endregion Unity Methods

        #region Private Variables
        private WaveParticlesQuadtree _Particles;

        private Water _Water;
        private Material _WaterWavesParticlesMaterial;
        private float _SimulationTime;
        private float _TimePerFrameExp;
        private bool _Prewarmed;

        private readonly List<IWavesParticleSystemPlugin> _Plugins;
        #endregion Private Variables

        #region Private Methods
        private void Prewarm()
        {
            _Prewarmed = true;

            while (_SimulationTime < _PrewarmTime)
                UpdateSimulationWithoutFrameBudget(0.1f);
        }

        private void UpdateSimulation(float deltaTime)
        {
            _SimulationTime += deltaTime;

            UpdatePlugins(deltaTime);
            _Particles.UpdateSimulation(_SimulationTime, _TimePerFrameExp);
        }

        private void UpdateSimulationWithoutFrameBudget(float deltaTime)
        {
            _SimulationTime += deltaTime;

            UpdatePlugins(deltaTime);
            _Particles.UpdateSimulation(_SimulationTime);
        }

        private void UpdatePlugins(float deltaTime)
        {
            int numPlugins = _Plugins.Count;
            for (int i = 0; i < numPlugins; ++i)
                _Plugins[i].UpdateParticles(_SimulationTime, deltaTime);
        }

        private void RenderParticles(DynamicWaterCameraData overlays)
        {
            var spray = GetComponent<Spray>();

            if (spray != null && spray.ParticlesBuffer != null)
                Graphics.SetRandomWriteTarget(3, spray.ParticlesBuffer);

            if (!_Water.ShaderSet.LocalEffectsDebug)
                Graphics.SetRenderTarget(new[] { overlays.DynamicDisplacementMap.colorBuffer, overlays.NormalMap.colorBuffer }, overlays.DynamicDisplacementMap.depthBuffer);
            else
                Graphics.SetRenderTarget(new[] { overlays.DynamicDisplacementMap.colorBuffer, overlays.NormalMap.colorBuffer, overlays.GetDebugMap(true).colorBuffer }, overlays.DynamicDisplacementMap.depthBuffer);

            Shader.SetGlobalMatrix("_ParticlesVP", GL.GetGPUProjectionMatrix(overlays.Camera.PlaneProjectorCamera.projectionMatrix, true) * overlays.Camera.PlaneProjectorCamera.worldToCameraMatrix);

            Vector4 localMapsShaderCoords = overlays.Camera.LocalMapsShaderCoords;
            float uniformWaterScale = GetComponent<Water>().UniformWaterScale;
            _WaterWavesParticlesMaterial.SetFloat("_WaterScale", uniformWaterScale);
            _WaterWavesParticlesMaterial.SetVector("_LocalMapsCoords", localMapsShaderCoords);
            _WaterWavesParticlesMaterial.SetPass(_Water.ShaderSet.LocalEffectsDebug ? 1 : 0);

            _Particles.Render(overlays.Camera.LocalMapsRect);

            Graphics.ClearRandomWriteTargets();
        }

        private void CheckResources()
        {
            if (_WaterWavesParticlesMaterial == null)
                _WaterWavesParticlesMaterial = new Material(_WaterWavesParticlesShader) { hideFlags = HideFlags.DontSave };

            if (_Particles == null)
            {
                _Particles = new WaveParticlesQuadtree(new Rect(-1000.0f, -1000.0f, 2000.0f, 2000.0f), _MaxParticlesPerTile,
                    _MaxParticles)
                { DebugMode = _Water.ShaderSet.LocalEffectsDebug };
            }
        }

        private void FreeResources()
        {
            if (_WaterWavesParticlesMaterial != null)
            {
                _WaterWavesParticlesMaterial.Destroy();
                _WaterWavesParticlesMaterial = null;
            }
        }
        #endregion Private Methods
    }
}