namespace UltimateWater.Internal
{
    using UnityEngine;

    public struct TemporaryRenderTexture : System.IDisposable
    {
        #region Public Methods
        public RenderTexture Texture
        {
            get { return _RenderTexture; }
        }

        public void Dispose()
        {
            if (_RenderTexture == null) return;

            _Cache.ReleaseTemporaryDirect(_RenderTexture);
            _RenderTexture = null;
        }

        public static implicit operator RenderTexture(TemporaryRenderTexture that)
        {
            return that.Texture;
        }
        #endregion Public Methods

        #region Private Variables
        private RenderTexture _RenderTexture;
        private readonly RenderTexturesCache _Cache;
        #endregion Private Variables

        #region Private Methods
        internal TemporaryRenderTexture(RenderTexturesCache renderTexturesCache)
        {
            _Cache = renderTexturesCache;
            _RenderTexture = renderTexturesCache.GetTemporaryDirect();
        }
        #endregion Private Methods
    }
}