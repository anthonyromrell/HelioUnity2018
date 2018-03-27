namespace UltimateWater.Samples
{
    using Utils;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Simple script that generates raindrops onto WaterRaindropsIME
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WaterRain : MonoBehaviour
    {
        #region Public Variables
        [Range(0.0f, 1.0f)]
        public float Intensity = 0.1f;

        [MinMaxRange(0.0f, 2.0f)]
        public MinMaxRange Size = new MinMaxRange() { MinValue = 0.3f, MaxValue = 0.5f };

        [MinMaxRange(0.0f, 8.0f)]
        public MinMaxRange Life = new MinMaxRange() { MinValue = 0.5f, MaxValue = 2.0f };

        public float Force = 5.0f;
        #endregion Public Variables

        #region Private Variables
        private readonly List<WaterRaindropsIME> _Simulations = new List<WaterRaindropsIME>();
        #endregion Private Variables

        #region Unity Messages
        private void Update()
        {
            for (int s = 0; s < _Simulations.Count; ++s)
            {
                var simulation = _Simulations[s];

                if (!(Random.Range(0.0f, 1.0f) < Intensity)) continue;

                var velocity = -transform.up * Force;
                var size = Size.Random();
                var position = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

                simulation.Spawn(velocity, size, Life.Random(), position.x, position.y);

                for (int i = 0; i < Random.Range(0, 20); ++i)
                {
                    var mod = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.05f;
                    simulation.Spawn(Vector3.zero, size * 0.2f, 0.5f + Random.Range(0.0f, 0.4f), position.x + mod.x, position.y + mod.y);
                }
            }
        }

        private void OnTriggerEnter(Collider colliderComponent)
        {
            var obj = colliderComponent.gameObject.GetComponent<WaterRaindropsIME>();
            if (obj != null)
            {
                _Simulations.Add(obj);
            }
        }
        private void OnTriggerExit(Collider colliderComponent)
        {
            var obj = colliderComponent.gameObject.GetComponent<WaterRaindropsIME>();
            if (obj != null)
            {
                _Simulations.Remove(obj);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
        }
        #endregion Unity Messages
    }
}