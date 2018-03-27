namespace UltimateWater
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    #region Public Types
    [System.Serializable]
    public struct WaterQualityLevel
    {
        #region Inspector Variables
        [FormerlySerializedAs("name")]
        public string Name;

        [FormerlySerializedAs("maxSpectrumResolution")]
        [Tooltip("All simulations will be performed at most with this resolution")]
        public int MaxSpectrumResolution;

        [FormerlySerializedAs("allowHighPrecisionTextures")]
        public bool AllowHighPrecisionTextures;

        [FormerlySerializedAs("allowHighQualityNormalMaps")]
        public bool AllowHighQualityNormalMaps;

        [Range(0.0f, 1.0f)]
        [FormerlySerializedAs("tileSizeScale")]
        public float TileSizeScale;

        [FormerlySerializedAs("wavesMode")]
        public WaterWavesMode WavesMode;

        [FormerlySerializedAs("allowSpray")]
        public bool AllowSpray;

        [Range(0.0f, 1.0f)]
        [FormerlySerializedAs("foamQuality")]
        public float FoamQuality;

        [Range(0.0f, 1.0f)]
        [FormerlySerializedAs("maxTesselationFactor")]
        public float MaxTesselationFactor;

        [FormerlySerializedAs("maxVertexCount")]
        public int MaxVertexCount;

        [FormerlySerializedAs("maxTesselatedVertexCount")]
        public int MaxTesselatedVertexCount;

        [FormerlySerializedAs("allowAlphaBlending")]
        public bool AllowAlphaBlending;
        #endregion Inspector Variables

        #region Public Methods
        public void ResetToDefaults()
        {
            Name = "";
            MaxSpectrumResolution = 256;
            AllowHighPrecisionTextures = true;
            TileSizeScale = 1.0f;
            WavesMode = WaterWavesMode.AllowAll;
            AllowSpray = true;
            FoamQuality = 1.0f;
            MaxTesselationFactor = 1.0f;
            MaxVertexCount = 500000;
            MaxTesselatedVertexCount = 120000;
            AllowAlphaBlending = true;
        }
        #endregion Public Methods
    }

    [System.Serializable]
    public class WaterRipplesData
    {
        #region Public Types
        [System.Serializable]
        public enum ShaderModes
        {
            ComputeShader,
            PixelShader
        }
        #endregion Public Types

        #region Inspector Variables
        [Tooltip("Static objects that interact with water (terrain, pillars, rocks)")]
        [SerializeField]
        private LayerMask _StaticDepthMask;

        [Tooltip("How many simulation steps are performed per frame, the more iterations, the faster the water waves are, but it's expensive")]
        [SerializeField]
        private int _Iterations = 1;

        [Header("Texture formats")]
        [Tooltip("Simulation data (only R channel is used)")]
        [SerializeField]
        private RenderTextureFormat _SimulationFormat = RenderTextureFormat.RHalf;
        [Tooltip("Screen space simulation data gather (only R channel is used)")]
        [SerializeField]
        private RenderTextureFormat _GatherFormat = RenderTextureFormat.RGHalf;
        [Tooltip("Depth data (only R channel is used)")]
        [SerializeField]
        private RenderTextureFormat _DepthFormat = RenderTextureFormat.RHalf;

        [Header("Shaders")]
        [SerializeField]
        private ShaderModes _ShaderMode = ShaderModes.PixelShader;
        #endregion Inspector Variables

        #region Public Variables
        public int Iterations { get { return _Iterations; } }

        #region Shaders
        public ShaderModes ShaderMode
        {
            get { return _ShaderMode; }
        }
        #endregion Shaders

        #region Masks
        public LayerMask StaticDepthMask
        {
            get
            {
                return _StaticDepthMask;
            }
        }
        #endregion Masks

        #region Texture Formats
        public RenderTextureFormat SimulationFormat
        {
            get
            {
                return _SimulationFormat;
            }
        }
        public RenderTextureFormat GatherFormat
        {
            get
            {
                return _GatherFormat;
            }
        }
        public RenderTextureFormat DepthFormat
        {
            get
            {
                return _DepthFormat;
            }
        }
        #endregion Texture Formats
        #endregion Public Variables
    }

    public enum WaterWavesMode
    {
        AllowAll,
        AllowNormalFFT,
        AllowGerstner,
        DisallowAll
    }
    #endregion Public Types

    public class WaterQualitySettings : ScriptableObjectSingleton
    {
        #region Public Variables
        public WaterQualityLevel CurrentQualityLevel
        {
            get { return _CurrentQualityLevel; }
        }
        public WaterRipplesData Ripples;

        public event System.Action Changed;

        public string[] Names
        {
            get
            {
                string[] names = new string[_QualityLevels.Length];

                for (int i = 0; i < _QualityLevels.Length; ++i)
                    names[i] = _QualityLevels[i].Name;

                return names;
            }
        }

        public WaterWavesMode WavesMode
        {
            get { return _CurrentQualityLevel.WavesMode; }
            set
            {
                if (_CurrentQualityLevel.WavesMode == value)
                    return;

                _CurrentQualityLevel.WavesMode = value;
                OnChange();
            }
        }

        public int MaxSpectrumResolution
        {
            get { return _CurrentQualityLevel.MaxSpectrumResolution; }
            set
            {
                if (_CurrentQualityLevel.MaxSpectrumResolution == value)
                    return;

                _CurrentQualityLevel.MaxSpectrumResolution = value;
                OnChange();
            }
        }
        public float TileSizeScale
        {
            get { return _CurrentQualityLevel.TileSizeScale; }
            set
            {
                if (_CurrentQualityLevel.TileSizeScale == value)
                    return;

                _CurrentQualityLevel.TileSizeScale = value;
                OnChange();
            }
        }

        public bool AllowHighPrecisionTextures
        {
            get { return _CurrentQualityLevel.AllowHighPrecisionTextures; }
            set
            {
                if (_CurrentQualityLevel.AllowHighPrecisionTextures == value)
                    return;

                _CurrentQualityLevel.AllowHighPrecisionTextures = value;
                OnChange();
            }
        }
        public bool AllowHighQualityNormalMaps
        {
            get { return _CurrentQualityLevel.AllowHighQualityNormalMaps; }
            set
            {
                if (_CurrentQualityLevel.AllowHighQualityNormalMaps == value)
                    return;

                _CurrentQualityLevel.AllowHighQualityNormalMaps = value;
                OnChange();
            }
        }

        public float MaxTesselationFactor
        {
            get { return _CurrentQualityLevel.MaxTesselationFactor; }
            set
            {
                if (_CurrentQualityLevel.MaxTesselationFactor == value)
                    return;

                _CurrentQualityLevel.MaxTesselationFactor = value;
                OnChange();
            }
        }
        public int MaxVertexCount
        {
            get { return _CurrentQualityLevel.MaxVertexCount; }
            set
            {
                if (_CurrentQualityLevel.MaxVertexCount == value)
                    return;

                _CurrentQualityLevel.MaxVertexCount = value;
                OnChange();
            }
        }
        public int MaxTesselatedVertexCount
        {
            get { return _CurrentQualityLevel.MaxTesselatedVertexCount; }
            set
            {
                if (_CurrentQualityLevel.MaxTesselatedVertexCount == value)
                    return;

                _CurrentQualityLevel.MaxTesselatedVertexCount = value;
                OnChange();
            }
        }
        public bool AllowAlphaBlending
        {
            get { return _CurrentQualityLevel.AllowAlphaBlending; }
            set
            {
                if (_CurrentQualityLevel.AllowAlphaBlending == value)
                    return;

                _CurrentQualityLevel.AllowAlphaBlending = value;
                OnChange();
            }
        }

        public static WaterQualitySettings Instance
        {
            get
            {
                // ReSharper disable once RedundantCast.0
                if ((object)_Instance == null || _Instance == null)
                {
                    _Instance = LoadSingleton<WaterQualitySettings>();
                    _Instance.Changed = null;
                    _Instance._WaterQualityIndex = -1;
                    _Instance.SynchronizeQualityLevel();
                }

                return _Instance;
            }
        }

        /// <summary>
        /// Are water quality settings synchronized with Unity?
        /// </summary>
        public bool SynchronizeWithUnity
        {
            get { return _SynchronizeWithUnity; }
        }
        #endregion Public Variables

        #region Public Methods
        public int GetQualityLevel()
        {
            return _WaterQualityIndex;
        }

        public void SetQualityLevel(int index)
        {
            if (!Application.isPlaying)
                _SavedCustomQualityLevel = index;

            _CurrentQualityLevel = _QualityLevels[index];
            _WaterQualityIndex = index;

            OnChange();
        }

        /// <summary>
        /// Synchronizes current water quality level with the one set in Unity quality settings.
        /// </summary>
        public void SynchronizeQualityLevel()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && _SynchronizeWithUnity)
                SynchronizeLevelNames();
