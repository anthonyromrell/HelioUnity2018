Shader "UltimateWater/Utility/Info"
{
    /*SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma target 5.0
            #pragma only_renderers d3d11

            #if UNITY_CAN_COMPILE_TESSELLATION
                #pragma vertex tessvert_surf
                #pragma fragment frag

                #pragma hull hs_surf
                #pragma domain ds_surf
            #endif

            #pragma multi_compile __ _WAVES_FFT
            #pragma multi_compile ____ _WAVES_GERSTNER
            #define _TRIANGLES 1

            #define POST_TESS_VERT vert
            #define TESS_OUTPUT VertexOutput

            #include "Mapper.cginc"
            #include "WaterTessellation.cginc"

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

            #pragma multi_compile __ _WAVES_FFT
            #pragma multi_compile ____ _WAVES_GERSTNER

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "Mapper.cginc"

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

            #pragma multi_compile ____ _WAVES_GERSTNER

            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            #include "Mapper.cginc"

            ENDCG
        }
    }
}