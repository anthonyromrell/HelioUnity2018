namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    public class ShipBowWavesEmitter : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("gpuParticleSystem")]
        private WaveParticlesSystemGPU _GPUParticleSystem;
        [SerializeField, FormerlySerializedAs("unityParticleSystem")] private ParticleSystem _UnityParticleSystem;

        [Range(0.02f, 0.98f)]
        [SerializeField, FormerlySerializedAs("waveSpeed")]
        private float _WaveSpeed = 0.5f;

        [SerializeField, FormerlySerializedAs("amplitude")] private float _Amplitude = 0.5f;
        [SerializeField, FormerlySerializedAs("wavelength")] private float _Wavelength = 6.0f;
        [SerializeField, FormerlySerializedAs("lifetime")] private float _Lifetime = 50.0f;
        [SerializeField, FormerlySerializedAs("foam")] private float _Foam = 1.0f;
        [SerializeField, FormerlySerializedAs("maxShipSpeed")] private float _MaxShipSpeed = 16.5f;
        [SerializeField, FormerlySerializedAs("leftRightSpace")] private float _LeftRightSpace = 1.0f;
        [Range(0.0f, 1.0f)] [SerializeField, FormerlySerializedAs("trailCalming")] private float _TrailCalming = 1.0f;
        [Range(0.0f, 8.0f)] [SerializeField, FormerlySerializedAs("trailFoam")] private float _TrailFoam = 1.0f;

        [Header("Advanced")]
        [Tooltip("Use for submarines. Allows emission to be moved to exposed ship parts during submerge process and completely disabled after complete submarge.")]
        [SerializeField, FormerlySerializedAs("advancedEmissionPositioning")]
        private bool _AdvancedEmissionPositioning;

        [Tooltip("Required if 'advancedEmissionPositioning' is enabled. Allows emitter to determine an emission point on that collider.")]
        [SerializeField, FormerlySerializedAs("shipCollider")]
        private Collider _ShipCollider;

        [SerializeField, FormerlySerializedAs("advancedEmissionOffset")] private float _AdvancedEmissionOffset = 2.0f;
        [SerializeField, FormerlySerializedAs("minTextureIndex")] private int _MinTextureIndex;
        [SerializeField, FormerlySerializedAs("maxTextureIndex")] private int _MaxTextureIndex = 4;

        [Range(0.1f, 1.0f)]
        [SerializeField, FormerlySerializedAs("emissionSpacing")]
        private float _EmissionSpacing = 0.45f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            if (_GPUParticleSystem == null)
                _GPUParticleSystem = FindObjectOfType<WaveParticlesSystemGPU>();

            _UseBuiltinParticleSystem = _UnityParticleSystem == null;

            _WaterComponent = _GPUParticleSystem.GetComponent<Water>();
            OnValidate();

            Vector2 bowPosition = GetVector2(transform.position);
            Vector2 bowPositionWithOffset = bowPosition + _WaterComponent.SurfaceOffset;
            _PreviousFrameBowPosition = bowPositionWithOffset;
        }

        private void OnValidate()
        {
            _Space = _Wavelength * _EmissionSpacing;

            float angle = Mathf.Acos(_WaveSpeed);
            _AngleSin = Mathf.Sin(angle);
            _AngleCos = Mathf.Cos(angle);
        }

        private void LateUpdate()
        {
            Vector2 bowPosition = GetVector2(transform.position);
            Vector2 bowPositionWithOffset = bowPosition + _WaterComponent.SurfaceOffset;
            Vector2 bowDelta = bowPositionWithOffset - _PreviousFrameBowPosition;
            Vector2 bowForward = GetVector2(transform.forward).normalized;

            _PreviousFrameBowPosition = bowPositionWithOffset;

            float bowDeltaMagnitudeSq = bowDelta.x * bowForward.x + bowDelta.y * bowForward.y;

            if (bowDeltaMagnitudeSq < 0.0f)
                return;

            float bowDeltaMagnitude = bowDeltaMagnitudeSq;
            _TotalBowDeltaMagnitude += bowDeltaMagnitude;

            if (_TotalBowDeltaMagnitude >= _Space)
            {
                float time = Time.time;
                float timeSpan = time - _LastBowEmitTime;
                _LastBowEmitTime = time;

                float shipSpeed = _TotalBowDeltaMagnitude / timeSpan;

                if (shipSpeed >= _MaxShipSpeed)
                    shipSpeed = _MaxShipSpeed;

                float waveSpeed = _WaveSpeed * shipSpeed;

                Vector2 rightWaveDirection = new Vector2(
                    bowForward.x * _AngleCos - bowForward.y * _AngleSin,
                    bowForward.x * _AngleSin + bowForward.y * _AngleCos
                    );

                Vector2 leftWaveDirection = new Vector2(
                    bowForward.x * _AngleCos + bowForward.y * _AngleSin,
                    bowForward.y * _AngleCos - bowForward.x * _AngleSin
                    );

                Vector2 bowRight = GetVector2(transform.right).normalized;

                do
                {
                    _TotalBowDeltaMagnitude -= _Space;

                    if (_AdvancedEmissionPositioning)
                    {
                        float waterElevation = _WaterComponent.transform.position.y + _WaterComponent.GetHeightAt(bowPosition.x, bowPosition.y, time);
                        RaycastHit hitInfo;

                        if (!_ShipCollider.Raycast(
                                new Ray(new Vector3(bowPosition.x, waterElevation, bowPosition.y), new Vector3(-bowForward.x, 0.0f, -bowForward.y)),
                                out hitInfo, 100.0f))
                            return;

                        bowPosition = GetVector2(hitInfo.point) + bowForward * _AdvancedEmissionOffset;
                    }

                    Vector2 displacement = _WaterComponent.GetHorizontalDisplacementAt(bowPosition.x, bowPosition.y, time);
                    bowPosition.x -= displacement.x;
                    bowPosition.y -= displacement.y;

                    if (_UseBuiltinParticleSystem)
                    {
                        _GPUParticleSystem.EmitParticle(new WaveParticlesSystemGPU.ParticleData()
                        {
                            Position = bowPosition + bowRight * _LeftRightSpace,
                            Direction = leftWaveDirection * waveSpeed,
                            Amplitude = _Amplitude,
                            Wavelength = _Wavelength,
                            InitialLifetime = _Lifetime,
                            Lifetime = _Lifetime,
                            Foam = _Foam,
                            UvOffsetPack =
                                Random.Range(0, _GPUParticleSystem.FoamAtlasHeight) / (float)_GPUParticleSystem.FoamAtlasHeight * 16 + Random.Range(_MinTextureIndex, _MaxTextureIndex) / (float)_GPUParticleSystem.FoamAtlasWidth,
                            TrailCalming = _TrailCalming,
                            TrailFoam = _TrailFoam
                        });

                        _GPUParticleSystem.EmitParticle(new WaveParticlesSystemGPU.ParticleData()
                        {
                            Position = bowPosition + bowRight * -_LeftRightSpace,
                            Direction = rightWaveDirection * waveSpeed,
                            Amplitude = _Amplitude,
                            Wavelength = _Wavelength,
                            InitialLifetime = _Lifetime,
                            Lifetime = _Lifetime,
                            Foam = _Foam,
                            UvOffsetPack =
                                Random.Range(0, _GPUParticleSystem.FoamAtlasHeight) / (float)_GPUParticleSystem.FoamAtlasHeight * 16 + Random.Range(_MinTextureIndex, _MaxTextureIndex) / (float)_GPUParticleSystem.FoamAtlasWidth,
                            TrailCalming = _TrailCalming,
                            TrailFoam = _TrailFoam
                        });
                    }
                    else
                    {
                        var emitParams = new ParticleSystem.EmitParams();
                        emitParams.position = new Vector3(bowPosition.x + bowRight.x * _LeftRightSpace, _WaterComponent.transform.position.y, bowPosition.y + bowRight.y * _LeftRightSpace);
                        emitParams.velocity = new Vector3(leftWaveDirection.x, 0.0f, leftWaveDirection.y) * waveSpeed;
                        _UnityParticleSystem.Emit(emitParams, 1);

                        emitParams.position = new Vector3(bowPosition.x + bowRight.x * -_LeftRightSpace, emitParams.position.y, bowPosition.y + bowRight.y * -_LeftRightSpace);
                        emitParams.velocity = new Vector3(rightWaveDirection.x, 0.0f, rightWaveDirection.y) * waveSpeed;
                        _UnityParticleSystem.Emit(emitParams, 1);
                    }
                } while (_TotalBowDeltaMagnitude >= _Space);
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private Vector2 _PreviousFrameBowPosition;
        private float _TotalBowDeltaMagnitude;
        private float _LastBowEmitTime;
        private float _AngleSin, _AngleCos;
        private float _Space;
        private bool _UseBuiltinParticleSystem;
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