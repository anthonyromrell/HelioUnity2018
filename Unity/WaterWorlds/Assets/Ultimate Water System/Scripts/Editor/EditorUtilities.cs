using UltimateWater.Internal;

namespace UltimateWater.Editors
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;

    public static class WaterEditorUtilities
    {
        #region Public Variables
        public static GUIStyle SelectionColor
        {
            get
            {
                if (_SelectedLevel != null) { return _SelectedLevel; }

                var texture = CreateTexture(EditorGUIUtility.isProSkin ?
                    new Color32(72, 72, 72, 255) : new Color32(255, 255, 255, 255));

                _SelectedLevel = new GUIStyle(GUI.skin.label)
                {
                    normal = { background = texture }
                };
                return _SelectedLevel;
            }
        }
        public static GUIStyle SeparatorColor
        {
            get
            {
                if (_Separator != null) { return _Separator; }

                var texture = CreateTexture(EditorGUIUtility.isProSkin ?
                    new Color32(144, 144, 144, 255) : new Color32(255, 255, 255, 255));

                _Separator = new GUIStyle
                {
                    normal = { background = texture },
                    stretchWidth = true,
                    fixedHeight = 1
                };
                return _Separator;
            }
        }
        #endregion Public Variables

        #region Public Methods
        public static void AssignAllProperties(this Editor obj)
        {
            var fields = obj.GetType().GetFields(
              BindingFlags.FlattenHierarchy |
              BindingFlags.NonPublic |
              BindingFlags.Public |
              BindingFlags.Static | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(SerializedProperty))
                {
                    var found = obj.serializedObject.FindProperty(field.Name);
                    if (found == null) continue;

                    field.SetValue(obj, found);
                }
            }
        }
        public static void AssignProperty(this Editor obj, string name)
        {
            var fields = obj.GetType().GetFields(
                BindingFlags.FlattenHierarchy |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.Name == name)
                {
                    field.SetValue(obj, obj.serializedObject.FindProperty(name));
                }
            }
        }

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property)
        {
            property = property.Copy();
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement)
            {
                nextElement = null;
            }

            property.NextVisible(true);
            while (true)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext)
                {
                    break;
                }
            }
        }
        #endregion Public Methods

        #region Private Variables
        private static GUIStyle _SelectedLevel;
        private static GUIStyle _Separator;
        #endregion Private Variables

        #region Helper Methods
        private static Texture2D CreateTexture(Color color)
        {
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true)
            {
                hideFlags = HideFlags.DontSave
            };

            FillTexture(texture, color);
            return texture;
        }
        private static void FillTexture(Texture2D tex, Color color)
        {
            for (int x = 0; x < tex.width; ++x)
            {
                for (int y = 0; y < tex.height; ++y)
                {
                    tex.SetPixel(x, y, color);
                }
            }

            tex.Apply();
        }
        #endregion Helper Methods
    }
}