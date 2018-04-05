namespace UltimateWater.Internal
{
    using UnityEngine;

    public abstract class GpuFFT
    {
        #region Public Variables
        public Texture2D Butterfly
        {
            get { return _Butterfly; }
        }

        public int Resolution
        {
            get { return _Resolution; }
        }
        #endregion Public Variables

        #region Public Methods
        public abstract void SetupMaterials();
        public abstract void ComputeFFT(Texture tex, RenderTexture target);

        public virtual void Dispose()
        {
            if (_Butterfly != null)
            {
                _Butterfly.Destroy();
                _Butterfly = null;
            }
        }
        #endregion Public Methods

        #region Private Variables
        private Texture2D _Butterfly;

        protected RenderTexturesCache _RenderTexturesSet;

        protected int _Resolution;
        protected int _NumButterflies;
        protected int _NumButterfliesPow2;
        protected bool _TwoChannels;

        private readonly bool _HighPrecision;
        private readonly bool _UsesUav;
        #endregion Private Variables

        #region Private Methods
        private void CreateTextures()
        {
            CreateButterflyTexture();
        }
        private void RetrieveRenderTexturesSet()
        {
            var format = _TwoChannels ?
                (_HighPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf) :
                (_HighPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf);

            _RenderTexturesSet = RenderTexturesCache.GetCache(_Resolution << 1, _Resolution << 1, 0, format, true, _UsesUav);
        }
        protected virtual void FillButterflyTexture(Texture2D butterfly, int[][] indices, Vector2[][] weights)
        {
            float floatResolutionx2 = _Resolution << 1;

            for (int row = 0; row < _NumButterflies; ++row)
            {
                for (int scaleIndex = 0; scaleIndex < 2; ++scaleIndex)
                {
                    int offset = scaleIndex == 0 ? 0 : _Resolution;

                    for (int col = 0; col < _Resolution; ++col)
                    {
                        Color c;

                        int indexX = _NumButterflies - row - 1;
                        int indexY = (col << 1);

                        c.r = (indices[indexX][indexY] + offset + 0.5f) / floatResolutionx2;
                        c.g = (indices[indexX][indexY + 1] + offset + 0.5f) / floatResolutionx2;

                        c.b = weights[row][col].x;
                        c.a = weights[row][col].y;

                        butterfly.SetPixel(offset + col, row, c);
                    }
                }
            }
        }
        private void CreateButterflyTexture()
        {
            _Butterfly = new Texture2D(_Resolution << 1, _NumButterfliesPow2,
                _HighPrecision ? TextureFormat.RGBAFloat : TextureFormat.RGBAHalf, false, true)
            {
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            int[][] indices;
            Vector2[][] weights;
            ButterflyFFTUtility.ComputeButterfly(_Resolution, _NumButterflies, out indices, out weights);
            FillButterflyTexture(_Butterfly, indices, weights);

            _Butterfly.Apply();
        }
        protected GpuFFT(int resolution, bool highPrecision, bool twoChannels, bool usesUAV)
        {
            _Resolution = resolution;
            _HighPrecision = highPrecision;
            _NumButterflies = (int)(Mathf.Log(resolution) / Mathf.Log(2.0f));
            _NumButterfliesPow2 = Mathf.NextPowerOfTwo(_NumButterflies);
            _TwoChannels = twoChannels;
            _UsesUav = usesUAV;

            RetrieveRenderTexturesSet();
            CreateTextures();
        }
        #endregion Private Methods
    }
}