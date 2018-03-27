using UnityEngine.Serialization;

namespace UltimateWater
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Rendering;

    #region Public Types
    public enum WaterTransparencyMode
    {
        Solid,
        Refractive
    }

    /// <summary>
    /// Duplicates UnityEngine ReflectionProbeUsage enum, because it is available only on Unity 5.4+.
    /// </summary>
#if !UNITY_5_4_OR_NEWER
    public enum ReflectionProbeUsage
    {
        Off,
        BlendProbes,
        BlendProbesAndSkybox,
        Simple,
    }
#endif

    public enum PlanarReflectionsMode
    {
        Disabled,
        Normal,
        HighQuality
    }

    public enum DynamicSmoothnessMode
    {
        CheapApproximation,
        Physical
    }

    public enum NormalMappingMode
    {
        /// <summary>
        /// Normal maps are not supported.
        /// </summary>
        Never,

        /// <summary>
        /// Normal maps are supported only when FFT normal maps are not generated.
        /// </summary>
        Auto,

        /// <summary>
        /// Normal maps are always supported.
        /// </summary>
        Always
    }

    public enum WindWavesRenderMode
    {
        FullFFT,
        GerstnerAndFFTNormals,
        Gerstner,
        Disabled
    }
    #endregion Public Types

    /// <summary>
    ///     Stores references to materials with chosen keywords to include them in builds.
    /// </summary>
    [System.Serializable]
    public class ShaderSet : ScriptableObject
    {
        #region Public Variables
#if UNITY_EDITOR

        public static IShaderSetBuilder ShaderCollectionBuilder;
#endif

        public WaterTransparencyMode TransparencyMode
        {
            get { return _TransparencyMode; }
            set { _TransparencyMode = value; }
        }

        public ReflectionProbeUsage ReflectionProbeUsage
        {
            get { return _ReflectionProbeUsage; }
            set { _ReflectionProbeUsage = value; }
        }

        public bool ReceiveShadows
        {
            get { return _ReceiveShadows; }
            set { _ReceiveShadows = value; }
        }

        public PlanarReflectionsMode PlanarReflections
        {
            get { return _PlanarReflections; }
            set { _PlanarReflections = value; }
        }

        public WindWavesRenderMode WindWavesMode
        {
            get { return _WindWavesMode; }
            set { _WindWavesMode = value; }
        }

        public Shader[] SurfaceShaders
        {
            get { return _SurfaceShaders; }
        }

        public Shader[] VolumeShaders
        {
            get { return _VolumeShaders; }
        }

        public bool LocalEffectsSupported
        {
            get { return _LocalEffectsSupported; }
        }

        public bool Foam
        {
            get { return _Foam; }
        }

        public bool LocalEffectsDebug
        {
            get { return _LocalEffectsDebug; }
        }

        public bool CustomTriangularGeometry
        {
            get { return _CustomTriangularGeometry; }
        }

        public bool ProjectionGrid
        {
            get { return _ProjectionGrid; }
        }

        public DynamicSmoothnessMode SmoothnessMode
        {
            get { return _DynamicSmoothnessMode; }
        }
        #endregion Public Variables

        #region Public Methods
        public static Shader GetRuntimeShaderVariant(string keywordsString, bool volume)
        {
            var shader = Shader.Find("UltimateWater/Variations/Water " + (volume ? "Volume " : "") + keywordsString);

            if (shader == null && !_ErrorDisplayed && Application.isPlaying)
            {
                Debug.LogError("Could not find proper water shader variation. Select your water and click \"Rebuild shaders\" from its context menu to build proper shaders. Missing shader: \"" + "UltimateWater/Variations/Water " + (volume ? "Volume " : "") + keywordsString + "\"");
                _ErrorDisplayed = true;
            }

            return shader;
        }

        public Shader GetShaderVariant(string[] localKeywords, string[] sharedKeywords, string additionalCode, string keywordsString, bool volume)
        {
            System.Array.Sort(localKeywords);
            System.Array.Sort(sharedKeywords);
            string shaderNameEnd = (volume ? "Volume " : "") + keywordsString;

#if UNITY_EDITOR
            var shaders = volume ? _VolumeShaders : _SurfaceShaders;

            if (shaders != null)
            {
                for (int i = 0; i < shaders.Length; ++i)
                {
                    var shader = shaders[i];

                    if (shader != null && shader.name.EndsWith(shaderNameEnd))
                        return shader;                                 // already added
                }
            }

            if (_Rebuilding)
            {
                var path = "UltimateWater/Variations/Water " + shaderNameEnd;

                var previous = Shader.Find(path);
                if (previous != null)
                {
                    AssetDatabase.DeleteAsset(path);
                }
            }

            if (!_Rebuilding)
            {
                var shader2 = Shader.Find("UltimateWater/Variations/Water " + shaderNameEnd);

                if (shader2 != null)
                {
                    AddShader(shader2, volume);
                    return shader2;
                }
            }

            if (ShaderCollectionBuilder != null)
            {
                var shader = ShaderCollectionBuilder.BuildShaderVariant(localKeywords, sharedKeywords, additionalCode, keywordsString, volume, _ForwardRenderMode, _DeferredRenderMode);
                AddShader(shader, volume);

                return shader;
            }

            Assert.IsTrue(false, "Shader Collection Builder is null in editor.");
            return null;
#else
            return Shader.Find("UltimateWater/Variations/Water " + shaderNameEnd);
#endif
        }

        public void FindBestShaders(out Shader surfaceShader, out Shader volumeShader)
        {
            var variant = new ShaderVariant();
            BuildShaderVariant(variant, WaterQualitySettings.Instance.CurrentQualityLevel);

            var desiredWaterKeywords = variant.GetKeywordsString().Split(' ');

            surfaceShader = null;
            volumeShader = null;

            if (_SurfaceShaders != null)
            {
                for (int i = 0; i < _SurfaceShaders.Length; ++i)
                {
                    if (_SurfaceShaders[i] == null)
                        continue;

                    string shaderName = _SurfaceShaders[i].name;

                    for (int ii = 0; ii < desiredWaterKeywords.Length; ++ii)
                    {
                        if (shaderName.Contains(desiredWaterKeywords[ii]))
                        {
                            surfaceShader = _SurfaceShaders[i];
                            break;
                        }
                    }

                    if (surfaceShader != null)
                        break;
                }
            }

            if (_VolumeShaders != null)
            {
                for (int i = 0; i < _VolumeShaders.Length; ++i)
                {
                    if (_VolumeShaders[i] == null)
                        continue;

                    string shaderName = _VolumeShaders[i].name;

                    for (int ii = 0; ii < desiredWaterKeywords.Length; ++ii)
                    {
                        if (shaderName.Contains(desiredWaterKeywords[ii]))
                        {
                            volumeShader = _VolumeShaders[i];
                            break;
                        }
                    }

                    if (volumeShader != null)
                        break;
                }
            }
        }

        [ContextMenu("Rebuild shaders")]
        public void Build()
        {
#if UNITY_EDITOR
            _Rebuilding = true;
            _SurfaceShaders = new Shader[0];
            _VolumeShaders = new Shader[0];

            var qualityLevels = WaterQualitySettings.Instance.GetQualityLevelsDirect();

            for (int i = qualityLevels.Length - 1; i >= 0; --i)
            {
                SetProgress((float)i / qualityLevels.Length);

                var qualityLevel = qualityLevels[i];

                var variant = new ShaderVariant();

                // main shader
                BuildShaderVariant(variant, qualityLevel);

                GetShaderVariant(variant.GetWaterKeywords(), variant.GetUnityKeywords(), variant.GetAdditionalSurfaceCode(), variant.GetKeywordsString(), false);

                //AddFallbackVariants(variant, collection, false, 0);

                SetProgress((i + 0.5f) / qualityLevels.Length);

                // volume shader
                for (int ii = 0; ii < _DisallowedVolumeKeywords.Length; ++ii)
                    variant.SetWaterKeyword(_DisallowedVolumeKeywords[ii], false);

                GetShaderVariant(variant.GetWaterKeywords(), variant.GetUnityKeywords(), variant.GetAdditionalVolumeCode(), variant.GetKeywordsString(), true);

                //AddFallbackVariants(variant, collection, true, 0);
            }

            SetProgress(1.0f);

            CollectUtilityShaders();
            _Rebuilding = false;
            ValidateWaterObjects();

            EditorUtility.SetDirty(this);
#endif
        }

        public bool ContainsShaderVariant(string keywordsString)
        {
            if (_SurfaceShaders != null)
            {
                for (int i = _SurfaceShaders.Length - 1; i >= 0; --i)
                {
                    var shader = _SurfaceShaders[i];
                    if (shader != null && shader.name.EndsWith(keywordsString))
                        return true; // already added
                }
            }

            if (_VolumeShaders != null)
            {
                for (int i = _VolumeShaders.Length - 1; i >= 0; --i)
                {
                    var shader = _VolumeShaders[i];
                    if (shader != null && shader.name.EndsWith(keywordsString))
                        return true; // already added
                }
            }

            return false;
        }

        public ComputeShader GetComputeShader(string shaderName)
        {
            for (int i = 0; i < _ComputeShaders.Length; ++i)
            {
                if (_ComputeShaders[i].name.Contains(shaderName))
                    return _ComputeShaders[i];
            }

            return null;
        }
        #endregion Public Methods

        #region Inspector Variables
        [Header("Reflection & Refraction")]
        [SerializeField, FormerlySerializedAs("transparencyMode")]
        private WaterTransparencyMode _TransparencyMode = WaterTransparencyMode.Refractive;

        [SerializeField, FormerlySerializedAs("reflectionProbeUsage")]
        private ReflectionProbeUsage _ReflectionProbeUsage = ReflectionProbeUsage.BlendProbesAndSkybox;

        [SerializeField, FormerlySerializedAs("planarReflections")]
        private PlanarReflectionsMode _PlanarReflections = PlanarReflectionsMode.Normal;

        [Tooltip("Affects direct light specular and diffuse components. Shadows currently work only for main directional light and you need to attach WaterShadowCastingLight script to it. Also it doesn't work at all on mobile platforms.")]
        [SerializeField, FormerlySerializedAs("receiveShadows")]
        private bool _ReceiveShadows;

        [Header("Waves")]
        [SerializeField, FormerlySerializedAs("windWavesMode")]
        private WindWavesRenderMode _WindWavesMode = WindWavesRenderMode.FullFFT;

        [SerializeField, FormerlySerializedAs("dynamicSmoothnessMode")]
        private DynamicSmoothnessMode _DynamicSmoothnessMode = DynamicSmoothnessMode.Physical;

        [SerializeField, FormerlySerializedAs("localEffectsSupported")]
        private bool _LocalEffectsSupported = true;

        [SerializeField, FormerlySerializedAs("localEffectsDebug")]
        private bool _LocalEffectsDebug;

        [SerializeField, FormerlySerializedAs("foam")]
        private bool _Foam = true;

        [Header("Render Modes")]
        [SerializeField, FormerlySerializedAs("forwardRenderMode")]
        private bool _ForwardRenderMode;

        [SerializeField, FormerlySerializedAs("deferredRenderMode")]
        private bool _DeferredRenderMode;

        [Header("Geometries Support")]
        [SerializeField, FormerlySerializedAs("projectionGrid")]
        private bool _ProjectionGrid;

        [SerializeField, FormerlySerializedAs("customTriangularGeometry")]
        private bool _CustomTriangularGeometry;

        [Header("Volumes")]
        [SerializeField, FormerlySerializedAs("displayOnlyInAdditiveVolumes")]
        private bool _DisplayOnlyInAdditiveVolumes;

        [SerializeField, FormerlySerializedAs("wavesAlign")]
        private bool _WavesAlign;

        [Header("Surface")]
        [SerializeField, FormerlySerializedAs("normalMappingMode")]
        private NormalMappingMode _NormalMappingMode = NormalMappingMode.Auto;

        [SerializeField, FormerlySerializedAs("supportEmission")]
        private bool _SupportEmission;

        [Header("Generated Shaders")]
        [SerializeField, FormerlySerializedAs("surfaceShaders")]
        private Shader[] _SurfaceShaders;

        [SerializeField, FormerlySerializedAs("volumeShaders")]
        private Shader[] _VolumeShaders;

#pragma warning disable 0414
        // ReSharper disable once NotAccessedField.Local
        [SerializeField, FormerlySerializedAs("utilityShaders")]
        private Shader[] _UtilityShaders;
#pragma warning restore 0414

        [SerializeField, FormerlySerializedAs("computeShaders")]
        private ComputeShader[] _ComputeShaders;
        #endregion Inspector Variables

        #region Private Variables
        private bool _Rebuilding;
        private static bool _ErrorDisplayed;

#if UNITY_EDITOR
        private static readonly string[] _DisallowedVolumeKeywords = {
            "_WAVES_FFT_NORMAL", "_WAVES_GERSTNER", "_WATER_FOAM_WS", "_PLANAR_REFLECTIONS", "_PLANAR_REFLECTIONS_HQ",
            "_INCLUDE_SLOPE_VARIANCE", "_NORMALMAP", "_PROJECTION_GRID", "_WATER_OVERLAYS", "_WAVES_ALIGN", "_TRIANGLES",
            "_BOUNDED_WATER"
        };
#endif
        #endregion Private Variables

        #region Private Methods
        private static void ValidateWaterObjects()
        {
            var waters = FindObjectsOfType<Water>();

            for (int i = waters.Length - 1; i >= 0; --i)
                waters[i].ResetWater();
        }

        private static void SetProgress(float progress)
        {
#if UNITY_EDITOR
            if (progress != 1.0f)
                EditorUtility.DisplayProgressBar("Building water shaders...", "This may take a minute.", progress);
            else
                EditorUtility.ClearProgressBar();
#endif
        }

        private void AddShader(Shader shader, bool volumeShader)
        {
            if (volumeShader)
            {
                if (_VolumeShaders != null)
                {
                    System.Array.Resize(ref _VolumeShaders, _VolumeShaders.Length + 1);
                    _VolumeShaders[_VolumeShaders.Length - 1] = shader;
                }
                else
                    _VolumeShaders = new[] { shader };
            }
            else
            {
                if (_SurfaceShaders != null)
                {
                    System.Array.Resize(ref _SurfaceShaders, _SurfaceShaders.Length + 1);
                    _SurfaceShaders[_SurfaceShaders.Length - 1] = shader;
                }
                else
                    _SurfaceShaders = new[] { shader };
            }
        }

        private void BuildShaderVariant(ShaderVariant variant, WaterQualityLevel qualityLevel)
        {
            bool refraction = _TransparencyMode == WaterTransparencyMode.Refractive && qualityLevel.AllowAlphaBlending;

            variant.SetWaterKeyword("_WATER_REFRACTION", refraction);
            variant.SetWaterKeyword("_CUBEMAP_REFLECTIONS", _ReflectionProbeUsage != ReflectionProbeUsage.Off);
            variant.SetWaterKeyword("_WATER_RECEIVE_SHADOWS", _ReceiveShadows);

            //variant.SetWaterKeyword("_ALPHATEST_ON", false);
            variant.SetWaterKeyword("_ALPHABLEND_ON", refraction);
            variant.SetWaterKeyword("_ALPHAPREMULTIPLY_ON", !refraction);

            //variant.SetUnityKeyword("_BOUNDED_WATER", !volume.Boundless && volume.HasRenderableAdditiveVolumes);
            variant.SetUnityKeyword("_TRIANGLES", _CustomTriangularGeometry);

            if (_ProjectionGrid)
                variant.SetAdditionalSurfaceCode("_PROJECTION_GRID", "\t\t\t#pragma multi_compile _PROJECTION_GRID_OFF _PROJECTION_GRID");

            variant.SetUnityKeyword("_WATER_OVERLAYS", _LocalEffectsSupported);
            variant.SetUnityKeyword("_LOCAL_MAPS_DEBUG", _LocalEffectsSupported && _LocalEffectsDebug);

            var windWavesRenderMode = BuildWindWavesVariant(variant, qualityLevel);

            variant.SetWaterKeyword("_WATER_FOAM_WS", _Foam && !_LocalEffectsSupported && windWavesRenderMode == WindWavesRenderMode.FullFFT);
            variant.SetUnityKeyword("_BOUNDED_WATER", _DisplayOnlyInAdditiveVolumes);
            variant.SetUnityKeyword("_WAVES_ALIGN", _WavesAlign);

            variant.SetWaterKeyword("_NORMALMAP", _NormalMappingMode == NormalMappingMode.Always || (_NormalMappingMode == NormalMappingMode.Auto && windWavesRenderMode > WindWavesRenderMode.GerstnerAndFFTNormals));
            variant.SetWaterKeyword("_EMISSION", _SupportEmission);
            variant.SetWaterKeyword("_PLANAR_REFLECTIONS", _PlanarReflections == PlanarReflectionsMode.Normal);
            variant.SetWaterKeyword("_PLANAR_REFLECTIONS_HQ", _PlanarReflections == PlanarReflectionsMode.HighQuality);
        }

        private WindWavesRenderMode BuildWindWavesVariant(ShaderVariant variant, WaterQualityLevel qualityLevel)
        {
            WindWavesRenderMode finalWindWavesMode;
            var qualityWindWavesMode = qualityLevel.WavesMode;

            if (_WindWavesMode == WindWavesRenderMode.Disabled || qualityWindWavesMode == WaterWavesMode.DisallowAll)
                finalWindWavesMode = WindWavesRenderMode.Disabled;
            else if (_WindWavesMode == WindWavesRenderMode.FullFFT && qualityWindWavesMode == WaterWavesMode.AllowAll)
                finalWindWavesMode = WindWavesRenderMode.FullFFT;
            else if (_WindWavesMode <= WindWavesRenderMode.GerstnerAndFFTNormals && qualityWindWavesMode <= WaterWavesMode.AllowNormalFFT)
                finalWindWavesMode = WindWavesRenderMode.GerstnerAndFFTNormals;
            else
                finalWindWavesMode = WindWavesRenderMode.Gerstner;

            switch (finalWindWavesMode)
            {
                case WindWavesRenderMode.FullFFT:
                variant.SetUnityKeyword("_WAVES_FFT", true);
                break;

                case WindWavesRenderMode.GerstnerAndFFTNormals:
                variant.SetWaterKeyword("_WAVES_FFT_NORMAL", true);
                variant.SetUnityKeyword("_WAVES_GERSTNER", true);
                break;

                case WindWavesRenderMode.Gerstner:
                variant.SetUnityKeyword("_WAVES_GERSTNER", true);
                break;
            }

            if (_DynamicSmoothnessMode == DynamicSmoothnessMode.Physical)
                variant.SetWaterKeyword("_INCLUDE_SLOPE_VARIANCE", true);

            return finalWindWavesMode;
        }

#if UNITY_EDITOR
        private void CollectUtilityShaders()
        {
            var shaders = new List<Shader>();

            if (_PlanarReflections != PlanarReflectionsMode.Disabled)
                AddUtilityShader(shaders, "UltimateWater/Utilities/PlanarReflection - Utilities");

            if (_WindWavesMode != WindWavesRenderMode.Disabled && _WindWavesMode != WindWavesRenderMode.Gerstner)
            {
                AddUtilityShader(shaders, "UltimateWater/Spectrum/Water Spectrum");
                AddUtilityShader(shaders, "UltimateWater/Base/FFT");
                AddUtilityShader(shaders, "UltimateWater/Utilities/FFT Utilities");
            }

            if (_LocalEffectsSupported)
            {
                AddUtilityShader(shaders, "UltimateWater/Utility/Map Local Displacements");
                AddUtilityShader(shaders, "UltimateWater/Utility/ShorelineMaskRender");
            }

            if (_Foam)
            {
                AddUtilityShader(shaders, "UltimateWater/Foam/Global");
                AddUtilityShader(shaders, "UltimateWater/Foam/Local");
            }

            _UtilityShaders = shaders.ToArray();

            var computeShaders = new List<ComputeShader>();

            if (_WindWavesMode != WindWavesRenderMode.Disabled && _WindWavesMode != WindWavesRenderMode.Gerstner)
                AddComputeShader(computeShaders, "DX11 FFT");

            if (_DynamicSmoothnessMode == DynamicSmoothnessMode.Physical)
                AddComputeShader(computeShaders, "Spectral Variances");

            _ComputeShaders = computeShaders.ToArray();
        }

        private static void AddUtilityShader(List<Shader> shaders, string name)
        {
            var shader = Shader.Find(name);

            if (shader != null)
            {
                shaders.Add(shader);
            }
            else
                Debug.LogErrorFormat("Your UltimateWater installation misses shader named \"{0}\". Please reinstall the package.", name);
        }

        private static void AddComputeShader(List<ComputeShader> shaders, string name)
        {
            var guids = AssetDatabase.FindAssets(string.Format("\"{0}\" t:ComputeShader", name));

            if (guids.Length != 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var computeShader = AssetDatabase.LoadAssetAtPath<ComputeShader>(path);

                if (computeShader != null)
                {
                    shaders.Add(computeShader);
                    return;
                }
            }

            Debug.LogErrorFormat("Your UltimateWater installation misses shader named \"{0}\". Please reinstall the package.", name);
        }

        // ReSharper disable once UnusedMember.Local
        private static void CreateShaderSet()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (System.IO.Path.GetExtension(path) != "")
            {
                var filePath = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
                if (filePath != null)
                {
                    path = path.Replace(filePath, "");
                }
            }

            var bundle = CreateInstance<ShaderSet>();
            AssetDatabase.CreateAsset(bundle, AssetDatabase.GenerateUniqueAssetPath(path + "/New Shader Collection.asset"));
            AssetDatabase.SaveAssets();

            Selection.activeObject = bundle;
        }
#endif
        #endregion Private Methods
    }
}