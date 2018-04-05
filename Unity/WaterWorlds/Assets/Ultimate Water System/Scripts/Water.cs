namespace UltimateWater
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;

    using Internal;

    /// <summary>
    ///     Main water component.
    /// </summary>
    [AddComponentMenu("Ultimate Water/Water (Base Component)", -1)]
    public sealed partial class Water : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region Public Types
        [Serializable]
        public struct WeightedProfile
        {
            public WaterProfileData Profile;
            public float Weight;

            public WeightedProfile(WaterProfile profile, float weight)
            {
                var forceInitialize = profile.Data.Spectrum;
                if (forceInitialize == null)
                {
                    Debug.Log("spectrum not created");
                }

                Profile = new WaterProfileData();
                Profile.Copy(profile.Data);
                Profile.TemplateProfile = profile;

                Weight = weight;
            }
        }

        [Serializable]
        public class WaterEvent : UnityEvent<Water> { };
        #endregion Public Types

        #region Public Variables
        [Tooltip("Synchronizes dynamic preset with default water profile")]
        public bool Synchronize = true;

        public LayerMask CustomEffectsLayerMask
        {
            get { return _DynamicWaterData.CustomEffectsLayerMask; }
            set { _DynamicWaterData.CustomEffectsLayerMask = value; }
        }

        public ProfilesManager ProfilesManager
        {
            get { return _ProfilesManager; }
        }

        public WaterMaterials Materials
        {
            get { return _Materials; }
        }
        public WaterGeometry Geometry
        {
            get { return _Geometry; }
        }
        public WaterRenderer Renderer
        {
            get { return _WaterRenderer; }
        }
        public WaterVolume Volume
        {
            get { return _Volume; }
            set { _Volume = value; }
        }

        public WaterUvAnimator UVAnimator
        {
            get { return _UVAnimator; }
        }
        public DynamicWater DynamicWater { get; private set; }
        public Foam Foam { get; private set; }
        public PlanarReflection PlanarReflection { get; private set; }
        public WindWaves WindWaves { get; private set; }

        public WaterSubsurfaceScattering SubsurfaceScattering
        {
            get { return _SubsurfaceScattering; }
        }

        public ShaderSet ShaderSet
        {
            get
            {
#if UNITY_EDITOR
                if (_ShaderSet == null)
                {
                    _ShaderSet = WaterPackageEditorUtilities.FindDefaultAsset<ShaderSet>("\"Ocean\" t:ShaderSet", "t:ShaderSet");
                }
#endif

                return _ShaderSet;
            }
        }

        /// <summary>
        ///		Use this property instead of disabling water completely, if you want to either:
        ///			- temporarily disable water from rendering without releasing its resources so that enabling it again won't cause hiccups,
        ///			- disable water from rendering, but keep the physics.
        /// </summary>
        public bool RenderingEnabled
        {
            get { return _RenderingEnabled; }
            set
            {
                if (_RenderingEnabled == value) { return; }

                _RenderingEnabled = value;
                if (_RenderingEnabled)
                {
                    if (enabled)
                    {
                        WaterSystem.Register(this);
                    }
                }
                else
                {
                    WaterSystem.Unregister(this);
                }
            }
        }

        public int ComputedSamplesCount { get; private set; }
        public float Density { get; private set; }
        public float Gravity { get; private set; }
        public float MaxHorizontalDisplacement { get; private set; }
        public float MaxVerticalDisplacement { get; private set; }

        public int Seed
        {
            get { return _Seed; }
            set { _Seed = value; }
        }
        public Vector2 SurfaceOffset
        {
            get { return float.IsNaN(_SurfaceOffset.x) ? new Vector2(-transform.position.x, -transform.position.z) : _SurfaceOffset; }
            set { _SurfaceOffset = value; }
        }
        public float Time
        {
            get { return _Time == -1 ? UnityEngine.Time.time : _Time; }
            set { _Time = value; }
        }
        public float UniformWaterScale
        {
            get { return transform.localScale.y; }
        }

        public int WaterId
        {
            get { return _WaterId; }
        }

        public bool AskForWaterCamera = true;
        #endregion Public Variables

        #region Public Methods
        /// <summary>
        /// Use this in your custom script that tries to access water very early after a scene gets loaded.
        /// </summary>
        public void ForceStartup()
        {
            CreateWaterComponents();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (!_IsPlaying)
                _ComponentsCreated = false;
        }

        public void ResetWater()
        {
            enabled = false;
            OnDestroy();
            _ComponentsCreated = false;
            enabled = true;
        }

        public static Water CreateWater(string name, ShaderSet shaderCollection)
        {
            var gameObject = new GameObject(name);
            var water = gameObject.AddComponent<Water>();
            water._ShaderSet = shaderCollection;
            return water;
        }

        public static Water FindWater(Vector3 position, float radius)
        {
            bool unused1, unused2;
            return FindWater(position, radius, null, out unused1, out unused2);
        }

        public static Water FindWater(Vector3 position, float radius, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
        {
            return FindWater(position, radius, null, out isInsideSubtractiveVolume, out isInsideAdditiveVolume);
        }

        public static Water FindWater(Vector3 position, float radius, List<Water> allowedWaters, out bool isInsideSubtractiveVolume, out bool isInsideAdditiveVolume)
        {
            isInsideSubtractiveVolume = false;
            isInsideAdditiveVolume = false;

#if UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            var collidersBuffer = Physics.OverlapSphere(position, radius, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
            int numHits = collidersBuffer.Length;
#else
            int numHits = Physics.OverlapSphereNonAlloc(position, radius, _CollidersBuffer, 1 << WaterProjectSettings.Instance.WaterCollidersLayer, QueryTriggerInteraction.Collide);
#endif

            _PossibleWaters.Clear();
            _ExcludedWaters.Clear();

            for (int i = 0; i < numHits; ++i)
            {
                var volume = WaterVolumeBase.GetWaterVolume(_CollidersBuffer[i]);

                if (volume != null)
                {
                    if (volume is WaterVolumeAdd)
                    {
                        isInsideAdditiveVolume = true;

                        if (allowedWaters == null || allowedWaters.Contains(volume.Water))
                            _PossibleWaters.Add(volume.Water);
                    }
                    else                // subtractive
                    {
                        isInsideSubtractiveVolume = true;
                        _ExcludedWaters.Add(volume.Water);
                    }
                }
            }

            for (int i = 0; i < _PossibleWaters.Count; ++i)
            {
                if (!_ExcludedWaters.Contains(_PossibleWaters[i]))
                    return _PossibleWaters[i];
            }

            var boundlessWaters = WaterSystem.Instance.BoundlessWaters;
            int numBoundlessWaters = boundlessWaters.Count;

            for (int i = 0; i < numBoundlessWaters; ++i)
            {
                var water = boundlessWaters[i];

                if ((allowedWaters == null || allowedWaters.Contains(water)) && water.Volume.IsPointInsideMainVolume(position, radius) && !_ExcludedWaters.Contains(water))
                    return water;
            }

            return null;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("shaderSet")]
        private ShaderSet _ShaderSet;

        [Tooltip("Set it to anything else than 0 if your game has multiplayer functionality or you want your water to behave the same way each time your game is played (good for intro etc.).")]
        [SerializeField, FormerlySerializedAs("seed")]
        private int _Seed;

        [SerializeField, FormerlySerializedAs("materials")] private WaterMaterials _Materials;
        [SerializeField, FormerlySerializedAs("profilesManager")] private ProfilesManager _ProfilesManager;
        [SerializeField, FormerlySerializedAs("geometry")] private WaterGeometry _Geometry;
        [SerializeField, FormerlySerializedAs("waterRenderer")] private WaterRenderer _WaterRenderer;
        [SerializeField, FormerlySerializedAs("uvAnimator")] private WaterUvAnimator _UVAnimator;
        [SerializeField, FormerlySerializedAs("volume")] private WaterVolume _Volume;
        [SerializeField, FormerlySerializedAs("subsurfaceScattering")] private WaterSubsurfaceScattering _SubsurfaceScattering;

        [SerializeField, FormerlySerializedAs("dynamicWaterData")] private DynamicWater.Data _DynamicWaterData;
        [SerializeField, FormerlySerializedAs("foamData")] private Foam.Data _FoamData;
        [SerializeField, FormerlySerializedAs("planarReflectionData")] private PlanarReflection.Data _PlanarReflectionData;
        [SerializeField, FormerlySerializedAs("windWavesData")] private WindWaves.Data _WindWavesData;

        [SerializeField, FormerlySerializedAs("dontRotateUpwards")] private bool _DontRotateUpwards;
        [SerializeField, FormerlySerializedAs("fastEnableDisable")] private bool _FastEnableDisable;

        [SerializeField, FormerlySerializedAs("version")]
#pragma warning disable 0414
        // ReSharper disable once NotAccessedField.Local
        private float _Version = 0.4f;
#pragma warning restore 0414
        #endregion Inspector Variables

        #region Unity Messages
        private void Awake()
        {
            WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
            WaterQualitySettings.Instance.Changed += OnQualitySettingsChanged;

            _Geometry.Awake(this);
            _WaterRenderer.Awake(this);
            _Materials.Awake(this);

            _ProfilesManager.Awake(this);
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;

            if (_FastEnableDisable && _ComponentsCreated)
                return;

            _IsPlaying = Application.isPlaying;          // can't access it from OnAfterDeserialize in other way

            CreateWaterComponents();

            if (!_ComponentsCreated)
            {
                return;
            }

            _ProfilesManager.OnEnable();
            _Geometry.OnEnable();
            _Modules.ForEach(x => x.Enable());

            if (_RenderingEnabled)
            {
                WaterSystem.Register(this);
            }
        }

        private void OnDisable()
        {
            if (!Application.isPlaying) return;

            if (_FastEnableDisable)
                return;

            _Modules.ForEach(x => x.Disable());
            _Geometry.OnDisable();
            _ProfilesManager.OnDisable();

            WaterSystem.Unregister(this);
        }

        private void OnDestroy()
        {
            if (_FastEnableDisable)
            {
                _FastEnableDisable = false;
                OnDisable();
            }

            WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;

            _Modules.ForEach(x => x.Destroy());
            _Modules.Clear();

            _Materials.OnDestroy();
            _ProfilesManager.OnDestroy();
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (!_DontRotateUpwards)
                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

            UpdateStatisticalData();

            _ProfilesManager.Update();
            _Geometry.Update();
            for (int i = 0; i < _Modules.Count; ++i)
            {
                _Modules[i].Update();
            }
        }

        public void OnValidate()
        {
            if (_ComponentsCreated)
            {
                _Modules.ForEach(x => x.Validate());

                _ProfilesManager.OnValidate();
                _Materials.OnValidate();
                _Geometry.OnValidate();
                _WaterRenderer.OnValidate();
            }
        }
        public void OnDrawGizmos()
        {
            if (_Geometry != null && _Geometry.GeometryType == WaterGeometry.Type.CustomMeshes)
            {
                var meshes = _Geometry.CustomSurfaceMeshes;
                for (int i = 0; i < meshes.Meshes.Length; ++i)
                {
                    Gizmos.matrix = transform.localToWorldMatrix;
                    Gizmos.color = Color.cyan * 0.2f;
                    Gizmos.DrawMesh(meshes.Meshes[i]);

                    Gizmos.color = Color.cyan * 0.4f;
                    Gizmos.DrawWireMesh(meshes.Meshes[i]);
                }
            }
        }
        #endregion Unity Messages

        #region Private Variables
        private readonly List<WaterModule> _Modules = new List<WaterModule>();

        private bool _ComponentsCreated;

        internal int _WaterId
        {
            get { return _WaterIdCache; }
            set
            {
                if (_WaterIdCache == value) { return; }

                _WaterIdCache = value;
                if (WaterIdChanged != null)
                {
                    WaterIdChanged.Invoke();
                }
            }
        }

        private Vector2 _SurfaceOffset = new Vector2(float.NaN, float.NaN);
        private float _Time = -1.0f;
        private bool _RenderingEnabled = true;

        public event Action WaterIdChanged;
        private int _WaterIdCache = -1;

        private static bool _IsPlaying;

        private static readonly Collider[] _CollidersBuffer = new Collider[30];
        private static readonly List<Water> _PossibleWaters = new List<Water>();
        private static readonly List<Water> _ExcludedWaters = new List<Water>();

        private static readonly float[] _CompensationStepWeights = { 0.85f, 0.75f, 0.83f, 0.77f, 0.85f, 0.75f, 0.85f, 0.75f, 0.83f, 0.77f, 0.85f, 0.75f, 0.85f, 0.75f };
        #endregion Private Variables

        #region Private Methods
        internal void OnWaterRender(WaterCamera waterCamera)
        {
            if (!isActiveAndEnabled) return;

            _Materials.OnWaterRender(waterCamera);
            for (int i = 0; i < _Modules.Count; ++i)
            {
                _Modules[i].OnWaterRender(waterCamera);
            }
        }

        internal void OnWaterPostRender(WaterCamera waterCamera)
        {
            for (int i = 0; i < _Modules.Count; ++i)
            {
                _Modules[i].OnWaterPostRender(waterCamera);
            }
        }
        internal void OnSamplingStarted()
        {
            ++ComputedSamplesCount;
        }

        internal void OnSamplingStopped()
        {
            --ComputedSamplesCount;
        }

        /// <summary>
        /// Creates some internal management classes, depending if they are needed by the used shader collection.
        /// </summary>
        private void CreateWaterComponents()
        {
            if (_ComponentsCreated)
                return;

            _ComponentsCreated = true;

            _Modules.Clear();
            _Modules.AddRange(new List<WaterModule> {
                _UVAnimator,
                _Volume,
                _SubsurfaceScattering
            });

            for (int i = 0; i < _Modules.Count; ++i)
            {
                _Modules[i].Start(this);
            }

            _ProfilesManager.Changed.AddListener(OnProfilesChanged);

            if (_ShaderSet.LocalEffectsSupported)
            {
                DynamicWater = new DynamicWater(this, _DynamicWaterData);
                _Modules.Add(DynamicWater);
            }

            if (_ShaderSet.PlanarReflections != PlanarReflectionsMode.Disabled)
            {
                PlanarReflection = new PlanarReflection(this, _PlanarReflectionData);
                _Modules.Add(PlanarReflection);
            }

            if (_ShaderSet.WindWavesMode != WindWavesRenderMode.Disabled)
            {
                WindWaves = new WindWaves(this, _WindWavesData);
                _Modules.Add(WindWaves);
            }

            if (_ShaderSet.Foam)
            {
                Foam = new Foam(this, _FoamData);   // has to be after wind waves
                _Modules.Add(Foam);
            }
        }

        internal void OnProfilesChanged(Water water)
        {
            var profiles = water.ProfilesManager.Profiles;

            Density = 0.0f;
            Gravity = 0.0f;

            for (int i = 0; i < profiles.Length; ++i)
            {
                var profile = profiles[i].Profile;
                float weight = profiles[i].Weight;

                Density += profile.Density * weight;
                Gravity -= profile.Gravity * weight;
            }
        }

        private void OnQualitySettingsChanged()
        {
            OnValidate();
        }

        private void UpdateStatisticalData()
        {
            MaxHorizontalDisplacement = 0.0f;
            MaxVerticalDisplacement = 0.0f;

            if (WindWaves != null)
            {
                MaxHorizontalDisplacement = WindWaves.MaxHorizontalDisplacement;
                MaxVerticalDisplacement = WindWaves.MaxVerticalDisplacement;
            }
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Water/Water (Base Component)", false, -1)]
        private static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var obj = new GameObject("Water");
            obj.AddComponent<Water>();

            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeObject = obj;
        }
#endif

        #endregion Private Methods
    }
}