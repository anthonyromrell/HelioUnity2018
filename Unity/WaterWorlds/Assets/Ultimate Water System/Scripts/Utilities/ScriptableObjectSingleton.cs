using UnityEngine;

namespace UltimateWater
{
    public class ScriptableObjectSingleton : ScriptableObject
    {
        protected static T LoadSingleton<T>() where T : ScriptableObject
        {
            var instance = Resources.Load<T>(typeof(T).Name);

#if UNITY_EDITOR
            if (instance == null)
            {
                instance = CreateInstance<T>();

                string path = WaterPackageEditorUtilities.WaterPackagePath + "/Resources/" + typeof(T).Name + ".asset";
                UnityEditor.AssetDatabase.CreateAsset(instance, path);
            }
#endif

            return instance;
        }
    }
}