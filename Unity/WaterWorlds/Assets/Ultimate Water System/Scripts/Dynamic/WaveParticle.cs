using UnityEngine.Serialization;

namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    public sealed class WaveParticle : IPoint2D
    {
        #region Public Variables
        public Vector2 Position
        {
            get { return _Position; }
        }

        public Vector4 PackedParticleData
        {
            get { return new Vector4(Direction.x * 2.0f * Mathf.PI / Frequency, Direction.y * 2.0f * Mathf.PI / Frequency, Shoaling, Speed); }
        }

        public Vector3 VertexData
        {
            get { return new Vector3(_Position.x, _Position.y, Amplitude); }
        }

        public Vector3 DebugData
        {
            get { return new Vector3(Group.Id, 0.0f, 0.0f); }
        }

        [FormerlySerializedAs("direction")] public Vector2 Direction;
        [FormerlySerializedAs("speed")] public float Speed;
        [FormerlySerializedAs("targetSpeed")] public float TargetSpeed = 1.0f;
        [FormerlySerializedAs("baseFrequency")] public float BaseFrequency;
        [FormerlySerializedAs("frequency")] public float Frequency;
        [FormerlySerializedAs("baseAmplitude")] public float BaseAmplitude;
        [FormerlySerializedAs("amplitude")] public float Amplitude;
        [FormerlySerializedAs("fadeFactor")] public float FadeFactor;
        [FormerlySerializedAs("energyBalance")] public float EnergyBalance;
        [FormerlySerializedAs("targetEnergyBalance")] public float TargetEnergyBalance;
        [FormerlySerializedAs("shoaling")] public float Shoaling;
        [FormerlySerializedAs("invkh")] public float Invkh = 1.0f;
        [FormerlySerializedAs("targetInvKh")] public float TargetInvKh = 1.0f;
        [FormerlySerializedAs("baseSpeed")] public float BaseSpeed;
        [FormerlySerializedAs("lifetime")] public float Lifetime;
        [FormerlySerializedAs("amplitudeModifiers")] public float AmplitudeModifiers;
        [FormerlySerializedAs("amplitudeModifiers2")] public float AmplitudeModifiers2 = 1.0f;
        [FormerlySerializedAs("expansionEnergyLoss")] public float ExpansionEnergyLoss;
        [FormerlySerializedAs("isShoreWave")] public bool IsShoreWave;
        [FormerlySerializedAs("isAlive")] public bool IsAlive = true;
        [FormerlySerializedAs("disallowSubdivision")] public bool DisallowSubdivision;
        [FormerlySerializedAs("leftNeighbour")] public WaveParticle LeftNeighbour;
        [FormerlySerializedAs("rightNeighbour")] public WaveParticle RightNeighbour;
        [FormerlySerializedAs("group")] public WaveParticlesGroup Group;
        #endregion Public Variables

        #region Public Methods
        static WaveParticle()
        {
            _WaveParticlesCache = new Stack<WaveParticle>();
            _AmplitudeFuncPrecomp = new float[2048];
            _FrequencyFuncPrecomp = new float[2048];

            for (int i = 0; i < 2048; ++i)
            {
                double p = (i + 0.49f) / 2047.0f;
                double ratio = 4.0 * (1.0 - System.Math.Pow(1.0 - p, 0.33333333));
                _AmplitudeFuncPrecomp[i] = ComputeAmplitudeAtShore(ratio);

                //frequencyFuncPrecomp[i] = 1.0f / ComputeWavelengthAtShore(ratio);		// this is a correct, physical value
                _FrequencyFuncPrecomp[i] = Mathf.Sqrt(1.0f / ComputeWavelengthAtShore(ratio));       // but this one looks better for some reason
            }
        }

        public static WaveParticle Create(Vector3 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
        {
            return Create(new Vector2(position.x, position.z), direction, baseFrequency, baseAmplitude, lifetime, isShoreWave);
        }

        public static WaveParticle Create(Vector2 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
        {
            WaveParticle particle;

            if (_WaveParticlesCache.Count != 0)
            {
                particle = _WaveParticlesCache.Pop();
                particle._Position = position;
                particle.Direction = direction;
                particle.BaseFrequency = baseFrequency;
                particle.BaseAmplitude = baseAmplitude;
                particle.FadeFactor = 0.0f;
                particle.IsShoreWave = isShoreWave;
                particle.BaseSpeed = 2.2f * Mathf.Sqrt(9.81f / baseFrequency);
                particle.Amplitude = baseAmplitude;
                particle.Frequency = baseFrequency;
                particle.TargetSpeed = 1.0f;
                particle.Invkh = 1.0f;
                particle.TargetInvKh = 1.0f;
                particle.EnergyBalance = 0.0f;
                particle.Shoaling = 0.0f;
                particle.Speed = 0.0f;
                particle.TargetEnergyBalance = 0.0f;
                particle.Lifetime = lifetime;
                particle.AmplitudeModifiers = 0.0f;
                particle.AmplitudeModifiers2 = 1.0f;
                particle.ExpansionEnergyLoss = 0.0f;
                particle.IsAlive = true;
                particle.DisallowSubdivision = false;
                if (particle.LeftNeighbour != null || particle.RightNeighbour != null)
                {
                    particle.LeftNeighbour = null;          // WYWALIC
                    particle.RightNeighbour = null;
                }
                particle.CostlyUpdate(null, 0.1f);
            }
            else
                particle = new WaveParticle(position, direction, baseFrequency, baseAmplitude, lifetime, isShoreWave);

            return particle.BaseAmplitude != 0.0f ? particle : null;
        }

        public void Destroy()
        {
            BaseAmplitude = Amplitude = 0.0f;
            IsAlive = false;

            if (LeftNeighbour != null)
            {
                LeftNeighbour.RightNeighbour = RightNeighbour;
                LeftNeighbour.DisallowSubdivision = true;
            }

            if (RightNeighbour != null)
            {
                RightNeighbour.LeftNeighbour = LeftNeighbour;
                RightNeighbour.DisallowSubdivision = true;
            }

            if (Group != null && Group.LeftParticle == this)			// group may be null when particle gets destroyed during constructor execution
                Group.LeftParticle = RightNeighbour;

            LeftNeighbour = null;
            RightNeighbour = null;
        }

        public void DelayedDestroy()
        {
            BaseAmplitude = Amplitude = 0.0f;
            IsAlive = false;
        }

        public void AddToCache()
        {
            _WaveParticlesCache.Push(this);
        }

        public WaveParticle Clone(Vector2 position)
        {
            var particle = Create(position, Direction, BaseFrequency, BaseAmplitude, Lifetime, IsShoreWave);
            if (particle == null) { return null; }

            particle.Amplitude = Amplitude;
            particle.Frequency = Frequency;
            particle.Speed = Speed;
            particle.TargetSpeed = TargetSpeed;
            particle.EnergyBalance = EnergyBalance;
            particle.Shoaling = Shoaling;
            particle.Group = Group;

            return particle;
        }

        public void Update(float deltaTime, float step, float invStep)
        {
            // fade-in and fade-out
            if (Lifetime > 0.0f)
            {
                if (FadeFactor != 1.0f)
                {
                    FadeFactor += deltaTime;

                    if (FadeFactor > 1.0f)
                        FadeFactor = 1.0f;
                }
            }
            else
            {
                FadeFactor -= deltaTime;

                if (FadeFactor <= 0.0f)
                {
                    Destroy();
                    return;
                }
            }

            // energy loss
            if (TargetEnergyBalance < EnergyBalance)
            {
                float energyLossFactor = step * 0.005f;
                EnergyBalance = EnergyBalance * (1.0f - energyLossFactor) + TargetEnergyBalance * energyLossFactor;     // inlined Mathf.Lerp(this.energyBalance, energyBalance, 0.05f);
            }
            else
            {
                float energyGainFactor = step * 0.0008f;
                EnergyBalance = EnergyBalance * (1.0f - energyGainFactor) + TargetEnergyBalance * energyGainFactor;      // inlined Mathf.Lerp(this.energyBalance, energyBalance, 0.008f);
            }

            BaseAmplitude += deltaTime * EnergyBalance;
            BaseAmplitude *= step * ExpansionEnergyLoss + 1.0f;

            if (BaseAmplitude <= 0.01f)
            {
                Destroy();
                return;
            }

            // shoaling effects
            Speed = invStep * Speed + step * TargetSpeed;
            float realSpeed = Speed + EnergyBalance * -20.0f;            // push wave forward when it starts to break as it may get trapped otherwise

            Invkh = invStep * Invkh + step * TargetInvKh;

            int precompIndex = (int)(2047.0f * (1.0f - Invkh * Invkh * Invkh) - 0.49f);
            float frequencyScale = precompIndex >= 2048 ? 1.0f : _FrequencyFuncPrecomp[precompIndex];
            Frequency = BaseFrequency * frequencyScale;
            Amplitude = FadeFactor * BaseAmplitude * (precompIndex >= 2048 ? 1.0f : _AmplitudeFuncPrecomp[precompIndex]);
            //shoaling = invkh;
            Shoaling = AmplitudeModifiers * 0.004f * -EnergyBalance / Amplitude;
            Amplitude *= AmplitudeModifiers;

            float speedMulDeltaTime = realSpeed * deltaTime;
            _Position.x += Direction.x * speedMulDeltaTime;
            _Position.y += Direction.y * speedMulDeltaTime;
        }

        public int CostlyUpdate(WaveParticlesQuadtree quadtree, float deltaTime)
        {
            float depth;

            if (Frequency < 0.025f)          // in case of big waves, sample center and front of the particle to get a better result
            {
                float posx = _Position.x + Direction.x / Frequency;
                float posy = _Position.y + Direction.y / Frequency;
                depth = Mathf.Max(StaticWaterInteraction.GetTotalDepthAt(_Position.x, _Position.y), StaticWaterInteraction.GetTotalDepthAt(posx, posy));
            }
            else
                depth = StaticWaterInteraction.GetTotalDepthAt(_Position.x, _Position.y);

            if (depth <= 0.001f)
            {
                Destroy();
                return 0;
            }

            UpdateWaveParameters(deltaTime, depth);

            int numSubdivisions = 0;

            if (quadtree != null && !DisallowSubdivision)
            {
                if (LeftNeighbour != null)
                    Subdivide(quadtree, LeftNeighbour, this, ref numSubdivisions);

                if (RightNeighbour != null)
                    Subdivide(quadtree, this, RightNeighbour, ref numSubdivisions);
            }

            return numSubdivisions;
        }
        #endregion Public Methods

        #region Private Variables
        private Vector2 _Position;

        private static readonly Stack<WaveParticle> _WaveParticlesCache;
        private static readonly float[] _AmplitudeFuncPrecomp;
        private static readonly float[] _FrequencyFuncPrecomp;
        #endregion Private Variables

        #region Private Methods
        private WaveParticle(Vector2 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
        {
            _Position = position;
            Direction = direction;
            BaseFrequency = baseFrequency;
            BaseAmplitude = baseAmplitude;
            FadeFactor = 0.0f;
            Frequency = baseFrequency;
            Amplitude = baseAmplitude;
            IsShoreWave = isShoreWave;
            BaseSpeed = 2.5f * Mathf.Sqrt(9.81f / baseFrequency);
            Lifetime = lifetime;

            CostlyUpdate(null, 0.1f);
        }

        private void UpdateWaveParameters(float deltaTime, float depth)
        {
            Lifetime -= deltaTime;

            TargetInvKh = 1.0f - 0.25f * BaseFrequency * depth;

            if (TargetInvKh < 0.0f)
                TargetInvKh = 0.0f;

            // a lot faster than: targetTanh = Mathf.Sqrt((float)System.Math.Tanh(baseFrequency * depth));
            int precompIndex = (int)(BaseFrequency * depth * 512.0f);
            TargetSpeed = BaseSpeed * (precompIndex >= 2048 ? 1.0f : FastMath.PositiveTanhSqrtNoZero[precompIndex]);

            if (TargetSpeed < 0.5f)
                TargetSpeed = 0.5f;

            //targetEnergyBalance = baseFrequency * -0.0004f;
            //targetEnergyBalance = 0.0f;
            float wavelength = 0.135f / Frequency;				// 0.5 * PI / 7 = 0.224

            if (wavelength < Amplitude)
                TargetEnergyBalance = -Amplitude * 5.0f;

            // refraction
            if (LeftNeighbour != null && RightNeighbour != null && !DisallowSubdivision)
            {
                Vector2 newDirection = new Vector2(
                    RightNeighbour._Position.y - LeftNeighbour._Position.y,
                    LeftNeighbour._Position.x - RightNeighbour._Position.x
                );

                // normalize
                float newDirectionLen = Mathf.Sqrt(newDirection.x * newDirection.x + newDirection.y * newDirection.y);

                if (newDirectionLen > 0.001f)
                {
                    if (newDirection.x * Direction.x + newDirection.y * Direction.y < 0)
                        newDirectionLen = -newDirectionLen;

                    newDirection.x /= newDirectionLen;
                    newDirection.y /= newDirectionLen;

                    float refractionFactor = 0.6f * deltaTime;
                    if (refractionFactor > 0.6f)
                        refractionFactor = 0.6f;

                    // inlined Vector2.Lerp(direction, newDirection, 0.00005f);
                    Direction.x = Direction.x * (1.0f - refractionFactor) + newDirection.x * refractionFactor;
                    Direction.y = Direction.y * (1.0f - refractionFactor) + newDirection.y * refractionFactor;

                    // normalize
                    float directionLen = Mathf.Sqrt(Direction.x * Direction.x + Direction.y * Direction.y);
                    Direction.x /= directionLen;
                    Direction.y /= directionLen;
                }

                //expansionEnergyLoss = 0.5f * (Vector2.Dot(direction, leftNeighbour.direction) + Vector2.Dot(direction, rightNeighbour.direction));			// inlined below
                ExpansionEnergyLoss = -1.0f + 0.5f * (Direction.x * (LeftNeighbour.Direction.x + RightNeighbour.Direction.x) + Direction.y * (LeftNeighbour.Direction.y + RightNeighbour.Direction.y));

                if (ExpansionEnergyLoss < -1.0f)
                    ExpansionEnergyLoss = -1.0f;

                if (LeftNeighbour.DisallowSubdivision)
                    LeftNeighbour.ExpansionEnergyLoss = ExpansionEnergyLoss;

                if (RightNeighbour.DisallowSubdivision)
                    RightNeighbour.ExpansionEnergyLoss = ExpansionEnergyLoss;
            }

            AmplitudeModifiers = 1.0f;

            if (IsShoreWave)
            {
                // inlined 1.0f - FastMath.TanhSqrt2048Positive(depth * 0.01f)
                int precompIndex2 = (int)(depth * (0.01f * 512.0f));

                if (precompIndex2 < 2048)
                    AmplitudeModifiers *= 1.0f - FastMath.PositiveTanhSqrtNoZero[precompIndex2];
            }

            AmplitudeModifiers *= AmplitudeModifiers2;
        }

        private void Subdivide(WaveParticlesQuadtree quadtree, WaveParticle left, WaveParticle right, ref int numSubdivisions)
        {
            Vector2 diff = left._Position - right._Position;
            float distance = diff.magnitude;

            if (distance * Frequency > 1.0f && distance > 1.0f && quadtree.FreeSpace != 0)          // don't subdivide below 1m on CPU
            {
                var newParticle = Create(right._Position + diff * 0.5f, (left.Direction + right.Direction) * 0.5f, (left.BaseFrequency + right.BaseFrequency) * 0.5f, (left.BaseAmplitude + right.BaseAmplitude) * 0.5f, (left.Lifetime + right.Lifetime) * 0.5f, left.IsShoreWave);

                if (newParticle != null)
                {
                    newParticle.Group = left.Group;
                    newParticle.Amplitude = (left.Amplitude + right.Amplitude) * 0.5f;
                    newParticle.Frequency = (left.Frequency + right.Frequency) * 0.5f;
                    newParticle.Speed = (left.Speed + right.Speed) * 0.5f;
                    newParticle.TargetSpeed = (left.TargetSpeed + right.TargetSpeed) * 0.5f;
                    newParticle.EnergyBalance = (left.EnergyBalance + right.EnergyBalance) * 0.5f;
                    newParticle.Shoaling = (left.Shoaling + right.Shoaling) * 0.5f;
                    newParticle.TargetInvKh = (left.TargetInvKh + right.TargetInvKh) * 0.5f;
                    newParticle.Lifetime = (left.Lifetime + right.Lifetime) * 0.5f;
                    newParticle.TargetEnergyBalance = (left.TargetEnergyBalance + right.TargetEnergyBalance) * 0.5f;
                    newParticle.AmplitudeModifiers = (left.AmplitudeModifiers + right.AmplitudeModifiers) * 0.5f;
                    newParticle.AmplitudeModifiers2 = (left.AmplitudeModifiers2 + right.AmplitudeModifiers2) * 0.5f;
                    newParticle.Invkh = (left.Invkh + right.Invkh) * 0.5f;
                    newParticle.BaseSpeed = (left.BaseSpeed + right.BaseSpeed) * 0.5f;
                    newParticle.ExpansionEnergyLoss = (left.ExpansionEnergyLoss + right.ExpansionEnergyLoss) * 0.5f;
                    newParticle.Direction = left.Direction;

                    if (quadtree.AddElement(newParticle))
                    {
                        /*const float subdivideEnergyLoss = 0.94f;

                        left.baseAmplitude *= subdivideEnergyLoss;
                        left.amplitude *= subdivideEnergyLoss;
                        right.baseAmplitude *= subdivideEnergyLoss;
                        right.amplitude *= subdivideEnergyLoss;
                        newParticle.baseAmplitude *= subdivideEnergyLoss;
                        newParticle.amplitude *= subdivideEnergyLoss;*/

                        newParticle.LeftNeighbour = left;
                        newParticle.RightNeighbour = right;
                        left.RightNeighbour = newParticle;
                        right.LeftNeighbour = newParticle;
                    }

                    ++numSubdivisions;
                }
            }
        }

        /// <summary>
        /// http://link.springer.com/referenceworkentry/10.1007%2F0-387-30843-1_413
        /// </summary>
        /// <param name="kh"></param>
        /// <returns></returns>
        private static float ComputeAmplitudeAtShore(double kh)
        {
            double cosh = System.Math.Cosh(kh);
            return (float)System.Math.Sqrt(2.0 * cosh * cosh / (System.Math.Sinh(2.0 * kh) + 2.0 * kh));
        }

        /// <summary>
        /// Fenton and McKee (1989)
        /// </summary>
        /// <param name="kh"></param>
        /// <returns></returns>
        private static float ComputeWavelengthAtShore(double kh)
        {
            return (float)System.Math.Pow(System.Math.Tanh(System.Math.Pow(kh * System.Math.Tanh(kh), 0.75)), 0.666666);
        }
        #endregion Private Methods
    }
}