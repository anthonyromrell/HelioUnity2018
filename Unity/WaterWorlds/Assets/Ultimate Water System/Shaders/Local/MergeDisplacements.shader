Shader "UltimateWater/Utility/MergeDisplacements"
{
    /*SubShader
    {
        Tags { "CustomType" = "Water" }
        Cull Back

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma target 5.0
            #pragma only_renderers d3d11

            #pragma multi_compile __ _WAVES_FFT
            #pragma multi_compile ____ _WAVES_GERSTNER
            #pragma multi_compile _______ _WAVES_ALIGN
            #pragma multi_compile ___ _TRIANGLES

            #if UNITY_CAN_COMPILE_TESSELLATION
                #pragma vertex tessvert_surf
                #pragma fragment frag

                #pragma hull hs_surf
                #pragma domain ds_surf
            #endif

            #define _WATER_OVERLAYS 1
            #define POST_TESS_VERT vert
            #define TESS_OUTPUT VertexOutput2

            #include "MergeDisplacements.cginc"
            #include "../Includes/WaterTessellation.cginc"

            ENDCG
        }
    }*/

    SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "MergeDisplacements.cginc"

            ENDCG
        }
    }

        SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            #include "MergeDisplacements.cginc"

            ENDCG
        }
    }
}