namespace UltimateWater
{
    using UnityEngine;
    using UnityEngine.Serialization;

    [RequireComponent(typeof(Water))]
    public sealed class WaterNormalMapAnimation : MonoBehaviour
    {
        #region Inspector Variables
        [HideInInspector, SerializeField, FormerlySerializedAs("normalMapShader")]
        private Shader _NormalMapShader;

        [SerializeField, FormerlySerializedAs("resolution")]
        private int _Resolution = 512;

        [SerializeField, FormerlySerializedAs("period")]
        private float _Period = 60.0f;

        [SerializeField, FormerlySerializedAs("animationSpeed")]
        private float _AnimationSpeed = 0.015f;

        [SerializeField, FormerlySerializedAs("intensity")]
        private float _Intensity = 2.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            OnValidate();

            _NormalMap1 = new RenderTexture(_Resolution, _Resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
            {
                name = "[UWS] WaterNormalMapAnimation - Normal Map 1",
                wrapMode = TextureWrapMode.Repeat
            };

            _NormalMapMaterial = new Material(_NormalMapShader) { hideFlags = HideFlags.DontSave };

            _Water = GetComponent<Water>();
            _SourceNormalMap = _Water.Materials.SurfaceMaterial.GetTexture("_BumpMap");
            _Water.Materials.SurfaceMaterial.SetTexture("_BumpMap", _NormalMap1);
        }

        private void OnValidate()
        {
            if (_NormalMapShader == null)
                _NormalMapShader = Shader.Find("UltimateWater/Utilities/WaterNormalMap");
        }

        private void Update()
        {
            _NormalMapMaterial.SetVector(ShaderVariables.Offset, new Vector4(0.0f, 0.0f, Time.time * _AnimationSpeed, 0.0f));
            _NormalMapMaterial.SetVector(ShaderVariables.Period, new Vector4(_Period, _Period, _Period, _Period));
            _NormalMapMaterial.SetFloat("_Param", _Intensity);
            Graphics.Blit(_SourceNormalMap, _NormalMap1, _NormalMapMaterial, 0);
        }
        #endregion Unity Methods

        #region Private Variables
        private Water _Water;
        private RenderTexture _NormalMap1;
        private Texture _SourceNormalMap;
        private Material _NormalMapMaterial;
        #endregion Private Variables
    }
}