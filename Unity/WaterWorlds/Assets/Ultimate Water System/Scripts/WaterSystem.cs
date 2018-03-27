namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class WaterSystem : ApplicationSingleton<WaterSystem>
    {
        #region Public Variables
        /// <summary>
        /// Enabled waters in the current scene.
        /// </summary>
        public List<Water> Waters { get { return _Waters; } }

        /// <summary>
        /// Enabled boundless waters in the current scene.
        /// </summary>
        public List<Water> BoundlessWaters { get { return _BoundlessWaters; } }

        /// <summary>
        /// Used for cleaning all global resources
        /// </summary>
        public event System.Action OnQuit;
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Checks if the scene contains at least one Water component
        /// </summary>
        public static bool IsWaterPossiblyVisible()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { return true; }
#endif

            var instance = Instance;
            if (instance == null)
            {
                return false;
            }

            return instance.Waters.Count != 0;
        }

        public static void Register(Water water)
        {
            RequestId(water);

            var instance = Instance;
            if (instance == null) { return; }

            if (!instance._Waters.Contains(water))
            {
                instance._Waters.Add(water);
            }

            if ((water.Volume == null || water.Volume.Boundless) && !instance._BoundlessWaters.Contains(water))
            {
                instance._BoundlessWaters.Add(water);
            }
        }
        public static void Unregister(Water water)
        {
            FreeId(water);

            var instance = Instance;
            if (instance == null) { return; }

            instance._Waters.Remove(water);
            instance._BoundlessWaters.Remove(water);
        }

        public static Water FindWater(Vector3 position, float radius)
        {
            bool unused1, unused2;
            return FindWater(position, radius, null, out unused1, out unused2);
        }
        public static Water FindWater(Vector3 position, float radius, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
        {
            return FindWater(position, radius, null, out isInsideSubtractiveVolume, out isInsideAdditiveVolume);
        }
        public static Water FindWater(Vector3 position, float radius, List<Water> allowedWaters, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
        {
            isInsideSubtractiveVolume = false;
            isInsideAdditiveVolume = false;

#if UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            var collidersBuffer = Physics.OverlapSphere(position, radius, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
            int numHits = collidersBuffer.Length;
#else
            int numHits = Physics.OverlapSphereNonAlloc(position, radius, _CollidersBuffer, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
#endif

            _PossibleWaters.Clear();
            _ExcludedWaters.Clear();

            for (int i = 0; i < numHits; ++i)
            {
                var volume = WaterVolumeBase.GetWaterVolume(_CollidersBuffer[i]);

                if (volume != null)
                {
                    if (volume is WaterVolumeAdd)
                    {
                        isInsideAdditiveVolume = true;

                        if (allowedWaters == null || allowedWaters.Contains(volume.Water))
                            _PossibleWaters.Add(volume.Water);
                    }
                    else                // subtractive
                    {
                        isInsideSubtractiveVolume = true;
                        _ExcludedWaters.Add(volume.Water);
                    }
                }
            }

            for (int i = 0; i < _PossibleWaters.Count; ++i)
            {
                if (!_ExcludedWaters.Contains(_PossibleWaters[i]))
                    return _PossibleWaters[i];
            }

            var boundlessWaters = WaterSystem.Instance.BoundlessWaters;
            int numBoundlessWaters = boundlessWaters.Count;

            for (int i = 0; i < numBoundlessWaters; ++i)
            {
                var water = boundlessWaters[i];

                if ((allowedWaters == null || allowedWaters.Contains(water)) && water.Volume.IsPointInsideMainVolume(position, radius) && !_ExcludedWaters.Contains(water))
                    return water;
            }

            return null;
        }
        #endregion Public Methods

        #region Unity Methods
        protected override void OnDestroy()
        {
            if (OnQuit != null) { OnQuit.Invoke(); }
            base.OnDestroy();
        }
        #endregion Unity Methods

        #region Private Variables
        private readonly List<Water> _Waters = new List<Water>();
        private readonly List<Water> _BoundlessWaters = new List<Water>();

        private static readonly List<Water> _PossibleWaters = new List<Water>();
        private static readonly List<Water> _ExcludedWaters = new List<Water>();

        private static readonly Collider[] _CollidersBuffer = new Collider[30];

        private static int _CurrentId = 1; // note: Water Id cannot be 0
        private static readonly List<int> _FreeIds = new List<int>();
        #endregion Private Variables

        #region Private Methods
        /// <summary>
        /// Assigned unique identifier to each Water
        /// </summary>
        /// <returns>If the id was changed</returns>
        private static bool RequestId(Water water)
        {
            // if the id is already assigned
            if (water._WaterId != -1) { return false; }

            // if we do not have any free id's
            if (_FreeIds.Count == 0)
            {
                water._WaterId = _CurrentId++;
            }
            else // pop the last one from the list
            {
                var last = _FreeIds.Count - 1;

                water._WaterId = _FreeIds[last];
                _FreeIds.RemoveAt(last);
            }

            return true;
        }

        /// <summary>
        /// Frees previously assigned indentifier
        /// </summary>
        private static void FreeId(Water water)
        {
            // the id was freed already
            if (water._WaterId == -1) { return; }

            // mark the slot free
            _FreeIds.Add(water._WaterId);

            water._WaterId = -1;
        }
        #endregion Private Methods
    }
}