using UnityEngine.Serialization;

namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    /// Emits water spray particles.
    /// </summary>
    public sealed class WaterSprayEmitter : MonoBehaviour
    {
        #region Public Variables
        public float StartVelocity
        {
            get { return _StartVelocity; }
            set { _StartVelocity = value; }
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("water")]
        private Spray _Water;

        [SerializeField, FormerlySerializedAs("emissionRate")]
        private float _EmissionRate = 5.0f;

        [SerializeField, FormerlySerializedAs("startIntensity")]
        private float _StartIntensity = 1.0f;

        [SerializeField, FormerlySerializedAs("startVelocity")]
        private float _StartVelocity = 1.0f;

        [SerializeField, FormerlySerializedAs("lifetime")]
        private float _Lifetime = 4.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            int particleIndex = 0;
            _TotalTime += Time.deltaTime;

            while (_TotalTime >= _TimeStep && particleIndex < _Particles.Length)
            {
                _TotalTime -= _TimeStep;

                _Particles[particleIndex].Lifetime = new Vector2(_Lifetime, _Lifetime);
                _Particles[particleIndex].MaxIntensity = _StartIntensity;
                _Particles[particleIndex].Position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                _Particles[particleIndex].Velocity = transform.forward * _StartVelocity;
                _Particles[particleIndex++].Offset = Random.Range(0.0f, 10.0f);
            }

            if (particleIndex != 0)
                _Water.SpawnCustomParticles(_Particles, particleIndex);
        }
        private void OnValidate()
        {
            _TimeStep = 1.0f / _EmissionRate;
        }
        private void Start()
        {
            OnValidate();
            _Particles = new Spray.Particle[Mathf.Max(1, (int)_EmissionRate)];
        }
        #endregion Unity Methods

        #region Private Variables
        private float _TotalTime;
        private float _TimeStep;
        private Spray.Particle[] _Particles;
        #endregion Private Variables
    }
}