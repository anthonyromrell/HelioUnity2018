namespace UltimateWater
{
    using UnityEngine;
    using System.Collections.Generic;

    using Utils;

    /// <summary>
    /// Used for adding forces to simulations for particle systems
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    [AddComponentMenu("Ultimate Water/Dynamic/Water Particles")]
    public class WaterParticleDisplacement : MonoBehaviour
    {
        #region Public Variables
        [Tooltip("What water to use to check particle collisions")]
        public Water Water;

        [Tooltip("Force that each particle inflicts on water")]
        public float Force = 1.0f;

        [Range(0.0f, 1.0f)]
        [Tooltip("Percentage of particles causing wave effects")]
        public float UsedParticles = 1.0f;

        [Tooltip("Particle force modifier based on particle speed")]
        public AnimationCurve ForceOverSpeed = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(10.0f, 0.0f));

        [Tooltip("Particle force modifier based on particle size")]
        public float SizeMultipier;

        [Tooltip("Calculate only a subset of all particles per frame")]
        [Range(0, 16)]
        public int FrameSplit = 2;

        public bool Initialized { get { return Water != null; } }
        #endregion Public Variables

        #region Public Methods
        public void Initialize(Water water)
        {
            if (Initialized)
            {
                return;
            }
            Water = water;
            Start();
        }
        #endregion Public Methods

        #region Unity Messages
        private void Awake()
        {
            Water = Utilities.GetWaterReference();
            if (Water.IsNullReference(this)) return;

            _System = GetComponent<ParticleSystem>();
            if (_System.IsNullReference(this)) return;

#if UNITY_5_6_OR_NEWER
            var main = _System.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
#else
            _System.simulationSpace = ParticleSystemSimulationSpace.World;
#endif
        }
        private void Start()
        {
            if (!Initialized)
            {
                return;
            }

#if UNITY_5_5_OR_NEWER
            _Particles = new ParticleSystem.Particle[_System.main.maxParticles];
#else
            _Particles = new ParticleSystem.Particle[_System.maxParticles];
#endif

            _Sampler = new WaterSample(Water);
        }
        private void Update()
        {
            if (!Initialized || _System.particleCount == 0)
            {
                return;
            }

            _Data.Clear();

            int count = _System.GetParticles(_Particles);
            int colided = 0;

            int from = 0;
            int to = count;

            // partition particles per-frame based on split value
            if (count > _SplitThreshold)
            {
                from = (int)(count * (_CurrentFrame + 0.0f) / (FrameSplit + 1.0f));
                to = (int)(count * (_CurrentFrame + 1.0f) / (FrameSplit + 1.0f));

                NextBatch();
            }

            // Process all particles, destroy those colliding with water
            // add force to wave simulation
            for (int i = from; i < to; ++i)
            {
                if (_Particles[i].remainingLifetime <= 0.0f) continue;

                float x = _Particles[i].position.x;
                float z = _Particles[i].position.z;
                float height = _Particles[i].position.y;

                // Get water height in particle position
                Vector3 displacement = _Sampler.GetAndReset(x, z);

                // Check if particle is under water and the distance is in range
                if (height < displacement.y && Mathf.Abs(height - displacement.y) < 0.1f)
                {
                    // if the particle was created underwater, ignore it
                    bool ignore = (_Particles[i].startLifetime - _Particles[i].remainingLifetime) < 0.1f;

                    // Check if the particle should inflict water effect based on probability setting
                    if (!ignore && UsedParticles > Random.Range(0.0f, 1.0f))
                    {
                        float speed = _Particles[i].velocity.magnitude;
                        float speedInfluence = ForceOverSpeed.Evaluate(speed);
                        float force = Force * (1.0f + speed * speedInfluence * 0.1f);

                        WaterForce.Data data;
                        data.Position = new Vector3(x, 0.0f, z);
                        data.Force = force * (1.0f + SizeMultipier * _Particles[i].GetCurrentSize(_System));
                        _Data.Add(data);
                    }

                    // Kill the particle
                    _Particles[i].remainingLifetime = 0.0f;
                    // Inform, that particle was destroyed
                    colided++;
                }
            }

            if (_Data.Count != 0)
            {
                WaterRipples.AddForce(_Data);
            }

            // If particles were destroyed in this iteration
            // assign updated data to particle system
            if (colided != 0)
            {
                _System.SetParticles(_Particles, count);
            }
        }
        #endregion Unity Messages

        #region Private Variables
        private ParticleSystem _System;
        private WaterSample _Sampler;

        private ParticleSystem.Particle[] _Particles;

        private int _CurrentFrame;
        private static readonly List<WaterForce.Data> _Data = new List<WaterForce.Data>(256);

        private const int _SplitThreshold = 64;
        #endregion Private Variables

        #region Private Methods
        private void NextBatch()
        {
            if (_CurrentFrame < FrameSplit)
            {
                _CurrentFrame++;
                return;
            }
            _CurrentFrame = 0;
        }
        #endregion Private Methods

        #region Validation
        [InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
        [SerializeField]
        private string _Validation;

        // ReSharper disable once UnusedMember.Local
        private string Validation()
        {
            string info = string.Empty;
            if (Water == null)
            {
                info += "warning: assign water component";
            }

            return info;
        }
        #endregion Validation
    }
}