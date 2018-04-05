namespace UltimateWater.Editors
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WaterSimulationArea))]
    public class WaterAreaEditor : Editor
    {
        #region Unity Methods
        private void OnEnable()
        {
            _Target = (WaterSimulationArea)target;
            this.AssignAllProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                _Target.Refresh();
            }

            RenderInfoGui();
        }
        #endregion Unity Methods

        #region Private Variables
#pragma warning disable 0649
        private WaterSimulationArea _Target;
#pragma warning restore 0649
        #endregion Private Variables

        #region Private Methods

        private void RenderInfoGui()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Info:");
                EditorGUILayout.Space();

                EditorGUILayout.LabelField(
                    "Texture size: [" + _Target.Resolution.x + "x" + _Target.Resolution.y + "]");

                EditorGUILayout.LabelField("Depth texture size: [" +
                  _Target.DepthResolution.x + "x" + _Target.DepthResolution.y + "]");
            }
            EditorGUILayout.EndVertical();
        }
        #endregion Private Methods
    }
}