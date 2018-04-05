using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    ///     Component that applies buoyancy, flow and drag forces to the rigid body.
    /// </summary>
    ///
    [AddComponentMenu("Ultimate Water/Water Physics")]
    public sealed class WaterPhysics : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Water in which this rigid body is currently submerged in.
        /// </summary>
        public Water AffectingWater
        {
            get { return (object)_WaterProbe != null ? _WaterProbe.CurrentWater : _WaterOverride; }
            set
            {
                bool wasNull = _WaterOverride == null;

                _WaterOverride = value;

                if (_WaterOverride == null)
                {
                    if (!wasNull)
                        OnWaterLeave();

                    CreateWaterProbe();
                }
                else
                {
                    DestroyWaterProbe();
                    OnWaterLeave();
                    OnWaterEnter();
                }
            }
        }

        /// <summary>
        /// Scale for buoyancy force intensity.
        /// 1.0 is the base value.
        /// </summary>
        public float BuoyancyIntensity
        {
            get { return _BuoyancyIntensity; }
            set
            {
                _BuoyancyIntensity = value;

                if (AffectingWater != null)
                {
                    PrecomputeBuoyancy();
                }
            }
        }

        /// <summary>
        /// Controls drag force. Determined experimentally in wind tunnels. Example values:
        /// https://en.wikipedia.org/wiki/Drag_coefficient#General
        /// </summary>
        public float DragCoefficient
        {
            get { return _DragCoefficient; }
            set
            {
                _DragCoefficient = value;

                if (AffectingWater != null)
                {
                    PrecomputeDrag();
                }
            }
        }

        /// <summary>
        /// Scale for flow force intensity. It is the force applied directly by the collision with the waves.
        /// 1.0 is the base value.
        /// </summary>
        public float FlowIntensity
        {
            get { return _FlowIntensity; }
            set
            {
                _FlowIntensity = value;

                if (AffectingWater != null)
                {
                    PrecomputeFlow();
                }
            }
        }

        public float AverageWaterElevation
        {
            get { return _AverageWaterElevation; }
        }

        public float Volume
        {
            get { return _Volume; }
        }
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Rigidbody Mass for which the Total Buoyance equals 100%
        /// </summary>
        public float GetEquilibriumMass(float fluidDensity = 999.8f)
        {
            return _Volume * _BuoyancyIntensity * fluidDensity;
        }

        /// <summary>
        /// Computes and returns total buoyancy force applied when the object is completely submerged.
        /// </summary>
        /// <param name="fluidDensity"></param>
        /// <returns></returns>
        public float GetTotalBuoyancy(float fluidDensity = 999.8f)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && !ValidateForEditor())
                return 0.0f;
