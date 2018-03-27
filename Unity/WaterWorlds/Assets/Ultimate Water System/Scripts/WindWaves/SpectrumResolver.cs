namespace UltimateWater.Internal
{
    using UnityEngine;

    /// <summary>
    ///     Renders all types of water spectra and animates them in time on CPU and GPU. This class in hierarchy contains GPU
    ///     code.
    /// </summary>
    public class SpectrumResolver : SpectrumResolverCPU
    {
        #region Public Types
        public enum SpectrumType
        {
            Height,
            Normal,
            Displacement,
            RawDirectional,
            RawOmnidirectional
        }
        #endregion Public Types

        #region Public Methods
        public SpectrumResolver(Water water, WindWaves windWaves, Shader spectrumShader) : base(water, windWaves, 4)
        {
            _Water = water;
            _WindWaves = windWaves;

            _AnimationMaterial = new Material(spectrumShader) { hideFlags = HideFlags.DontSave };
            _AnimationMaterial.SetFloat(ShaderVariables.RenderTime, Time.time);

            if (windWaves.LoopDuration != 0.0f)
            {
                _AnimationMaterial.EnableKeyword("_LOOPING");
                _AnimationMaterial.SetFloat("_LoopDuration", windWaves.LoopDuration);
            }
        }

        public Texture TileSizeLookup
        {
            get
            {
                ValidateTileSizeLookup();
                return _TileSizeLookup;
            }
        }

        public float RenderTime { get; private set; }

        public Texture RenderHeightSpectrumAt(float time)
        {
            CheckResources();

            var directionalSpectrum = GetRawDirectionalSpectrum();

            RenderTime = time;
            _AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
            Graphics.Blit(directionalSpectrum, _HeightSpectrum, _AnimationMaterial, 0);

            return _HeightSpectrum;
        }

        public Texture RenderNormalsSpectrumAt(float time)
        {
            CheckResources();

            var directionalSpectrum = GetRawDirectionalSpectrum();

            RenderTime = time;
            _AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
            Graphics.Blit(directionalSpectrum, _NormalSpectrum, _AnimationMaterial, 1);

            return _NormalSpectrum;
        }

        public void RenderDisplacementsSpectraAt(float time, out Texture height, out Texture displacement)
        {
            CheckResources();

            height = _HeightSpectrum;
            displacement = _DisplacementSpectrum;

            // it's necessary to set it each frame for some reason
            _RenderTargetsx2[0] = _HeightSpectrum.colorBuffer;
            _RenderTargetsx2[1] = _DisplacementSpectrum.colorBuffer;

            var directionalSpectrum = GetRawDirectionalSpectrum();

            RenderTime = time;
            _AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
            Graphics.SetRenderTarget(_RenderTargetsx2, _HeightSpectrum.depthBuffer);
            Graphics.Blit(directionalSpectrum, _AnimationMaterial, 5);
            Graphics.SetRenderTarget(null);
        }

        public void RenderCompleteSpectraAt(float time, out Texture height, out Texture normal, out Texture displacement)
        {
            CheckResources();

            height = _HeightSpectrum;
            normal = _NormalSpectrum;
            displacement = _DisplacementSpectrum;

            // it's necessary to set it each frame for some reason
            _RenderTargetsx3[0] = _HeightSpectrum.colorBuffer;
            _RenderTargetsx3[1] = _NormalSpectrum.colorBuffer;
            _RenderTargetsx3[2] = _DisplacementSpectrum.colorBuffer;

            var directionalSpectrum = GetRawDirectionalSpectrum();

            RenderTime = time;
            _AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
            Graphics.SetRenderTarget(_RenderTargetsx3, _HeightSpectrum.depthBuffer);
            Graphics.Blit(directionalSpectrum, _AnimationMaterial, 2);
            Graphics.SetRenderTarget(null);
        }

        public Texture GetSpectrum(SpectrumType type)
        {
            switch (type)
            {
                case SpectrumType.Height: return _HeightSpectrum;
                case SpectrumType.Normal: return _NormalSpectrum;
                case SpectrumType.Displacement: return _DisplacementSpectrum;
                case SpectrumType.RawDirectional: return _DirectionalSpectrum;
                case SpectrumType.RawOmnidirectional: return _OmnidirectionalSpectrum;
                default: throw new System.InvalidOperationException();
            }
        }

        public override void AddSpectrum(WaterWavesSpectrumDataBase spectrum)
        {
            base.AddSpectrum(spectrum);
            _DirectionalSpectrumDirty = true;
        }

        public override void RemoveSpectrum(WaterWavesSpectrumDataBase spectrum)
        {
            base.RemoveSpectrum(spectrum);
            _DirectionalSpectrumDirty = true;
        }

        public override void SetDirectionalSpectrumDirty()
        {
            base.SetDirectionalSpectrumDirty();

            _DirectionalSpectrumDirty = true;
        }
        #endregion Public Methods

        #region Private Variables
        private Texture2D _TileSizeLookup;           // 2x2 tile sizes tex
        private Texture _OmnidirectionalSpectrum;
        private RenderTexture _TotalOmnidirectionalSpectrum;
        private RenderTexture _DirectionalSpectrum;
        private RenderTexture _HeightSpectrum, _NormalSpectrum, _DisplacementSpectrum;
        private RenderBuffer[] _RenderTargetsx2;
        private RenderBuffer[] _RenderTargetsx3;

        private bool _TileSizesLookupDirty = true;
        private bool _DirectionalSpectrumDirty = true;
        private Vector4 _TileSizes;
        private Mesh _SpectrumDownsamplingMesh;

        private readonly Material _AnimationMaterial;
        private readonly Water _Water;
        private readonly WindWaves _WindWaves;
        #endregion Private Variables

        #region Private Methods
        internal override void OnProfilesChanged()
        {
            base.OnProfilesChanged();

            if (_TileSizes != _WindWaves.TileSizes)
            {
                _TileSizesLookupDirty = true;
                _TileSizes = _WindWaves.TileSizes;
            }

            RenderTotalOmnidirectionalSpectrum();
        }

        private void RenderTotalOmnidirectionalSpectrum()
        {
            _AnimationMaterial.SetFloat("_Gravity", _Water.Gravity);
            _AnimationMaterial.SetVector("_TargetResolution", new Vector4(_WindWaves.FinalResolution, _WindWaves.FinalResolution, 0.0f, 0.0f));

            var profiles = _Water.ProfilesManager.Profiles;

            if (profiles.Length > 1)
            {
                var totalOmnidirectionalSpectrum = GetTotalOmnidirectionalSpectrum();

                totalOmnidirectionalSpectrum.Clear(Color.black);

                for (int i = 0; i < profiles.Length; ++i)
                {
                    var weightedProfile = profiles[i];

                    if (weightedProfile.Weight <= 0.0001f)
                        continue;

                    var spectrum = weightedProfile.Profile.Spectrum;

                    WaterWavesSpectrumData spectrumData;

                    if (!_SpectraDataCache.TryGetValue(spectrum, out spectrumData))
                        spectrumData = GetSpectrumData(spectrum);

                    _AnimationMaterial.SetFloat("_Weight", spectrumData.Weight);
                    Graphics.Blit(spectrumData.Texture, totalOmnidirectionalSpectrum, _AnimationMaterial, 4);
                }

                _OmnidirectionalSpectrum = totalOmnidirectionalSpectrum;
            }
            else if (profiles.Length != 0)
            {
                var spectrum = profiles[0].Profile.Spectrum;
                WaterWavesSpectrumData spectrumData;

                if (!_SpectraDataCache.TryGetValue(spectrum, out spectrumData))
                    spectrumData = GetSpectrumData(spectrum);

                spectrumData.Weight = 1.0f;
                _OmnidirectionalSpectrum = spectrumData.Texture;
            }

            _Water.Renderer.PropertyBlock.SetFloat("_MaxDisplacement", MaxHorizontalDisplacement);
        }

        private void RenderDirectionalSpectrum()
        {
            if (_OmnidirectionalSpectrum == null)
                RenderTotalOmnidirectionalSpectrum();

            ValidateTileSizeLookup();

            _AnimationMaterial.SetFloat("_Directionality", 1.0f - _WindWaves.SpectrumDirectionality);
            _AnimationMaterial.SetVector("_WindDirection", WindDirection);
            _AnimationMaterial.SetTexture("_TileSizeLookup", _TileSizeLookup);
            Graphics.Blit(_OmnidirectionalSpectrum, _DirectionalSpectrum, _AnimationMaterial, 3);

            AddOverlayToDirectionalSpectrum();

            _DirectionalSpectrumDirty = false;
        }

        private void AddOverlayToDirectionalSpectrum()
        {
            if (_SpectrumDownsamplingMesh == null)
                _SpectrumDownsamplingMesh = CreateDownsamplingMesh();

            for (int i = _OverlayedSpectra.Count - 1; i >= 0; --i)
            {
                var spectrumData = _OverlayedSpectra[i];
                var texture = spectrumData.Texture;

                _AnimationMaterial.SetFloat("_Weight", spectrumData.Weight);
                _AnimationMaterial.SetVector("_WindDirection", spectrumData.WindDirection);

                float radius = spectrumData.WeatherSystemRadius;
                _AnimationMaterial.SetVector("_WeatherSystemRadius", new Vector4(2.0f * radius, radius * radius, 0.0f, 0.0f));

                Vector2 offset = spectrumData.WeatherSystemOffset;
                _AnimationMaterial.SetVector("_WeatherSystemOffset", new Vector4(offset.x, offset.y, offset.magnitude, 0.0f));

                Graphics.Blit(texture, _DirectionalSpectrum, _AnimationMaterial, 6);

                /*animationMaterial.mainTexture = texture;
                animationMaterial.SetFloat("_ResolutionRatio", (float)texture.width/directionalSpectrum.width);
                GL.PushMatrix();
                GL.modelview = Matrix4x4.identity;
                GL.LoadProjectionMatrix(Matrix4x4.identity);

                if (animationMaterial.SetPass(6))
                {
                    Graphics.SetRenderTarget(directionalSpectrum);
                    Graphics.DrawMeshNow(spectrumDownsamplingMesh, Matrix4x4.identity);
                }

                GL.PopMatrix();*/
            }

            //Graphics.SetRenderTarget(null);
            //animationMaterial.mainTexture = null;
        }

        internal RenderTexture GetRawDirectionalSpectrum()
        {
            if ((_DirectionalSpectrumDirty || !_DirectionalSpectrum.IsCreated()) && Application.isPlaying)
            {
                CheckResources();
                RenderDirectionalSpectrum();
            }

            return _DirectionalSpectrum;
        }

        private RenderTexture GetTotalOmnidirectionalSpectrum()
        {
            if (_TotalOmnidirectionalSpectrum == null)
            {
                int finalResolutionx2 = _WindWaves.FinalResolution << 1;

                _TotalOmnidirectionalSpectrum = new RenderTexture(finalResolutionx2, finalResolutionx2, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] SpectrumResolver - Total Omnidirectional Spectrum",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Repeat
                };
            }

            return _TotalOmnidirectionalSpectrum;
        }

        private void CheckResources()
        {
            if (_HeightSpectrum == null)          // these are always all null or non-null
            {
                int finalResolutionx2 = _WindWaves.FinalResolution << 1;
                bool highPrecision = _WindWaves.FinalHighPrecision;

                _HeightSpectrum = new RenderTexture(finalResolutionx2, finalResolutionx2, 0, highPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] SpectrumResolver - Water Height Spectrum",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Point
                };

                _NormalSpectrum = new RenderTexture(finalResolutionx2, finalResolutionx2, 0, highPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] SpectrumResolver - Water Normals Spectrum",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Point
                };

                _DisplacementSpectrum = new RenderTexture(finalResolutionx2, finalResolutionx2, 0, highPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
                {
                    name = "[UWS] SpectrumResolver - Water Displacement Spectrum",
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Point
                };

                _DirectionalSpectrum = new RenderTexture(finalResolutionx2, finalResolutionx2, 0, highPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
                {
                    hideFlags = HideFlags.DontSave,
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };

                _RenderTargetsx2 = new[] { _HeightSpectrum.colorBuffer, _DisplacementSpectrum.colorBuffer };
                _RenderTargetsx3 = new[] { _HeightSpectrum.colorBuffer, _NormalSpectrum.colorBuffer, _DisplacementSpectrum.colorBuffer };
            }
        }

        internal override void OnMapsFormatChanged(bool resolution)
        {
            base.OnMapsFormatChanged(resolution);

            if (_TotalOmnidirectionalSpectrum != null)
            {
                Object.Destroy(_TotalOmnidirectionalSpectrum);
                _TotalOmnidirectionalSpectrum = null;
            }

            if (_HeightSpectrum != null)
            {
                Object.Destroy(_HeightSpectrum);
                _HeightSpectrum = null;
            }

            if (_NormalSpectrum != null)
            {
                Object.Destroy(_NormalSpectrum);
                _NormalSpectrum = null;
            }

            if (_DisplacementSpectrum != null)
            {
                Object.Destroy(_DisplacementSpectrum);
                _DisplacementSpectrum = null;
            }

            if (_DirectionalSpectrum != null)
            {
                Object.Destroy(_DirectionalSpectrum);
                _DirectionalSpectrum = null;
            }

            if (_TileSizeLookup != null)
            {
                Object.Destroy(_TileSizeLookup);
                _TileSizeLookup = null;
                _TileSizesLookupDirty = true;
            }

            _OmnidirectionalSpectrum = null;
            _RenderTargetsx2 = null;
            _RenderTargetsx3 = null;
        }

        private void ValidateTileSizeLookup()
        {
            if (_TileSizesLookupDirty)
            {
                if (_TileSizeLookup == null)
                {
                    _TileSizeLookup = new Texture2D(2, 2, SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBAHalf, false, true)
                    {
                        hideFlags = HideFlags.DontSave,
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Point
                    };
                }

                _TileSizeLookup.SetPixel(0, 0, new Color(0.5f, 0.5f, 1.0f / _TileSizes.x, 0.0f));
                _TileSizeLookup.SetPixel(1, 0, new Color(1.5f, 0.5f, 1.0f / _TileSizes.y, 0.0f));
                _TileSizeLookup.SetPixel(0, 1, new Color(0.5f, 1.5f, 1.0f / _TileSizes.z, 0.0f));
                _TileSizeLookup.SetPixel(1, 1, new Color(1.5f, 1.5f, 1.0f / _TileSizes.w, 0.0f));
                _TileSizeLookup.Apply(false, false);

                _TileSizesLookupDirty = false;
            }
        }

        private static void AddQuad(Vector3[] vertices, Vector3[] origins, Vector2[] uvs, int index, float xOffset, float yOffset, int originIndex)
        {
            originIndex += index;

            float xOffsetV = xOffset * 2.0f - 1.0f;
            float yOffsetV = yOffset * 2.0f - 1.0f;

            uvs[index] = new Vector2(xOffset, yOffset);
            vertices[index++] = new Vector3(xOffsetV, yOffsetV, 0.1f);

            uvs[index] = new Vector2(xOffset, yOffset + 0.25f);
            vertices[index++] = new Vector3(xOffsetV, yOffsetV + 0.5f, 0.1f);

            uvs[index] = new Vector2(xOffset + 0.25f, yOffset + 0.25f);
            vertices[index++] = new Vector3(xOffsetV + 0.5f, yOffsetV + 0.5f, 0.1f);

            uvs[index] = new Vector2(xOffset + 0.25f, yOffset);
            vertices[index] = new Vector3(xOffsetV + 0.5f, yOffsetV, 0.1f);

            origins[index--] = vertices[originIndex];
            origins[index--] = vertices[originIndex];
            origins[index--] = vertices[originIndex];
            origins[index] = vertices[originIndex];
        }

        private static void AddQuads(Vector3[] vertices, Vector3[] origins, Vector2[] uvs, int index, float xOffset, float yOffset)
        {
            AddQuad(vertices, origins, uvs, index, xOffset, yOffset, 0);
            AddQuad(vertices, origins, uvs, index + 4, xOffset + 0.25f, yOffset, 3);
            AddQuad(vertices, origins, uvs, index + 8, xOffset, yOffset + 0.25f, 1);
            AddQuad(vertices, origins, uvs, index + 12, xOffset + 0.25f, yOffset + 0.25f, 2);
        }

        private static Mesh CreateDownsamplingMesh()
        {
            var mesh = new Mesh { name = "[PW Water] Spectrum Downsampling Mesh" };

            var vertices = new Vector3[64];
            var origins = new Vector3[64];
            var uvs = new Vector2[64];
            var indices = new int[64];

            for (int i = 0; i < indices.Length; ++i)
                indices[i] = i;

            AddQuads(vertices, origins, uvs, 0, 0.0f, 0.0f);        // tile 0
            AddQuads(vertices, origins, uvs, 16, 0.5f, 0.0f);        // tile 1
            AddQuads(vertices, origins, uvs, 32, 0.0f, 0.5f);        // tile 2
            AddQuads(vertices, origins, uvs, 48, 0.5f, 0.5f);        // tile 3

            mesh.vertices = vertices;
            mesh.normals = origins;
            mesh.uv = uvs;
            mesh.SetIndices(indices, MeshTopology.Quads, 0);

            return mesh;
        }
        #endregion Private Methods
    }
}