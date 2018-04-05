namespace UltimateWater.Utils
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Description))]
    public class DescriptionEditor : Editor
    {
        #region Private Variables
        private readonly GUIStyle _Style = new GUIStyle();

        private static readonly Color _CommentColorPro = new Color(0.5f, 0.7f, 0.3f, 1.0f);
        private static readonly Color _CommentColorFree = new Color(0.2f, 0.3f, 0.1f, 1.0f);

        private static readonly Color _EditableColor = new Color(0.7f, 0.3f, 1.0f, 1.0f);
        #endregion Private Variables

        #region Unity Methods
        public sealed override void OnInspectorGUI()
        {
            var handle = (Description)target;
            _Style.wordWrap = true;

            if (Description.Editable)
            {
                _Style.normal.textColor = _EditableColor;
                handle.Text = EditorGUILayout.TextArea(handle.Text, _Style);
            }
            else
            {
                _Style.normal.textColor = EditorGUIUtility.isProSkin ? _CommentColorPro : _CommentColorFree;
                EditorGUILayout.LabelField(handle.Text, _Style);
            }
        }
        #endregion Unity Methods
    }
}