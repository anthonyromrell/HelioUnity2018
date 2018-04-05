using System.Collections.Generic;

namespace UltimateWater
{
    using UnityEngine;

    public class ParticleController : MonoBehaviour
    {
        #region Public Types
        public enum EmissionType
        {
            OnWaterEnter,
            OnWaterExit,
            OnWaterEnterAndExit
        }
        #endregion Public Types

        #region Inspector Variables

        [SerializeField] private List<ParticleSystem> _Particles;
        #endregion Inspector Variables

        #region Public Variables
        [Space]
        public WaterSampler Sampler;

        [Space]
        public EmissionType Type = EmissionType.OnWaterEnterAndExit;

        [Space]
        public float Emission = 1.0f;
        public float Speed = 1.0f;

        [Tooltip("How fast the emission decreases")]
        public float Decrease = 1.0f;
        #endregion Public Variables

        #region Private Variables
        private readonly ParticleModifier _Modifier = new ParticleModifier();
        private float _CurrentEmission;
        #endregion Private Variables

        #region Unity Messages
        private void Awake()
        {
            _CurrentEmission = Emission;

            _Modifier.Initialize(_Particles.ToArray());
            _Modifier.Speed = Speed;
            _Modifier.Active = false;
        }

        private void OnEnable()
        {
            Sampler.OnSubmersionStateChanged.AddListener(OnChange);
        }
        private void OnDisable()
        {
            Sampler.OnSubmersionStateChanged.RemoveListener(OnChange);
        }

        private void LateUpdate()
        {
            _Modifier.Emission = _CurrentEmission > 0 ? _CurrentEmission : 0.0f;

            if (_CurrentEmission > 0.0f)
            {
                _CurrentEmission -= Time.deltaTime * Decrease;
                if (_CurrentEmission <= 0.0f)
                {
                    _Modifier.Active = false;
                }
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying || !_Modifier.IsInitialized)
            {
                return;
            }

            _Modifier.Speed = Speed;
        }
        private void Reset()
        {
            Sampler = GetComponent<WaterSampler>();
        }
        #endregion Unity Messages

        #region Private Methods
        private void OnChange(WaterSampler.SubmersionState state)
        {
            bool activate = false;
            switch (Type)
            {
                case EmissionType.OnWaterEnter: activate = state == WaterSampler.SubmersionState.Under; break;
                case EmissionType.OnWaterExit: activate = state == WaterSampler.SubmersionState.Above; break;
                case EmissionType.OnWaterEnterAndExit: activate = true; break;
            }

            if (!activate)
            {
                return;
            }
            _Modifier.Active = true;
            _Modifier.Emission = _CurrentEmission = Emission;
        }
        #endregion Private Methods
    }
}