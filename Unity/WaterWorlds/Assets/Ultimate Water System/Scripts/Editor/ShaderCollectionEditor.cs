namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ShaderSet))]
    public class ShaderCollectionEditor : Editor
    {
        #region Unity Methods
        public override void OnInspectorGUI()
        {
            if (_TemporaryShaderCollection == null)
            {
                _TemporaryShaderCollection = CreateInstance<ShaderSet>();
                EditorUtility.CopySerialized(target, _TemporaryShaderCollection);
            }

            CreateCachedEditor(_TemporaryShaderCollection, typeof(Editor), ref _NestedEditor);
            _NestedEditor.OnInspectorGUI();

            if (GUI.changed)
                _Modified = true;

            if (_Modified)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Apply changes", GUILayout.Width(140.0f)))
                {
                    EditorUtility.CopySerialized(_TemporaryShaderCollection, target);

                    var shaderCollection = (ShaderSet)target;
                    shaderCollection.Build();

                    EditorUtility.CopySerialized(target, _TemporaryShaderCollection);

                    _Modified = false;
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        private void OnDisable()
        {
            DestroyImmediate(_NestedEditor);
            _NestedEditor = null;
        }
        #endregion Unity Methods

        #region Private Variables
        private ShaderSet _TemporaryShaderCollection;
        private Editor _NestedEditor;
        private bool _Modified;
        #endregion Private Variables
    }
}