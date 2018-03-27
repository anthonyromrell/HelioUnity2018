namespace UltimateWater
{
    using UnityEngine;

#if UNITY_EDITOR

    using UnityEditor;

#endif

    [System.Serializable]
    public class WaterProfile : ScriptableObject
    {
        #region Public Variables
        public WaterProfileData Data
        {
            get { return _Data; }
        }
        #endregion Public Variables

        #region Private Variables
        [SerializeField] private WaterProfileData _Data = new WaterProfileData();
        #endregion Private Variables

        #region Editor Methods
#if UNITY_EDITOR
        [MenuItem("Assets/Create/UltimateWater/Profile")]
        public static void CreateProfile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (System.IO.Path.GetExtension(path) != "")
            {
                var filePath = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
                if (filePath != null)
                {
                    path = path.Replace(filePath, "");
                }
            }

            var bundle = CreateInstance<WaterProfile>();
            bundle.Data.TemplateProfile = bundle;

            AssetDatabase.CreateAsset(bundle, AssetDatabase.GenerateUniqueAssetPath(path + "/New Water Profile.asset"));
            AssetDatabase.SaveAssets();

            Selection.activeObject = bundle;
        }
#endif
        #endregion Editor Methods
    }
}