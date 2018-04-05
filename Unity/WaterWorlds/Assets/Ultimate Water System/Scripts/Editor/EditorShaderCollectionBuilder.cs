namespace UltimateWater.Editors
{
#if UNITY_EDITOR

    using UnityEditor;

#endif

    using System;
    using UnityEngine;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Builds shader collections. It's separated to editor script because it runs in less restrictive .net environment.
    /// </summary>
    public class EditorShaderCollectionBuilder : IShaderSetBuilder
    {
        #region Public Methods
        [InitializeOnLoadMethod]
        public static void RegisterShaderCollectionBuilder()
        {
            var instance = new EditorShaderCollectionBuilder();
            ShaderSet.ShaderCollectionBuilder = instance;
        }

        public Shader BuildShaderVariant(string[] localKeywords, string[] sharedKeywords, string additionalCode, string keywordsString, bool volume, bool useForwardPasses, bool useDeferredPass)
        {
            string shaderPath;
            string shaderCodeTemplate = File.ReadAllText(!volume ? WaterPackageEditorUtilities.WaterPackagePath + "/Shaders/Water/UltimateWater (TEMPLATE).shader" : WaterPackageEditorUtilities.WaterPackagePath + "/Shaders/Water/UltimateWater - Volume (TEMPLATE).shader");
            string shaderCode = BuildShader(shaderCodeTemplate, localKeywords, sharedKeywords, additionalCode, volume, keywordsString, useForwardPasses, useDeferredPass);

            if (!volume)
                shaderPath = WaterPackageEditorUtilities.WaterPackagePath + "/Shaders/Water/UltimateWater Variation #" + HashString(keywordsString) + ".shader";
            else
                shaderPath = WaterPackageEditorUtilities.WaterPackagePath + "/Shaders/Water/UltimateWater Volume Variation #" + HashString(keywordsString) + ".shader";

            File.WriteAllText(shaderPath, shaderCode);
            AssetDatabase.Refresh();

            var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
            return shader;
        }

        public void CleanUpUnusedShaders()
        {
            var files = new List<string>(
                Directory.GetFiles(WaterPackageEditorUtilities.WaterPackagePath + "/Shaders/Water/")
                    .Where(f => f.Contains(" Variation ") && !f.EndsWith(".meta"))
                );

            for (int i = files.Count - 1; i >= 0; --i)
                files[i] = files[i].Replace('\\', '/');

            var guids = AssetDatabase.FindAssets("t:ShaderSet", null);

            for (int i = 0; i < guids.Length; ++i)
            {
                var shaderCollection = AssetDatabase.LoadAssetAtPath<ShaderSet>(AssetDatabase.GUIDToAssetPath(guids[i]));

                var surfaceShaders = shaderCollection.SurfaceShaders;

                if (surfaceShaders != null)
                {
                    for (int ii = 0; ii < surfaceShaders.Length; ++ii)
                    {
                        string shaderPath = AssetDatabase.GetAssetPath(surfaceShaders[ii]).Replace('\\', '/');
                        files.Remove(shaderPath);
                    }
                }

                var volumeShaders = shaderCollection.VolumeShaders;

                if (volumeShaders != null)
                {
                    for (int ii = 0; ii < volumeShaders.Length; ++ii)
                    {
                        string shaderPath = AssetDatabase.GetAssetPath(volumeShaders[ii]);
                        files.Remove(shaderPath);
                    }
                }
            }

            for (int i = files.Count - 1; i >= 0; --i)
                AssetDatabase.DeleteAsset(files[i]);
        }
        #endregion Public Methods

        #region Private Variables
        private const string _LocalKeywordDefinitionFormat = "#define {0} 1\r\n";
        private const string _SharedKeywordDefinitionFormat = "#pragma multi_compile {0}\r\n";
        private const string _ForwardPassesStart = "// START FORWARD_PASSES";
        private const string _ForwardPassesEnd = "// END FORWARD_PASSES";
        private const string _DeferredPassStart = "// START DEFERRED_PASS";
        private const string _DeferredPassEnd = "// END DEFERRED_PASS";
        #endregion Private Variables

        #region Private Methods
        private static string BuildShader(string code, string[] localKeywords, string[] sharedKeywords, string additionalCode, bool volume, string keywordsString, bool useForwardPasses, bool useDeferredPass)
        {
            var localKeywordsCode = localKeywords.Select(k => string.Format(_LocalKeywordDefinitionFormat, k)).ToArray();
            var sharedKeywordsCode = sharedKeywords.Select(k => string.Format(_SharedKeywordDefinitionFormat, k)).ToArray();

            string keywordsCode = string.Join("\t\t\t", localKeywordsCode) + "\r\n\t\t\t" + string.Join("\t\t\t", sharedKeywordsCode);

            if (!string.IsNullOrEmpty(additionalCode))
                keywordsCode += "\r\n\t\t\t" + additionalCode;

            if (!useForwardPasses)
            {
                int startIndex = code.IndexOf(_ForwardPassesStart, StringComparison.InvariantCulture);
                int endIndex = code.IndexOf(_ForwardPassesEnd, StringComparison.InvariantCulture) + _ForwardPassesEnd.Length;

                if (startIndex != -1 && endIndex != -1)
                    code = code.Remove(startIndex, endIndex - startIndex);
            }

            if (!useDeferredPass)
            {
                int startIndex = code.IndexOf(_DeferredPassStart, StringComparison.InvariantCulture);
                int endIndex = code.IndexOf(_DeferredPassEnd, StringComparison.InvariantCulture) + _DeferredPassEnd.Length;

                if (startIndex != -1 && endIndex != -1)
                    code = code.Remove(startIndex, endIndex - startIndex);
            }

            return code.Replace("UltimateWater/Standard" + (volume ? " Volume" : ""), "UltimateWater/Variations/Water " + (volume ? "Volume " : "") + keywordsString)
                .Replace("#define PLACE_KEYWORDS_HERE", keywordsCode);
        }

        private static int HashString(string text)
        {
            int len = text.Length;
            int hash = 23;

            for (int i = 0; i < len; ++i)
                hash = hash * 31 + text[i];

            return hash;
        }
        #endregion Private Methods
    }

    public class WaterShadersCleanupTask : AssetModificationProcessor
    {
        #region Public Methods
        public static string[] OnWillSaveAssets(string[] paths)
        {
            var shaderCollectionBuilder = (EditorShaderCollectionBuilder)ShaderSet.ShaderCollectionBuilder;
            shaderCollectionBuilder.CleanUpUnusedShaders();

            return paths;
        }
        #endregion Public Methods
    }
}