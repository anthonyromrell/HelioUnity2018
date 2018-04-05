// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Base/FFT"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _ButterflyTex("", 2D) = "" {}
    }

        CGINCLUDE

        #include "UnityCG.cginc"

        struct VertexInput
    {
        float4 vertex	: POSITION;
        float2 uv0		: TEXCOORD0;
    };

    struct VertexOutput
    {
        float4 pos	: SV_POSITION;
        half2 uv	: TEXCOORD0;
    };

    sampler2D _MainTex;
    sampler2D _ButterflyTex;
    half _ButterflyPass;

    VertexOutput vert(VertexInput vi)
    {
        VertexOutput vo;

        vo.pos = UnityObjectToClipPos(vi.vertex);
        vo.uv = vi.uv0;

        return vo;
    }

    ///
    /// FFT
    ///
    inline float4 FFT_1(sampler2D tex, half2 uv1, half2 uv2, float2 weights)
    {
        float2 a1 = tex2D(tex, uv1).xy;
        float2 b1 = tex2D(tex, uv2).xy;

        return float4(a1 + weights.xy * b1.xx + weights.yx * float2(-1, 1) * b1.yy, 0, 1);
    }

    inline float4 FFT_2(sampler2D tex, half2 uv1, half2 uv2, float2 weights)
    {
        float4 a1 = tex2D(tex, uv1).xyzw;
        float4 b1 = tex2D(tex, uv2).xyzw;

        return a1 + weights.xyxy * b1.xxzz + weights.yxyx * float4(-1, 1, -1, 1) * b1.yyww;
    }

    ///
    /// Single FFT
    ///
    float4 hfft_1(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.x, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_1(_MainTex, half2(indices.x, In.uv.y), half2(indices.y, In.uv.y), weights);
    }

        float4 vfft_1(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.y, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_1(_MainTex, half2(In.uv.x, indices.x), half2(In.uv.x, indices.y), weights);
    }

        ///
        /// Two FFTs at a time
        ///
        float4 hfft_2(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.x, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_2(_MainTex, half2(indices.x, In.uv.y), half2(indices.y, In.uv.y), weights);
    }

        float4 vfft_2(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.y, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_2(_MainTex, half2(In.uv.x, indices.x), half2(In.uv.x, indices.y), weights);
    }

        ///
        /// Real-valued output versions
        ///
        float4 vfft_1r(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.y, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_1(_MainTex, half2(In.uv.x, indices.x), half2(In.uv.x, indices.y), weights).rgba;
    }

        float4 vfft_2r(VertexOutput In) : SV_Target
    {
        float4 butterfly = tex2D(_ButterflyTex, half2(In.uv.y, _ButterflyPass));

        half2 indices = butterfly.rg;
        float2 weights = butterfly.ba;

        return FFT_2(_MainTex, half2(In.uv.x, indices.x), half2(In.uv.x, indices.y), weights).rbbb;
    }

        ENDCG

        SubShader
    {
        Cull Off
            ZWrite Off
            ZTest Always
            Blend Off

            Pass
        {
            Name "hFFT1"
            ColorMask RG

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment hfft_1

            ENDCG
        }

            Pass
        {
            Name "vFFT1"
            ColorMask RG

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment vfft_1

            ENDCG
        }

            Pass
        {
            Name "hFFT2"
            ColorMask RGBA

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment hfft_2

            ENDCG
        }

            Pass
        {
            Name "vFFT2"
            ColorMask RGBA

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment vfft_2

            ENDCG
        }

            Pass
        {
            Name "vFFT1r"
            ColorMask R

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment vfft_1r

            ENDCG
        }

            Pass
        {
            Name "vFFT2r"
            ColorMask RG

            CGPROGRAM

            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment vfft_2r

            ENDCG
        }
    }
}