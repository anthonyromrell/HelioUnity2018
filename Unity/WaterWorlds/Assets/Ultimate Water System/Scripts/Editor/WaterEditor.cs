namespace UltimateWater.Editors
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.AnimatedValues;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEngine.Rendering;
    using Internal;

    [CustomEditor(typeof(Water))]
    public class WaterEditor : WaterEditorBase
    {
        #region Private Variables
        private readonly AnimBool _EnvironmentFoldout = new AnimBool(true);
        private readonly AnimBool _SurfaceFoldout = new AnimBool(false);
        private readonly AnimBool _PlanarReflectionsFoldout = new AnimBool(false);
        private readonly AnimBool _GeometryFoldout = new AnimBool(false);
        private readonly AnimBool _SubsurfaceScatteringFoldout = new AnimBool(false);
        private readonly AnimBool _InspectFoldout = new AnimBool(false);

        private GUIContent[] _WaterShaderSets;
        private string[] _WaterShaderSetsPaths;
        private int _CurrentShaderSetIndex;

        private GUIContent[] _WaterProfiles;
        private string[] _WaterProfilesPaths;
        private int _CurrentWaterProfileIndex;

        private int _SelectedMapIndex = -1;
        private bool _AskedAboutCameras;
        #endregion Private Variables

        public override void OnInspectorGUI()
        {
            var water = (Water)target;

            if (Event.current.type == EventType.Layout)
            {
                LookForWaterCamera(water);
            }

            UpdateGui();

            GUILayout.Space(4);

            DrawProfileField();
            DrawShaderSetField();

            EditorGUI.BeginChangeCheck();
            PropertyField("Synchronize");
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var profile in water.ProfilesManager.Profiles)
                {
                    profile.Profile.Synchronize();
                }
            }

            DrawNotifications();

            if (water.ShaderSet == null)
            {
                EditorGUILayout.HelpBox("Please select a shader set for this water instance.", MessageType.Info);
                return;
            }

            if (BeginGroup("Environment", _EnvironmentFoldout))
            {
                if (water.ShaderSet.ReflectionProbeUsage != ReflectionProbeUsage.Off)
                    SubPropertyField("_WaterRenderer", "_ReflectionProbeAnchor", "Reflection Probes Anchor");

                PropertyField("_Seed");
                SubPropertyField("_Volume", "_Boundless", "Boundless");
            }

            EndGroup();

            if (BeginGroup("Geometry", _GeometryFoldout))
            {
                SubPropertyField("_Geometry", "_Type", "Type");

                SubPropertyField("_Geometry", "_BaseVertexCount", "Vertices");
                SubPropertyField("_Geometry", "_TesselatedBaseVertexCount", "Vertices (Tesselation)");
                SubSubPropertyField("_Geometry", "_CustomSurfaceMeshes", "_CustomMeshes", "Custom Meshes");

                if (water.Geometry.GeometryType == WaterGeometry.Type.ProjectionGrid && !water.ShaderSet.ProjectionGrid)
                    EditorGUILayout.HelpBox("You have chosen a projection grid geometry, byt selected water shader doesn't support it. Change the shader, or enable projection grid support on it.", MessageType.Error);

                if (water.Geometry.GeometryType == WaterGeometry.Type.CustomMeshes && water.Geometry.CustomSurfaceMeshes.Meshes.Length != 0 && water.Geometry.CustomSurfaceMeshes.Triangular &&
                    !water.ShaderSet.CustomTriangularGeometry)
                    EditorGUILayout.HelpBox("You have chosen a custom triangular geometry, but selected water shader doesn't support it. Change the shader, or enable custom triangular geometry support on it.", MessageType.Error);
            }

            EndGroup();

            if (BeginGroup("Shading", _SurfaceFoldout))
            {
                SubPropertyField("_Materials", "_TesselationFactor", "Tesselation Factor");
            }

            EndGroup();

            GUI.enabled = water.ShaderSet.PlanarReflections != PlanarReflectionsMode.Disabled;

            if (BeginGroup(GUI.enabled ? "Planar Reflections" : "Planar Reflections (Disabled)", _PlanarReflectionsFoldout))
            {
                SubPropertyField("_PlanarReflectionData", "ReflectionMask", "Reflection Mask");
                SubPropertyField("_PlanarReflectionData", "ReflectSkybox", "Reflect Skybox");
                SubPropertyField("_PlanarReflectionData", "RenderShadows", "Render Shadows");
                SubPropertyField("_PlanarReflectionData", "Resolution", "Resolution");
                SubPropertyField("_PlanarReflectionData", "RetinaResolution", "Retina Resolution");
            }

            EndGroup();

            GUI.enabled = true;

            if (BeginGroup("Subsurface Scattering", _SubsurfaceScatteringFoldout))
            {
                var mode = SubPropertyField("_SubsurfaceScattering", "_Mode", "Mode");

                if ((WaterSubsurfaceScattering.SubsurfaceScatteringMode)mode.enumValueIndex == WaterSubsurfaceScattering.SubsurfaceScatteringMode.TextureSpace)
                {
                    SubPropertyField("_SubsurfaceScattering", "_AmbientResolution", "Resolution");
                    DrawLightLayerField();
                    SubPropertyField("_SubsurfaceScattering", "_LightCount", "Light Count");
                }
            }

            EndGroup();

            GUI.enabled = water.ShaderSet.WindWavesMode != WindWavesRenderMode.Disabled;

            if (BeginGroup(GUI.enabled ? "Wind Waves" : "Wind Waves (Disabled)", _InspectFoldout))
            {
                DrawWindWavesInspector(serializedObject.FindProperty("_WindWavesData"));
            }

            EndGroup();

            if (BeginGroup("Inspect", _InspectFoldout))
            {
                var maps = GetWaterMaps();
                _SelectedMapIndex = EditorGUILayout.Popup("Texture", _SelectedMapIndex, maps.Select(m => m.Name).ToArray());

                if (_SelectedMapIndex >= 0 && _SelectedMapIndex < maps.Count)
                {
                    var texture = maps[_SelectedMapIndex].Getter();
                    DisplayTextureInspector(texture);
                }
            }

            EndGroup();

            GUILayout.Space(10);
            DrawFeatureSelector();
            GUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }

        private void LookForWaterCamera(Water water)
        {
            foreach (var camera in Camera.allCameras)
            {
                if (WaterCamera.GetWaterCamera(camera) != null)
                    return;
            }

            if (Camera.main == null)
                return;

            if (_AskedAboutCameras || !water.AskForWaterCamera || !WaterProjectSettings.Instance.AskForWaterCameras)
            {
                water.AskForWaterCamera = false;
                return;
            }

            water.AskForWaterCamera = false;
            _AskedAboutCameras = true;
            switch (EditorUtility.DisplayDialogComplex("UltimateWater - Missing water camera", "Your scene doesn't contain any cameras with WaterCamera component, but only such cameras may actually see the water. Would you like to add this component to camera named \"" + Camera.main.name + "\"? ", "Ok", "Cancel", "Don't ask again"))
            {
                case 0:
                    {
                        Camera.main.gameObject.AddComponent<WaterCamera>();

                        break;
                    }

                case 2:
                    {
                        WaterProjectSettings.Instance.AskForWaterCameras = false;
                        break;
                    }
            }
        }

        private void DrawShaderSetField()
        {
            var shaderSetProp = serializedObject.FindProperty("_ShaderSet");

            if (shaderSetProp.objectReferenceValue == null)
            {
                shaderSetProp.objectReferenceValue = WaterPackageEditorUtilities.FindDefaultAsset<ShaderSet>("\"Ocean\" t:ShaderSet", "t:ShaderSet");
                serializedObject.ApplyModifiedProperties();
            }

            if (_WaterShaderSets == null && Event.current.type == EventType.Layout)
            {
                FindAssets(
                    "ShaderSet", shaderSetProp.objectReferenceValue,
                    out _CurrentShaderSetIndex, out _WaterShaderSets, out _WaterShaderSetsPaths);
            }

            EditorGUILayout.BeginHorizontal();
            int newShaderSetIndex = EditorGUILayout.Popup(new GUIContent("Shader Set"), _CurrentShaderSetIndex, _WaterShaderSets, EditorStyles.popup);

            if (_CurrentShaderSetIndex != newShaderSetIndex)
            {
                _CurrentShaderSetIndex = newShaderSetIndex;
                shaderSetProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath<ShaderSet>(_WaterShaderSetsPaths[_CurrentShaderSetIndex]);
            }

            if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(50)))
                Selection.activeObject = shaderSetProp.objectReferenceValue;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawProfileField()
        {
            var profileField = serializedObject.FindProperty("_ProfilesManager").FindPropertyRelative("_InitialProfile");

            if (profileField.objectReferenceValue == null)
                profileField.objectReferenceValue = WaterPackageEditorUtilities.FindDefaultAsset<WaterProfile>("\"Sea - 6. Strong Breeze\" t:WaterProfile", "t:WaterProfile");

            if (_WaterProfiles == null)
            {
                FindAssets(
                    "WaterProfile", profileField.objectReferenceValue,
                    out _CurrentWaterProfileIndex, out _WaterProfiles, out _WaterProfilesPaths);
            }

            EditorGUILayout.BeginHorizontal();
            int newShaderSetIndex = EditorGUILayout.Popup(new GUIContent("Profile"), _CurrentWaterProfileIndex, _WaterProfiles, EditorStyles.popup);

            if (_CurrentWaterProfileIndex != newShaderSetIndex)
            {
                _CurrentWaterProfileIndex = newShaderSetIndex;
                profileField.objectReferenceValue = AssetDatabase.LoadAssetAtPath<WaterProfile>(_WaterProfilesPaths[_CurrentWaterProfileIndex]);
            }

            if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(50)))
                Selection.activeObject = profileField.objectReferenceValue;

            EditorGUILayout.EndHorizontal();
        }

        private void DrawNotifications()
        {
            if (Application.isPlaying)
            {
                return;
            }

            var versionProp = serializedObject.FindProperty("_Version");
            if (versionProp.floatValue == WaterProjectSettings.CurrentVersion)
            {
            }
        }

        private void DrawFeatureSelector()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Add feature...", GUILayout.Width(120)))
                {
                    var menu = new GenericMenu();

                    AddMenuItem(menu, "Network Water", typeof(NetworkWater));

                    menu.ShowAsContext();
                }

                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawLightLayerField()
        {
            var lightLayerProp = serializedObject.FindProperty("_SubsurfaceScattering").FindPropertyRelative("_LightingLayer");
            lightLayerProp.intValue = EditorGUILayout.LayerField("Light Layer", lightLayerProp.intValue);
        }

        private void SaveWaterAssetFileTo(string path)
        {
            var shaderCollection = CreateInstance<ShaderSet>();
            AssetDatabase.CreateAsset(shaderCollection, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            serializedObject.FindProperty("shaderCollection").objectReferenceValue = shaderCollection;
            serializedObject.FindProperty("sceneHash").intValue = GetSceneHash();
        }

        private void AddMenuItem(GenericMenu menu, string label, System.Type type)
        {
            var water = (Water)target;

            if (water.GetComponent(type) == null)
            {
                menu.AddItem(new GUIContent(label), false, OnAddComponent, type);
            }
        }

        private void OnAddComponent(object componentTypeObj)
        {
            var water = (Water)target;
            water.gameObject.AddComponent((System.Type)componentTypeObj);
        }

        private static void FindAssets(string typeName, Object currentAsset, out int currentIndex, out GUIContent[] labels, out string[] assetPaths)
        {
            var guids = AssetDatabase.FindAssets("t:" + typeName);
            assetPaths = guids.Select<string, string>(AssetDatabase.GUIDToAssetPath).ToArray();
            labels =
                assetPaths.Select(path => new GUIContent(Path.GetFileNameWithoutExtension(path)))
                    .ToArray();

            currentIndex = currentAsset != null
                ? System.Array.IndexOf(assetPaths, AssetDatabase.GetAssetPath(currentAsset))
                : -1;
        }

        private List<WaterMap> GetWaterMaps()
        {
            var water = (Water)target;
            var textures = new List<WaterMap>();

            var windWaves = water.WindWaves;

            if (windWaves != null)
            {
                textures.Add(new WaterMap("WindWaves - Raw Omnidirectional Spectrum", () => windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.RawOmnidirectional)));
                textures.Add(new WaterMap("WindWaves - Raw Directional Spectrum", () => windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.RawDirectional)));
                textures.Add(new WaterMap("WindWaves - Height Spectrum", () => windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Height)));
                textures.Add(new WaterMap("WindWaves - Normal Spectrum", () => windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Normal)));
                textures.Add(new WaterMap("WindWaves - Horizontal Displacement Spectrum", () => windWaves.SpectrumResolver.GetSpectrum(SpectrumResolver.SpectrumType.Displacement)));

                var wavesFft = windWaves.WaterWavesFFT;
                textures.Add(new WaterMap("WindWaves - Displacement Map 0", () => wavesFft != null ? wavesFft.GetDisplacementMap(0) : null));
                textures.Add(new WaterMap("WindWaves - Displacement Map 1", () => wavesFft != null ? wavesFft.GetDisplacementMap(1) : null));
                textures.Add(new WaterMap("WindWaves - Displacement Map 2", () => wavesFft != null ? wavesFft.GetDisplacementMap(2) : null));
                textures.Add(new WaterMap("WindWaves - Displacement Map 3", () => wavesFft != null ? wavesFft.GetDisplacementMap(3) : null));
                textures.Add(new WaterMap("WindWaves - Normal Map 0", () => wavesFft != null ? wavesFft.GetNormalMap(0) : null));
                textures.Add(new WaterMap("WindWaves - Normal Map 1", () => wavesFft != null ? wavesFft.GetNormalMap(1) : null));

                var foam = water.Foam;
                textures.Add(new WaterMap("WaterFoam - Foam Map", () => foam != null ? foam.FoamMap : null));

                var properties = water.Renderer.PropertyBlock;
                textures.Add(new WaterMap("WaterFoam - Local Displacement", () => properties.GetTexture("_LocalDisplacementMap")));
                textures.Add(new WaterMap("WaterFoam - Local Normals", () => properties.GetTexture("_LocalNormalMap")));
                textures.Add(new WaterMap("WaterFoam - Local Diffuse", () => properties.GetTexture("_LocalDiffuseMap")));

