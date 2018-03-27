namespace UltimateWater
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using UnityEngine;

    public class WaterRipplesProfile : ScriptableObject
    {
        #region Public Variables
        [Header("Settings")]
        [Range(0.0f, 1.0f)]
        [Tooltip("How fast wave amplitude decreases with time")]
        public float Damping = 0.3f;

        [Range(0.0f, 1.0f)]
        [Tooltip("How fast the waves spread")]
        public float Propagation = 1.0f;

        [Tooltip("Force inflicted by interacting objects")]
        public float Gain = 30.0f;

        [Tooltip("Wave amplitude decrease with depth")]
        public float HeightGain = 2.0f;

        [Tooltip("Wave amplitude decrease offset")]
        public float HeightOffset = 2.0f;

        [Tooltip("Wave height multiplier")]
        public float Amplitude = 1.0f;

        [Header("Smooth")]
        [Range(0.0f, 1.0f)]
        [Tooltip("How much smoothing is applied between iterations")]
        public float Sigma;

        [Header("Normals")]
        [Tooltip("How strong are wave normals")]
        public float Multiplier = 1.0f;

        [Tooltip("How wide is sampling distance for normal calculations")]
        public float Spread = 0.001f;
        #endregion Public Variables

        #region Unity Messages
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var areas = FindObjectsOfType<WaterSimulationArea>();
            foreach (var entry in areas)
            {
                entry.UpdateShaderVariables();
            }
        }
        #endregion Unity Messages

        #region Editor Methods
#if UNITY_EDITOR
        [MenuItem("Assets/Create/UltimateWater/Ripples Profile")]
        public static void CreateProfile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (System.IO.Path.GetExtension(path) != "")
            {
                var assetPath = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
                if (assetPath != null) { path = path.Replace(assetPath, ""); }
            }

            var bundle = CreateInstance<WaterRipplesProfile>();

            AssetDatabase.CreateAsset(bundle, AssetDatabase.GenerateUniqueAssetPath(path + "/New Water Ripples Profile.asset"));
            AssetDatabase.SaveAssets();

            Selection.activeObject = bundle;
        }
#endif
        #endregion Editor Methods
    }
}