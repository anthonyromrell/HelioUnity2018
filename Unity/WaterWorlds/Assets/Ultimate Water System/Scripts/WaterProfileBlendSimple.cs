namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    /// Simpler version of the WaterProfileBlend with only two profiles and
    /// one variable to blend between them
    /// </summary>
    [AddComponentMenu("Ultimate Water/Water Profile Blend (simple)")]
    public class WaterProfileBlendSimple : MonoBehaviour
    {
        #region Public Variables
        public Water Water
        {
            get { return _Water; }
            set { _Water = value; UpdateProfiles(); }
        }

        public WaterProfile First
        {
            get { return _First; }
            set { _First = value; UpdateProfiles(); }
        }

        public WaterProfile Second
        {
            get { return _Second; }
            set { _Second = value; UpdateProfiles(); }
        }

        /// <summary>
        /// Blends between First and Second Profile, 0 - First, 1 - Second,
        /// the value is clamped 0-1 inclusive.
        /// </summary>
        public float Factor
        {
            get { return _Factor; }
            set
            {
                _Factor = Mathf.Clamp01(value);
                UpdateProfiles();
            }
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Water _Water;
        [Header("Profiles")]
        [SerializeField]
        private WaterProfile _First;
        [SerializeField] private WaterProfile _Second;

        [Header("Blend:"), Range(0.0f, 1.0f), SerializeField]
        private float _Factor;
        #endregion Inspector Variables

        #region Unity Message
        private void Awake()
        {
            // try to find suitable water
            if (Water == null) { Water = Utilities.GetWaterReference(); }
        }
        private void Start()
        {
            UpdateProfiles();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying || Water == null || Water.WindWaves == null) { return; }

            UpdateProfiles();
        }
        private void Reset()
        {
            if (Water == null) { Water = Utilities.GetWaterReference(); }
        }
        #endregion Unity Message

        #region Private Variables
        private readonly Water.WeightedProfile[] _Profiles = new Water.WeightedProfile[2];
        #endregion Private Variables

        #region Private Methods
        private void UpdateProfiles()
        {
            if (Water == null || First == null || Second == null) { return; }

            _Profiles[0] = new Water.WeightedProfile(First, 1.0f - Factor);
            _Profiles[1] = new Water.WeightedProfile(Second, Factor);

            Water.ProfilesManager.SetProfiles(_Profiles);
        }
        #endregion Private Methods
    }
}