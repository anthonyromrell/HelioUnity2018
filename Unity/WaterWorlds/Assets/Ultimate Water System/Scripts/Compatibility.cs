namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;
    using Utils;

#if UNITY_EDITOR

    using UnityEditor;

    [InitializeOnLoad]
#endif
    public class Compatibility
    {
        #region Public Methods
        static Compatibility()
        {
            const string key = "[Ultimate Water System]: compatibility check";

            if (PlayerPrefs.GetInt(key) == Versioning.Number) { return; }
            CheckFormats();
            PlayerPrefs.SetInt(key, Versioning.Number);
        }

        public static RenderTextureFormat? GetFormat(RenderTextureFormat preferred, IEnumerable<RenderTextureFormat> fallback = null)
        {
            if (IsFormatSupported(preferred)) return preferred;
            if (fallback == null)
            {
                WaterLogger.Error("Compatibility", "GetFormat",
                    "preferred format not supported, and no fallback formats available for :" + preferred);

                return null;
            }

            foreach (var format in fallback)
            {
                if (SystemInfo.SupportsRenderTextureFormat(format))
                {
                    WaterLogger.Warning("Compatibility", "GetFormat",
                        "preferred format not supported, chosen fallback: " + format);
                    return format;
                }
            }

            return null;
        }
        #endregion Public Methods

        #region Private Methods
        private static void CheckFormats()
        {
            var formats = new[]
            {
                RenderTextureFormat.ARGBFloat,
                RenderTextureFormat.ARGBHalf,
                RenderTextureFormat.ARGB32,

                RenderTextureFormat.RGHalf,

                RenderTextureFormat.RFloat,
                RenderTextureFormat.RHalf,
                RenderTextureFormat.R8,

                RenderTextureFormat.Depth
            };

            var result = true;
            for (int i = 0; i < formats.Length; ++i)
            {
                var current = formats[i];
                if (!IsFormatSupported(current))
                {
                    WaterLogger.Info("Compatibility", "CheckFormats",
                        "RenderTexture format not supported: " + current);
                    result = false;
                }
            }

            if (result)
            {
                WaterLogger.Info("Compatibility", "CheckFormats", "all necessary RenderTexture formats supported");
            }
            else
            {
                WaterLogger.Warning("Compatibility", "CheckFormats", "some of the necessary render texture formats not supported, \n" +
                               "some features will not be available");
            }
        }
        #endregion Private Methods

        #region Helper Methods
        private static bool IsFormatSupported(RenderTextureFormat format)
        {
            var supports = SystemInfo.SupportsRenderTextureFormat(format);
            return supports;
        }
        #endregion Helper Methods
    }
}