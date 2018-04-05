namespace UltimateWater.Editors
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WaterQualitySettings))]
    public class WaterQualitySettingsEditor : WaterEditorBase
    {
        #region Unity Methods
        public override void OnInspectorGUI()
        {
            UpdateGui();

            var qualitySettings = (WaterQualitySettings)target;
            if (Event.current.type == EventType.Layout)
            {
                qualitySettings.SynchronizeQualityLevel();
            }

            DrawLevelSettings();
            DrawCurrentLevelGui();
            DrawRippleSettings();

            if (serializedObject.ApplyModifiedProperties())
            {
                WaterQualitySettings.Instance.SetQualityLevel(WaterQualitySettings.Instance.GetQualityLevel());
            }
        }
        #endregion Unity Methods

        #region Private Methods
        [MenuItem("Edit/Project Settings/Water Quality")]
        public static void OpenQualitySettings()
        {
            var instance = WaterQualitySettings.Instance;
            Selection.activeObject = instance;
        }

        private void DrawLevelSettings()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Levels", EditorStyles.boldLabel);

                    var qualityLevelsProp = serializedObject.FindProperty("_QualityLevels");
                    int numQualityLevels = qualityLevelsProp.arraySize;

                    for (int levelIndex = 0; levelIndex < numQualityLevels; ++levelIndex)
                    {
                        DrawLevelGui(levelIndex, qualityLevelsProp.GetArrayElementAtIndex(levelIndex));
                    }

                    GUILayout.Space(10);

                    if (GUILayout.Button("Open Unity Settings"))
                    {
                        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Quality");
                    }

                    GUILayout.Space(10);

                    DrawGeneralOptionsGui();
                }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("", WaterEditorUtilities.SeparatorColor);
        }
        private void DrawCurrentLevelGui()
        {
            int qualityLevelIndex = WaterQualitySettings.Instance.GetQualityLevel();
            if (qualityLevelIndex == -1)
            {
                return;
            }

            GUILayout.Space(10);
            GUILayout.Label("Water", EditorStyles.boldLabel);

            var currentLevelProp = serializedObject.FindProperty("_QualityLevels").GetArrayElementAtIndex(qualityLevelIndex);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("Name"));
            GUI.enabled = true;

            if (BeginGroup("Spectrum", null))
            {
                WaterEditor.DrawResolutionGui(currentLevelProp.FindPropertyRelative("MaxSpectrumResolution"), "Max Resolution");
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("AllowHighPrecisionTextures"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("TileSizeScale"));
            }

            EndGroup();

            if (BeginGroup("Simulation", null))
            {
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("WavesMode"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("AllowHighQualityNormalMaps"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("AllowSpray"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("FoamQuality"));
            }

            EndGroup();

            if (BeginGroup("Shader", null))
            {
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("MaxTesselationFactor"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("MaxVertexCount"));
                EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("MaxTesselatedVertexCount"));

                // TODO: disabled for now, needs documentation and use-cases
                //EditorGUILayout.PropertyField(currentLevelProp.FindPropertyRelative("AllowAlphaBlending"));
            }

            EndGroup();
        }
        private void DrawRippleSettings()
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Label("Dynamic waves simulation", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            DisplayChildren("Ripples");
        }

        private void DrawLevelGui(int index, SerializedProperty property)
        {
            var propertyName = property.FindPropertyRelative("Name").stringValue;

            var qualitySettings = WaterQualitySettings.Instance;
            var style = WaterQualitySettings.Instance.GetQualityLevel() == index ? WaterEditorUtilities.SelectionColor : GUI.skin.label;

            if (GUILayout.Button(propertyName, style, GUILayout.Width(180)))
            {
                if (qualitySettings.SynchronizeWithUnity)
                    QualitySettings.SetQualityLevel(index);

                WaterQualitySettings.Instance.SetQualityLevel(index);
            }
        }

        private void DrawGeneralOptionsGui()
        {
            var syncWithUnityProp = serializedObject.FindProperty("_SynchronizeWithUnity");
            EditorGUILayout.PropertyField(syncWithUnityProp);
        }
        #endregion Private Methods
    }
}