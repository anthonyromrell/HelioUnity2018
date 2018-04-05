namespace UltimateWater
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Internal;

    /// <summary>
    ///		Manages all materials used for water surface rendering and manages their most basic properties.
    /// </summary>
    [Serializable]
    public class WaterMaterials
    {
        #region Public Types
        public enum ColorParameter
        {
            AbsorptionColor = 0,
            DiffuseColor = 1,
            SpecularColor = 2,
            DepthColor = 3,
            EmissionColor = 4,
            ReflectionColor = 5
        }

        public enum FloatParameter
        {
            DisplacementScale = 6,
            Glossiness = 7,
            RefractionDistortion = 10,
            SpecularFresnelBias = 11,
            DisplacementNormalsIntensity = 13,
            EdgeBlendFactorInv = 14,
            LightSmoothnessMultiplier = 18
        }

        public enum VectorParameter
        {
            /// <summary>
            /// x = subsurfaceScattering, y = 0.15f, z = 1.65f, w = unused
            /// </summary>
            SubsurfaceScatteringPack = 8,

            /// <summary>
            /// x = directionalWrapSSS, y = 1.0f / (1.0f + directionalWrapSSS), z = pointWrapSSS, w = 1.0f / (1.0f + pointWrapSSS)
            /// </summary>
            WrapSubsurfaceScatteringPack = 9,
            DetailFadeFactor = 12,
            PlanarReflectionPack = 15,
            BumpScale = 16,
            FoamTiling = 17
        }

        public enum TextureParameter
        {
            BumpMap = 19,
            FoamTex = 20,
            FoamNormalMap = 21
        }

        public abstract class AWaterParameter<T>
        {
            #region Public Variables
            public readonly int Hash;
            public abstract T Value { get; set; }
            #endregion Public Variables

            #region Public Methods
            protected AWaterParameter(MaterialPropertyBlock propertyBlock, int hash, T value)
            {
                _PropertyBlock = propertyBlock;
                Hash = hash;
                _Value = value;
            }
            #endregion Public Methods

            #region Private Variables
            protected readonly MaterialPropertyBlock _PropertyBlock;
            protected T _Value;
            #endregion Private Variables
        }

        public class WaterParameterFloat : AWaterParameter<float>
        {
            #region Public Variables
            public override float Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    _PropertyBlock.SetFloat(Hash, value);
                }
            }
            #endregion Public Variables

            #region Public Methods
            public WaterParameterFloat(MaterialPropertyBlock propertyBlock, int hash, float value)
                : base(propertyBlock, hash, value) { }
            #endregion Public Methods
        }
        public class WaterParameterVector4 : AWaterParameter<Vector4>
        {
            #region Public Variables
            public override Vector4 Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    _PropertyBlock.SetVector(Hash, value);
                }
            }
            #endregion Public Variables

            #region Public Methods
            public WaterParameterVector4(MaterialPropertyBlock propertyBlock, int hash, Vector4 value)
                : base(propertyBlock, hash, value) { }
            #endregion Public Methods
        }
        #endregion Public Types

        #region Public Variables
        public Material SurfaceMaterial { get; private set; }
        public Material SurfaceBackMaterial { get; private set; }
        public Material VolumeMaterial { get; private set; }
        public Material VolumeBackMaterial { get; private set; }

        public Texture2D UnderwaterAbsorptionColorByDepth
        {
            get
            {
                if (_AbsorptionGradientDirty)
                    ComputeAbsorptionGradient();

                return _AbsorptionGradient;
            }
        }

        public float HorizontalDisplacementScale { get; private set; }

        public string UsedKeywords { get; private set; }

        public float UnderwaterBlurSize { get; private set; }

        public float UnderwaterLightFadeScale { get; private set; }

        public float UnderwaterDistortionsIntensity { get; private set; }

        public float UnderwaterDistortionAnimationSpeed { get; private set; }

        #endregion Public Variables

        #region Public Methods
        public void SetKeyword(string keyword, bool enable)
        {
            if (enable)
            {
                SurfaceMaterial.EnableKeyword(keyword);
                SurfaceBackMaterial.EnableKeyword(keyword);
                VolumeMaterial.EnableKeyword(keyword);
                VolumeBackMaterial.EnableKeyword(keyword);
            }
            else
            {
                SurfaceMaterial.DisableKeyword(keyword);
                SurfaceBackMaterial.DisableKeyword(keyword);
                VolumeMaterial.DisableKeyword(keyword);
                VolumeBackMaterial.DisableKeyword(keyword);
            }
        }
        public void UpdateGlobalLookupTex()
        {
            int waterId = _Water.WaterId;

            if (_GlobalWaterLookupTex == null || waterId == -1)
                return;

            if (waterId >= 256)
            {
                Debug.LogError("There is more than 256 water objects in the scene. This is not supported in deferred water render mode. You have to switch to WaterCameraSimple.");
                return;
            }

            var absorptionColor = GetParameterValue(ColorParameter.AbsorptionColor);
            absorptionColor.a = _Water.Renderer.PropertyBlock.GetFloat(_ParameterHashes[10]);                 // _RefractionDistortion
            _GlobalWaterLookupTex.SetPixel(waterId, 0, absorptionColor);

            var reflectionColor = GetParameterValue(ColorParameter.ReflectionColor);                        // alpha channel is free here
            reflectionColor.a = _ForwardScatteringIntensity;
            _GlobalWaterLookupTex.SetPixel(waterId, 1, reflectionColor);

            var surfaceOffset = _Water.SurfaceOffset;
            _GlobalWaterLookupTex.SetPixel(waterId, 2, new Color(surfaceOffset.x, _Water.transform.position.y, surfaceOffset.y, _DirectionalLightsIntensity));

            var diffuseColor = GetParameterValue(ColorParameter.DiffuseColor);                              // alpha channel is free here
            diffuseColor.a = _Smoothness / _AmbientSmoothness;
            _GlobalWaterLookupTex.SetPixel(waterId, 3, diffuseColor);

            _GlobalLookupDirty = true;
        }
        public static void ValidateGlobalWaterDataLookupTex()
        {
            if (_GlobalWaterLookupTex == null)
            {
                CreateGlobalWaterDataLookupTex();
                _GlobalLookupDirty = false;
            }
            else if (_GlobalLookupDirty)
            {
                _GlobalWaterLookupTex.Apply(false, false);
                _GlobalLookupDirty = false;

                Shader.SetGlobalTexture("_GlobalWaterLookupTex", _GlobalWaterLookupTex);
            }
        }
        public void UpdateSurfaceMaterial()
        {
            var qualityLevel = WaterQualitySettings.Instance.CurrentQualityLevel;

            SurfaceMaterial.SetFloat(ShaderVariables.Cull, 2);

            float maxTesselationFactor = Mathf.Sqrt(2000000.0f / Mathf.Min(_Water.Geometry.TesselatedBaseVertexCount, WaterQualitySettings.Instance.MaxTesselatedVertexCount));
            _Water.Renderer.PropertyBlock.SetFloat("_TesselationFactor", Mathf.Lerp(1.0f, maxTesselationFactor, Mathf.Min(_TesselationFactor, qualityLevel.MaxTesselationFactor)));

            if (!Application.isPlaying)
            {
                bool alphaBlend = _Water.ShaderSet.TransparencyMode == WaterTransparencyMode.Refractive && qualityLevel.AllowAlphaBlending;

                if (Camera.main != null)
                {
                    var mainWaterCamera = Camera.main.GetComponent<WaterCamera>();

                    if (mainWaterCamera == null)
                        mainWaterCamera = UnityEngine.Object.FindObjectOfType<WaterCamera>();

                    if (mainWaterCamera != null && mainWaterCamera.RenderMode < WaterRenderMode.ImageEffectDeferred)
                        SetBlendMode(alphaBlend);
                    else
                        SetBlendMode(false);
                }
                else
                    SetBlendMode(false);
            }

            _Water.Renderer.PropertyBlock.SetFloat(_ParameterHashes[23], -1.0f);            // _RefractionMaxDepth

            if (_AlphaBlendMode != 0)
                SetBlendMode(_AlphaBlendMode == 2);

            // set keywords
            string shaderName = SurfaceMaterial.shader.name;

            if (shaderName.Contains("_WAVES_FFT"))
                SurfaceMaterial.EnableKeyword("_WAVES_FFT");

            if (shaderName.Contains("_BOUNDED_WATER"))
                SurfaceMaterial.EnableKeyword("_BOUNDED_WATER");

            if (shaderName.Contains("_WAVES_ALIGN"))
                SurfaceMaterial.EnableKeyword("_WAVES_ALIGN");

            if (shaderName.Contains("_WAVES_GERSTNER"))
                SurfaceMaterial.EnableKeyword("_WAVES_GERSTNER");

            if (_Water.Geometry.Triangular)
                SurfaceMaterial.EnableKeyword("_TRIANGLES");
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField]
        private float _DirectionalLightsIntensity = 1.0f;

        [Tooltip("May hurt performance on some systems.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _TesselationFactor = 1.0f;
        #endregion Inspector Variables

        #region Private Variables
        private Water _Water;

        private float _Smoothness, _AmbientSmoothness;
        private float _ForwardScatteringIntensity;
        private Vector4 _LastSurfaceOffset;
        private Texture2D _AbsorptionGradient;
        private bool _AbsorptionGradientDirty = true;
        private int _AlphaBlendMode;

        private WaterParameterFloat[] _FloatOverrides = new WaterParameterFloat[0];
        private WaterParameterVector4[] _VectorOverrides = new WaterParameterVector4[0];

        private const int _GradientResolution = 512;
        private static readonly Color[] _AbsorptionColorsBuffer = new Color[_GradientResolution];

        private static int[] _ParameterHashes;

        private static Texture2D _GlobalWaterLookupTex;
        private static bool _GlobalLookupDirty;

        private static readonly string[] _ParameterNames = {
            "_AbsorptionColor", "_Color", "_SpecColor", "_DepthColor", "_EmissionColor", "_ReflectionColor", "_DisplacementsScale", "_Glossiness",
            "_SubsurfaceScatteringPack", "_WrapSubsurfaceScatteringPack", "_RefractionDistortion", "_SubsurfaceScatteringShoreColor", "_DetailFadeFactor",
            "_DisplacementNormalsIntensity", "_EdgeBlendFactorInv", "_PlanarReflectionPack", "_BumpScale", "_FoamTiling", "_LightSmoothnessMul",
            "_BumpMap", "_FoamTex", "_FoamNormalMap",
            "_FoamSpecularColor", "_RefractionMaxDepth", "_FoamDiffuseColor"
        };
        #endregion Private Variables

        #region Private Methods
        internal void Awake(Water water)
        {
            _Water = water;

            water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
            water.WaterIdChanged += OnWaterIdChanged;

            CreateParameterHashes();
            CreateMaterials();
        }

        internal void OnWaterRender(WaterCamera waterCamera)
        {
            var surfaceOffset2D = _Water.SurfaceOffset;
            var surfaceOffset = new Vector4(surfaceOffset2D.x, _Water.transform.position.y, surfaceOffset2D.y, _Water.UniformWaterScale);

            if (surfaceOffset.x != _LastSurfaceOffset.x || surfaceOffset.y != _LastSurfaceOffset.y || surfaceOffset.z != _LastSurfaceOffset.z || surfaceOffset.w != _LastSurfaceOffset.w)
            {
                _LastSurfaceOffset = surfaceOffset;
                _Water.Renderer.PropertyBlock.SetVector(ShaderVariables.SurfaceOffset, surfaceOffset);
                UpdateGlobalLookupTexOffset();
            }

            Shader.SetGlobalColor(_ParameterHashes[0], GetParameterValue(ColorParameter.AbsorptionColor));           // caustics need that

            if (waterCamera.Type == WaterCamera.CameraType.Normal)
            {
                int alphaBlendMode;

                if (waterCamera.RenderMode < WaterRenderMode.ImageEffectDeferred)
                {
                    var qualityLevel = WaterQualitySettings.Instance.CurrentQualityLevel;
                    bool refraction = _Water.ShaderSet.TransparencyMode == WaterTransparencyMode.Refractive && qualityLevel.AllowAlphaBlending;
                    alphaBlendMode = refraction ? 2 : 1;
                }
                else
                    alphaBlendMode = 1;

                if (_AlphaBlendMode != alphaBlendMode)
                    SetBlendMode(alphaBlendMode == 2);
            }
        }

        internal void OnDestroy()
        {
            SurfaceMaterial.Destroy();
            SurfaceMaterial = null;

            SurfaceBackMaterial.Destroy();
            SurfaceBackMaterial = null;

            VolumeMaterial.Destroy();
            VolumeMaterial = null;

            VolumeBackMaterial.Destroy();
            VolumeBackMaterial = null;

            TextureUtility.Release(ref _GlobalWaterLookupTex);
            TextureUtility.Release(ref _AbsorptionGradient);

            if (!Application.isPlaying) return;

            if (_Water != null)
            {
                _Water.ProfilesManager.Changed.RemoveListener(OnProfilesChanged);
                _Water.WaterIdChanged -= OnWaterIdChanged;
            }
        }

        internal void OnValidate()
        {
            if (_Water != null)
            {
                UpdateShaders();
                UpdateSurfaceMaterial();
            }
        }

        private void SetBlendMode(bool alphaBlend)
        {
            _AlphaBlendMode = alphaBlend ? 2 : 1;

            var surfaceMaterial = SurfaceMaterial;
            surfaceMaterial.SetOverrideTag("RenderType", alphaBlend ? "Transparent" : "Opaque");
            surfaceMaterial.SetFloat("_Mode", alphaBlend ? 2 : 0);
            surfaceMaterial.SetInt("_SrcBlend", (int)(alphaBlend ? BlendMode.SrcAlpha : BlendMode.One));
            surfaceMaterial.SetInt("_DstBlend", (int)(alphaBlend ? BlendMode.OneMinusSrcAlpha : BlendMode.Zero));
            surfaceMaterial.renderQueue = alphaBlend ? 2990 : 2000;       // 2000 - geometry, 3000 - transparent

            UpdateSurfaceBackMaterial();
            UpdateVolumeMaterials();
        }
        private void CreateMaterials()
        {
            if (SurfaceMaterial != null)
                return;

            int waterId = _Water.WaterId;

            Shader surfaceShader;
            Shader volumeShader;
            _Water.ShaderSet.FindBestShaders(out surfaceShader, out volumeShader);

            if (surfaceShader == null || volumeShader == null)
                throw new InvalidOperationException("Water in a scene '" + _Water.gameObject.scene.name + "' doesn't contain necessary shaders to function properly. Please open this scene in editor and simply select the water to update its shaders.");

            SurfaceMaterial = new Material(surfaceShader) { hideFlags = HideFlags.DontSave };
            SurfaceMaterial.SetFloat("_WaterStencilId", waterId);
            SurfaceMaterial.SetFloat("_WaterStencilIdInv", (~waterId) & 255);

            // todo: move this to WaterRenderer.cs
            _Water.Renderer.PropertyBlock.SetVector("_WaterId", new Vector4(1 << waterId, 1 << (waterId + 1), (waterId + 0.5f) / 256.0f, 0));

            UpdateSurfaceMaterial();

            SurfaceBackMaterial = new Material(surfaceShader) { hideFlags = HideFlags.DontSave };
            VolumeMaterial = new Material(volumeShader) { hideFlags = HideFlags.DontSave };
            VolumeBackMaterial = new Material(volumeShader) { hideFlags = HideFlags.DontSave };

            UpdateSurfaceBackMaterial();
            UpdateVolumeMaterials();

            UsedKeywords = string.Join(" ", SurfaceMaterial.shaderKeywords);
        }
        private void UpdateShaders()
        {
            Shader surfaceShader;
            Shader volumeShader;
            _Water.ShaderSet.FindBestShaders(out surfaceShader, out volumeShader);

            if (SurfaceMaterial.shader != surfaceShader || VolumeMaterial.shader != volumeShader)
            {
                SurfaceMaterial.shader = surfaceShader;
                SurfaceBackMaterial.shader = surfaceShader;
                VolumeMaterial.shader = volumeShader;
                VolumeBackMaterial.shader = volumeShader;

                UpdateSurfaceMaterial();
                UpdateSurfaceBackMaterial();
                UpdateVolumeMaterials();
            }
        }
        private void OnProfilesChanged(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;
            var topProfile = profiles[0].Profile;
            float topWeight = 0.0f;

            for (int i = 0; i < profiles.Length; ++i)
            {
                var weightedProfile = profiles[i];

                if (topProfile == null || topWeight < weightedProfile.Weight)
                {
                    topProfile = weightedProfile.Profile;
                    topWeight = weightedProfile.Weight;
                }
            }

            HorizontalDisplacementScale = 0.0f;
            UnderwaterBlurSize = 0.0f;
            UnderwaterLightFadeScale = 0.0f;
            UnderwaterDistortionsIntensity = 0.0f;
            UnderwaterDistortionAnimationSpeed = 0.0f;

            Color absorptionColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color diffuseColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color specularColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color depthColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color emissionColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color reflectionColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color foamDiffuseColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color foamSpecularColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            Color subsurfaceScatteringShoreColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            _Smoothness = 0.0f;
            _AmbientSmoothness = 0.0f;
            float refractionDistortion = 0.0f;
            float detailFadeDistance = 0.0f;
            float displacementNormalsIntensity = 0.0f;
            float edgeBlendFactor = 0.0f;
            float directionalWrapSss = 0.0f;
            float pointWrapSss = 0.0f;
            _ForwardScatteringIntensity = 0.0f;

            Vector3 planarReflectionPack = new Vector3();
            Vector2 foamTiling = new Vector2();
            var normalMapAnimation1 = new NormalMapAnimation();
            var normalMapAnimation2 = new NormalMapAnimation();

            for (int i = 0; i < profiles.Length; ++i)
            {
                var profile = profiles[i].Profile;
                float weight = profiles[i].Weight;

                HorizontalDisplacementScale += profile.HorizontalDisplacementScale * weight;
                UnderwaterBlurSize += profile.UnderwaterBlurSize * weight;
                UnderwaterLightFadeScale += profile.UnderwaterLightFadeScale * weight;
                UnderwaterDistortionsIntensity += profile.UnderwaterDistortionsIntensity * weight;
                UnderwaterDistortionAnimationSpeed += profile.UnderwaterDistortionAnimationSpeed * weight;

                absorptionColor += profile.AbsorptionColor * weight;
                diffuseColor += profile.DiffuseColor * weight;
                specularColor += profile.SpecularColor * weight;
                depthColor += profile.DepthColor * weight;
                emissionColor += profile.EmissionColor * weight;
                reflectionColor += profile.ReflectionColor * weight;
                foamDiffuseColor += profile.FoamDiffuseColor * weight;
                foamSpecularColor += profile.FoamSpecularColor * weight;

                _Smoothness += profile.Smoothness * weight;
                _AmbientSmoothness += profile.AmbientSmoothness * weight;
                refractionDistortion += profile.RefractionDistortion * weight;
                subsurfaceScatteringShoreColor += profile.SubsurfaceScatteringShoreColor * weight;
                detailFadeDistance += profile.DetailFadeDistance * weight;
                displacementNormalsIntensity += profile.DisplacementNormalsIntensity * weight;
                edgeBlendFactor += profile.EdgeBlendFactor * weight;
                directionalWrapSss += profile.DirectionalWrapSSS * weight;
                pointWrapSss += profile.PointWrapSSS * weight;
                _ForwardScatteringIntensity += profile.ForwardScatteringIntensity * weight;

                planarReflectionPack.x += profile.PlanarReflectionIntensity * weight;
                planarReflectionPack.y += profile.PlanarReflectionFlatten * weight;
                planarReflectionPack.z += profile.PlanarReflectionVerticalOffset * weight;

                foamTiling += profile.FoamTiling * weight;
                normalMapAnimation1 += profile.NormalMapAnimation1 * weight;
                normalMapAnimation2 += profile.NormalMapAnimation2 * weight;
            }

            if (water.WindWaves != null && water.WindWaves.FinalRenderMode == WaveSpectrumRenderMode.GerstnerAndFFTNormals)
                displacementNormalsIntensity *= 0.5f;

            var block = water.Renderer.PropertyBlock;

            subsurfaceScatteringShoreColor.a = _ForwardScatteringIntensity;

            // apply to materials
            block.SetVector(_ParameterHashes[0], absorptionColor);                    // _AbsorptionColor
            block.SetColor(_ParameterHashes[1], diffuseColor);                       // _Color
            block.SetColor(_ParameterHashes[2], specularColor);                      // _SpecColor
            block.SetColor(_ParameterHashes[3], depthColor);                         // _DepthColor
            block.SetColor(_ParameterHashes[4], emissionColor);                      // _EmissionColor
            block.SetColor(_ParameterHashes[5], reflectionColor);                    // _ReflectionColor
            block.SetColor(_ParameterHashes[24], foamDiffuseColor);                 // _FoamDiffuseColor
            block.SetColor(_ParameterHashes[22], foamSpecularColor);                 // _FoamSpecularColor
            block.SetFloat(_ParameterHashes[6], HorizontalDisplacementScale);        // _DisplacementsScale

            block.SetFloat(_ParameterHashes[7], _AmbientSmoothness);                         // _Glossiness
            block.SetVector(_ParameterHashes[9], new Vector4(directionalWrapSss, 1.0f / (1.0f + directionalWrapSss), pointWrapSss, 1.0f / (1.0f + pointWrapSss)));           // _WrapSubsurfaceScatteringPack
            block.SetFloat(_ParameterHashes[10], refractionDistortion);               // _RefractionDistortion
            block.SetColor(_ParameterHashes[11], subsurfaceScatteringShoreColor);     // _SubsurfaceScatteringShoreColor
            block.SetFloat(_ParameterHashes[12], 1.0f / detailFadeDistance);         // _DetailFadeFactor
            block.SetFloat(_ParameterHashes[13], displacementNormalsIntensity);      // _DisplacementNormalsIntensity
            block.SetFloat(_ParameterHashes[14], 1.0f / edgeBlendFactor);            // _EdgeBlendFactorInv
            block.SetVector(_ParameterHashes[15], planarReflectionPack);             // _PlanarReflectionPack
            block.SetVector(_ParameterHashes[16], new Vector4(normalMapAnimation1.Intensity, normalMapAnimation2.Intensity, -(normalMapAnimation1.Intensity + normalMapAnimation2.Intensity) * 0.5f, 0.0f));             // _BumpScale
            block.SetVector(_ParameterHashes[17], new Vector2(foamTiling.x / normalMapAnimation1.Tiling.x, foamTiling.y / normalMapAnimation1.Tiling.y));                    // _FoamTiling
            block.SetFloat(_ParameterHashes[18], _Smoothness / _AmbientSmoothness);    // _LightSmoothnessMul

            SurfaceMaterial.SetTexture(_ParameterHashes[19], topProfile.NormalMap);            // _BumpMap
            SurfaceMaterial.SetTexture(_ParameterHashes[20], topProfile.FoamDiffuseMap);       // _FoamTex
            SurfaceMaterial.SetTexture(_ParameterHashes[21], topProfile.FoamNormalMap);        // _FoamNormalMap

            water.UVAnimator.NormalMapAnimation1 = normalMapAnimation1;
            water.UVAnimator.NormalMapAnimation2 = normalMapAnimation2;

            if (_VectorOverrides != null)
                ApplyOverridenParameters();

            _AbsorptionGradientDirty = true;

            UpdateSurfaceBackMaterial();
            UpdateVolumeMaterials();
            UpdateGlobalLookupTex();
        }
        private void UpdateSurfaceBackMaterial()
        {
            if (SurfaceBackMaterial == null)
                return;

            if (SurfaceBackMaterial.shader != SurfaceMaterial.shader)
                SurfaceBackMaterial.shader = SurfaceMaterial.shader;

            Color specularBackColor = SurfaceBackMaterial.GetColor(_ParameterHashes[2]);
            SurfaceBackMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);
            SurfaceBackMaterial.SetColor(_ParameterHashes[2], specularBackColor);
            SurfaceBackMaterial.SetFloat(_ParameterHashes[11], 0.0f); // air has IOR of 1.0, fresnel bias should be 0.0
            SurfaceBackMaterial.EnableKeyword("_WATER_BACK");
            SurfaceBackMaterial.SetFloat(ShaderVariables.Cull, 1);
        }
        private void UpdateVolumeMaterials()
        {
            if (VolumeMaterial == null)
                return;

            VolumeMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);
            VolumeBackMaterial.CopyPropertiesFromMaterial(SurfaceMaterial);

            if (SurfaceMaterial.renderQueue == 2990)
                VolumeBackMaterial.renderQueue = VolumeMaterial.renderQueue = 2991;

            VolumeBackMaterial.SetFloat(ShaderVariables.WaterId, 1);
        }
        private void ApplyOverridenParameters()
        {
            var block = _Water.Renderer.PropertyBlock;

            for (int i = 0; i < _VectorOverrides.Length; ++i)
                block.SetVector(_VectorOverrides[i].Hash, _VectorOverrides[i].Value);

            for (int i = 0; i < _FloatOverrides.Length; ++i)
                block.SetFloat(_FloatOverrides[i].Hash, _FloatOverrides[i].Value);
        }
        private void OnWaterIdChanged()
        {
            int waterId = _Water.WaterId;

            _Water.Renderer.PropertyBlock.SetVector(ShaderVariables.WaterId, new Vector4(1 << waterId, 1 << (waterId + 1), (waterId + 0.5f) / 256.0f, 0));
        }
        private static void RemoveArrayElementAt<T>(ref T[] array, int index)
        {
            var originalArray = array;
            var newArray = new T[array.Length - 1];

            for (int i = 0; i < index; ++i)
                newArray[i] = originalArray[i];

            for (int i = index; i < newArray.Length; ++i)
                newArray[i] = originalArray[i + 1];

            array = newArray;
        }
        private static void CreateParameterHashes()
        {
            if (_ParameterHashes != null && _ParameterHashes.Length == _ParameterNames.Length)
                return;

            int numParameters = _ParameterNames.Length;
            _ParameterHashes = new int[numParameters];

            for (int i = 0; i < numParameters; ++i)
                _ParameterHashes[i] = Shader.PropertyToID(_ParameterNames[i]);
        }
        private void UpdateGlobalLookupTexOffset()
        {
            int waterId = _Water.WaterId;

            if (_GlobalWaterLookupTex == null || waterId == -1)
                return;

            if (waterId >= 256)
            {
                Debug.LogError("There is more than 256 water objects in the scene. This is not supported in deferred water render mode. You have to switch to WaterCameraSimple.");
                return;
            }

            var surfaceOffset = _Water.SurfaceOffset;
            _GlobalWaterLookupTex.SetPixel(waterId, 2, new Color(surfaceOffset.x, _Water.transform.position.y, surfaceOffset.y, _DirectionalLightsIntensity));

            _GlobalLookupDirty = true;
        }
        private static void CreateGlobalWaterDataLookupTex()
        {
            _GlobalWaterLookupTex = new Texture2D(256, 4, TextureFormat.RGBAHalf, false, true)
            {
                name = "[UWS] WaterMaterials - _GlobalWaterLookupTex",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            Color clearColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            for (int i = 0; i < 256; ++i)
            {
                _GlobalWaterLookupTex.SetPixel(i, 0, clearColor);
                _GlobalWaterLookupTex.SetPixel(i, 1, clearColor);
                _GlobalWaterLookupTex.SetPixel(i, 2, clearColor);
                _GlobalWaterLookupTex.SetPixel(i, 3, clearColor);
            }

            var waters = WaterSystem.Instance.Waters;
            int numWaters = waters.Count;

            for (int i = 0; i < numWaters; ++i)
                waters[i].Materials.UpdateGlobalLookupTex();

            _GlobalWaterLookupTex.Apply(false, false);

            Shader.SetGlobalTexture("_GlobalWaterLookupTex", _GlobalWaterLookupTex);
        }
        private void ComputeAbsorptionGradient()
        {
            _AbsorptionGradientDirty = false;

            if (_AbsorptionGradient == null)
                _AbsorptionGradient = new Texture2D(_GradientResolution, 1, TextureFormat.RGBAHalf, false, true)
                {
                    name = "[UWS] Water Materials - Absorption Gradient",
                    hideFlags = HideFlags.DontSave,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear
                };

            var profiles = _Water.ProfilesManager.Profiles;

            for (int pixelIndex = 0; pixelIndex < _GradientResolution; ++pixelIndex)
                _AbsorptionColorsBuffer[pixelIndex] = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            for (int profileIndex = 0; profileIndex < profiles.Length; ++profileIndex)
            {
                var gradient = profiles[profileIndex].Profile.AbsorptionColorByDepth;
                float weight = profiles[profileIndex].Weight;

                for (int pixelIndex = 0; pixelIndex < _GradientResolution; ++pixelIndex)
                    _AbsorptionColorsBuffer[pixelIndex] += gradient.Evaluate(pixelIndex / 31.0f) * weight;
            }

            _AbsorptionGradient.SetPixels(_AbsorptionColorsBuffer);
            _AbsorptionGradient.Apply();
        }
        #endregion Private Methods

        #region Parameter Overrides
        /// <summary>
        /// Returns current value of some water parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Color GetParameterValue(ColorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    return _VectorOverrides[i].Value;
            }

            return _Water.Renderer.PropertyBlock.GetVector(hash);
        }

        /// <summary>
        /// Returns a class that defines custom value for some water parameter. Value that this class holds will override values that are evaluated from water profiles.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public WaterParameterVector4 GetParameterOverride(ColorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    return _VectorOverrides[i];
            }

            Vector4 defaultValue = _Water.Renderer.PropertyBlock.GetVector(hash);
            Array.Resize(ref _VectorOverrides, _VectorOverrides.Length + 1);
            return _VectorOverrides[_VectorOverrides.Length - 1] = new WaterParameterVector4(_Water.Renderer.PropertyBlock, hash, defaultValue);
        }

        /// <summary>
        /// Resets a water parameter so that it will be evaluated from water profiles again.
        /// </summary>
        /// <param name="parameter"></param>
        public void ResetParameter(ColorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    RemoveArrayElementAt(ref _VectorOverrides, i);
            }
        }

        /// <summary>
        /// Returns current value of some water parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Vector4 GetParameterValue(VectorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    return _VectorOverrides[i].Value;
            }

            return _Water.Renderer.PropertyBlock.GetVector(hash);
        }

        /// <summary>
        /// Returns a class that defines custom value for some water parameter. Value that this class holds will override values that are evaluated from water profiles.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public WaterParameterVector4 GetParameterOverride(VectorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    return _VectorOverrides[i];
            }

            Vector4 defaultValue = _Water.Renderer.PropertyBlock.GetVector(hash);
            Array.Resize(ref _VectorOverrides, _VectorOverrides.Length + 1);
            return _VectorOverrides[_VectorOverrides.Length - 1] = new WaterParameterVector4(_Water.Renderer.PropertyBlock, hash, defaultValue);
        }

        /// <summary>
        /// Resets a water parameter so that it will be evaluated from water profiles again.
        /// </summary>
        /// <param name="parameter"></param>
        public void ResetParameter(VectorParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _VectorOverrides.Length; ++i)
            {
                if (_VectorOverrides[i].Hash == hash)
                    RemoveArrayElementAt(ref _VectorOverrides, i);
            }
        }

        /// <summary>
        /// Returns current value of some water parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public float GetParameterValue(FloatParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _FloatOverrides.Length; ++i)
            {
                if (_FloatOverrides[i].Hash == hash)
                    return _FloatOverrides[i].Value;
            }

            return _Water.Renderer.PropertyBlock.GetFloat(hash);
        }

        /// <summary>
        /// Returns a class that defines custom value for some water parameter. Value that this class holds will override values that are evaluated from water profiles.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public WaterParameterFloat GetParameterOverride(FloatParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _FloatOverrides.Length; ++i)
            {
                if (_FloatOverrides[i].Hash == hash)
                    return _FloatOverrides[i];
            }

            float defaultValue = _Water.Renderer.PropertyBlock.GetFloat(hash);
            Array.Resize(ref _FloatOverrides, _FloatOverrides.Length + 1);
            return _FloatOverrides[_FloatOverrides.Length - 1] = new WaterParameterFloat(_Water.Renderer.PropertyBlock, hash, defaultValue);
        }

        /// <summary>
        /// Resets a water parameter so that it will be evaluated from water profiles again.
        /// </summary>
        /// <param name="parameter"></param>
        public void ResetParameter(FloatParameter parameter)
        {
            int hash = _ParameterHashes[(int)parameter];

            for (int i = 0; i < _FloatOverrides.Length; ++i)
            {
                if (_FloatOverrides[i].Hash == hash)
                    RemoveArrayElementAt(ref _FloatOverrides, i);
            }
        }
        #endregion Parameter Overrides
    }
}