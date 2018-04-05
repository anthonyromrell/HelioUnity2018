namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// Emits waves at a steady rate. Use Power property to adjust its intensity.
    /// </summary>
    public class WavesEmitter : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// General intensity of the waves.
        /// </summary>
        public float Power
        {
            get { return _Power; }
            set
            {
                _Power = value > 0.0f ? value : 0.0f;
                _FinalEmissionInterval = _EmissionInterval / _Power;
                enabled = _Power != 0.0f;
            }
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("water")] private WaveParticlesSystemGPU _Water;

        [SerializeField, FormerlySerializedAs("amplitude")] private float _Amplitude = 0.1f;
        [SerializeField, FormerlySerializedAs("wavelength")] private float _Wavelength = 10.0f;
        [SerializeField, FormerlySerializedAs("lifetime")] private float _Lifetime = 50.0f;
        [SerializeField, FormerlySerializedAs("speed")] private float _Speed = 3.5f;
        [SerializeField, FormerlySerializedAs("foam")] private float _Foam = 1.0f;
        [SerializeField, FormerlySerializedAs("emissionArea")] private float _EmissionArea = 1.0f;
        [SerializeField, FormerlySerializedAs("emissionInterval")] private float _EmissionInterval = 0.15f;
        [Range(0.0f, 1.0f)] [SerializeField, FormerlySerializedAs("trailCalming")] private float _TrailCalming = 1.0f;
        [Range(0.0f, 8.0f)] [SerializeField, FormerlySerializedAs("trailFoam")] private float _TrailFoam = 1.0f;
        [Range(0.0f, 180.0f)] [SerializeField, FormerlySerializedAs("emissionAngle")] private float _EmissionAngle;

        [Header("Advanced")]
        [SerializeField, FormerlySerializedAs("minTextureU")]
        private int _MinTextureU = 4;
        [SerializeField, FormerlySerializedAs("maxTextureU")]
        private int _MaxTextureU = 8;

        [Range(0.0f, 1.0f)] [SerializeField, FormerlySerializedAs("initialPower")] private float _InitialPower = 1.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnValidate()
        {
            _FinalEmissionInterval = _EmissionInterval / _Power;
        }
        private void LateUpdate()
        {
            float time = Time.time;

            if (time - _LastEmitTime >= _FinalEmissionInterval)
            {
                _LastEmitTime = time;

                Vector2 sternPosition = GetVector2(transform.position);
                Vector2 sternForward = GetVector2(transform.forward).normalized;
                Vector2 sternRight = GetVector2(transform.right).normalized;

                if (sternForward.x == 0.0f && sternForward.y == 0.0f)
                    sternForward = GetVector2(transform.up).normalized;

                float emissionAngle = _EmissionAngle * Mathf.Deg2Rad;
                float angle = Random.Range(-emissionAngle, emissionAngle);
                float sin, cos;
                FastMath.SinCos2048(angle, out sin, out cos);

                Vector2 waveDirection = new Vector2(
                        sternForward.x * cos - sternForward.y * sin,
                        sternForward.x * sin + sternForward.y * cos
                    );

                Vector2 emitPosition = sternPosition + sternRight * Random.Range(-_EmissionArea, _EmissionArea);

                Vector2 displacement = _WaterComponent.GetHorizontalDisplacementAt(emitPosition.x, emitPosition.y, Time.time);
                emitPosition.x -= displacement.x;
                emitPosition.y -= displacement.y;

                _Water.EmitParticle(new WaveParticlesSystemGPU.ParticleData()
                {
                    Position = emitPosition,
                    Direction = waveDirection * (_Speed * _Power),
                    Amplitude = _Amplitude * _Power,
                    Wavelength = _Wavelength,
                    InitialLifetime = _Lifetime * _Power,
                    Lifetime = _Lifetime * _Power,
                    Foam = _Foam * _Power,
                    UvOffsetPack = Random.Range(0, _Water.FoamAtlasHeight) / (float)_Water.FoamAtlasHeight * 16 + Random.Range(_MinTextureU, _MaxTextureU) / (float)_Water.FoamAtlasWidth,
                    TrailCalming = _TrailCalming,
                    TrailFoam = _TrailFoam
                });
            }
        }
        private void Start()
        {
            if (_Water == null)
                _Water = FindObjectOfType<WaveParticlesSystemGPU>();

            _WaterComponent = _Water.GetComponent<Water>();
            Power = _InitialPower;
        }
        #endregion Unity Methods

        #region Private Variables
        private float _Power = -1;
        private float _LastEmitTime;
        private float _FinalEmissionInterval;
        private Water _WaterComponent;
        #endregion Private Variables

        #region Private Methods
        private static Vector2 GetVector2(Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
        #endregion Private Methods
    }
}