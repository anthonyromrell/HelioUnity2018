// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Caustics/Utility"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
    {
        float4 vertex : POSITION;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        float2 worldPos : TEXCOORD0;
    };

    float4x4 _InvProjMatrix;
    half _CausticLightIntensity;
    sampler2D _MainTex;

    v2f vert(appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.worldPos.xy = mul(_InvProjMatrix, o.vertex).xz;
        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        return half4(i.worldPos.x, -200.0, i.worldPos.y, 0.0);
    }

        struct appdata_MaskEdges
    {
        float4 vertex	: POSITION;
        float2 uv		: TEXCOORD0;
    };

    struct v2f_maskEdges
    {
        float4 vertex	: SV_POSITION;
        half2 uv		: TEXCOORD0;
    };

    v2f_maskEdges vertMaskEdges(appdata_MaskEdges v)
    {
        v2f_maskEdges o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = (v.uv * 2 - 1);
        return o;
    }

    half4 fragMaskEdges(v2f_maskEdges i) : SV_Target
    {
        half p = min(1.0, pow(length(i.uv), 2));
        return half4(_CausticLightIntensity * 0.7 * p, 0.0, 0.0, (1.0 - p));
    }
        ENDCG

        SubShader
    {
        Cull Off ZWrite Off ZTest Always

            Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }

            Pass
        {
            Blend One SrcAlpha

            CGPROGRAM
            #pragma vertex vertMaskEdges
            #pragma fragment fragMaskEdges

            ENDCG
        }
    }
}