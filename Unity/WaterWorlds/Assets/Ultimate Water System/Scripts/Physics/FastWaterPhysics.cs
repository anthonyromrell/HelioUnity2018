namespace UltimateWater
{
    using UnityEngine;
    using Internal;

    /// <summary>
    /// Use this component for small objects that need physical simulation.
    /// </summary>
    public class FastWaterPhysics : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField]
        private Water _Water;

        [Tooltip("Adjust buoyancy proportionally, if your collider is bigger or smaller than the actual object." +
                 " Lowering this may fix some weird behavior of objects with extremely low density like beach balls or baloons.")]
        [SerializeField]
        private float _BuoyancyIntensity = 1.0f;

        [Range(0.0f, 3.0f)]
        [Tooltip("Controls drag force. Determined experimentally in wind tunnels. Example values:\n https://en.wikipedia.org/wiki/Drag_coefficient#General")]
        [SerializeField]
        private float _DragCoefficient = 0.9f;

        [Tooltip("Horizontal flow force intensity.")]
        [SerializeField]
        private float _FlowIntensity = 1.0f;
        #endregion Inspector Variables

        #region Unity Messages
        private void Awake()
        {
            _RayUp = new Ray(Vector3.zero, Vector3.up);
            _RayDown = new Ray(Vector3.zero, Vector3.down);
        }

        private void Start()
        {
            _RigidBody = GetComponentInParent<Rigidbody>();
            _LocalCollider = GetComponentInParent<Collider>();

            if (_Water == null)
                _Water = WaterSystem.Instance.BoundlessWaters[0];

            OnValidate();

            Vector3 position = transform.position;
            _LastPositionX = position.x;
            _LastPositionZ = position.z;

            _Sample = new WaterSample(_Water, WaterSample.DisplacementMode.HeightAndForces);
            _Sample.Start(transform.position);
        }

        private void OnValidate()
        {
            if (_LocalCollider != null)
            {
                _Volume = _LocalCollider.ComputeVolume();
                _Area = _LocalCollider.ComputeArea();
            }

            if (_FlowIntensity < 0) _FlowIntensity = 0;
            if (_BuoyancyIntensity < 0) _BuoyancyIntensity = 0;

            if (_Water != null)
            {
                PrecomputeBuoyancy();
                PrecomputeDrag();
                PrecomputeFlow();
            }
        }

        private void FixedUpdate()
        {
            if (_RigidBody.isKinematic)
            {
                return;
            }

            var bounds = _LocalCollider.bounds;
            float min = bounds.min.y;
            float max = bounds.max.y;

            Vector3 displaced;
            Vector3 flowForce;
            var position = transform.position;

            float height = max - min + 80.0f;
            float fixedDeltaTime = Time.fixedDeltaTime;
            float forceToVelocity = fixedDeltaTime * (1.0f - _RigidBody.drag * fixedDeltaTime) / _RigidBody.mass;
            float time = _Water.Time;
            RaycastHit hitInfo;

            /*
             * Compute new samples.
             */
            Vector3 point = transform.position;
            _Sample.GetAndResetFast(point.x, point.z, time, out displaced, out flowForce);

            displaced.x += position.x - _LastPositionX;
            displaced.z += position.z - _LastPositionZ;

            float waterHeight = displaced.y;
            displaced.y = min - 20.0f;
            _RayUp.origin = displaced;

            if (_LocalCollider.Raycast(_RayUp, out hitInfo, height))
            {
                float low = hitInfo.point.y;
                Vector3 normal = hitInfo.normal;

                displaced.y = max + 20.0f;
                _RayDown.origin = displaced;
                _LocalCollider.Raycast(_RayDown, out hitInfo, height);

                float high = hitInfo.point.y;
                float frc = (waterHeight - low) / (high - low);

                if (!(frc > 0.0f)) // this condition looks weird, but includes NaNs
                    return;

                if (frc > 1.0f)
                    frc = 1.0f;

                // buoyancy
                Vector3 force = _BuoyancyPart * frc;

                float t = frc * 0.5f;
                displaced.y = low * (1.0f - t) + high * t;

                // hydrodynamic drag
                if (_UseCheapDrag)
                {
                    var pointVelocity = _RigidBody.GetPointVelocity(displaced);
                    var velocity = pointVelocity + force * forceToVelocity;

                    Vector3 sqrVelocity;
                    sqrVelocity.x = velocity.x > 0.0f ? -velocity.x * velocity.x : velocity.x * velocity.x;
                    sqrVelocity.y = velocity.y > 0.0f ? -velocity.y * velocity.y : velocity.y * velocity.y;
                    sqrVelocity.z = velocity.z > 0.0f ? -velocity.z * velocity.z : velocity.z * velocity.z;

                    var dragForce = sqrVelocity * _DragPart;

                    float dragVelocityDelta = dragForce.magnitude * forceToVelocity;
                    float dragVelocityDeltaSq = dragVelocityDelta * dragVelocityDelta;
                    float pointVelocitySq = Vector3.Dot(pointVelocity, pointVelocity);

                    // limit drag to avoid inverting velocity direction
                    if (dragVelocityDeltaSq > pointVelocitySq)
                    {
                        frc *= Mathf.Sqrt(pointVelocitySq) / dragVelocityDelta;
                    }

                    force += dragForce * frc;
                }

                // apply buoyancy and drag
                _RigidBody.AddForceAtPosition(force, displaced, ForceMode.Force);

                if (_UseCheapFlow)
                {
                    // flow force
                    float flowForceMagnitude = Vector3.Dot(flowForce, flowForce);
                    if (flowForceMagnitude != 0)
                    {
                        t = -1.0f / flowForceMagnitude;
                        float d = Vector3.Dot(normal, flowForce) * t + 0.5f;

                        if (d > 0)
                        {
                            // apply flow force
                            force = flowForce * (d * _FlowPart);
                            displaced.y = low;
                            _RigidBody.AddForceAtPosition(force, displaced, ForceMode.Force);
                        }
                    }
                }

#if UNITY_EDITOR
                if (WaterProjectSettings.Instance.DebugPhysics)
                {
                    displaced.y = waterHeight;
                    Debug.DrawLine(displaced, displaced + force / _RigidBody.mass, Color.white, 0.0f, false);
                }
#endif
            }

            _LastPositionX = position.x;
            _LastPositionZ = position.z;
        }
        #endregion Unity Messages

        #region Private Variables
        private Rigidbody _RigidBody;
        private Collider _LocalCollider;
        private WaterSample _Sample;
        private float _LastPositionX, _LastPositionZ;
        private Vector3 _BuoyancyPart;
        private float _DragPart, _FlowPart;
        private float _Volume, _Area;
        private bool _UseCheapDrag, _UseCheapFlow;

        private static Ray _RayUp;
        private static Ray _RayDown;
        #endregion Private Variables

        #region Private Methods
        private void PrecomputeBuoyancy()
        {
            _BuoyancyPart = -Physics.gravity * (_Volume * _BuoyancyIntensity * _Water.Density);
        }

        private void PrecomputeDrag()
        {
            _UseCheapDrag = _DragCoefficient > 0.0f;
            _DragPart = 0.25f * _DragCoefficient * _Area * _Water.Density;
        }

        private void PrecomputeFlow()
        {
            _UseCheapFlow = _FlowIntensity > 0.0f;
            _FlowPart = _FlowIntensity * _DragCoefficient * _Area * 100.0f;
        }
        #endregion Private Methods
    }
}