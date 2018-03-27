Shader "UltimateWater/Wettable Shaders/Terrain Standard" {
    Properties{
        // set by terrain engine
        [HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
        [HideInInspector][Gamma] _Metallic0("Metallic 0", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic1("Metallic 1", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic2("Metallic 2", Range(0.0, 1.0)) = 0.0
        [HideInInspector][Gamma] _Metallic3("Metallic 3", Range(0.0, 1.0)) = 0.0
        [HideInInspector] _Smoothness0("Smoothness 0", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness1("Smoothness 1", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness2("Smoothness 2", Range(0.0, 1.0)) = 1.0
        [HideInInspector] _Smoothness3("Smoothness 3", Range(0.0, 1.0)) = 1.0

        _WetEffectIntensity("Wet Effect Intensity", Float) = 1.0

            // used in fallback on old cards & base map
            [HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
            [HideInInspector] _Color("Main Color", Color) = (1,1,1,1)

            [HideInInspector] _WetnessMap("Wetness", 2D) = "black" {}
    }

        SubShader{
            Tags {
                "Queue" = "Geometry-100"
                "RenderType" = "Opaque"
                "CustomType" = "Wettable"
            }

            CGPROGRAM
            #pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer fullforwardshadows
        //#pragma multi_compile_fog
        #pragma target 3.0
        // needs more than 8 texcoords
        #pragma exclude_renderers gles
        #include "UnityPBSLighting.cginc"

        #pragma multi_compile __ _TERRAIN_NORMAL_MAP
        #pragma multi_compile ___ _WET_NEAR_CAMERA

        #define TERRAIN_STANDARD_SHADER
        #define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
        #include "TerrainSplatmapCommon.cginc"

        half _Metallic0;
        half _Metallic1;
        half _Metallic2;
        half _Metallic3;

        half _Smoothness0;
        half _Smoothness1;
        half _Smoothness2;
        half _Smoothness3;

        sampler2D _WetnessMap;

        void surf(Input IN, inout SurfaceOutputStandard o)
 {
half4 splat_control;
half weight;
fixed4 mixedDiffuse;
half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);
o.Albedo = mixedDiffuse.rgb;
o.Alpha = weight;
o.Smoothness = mixedDiffuse.a;
o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));

#if defined(_WET_NEAR_CAMERA)
            half2 wetnessUv = IN.localUv.xy;
#else
            half2 wetnessUv = IN.tc_Control.xy;
#endif

            half3 wetness;
            wetness.xy = tex2D(_WetnessMap, wetnessUv).xx * half2(0.08, 0.01);
            wetness.z = sin(min(wetness.x * 2.0, 3.14159));
            wetness.xy = saturate(wetness.xy);

            // wetness
            o.Metallic = lerp(o.Metallic, 1.0, wetness.y);
            o.Albedo *= 1.0 - wetness.x * 0.3;
            o.Smoothness += (1.0 - o.Smoothness) * wetness.x * 0.4;
        }
        ENDCG
    }

        Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
            Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"

            Fallback "Nature/Terrain/Diffuse"
}
