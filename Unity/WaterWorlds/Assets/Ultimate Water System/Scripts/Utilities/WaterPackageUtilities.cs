namespace UltimateWater
{
#if UNITY_EDITOR

    using System.IO;

    /// <summary>
    /// Helps locating the UltimateWater folder and find stuff in it.
    /// </summary>
    public class WaterPackageEditorUtilities
    {
        #region Public Variables
        public static string WaterPackagePath
        {
            get
            {
                return _WaterPackagePath != null ? _WaterPackagePath : (_WaterPackagePath = Find("Assets" + Path.DirectorySeparatorChar, ""));
            }
        }
        #endregion Public Variables

        #region Public Methods
        public static T FindDefaultAsset<T>(string searchString, string searchStringFallback) where T : UnityEngine.Object
        {
            var guids = UnityEditor.AssetDatabase.FindAssets(searchString);

            if (guids.Length == 0)
                guids = UnityEditor.AssetDatabase.FindAssets(searchStringFallback);

            if (guids.Length == 0)
                return null;

            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly string _WaterSpecificPath = "Ultimate Water System" + Path.DirectorySeparatorChar + "Graphics";
        private static string _WaterPackagePath;
        #endregion Private Variables

        #region Private Methods
        private static string Find(string directory, string parentDirectory)
        {
            if (directory.EndsWith(_WaterSpecificPath))
                return parentDirectory.Replace(Path.DirectorySeparatorChar, '/');

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                string result = Find(subDirectory, directory);

                if (result != null)
                    return result;
            }

            return null;
        }
        #endregion Private Methods
    }
#endif
}