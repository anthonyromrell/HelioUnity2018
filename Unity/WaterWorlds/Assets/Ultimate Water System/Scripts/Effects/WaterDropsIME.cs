using UltimateWater.Utils;

namespace UltimateWater
{
    using UnityEngine;
    using Internal;

    public class WaterDropsIME : MonoBehaviour, IWaterImageEffect
    {
        #region Public Types
        public enum Type
        {
            NormalMap,
            Blur
        }

        public interface IWaterDropsModule
        {
            void Initialize(WaterDropsIME ime);

            void Validate();
            void Advance();

            void UpdateMask(RenderTexture mask);
            void RenderImage(RenderTexture source, RenderTexture destination);
        }

        [System.Serializable]
        public struct BlurModule : IWaterDropsModule
        {
            #region Public Variables
            public float FadeSpeed;
            #endregion Public Variables

            #region Public Methods
            public void Initialize(WaterDropsIME effect)
            {
                _Reference = effect;
                _Camera = effect.GetComponent<WaterCamera>();
            }
            public void Validate()
            {
                _Blur.Validate("UltimateWater/Utilities/Blur (VisionBlur)");
            }

            public void Advance()
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                _Intensity += Mathf.Max(0.0f, _Camera.WaterLevel - _Reference.transform.position.y);
                _Intensity *= 1.0f - Time.deltaTime * FadeSpeed;

                _Intensity = Mathf.Clamp01(_Intensity);
            }

            public void RenderImage(RenderTexture source, RenderTexture destination)
            {
                float blurSize = _Blur.Size;
                _Blur.Size *= _Intensity;
                _Blur.Apply(source);
                _Blur.Size = blurSize;

                Graphics.Blit(source, destination);
            }
            #endregion Public Methods

            #region Private Variables
            private WaterDropsIME _Reference;
            private WaterCamera _Camera;
            private float _Intensity;

            [HideInInspector] [SerializeField] private Blur _Blur;
            #endregion Private Variables

            #region Unused Interfaces
            public void UpdateMask(RenderTexture mask)
            {
            }
            #endregion Unused Interfaces
        }

        [System.Serializable]
        public struct NormalModule : IWaterDropsModule
        {
            #region Public Variables
            public Texture NormalMap;
            public float Intensity;
            public bool Preview;
            #endregion Public Variables

            #region Public Methods
            public void Initialize(WaterDropsIME effect)
            {
                _Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsNormal);
                _Material.SetTexture("_NormalMap", NormalMap);
            }

            public void UpdateMask(RenderTexture mask)
            {
                if (Preview)
                {
                    Graphics.Blit(DefaultTextures.Get(Color.white), mask);
                }

                _Material.SetTexture("_Mask", mask);
            }
            public void RenderImage(RenderTexture source, RenderTexture destination)
            {
                if (!Application.isPlaying && !Preview)
                {
                    Graphics.Blit(source, destination);
                    return;
                }

                _Material.SetFloat("_Intensity", Intensity);
                Graphics.Blit(source, destination, _Material);
            }

            public void Validate()
            {
                ShaderUtility.Instance.Use(ShaderList.WaterdropsNormal);

                if (_Material == null)
                {
                    _Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsNormal);
                }
                _Material.SetTexture("_NormalMap", NormalMap);
            }
            #endregion Public Methods

            #region Private Variables
            private Material _Material;
            #endregion Private Variables

            #region Unused Interfaces
            public void Advance()
            {
            }
            #endregion Unused Interfaces
        }
        #endregion Public Types

        #region Public Variables
        public RenderTexture Mask
        {
            get { return _Masking._MaskB; }
        }

        [Range(0.95f, 1.0f)]
        [Tooltip("How slow the effects disappear")]
        public float Fade = 1.0f;

        public BlurModule Blur;
        public NormalModule Normal;
        #endregion Public Variables

        #region Public Methods
        public void OnWaterCameraEnabled()
        {
        }

        public void OnWaterCameraPreCull()
        {
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Type _Type = Type.NormalMap;
        #endregion Inspector Variables

        #region Unity Messages
        private void Awake()
        {
            _Masking._Material = ShaderUtility.Instance.CreateMaterial(ShaderList.WaterdropsMask);

            AssignModule();
        }

        private void OnValidate()
        {
            ShaderUtility.Instance.Use(ShaderList.WaterdropsMask);

            AssignModule();

            _SelectedModule.Validate();
            if (Application.isPlaying && _Masking._Material != null)
            {
                _Masking._Material.SetFloat("_Fadeout", Fade * 0.98f);
            }
        }

        private void OnPreCull()
        {
            _SelectedModule.Advance();
        }
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Application.isPlaying)
            {
                CheckResources();

                _Masking.Blit();
                _Masking.Swap();

                _SelectedModule.UpdateMask(_Masking._MaskB);
            }

            _SelectedModule.RenderImage(source, destination);
        }
        private void Reset()
        {
            Normal.Intensity = 1.0f;
            Blur.FadeSpeed = 1.0f;
        }

        private void OnDestroy()
        {
            _Masking.Release();
        }
        #endregion Unity Messages

        #region Private Types
        private struct MaskingModule
        {
            #region Public Variables
            internal Material _Material;
            internal RenderTexture _MaskA;
            internal RenderTexture _MaskB;
            #endregion Public Variables

            #region Public Methods
            internal void Blit()
            {
                Graphics.Blit(_MaskA, _MaskB, _Material, 0);
            }
            internal void Swap()
            {
                var temp = _MaskA;
                _MaskA = _MaskB;
                _MaskB = temp;
            }
            internal void Release()
            {
                TextureUtility.Release(ref _MaskA);
                TextureUtility.Release(ref _MaskB);
            }
            #endregion Public Methods
        }
        #endregion Private Types

        #region Private Variables
        private MaskingModule _Masking;
        private IWaterDropsModule _SelectedModule;
        #endregion Private Variables

        #region Private Methods
        private void AssignModule()
        {
            switch (_Type)
            {
                case Type.Blur: _SelectedModule = Blur; break;
                case Type.NormalMap: _SelectedModule = Normal; break;
            }
            _SelectedModule.Initialize(this);
        }

        private void CheckResources()
        {
            if (_Masking._MaskA == null || _Masking._MaskA.width != Screen.width >> 1 || _Masking._MaskA.height != Screen.height >> 1)
            {
                _Masking._MaskA = CreateMaskRt();
                _Masking._MaskB = CreateMaskRt();

                _Masking._MaskA.name = "[UWS] WaterDropsIME - Mask A";
                _Masking._MaskB.name = "[UWS] WaterDropsIME - Mask B";
            }
        }

        private static RenderTexture CreateMaskRt()
        {
            var renderTexture = new RenderTexture(Screen.width >> 1, Screen.height >> 1, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear)
            {
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                name = "[UWS] WaterDropsIME - CreateMaskRt"
            };

            Graphics.SetRenderTarget(renderTexture);
            GL.Clear(false, true, Color.black);

            return renderTexture;
        }
        #endregion Private Methods

        #region Validation
        [InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
        [SerializeField]
        private string _Validation;
        // ReSharper disable once UnusedMember.Local
        private string Validation()
        {
            string info = string.Empty;
            if (_Type == Type.NormalMap && Normal.NormalMap == null)
            {
                info += "warning: select normal map texture";
            }

            return info;
        }
        #endregion Validation
    }
}