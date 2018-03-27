namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.Rendering;

    [CustomEditor(typeof(LightWaterEffects))]
    public class LightWaterEffectsEditor : WaterEditorBase
    {
        #region Unity Methods
        public override void OnInspectorGUI()
        {
            var light = ((LightWaterEffects)target).GetComponent<Light>();

            PropertyField("CastShadows");

            var modeField = serializedObject.FindProperty("_CausticsMode");
            modeField.intValue = EditorGUILayout.Popup("Caustics Mode", modeField.intValue, _Options);

            if (modeField.intValue != 0)
            {
                PropertyField("Intensity");

                if (modeField.intValue == 1) // projected texture
                {
                    PropertyField("ProjectedTexture");
                    PropertyField("ScrollDirectionPointer");
                    PropertyField("ScrollSpeed");
                    PropertyField("Distortions1");
                    PropertyField("Distortions2");
                    PropertyField("UvScale");
                }
                else // physical
                {
                    PropertyField("_CausticReceiversMask");
                    PropertyField("_Blur");
                    PropertyField("_SkipTerrainTrees");
                }

                EditorGUILayout.HelpBox(
                    "This component will set this light position at runtime to encode some information in it for the shader. In most cases it is nothing to worry about.",
                    MessageType.Info);

                var deferredShader = Shader.Find("Hidden/UltimateWater-Scene-DeferredShading");

                if (GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredShading) != BuiltinShaderMode.UseCustom ||
                    GraphicsSettings.GetCustomShader(BuiltinShaderType.DeferredShading) != deferredShader)
                {
                    EditorGUILayout.HelpBox(
                        "You have to use \"UltimateWater-Scene-DeferredShading.shader\" shader for deferred rendering. You can set it manually in \"Edit/Project Settings/Graphics\" or click a button below.",
                        MessageType.Error);

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Set Shaders"))
                    {
                        GraphicsSettings.SetCustomShader(BuiltinShaderType.DeferredShading, deferredShader);
                        GraphicsSettings.SetShaderMode(BuiltinShaderType.DeferredShading, BuiltinShaderMode.UseCustom);
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }

            if (light != null && light.type != LightType.Directional)
                EditorGUILayout.HelpBox("This component works only with directional lights for the time being.", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion Unity Methods

        #region Private Variables
        private static readonly string[] _Options = { "None", "Projected Texture (Recommended)", "Physical" };
        #endregion Private Variables
    }
}