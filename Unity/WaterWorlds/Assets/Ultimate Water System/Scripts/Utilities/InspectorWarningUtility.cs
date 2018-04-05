namespace UltimateWater.Utils
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using UnityEngine;

    public static class InspectorWarningUtility
    {
#if UNITY_EDITOR
        #region Public Variables

        public static readonly GUIStyle Style = new GUIStyle(EditorStyles.helpBox);

        public static readonly Color NoteColor = new Color(0.7f, 0.7f, 0.3f, 1.0f);
        public static readonly Color WarningColor = new Color(0.9f, 0.6f, 0.2f, 1.0f);
        public static readonly Color ErrorColor = new Color(0.6f, 0.2f, 0.2f, 1.0f);
        #endregion Public Variables

        #region Public Methods
        public static Color GetColor(InspectorWarningAttribute.InfoType type)
        {
            switch (type)
            {
                case InspectorWarningAttribute.InfoType.Note: return NoteColor;
                case InspectorWarningAttribute.InfoType.Warning: return WarningColor;
                case InspectorWarningAttribute.InfoType.Error: return ErrorColor;
                default: return Color.clear;
            }
        }

        public static void WarningField(string info, InspectorWarningAttribute.InfoType type)
        {
            SetStyle();
            Style.normal.textColor = GetColor(type);
            EditorGUILayout.LabelField(info, Style);
        }

        public static void WarningField(Rect rect, string info, InspectorWarningAttribute.InfoType type)
        {
            SetStyle();
            Style.normal.textColor = GetColor(type);

            EditorGUI.LabelField(rect, info, Style);
        }

        #endregion Public Methods

        #region Private Methods
        private static void SetStyle()
        {
            Style.fontSize = 11;
            Style.padding = new RectOffset(4, 4, 4, 4);
            Style.fontStyle = FontStyle.Bold;
        }

        #endregion Private Methods
#endif
    }
}