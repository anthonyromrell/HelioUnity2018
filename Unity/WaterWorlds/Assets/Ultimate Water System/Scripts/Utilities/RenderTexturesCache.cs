namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///     Alternative for RenderTexture.GetTemporary with UAV textures support and no allocations.
    /// </summary>
    public class RenderTexturesCache
    {
        #region Public Methods
        public RenderTexturesCache(ulong hash, int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps)
        {
            _Hash = hash;
            _Width = width;
            _Height = height;
            _DepthBuffer = depthBuffer;
            _Format = format;
            _Linear = linear;
            _Uav = uav;
            _MipMaps = mipMaps;
            _RenderTextures = new Queue<RenderTexture>();
        }

        public static RenderTexturesCache GetCache(int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps = false)
        {
            RenderTexturesUpdater.EnsureInstance();

            ulong hash = 0;

            hash |= (uint)width;
            hash |= ((uint)height << 16);
            hash |= ((ulong)depthBuffer << 29);        // >> 3 << 32
            hash |= ((linear ? 1UL : 0UL) << 34);
            hash |= ((uav ? 1UL : 0UL) << 35);
            hash |= ((mipMaps ? 1UL : 0UL) << 36);
            hash |= ((ulong)format << 37);

            RenderTexturesCache renderTexturesCache;
            if (!_Cache.TryGetValue(hash, out renderTexturesCache))
            {
                _Cache[hash] = renderTexturesCache = new RenderTexturesCache(hash, width, height, depthBuffer, format, linear, uav, mipMaps);
            }

            return renderTexturesCache;
        }

        public static TemporaryRenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, bool linear, bool uav, bool mipMaps = false)
        {
            return GetCache(width, height, depthBuffer, format, linear, uav, mipMaps).GetTemporary();
        }

        public TemporaryRenderTexture GetTemporary()
        {
            return new TemporaryRenderTexture(this);
        }

        public RenderTexture GetTemporaryDirect()
        {
            RenderTexture renderTexture;

            if (_RenderTextures.Count == 0)
            {
                renderTexture = new RenderTexture(_Width, _Height, _DepthBuffer, _Format, _Linear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
                renderTexture.hideFlags = HideFlags.DontSave;
                renderTexture.name = "[UWS] RenderTexturesCache - Temporary#" + _Hash;
                renderTexture.filterMode = FilterMode.Point;
                renderTexture.anisoLevel = 1;
                renderTexture.wrapMode = TextureWrapMode.Repeat;
                renderTexture.useMipMap = _MipMaps;
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4
                renderTexture.generateMips = _MipMaps;
#else
                renderTexture.autoGenerateMips = _MipMaps;
#endif

                if (_Uav)
                    renderTexture.enableRandomWrite = true;
            }
            else
                renderTexture = _RenderTextures.Dequeue();

            if (_Uav && !renderTexture.IsCreated())
                renderTexture.Create();

            if (_RenderTextures.Count == 0)
                _LastFrameAllUsed = Time.frameCount;

            return renderTexture;
        }

        public void ReleaseTemporaryDirect(RenderTexture renderTexture)
        {
            _RenderTextures.Enqueue(renderTexture);
        }
        #endregion Public Methods

        #region Private Types
        [ExecuteInEditMode]
        public class RenderTexturesUpdater : MonoBehaviour
        {
            #region Public Methods
            public static void EnsureInstance()
            {
                if (_Instance != null)
                {
                    return;
                }

                var go = new GameObject("Water.RenderTexturesCache") { hideFlags = HideFlags.HideAndDontSave };
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(go);
                }

                _Instance = go.AddComponent<RenderTexturesUpdater>();
            }
            #endregion Public Methods

            #region Private Variables
            private static RenderTexturesUpdater _Instance;
            #endregion Private Variables

            #region Private Methods
            private void Update()
            {
                int frame = Time.frameCount;

                var enumerator = _Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Value.Update(frame);
                }

                enumerator.Dispose();
            }
            private void OnApplicationQuit()
            {
                var enumerator = _Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Value.Release();
                }

                enumerator.Dispose();
            }
            #endregion Private Methods
        }
        #endregion Private Types

        #region Private Variables
        private static readonly Dictionary<ulong, RenderTexturesCache> _Cache = new Dictionary<ulong, RenderTexturesCache>(UInt64EqualityComparer.Default);

        private readonly Queue<RenderTexture> _RenderTextures;
        private int _LastFrameAllUsed;

        private readonly ulong _Hash;
        private readonly int _Width, _Height, _DepthBuffer;
        private readonly RenderTextureFormat _Format;
        private readonly bool _Linear, _Uav, _MipMaps;
        #endregion Private Variables

        #region Private Methods
        internal void Update(int frame)
        {
            if (frame - _LastFrameAllUsed > 3 && _RenderTextures.Count != 0)
            {
                var renderTexture = _RenderTextures.Dequeue();
                renderTexture.Destroy();
            }
        }
        internal void Release()
        {
            foreach (var texture in _RenderTextures)
            {
                if (texture != null)
                {
                    texture.Release();
                    texture.Destroy();
                }
            }
            _RenderTextures.Clear();
        }
        #endregion Private Methods
    }
}