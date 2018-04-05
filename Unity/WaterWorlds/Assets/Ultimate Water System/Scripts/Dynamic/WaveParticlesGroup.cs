namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    public class WaveParticlesGroup
    {
        #region Public Variables
        public int ParticleCount
        {
            get
            {
                int count = 0;
                var particle = LeftParticle;

                while (particle != null)
                {
                    ++count;
                    particle = particle.RightNeighbour;
                }

                return count;
            }
        }
        public int Id
        {
            get { return _Id; }
        }
        public WaveParticle LastParticle
        {
            get
            {
                if (LeftParticle == null)
                    return null;

                WaveParticle p = LeftParticle;

                while (p.RightNeighbour != null)
                    p = p.RightNeighbour;

                return p;
            }
        }

        [FormerlySerializedAs("lastUpdateTime")] public float LastUpdateTime;
        [FormerlySerializedAs("lastCostlyUpdateTime")] public float LastCostlyUpdateTime;
        [FormerlySerializedAs("leftParticle")] public WaveParticle LeftParticle;
        #endregion Public Variables

        #region Public Methods
        public WaveParticlesGroup(float startTime)
        {
            _Id = ++_NextId;

            LastUpdateTime = LastCostlyUpdateTime = startTime;
        }

        public void CostlyUpdate(WaveParticlesQuadtree quadtree, float time)
        {
            WaveParticle particle = LeftParticle;
            float deltaTime = time - LastCostlyUpdateTime;
            LastCostlyUpdateTime = time;
            int numSubdivisions = 0;

            do
            {
                var p = particle;
                particle = particle.RightNeighbour;
                numSubdivisions += p.CostlyUpdate(numSubdivisions < 30 ? quadtree : null, deltaTime);
            }
            while (particle != null);

            particle = LeftParticle;

            if (particle == null)
                return;

            WaveParticle firstParticleInWave = particle;
            int waveLength = 0;

            do
            {
                var p = particle;
                particle = particle.RightNeighbour;

                ++waveLength;

                if (p != firstParticleInWave && (p.DisallowSubdivision || particle == null))
                {
                    if (waveLength > 3)
                        FilterRefractedDirections(firstParticleInWave, waveLength);

                    firstParticleInWave = particle;
                    waveLength = 0;
                }
            }
            while (particle != null);
        }
        public void Update(float time)
        {
            WaveParticle particle = LeftParticle;
            float deltaTime = time - LastUpdateTime;
            LastUpdateTime = time;

            float step = deltaTime < 1.0f ? deltaTime : 1.0f;
            float invStep = 1.0f - step;

            do
            {
                var p = particle;
                particle = particle.RightNeighbour;
                p.Update(deltaTime, step, invStep);
            }
            while (particle != null);
        }
        #endregion Public Methods

        #region Private Variables
        private readonly int _Id;
        private static int _NextId;
        #endregion Private Variables

        #region Private Methods
        /// <summary>
        /// Ensures that whole wave is either expanding or contracting.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="waveLength"></param>
        private static void FilterRefractedDirections(WaveParticle left, int waveLength)
        {
            WaveParticle p = left;
            int halfLength = waveLength / 2;

            Vector2 leftDirection = new Vector2();

            for (int i = 0; i < halfLength; ++i)
            {
                leftDirection += p.Direction;
                p = p.RightNeighbour;
            }

            Vector2 rightDirection = new Vector2();

            for (int i = halfLength; i < waveLength; ++i)
            {
                rightDirection += p.Direction;
                p = p.RightNeighbour;
            }

            leftDirection.Normalize();
            rightDirection.Normalize();

            p = left;

            for (int i = 0; i < waveLength; ++i)
            {
                p.Direction = Vector2.Lerp(leftDirection, rightDirection, (float)i / (waveLength - 1));
                p = p.RightNeighbour;
            }
        }
        #endregion Private Methods
    }
}