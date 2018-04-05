using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    public class ComplexWavesEmitter : MonoBehaviour, IWavesParticleSystemPlugin
    {
        #region Public Types
        public enum WavesSource
        {
            CustomWaveFrequency,
            WindWavesSpectrum,
            Shoaling,
            Vehicle
        }
        #endregion Public Types

        #region Public Methods
        public void UpdateParticles(float time, float deltaTime)
        {
            if (!isActiveAndEnabled)
                return;

            switch (_WavesSource)
            {
                case WavesSource.CustomWaveFrequency:
                {
                    if (time > _NextSpawnTime)
                    {
                        Vector3 position = transform.position;
                        Vector3 direction = transform.forward;

                        var particle = WaveParticle.Create(
                            new Vector2(position.x, position.z),
                            new Vector2(direction.x, direction.z).normalized,
                            2.0f * Mathf.PI / _Wavelength, _Amplitude, _Lifetime, _ShoreWaves
                        );

                        if (particle != null)
                        {
                            _WavesParticleSystem.Spawn(particle, _Width, _WaveShapeIrregularity);

                            particle.Destroy();
                            particle.AddToCache();
                        }

                        _NextSpawnTime += _TimeStep;
                    }

                    break;
                }

                case WavesSource.WindWavesSpectrum:
                {
                    if (_SpawnPoints == null)
                        CreateSpectralWavesSpawnPoints();

                    UpdateSpawnPoints(deltaTime);

                    break;
                }

                case WavesSource.Shoaling:
                {
                    if (_SpawnPoints == null)
                        CreateShoalingSpawnPoints();

                    UpdateSpawnPoints(deltaTime);

                    break;
                }
            }
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("wavesParticleSystem")] private WaveParticleSystem _WavesParticleSystem;
        [SerializeField, FormerlySerializedAs("wavesSource")] private WavesSource _WavesSource;

        // single wave frequency
        [SerializeField, FormerlySerializedAs("wavelength")] private float _Wavelength = 120.0f;
        [SerializeField, FormerlySerializedAs("amplitude")] private float _Amplitude = 0.6f;
        [SerializeField, FormerlySerializedAs("emissionRate")] private float _EmissionRate = 2.0f;
        [SerializeField, FormerlySerializedAs("width")] private int _Width = 8;

        // spectrum wave frequencies set
        [Range(0.0f, 180.0f)]
        [SerializeField, FormerlySerializedAs("spectrumCoincidenceRange")]
        private float _SpectrumCoincidenceRange = 20.0f;

        [Range(0, 100)]
        [SerializeField, FormerlySerializedAs("spectrumWavesCount")]
        private int _SpectrumWavesCount = 30;

        [Tooltip("Affects both waves and emission area width.")]
        [SerializeField, FormerlySerializedAs("span")]
        private float _Span = 1000.0f;

        [Range(1.0f, 3.5f)]
        [SerializeField, FormerlySerializedAs("waveShapeIrregularity")]
        private float _WaveShapeIrregularity = 2.0f;

        [SerializeField, FormerlySerializedAs("lifetime")]
        private float _Lifetime = 200.0f;

        [SerializeField, FormerlySerializedAs("shoreWaves")]
        private bool _ShoreWaves = true;

        [SerializeField, FormerlySerializedAs("boundsSize")]
        private Vector2 _BoundsSize = new Vector2(500.0f, 500.0f);

        [Range(3.0f, 80.0f)]
        [SerializeField, FormerlySerializedAs("spawnDepth")]
        private float _SpawnDepth = 8.0f;

        [Range(0.01f, 2.0f)]
        [SerializeField, FormerlySerializedAs("emissionFrequencyScale")]
        private float _EmissionFrequencyScale = 1.0f;

        [SerializeField, FormerlySerializedAs("spawnPointsDensity")]
        private float _SpawnPointsDensity = 1.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _WindWaves = _WavesParticleSystem.GetComponent<Water>().WindWaves;

            OnValidate();
            _WavesParticleSystem.RegisterPlugin(this);
        }

        private void OnEnable()
        {
            OnValidate();
            _NextSpawnTime = Time.time + Random.Range(0.0f, _TimeStep);
        }

        private void OnValidate()
        {
            _TimeStep = _Wavelength / _EmissionRate;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            switch (_WavesSource)
            {
                case WavesSource.Shoaling:
                {
                    Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
                    Gizmos.DrawCube(transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(_BoundsSize.x, 0.01f, _BoundsSize.y));

                    break;
                }

                case WavesSource.WindWavesSpectrum:
                {
                    UnityEditor.Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
                    UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.AngleAxis(-_SpectrumCoincidenceRange, Vector3.up) * Vector3.forward, _SpectrumCoincidenceRange * 2.0f, Camera.current != null ? (Camera.current.transform.position.y - transform.position.y) * 0.5f : 10.0f);

                    break;
                }
            }
        }
