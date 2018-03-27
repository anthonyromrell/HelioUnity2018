namespace UltimateWater.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public class UtilityShaderVariants
    {
        #region Public Variables
        public static UtilityShaderVariants Instance
        {
            get { return _Instance != null ? _Instance : (_Instance = new UtilityShaderVariants()); }
        }
        #endregion Public Variables

        #region Public Methods
        public Material GetVariant(Shader shader, string keywords)
        {
            Material material;

            int hash = shader.GetInstanceID() ^ keywords.GetHashCode();

            if (!_Materials.TryGetValue(hash, out material))
            {
                material = new Material(shader)
                {
                    hideFlags = HideFlags.DontSave,
                    shaderKeywords = keywords.Split(' ')
                };

                _Materials[hash] = material;
            }

            return material;
        }
        #endregion Public Methods

        #region Private Variables
        private readonly Dictionary<int, Material> _Materials;

        private static UtilityShaderVariants _Instance;
        #endregion Private Variables

        #region Private Methods
        private UtilityShaderVariants()
        {
            _Materials = new Dictionary<int, Material>();
        }
        #endregion Private Methods
    }
}