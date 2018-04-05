namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DynamicWaterCameraData
    {
        #region Public Variables
        public RenderTexture DynamicDisplacementMap { get { return _Textures[(int)TextureTypes.Displacement]; } }
        public RenderTexture NormalMap { get { return _Textures[(int)TextureTypes.Normal]; } }
        public RenderTexture FoamMap { get { return _Textures[(int)TextureTypes.Foam]; } }
        public RenderTexture FoamMapPrevious { get { return _Textures[(int)TextureTypes.FoamPrevious]; } }
        public RenderTexture DisplacementsMask { get { return _Textures[(int)TextureTypes.DisplacementMask]; } }
        public RenderTexture DiffuseMap { get { return _Textures[(int)TextureTypes.Diffuse]; } }

        public DynamicWater DynamicWater
        {
            get { return _DynamicWater; }
        }

        public RenderTexture TotalDisplacementMap
        {
            get
            {
                var totalDisplacement = _Textures[(int)TextureTypes.TotalDisplacement];

                if (!_TotalDisplacementMapDirty)
                {
                    return totalDisplacement;
                }

                _DynamicWater.RenderTotalDisplacementMap(Camera, totalDisplacement);
                _TotalDisplacementMapDirty = false;

                return totalDisplacement;
            }
        }

        public WaterCamera Camera { get; private set; }
        #endregion Public Variables

        #region Public Methods
        public DynamicWaterCameraData(DynamicWater dynamicWater, WaterCamera camera, int antialiasing)
        {
            ResolveFormats();

            Camera = camera;

            _DynamicWater = dynamicWater;
            _Antialiasing = antialiasing;

            camera.RenderTargetResized += Camera_RenderTargetResized;
            CreateRenderTargets();
        }

        public RenderTexture GetDebugMap(bool createIfNotExists = false)
        {
            // check if debug texture was created
            if (!_Textures.ContainsKey((int)TextureTypes.Debug))
            {
                // if we do not want to create the texture
                if (!createIfNotExists)
                {
                    return null;
                }

                _Textures.Add((int)TextureTypes.Debug, DynamicDisplacementMap.CreateRenderTexture());
                _Textures[(int)TextureTypes.Debug].name = "[UWS] DynamicWaterCameraData - Debug";

                return _Textures[(int)TextureTypes.Debug];
            }

            return _Textures[(int)TextureTypes.Debug];
        }

        public RenderTexture GetTotalDisplacementMap()
        {
            return TotalDisplacementMap;
        }

        public void Dispose()
        {
            Camera.RenderTargetResized -= Camera_RenderTargetResized;
            DisposeTextures();
        }
        public void ClearOverlays()
        {
            ValidateRTs();

            var enumerator = _Textures.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;

                Graphics.SetRenderTarget(entry.Value);
                GL.Clear(false, true, entry.Key == (int)TextureTypes.DisplacementMask ? Color.white : Color.clear);
            }
            enumerator.Dispose();

            Graphics.SetRenderTarget(null);

            _TotalDisplacementMapDirty = true;
        }
        public void ValidateRTs()
        {
            var enumerator = _Textures.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;
                entry.Value.Verify();
            }
            enumerator.Dispose();
        }

        public void SwapFoamMaps()
        {
            var t = _Textures[(int)TextureTypes.FoamPrevious];
            _Textures[(int)TextureTypes.FoamPrevious] = _Textures[(int)TextureTypes.Foam];
            _Textures[(int)TextureTypes.Foam] = t;
        }
        #endregion Public Methods

        #region Private Types
        private enum TextureTypes
        {
            Displacement,
            DisplacementMask,
            Normal,
            Foam,
            FoamPrevious,
            Diffuse,
            Debug,
            TotalDisplacement
        }
        #endregion Private Types

        #region Private Variables
        internal int _LastFrameUsed;

        private bool _TotalDisplacementMapDirty;
        private readonly int _Antialiasing;
        private readonly DynamicWater _DynamicWater;

        private readonly Dictionary<int, RenderTexture> _Textures
            = new Dictionary<int, RenderTexture>();

        private RenderTextureFormat _DisplacementFormat;
        private RenderTextureFormat _NormalFormat;
        private RenderTextureFormat _FoamFormat;
        private RenderTextureFormat _DiffuseFormat;
        private RenderTextureFormat _TotalDisplacementFormat;
        #endregion Private Variables

        #region Private Methods
        private void DisposeTextures()
        {
            foreach (var entry in _Textures)
            {
                entry.Value.Destroy();
            }
            _Textures.Clear();
        }

        private void Camera_RenderTargetResized(WaterCamera camera)
        {
            CreateRenderTargets();
        }

        private void CreateRenderTargets()
        {
            DisposeTextures();

            int width = Mathf.RoundToInt(Camera.CameraComponent.pixelWidth);
            int height = Mathf.RoundToInt(Camera.CameraComponent.pixelHeight);

            var displacementDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Displacement")
            {
                Width = width,
                Height = height,
                Antialiasing = _Antialiasing,
                Format = _DisplacementFormat
            };
            var normalDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Normals")
            {
                Width = width,
                Height = height,
                Antialiasing = _Antialiasing,
                Format = _NormalFormat
            };
            var foamDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Foam")
            {
                Width = width,
                Height = height,
                Antialiasing = _Antialiasing,
                Format = _FoamFormat
            };
            var diffuseDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Diffuse")
            {
                Width = width,
                Height = height,
                Antialiasing = _Antialiasing,
                Format = _DiffuseFormat
            };
            var totalDisplacementDesc = new TextureUtility.RenderTextureDesc("[UWS] DynamicWaterCameraData - Total Displacement")
            {
                Width = 256,
                Height = 256,
                Antialiasing = 1,
                Format = _TotalDisplacementFormat
            };

            _Textures.Add((int)TextureTypes.Displacement, displacementDesc.CreateRenderTexture());
            _Textures.Add((int)TextureTypes.DisplacementMask, displacementDesc.CreateRenderTexture());
            _Textures.Add((int)TextureTypes.Normal, normalDesc.CreateRenderTexture());
            _Textures.Add((int)TextureTypes.Foam, foamDesc.CreateRenderTexture());
            _Textures.Add((int)TextureTypes.FoamPrevious, foamDesc.CreateRenderTexture());
            _Textures.Add((int)TextureTypes.Diffuse, diffuseDesc.CreateRenderTexture());

            _Textures.Add((int)TextureTypes.TotalDisplacement, totalDisplacementDesc.CreateRenderTexture());
        }

        private void ResolveFormats()
        {
            // displacement && total displacement
            var format = Compatibility.GetFormat(RenderTextureFormat.ARGBHalf, new[] {
                RenderTextureFormat.ARGBFloat
            });
            SetFormat(format, ref _DisplacementFormat);
            SetFormat(format, ref _TotalDisplacementFormat);

            // normal && foam
            format = Compatibility.GetFormat(RenderTextureFormat.RGHalf, new[] {
                RenderTextureFormat.RGFloat
            });
            SetFormat(format, ref _NormalFormat);
            SetFormat(format, ref _FoamFormat);

            // diffuse
            format = Compatibility.GetFormat(RenderTextureFormat.ARGB32, new[] {
                RenderTextureFormat.ARGBHalf,
                RenderTextureFormat.ARGBFloat
            });
            SetFormat(format, ref _DiffuseFormat);
        }

        private void SetFormat(RenderTextureFormat? src, ref RenderTextureFormat dest)
        {
            if (!src.HasValue)
            {
                Debug.LogError("Target device does not support DynamicWaterEffects texture formats");
                return;
            }

            dest = src.Value;
        }
        #endregion Private Methods
    }
}