#endif
        #endregion Unity Methods

        #region Private Types
        private class SpawnPoint
        {
            #region Public Variables
            public readonly Vector2 Position;
            public Vector2 Direction;
            public readonly float Frequency;
            public readonly float Amplitude;
            public readonly float TimeInterval;
            public float TimeLeft;
            #endregion Public Variables

            #region Public Methods
            public SpawnPoint(Vector2 position, Vector2 direction, float frequency, float amplitude, float speed)
            {
                Position = position;
                Direction = direction;
                Frequency = frequency;
                Amplitude = amplitude;

                TimeInterval = 2.0f * Mathf.PI / speed * Random.Range(1.0f, 8.0f);
                TimeLeft = Random.Range(0.0f, TimeInterval);
            }
            #endregion Public Methods
        }
        #endregion Private Types

        #region Private Variables
        private SpawnPoint[] _SpawnPoints;
        private WindWaves _WindWaves;
        private float _NextSpawnTime;
        private float _TimeStep;
        #endregion Private Variables

        #region Private Methods
        private void UpdateSpawnPoints(float deltaTime)
        {
            deltaTime *= _EmissionFrequencyScale;

            for (int i = 0; i < _SpawnPoints.Length; ++i)
            {
                var spawnPoint = _SpawnPoints[i];
                spawnPoint.TimeLeft -= deltaTime;

                if (spawnPoint.TimeLeft < 0)
                {
                    float waveLength = 2.0f * Mathf.PI / spawnPoint.Frequency;
                    float preferredParticleCount = (_Span * 0.3f) / waveLength;
                    int minParticles = Mathf.Max(2, Mathf.RoundToInt(preferredParticleCount * 0.7f));
                    int maxParticles = Mathf.Max(2, Mathf.RoundToInt(preferredParticleCount * 1.429f));

                    spawnPoint.TimeLeft += spawnPoint.TimeInterval;
                    Vector2 position = spawnPoint.Position + new Vector2(spawnPoint.Direction.y, -spawnPoint.Direction.x) * Random.Range(-_Span * 0.35f, _Span * 0.35f);

                    var particle = WaveParticle.Create(position, spawnPoint.Direction, spawnPoint.Frequency, spawnPoint.Amplitude, _Lifetime, _ShoreWaves);

                    if (particle != null)
                    {
                        _WavesParticleSystem.Spawn(particle, Random.Range(minParticles, maxParticles), _WaveShapeIrregularity);
                        particle.Destroy();
                        particle.AddToCache();
                    }
                }
            }
        }

        private void CreateShoalingSpawnPoints()
        {
            var bounds = new Bounds(transform.position, new Vector3(_BoundsSize.x, 0.0f, _BoundsSize.y));

            float minX = bounds.min.x;
            float minZ = bounds.min.z;
            float maxX = bounds.max.x;
            float maxZ = bounds.max.z;
            float spawnPointsDensitySqrt = Mathf.Sqrt(_SpawnPointsDensity);
            float stepX = Mathf.Max(35.0f, bounds.size.x / 256.0f) / spawnPointsDensitySqrt;
            float stepZ = Mathf.Max(35.0f, bounds.size.z / 256.0f) / spawnPointsDensitySqrt;

            var tiles = new bool[32, 32];
            var spawnPoints = new List<SpawnPoint>();

            var waves = _WindWaves.SpectrumResolver.SelectShorelineWaves(50, 0, 360);

            if (waves.Length == 0)
            {
                _SpawnPoints = new SpawnPoint[0];
                return;
            }

            float minSpawnDepth = _SpawnDepth * 0.85f;
            float maxSpawnDepth = _SpawnDepth * 1.18f;

            for (float z = minZ; z < maxZ; z += stepZ)
            {
                for (float x = minX; x < maxX; x += stepX)
                {
                    int tileX = Mathf.FloorToInt(32.0f * (x - minX) / (maxX - minX));
                    int tileZ = Mathf.FloorToInt(32.0f * (z - minZ) / (maxZ - minZ));

                    if (!tiles[tileX, tileZ])
                    {
                        float depth = StaticWaterInteraction.GetTotalDepthAt(x, z);

                        if (depth > minSpawnDepth && depth < maxSpawnDepth && Random.value < 0.06f)
                        {
                            tiles[tileX, tileZ] = true;

                            Vector2 dir;
                            dir.x = StaticWaterInteraction.GetTotalDepthAt(x - 3.0f, z) - StaticWaterInteraction.GetTotalDepthAt(x + 3.0f, z);
                            dir.y = StaticWaterInteraction.GetTotalDepthAt(x, z - 3.0f) - StaticWaterInteraction.GetTotalDepthAt(x, z + 3.0f);
                            dir.Normalize();

                            var bestWave = waves[0];
                            float bestWaveDot = Vector2.Dot(dir, waves[0].Direction);

                            for (int i = 1; i < waves.Length; ++i)
                            {
                                float dot = Vector2.Dot(dir, waves[i].Direction);

                                if (dot > bestWaveDot)
                                {
                                    bestWaveDot = dot;
                                    bestWave = waves[i];
                                }
                            }

                            spawnPoints.Add(new SpawnPoint(new Vector2(x, z), dir, bestWave.Frequency, Mathf.Abs(bestWave.Amplitude), bestWave.Speed));
                        }
                    }
                }
            }

            _SpawnPoints = spawnPoints.ToArray();
        }

        private void CreateSpectralWavesSpawnPoints()
        {
            Vector3 forward = transform.forward.normalized;
            float angle = Mathf.Atan2(forward.x, forward.z);
            var waves = _WindWaves.SpectrumResolver.SelectShorelineWaves(_SpectrumWavesCount, angle * Mathf.Rad2Deg, _SpectrumCoincidenceRange);
            _SpectrumWavesCount = waves.Length;

            Vector3 center = new Vector3(transform.position.x + _Span * 0.5f, 0.0f, transform.position.z + _Span * 0.5f);
            Vector2 centerPos = new Vector2(center.x, center.z);

            var spawnPoints = new List<SpawnPoint>();

            for (int i = 0; i < _SpectrumWavesCount; ++i)
            {
                var wave = waves[i];

                if (wave.Amplitude != 0.0f)
                {
                    Vector2 point = centerPos - wave.Direction * _Span * 0.5f;
                    spawnPoints.Add(new SpawnPoint(point, wave.Direction, wave.Frequency, Mathf.Abs(wave.Amplitude), wave.Speed));
                }
            }

            _SpawnPoints = spawnPoints.ToArray();
        }
        #endregion Private Methods
    }
}