#endif

            return Physics.gravity.magnitude * _Volume * _BuoyancyIntensity * fluidDensity / _RigidBody.mass;
        }
        #endregion Public Methods

        #region Inspector Variables
        [Tooltip("Controls precision of the simulation. Keep it low (1 - 2) for small and not important objects. Prefer high values (15 - 30) for ships etc.")]
        [Range(1, 30)]
        [SerializeField, FormerlySerializedAs("sampleCount")]
        private int _SampleCount = 20;

        [Range(0.0f, 6.0f)]
        [Tooltip("Controls drag force. Determined experimentally in wind tunnels. Example values:\n https://en.wikipedia.org/wiki/Drag_coefficient#General")]
        [SerializeField, FormerlySerializedAs("dragCoefficient")]
        private float _DragCoefficient = 0.9f;

        [Range(0.125f, 1.0f)]
        [Tooltip("Determines how many waves will be used in computations. Set it low for big objects, larger than most of the waves. Set it high for smaller objects of size comparable to many waves.")]
        [SerializeField, FormerlySerializedAs("precision")]
        private float _Precision = 0.5f;

        [Tooltip("Adjust buoyancy proportionally, if your collider is bigger or smaller than the actual object. Lowering this may fix some weird behaviour of objects with extremely low density like beach balls or baloons.")]
        [SerializeField, FormerlySerializedAs("buoyancyIntensity")]
        [Range(0.1f, 10.0f)]
        private float _BuoyancyIntensity = 1.0f;

        [Tooltip("Horizontal flow force intensity.")]
        [SerializeField, FormerlySerializedAs("flowIntensity")]
        private float _FlowIntensity = 1.0f;

        [Tooltip("Temporarily supports only mesh colliders.")]
        [SerializeField, FormerlySerializedAs("useImprovedDragAndFlowForces")]
        private bool _UseImprovedDragAndFlowForces;
        #endregion Inspector Variables

        #region Unity Messages
        private void Awake()
        {
            _LocalCollider = GetComponent<Collider>();
            _RigidBody = GetComponent<Rigidbody>();

            _RayUp = new Ray(Vector3.zero, Vector3.up);
            _RayDown = new Ray(Vector3.zero, Vector3.down);

            if (_LocalCollider.IsNullReference(this) ||
                _RigidBody.IsNullReference(this))
            {
                return;
            }

            Vector3 position = transform.position;
            _LastPositionX = position.x;
            _LastPositionZ = position.z;

            OnValidate();
            PrecomputeSamples();

            if (_UseImprovedDragAndFlowForces)
            {
                PrecomputeImprovedDrag();
            }
        }

        private void OnEnable()
        {
            if (_WaterOverride == null)
            {
                CreateWaterProbe();
            }
        }

        private void OnDisable()
        {
            DestroyWaterProbe();
            OnWaterLeave();
        }

        private void OnValidate()
        {
            _NumSamplesInv = 1.0f / _SampleCount;

            if (_LocalCollider != null)
            {
                _Volume = _LocalCollider.ComputeVolume();
                _Area = _LocalCollider.ComputeArea();

                if (_TotalArea == 0.0f)
                {
                    UpdateTotalArea();
                }

                if (_UseImprovedDragAndFlowForces && !(_LocalCollider is MeshCollider))
                {
                    _UseImprovedDragAndFlowForces = false;

                    Debug.LogErrorFormat("Improved drag force won't work colliders other than mesh colliders. '{0}' collider has a wrong type.", name);
                }

                if (_UseImprovedDragAndFlowForces && ((MeshCollider)_LocalCollider).sharedMesh.vertexCount > 3000)
                {
                    _UseImprovedDragAndFlowForces = false;

                    var mesh = ((MeshCollider)_LocalCollider).sharedMesh;
                    Debug.LogErrorFormat("Improved drag force won't work with meshes that have more than 3000 vertices. '{0}' has {1} vertices.", mesh.name, mesh.vertexCount);
                }
            }

            _FlowIntensity = Mathf.Max(_FlowIntensity, 0.0f);
            _BuoyancyIntensity = Mathf.Max(_BuoyancyIntensity, 0.0f);

            if (AffectingWater != null)
            {
                PrecomputeBuoyancy();
                PrecomputeDrag();
                PrecomputeFlow();
            }
        }

        private void FixedUpdate()
        {
            if (_UseImprovedDragAndFlowForces)
            {
                ImprovedFixedUpdate();
            }
            else
            {
                SimpleFixedUpdate();
            }
        }

        private void Reset()
        {
            var rigidbodyComponent = GetComponent<Rigidbody>();
            var colliderComponent = GetComponent<Collider>();
            if (rigidbodyComponent == null && colliderComponent != null)
            {
                rigidbodyComponent = gameObject.AddComponent<Rigidbody>();
                rigidbodyComponent.mass = GetEquilibriumMass();
            }
        }
        #endregion Unity Messages

        #region Private Variables
        private Vector3[] _CachedSamplePositions;
        private int _CachedSampleIndex;
        private int _CachedSampleCount;

        private Collider _LocalCollider;
        private Rigidbody _RigidBody;

        private float _Volume;
        private float _Area = -1.0f;
        private float _TotalArea;

        private WaterSample[] _Samples;

        // precomputed stuff
        private float _NumSamplesInv;
        private Vector3 _BuoyancyPart;
        private Vector3 _ImprovedBuoyancyPart;
        private float _DragPart;
        private float _ImprovedDragPart;
        private float _FlowPart;
        private float _ImprovedFlowPart;
        private float _AverageWaterElevation;
        private bool _UseCheapDrag, _UseCheapFlow;
        private Water _WaterOverride;
        private WaterVolumeProbe _WaterProbe;
        private float _LastPositionX, _LastPositionZ;
        private Vector3[] _DragNormals;
        private Vector3[] _DragCenters;
        private Vector3[] _DragVertices;
        private float[] _PolygonVolumes;
        private float[] _DragAreas;
        private WaterSample[] _ImprovedDragSamples;

        private static Ray _RayUp;
        private static Ray _RayDown;
        #endregion Private Variables

        #region Private Methods
        private void SimpleFixedUpdate()
        {
            var currentWater = AffectingWater;

            if (((object)currentWater) == null || _RigidBody.isKinematic)
            {
                return;
            }

            var bounds = _LocalCollider.bounds;
            float min = bounds.min.y;
            float max = bounds.max.y;

            Vector3 position = transform.position;

            float height = max - min + 80.0f;
            float fixedDeltaTime = Time.fixedDeltaTime;
            float forceToVelocity = fixedDeltaTime * (1.0f - _RigidBody.drag * fixedDeltaTime) / _RigidBody.mass;
            float time = currentWater.Time;
            _AverageWaterElevation = 0.0f;

            /*
             * Compute new samples.
             */
            for (int i = 0; i < _SampleCount; ++i)
            {
                Vector3 point = transform.TransformPoint(_CachedSamplePositions[_CachedSampleIndex]);
                Vector3 displaced;
                Vector3 flowForce;
                _Samples[i].GetAndResetFast(point.x, point.z, time, out displaced, out flowForce);

                displaced.x += position.x - _LastPositionX;
                displaced.z += position.z - _LastPositionZ;

                float waterHeight = displaced.y;
                displaced.y = min - 20.0f;
                _RayUp.origin = displaced;

                _AverageWaterElevation += waterHeight;

                RaycastHit hitInfo;
                if (_LocalCollider.Raycast(_RayUp, out hitInfo, height))
                {
                    float low = hitInfo.point.y;
                    Vector3 normal = hitInfo.normal;

                    displaced.y = max + 20.0f;
                    _RayDown.origin = displaced;
                    _LocalCollider.Raycast(_RayDown, out hitInfo, height);

                    float high = hitInfo.point.y;
                    float frc = (waterHeight - low) / (high - low);

                    if (!(frc > 0.0f))           // this condition looks weird, but includes NaNs
                        continue;

                    if (frc > 1.0f)
                        frc = 1.0f;

                    // buoyancy
                    var force = _BuoyancyPart * frc;

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

                if (++_CachedSampleIndex >= _CachedSampleCount)
                {
                    _CachedSampleIndex = 0;
                }
            }

            _AverageWaterElevation *= _NumSamplesInv;

            _LastPositionX = position.x;
            _LastPositionZ = position.z;
        }

        private void ImprovedFixedUpdate()
        {
            var currentWater = AffectingWater;

            if (((object)currentWater) == null || _RigidBody.isKinematic)
            {
                return;
            }

            float time = currentWater.Time;

            float improvedDragPartValue = _ImprovedDragPart;
            var localToWorldMatrix = transform.localToWorldMatrix;
            var localToWorldRow1 = localToWorldMatrix.GetRow(1);
            var center = _LocalCollider.bounds.center;
            _AverageWaterElevation = 0.0f;
            int vertexIndex = 0;

            for (int i = 0; i < _DragNormals.Length; ++i)
            {
                var polygonCenter = localToWorldMatrix.MultiplyPoint3x4(_DragCenters[i]);
                var v = _RigidBody.GetPointVelocity(polygonCenter);
                var wn = localToWorldMatrix.MultiplyVector(_DragNormals[i]);

                float waterElevation;
                Vector3 flowForce;
                _ImprovedDragSamples[i].GetAndResetFast(polygonCenter.x, polygonCenter.z, time, out waterElevation, out flowForce);

                _AverageWaterElevation += waterElevation;

                float dotDrag = Vector3.Dot(wn, v);
                float dotFlow = Vector3.Dot(flowForce, wn) * _ImprovedFlowPart;

                float p;

                if (dotDrag > 0.0f || dotFlow > 0.0f)
                {
                    float a = SingleComponentTransform(ref _DragVertices[vertexIndex++], ref localToWorldRow1);
                    float b = SingleComponentTransform(ref _DragVertices[vertexIndex++], ref localToWorldRow1);
                    float c = SingleComponentTransform(ref _DragVertices[vertexIndex++], ref localToWorldRow1);

                    float da = waterElevation - a;
                    float db = waterElevation - b;
                    float dc = waterElevation - c;

                    if (da > 0.0f)
                    {
                        if (db > 0.0f)
                        {
                            p = dc >= 0.0f ? 1.0f : (da + db) / (da + db - dc);
                        }
                        else
                        {
                            p = dc >= 0.0f ? (da + dc) / (da - db + dc) : da / (da - db - dc);
                        }
                    }
                    else
                    {
                        if (db > 0.0f)
                        {
                            p = dc >= 0.0f ? (db + dc) / (db + dc - da) : db / (db - dc - da);
                        }
                        else
                        {
                            p = dc >= 0.0f ? dc / (dc - da - db) : 0.0f;
                        }
                    }

                    if (!(p > 0.0f && p <= 1.02f))
                    {
                        p = 0.0f;
                    }
                }
                else
                {
                    p = 0.0f;
                    vertexIndex += 3;
                }

                float submergedArea = _DragAreas[i] * p;

                // drag
                float drag = dotDrag > 0.0f ? improvedDragPartValue * dotDrag * dotDrag * submergedArea : 0.0f;

                float t = v.magnitude;
                drag = t != 0.0f ? drag / t : 0.0f;     // normalization factor, not a part of drag equation
                var force = v * drag;

                // buoyancy
                if (center.y > polygonCenter.y)
                {
                    if (waterElevation > center.y)
                    {
                        p = _PolygonVolumes[i];
                        force.x += _ImprovedBuoyancyPart.x * p;
                        force.y += _ImprovedBuoyancyPart.y * p;
                        force.z += _ImprovedBuoyancyPart.z * p;
                    }
                    else if (waterElevation > polygonCenter.y)
                    {
                        p = _PolygonVolumes[i] * (waterElevation - polygonCenter.y) / (center.y - polygonCenter.y);
                        force.x += _ImprovedBuoyancyPart.x * p;
                        force.y += _ImprovedBuoyancyPart.y * p;
                        force.z += _ImprovedBuoyancyPart.z * p;
                    }
                }
                else if (waterElevation > polygonCenter.y)
                {
                    p = _PolygonVolumes[i];
                    force.x += _ImprovedBuoyancyPart.x * p;
                    force.y += _ImprovedBuoyancyPart.y * p;
                    force.z += _ImprovedBuoyancyPart.z * p;
                }
                else if (waterElevation > center.y)
                {
                    p = _PolygonVolumes[i] * (waterElevation - center.y) / (polygonCenter.y - center.y);
                    force.x += _ImprovedBuoyancyPart.x * p;
                    force.y += _ImprovedBuoyancyPart.y * p;
                    force.z += _ImprovedBuoyancyPart.z * p;
                }

                // flow
                if (dotFlow > 0.0f)
                {
                    t = flowForce.magnitude;
                    float flow = t != 0.0f ? dotFlow * submergedArea / t : 0.0f;     // flowForce.magnitude is a normalization factor, not a part of flow equation
                    force.x += flowForce.x * flow;
                    force.y += flowForce.y * flow;
                    force.z += flowForce.z * flow;
                }

                _RigidBody.AddForceAtPosition(force, polygonCenter, ForceMode.Force);
            }

            _AverageWaterElevation /= _DragNormals.Length;
        }

        private static float SingleComponentTransform(ref Vector3 point, ref Vector4 row)
        {
            return point.x * row.x + point.y * row.y + point.z * row.z + row.w;
        }

        private void CreateWaterProbe()
        {
            if (_WaterProbe == null)
            {
                _WaterProbe = WaterVolumeProbe.CreateProbe(_RigidBody.transform, _LocalCollider.bounds.extents.magnitude);
                _WaterProbe.Enter.AddListener(OnWaterEnter);
                _WaterProbe.Leave.AddListener(OnWaterLeave);
            }
        }

        private void DestroyWaterProbe()
        {
            if (_WaterProbe != null)
            {
                _WaterProbe.gameObject.Destroy();
                _WaterProbe = null;
            }
        }

        private void OnWaterEnter()
        {
            CreateWaterSamplers();
            AffectingWater.ProfilesManager.ValidateProfiles();
            PrecomputeBuoyancy();
            PrecomputeDrag();
            PrecomputeFlow();
        }

        private void OnWaterLeave()
        {
            if (_Samples != null)
            {
                for (int i = 0; i < _SampleCount; ++i)
                    _Samples[i].Stop();

                _Samples = null;
            }
        }

        private bool ValidateForEditor()
        {
            if (_LocalCollider == null)
            {
                _LocalCollider = GetComponent<Collider>();
                _RigidBody = GetComponentInParent<Rigidbody>();
                OnValidate();
            }

            return _LocalCollider != null && _RigidBody != null;
        }

        private void PrecomputeSamples()
        {
            var samplePositions = new List<Vector3>();

            float offset = 0.5f;
            float step = 1.0f;
            int targetPoints = _SampleCount * 18;
            var transformComponent = transform;

            Vector3 min, max;
            ColliderExtensions.GetLocalMinMax(_LocalCollider, out min, out max);

            for (int i = 0; i < 4 && samplePositions.Count < targetPoints; ++i)
            {
                for (float x = offset; x <= 1.0f; x += step)
                {
                    for (float y = offset; y <= 1.0f; y += step)
                    {
                        for (float z = offset; z <= 1.0f; z += step)
                        {
                            Vector3 p = new Vector3(Mathf.Lerp(min.x, max.x, x), Mathf.Lerp(min.y, max.y, y), Mathf.Lerp(min.z, max.z, z));

                            if (_LocalCollider.IsPointInside(transformComponent.TransformPoint(p)))
                                samplePositions.Add(p);
                        }
                    }
                }

                step = offset;
                offset *= 0.5f;
            }

            _CachedSamplePositions = samplePositions.ToArray();
            _CachedSampleCount = _CachedSamplePositions.Length;
            Shuffle(_CachedSamplePositions);
        }

        private void PrecomputeImprovedDrag()
        {
            var meshCollider = (MeshCollider)_LocalCollider;
            var mesh = meshCollider.sharedMesh;
            var vertices = mesh.vertices;
            var normals = mesh.normals;
            var indices = mesh.GetIndices(0);

            int numPolygons = indices.Length / 3;

            _DragNormals = new Vector3[numPolygons];
            _DragVertices = new Vector3[numPolygons * 3];
            _DragCenters = new Vector3[numPolygons];
            _DragAreas = new float[numPolygons];
            _PolygonVolumes = new float[numPolygons];
            Vector3 center = _LocalCollider.transform.InverseTransformPoint(_LocalCollider.bounds.center);

            int index = 0;

            for (int i = 0; i < indices.Length;)
            {
                Vector3 a = vertices[indices[i]];
                Vector3 b = vertices[indices[i + 1]];
                Vector3 c = vertices[indices[i + 2]];

                _DragVertices[i] = a;
                _DragVertices[i + 1] = b;
                _DragVertices[i + 2] = c;

                _DragAreas[index] = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
                _DragCenters[index] = (a + b + c) * 0.333333333f;

                Vector3 na = normals[indices[i++]];
                Vector3 nb = normals[indices[i++]];
                Vector3 nc = normals[indices[i++]];

                _DragNormals[index] = (na + nb + nc) * 0.333333333f;

                Vector3 p1 = a - center;
                Vector3 p2 = b - center;
                Vector3 p3 = c - center;

                _PolygonVolumes[index++] = Mathf.Abs(ColliderExtensions.SignedVolumeOfTriangle(p1, p2, p3));     // improved physics are meant only for concave colliders, so we don't need a sign here
            }

            _ImprovedDragSamples = new WaterSample[numPolygons];
        }

        private void UpdateTotalArea()
        {
            var rigidbodyComponent = GetComponentInParent<Rigidbody>();
            var waterPhysics = rigidbodyComponent.GetComponentsInChildren<WaterPhysics>();

            _TotalArea = 0.0f;

            for (int i = 0; i < waterPhysics.Length; ++i)
            {
                var target = waterPhysics[i];

                if (target.GetComponentInParent<Rigidbody>() != rigidbodyComponent) continue;

                if (target._Area == -1.0f && target._LocalCollider != null)
                {
                    target._Area = target._LocalCollider.ComputeArea();
                }

                _TotalArea += target._Area;
            }

            for (int i = 0; i < waterPhysics.Length; ++i)
            {
                waterPhysics[i]._TotalArea = _TotalArea;
            }
        }

        private void CreateWaterSamplers()
        {
            var affectingWater = AffectingWater;

            if (_UseImprovedDragAndFlowForces)
            {
                for (int i = 0; i < _ImprovedDragSamples.Length; ++i)
                {
                    _ImprovedDragSamples[i] = new WaterSample(affectingWater, WaterSample.DisplacementMode.HeightAndForces, _Precision);
                    _ImprovedDragSamples[i].Start(transform.TransformPoint(_DragCenters[i]));
                }
            }
            else
            {
                if (_Samples == null || _Samples.Length != _SampleCount)
                    _Samples = new WaterSample[_SampleCount];

                for (int i = 0; i < _SampleCount; ++i)
                {
                    _Samples[i] = new WaterSample(affectingWater, WaterSample.DisplacementMode.HeightAndForces, _Precision);
                    _Samples[i].Start(transform.TransformPoint(_CachedSamplePositions[_CachedSampleIndex]));

                    if (++_CachedSampleIndex >= _CachedSampleCount)
                        _CachedSampleIndex = 0;
                }
            }
        }

        private void PrecomputeBuoyancy()
        {
            _BuoyancyPart = -Physics.gravity * (_NumSamplesInv * _Volume * _BuoyancyIntensity * AffectingWater.Density);
            _ImprovedBuoyancyPart = -Physics.gravity * (_BuoyancyIntensity * AffectingWater.Density);
        }

        private void PrecomputeDrag()
        {
            _UseCheapDrag = _DragCoefficient > 0.0f && !_UseImprovedDragAndFlowForces;
            _DragPart = 0.5f * _DragCoefficient * _Area * _NumSamplesInv * AffectingWater.Density;
            _ImprovedDragPart = -0.5f * _DragCoefficient * AffectingWater.Density;
        }

        private void PrecomputeFlow()
        {
            _UseCheapFlow = _FlowIntensity > 0.0f && !_UseImprovedDragAndFlowForces;
            _FlowPart = _FlowIntensity * _DragCoefficient * _Area * _NumSamplesInv * 100.0f;
            _ImprovedFlowPart = _FlowIntensity * _DragCoefficient * -100.0f;           // minus here negates the normal in the main equation
        }

        private static void Shuffle<T>(IList<T> array)
        {
            int n = array.Count;

            while (n > 1)
            {
                int k = Random.Range(0, n--);

                var t = array[n];
                array[n] = array[k];
                array[k] = t;
            }
        }
        #endregion Private Methods
    }
}