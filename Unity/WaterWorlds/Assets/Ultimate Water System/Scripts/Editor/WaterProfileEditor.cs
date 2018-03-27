namespace UltimateWater.Editors
{
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(WaterProfile))]
    public class WaterProfileEditor : WaterEditorBase
    {
        #region Unity Methods
        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            UpdateGui();

            var profile = (WaterProfile)target;
            profile.Data.TemplateProfile = profile;

            GUI.enabled = !Application.isPlaying;

            ClearStack();
            Push("_Data");

            PropertyField("_SpectrumType");

            DrawWindSpeedGui();

            PropertyField("_TileSize");
            PropertyField("_TileScale");
            PropertyField("_WavesAmplitude");
            PropertyField("_WavesFrequencyScale");
            GUI.enabled = true;

            PropertyField("_HorizontalDisplacementScale");

            if (profile.Data.SpectrumType == WaterProfileData.WaterSpectrumType.Phillips)
                PropertyField("_PhillipsCutoffFactor", "Cutoff Factor");

            PropertyField("_Directionality");
            PropertyField("_Fetch");

            GUILayout.Space(12.0f);

            GUILayout.Label("Colors", EditorStyles.boldLabel);

            PropertyField("_DiffuseColor", "Diffuse");
            PropertyField("_ReflectionColor", "Reflection");

            var serializedSettings = new SerializedObject(WaterProjectSettings.Instance);

            var absorptionEditModeProp = serializedSettings.FindProperty("_AbsorptionEditMode");
            EditorGUILayout.PropertyField(absorptionEditModeProp);
            var absorptionEditMode = (WaterProjectSettings.AbsorptionEditMode)absorptionEditModeProp.enumValueIndex;

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(20.0f);

                EditorGUILayout.BeginVertical();
                {
                    if (absorptionEditMode == WaterProjectSettings.AbsorptionEditMode.Absorption)
                    {
                        DrawAbsorptionColorField("_AbsorptionColor", "Absorption", absorptionEditMode);
                        var customUnderwaterAbsorptionField = PropertyField("_CustomUnderwaterAbsorptionColor", "Custom Underwater Absorption");
                        if (customUnderwaterAbsorptionField.boolValue)
                            DrawAbsorptionGradientField("_AbsorptionColorByDepth", "Absorption (Underwater IME)", absorptionEditMode);
                    }
                    else
                    {
                        DrawAbsorptionColorField("_AbsorptionColor", "Transmission", absorptionEditMode);
                        var customUnderwaterAbsorptionField = PropertyField("_CustomUnderwaterAbsorptionColor", "Custom Underwater Transmission");
                        if (customUnderwaterAbsorptionField.boolValue)
                            DrawAbsorptionGradientField("_AbsorptionColorByDepth", "Transmission (Underwater IME)", absorptionEditMode);
                    }

                    if (GUI.changed)
                        UpdateFlatAbsorptionGradient();

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }

            var specularEditModeProp = serializedSettings.FindProperty("_SpecularEditMode");
            EditorGUILayout.PropertyField(specularEditModeProp);
            var specularEditMode = (WaterProjectSettings.SpecularEditMode)specularEditModeProp.enumValueIndex;

            serializedSettings.ApplyModifiedProperties();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(20.0f);

                EditorGUILayout.BeginVertical();
                {
                    if (specularEditMode == WaterProjectSettings.SpecularEditMode.IndexOfRefraction)
                    {
                        float ior = BiasToIor((profile.Data.SpecularColor.r + profile.Data.SpecularColor.g + profile.Data.SpecularColor.b) * 0.333333f);
                        float newIor = EditorGUILayout.Slider(new GUIContent("Specular (Index of refraction)", "Water index of refraction is 1.330."), ior, 1.0f, 4.05f);

                        if (newIor != ior)
                        {
                            float bias = IorToBias(newIor);
                            GetProperty("_SpecularColor").colorValue = new Color(bias, bias, bias);
                        }
                    }
                    else
                        PropertyField("_SpecularColor", "Specular (Custom color)");

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(8.0f);

            GUILayout.Label("Subsurface Scattering", EditorStyles.boldLabel);
            PropertyField("_IsotropicScatteringIntensity", "Isotropic");
            PropertyField("_ForwardScatteringIntensity", "Forward");
            PropertyField("_SubsurfaceScatteringContrast", "Contrast");
            PropertyField("_SubsurfaceScatteringShoreColor", "Shore Color");
            PropertyField("_DirectionalWrapSss", "Directional Wrap SSS");
            PropertyField("_PointWrapSss", "Point Wrap SSS");

            GUILayout.Label("Basic Properties", EditorStyles.boldLabel);

            PropertyField("_Smoothness");
            var customAmbientSmoothnessProp = PropertyField("_CustomAmbientSmoothness");

            if (!customAmbientSmoothnessProp.hasMultipleDifferentValues)
            {
                if (customAmbientSmoothnessProp.boolValue)
                    PropertyField("_AmbientSmoothness");
            }

            PropertyField("_DynamicSmoothnessIntensity");
            PropertyField("_RefractionDistortion", "Refraction Distortion");
            PropertyField("_EdgeBlendFactor", "Edge Blend Factor");
            PropertyField("_Density");

            GUILayout.Space(8.0f);

            GUILayout.Label("Normals", EditorStyles.boldLabel);
            PropertyField("_DetailFadeDistance", "Detail Fade Distance");
            PropertyField("_DisplacementNormalsIntensity", "Normal Intensity");
            DrawNormalAnimationEditor();

            GUILayout.Space(8.0f);

            GUILayout.Label("Foam", EditorStyles.boldLabel);
            PropertyField("_FoamIntensity", "Intensity");
            PropertyField("_FoamThreshold", "Threshold");
            PropertyField("_FoamFadingFactor", "Fade Factor");
            PropertyField("_FoamShoreIntensity", "Foam Shore Intensity");
            PropertyField("_FoamShoreExtent", "Foam Shore Extent");
            PropertyField("_FoamNormalScale", "Foam Normal Scale");
            PropertyField("_FoamDiffuseColor", "Foam Diffuse Color");
            PropertyField("_FoamSpecularColor", "Foam Specular Color");

            GUILayout.Space(8.0f);

            GUILayout.Label("Planar Reflections", EditorStyles.boldLabel);
            PropertyField("_PlanarReflectionIntensity", "Intensity");
            PropertyField("_PlanarReflectionFlatten", "Flatten");
            PropertyField("_PlanarReflectionVerticalOffset", "Offset");

            GUILayout.Space(8.0f);

            GUILayout.Label("Underwater", EditorStyles.boldLabel);
            PropertyField("_UnderwaterBlurSize", "Blur Size");
            PropertyField("_UnderwaterLightFadeScale", "Underwater Light Fade Scale");
            PropertyField("_UnderwaterDistortionsIntensity", "Distortion Intensity");
            PropertyField("_UnderwaterDistortionAnimationSpeed", "Distortion Animation Speed");

            GUILayout.Space(8.0f);

            GUILayout.Label("Spray", EditorStyles.boldLabel);
            PropertyField("_SprayThreshold", "Threshold");
            PropertyField("_SpraySkipRatio", "Skip Ratio");
            PropertyField("_SpraySize", "Size");

            GUILayout.Space(8.0f);

            GUILayout.Label("Textures", EditorStyles.boldLabel);
            PropertyField("_NormalMap", "Normal Map");
            //PropertyField("heightMap", "Height Map");
            PropertyField("_FoamDiffuseMap", "Foam Diffuse Map");
            PropertyField("_FoamNormalMap", "Foam Normal Map");
            PropertyField("_FoamTiling", "Foam Tiling");

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                ValidateWaterObjects();

            Pop();
        }
        #endregion Unity Methods

        #region Private Variables
        private Texture2D _IllustrationTex;

        private GUIStyle _WarningLabel;
        private GUIStyle _NormalMapLabel;
        private bool _Initialized;

        private static GradientContainer _GradientContainer;
        private static SerializedObject _SerializedGradientContainer;
        #endregion Private Variables

        #region Private Methods
        private void DrawAbsorptionColorField(string propertyName, string label, WaterProjectSettings.AbsorptionEditMode editMode)
        {
            var property = GetProperty(propertyName);

            switch (editMode)
            {
                case WaterProjectSettings.AbsorptionEditMode.Absorption:
                    {
                        PropertyField(propertyName, label);
                        break;
                    }

                case WaterProjectSettings.AbsorptionEditMode.Transmission:
                    {
                        Color transmissionColor = property.colorValue;
                        transmissionColor.r = Mathf.Exp(-transmissionColor.r);
                        transmissionColor.g = Mathf.Exp(-transmissionColor.g);
                        transmissionColor.b = Mathf.Exp(-transmissionColor.b);

                        if (property.hasMultipleDifferentValues)
                            EditorGUI.showMixedValue = true;

                        Color newTransmissionColor = EditorGUILayout.ColorField(new GUIContent(label), transmissionColor, false, false, true, new ColorPickerHDRConfig(0.0f, 1.0f, 0.0f, 1.0f));

                        EditorGUI.showMixedValue = false;

                        if (transmissionColor != newTransmissionColor)
                        {
                            var newAbsorptionColor = newTransmissionColor;
                            newAbsorptionColor.r = -Mathf.Log(newAbsorptionColor.r);
                            newAbsorptionColor.g = -Mathf.Log(newAbsorptionColor.g);
                            newAbsorptionColor.b = -Mathf.Log(newAbsorptionColor.b);
                            property.colorValue = newAbsorptionColor;
                        }

                        break;
                    }
            }
        }
        private void DrawAbsorptionGradientField(string propertyName, string label, WaterProjectSettings.AbsorptionEditMode editMode)
        {
            var property = GetProperty(propertyName);

            switch (editMode)
            {
                case WaterProjectSettings.AbsorptionEditMode.Absorption:
                    {
                        PropertyField(propertyName, label);
                        break;
                    }

                case WaterProjectSettings.AbsorptionEditMode.Transmission:
                    {
                        if (property.hasMultipleDifferentValues)
                            return;                 // multiple gradients editing is not supported for now

                        var profile = (WaterProfile)target;
                        Gradient absorptionGradient = profile.Data.AbsorptionColorByDepth;

                        if (absorptionGradient == null)
                        {
                            profile.Data.AbsorptionColorByDepth = absorptionGradient = new Gradient();
                        }

                        if (_GradientContainer == null)
                        {
                            _GradientContainer = CreateInstance<GradientContainer>();
                            _GradientContainer.hideFlags = HideFlags.DontSave;
                            _GradientContainer.Gradient = new Gradient();

                            _SerializedGradientContainer = new SerializedObject(_GradientContainer);
                        }

                        Gradient transmissionGradient = _GradientContainer.Gradient;
                        var absorptionKeys = absorptionGradient.colorKeys;

                        for (int i = 0; i < absorptionKeys.Length; ++i)
                        {
                            Color absorptionColor = absorptionKeys[i].color;
                            absorptionKeys[i].color = new Color(
                                Mathf.Exp(-absorptionColor.r),
                                Mathf.Exp(-absorptionColor.g),
                                Mathf.Exp(-absorptionColor.b),
                                absorptionColor.a
                            );
                        }

                        transmissionGradient.colorKeys = absorptionKeys;
                        transmissionGradient.alphaKeys = absorptionGradient.alphaKeys;
                        _SerializedGradientContainer.Update();
                        EditorGUILayout.PropertyField(_SerializedGradientContainer.FindProperty("Gradient"), new GUIContent(label));

                        if (_SerializedGradientContainer.ApplyModifiedPropertiesWithoutUndo())
                        {
                            transmissionGradient = _GradientContainer.Gradient;
                            absorptionKeys = transmissionGradient.colorKeys;

                            for (int i = 0; i < absorptionKeys.Length; ++i)
                            {
                                Color transmissionColor = absorptionKeys[i].color;
                                absorptionKeys[i].color = new Color(
                                    -Mathf.Log(transmissionColor.r),
                                    -Mathf.Log(transmissionColor.g),
                                    -Mathf.Log(transmissionColor.b),
                                    transmissionColor.a
                                );
                            }

                            absorptionGradient.colorKeys = absorptionKeys;
                            absorptionGradient.alphaKeys = transmissionGradient.alphaKeys;
                            serializedObject.Update();
                            EditorUtility.SetDirty(target);
                        }

                        break;
                    }
            }
        }
        private void DrawWindSpeedGui()
        {
            var windSpeedProp = GetProperty("_WindSpeed");

            float mps = windSpeedProp.floatValue;
            float knots = MpsToKnots(mps);

            if (windSpeedProp.hasMultipleDifferentValues)
                EditorGUI.showMixedValue = true;

            float newKnots = EditorGUILayout.Slider(new GUIContent(string.Format("Wind Speed ({0})", GetWindSpeedClassification(knots)), "Wind speed in knots."), knots, 0.0f, 70.0f);

            EditorGUI.showMixedValue = false;

            if (knots != newKnots)
                windSpeedProp.floatValue = KnotsToMps(newKnots);
        }
        private void DrawNormalAnimationEditor()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(60.0f));
            {
                GUILayout.Space(10);
                GUILayout.Label("Tiles 1", _NormalMapLabel);

                EditorGUILayout.BeginVertical();
                {
                    SubPropertyField("_NormalMapAnimation1", "_Speed", "Speed");
                    SubPropertyField("_NormalMapAnimation1", "_Deviation", "Deviation");
                    SubPropertyField("_NormalMapAnimation1", "_Intensity", "Intensity");
                    SubPropertyField("_NormalMapAnimation1", "_Tiling", "Tiling");

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(6);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(60.0f));
            {
                GUILayout.Space(10);
                GUILayout.Label("Tiles 2", _NormalMapLabel);

                EditorGUILayout.BeginVertical();
                {
                    SubPropertyField("_NormalMapAnimation2", "_Speed", "Speed");
                    SubPropertyField("_NormalMapAnimation2", "_Deviation", "Deviation");
                    SubPropertyField("_NormalMapAnimation2", "_Intensity", "Intensity");
                    SubPropertyField("_NormalMapAnimation2", "_Tiling", "Tiling");

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        private void UpdateFlatAbsorptionGradient()
        {
            var absorptionColorProp = GetProperty("_AbsorptionColor");

            var gradientProp = GetProperty("_AbsorptionColorByDepthFlatGradient");
            gradientProp.FindPropertyRelative("m_NumColorKeys").intValue = 1;
            gradientProp.FindPropertyRelative("m_NumAlphaKeys").intValue = 1;
            gradientProp.FindPropertyRelative("key0").colorValue = absorptionColorProp.colorValue;
        }
        private float MpsToKnots(float f)
        {
            return f / 0.5144f;
        }
        private float KnotsToMps(float f)
        {
            return 0.5144f * f;
        }
        private string GetWindSpeedClassification(float f)
        {
            if (f < 1.0f)
                return "Calm";
            else if (f < 3.0f)
                return "Light Air";
            else if (f < 6.0f)
                return "Light Breeze";
            else if (f < 10.0f)
                return "Gentle Breeze";
            else if (f < 16.0f)
                return "Moderate Breeze";
            else if (f < 21.0f)
                return "Fresh Breeze";
            else if (f < 27.0f)
                return "Strong Breeze";
            else if (f < 33.0f)
                return "Near Gale";
            else if (f < 40.0f)
                return "Gale";
            else if (f < 47.0f)
                return "Strong Gale";
            else if (f < 55.0f)
                return "Storm";
            else if (f < 63.0f)
                return "Violent Storm";
            else
                return "Hurricane";
        }
        private void OnUndoRedoPerformed()
        {
            serializedObject.Update();
            ValidateWaterObjects();
            Repaint();
        }
        private float IorToBias(float ior)
        {
            float a = (1.0f - ior);
            float b = (1.0f + ior);
            return (a * a) / (b * b);
        }
        private float BiasToIor(float bias)
        {
            return (Mathf.Sqrt(bias) + 1) / (1 - Mathf.Sqrt(bias));
        }
        private static void ValidateWaterObjects()
        {
            if (!Application.isPlaying) { return; }

            var waters = FindObjectsOfType<Water>();

            for (int i = waters.Length - 1; i >= 0; --i)
            {
                var profilesManager = waters[i].ProfilesManager;
                foreach (var profile in profilesManager.Profiles)
                {
                    if (!profile.Profile.IsTemplate && waters[i].Synchronize)
                    {
                        profile.Profile.Synchronize();
                    }
                }

                profilesManager.SetProfiles(profilesManager.Profiles);
                profilesManager.ValidateProfiles();
            }
        }
        protected override void UpdateStyles()
        {
            base.UpdateStyles();

            if (!_Initialized)
            {
                // ReSharper disable once DelegateSubtraction
                Undo.undoRedoPerformed -= OnUndoRedoPerformed;
                Undo.undoRedoPerformed += OnUndoRedoPerformed;
                _Initialized = true;
            }

            if (_WarningLabel == null)
            {
                _WarningLabel = new GUIStyle(GUI.skin.label)
                {
                    wordWrap = true,
                    normal = { textColor = new Color32(255, 201, 2, 255) }
                };
            }

            if (_IllustrationTex == null)
            {
                string texPath = WaterPackageEditorUtilities.WaterPackagePath + "Graphics/Textures/Editor/Illustration.png";
                _IllustrationTex = (Texture2D)AssetDatabase.LoadMainAssetAtPath(texPath);
            }

            if (_NormalMapLabel == null)
            {
                _NormalMapLabel = new GUIStyle(GUI.skin.label)
                {
                    stretchHeight = true,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft
                };
            }
        }
        #endregion Private Methods
    }
}