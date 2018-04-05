using UnityEngine.Rendering;

namespace UltimateWater.Internal
{
    using UnityEngine;

    public static class TextureUtility
    {
        #region Public Types
        public struct RenderTextureDesc
        {
            #region Public Variables
            public string Name;

            public int Width;
            public int Height;
            public int Depth;
            public int VolumeDepth;
            public int Antialiasing;

            public RenderTextureFormat Format;
            public HideFlags Flags;
            public FilterMode Filter;
            public TextureWrapMode Wrap;
            public RenderTextureReadWrite ColorSpace;

            public bool EnableRandomWrite;

            public bool GenerateMipmaps;
            public bool UseMipmaps;
            public float MipmapBias;
            #endregion Public Variables

            public RenderTextureDesc(string name)
            {
                Width = 0;
                Height = 0;
                Depth = 0;
                Format = RenderTextureFormat.Default;
                ColorSpace = RenderTextureReadWrite.Linear;

                Name = name;

                VolumeDepth = 0;
                Antialiasing = 1;

                Flags = HideFlags.DontSave;
                Filter = FilterMode.Bilinear;
                Wrap = TextureWrapMode.Clamp;

                EnableRandomWrite = false;

                GenerateMipmaps = false;
                UseMipmaps = false;
                MipmapBias = 0.0f;
            }

            public RenderTextureDesc(RenderTexture source)
            {
                Name = "RenderTexture";

                Width = source.width;
                Height = source.height;
                Depth = source.depth;
                VolumeDepth = source.volumeDepth;
                Antialiasing = source.antiAliasing;

                Format = source.format;
                Flags = source.hideFlags;
                Filter = source.filterMode;
                Wrap = source.wrapMode;
                ColorSpace = source.sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear;

                EnableRandomWrite = source.enableRandomWrite;
#if UNITY_5_5_OR_NEWER
                GenerateMipmaps = source.autoGenerateMips;
#else
                GenerateMipmaps = source.generateMips;
#endif
                UseMipmaps = source.useMipMap;
                MipmapBias = source.mipMapBias;
            }
        }

        #endregion Public Types

        #region Public Methods
        public static void Release(ref RenderTexture texture)
        {
            if (texture != null)
            {
                texture.Release();
                texture.Destroy();
            }
            texture = null;
        }
        public static void Release(ref Texture2D texture)
        {
            if (texture != null)
            {
                texture.Destroy();
            }
            texture = null;
        }

        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }
        #endregion Public Methods

        #region Extension Methods
        /// <summary>
        /// Calls GL.Clear on the texture
        /// </summary>
        public static void Clear(this RenderTexture texture, bool clearDepth = false, bool clearColor = true)
        {
            Clear(texture, Color.clear, clearDepth, clearColor);
        }

        /// <summary>
        /// Calls GL.Clear on the texture
        /// </summary>
        public static void Clear(this RenderTexture texture, Color color, bool clearDepth = false, bool clearColor = true)
        {
            Graphics.SetRenderTarget(texture);
            GL.Clear(clearDepth, clearColor, color);
            Graphics.SetRenderTarget(null);
        }

        /// <summary>
        /// Resizes valid(not-null and created) RenderTexture
        /// </summary>
        public static void Resize(this RenderTexture texture, int width, int height)
        {
            if (texture == null)
            {
                Debug.LogWarning("Trying to resize null RenderTexture");
                return;
            }
            if (!texture.IsCreated())
            {
                Debug.LogWarning("Trying to resize not created RenderTexture");
                return;
            }

            if (width <= 0 || height <= 0)
            {
                Debug.LogWarning("Trying to resize to invalid(<=0) width or height ");
                return;
            }

            if (texture.width == width && texture.height == height)
            {
                return;
            }

            texture.Release();
            texture.width = width;
            texture.height = height;
            texture.Create();
        }

        /// <summary>
        /// Checks if RenderTexture was "lost" and recreates it if needed
        /// </summary>
        /// <returns>Did the texture was recreated</returns>
        public static bool Verify(this RenderTexture texture, bool clear = true)
        {
            if (texture == null)
            {
                Debug.LogWarning("Trying to resolve null RenderTexture");
                return false;
            }

            if (texture.IsCreated()) { return false; }
            texture.Create();

            if (clear)
            {
                Clear(texture, Color.clear);
            }
            return true;
        }

        /// <summary>
        /// Creates cleared render texture
        /// </summary>
        public static RenderTexture CreateRenderTexture(this RenderTexture template)
        {
            return template.GetDesc().CreateRenderTexture();
        }

        public static RenderTexture CreateRenderTexture(this RenderTextureDesc desc)
        {
            var result = new RenderTexture(desc.Width, desc.Height, desc.Depth, desc.Format, desc.ColorSpace)
            {
                name = desc.Name,

                volumeDepth = desc.VolumeDepth,
                antiAliasing = desc.Antialiasing,

                hideFlags = desc.Flags,
                filterMode = desc.Filter,
                wrapMode = desc.Wrap,

#if UNITY_5_5_OR_NEWER
                autoGenerateMips = desc.GenerateMipmaps,
#else
                generateMips = desc.GenerateMipmaps,
#endif
                useMipMap = desc.UseMipmaps,
                mipMapBias = desc.MipmapBias
            };

            if (desc.EnableRandomWrite)
            {
                result.Release();
                result.enableRandomWrite = true;
                result.Create();
            }
            else
            {
                result.Create();
            }
            return result;
        }

        public static RenderTexture CreateTemporary(this RenderTexture template)
        {
            var temporary = RenderTexture.GetTemporary(
                template.width, template.height,
                template.depth, template.format,
                template.sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear,
                template.antiAliasing);

#if UNITY_5_5_OR_NEWER
            temporary.autoGenerateMips = template.autoGenerateMips;
#else
            temporary.generateMips = template.generateMips;
#endif
            temporary.wrapMode = template.wrapMode;
            temporary.filterMode = template.filterMode;
            temporary.volumeDepth = template.volumeDepth;
            temporary.anisoLevel = template.anisoLevel;
            temporary.mipMapBias = template.mipMapBias;

            bool recreate = false;

            bool mipsMismatch = temporary.useMipMap != template;
            bool randomWriteMismatch = temporary.enableRandomWrite && !template.enableRandomWrite;

            recreate |= mipsMismatch;
            recreate |= randomWriteMismatch;

            if (recreate)
            {
                temporary.Release();

                if (mipsMismatch) { temporary.useMipMap = template.useMipMap; }
                if (randomWriteMismatch) { temporary.enableRandomWrite = template.enableRandomWrite; }

                temporary.Create();
            }

            temporary.Create();
            return temporary;
        }

        public static void ReleaseTemporary(this RenderTexture texture)
        {
            if (texture != null)
            {
                RenderTexture.ReleaseTemporary(texture);
            }
        }

        public static RenderTextureDesc GetDesc(this RenderTexture source)
        {
            return new RenderTextureDesc(source);
        }
        #endregion Extension Methods
    }
}