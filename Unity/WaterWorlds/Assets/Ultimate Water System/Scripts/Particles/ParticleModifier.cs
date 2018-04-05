namespace UltimateWater
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ParticleModifier
    {
        #region Public Variables
        public float Emission
        {
            set
            {
                for (int i = 0; i < _Systems.Length; ++i)
                {
                    var data = _InitialData[i];

                    var system = _Systems[i];
                    var module = system.emission;

                    // emission rate
                    var cached = data.EmissionRate;
#if UNITY_5_6_OR_NEWER
                    module.rateOverTime = MultiplyMinMaxCurve(value, cached);
#else
                    module.rate = MultiplyMinMaxCurve(value, cached);
#endif

                    // burst
                    int bursts = module.GetBursts(_Bursts);
                    for (int b = 0; b < bursts; ++b)
                    {
                        _Bursts[b].minCount = (short)(data.BurstRates[b].x * value);
                        _Bursts[b].maxCount = (short)(data.BurstRates[b].y * value);
                    }
                    module.SetBursts(_Bursts, bursts);
                }
            }
        }
        public float Speed
        {
            set
            {
                for (int i = 0; i < _Systems.Length; ++i)
                {
                    var data = _InitialData[i];
                    var system = _Systems[i];

#if UNITY_5_6_OR_NEWER
                    var main = system.main;
                    main.startSpeedMultiplier = data.Speed * value;
#else
                    system.startSpeed = data.Speed * value;
#endif
                }
            }
        }
        public bool Active
        {
            set
            {
                if (value)
                {
                    for (int i = 0; i < _Systems.Length; ++i)
                    {
                        var system = _Systems[i];
                        system.Play();
                    }
                }
                else
                {
                    for (int i = 0; i < _Systems.Length; ++i)
                    {
                        var system = _Systems[i];
                        system.Stop();
                    }
                }
            }
        }

        public bool IsInitialized
        {
            get
            {
                return _Systems != null;
            }
        }

        #endregion Public Variables

        #region Private Types
        private struct InitialData
        {
            public ParticleSystem.MinMaxCurve EmissionRate;
            public List<Vector2> BurstRates;
            public float Speed;
        }
        #endregion Private Types

        #region Private Variables
        private ParticleSystem[] _Systems;
        private List<InitialData> _InitialData;
        #endregion Private Variables

        #region Public Methods
        public void Initialize(ParticleSystem[] particleSystems)
        {
            _Systems = particleSystems;
            _InitialData = new List<InitialData>(_Systems.Length);

            foreach (var system in _Systems)
            {
                InitialData data;

#if UNITY_5_6_OR_NEWER
                data.Speed = system.main.startSpeedMultiplier;
                data.EmissionRate = system.emission.rateOverTime;
#else
                data.Speed = system.startSpeed;
                data.EmissionRate = system.emission.rate;
#endif
                data.BurstRates = new List<Vector2>();

                int bursts = system.emission.GetBursts(_Bursts);
                for (int i = 0; i < bursts; ++i)
                {
                    data.BurstRates.Add(new Vector2(_Bursts[i].minCount, _Bursts[i].maxCount));
                }

                _InitialData.Add(data);
            }
        }
        #endregion Public Methods

        #region Helper Methods
        private static readonly ParticleSystem.Burst[] _Bursts = new ParticleSystem.Burst[4];

        private static ParticleSystem.MinMaxCurve MultiplyMinMaxCurve(float value, ParticleSystem.MinMaxCurve result)
        {
            switch (result.mode)
            {
                case ParticleSystemCurveMode.Constant:
                {
                    result.constant *= value;
                    break;
                }

                case ParticleSystemCurveMode.Curve:
                case ParticleSystemCurveMode.TwoCurves:
                {
#if UNITY_5_5_OR_NEWER
                    result.curveMultiplier = value;
#else
                    result.curveScalar = value;
#endif
                    break;
                }

                case ParticleSystemCurveMode.TwoConstants:
                {
                    result.constantMin *= value;
                    result.constantMax *= value;
                    break;
                }
            }

            return result;
        }
        #endregion Helper Methods
    }
}