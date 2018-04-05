namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(StaticWaterInteraction))]
    public class StaticWaterInteractionEditor : WaterEditorBase
    {
        #region Unity Methods
        public override void OnInspectorGUI()
        {
            UpdateStyles();

            var underwaterAreasModeProp = PropertyField("underwaterAreasMode");
            PropertyField("hasBottomFaces", "Mesh Has Bottom Faces");
            PropertyField("waveDampingThreshold", "Wave Damping Threshold (Scene Units)");
            PropertyField("depthScale", "Depth Scale");

            if (((StaticWaterInteraction.UnderwaterAreasMode)underwaterAreasModeProp.enumValueIndex) == StaticWaterInteraction.UnderwaterAreasMode.Generate)
                DrawShoreAngleProperty();

            DrawIntensityMask();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion Unity Methods

        #region Private Variables
        private GUIStyle _BoxStyle;
        #endregion Private Variables

        #region Private Methods
        protected override void UpdateStyles()
        {
            if (_BoxStyle == null)
            {
                _BoxStyle = new GUIStyle(GUI.skin.box)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                if (EditorGUIUtility.isProSkin)
                    _BoxStyle.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        private void DrawShoreAngleProperty()
        {
            var shoreSmoothness = PropertyField("shoreSmoothness", "Shore Smoothnes (Degrees)").floatValue;
            string type;

            if (shoreSmoothness <= 1.0f)
                type = "Cliff";
            else if (shoreSmoothness < 8.0f)
                type = "Coast";
            else if (shoreSmoothness < 35.0f)
                type = "Beach (Steep)";
            else
                type = "Beach (Gentle)";

            EditorGUILayout.LabelField("Type", type);
        }

        private void DrawIntensityMask()
        {
            GUILayout.Space(6);

            var current = (StaticWaterInteraction)target;
            if (current == null) { return; }

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Box(current.IntensityMask != null ? "" : "NOT AVAILABLE", _BoxStyle, GUILayout.Width(Screen.width * 0.6f), GUILayout.Height(Screen.width * 0.6f));
                Rect texRect = GUILayoutUtility.GetLastRect();

                if (current.IntensityMask != null && Event.current.type == EventType.Repaint)
                {
                    Graphics.DrawTexture(texRect, current.IntensityMask);
                    Repaint();
                }

                GUILayout.FlexibleSpace();
            }

            GUILayout.EndHorizontal();
        }
        #endregion Private Methods
    }
}