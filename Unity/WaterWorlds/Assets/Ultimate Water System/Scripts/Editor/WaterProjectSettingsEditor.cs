namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;
    using UltimateWater;

    [CustomEditor(typeof(WaterProjectSettings))]
    public class WaterProjectSettingsEditor : WaterEditorBase
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Ultimate Water System version " + WaterProjectSettings.CurrentVersionString, EditorStyles.boldLabel);

            var waterLayerProp = serializedObject.FindProperty("_WaterLayer");
            waterLayerProp.intValue = EditorGUILayout.LayerField(new GUIContent(waterLayerProp.displayName, waterLayerProp.tooltip), waterLayerProp.intValue);

            var waterTempLayerProp = serializedObject.FindProperty("_WaterTempLayer");
            waterTempLayerProp.intValue = EditorGUILayout.LayerField(new GUIContent(waterTempLayerProp.displayName, waterTempLayerProp.tooltip), waterTempLayerProp.intValue);

            var waterCollidersLayerProp = serializedObject.FindProperty("_WaterCollidersLayer");
            waterCollidersLayerProp.intValue = EditorGUILayout.LayerField(new GUIContent(waterCollidersLayerProp.displayName, waterCollidersLayerProp.tooltip), waterCollidersLayerProp.intValue);

            PropertyField("_PhysicsThreads");
            PropertyField("_PhysicsThreadsPriority");
            PropertyField("_AllowCpuFFT");
            PropertyField("_AllowFloatingPointMipMapsOverride");
            PropertyField("_DebugPhysics");

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            bool simd = defines.Contains("WATER_SIMD");
            bool newSimd = EditorGUILayout.Toggle("Use SIMD Acceleration", simd);

            if (simd != newSimd)
            {
                if (newSimd)
                {
                    EditorUtility.DisplayDialog("DLL", "To make SIMD acceleration work, you will need to copy Mono.Simd.dll from \"(Unity Editor Path)/Unity/Editor/Data/Mono/lib/mono/2.0\" to a Plugins folder in your project.", "OK");
                }

                SetSimd(newSimd, BuildTargetGroup.Standalone);
                SetSimd(newSimd, BuildTargetGroup.PS4);
                SetSimd(newSimd, BuildTargetGroup.XboxOne);
            }

            PropertyField("_AskForWaterCameras");
            PropertyField("_DisableMultisampling");
            PropertyField("_RenderInSceneView");

            EditorGUILayout.LabelField("Camera", EditorStyles.boldLabel);

            var enableRange = PropertyField("_ClipWaterCameraRange");
            if (enableRange.boolValue)
            {
                PropertyField("_CameraClipRange");
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private static void SetSimd(bool simd, BuildTargetGroup buildTargetGroup)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (simd)
                defines += " WATER_SIMD";
            else
                defines = defines.Replace(" WATER_SIMD", "").Replace(" WATER_SIMD", "").Replace("WATER_SIMD", "");          // it's an editor script so whatever :)

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        [MenuItem("Edit/Project Settings/Water")]
        public static void OpenSettings()
        {
            var instance = WaterProjectSettings.Instance;

            Selection.activeObject = instance;
        }
    }
}