using System.Collections.Generic;

namespace UltimateWater.Editor
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(WaterRipplesProfile))]
    public class WaterRipplesProfileDrawer : PropertyDrawer
    {
        #region Public Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selected = property.objectReferenceValue as WaterRipplesProfile;

            // if there is no profile selected
            if (selected == null)
            {
                // autoselect first if there are any profiles created
                var all = FindAssetsByType<WaterRipplesProfile>();
                if (all.Count != 0)
                {
                    property.objectReferenceValue = all[0];
                }
            }

            EditorGUI.PropertyField(position, property);
        }
        #endregion Public Methods

        #region Helper Methods
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            var assets = new List<T>();
            var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
        #endregion Helper Methods
    }
}