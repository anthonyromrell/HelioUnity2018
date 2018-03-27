using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class WaterProjectSettings : ScriptableObjectSingleton
    {
        #region Public Types
        public enum AbsorptionEditMode
        {
            Absorption,
            Transmission
        }

        public enum SpecularEditMode
        {
            IndexOfRefraction,
            CustomColor
        }
        #endregion Public Types

        #region Public Variables
        public static WaterProjectSettings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = LoadSingleton<WaterProjectSettings>();
                }

                return _Instance;
            }
        }

        public bool ClipWaterCameraRange { get { return _ClipWaterCameraRange; } }
        public float CameraClipRange { get { return _CameraClipRange; } }

        public int PhysicsThreads
        {
            get { return _PhysicsThreads; }
            set { _PhysicsThreads = value; }
        }

        public int WaterLayer
        {
            get { return _WaterLayer; }
        }

        public int WaterTempLayer
        {
            get { return _WaterTempLayer; }
        }

        public int WaterCollidersLayer
        {
            get { return _WaterCollidersLayer; }
        }

        public System.Threading.ThreadPriority PhysicsThreadsPriority
        {
            get { return _PhysicsThreadsPriority; }
        }

        public bool AllowCpuFFT
        {
            get { return _AllowCpuFFT; }
        }

        public bool AllowFloatingPointMipMaps
        {
            get
            {
                if (_AllowFloatingPointMipMapsChecked)
                {
                    return _AllowFloatingPointMipMaps && _AllowFloatingPointMipMapsOverride;
                }
                _AllowFloatingPointMipMapsChecked = true;

                var vendor = SystemInfo.graphicsDeviceVendor.ToLowerInvariant();
                _AllowFloatingPointMipMaps = !vendor.Contains("amd")
                                             && !vendor.Contains("ati")
                                             && !SystemInfo.graphicsDeviceName.ToLowerInvariant().Contains("radeon");

                return _AllowFloatingPointMipMaps && _AllowFloatingPointMipMapsOverride;
            }
        }

        // Accessed in WaterProfileEditor through SerializedObject
        public AbsorptionEditMode InspectorAbsorptionEditMode
        {
            get { return _AbsorptionEditMode; }
        }

        // Accessed in WaterProfileEditor through SerializedObject
        public SpecularEditMode InspectorSpecularEditMode
        {
            get { return _SpecularEditMode; }
        }

        public bool DebugPhysics
        {
            get { return _DebugPhysics; }
        }

        public bool AskForWaterCameras
        {
            get { return _AskForWaterCameras; }
            set { _AskForWaterCameras = value; }
        }

        public bool SinglePassStereoRendering
        {
            get { return _SinglePassStereoRendering; }
        }

        public bool RenderInSceneView
        {
            get { return _RenderInSceneView; }
            set { _RenderInSceneView = value; }
        }

        public static readonly float CurrentVersion = 2.1f;
        public static readonly string CurrentVersionString = "2.1.0";
        #endregion Public Variables

        #region Public Methods
#if UNITY_EDITOR
        static WaterProjectSettings()
        {
            // ReSharper disable once DelegateSubtraction
            UnityEditor.EditorApplication.update -= Validate;
            UnityEditor.EditorApplication.update += Validate;
        }

        private static void Validate()
        {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
            bool singlePassStereoRendering = false;
#elif UNITY_5_4
            bool singlePassStereoRendering = UnityEditor.PlayerSettings.virtualRealitySupported && UnityEditor.PlayerSettings.singlePassStereoRendering;
#else
            bool singlePassStereoRendering = UnityEditor.PlayerSettings.virtualRealitySupported && UnityEditor.PlayerSettings.stereoRenderingPath != UnityEditor.StereoRenderingPath.MultiPass;
#endif

            if (Instance._SinglePassStereoRendering != singlePassStereoRendering)
            {
                Instance._SinglePassStereoRendering = singlePassStereoRendering;
                UnityEditor.EditorUtility.SetDirty(Instance);
            }

            if (Instance._DisableMultisampling)
            {
                QualitySettings.antiAliasing = 1;
            }
        }
#endif
        #endregion Public Methods

        #region Inspector Variables

#pragma warning disable 0414
        [SerializeField, FormerlySerializedAs("serializedVersion")]
        // ReSharper disable once NotAccessedField.Local
        private float _SerializedVersion = 1.0f;
#pragma warning restore 0414

        [SerializeField, FormerlySerializedAs("waterLayer")]
        private int _WaterLayer = 4;

        [Tooltip("Used for some camera effects. Has to be unused. You don't need to mask it on your cameras.")]
        [SerializeField, FormerlySerializedAs("waterTempLayer")]
        private int _WaterTempLayer = 22;

        [Tooltip("UltimateWater internally uses colliders to detect camera entering into subtractive volumes etc. You will have to ignore this layer in your scripting raycasts.")]
        [SerializeField, FormerlySerializedAs("waterCollidersLayer")]
        private int _WaterCollidersLayer = 1;

        [Tooltip("More threads increase physics precision under stress, but also decrease overall performance a bit.")]
        [SerializeField, FormerlySerializedAs("physicsThreads")]
        private int _PhysicsThreads = 1;

        [SerializeField, FormerlySerializedAs("physicsThreadsPriority")]
        private System.Threading.ThreadPriority _PhysicsThreadsPriority = System.Threading.ThreadPriority.BelowNormal;

        [SerializeField, FormerlySerializedAs("allowCpuFFT")]
        private bool _AllowCpuFFT = true;

        [Tooltip("Some hardware doesn't support floating point mip maps correctly and they are forcefully disabled. You may simulate how the water would look like on such hardware by disabling this option. Most notably fp mip maps don't work correctly on most AMD graphic cards (for now).")]
        [SerializeField, FormerlySerializedAs("allowFloatingPointMipMaps")]
        private bool _AllowFloatingPointMipMapsOverride = true;

        [SerializeField, FormerlySerializedAs("debugPhysics")]
        private bool _DebugPhysics;

        [SerializeField, FormerlySerializedAs("askForWaterCameras")]
        private bool _AskForWaterCameras = true;

        [SerializeField, FormerlySerializedAs("absorptionEditMode")]
        private AbsorptionEditMode _AbsorptionEditMode = AbsorptionEditMode.Transmission;

        [SerializeField, FormerlySerializedAs("specularEditMode")]
        private SpecularEditMode _SpecularEditMode = SpecularEditMode.IndexOfRefraction;

        [SerializeField] private bool _SinglePassStereoRendering;
        [SerializeField] private bool _DisableMultisampling = true;

        [SerializeField] private bool _ClipWaterCameraRange;
        [SerializeField] private float _CameraClipRange = 1000.0f;
        [SerializeField] private bool _RenderInSceneView;
        #endregion Inspector Variables

        #region Private Variables
        private static WaterProjectSettings _Instance;

        private static bool _AllowFloatingPointMipMaps;
        private static bool _AllowFloatingPointMipMapsChecked;
        #endregion Private Variables
    }
}