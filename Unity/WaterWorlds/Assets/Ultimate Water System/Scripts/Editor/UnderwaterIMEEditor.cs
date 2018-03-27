namespace UltimateWater.Editors
{
    using UnityEditor;

    [CustomEditor(typeof(UnderwaterIME))]
    public class UnderwaterIMEEditor : WaterEditorBase
    {
        #region Public Methods
        public override void OnInspectorGUI()
        {
            SubPropertyField("_Blur", "_Iterations", "Blur Quality");
            PropertyField("_UnderwaterAudio");

            _UseFoldouts = true;

            PropertyField("_CameraBlurScale");
            PropertyField("_MaskResolution");

            serializedObject.ApplyModifiedProperties();
        }
        #endregion Public Methods
    }
}