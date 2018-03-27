namespace UltimateWater
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ShaderVariant
    {
        #region Public Methods
        public ShaderVariant()
        {
            _UnityKeywords = new Dictionary<string, bool>();
            _WaterKeywords = new Dictionary<string, bool>();
            _SurfaceShaderParts = new Dictionary<string, string>();
            _VolumeShaderParts = new Dictionary<string, string>();
        }

        public void SetUnityKeyword(string keyword, bool value)
        {
            if (value)
                _UnityKeywords[keyword] = true;
            else
                _UnityKeywords.Remove(keyword);
        }

        public void SetWaterKeyword(string keyword, bool value)
        {
            if (value)
                _WaterKeywords[keyword] = true;
            else
                _WaterKeywords.Remove(keyword);
        }

        public void SetAdditionalSurfaceCode(string keyword, string code)
        {
            if (code != null)
                _SurfaceShaderParts[keyword] = code;
            else
                _SurfaceShaderParts.Remove(keyword);
        }

        public void SetAdditionalVolumeCode(string keyword, string code)
        {
            if (code != null)
                _VolumeShaderParts[keyword] = code;
            else
                _VolumeShaderParts.Remove(keyword);
        }

        public bool IsUnityKeywordEnabled(string keyword)
        {
            bool value;

            if (_UnityKeywords.TryGetValue(keyword, out value))
                return true;

            return false;
        }

        public bool IsWaterKeywordEnabled(string keyword)
        {
            bool value;

            if (_WaterKeywords.TryGetValue(keyword, out value))
                return true;

            return false;
        }

        public string GetAdditionalSurfaceCode()
        {
            StringBuilder sb = new StringBuilder(512);

            foreach (string code in _SurfaceShaderParts.Values)
                sb.Append(code);

            return sb.ToString();
        }

        public string GetAdditionalVolumeCode()
        {
            StringBuilder sb = new StringBuilder(512);

            foreach (string code in _VolumeShaderParts.Values)
                sb.Append(code);

            return sb.ToString();
        }

        public string[] GetUnityKeywords()
        {
            string[] keywords = new string[_UnityKeywords.Count];
            int index = 0;

            foreach (string keyword in _UnityKeywords.Keys)
                keywords[index++] = keyword;

            return keywords;
        }

        public string[] GetWaterKeywords()
        {
            string[] keywords = new string[_WaterKeywords.Count];
            int index = 0;

            foreach (string keyword in _WaterKeywords.Keys)
                keywords[index++] = keyword;

            return keywords;
        }

        public string GetKeywordsString()
        {
            StringBuilder sb = new StringBuilder(512);
            bool notFirst = false;

            foreach (string keyword in _WaterKeywords.Keys.OrderBy(k => k))
            {
                if (notFirst)
                    sb.Append(' ');
                else
                    notFirst = true;

                sb.Append(keyword);
            }

            foreach (string keyword in _UnityKeywords.Keys.OrderBy(k => k))
            {
                if (notFirst)
                    sb.Append(' ');
                else
                    notFirst = true;

                sb.Append(keyword);
            }

            foreach (string keyword in _SurfaceShaderParts.Keys.OrderBy(k => k))
            {
                if (notFirst)
                    sb.Append(' ');
                else
                    notFirst = true;

                sb.Append(keyword);
            }

            return sb.ToString();
        }
        #endregion Public Methods
        #region Private Variables
        private readonly Dictionary<string, bool> _UnityKeywords;
        private readonly Dictionary<string, bool> _WaterKeywords;
        private readonly Dictionary<string, string> _SurfaceShaderParts;
        private readonly Dictionary<string, string> _VolumeShaderParts;
        #endregion Private Variables
    }
}