#if UNITY_5_6_OR_NEWER  // GetGlobalTexture isn't supported in earlier versions

                textures.Add(new WaterMap("Waterless Depth", () => Shader.GetGlobalTexture(ShaderVariables.WaterDepthTexture)));
#endif
            }

            return textures;
        }

        private void SearchShaderVariantCollection()
        {
            var editedWater = (Water)target;
            var transforms = FindObjectsOfType<Transform>();

            foreach (var root in transforms)
            {
                if (root.parent == null)     // if that's really a root
                {
                    var waters = root.GetComponentsInChildren<Water>(true);

                    foreach (var water in waters)
                    {
                        if (water != editedWater && water.ShaderSet != null)
                        {
                            serializedObject.FindProperty("shaderCollection").objectReferenceValue = water.ShaderSet;
                            serializedObject.FindProperty("sceneHash").intValue = GetSceneHash();
                            serializedObject.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }

        private int GetSceneHash()
        {
            var md5 = System.Security.Cryptography.MD5.Create();

#if UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            string sceneName = EditorApplication.currentScene + "#" + target.name;
#else
            string sceneName = ((Water)target).gameObject.scene.name;
#endif

            if (!string.IsNullOrEmpty(sceneName))
            {
                var hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(sceneName));
                return System.BitConverter.ToInt32(hash, 0);
            }

            return -1;
        }

        private readonly AnimBool _FftFoldout = new AnimBool(false);
        private readonly AnimBool _GerstnerFoldout = new AnimBool(false);

        private static readonly GUIContent[] _ResolutionLabels = { new GUIContent("4x32x32 (runs on potatos)"), new GUIContent("4x64x64"), new GUIContent("4x128x128"), new GUIContent("4x256x256 (very high; most PCs)"), new GUIContent("4x512x512 (extreme; gaming PCs)"), new GUIContent("4x1024x1024 (as seen in Titanic® and Water World®; future PCs)") };
        private static readonly int[] _Resolutions = { 32, 64, 128, 256, 512, 1024, 2048, 4096 };

        public void DrawWindWavesInspector(SerializedProperty windWavesData)
        {
            bool looping = windWavesData.FindPropertyRelative("LoopDuration").floatValue > 0.0f;

            //if(BeginGroup("Rendering", null))
            {
                var copyFromProp = windWavesData.FindPropertyRelative("CopyFrom");

                GUI.enabled = copyFromProp.objectReferenceValue == null;

                DrawResolutionGui(windWavesData);
                PropertyField(windWavesData, "CpuDesiredStandardError", "Desired Standard Error (CPU)");
                PropertyField(windWavesData, "HighPrecision");

                PropertyField(windWavesData, "WindDirectionPointer");
                PropertyField(windWavesData, "LoopDuration");
                GUI.enabled = true;

                //SubPropertyField("dynamicSmoothness", "enabled", "Dynamic Smoothness");
                PropertyField(windWavesData, "CopyFrom");
            }

            //EndGroup();

            _UseFoldouts = true;

            if (BeginGroup("FFT", _FftFoldout, 14))
            {
                SubPropertyField(windWavesData, "WavesRendererFFTData", "HighQualityNormalMaps", "High Quality Normal Maps");
                SubPropertyField(windWavesData, "WavesRendererFFTData", "ForcePixelShader", "Force Pixel Shader");
                SubPropertyField(windWavesData, "WavesRendererFFTData", "FlattenMode", "Flatten Mode");

                if (looping)
                    SubPropertyField(windWavesData, "WavesRendererFFTData", "CachedFrameCount", "Cached Frame Count");
            }

            EndGroup();

            if (BeginGroup("Gerstner", _GerstnerFoldout, 14))
            {
                SubPropertyField(windWavesData, "WavesRendererGerstnerData", "NumGerstners", "Waves Count");
            }

            EndGroup();

            _UseFoldouts = false;

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                ((Water)target).OnValidate();
            }

            serializedObject.Update();
        }

        private static void DrawResolutionGui(SerializedProperty windWaves)
        {
            var property = windWaves.FindPropertyRelative("Resolution");
            DrawResolutionGui(property, null);
        }

        public static void DrawResolutionGui(SerializedProperty property, string name)
        {
            const string tooltip = "Higher values increase quality, but also decrease performance. Directly controls quality of waves, foam and spray.";

            int newResolution = IndexToResolution(EditorGUILayout.Popup(new GUIContent(name != null ? name : property.displayName, tooltip), ResolutionToIndex(property.intValue), _ResolutionLabels));
            property.intValue = newResolution;
        }

        private static int ResolutionToIndex(int resolution)
        {
            switch (resolution)
            {
                case 32: return 0;
                case 64: return 1;
                case 128: return 2;
                case 256: return 3;
                case 512: return 4;
                case 1024: return 5;
                case 2048: return 6;
                case 4096: return 7;
            }

            return 0;
        }

        private static int IndexToResolution(int index)
        {
            return _Resolutions[index];
        }
    }
}