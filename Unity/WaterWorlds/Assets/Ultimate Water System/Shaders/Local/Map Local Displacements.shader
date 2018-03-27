Shader "UltimateWater/Utility/Map Local Displacements"
{
    SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back
        ZTest Always
        ZWrite Off

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma multi_compile __ _WAVES_FFT
            #pragma multi_compile ____ _WAVES_GERSTNER
            #pragma multi_compile _______ _WAVES_ALIGN

            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag

            #define _WATER_OVERLAYS 1
            #define _FORCE_FULL_DISPLACEMENT 1

            #include "MergeDisplacements.cginc"

            ENDCG
        }
    }

        SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back
        ZTest Always
        ZWrite Off

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma multi_compile __ _WAVES_FFT
            #pragma multi_compile ____ _WAVES_GERSTNER
            #pragma multi_compile _______ _WAVES_ALIGN

            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #define _WATER_OVERLAYS 1
            #define _FORCE_FULL_DISPLACEMENT 1

            #include "MergeDisplacements.cginc"

            ENDCG
        }
    }

        SubShader
    {
        Tags{ "CustomType" = "Water" }
        Cull Back
        ZTest Always
        ZWrite Off

        Pass
        {
            Fog{ Mode Off }

            CGPROGRAM

            #pragma multi_compile ____ _WAVES_GERSTNER

            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            #define _FORCE_FULL_DISPLACEMENT 1

            #include "MergeDisplacements.cginc"

            ENDCG
        }
    }
}