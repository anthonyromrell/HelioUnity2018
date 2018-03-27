Shader "UltimateWater/Depth/Depth Copy" {
    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata_t
    {
        float4 vertex : POSITION;
        half2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        half2 texcoord : TEXCOORD0;
    };

    sampler2D_float _CameraDepthTexture;
    float4 _CameraDepthTexture_TexelSize;
    sampler2D_float _WaterDepthTexture;
    sampler2D_float _WaterlessDepthTexture;

    v2f vert(appdata_t v)
    {
        v2f o;
        o.vertex = float4(v.vertex.xyz, 1.0);
        #if UNITY_SINGLE_PASS_STEREO
        o.vertex.x = o.vertex.x * 1.0;
        #endif
        //o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
        o.texcoord = v.texcoord.xy;
        return o;
    }

    float fragDepthBuffer(v2f i) : SV_Depth
    {
        return tex2D(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(i.texcoord));
    }

        float fragMixDepthBuffer(v2f i) : SV_Depth
    {
        float d1 = tex2D(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(i.texcoord));
        float d2 = tex2D(_WaterDepthTexture, UnityStereoTransformScreenSpaceTex(i.texcoord));

#if UNITY_VERSION < 550
            return min(d1, d2);
#else
            return max(d1, d2);
#endif
    }

        float fragMixDeferredDepthBuffer(v2f i) : SV_Depth
    {
        float d1 = tex2D(_WaterlessDepthTexture, i.texcoord);
        float d2 = tex2D(_CameraDepthTexture, i.texcoord);

#if UNITY_VERSION < 550
            return min(d1, d2);
#else
            return max(d1, d2);
#endif
    }

        float4 fragFloat(v2f i) : SV_Target
    {
        return tex2D(_CameraDepthTexture, i.texcoord);
    }

        float4 fragMixFloat(v2f i) : SV_Target
    {
        float d1 = tex2D(_CameraDepthTexture, i.texcoord);
        float d2 = tex2D(_WaterDepthTexture, i.texcoord);

#if UNITY_VERSION < 550
            return min(d1, d2);
#else
            return max(d1, d2);
#endif
    }

        float4 fragMixDeferredFloat(v2f i) : SV_Target
    {
        float d1 = tex2D(_WaterlessDepthTexture, i.texcoord);
        float d2 = tex2D(_CameraDepthTexture, i.texcoord);

#if UNITY_VERSION < 550
            return min(d1, d2);
#else
            return max(d1, d2);
#endif
    }
        ENDCG

        SubShader
    {
        /* Float Target */
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragFloat

            ENDCG
        }

            Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragMixFloat

            ENDCG
        }

            Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragMixDeferredFloat

            ENDCG
        }

            /* Depth Map Target */
            Pass
        {
            ZTest Always Cull Off ZWrite On ColorMask 0

            CGPROGRAM

            #pragma target 4.0

            #pragma vertex vert
            #pragma fragment fragDepthBuffer

            ENDCG
        }

            Pass
        {
            ZTest Always Cull Off ZWrite On ColorMask 0

            CGPROGRAM

            #pragma target 4.0

            #pragma vertex vert
            #pragma fragment fragMixDepthBuffer

            ENDCG
        }

            Pass
        {
            ZTest Always Cull Off ZWrite On ColorMask 0

            CGPROGRAM

            #pragma target 4.0

            #pragma vertex vert
            #pragma fragment fragMixDeferredDepthBuffer

            ENDCG
        }
    }

    SubShader
    {
        /* Float Target */
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragFloat

            ENDCG
        }

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragMixFloat

            ENDCG
        }

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment fragMixDeferredFloat

            ENDCG
        }
    }

        Fallback Off
}