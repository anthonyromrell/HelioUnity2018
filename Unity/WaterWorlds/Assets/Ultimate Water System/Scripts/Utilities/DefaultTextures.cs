namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DefaultTextures : ApplicationSingleton<DefaultTextures>
    {
        #region Public Variables
        public static Texture2D Get(Color color)
        {
            var instance = Instance;
            if (instance == null) { return null; }

            Texture2D result;
            if (_Cache.TryGetValue(color, out result)) { return result; }

            result = CreateColorTexure(color, "[UWS] DefaultTextures - " + color);
            _Cache.Add(color, result);

            return result;
        }
        #endregion Public Variables

        #region Unity Methods

        protected override void OnDestroy()
        {
            foreach (var texture in _Cache)
            {
                texture.Value.Destroy();
            }
            _Cache.Clear();

            base.OnDestroy();
        }
        #endregion Unity Methods

        #region Private Variables
        private static readonly Dictionary<Color, Texture2D> _Cache =
            new Dictionary<Color, Texture2D>();
        #endregion Private Variables

        #region Private Methods
        private static Texture2D CreateColorTexure(Color color, string name)
        {
            var result = new Texture2D(2, 2, TextureFormat.ARGB32, false)
            {
                name = name,
                hideFlags = HideFlags.DontSave
            };

            result.SetPixel(0, 0, color);
            result.SetPixel(1, 0, color);
            result.SetPixel(0, 1, color);
            result.SetPixel(1, 1, color);

            result.Apply(false, true);

            return result;
        }
        #endregion Private Methods
    }
}