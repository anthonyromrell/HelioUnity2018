namespace UltimateWater.Internal
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using System.Collections.Generic;
    using UnityEngine;

    #region Public Types
    public enum ShaderList
    {
        // depth
        DepthCopy,
        WaterDepth,

        // volumes
        VolumesFront,
        VolumesBack,
        VolumesFrontSimple,

        // dynamic
        Depth,
        Velocity,
        Simulation,
        Translate,

        // underwater
        ScreenSpaceMask,
        BaseIME,
        ComposeUnderWaterMask,

        // ime
        WaterdropsMask,
        WaterdropsNormal,

        // raindrops
        RaindropsFade,
        RaindropsFinal,
        RaindropsParticle,

        // refraction
        CollectLight,
        Transmission,

        // deferred
        GBuffer0Mix,
        GBuffer123Mix,
        FinalColorMix,
        DeferredReflections,
        DeferredShading,

        // utility
        ShorelineMask,
        ShorelineMaskSimple,
        Noise,
        ShadowEnforcer,
        MergeDisplacements
    }
    public enum ComputeShaderList
    {
        Simulation,
        Setup,
        Gauss,
        Transfer
    }
    #endregion Public Types

    public class ShaderUtility : ScriptableObjectSingleton
    {
        #region Public Methods
        public static ShaderUtility Instance
        {
            get { return _Instance != null ? _Instance : (_Instance = LoadSingleton<ShaderUtility>()); }
        }

        public Shader Get(ShaderList type)
        {
            if (_ShaderList == null)
            {
                InitializeShaderList();
            }

            AddReference(_ShaderList[(int)type]);
            return _ShaderList[(int)type];
        }
        public ComputeShader Get(ComputeShaderList type)
        {
            if (_ComputeShaderList == null)
            {
                InitializeComputeShaderList();
            }

            AddReference(_ComputeShaderList[(int)type]);
            return _ComputeShaderList[(int)type];
        }

        public Material CreateMaterial(ShaderList type, HideFlags flags = HideFlags.None)
        {
            return new Material(Get(type))
            {
                hideFlags = flags
            };
        }

        public void Use(ShaderList shader)
        {
            if (_ShaderList == null)
            {
                InitializeShaderList();
            }
            AddReference(_ShaderList[(int)shader]);
        }
        public void Use(ComputeShaderList shader)
        {
            if (_ComputeShaderList == null)
            {
                InitializeComputeShaderList();
            }
            AddReference(_ComputeShaderList[(int)shader]);
        }
        #endregion Public Methods

        #region Private Variables
        private Dictionary<int /*ShaderList*/, Shader> _ShaderList;
        private Dictionary<int /*ComputeShaderList*/, ComputeShader> _ComputeShaderList;

        [SerializeField] private List<Object> _References;

        private static ShaderUtility _Instance;
        #endregion Private Variables

        #region Private Methods
        private void InitializeShaderList()
        {
            _ShaderList = new Dictionary<int, Shader>
            {
                // depth
                { (int)ShaderList.DepthCopy, Shader.Find("UltimateWater/Depth/Depth Copy")},
                { (int)ShaderList.WaterDepth, Shader.Find("UltimateWater/Depth/Water Depth")},

                // volumes
                { (int)ShaderList.VolumesFront, Shader.Find("UltimateWater/Volumes/Front")},
                { (int)ShaderList.VolumesBack, Shader.Find("UltimateWater/Volumes/Back")},
                { (int)ShaderList.VolumesFrontSimple, Shader.Find("UltimateWater/Volumes/Front Simple")},

                // dynamic
                { (int)ShaderList.Depth, Shader.Find("UltimateWater/Dynamic/Depth") },
                { (int)ShaderList.Velocity, Shader.Find("UltimateWater/Dynamic/Velocity") },
                { (int)ShaderList.Simulation, Shader.Find("UltimateWater/Dynamic/Simulation") },
                { (int)ShaderList.Translate, Shader.Find("UltimateWater/Dynamic/Translate") },

                // underwater
                { (int)ShaderList.ScreenSpaceMask, Shader.Find("UltimateWater/Underwater/Screen-Space Mask")},
                { (int)ShaderList.BaseIME, Shader.Find("UltimateWater/Underwater/Base IME")},
                { (int)ShaderList.ComposeUnderWaterMask, Shader.Find("UltimateWater/Underwater/Compose Underwater Mask")},

                // ime
                { (int)ShaderList.WaterdropsMask, Shader.Find("UltimateWater/IME/Water Drops Mask")},
                { (int)ShaderList.WaterdropsNormal, Shader.Find("UltimateWater/IME/Water Drops Normal")},

                // raindrops
                { (int)ShaderList.RaindropsFinal, Shader.Find("UltimateWater/Raindrops/Final")},
                { (int)ShaderList.RaindropsFade, Shader.Find("UltimateWater/Raindrops/Fade")},
                { (int)ShaderList.RaindropsParticle, Shader.Find("UltimateWater/Raindrops/PreciseParticle")},

                // refraction
                { (int)ShaderList.CollectLight, Shader.Find("UltimateWater/Refraction/Collect Light")},
                { (int)ShaderList.Transmission, Shader.Find("UltimateWater/Refraction/Transmission")},

                // deferred
                { (int)ShaderList.GBuffer0Mix, Shader.Find("UltimateWater/Deferred/GBuffer0Mix")},
                { (int)ShaderList.GBuffer123Mix, Shader.Find("UltimateWater/Deferred/GBuffer123Mix")},
                { (int)ShaderList.FinalColorMix, Shader.Find("UltimateWater/Deferred/FinalColorMix")},
                { (int)ShaderList.DeferredReflections, Shader.Find("Hidden/UltimateWater-Internal-DeferredReflections")},
                { (int)ShaderList.DeferredShading, Shader.Find("Hidden/UltimateWater-Internal-DeferredShading")},

                // utility
                { (int)ShaderList.ShorelineMask, Shader.Find("UltimateWater/Utility/ShorelineMaskRender")},
                { (int)ShaderList.ShorelineMaskSimple, Shader.Find("UltimateWater/Utility/ShorelineMaskRenderSimple")},
                { (int)ShaderList.Noise, Shader.Find("UltimateWater/Utility/Noise")},
                { (int)ShaderList.ShadowEnforcer, Shader.Find("UltimateWater/Utility/ShadowEnforcer")},
                { (int)ShaderList.MergeDisplacements, Shader.Find("UltimateWater/Utility/MergeDisplacements")}
            };
        }

        private void InitializeComputeShaderList()
        {
            _ComputeShaderList = new Dictionary<int, ComputeShader>
            {
                { (int)ComputeShaderList.Simulation, Resources.Load<ComputeShader>("Shaders/Ripples - Simulation") },
                { (int)ComputeShaderList.Setup, Resources.Load<ComputeShader>("Shaders/Ripples - Setup") },
                { (int)ComputeShaderList.Gauss, Resources.Load<ComputeShader>("Shaders/Gauss") },
                { (int)ComputeShaderList.Transfer, Resources.Load<ComputeShader>("Shaders/Ripples - Transfer") }
            };
        }

        private void AddReference(Object obj)
        {
#if UNITY_EDITOR
            if (_References.Contains(obj))
            {
                return;
            }
            _References.Add(obj);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
        #endregion Private Methods
    }
}