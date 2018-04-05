Shader "UltimateWater/Depth/Water Depth"
{
    Properties
    {
        _LocalDisplacementMap("", 2D) = "black" {}
        _WaterTileSizeInv("", Vector) = (180.0, 18.0, 600.0, 1800.0)
        _SurfaceOffset("", Vector) = (0.0, 0.0, 0.0, 0.0)
        _WaterId("", Vector) = (128, 256, 0, 0)
    }

        /*
         * WATER
         */
            SubShader
        {
            Pass
            {
                Cull Off
                ZTest Less
                ZWrite On

                Fog { Mode Off }

                CGPROGRAM

                #pragma target 5.0

                #if UNITY_CAN_COMPILE_TESSELLATION
                    #pragma vertex tessvert_surf
                    #pragma fragment frag

                    #pragma hull hs_surf
                    #pragma domain ds_surf
                #endif

                #pragma multi_compile __ _WAVES_FFT
                #pragma multi_compile ____ _WAVES_GERSTNER
                #pragma multi_compile _____ _BOUNDED_WATER
                #pragma multi_compile _______ _WAVES_ALIGN
                #pragma multi_compile ___ _TRIANGLES

                #define _DEPTH 1
                #define _WATER_OVERLAYS 1

                #define POST_TESS_VERT vert
                #define TESS_OUTPUT v2f

                #include "Depth.cginc"
                #include "../Includes/WaterTessellation.cginc"

                ENDCG
            }

            Pass
            {
                Cull Off
                ZTest Less
                ZWrite On

                Fog { Mode Off }

                CGPROGRAM

                #pragma multi_compile __ _WAVES_FFT
                #pragma multi_compile ____ _WAVES_GERSTNER

                #define _DISPLACED_VOLUME 1
                #define _CLIP_ABOVE 1
                #define _DEPTH 1

                #pragma target 3.0
                #pragma vertex vert
                #pragma fragment frag

                #include "Depth.cginc"

                ENDCG
            }
        }

            SubShader
        {
            Cull Off
            ZTest Less
            ZWrite On

            Pass
            {
                Fog { Mode Off }

                CGPROGRAM

                #pragma multi_compile __ _WAVES_FFT
                #pragma multi_compile ____ _WAVES_GERSTNER
                #pragma multi_compile _____ _BOUNDED_WATER
                #pragma multi_compile _______ _WAVES_ALIGN

                #define _DEPTH 1
                #define _WATER_OVERLAYS 1

                #pragma target 3.0
                #pragma vertex vert
                #pragma fragment frag

                #include "Depth.cginc"

                ENDCG
            }
        }

            /*SubShader
            {
                Tags{ "CustomType" = "Water" }
                Cull Off
                ZTest Less
                ZWrite On

                Pass
                {
                    Fog{ Mode Off }

                    CGPROGRAM

                    #pragma multi_compile ____ _WAVES_GERSTNER
                    #pragma multi_compile _____ _BOUNDED_WATER

                    #define _DEPTH 1
                    #define _WATER_OVERLAYS 1

                    #pragma target 2.0
                    #pragma vertex vert
                    #pragma fragment frag

                    #include "Depth.cginc"

                    ENDCG
                }
            }*/

            /*
             * WATER VOLUME
             */
             /*SubShader
             {
                 Tags { "CustomType"="WaterVolume" }
                 Cull Off
                 ZTest Less
                 ZWrite On

                 Pass
                 {
                     Fog { Mode Off }

                     CGPROGRAM

                     #pragma multi_compile __ _WAVES_FFT
                     #pragma multi_compile ____ _WAVES_GERSTNER

                     #define _DISPLACED_VOLUME 1
                     #define _CLIP_ABOVE 1
                     #define _DEPTH 1

                     #pragma target 3.0
                     #pragma vertex vert
                     #pragma fragment frag

                     #include "Depth.cginc"

                     ENDCG
                 }
             }*/

             /*SubShader
             {
                 Tags{ "CustomType" = "WaterVolume" }
                 Cull Off
                 ZTest Less
                 ZWrite On

                 Pass
                 {
                     Fog{ Mode Off }

                     CGPROGRAM

                     #pragma multi_compile ____ _WAVES_GERSTNER

                     #define _DISPLACED_VOLUME 1
                     #define _CLIP_ABOVE 1
                     #define _DEPTH 1

                     #pragma target 2.0
                     #pragma vertex vert
                     #pragma fragment frag

                     #include "Depth.cginc"

                     ENDCG
                 }
             }*/
}