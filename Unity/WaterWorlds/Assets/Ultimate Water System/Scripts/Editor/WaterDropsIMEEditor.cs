namespace UltimateWater.Editors
{
    using UnityEditor;

    [CustomEditor(typeof(WaterDropsIME))]
    public class WaterDropsIMEEditor : WaterEditorBase
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            PropertyField("Fade");

            EditorGUILayout.Space();
            var type = PropertyField("_Type");

            EditorGUI.indentLevel = 1;
            switch ((WaterDropsIME.Type)type.enumValueIndex)
            {
                case WaterDropsIME.Type.NormalMap:
                {
                    DisplayChildren("Normal");
                    break;
                }
                case WaterDropsIME.Type.Blur:
                {
                    DisplayChildren("Blur");
                    break;
                }
            }

            EditorGUI.indentLevel = 0;
            PropertyField("_Validation");

            serializedObject.ApplyModifiedProperties();
        }
    }
}