using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater
{
    public class PlayWayWaterShip : MonoBehaviour
    {
        #region Public Types
        public class ParticleSystemData
        {
            #region Public Variables
            public float RateOverTime;
            public float StartSpeed;

            public bool UseAlphaGradient;
            public Color Color;
            public Gradient Gradient;
            #endregion Public Variables

            #region Public Methods
            public ParticleSystemData(ParticleSystem particleSystem)
            {
#if UNITY_5_6_OR_NEWER
                RateOverTime = particleSystem.emission.rateOverTimeMultiplier;
                StartSpeed = particleSystem.main.startSpeedMultiplier;

                switch (particleSystem.main.startColor.mode)
                {
                    case ParticleSystemGradientMode.Color:
                    {
                        UseAlphaGradient = false;
                        Color = particleSystem.main.startColor.color;

                        break;
                    }

                    case ParticleSystemGradientMode.RandomColor:
                    case ParticleSystemGradientMode.Gradient:
                    {
                        UseAlphaGradient = true;
                        Gradient = particleSystem.main.startColor.gradient;

                        break;
                    }

                    default:
                    throw new System.ArgumentException("Unsupported startColor mode: " + particleSystem.main.startColor.mode);
                }
#else
                RateOverTime = particleSystem.emission.rate.constant;
                StartSpeed = particleSystem.startSpeed;
                Color = particleSystem.startColor;
#endif
            }
            #endregion Public Methods
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("rigidBody")] private Rigidbody _RigidBody;
        [SerializeField, FormerlySerializedAs("mainCollider")] private Collider _MainCollider;

        [SerializeField, FormerlySerializedAs("sternEffects")] private ParticleSystem[] _SternEffects;
        [SerializeField, FormerlySerializedAs("sternWaveEmitters")] private WavesEmitter[] _SternWaveEmitters;

        [SerializeField, FormerlySerializedAs("bowWavesEmitter")] private ShipBowWavesEmitter _BowWavesEmitter;
        [SerializeField, FormerlySerializedAs("bowSprayEmitters")] private ParticleSystem[] _BowSprayEmitters;
        [SerializeField, FormerlySerializedAs("maxVelocity")] private float _MaxVelocity = 7.5f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            _PropellerEffectsData = new ParticleSystemData[_SternEffects.Length];

            for (int i = _PropellerEffectsData.Length - 1; i >= 0; --i)
                _PropellerEffectsData[i] = new ParticleSystemData(_SternEffects[i]);

            _BowSprayEmittersData = new ParticleSystemData[_BowSprayEmitters.Length];

            for (int i = _BowSprayEmittersData.Length - 1; i >= 0; --i)
                _BowSprayEmittersData[i] = new ParticleSystemData(_BowSprayEmitters[i]);
        }

        private void OnEnable()
        {
            SetEnabled(true);
        }

        private void OnDisable()
        {
            SetEnabled(false);
        }

        private void Update()
        {
            float baseIntensity = _RigidBody.velocity.magnitude / _MaxVelocity;

            float bowDepthFactor = Mathf.Clamp01(1.0f - (-_BowWavesEmitter.transform.position.y - 1.0f) * 0.5f);
            float bowEffectsIntensity = ClampIntensity(baseIntensity * bowDepthFactor);

            if (bowEffectsIntensity != _PreviousBowEffectsIntensity)
            {
                _PreviousBowEffectsIntensity = bowEffectsIntensity;

                _BowWavesEmitter.enabled = bowEffectsIntensity > 0.0f;

                for (int i = _BowSprayEmitters.Length - 1; i >= 0; --i)
                    SetEffectIntensity(_BowSprayEmitters[i], _BowSprayEmittersData[i], bowEffectsIntensity);
            }

            float sternDepthFactor = Mathf.Clamp01(1.0f - (-_MainCollider.bounds.max.y - 1.0f) * 0.5f);
            float sternEffectsIntensity = ClampIntensity(baseIntensity * sternDepthFactor);

            if (sternEffectsIntensity != _PreviousSternEffectsIntensity)
            {
                _PreviousSternEffectsIntensity = sternEffectsIntensity;

                for (int i = _SternWaveEmitters.Length - 1; i >= 0; --i)
                    _SternWaveEmitters[i].Power = sternEffectsIntensity;

                for (int i = 0; i < _SternEffects.Length; ++i)
                    SetEffectIntensity(_SternEffects[i], _PropellerEffectsData[i], sternEffectsIntensity);
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private ParticleSystemData[] _PropellerEffectsData;
        private ParticleSystemData[] _BowSprayEmittersData;
        private float _PreviousSternEffectsIntensity = float.NaN;
        private float _PreviousBowEffectsIntensity = float.NaN;
        #endregion Private Variables

        #region Private Methods
        private static void SetEffectIntensity(ParticleSystem particleSystem, ParticleSystemData data, float intensity)
        {
            float intensity1 = intensity == 0.0f ? 0.0f : 0.5f + intensity * 0.5f;

#if UNITY_5_6_OR_NEWER
            float intensity2 = intensity * intensity;

            var emission = particleSystem.emission;
            emission.rateOverTimeMultiplier = data.RateOverTime * intensity1;

            var main = particleSystem.main;
            main.startSpeedMultiplier = data.StartSpeed * intensity1;

            if (data.UseAlphaGradient)
            {
                Gradient gradient = data.Gradient;
                var alphaKeys = gradient.alphaKeys;

                for (int i = 0; i < alphaKeys.Length; ++i)
                    alphaKeys[i].alpha *= intensity2;

                main.startColor = gradient;
            }
            else
            {
                Color color = data.Color;
                color.a *= intensity2;
                main.startColor = color;
            }
#else
            var emission = particleSystem.emission;
            emission.rate = data.RateOverTime * intensity1;

            particleSystem.startSpeed = data.StartSpeed * intensity1;
            particleSystem.startColor = data.Color;
#endif
        }

        private static float ClampIntensity(float x)
        {
            return x > 1.0f ? 1.0f : (x < 0.2f ? 0.0f : x);
        }

        private void SetEnabled(bool enable)
        {
            for (int i = 0; i < _SternWaveEmitters.Length; ++i)
                _SternWaveEmitters[i].enabled = enable;

            if (_BowWavesEmitter != null)
                _BowWavesEmitter.enabled = enable;
        }
        #endregion Private Methods
    }
}