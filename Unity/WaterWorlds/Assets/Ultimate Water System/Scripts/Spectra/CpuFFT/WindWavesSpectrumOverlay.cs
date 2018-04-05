namespace UltimateWater
{
    using UnityEngine;

    public class WindWavesSpectrumOverlay
    {
        #region Public Variables
        public event System.Action Cleared;

        #endregion Public Variables

        #region Public Methods
        public WindWavesSpectrumOverlay(WindWaves windWaves)
        {
            _WindWaves = windWaves;

            _SpectrumData = new Vector2[4][];

            for (int tileIndex = 0; tileIndex < 4; ++tileIndex)
                _SpectrumData[tileIndex] = new Vector2[windWaves.FinalResolution * windWaves.FinalResolution];
        }

        public void Destroy()
        {
            _SpectrumData = null;
            Cleared = null;

            if (_Texture != null)
            {
                Object.Destroy(_Texture);
                _Texture = null;
            }
        }

        public Texture2D Texture
        {
            get
            {
                if (_TextureDirty)
                    ValidateTexture();

                return _Texture;
            }
        }

        public Vector2[] GetSpectrumDataDirect(int tileIndex)
        {
            return _SpectrumData[tileIndex];
        }

        public void Refresh()
        {
            int finalResolution = _WindWaves.FinalResolution;
            int finalResolutionSqr = finalResolution * finalResolution;

            for (int tileIndex = 0; tileIndex < 4; ++tileIndex)
            {
                var data = _SpectrumData[tileIndex];

                if (data.Length == finalResolutionSqr)
                {
                    for (int i = 0; i < data.Length; ++i)
                        data[i] = new Vector2(0.0f, 0.0f);
                }
                else
                    _SpectrumData[tileIndex] = new Vector2[finalResolutionSqr];
            }

            _TextureDirty = true;

            if (Cleared != null)
                Cleared();
        }
        #endregion Public Methods

        #region Private Variables
        private Vector2[][] _SpectrumData;
        private Texture2D _Texture;
        private bool _TextureDirty = true;

        private readonly WindWaves _WindWaves;
        #endregion Private Variables

        #region Private Methods
        private void ValidateTexture()
        {
            _TextureDirty = false;

            int finalResolution = _WindWaves.FinalResolution;
            int finalResolutionx2 = finalResolution << 1;

            if (_Texture != null && _Texture.width != finalResolutionx2)
            {
                Object.Destroy(_Texture);
                _Texture = null;
            }

            if (_Texture == null)
            {
                _Texture = new Texture2D(finalResolutionx2, finalResolutionx2, TextureFormat.RGHalf, false, true)
                {
                    filterMode = FilterMode.Point
                };
            }

            for (int tileIndex = 0; tileIndex < 4; ++tileIndex)
            {
                var data = _SpectrumData[tileIndex];

                int xOffset = tileIndex == 1 || tileIndex == 3 ? finalResolution : 0;
                int yOffset = tileIndex == 2 || tileIndex == 3 ? finalResolution : 0;

                for (int x = finalResolution - 1; x >= 0; --x)
                {
                    for (int y = finalResolution - 1; y >= 0; --y)
                    {
                        Vector2 value = data[x * finalResolution + y];
                        _Texture.SetPixel(xOffset + x, yOffset + y, new Color(value.x, value.y, 0.0f, 0.0f));
                    }
                }
            }

            _Texture.Apply(false, false);
        }
        #endregion Private Methods
    }
}