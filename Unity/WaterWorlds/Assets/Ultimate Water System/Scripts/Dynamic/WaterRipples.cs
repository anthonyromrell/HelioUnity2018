namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// Water ripples settings,
    /// used for adding forces to simulations
    /// passes rendered screen space textures to water render shader
    /// </summary>
    public sealed class WaterRipples : SceneSingleton<WaterRipples>
    {
        #region Public Methods
        /// <summary>
        // Add new WaterSimulationArea to simulation
        /// </summary>
        public static void Register(WaterSimulationArea area)
        {
            var ripples = Instance;
            if (ripples == null || ripples._Areas.Contains(area)) { return; }

            ripples._Areas.Add(area);
        }

        /// <summary>
        /// Remove WaterSimulationArea from simulation
        /// </summary>
        public static void Unregister(WaterSimulationArea area)
        {
            var ripples = Instance;
            if (ripples == null) { return; }

            ripples._Areas.Remove(area);
        }

        /// <summary>
        /// Adds force to simulation area containing provided position
        /// </summary>
        public static void AddForce(List<WaterForce.Data> data, float radius = 1.0f)
        {
            var ripples = Instance;
            if (ripples == null || Time.timeScale == 0.0f) { return; }

            for (var i = ripples._Areas.Count - 1; i >= 0; --i)
            {
                var entry = ripples._Areas[i];
                entry.AddForce(data, radius);
            }
        }
        #endregion Public Methods

        #region Unity Messages
        /// <summary>
        /// Progresses all simulation areas
        /// </summary>
        private void FixedUpdate()
        {
            var interations = WaterQualitySettings.Instance.Ripples.Iterations;
            for (int i = 0; i < interations; ++i)
            {
                // simulation pass
                for (int index = 0; index < _Areas.Count; ++index)
                {
                    _Areas[index].Simulate();
                }

                // smoothing pass
                for (int index = 0; index < _Areas.Count; ++index)
                {
                    _Areas[index].Smooth();
                }

                // swapping
                for (int index = 0; index < _Areas.Count; ++index)
                {
                    _Areas[index].Swap();
                }
            }
        }

        private void OnDisable()
        {
            _Areas.Clear();
        }
        #endregion Unity Messages

        #region Private Variables
        private readonly List<WaterSimulationArea> _Areas = new List<WaterSimulationArea>();
        #endregion Private Variables
    }
}