namespace UltimateWater
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class ProfilesManager
    {
        #region Public Variables
        /// <summary>
        /// Currently set water profiles with their associated weights.
        /// </summary>
        public Water.WeightedProfile[] Profiles { get; private set; }

        public Water.WaterEvent Changed
        {
            get { return _Changed; }
        }
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Caches profiles for later use to avoid hiccups.
        /// </summary>
        /// <param name="profiles"></param>
        public void CacheProfiles(params WaterProfileData[] profiles)
        {
            var windWaves = _Water.WindWaves;

            if (windWaves != null)
            {
                for (int i = 0; i < profiles.Length; ++i)
                {
                    windWaves.SpectrumResolver.CacheSpectrum(profiles[i].Spectrum);
                }
            }
        }

        /// <summary>
        /// Lets you quickly evaluate a chosen property from the water profiles by using a lambda expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public float EvaluateProfilesParameter(Func<WaterProfileData, float> func)
        {
            float sum = 0.0f;

            var profiles = Profiles;

            for (int i = profiles.Length - 1; i >= 0; --i)
            {
                sum += func(profiles[i].Profile) * profiles[i].Weight;
            }

            return sum;
        }

        /// <summary>
        /// Sets water profiles with custom weights.
        /// </summary>
        /// <param name="profiles"></param>
        public void SetProfiles(params Water.WeightedProfile[] profiles)
        {
            for (int i = 0; i < profiles.Length; ++i)
            {
                CacheProfiles(profiles[i].Profile);
            }

            CheckProfiles(profiles);

            Profiles = profiles;
            _ProfilesDirty = true;
        }

        /// <summary>
        /// Instantly populates all water properties from the used profiles. Normally it's delayed and done on each update.
        /// </summary>
        public void ValidateProfiles()
        {
            bool valuesDirty = false;
            foreach (var profile in Profiles)
            {
                valuesDirty |= profile.Profile.Dirty;
            }

            if (valuesDirty || _ProfilesDirty)
            {
                _ProfilesDirty = false;
                _Changed.Invoke(_Water);
            }
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("initialProfile")]
        private WaterProfile _InitialProfile;

        [SerializeField, FormerlySerializedAs("changed")]
        private Water.WaterEvent _Changed = new Water.WaterEvent();
        #endregion Inspector Variables

        #region Private Variables
        private Water _Water;
        private bool _ProfilesDirty;
        private WaterProfile _InitialProfileCopy;
        #endregion Private Variables

        #region Private Methods
        internal void Awake(Water water)
        {
            _Water = water;

            if (_Changed == null)
            {
                _Changed = new Water.WaterEvent();
            }

#if UNITY_EDITOR
            if (_InitialProfile == null)
                _InitialProfile = WaterPackageEditorUtilities.FindDefaultAsset<WaterProfile>("\"Sea - 6. Strong Breeze\" t:WaterProfile", "t:WaterProfile");
#endif

            if (Profiles == null)
            {
                if (_InitialProfile != null)
                    SetProfiles(new Water.WeightedProfile(_InitialProfile, 1.0f));
                else
                    Profiles = new Water.WeightedProfile[0];
            }

            WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
            WaterQualitySettings.Instance.Changed += OnQualitySettingsChanged;
        }

        internal void OnEnable()
        {
            _ProfilesDirty = true;
        }

        internal void OnDisable()
        {
            _ProfilesDirty = false;
        }

        internal void OnDestroy()
        {
            WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
        }

        internal void Update()
        {
            ValidateProfiles();
        }

        internal void OnValidate()
        {
            if (Profiles != null && Profiles.Length != 0 && (_InitialProfileCopy == _InitialProfile || _InitialProfileCopy == null))
            {
                _InitialProfileCopy = _InitialProfile;
                _ProfilesDirty = true;
            }
            else if (_InitialProfile != null)
            {
                _InitialProfileCopy = _InitialProfile;
                Profiles = new[] { new Water.WeightedProfile(_InitialProfile, 1.0f) };
                _ProfilesDirty = true;
            }
        }

        private void OnQualitySettingsChanged()
        {
            _ProfilesDirty = true;
        }

        /// <summary>
        /// Ensures that profiles are fine to use.
        /// </summary>
        /// <param name="profiles"></param>
        private static void CheckProfiles(IList<Water.WeightedProfile> profiles)
        {
            if (profiles == null)
            {
                return;
            }

            if (profiles.Count == 0)
                throw new ArgumentException("Water has to use at least one profile.");

            float tileSize = profiles[0].Profile.TileSize;

            for (int i = 1; i < profiles.Count; ++i)
            {
                if (profiles[i].Profile.TileSize != tileSize)
                {
                    Debug.LogError("TileSize varies between used water profiles. It is the only parameter that you should keep equal on all profiles used at a time.");
                    break;
                }
            }
        }
        #endregion Private Methods
    }
}