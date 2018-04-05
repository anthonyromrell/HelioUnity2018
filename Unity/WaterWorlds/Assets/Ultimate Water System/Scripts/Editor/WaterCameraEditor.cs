namespace UltimateWater.Editors
{
    using UnityEditor;
    using UnityEngine;
    using Internal;

    [CustomEditor(typeof(WaterCamera), true)]
    public class WaterCameraEditor : WaterEditorBase
    {
        #region Unity Messages
        public override void OnInspectorGUI()
        {
            var waterCamera = (WaterCamera)target;
            var camera = waterCamera.GetComponent<Camera>();

            var renderModeProp = PropertyField("_RenderMode", "Render Mode");
            var renderMode = (WaterRenderMode)renderModeProp.enumValueIndex;

            if (waterCamera.RenderMode == WaterRenderMode.DefaultQueue)
            {
                EditorGUILayout.HelpBox("This render mode doesn't support opaque image effects like SSAO, SSR," +
                                        " global fog and atmospheric scattering, but it is lightweight and fast.\n\n" +
                                        "If you use Unity's deferred render mode, don't disable Blend Edges and/or Refraction on Water objects.",
                                        MessageType.Info);
            }

            PropertyField("_GeometryType", "Water Geometry");

            if (renderMode != WaterRenderMode.ImageEffectDeferred)
            {
                PropertyField("_RenderWaterDepth", "Render Water Depth");
            }

            PropertyField("_RenderVolumes", "Render Volumes");
            PropertyField("_EffectsLight", "Effects Light");
            PropertyField("_BaseEffectsQuality", "Base Effects Quality");

            if (renderMode == WaterRenderMode.ImageEffectDeferred)
                PropertyField("_MainWater", "Main Water");

            DrawSubmersionEditor(waterCamera);

            if (camera.farClipPlane < 100.0f)
            {
                EditorGUILayout.HelpBox("Your camera farClipPlane is set below 100 units. " +
                                        "It may be too low for the underwater effects to \"see\" the max depth and they may produce some artifacts.",
                                        MessageType.Warning, true);
            }

            ManageSceneCamera();
            CheckDeferredSupport(waterCamera);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion Unity Messages

        #region Private Methods
        private void DrawSubmersionEditor(WaterCamera waterCamera)
        {
            // ReSharper disable once AssignmentInConditionalExpression
            if (waterCamera._EditSubmersion = EditorGUILayout.Foldout(waterCamera._EditSubmersion, "Submersion (" + waterCamera.SubmersionState + ")"))
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.Space();

                    var cameraSubmersion = GetProperty("_CameraSubmersion");

                    GUI.enabled = !Application.isPlaying;
                    PropertyField(cameraSubmersion, "_Subdivisions");
                    GUI.enabled = true;

                    PropertyField(cameraSubmersion, "_Radius");

                    PropertyField("_SubmersionStateChanged", "Submersion State Changed");
                }
                GUILayout.EndVertical();
            }
        }

        private void ManageSceneCamera()
        {
            if (GUILayout.Button("Toggle SceneView rendering"))
            {
                WaterProjectSettings.Instance.RenderInSceneView = !WaterProjectSettings.Instance.RenderInSceneView;
            }
        }

        private void CheckDeferredSupport(WaterCamera waterCamera)
        {
            if (VersionCompatibility.Version <= 545 && waterCamera.RenderMode == WaterRenderMode.ImageEffectDeferred)
            {
                waterCamera.RenderMode = WaterRenderMode.ImageEffectForward;
                Debug.LogWarning("Deferred mode is not supported in this version");
            }
        }
        #endregion Private Methods
    }
}