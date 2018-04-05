namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.AnimatedValues;
    using System.Collections.Generic;

    public class WaterEditorBase : Editor
    {
        #region Private Types
        protected struct WaterMap
        {
            public readonly string Name;
            public readonly System.Func<Texture> Getter;

            public WaterMap(string name, System.Func<Texture> getter)
            {
                Name = name;
                Getter = getter;
            }
        }
        #endregion Private Types

        #region Private Variables
        private GUIStyle _HeaderStyle;
        private GUIStyle _GroupStyle;
        private GUIStyle _TextureBoxStyle;
        private GUIStyle _TextureLabelStyle;
        private GUIStyle _TextureBox;

        private static Material _InspectMaterial;
        private readonly Stack<bool> _Foldouts = new Stack<bool>();
        private float _InspectMinValue;
        private float _InspectMaxValue = 1.0f;
        protected bool _UseFoldouts;

        private readonly Stack<SerializedProperty> _ProperiesStack = new Stack<SerializedProperty>();

        #endregion Private Variables

        #region Private Methods
        protected void Push(string name)
        {
            if (_ProperiesStack.Count == 0)
            {
                _ProperiesStack.Push(serializedObject.FindProperty(name));
                return;
            }

            _ProperiesStack.Push(_ProperiesStack.Peek().FindPropertyRelative(name));
        }
        protected void ClearStack()
        {
            _ProperiesStack.Clear();
        }
        protected void Pop()
        {
            _ProperiesStack.Pop();
        }

        protected SerializedProperty GetProperty(string name)
        {
            if (_ProperiesStack.Count == 0)
            {
                return serializedObject.FindProperty(name);
            }
            return _ProperiesStack.Peek().FindPropertyRelative(name);
        }

        protected virtual void UpdateValues()
        {
        }

        protected virtual void UpdateStyles()
        {
            if (_HeaderStyle == null)
            {
                _HeaderStyle = new GUIStyle(EditorStyles.foldout)
                {
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 10,
                    stretchHeight = false
                };
            }

            if (_GroupStyle == null)
            {
                _GroupStyle = new GUIStyle
                {
                    margin = new RectOffset(0, 0, 0, 0)
                };
            }

            if (_TextureBoxStyle == null)
            {
                _TextureBoxStyle = new GUIStyle(GUI.skin.box)
                {
                    margin = new RectOffset(GUI.skin.label.margin.left + 2, 0, 0, 0)
                };
            }

            if (_TextureLabelStyle == null)
            {
                _TextureLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleLeft
                };
            }
        }

        protected void UpdateGui()
        {
            if (Event.current.type == EventType.Layout)
                UpdateValues();

            UpdateStyles();
        }

        protected bool BeginGroup(string label, AnimBool anim, float space = 0)
        {
            if (_HeaderStyle == null)
                UpdateStyles();

            GUILayout.Space(4);

            if (!_UseFoldouts)
            {
                GUILayout.Label(label, EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(_GroupStyle);
                return true;
            }
            else
            {
                if (anim.isAnimating)
                    Repaint();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
                anim.target = EditorGUILayout.Foldout(anim.target, label, _HeaderStyle);

                if (EditorGUILayout.BeginFadeGroup(anim.faded))
                {
                    EditorGUILayout.BeginVertical(_GroupStyle);
                    _Foldouts.Push(true);
                    return true;
                }

                _Foldouts.Push(false);
                return false;
            }
        }

        protected void EndGroup()
        {
            if (!_UseFoldouts)
            {
                GUILayout.Space(6);
                EditorGUILayout.EndVertical();
            }
            else
            {
                if (_Foldouts.Pop())
                {
                    GUILayout.Space(6);
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndFadeGroup();
                EditorGUILayout.EndHorizontal();
            }
        }

        protected static void MaterialFloatSlider(Material material, string label, string property, float spaceLeft = 0, float spaceRight = 0, float min = 0.0f, float max = 1.0f)
        {
            MaterialFloatSlider(material, new GUIContent(label), property, spaceLeft, spaceRight, min, max);
        }

        protected static void MaterialFloatSlider(Material material, GUIContent label, string property, float spaceLeft = 0, float spaceRight = 0, float min = 0.0f, float max = 1.0f)
        {
            EditorGUILayout.BeginHorizontal();

            if (spaceLeft != 0)
                GUILayout.Space(spaceLeft);

            float val = material.GetFloat(property);
            float newValue = EditorGUILayout.Slider(label, val, min, max);

            if (val != newValue)
                material.SetFloat(property, newValue);

            if (spaceRight != 0)
                GUILayout.Space(spaceRight);

            EditorGUILayout.EndHorizontal();
        }

        protected bool MaterialFloat(Material material, string name, string label, float leftSpace = 0, float rightSpace = 0)
        {
            return MaterialFloat(material, name, new GUIContent(label), leftSpace, rightSpace);
        }

        protected bool MaterialFloat(Material material, string name, GUIContent label, float leftSpace = 0, float rightSpace = 0)
        {
            EditorGUILayout.BeginHorizontal();

            if (leftSpace != 0)
                GUILayout.Space(leftSpace);

            float val = material.GetFloat(name);
            float newVal = EditorGUILayout.FloatField(label, val);

            if (rightSpace != 0)
                GUILayout.Space(rightSpace);

            EditorGUILayout.EndHorizontal();

            if (newVal != val)
            {
                material.SetFloat(name, newVal);
                return true;
            }

            return false;
        }

        protected bool MaterialColor(Material material, string name, string label, float space = 0, bool showAlpha = true)
        {
            return MaterialColor(material, name, new GUIContent(label), space, showAlpha);
        }

        protected bool MaterialColor(Material material, string name, GUIContent label, float space = 0, bool showAlpha = true)
        {
            EditorGUILayout.BeginHorizontal();

            if (space != 0)
                GUILayout.Space(space);

            var color = material.GetColor(name);
            var newColor = EditorGUILayout.ColorField(label, color, true, showAlpha, false, new ColorPickerHDRConfig(0.0f, 1.0f, 0.0f, 1.0f));

            EditorGUILayout.EndHorizontal();

            if (newColor != color)
            {
                material.SetColor(name, newColor);
                return true;
            }

            return false;
        }

        protected SerializedProperty SubPropertyField(string topProperty, string subProperty, string label, float space = 0)
        {
            if (space != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
            }

            var prop = GetProperty(topProperty);
            var property = prop.FindPropertyRelative(subProperty);
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip), true);

            if (space != 0)
                EditorGUILayout.EndHorizontal();

            return property;
        }

        protected SerializedProperty SubPropertyField(SerializedProperty baseProperty, string topProperty, string subProperty, string label, float space = 0)
        {
            if (space != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
            }

            var prop = baseProperty.FindPropertyRelative(topProperty);
            var property = prop.FindPropertyRelative(subProperty);
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip), true);

            if (space != 0)
                EditorGUILayout.EndHorizontal();

            return property;
        }

        protected SerializedProperty SubSubPropertyField(string topProperty, string midProperty, string subProperty, string label, float space = 0)
        {
            if (space != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
            }

            var prop = GetProperty(topProperty).FindPropertyRelative(midProperty);
            var property = prop.FindPropertyRelative(subProperty);
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip), true);

            if (space != 0)
                EditorGUILayout.EndHorizontal();

            return property;
        }

        protected void DisplayChildren(string name)
        {
            var property = GetProperty(name);
            var children = property.GetChildren();

            foreach (var child in children)
            {
                EditorGUILayout.PropertyField(child, true);
            }
        }

        protected SerializedProperty PropertyField(string name)
        {
            var property = GetProperty(name);
            EditorGUILayout.PropertyField(property, true);

            return property;
        }

        protected SerializedProperty PropertyField(SerializedProperty baseProperty, string name)
        {
            var property = baseProperty.FindPropertyRelative(name);
            EditorGUILayout.PropertyField(property, true);

            return property;
        }

        protected SerializedProperty PropertyField(string name, string label, float space = 0)
        {
            if (space != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
            }

            var property = GetProperty(name);
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip), true);

            if (space != 0)
                EditorGUILayout.EndHorizontal();

            return property;
        }

        protected SerializedProperty PropertyField(SerializedProperty baseProperty, string name, string label, float space = 0)
        {
            if (space != 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(space);
            }

            var property = baseProperty.FindPropertyRelative(name);
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip), true);

            if (space != 0)
                EditorGUILayout.EndHorizontal();

            return property;
        }

        protected void DisplayTextureInspector(Texture texture)
        {
            if (_TextureBox == null)
            {
                _TextureBox = new GUIStyle(GUI.skin.box)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = EditorStyles.boldLabel.normal.textColor },
                    active = { textColor = EditorStyles.boldLabel.normal.textColor },
                    focused = { textColor = EditorStyles.boldLabel.normal.textColor }
                };
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Box(texture != null ? "" : "NOT AVAILABLE", _TextureBox, GUILayout.Width(Screen.width * 0.85f), GUILayout.Height(Screen.width * 0.85f));
                var texRect = GUILayoutUtility.GetLastRect();

                if (texture != null && Event.current.type == EventType.Repaint)
                {
                    if (_InspectMaterial == null)
                    {
                        _InspectMaterial = new Material(Shader.Find("UltimateWater/Editor/Inspect Texture"))
                        {
                            hideFlags = HideFlags.DontSave
                        };
                    }

                    _InspectMaterial.SetVector("_RangeR", new Vector4(_InspectMinValue, 1.0f / (_InspectMaxValue - _InspectMinValue)));
                    _InspectMaterial.SetVector("_RangeG", new Vector4(_InspectMinValue, 1.0f / (_InspectMaxValue - _InspectMinValue)));
                    _InspectMaterial.SetVector("_RangeB", new Vector4(_InspectMinValue, 1.0f / (_InspectMaxValue - _InspectMinValue)));
                    Graphics.DrawTexture(texRect, texture, _InspectMaterial);
                    Repaint();
                }

                GUILayout.FlexibleSpace();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.MinMaxSlider(ref _InspectMinValue, ref _InspectMaxValue, 0.0f, 1.0f);
        }
        #endregion Private Methods
    }
}