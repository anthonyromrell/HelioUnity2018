// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utilities/Blur (Subsurface Scattering)"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

        CGINCLUDE

        #define KERNEL_MEDIUM 1

        #include "UnityCG.cginc"
        #include "../Includes/DiskKernels.cginc"

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
    half _MaxDistance;
    half _PixelSize;
    half3 _AbsorptionColorPerPixel;
    half2 _ScatteringParams;

    VertexOutput vert(VertexInput vi)
    {
        VertexOutput vo;

        vo.pos = UnityObjectToClipPos(vi.vertex);
        vo.uv = vi.uv0;

        return vo;
    }

    half4 frag(VertexOutput i) : SV_Target
    {
        half3 scattering = 0.0;

        UNITY_LOOP for(int si = 0; si < kSampleCount; si++)
        {
            float2 disp = kDiskKernel[si] * _MaxDistance;
            float dist = length(disp);

            half3 samp = tex2D(_MainTex, i.uv + disp);
            half3 bgWeight = exp(_AbsorptionColorPerPixel * dist);

            scattering += samp * bgWeight;
        }

        return half4(scattering * 0.025, 1);
    }

        struct VertexInput2
    {
        float4 vertex	: POSITION;
        float2 uv0		: TEXCOORD0;
    };

    struct VertexOutput3
    {
        float4 pos		: SV_POSITION;
        half2 uv		: TEXCOORD0;
        half2 uv1		: TEXCOORD1;
    };

    VertexOutput3 vertFinalize(VertexInput2 vi)
    {
        VertexOutput3 vo;

        vo.pos = UnityObjectToClipPos(vi.vertex);
        vo.uv = vi.uv0;
        vo.uv1 = vi.uv0 * 2.0 - 1.0;

        return vo;
    }

    half4 fragFinalize(VertexOutput3 vo) : SV_Target
    {
        half3 color1 = tex2D(_MainTex, vo.uv);
        half3 color2 = tex2D(_MainTex, half2(0.4, 0.4)) * 0.2;
        color2 += tex2D(_MainTex, half2(0.6, 0.6)) * 0.2;
        color2 += tex2D(_MainTex, half2(0.4, 0.6)) * 0.2;
        color2 += tex2D(_MainTex, half2(0.6, 0.4)) * 0.2;

        half p = min(1.0, pow(length(vo.uv1), 2));
        half3 color = lerp(color1, color2, p);

        //half normal = tex2D(_MainTex, vo.uv).a;
        //color *= lerp(1.0, normal, _ScatteringContrast);

        return half4(color, _ScatteringParams.x /* lerp(1.0, tex2D(_MainTex, vo.uv).a, _ScatteringParams.y * (1.0 - p))*/);
    }

        ENDCG

        SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 3.0

            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }

            Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 3.0

            #pragma vertex vertFinalize
            #pragma fragment fragFinalize

            ENDCG
        }
    }
}