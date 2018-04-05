namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Ultimate Water/Water Profile Blend")]
    public class WaterProfileBlend : MonoBehaviour
    {
        #region Inspector Variables
        public Water Water;
        #endregion Inspector Variables

        #region Private Variables
        [SerializeField] private List<WaterProfile> _Profiles = new List<WaterProfile> { null };
        [SerializeField] private List<float> _Weights = new List<float> { 1.0f };
        #endregion Private Variables

        #region Unity Message
        private void Awake()
        {
            // try to find suitable water
            if (Water == null)
            {
                Water = Utilities.GetWaterReference();
                if (Water.IsNullReference(this)) return;
            }

            // validate profile data
            _Profiles.RemoveAll(x => x == null);
            if (_Profiles.Count == 0)
            {
                Debug.LogError("[WaterProfileBlend] : no valid profiles found");
                enabled = false;
            }
        }
        private void Start()
        {
            Water.ProfilesManager.SetProfiles(CreateProfiles(_Profiles, _Weights));
        }

        private void OnValidate()
        {
            if (Application.isPlaying && Water != null && Water.WindWaves != null)
            {
                Water.ProfilesManager.SetProfiles(CreateProfiles(_Profiles, _Weights));
            }

            if (WeightSum(_Weights) == 0.0f)
            {
                _Weights[0] = 1.0f;
            }
        }
        private void Reset()
        {
            if (Water == null)
            {
                Water = Utilities.GetWaterReference();
            }
        }
        #endregion Unity Message

        #region Private Methods
        private static Water.WeightedProfile[] CreateProfiles(List<WaterProfile> profiles, List<float> weights)
        {
            // the sum of the weights needs to be in <0, 1> range
            var normalized = NormalizeWeights(weights);

            int count = profiles.Count;
            var result = new Water.WeightedProfile[count];
            for (int i = 0; i < count; ++i)
            {
                result[i] = new Water.WeightedProfile(profiles[i], normalized[i]);
            }

            return result;
        }
        private static float WeightSum(List<float> weights)
        {
            float result = 0.0f;
            for (int i = 0; i < weights.Count; ++i)
            {
                result += weights[i];
            }
            return result;
        }

        private static List<float> NormalizeWeights(List<float> weights)
        {
            List<float> result = new List<float>(weights.Count);

            var sum = WeightSum(weights);
            for (int i = 0; i < weights.Count; ++i)
            {
                result.Add(weights[i] / sum);
            }

            return result;
        }
        #endregion Private Methods
    }
}