#endif

            int currentQualityIndex = -1;

            if (_SynchronizeWithUnity)
                currentQualityIndex = FindQualityLevel(QualitySettings.names[QualitySettings.GetQualityLevel()]);

            if (currentQualityIndex == -1)
                currentQualityIndex = _SavedCustomQualityLevel;

            currentQualityIndex = Mathf.Clamp(currentQualityIndex, 0, _QualityLevels.Length - 1);

            if (currentQualityIndex != _WaterQualityIndex)
                SetQualityLevel(currentQualityIndex);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, FormerlySerializedAs("qualityLevels")]
        private WaterQualityLevel[] _QualityLevels;

        [SerializeField, FormerlySerializedAs("synchronizeWithUnity")]
        private bool _SynchronizeWithUnity = true;

        [SerializeField, FormerlySerializedAs("savedCustomQualityLevel")]
        private int _SavedCustomQualityLevel;
        #endregion Inspector Variables

        #region Private Variables
        private int _WaterQualityIndex;
        // working copy of the current quality level free for temporary modifications
        private WaterQualityLevel _CurrentQualityLevel;
        private static WaterQualitySettings _Instance;
        #endregion Private Variables

        #region Private Methods
        internal WaterQualityLevel[] GetQualityLevelsDirect()
        {
            return _QualityLevels;
        }
        private void OnChange()
        {
            if (Changed != null)
                Changed();
        }

        private int FindQualityLevel(string levelName)
        {
            for (int i = 0; i < _QualityLevels.Length; ++i)
            {
                if (_QualityLevels[i].Name == levelName)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Ensures that water quality settings are named the same way that Unity ones.
        /// </summary>
        private void SynchronizeLevelNames()
        {
#if UNITY_EDITOR
            if (_QualityLevels == null)
                _QualityLevels = new WaterQualityLevel[0];

            string[] unityNames = QualitySettings.names;
            var currentNames = Names;
            int index = 0;

            // check if the names are created
            if (currentNames != null && currentNames.Length == unityNames.Length)
            {
                bool noChanges = true;
                foreach (var levelName in currentNames)
                {
                    if (unityNames[index] != levelName)
                    {
                        noChanges = false;
                        break;
                    }
                }

                if (noChanges)
                    return;
            }

            var matches = new List<WaterQualityLevel>();
            var availableLevels = new List<WaterQualityLevel>(_QualityLevels);
            var availableUnityLevels = new List<string>(unityNames);

            // keep existing levels with matching names
            for (int i = 0; i < availableLevels.Count; ++i)
            {
                var level = availableLevels[i];

                if (availableUnityLevels.Contains(level.Name))
                {
                    matches.Add(level);
                    availableLevels.RemoveAt(i--);
                    availableUnityLevels.Remove(level.Name);
                }
            }

            // use non-matched levels as-is // possibly just their name or order has changed
            while (availableLevels.Count > 0 && availableUnityLevels.Count > 0)
            {
                var level = availableLevels[0];
                level.Name = availableUnityLevels[0];

                matches.Add(level);

                availableLevels.RemoveAt(0);
                availableUnityLevels.RemoveAt(0);
            }

            // create new levels if there is more of them left
            while (availableUnityLevels.Count > 0)
            {
                var level = new WaterQualityLevel();
                level.ResetToDefaults();
                level.Name = availableUnityLevels[0];

                matches.Add(level);

                availableUnityLevels.RemoveAt(0);
            }

            // create new list with the same order as in Unity
            _QualityLevels = new WaterQualityLevel[unityNames.Length];

            for (int i = 0; i < _QualityLevels.Length; ++i)
                _QualityLevels[i] = matches.Find(l => l.Name == unityNames[i]);

            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        #endregion Private Methods
    }
}