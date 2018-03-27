namespace UltimateWater
{
    using UnityEngine;

    /// <summary>
    /// Maps shader ids to static variables to prevent the need to use strings
    ///
    /// [important note] PropertyToID returns different values for each game/machine, so we cannot store it
    /// instead, we cache it after first query
    ///
    /// [reference] https://docs.unity3d.com/ScriptReference/Shader.PropertyToID.html
    /// </summary>
    public class ShaderVariables
    {
        #region Public Variables
        #region Local
        public static int LocalDisplacementMap { get { return Cache(ref _LocalDisplacementMap, "_LocalDisplacementMap"); } }
        public static int LocalNormalMap { get { return Cache(ref _LocalNormalMap, "_LocalNormalMap"); } }
        public static int LocalDiffuseMap { get { return Cache(ref _LocalDiffuseMap, "_LocalDiffuseMap"); } }

        public static int LocalMapsCoords { get { return Cache(ref _LocalMapsCoords, "_LocalMapsCoords"); } }
        public static int LocalMapsCoordsPrevious { get { return Cache(ref _LocalMapsCoordsPrevious, "_LocalMapsCoordsPrevious"); } }
        #endregion Local
        #region Masks
        public static int UnderwaterMask { get { return Cache(ref _UnderwaterMask, "_UnderwaterMask"); } }
        public static int UnderwaterMask2 { get { return Cache(ref _UnderwaterMask2, "_UnderwaterMask2"); } }
        public static int AdditiveMask { get { return Cache(ref _AdditiveMask, "_AdditiveMask"); } }
        public static int SubtractiveMask { get { return Cache(ref _SubtractiveMask, "_SubtractiveMask"); } }
        public static int DisplacementsMask { get { return Cache(ref _DisplacementsMask, "_DisplacementsMask"); } }
        #endregion Masks
        #region GBuffer
        public static int Gbuffer0 { get { return Cache(ref _Gbuffer0, "_CameraGBufferTextureOriginal0"); } }
        public static int Gbuffer1 { get { return Cache(ref _Gbuffer1, "_CameraGBufferTextureOriginal1"); } }
        public static int Gbuffer2 { get { return Cache(ref _Gbuffer2, "_CameraGBufferTextureOriginal2"); } }
        public static int Gbuffer3 { get { return Cache(ref _Gbuffer3, "_CameraGBufferTextureOriginal3"); } }
        #endregion GBuffer
        #region Occlusion
        public static int OcclusionMap { get { return Cache(ref _OcclusionMap, "_OcclusionMap"); } }
        public static int OcclusionMap2 { get { return Cache(ref _OcclusionMap2, "_OcclusionMap2"); } }
        public static int OcclusionMapProjection { get { return Cache(ref _OcclusionMapProjection, "_OcclusionMapProjection"); } }
        #endregion Occlusion
        #region Foam
        public static int FoamParameters { get { return Cache(ref _FoamParameters, "_FoamParameters"); } }
        public static int FoamShoreIntensity { get { return Cache(ref _FoamShoreIntensity, "_FoamShoreIntensity"); } }
        public static int FoamIntensity { get { return Cache(ref _FoamIntensity, "_FoamIntensity"); } }
        public static int WaterTileSizeInvSrt { get { return Cache(ref _WaterTileSizeInvSrt, "_WaterTileSizeInvSRT"); } }

        public static int[] DisplacementDeltaMaps
        {
            get
            {
                if (_DisplacementDeltaMaps == null)
                {
                    _DisplacementDeltaMaps = new[] { -1, -1, -1, -1 };

                    Cache(ref _DisplacementDeltaMaps[0], "_DisplacementDeltaMap");
                    Cache(ref _DisplacementDeltaMaps[1], "_DisplacementDeltaMap1");
                    Cache(ref _DisplacementDeltaMaps[2], "_DisplacementDeltaMap2");
                    Cache(ref _DisplacementDeltaMaps[3], "_DisplacementDeltaMap3");
                }

                return _DisplacementDeltaMaps;
            }
        }

        #endregion Foam
        #region Tiles
        public static int WaterTileSize { get { return Cache(ref _WaterTileSize, "_WaterTileSize"); } }
        public static int WaterTileSizeInv { get { return Cache(ref _WaterTileSizeInv, "_WaterTileSizeInv"); } }
        public static int WaterTileSizeScales { get { return Cache(ref _WaterTileSizeScales, "_WaterTileSizeScales"); } }
        public static int MaxDisplacement { get { return Cache(ref _MaxDisplacement, "_MaxDisplacement"); } }
        #endregion Tiles
        #region Misc
        public static int TotalDisplacementMap { get { return Cache(ref _TotalDisplacementMap, "_TotalDisplacementMap"); } }
        public static int DisplacementMask { get { return Cache(ref _DisplacementMask, "_DisplacementsMask"); } }
        public static int WaterDepthTexture { get { return Cache(ref _WaterDepthTexture, "_WaterDepthTexture"); } }
        public static int UnityMatrixVPInverse { get { return Cache(ref _UnityMatrixVPInverse, "UNITY_MATRIX_VP_INVERSE"); } }
        public static int DepthClipMultiplier { get { return Cache(ref _DepthClipMultiplier, "_DepthClipMultiplier"); } }
        public static int RefractTex { get { return Cache(ref _RefractTex, "_RefractionTex"); } }
        public static int WaterlessDepthTexture { get { return Cache(ref _WaterlessDepthTexture, "_WaterlessDepthTexture"); } }
        public static int CameraDepthTexture2 { get { return Cache(ref _CameraDepthTexture2, "_CameraDepthTexture2"); } }
        public static int Velocity { get { return Cache(ref _Velocity, "Velocity"); } }
        public static int WaterShadowmap { get { return Cache(ref _WaterShadowmap, "_WaterShadowmap"); } }
        public static int PlanarReflectionTex { get { return Cache(ref _PlanarReflectionTex, "_PlanarReflectionTex"); } }
        public static int ButterflyTex { get { return Cache(ref _ButterflyTex, "_ButterflyTex"); } }
        public static int ButterflyPass { get { return Cache(ref _ButterflyPass, "_ButterflyPass"); } }
        public static int Offset { get { return Cache(ref _Offset, "_Offset"); } }
        public static int MaxDistance { get { return Cache(ref _MaxDistance, "_MaxDistance"); } }
        public static int AbsorptionColorPerPixel { get { return Cache(ref _AbsorptionColorPerPixel, "_AbsorptionColorPerPixel"); } }
        public static int SurfaceOffset { get { return Cache(ref _SurfaceOffset, "_SurfaceOffset"); } }
        public static int WaterId { get { return Cache(ref _WaterId, "_WaterId"); } }
        public static int Cull { get { return Cache(ref _Cull, "_Cull"); } }
        public static int Period { get { return Cache(ref _Period, "_Period"); } }
        public static int BumpMapST { get { return Cache(ref _BumpMapST, "_BumpMap_ST"); } }
        public static int DetailAlbedoMapST { get { return Cache(ref _DetailAlbedoMapST, "_DetailAlbedoMap_ST"); } }
        public static int HeightTex { get { return Cache(ref _HeightTex, "_HeightTex"); } }
        public static int DisplacementTex { get { return Cache(ref _DisplacementTex, "_DisplacementTex"); } }
        public static int HorizontalDisplacementScale { get { return Cache(ref _HorizontalDisplacementScale, "_HorizontalDisplacementScale"); } }
        public static int JacobianScale { get { return Cache(ref _JacobianScale, "_JacobianScale"); } }

        public static int RenderTime { get { return Cache(ref _RenderTime, "_RenderTime"); } }
        #endregion Misc
        #endregion Public Variables

        #region Private Variables
        #region Local
        private static int _LocalDisplacementMap = -1;
        private static int _LocalNormalMap = -1;
        private static int _LocalDiffuseMap = -1;

        private static int _LocalMapsCoords = -1;
        private static int _LocalMapsCoordsPrevious = -1;
        #endregion Local
        #region Masks
        private static int _UnderwaterMask = -1;
        private static int _UnderwaterMask2 = -1;
        private static int _AdditiveMask = -1;
        private static int _SubtractiveMask = -1;
        private static int _DisplacementsMask = -1;
        #endregion Masks
        #region Gbuffer
        private static int _Gbuffer0 = -1;
        private static int _Gbuffer1 = -1;
        private static int _Gbuffer2 = -1;
        private static int _Gbuffer3 = -1;
        #endregion Gbuffer
        #region Occlusion
        private static int _OcclusionMap = -1;
        private static int _OcclusionMap2 = -1;
        private static int _OcclusionMapProjection = -1;
        #endregion Occlusion
        #region Foam
        private static int _FoamParameters = -1;
        private static int _FoamShoreIntensity = -1;
        private static int _FoamIntensity = -1;
        private static int _WaterTileSizeInvSrt = -1;
        #endregion Foam
        #region Tiles
        private static int _WaterTileSize = -1;
        private static int _WaterTileSizeInv = -1;
        private static int _WaterTileSizeScales = -1;
        private static int _MaxDisplacement = -1;
        #endregion Tiles
        #region Misc
        private static int _TotalDisplacementMap = -1;
        private static int _DisplacementMask = -1;

        private static int _WaterDepthTexture = -1;

        private static int _UnityMatrixVPInverse = -1;
        private static int _DepthClipMultiplier = -1;

        private static int _RefractTex = -1;
        private static int _WaterlessDepthTexture = -1;
        private static int _WaterShadowmap = -1;
        private static int _Velocity = -1;

        private static int _CameraDepthTexture2 = -1;
        private static int _PlanarReflectionTex = -1;

        private static int _ButterflyTex = -1;
        private static int _ButterflyPass = -1;

        private static int _Offset = -1;

        private static int _MaxDistance = -1;
        private static int _AbsorptionColorPerPixel = -1;

        private static int _SurfaceOffset = -1;
        private static int _WaterId = -1;
        private static int _Cull = -1;
        private static int _Period = -1;

        private static int _BumpMapST = -1;
        private static int _DetailAlbedoMapST = -1;

        private static int _HeightTex = -1;
        private static int _DisplacementTex = -1;
        private static int _HorizontalDisplacementScale = -1;
        private static int _JacobianScale = -1;
        private static int _RenderTime = -1;

        private static int[] _DisplacementDeltaMaps;
        #endregion Misc
        #endregion Private Variables

        #region Private Methods
        private static int Cache(ref int value, string name)
        {
            if (value != -1) { return value; }
            value = Shader.PropertyToID(name);
            return value;
        }
        #endregion Private Methods
    }
}