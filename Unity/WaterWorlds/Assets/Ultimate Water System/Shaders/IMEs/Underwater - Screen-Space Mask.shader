Shader "UltimateWater/Underwater/Screen-Space Mask"
{
    SubShader
    {
        Tags { "CustomType" = "Water" }

        Pass
        {
            ZTest Always
            //Cull Front
            Cull Off
            ColorMask R
        //BlendOp Min
        Blend One One

        CGPROGRAM

        #pragma target 5.0
        #pragma only_renderers d3d11

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile ____ _WAVES_GERSTNER
        #pragma multi_compile _______ _WAVES_ALIGN
        #pragma multi_compile ___ _TRIANGLES

        #if UNITY_CAN_COMPILE_TESSELLATION
            #pragma vertex tessvert_surf
            #pragma fragment maskFrag

            #pragma hull hs_surf
            #pragma domain ds_surf
        #endif

        #define _WATER_OVERLAYS 1
        #define POST_TESS_VERT vert
        #define TESS_OUTPUT VertexOutput

        #include "Underwater - Screen-Space Mask.cginc"
        #include "../Includes/WaterTessellation.cginc"

        ENDCG
    }
    }

        SubShader
    {
        Tags{ "CustomType" = "Water" }

        Pass
        {
            ZTest Always
            //Cull Front
            Cull Off
            ColorMask R
        //BlendOp Min
        Blend One One

        CGPROGRAM

        #pragma target 3.0

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile ____ _WAVES_GERSTNER
        #pragma multi_compile _______ _WAVES_ALIGN

        #define _WATER_OVERLAYS 1

        #pragma vertex vert
        #pragma fragment maskFrag

        #include "Underwater - Screen-Space Mask.cginc"

        ENDCG
    }
    }

        SubShader
    {
        Tags{ "CustomType" = "Water" }

        Pass
        {
            ZTest Always
            //Cull Front
            Cull Off
            ColorMask R
        //BlendOp Min
        Blend One One

        CGPROGRAM

        #pragma target 2.0

        #pragma multi_compile ____ _WAVES_GERSTNER

        #pragma vertex vert
        #pragma fragment maskFrag

        #include "Underwater - Screen-Space Mask.cginc"

        ENDCG
    }
    }
}