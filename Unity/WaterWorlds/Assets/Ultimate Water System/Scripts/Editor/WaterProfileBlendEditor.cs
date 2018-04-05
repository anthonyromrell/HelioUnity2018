namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(WaterProfileBlend))]
    public class WaterProfileBlendEditor : WaterEditorBase
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            PropertyField("Water");
            EditorGUILayout.Space();

            var profilesProperty = GetProperty("_Profiles");
            var weightsProperty = GetProperty("_Weights");

            int count = profilesProperty.arraySize;
            for (int i = 0; i < count; ++i)
            {
                var profile = profilesProperty.GetArrayElementAtIndex(i);
                var weight = weightsProperty.GetArrayElementAtIndex(i);

                GUILayout.BeginHorizontal();
                profile.objectReferenceValue = EditorGUILayout.ObjectField(profile.objectReferenceValue, typeof(WaterProfile), false);
                weight.floatValue = EditorGUILayout.Slider(weight.floatValue, 0.0f, 1.0f, GUILayout.MinWidth(150.0f));

                if (count > 1)
                {
                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(40.0f)))
                    {
                        profilesProperty.GetArrayElementAtIndex(i).objectReferenceValue = null;

                        profilesProperty.DeleteArrayElementAtIndex(i);
                        weightsProperty.DeleteArrayElementAtIndex(i);

                        break;
                    }
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("+", EditorStyles.miniButton))
            {
                profilesProperty.InsertArrayElementAtIndex(count);
                weightsProperty.InsertArrayElementAtIndex(count);

                weightsProperty.GetArrayElementAtIndex(count).floatValue = count == 0 ? 1.0f : 0.0f;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}