// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Underwater/Compose Underwater Mask"
{
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
        float2 uv	: TEXCOORD0;
    };

    sampler2D _SubtractiveMask;

    VertexOutput vert(VertexInput vi)
    {
        VertexOutput vo;
        vo.pos = UnityObjectToClipPos(vi.vertex);
        vo.uv = vi.uv0;

        return vo;
    }

    fixed4 frag(VertexOutput vo) : SV_Target
    {
        half4 c = tex2D(_SubtractiveMask, vo.uv);
        fixed4 result;
#if UNITY_VERSION >= 550
        result.rgb = c.z > 900000 ? c.w : 1;
        result.a = c.z > 700000 && c.z < 900000 ? 1 : 0;
#else
        result.rgb = c.y > 900000 ? c.w : 1;
        result.a = c.y > 700000 && c.y < 900000 ? 1 : 0;
#endif
        return result;
    }

        ENDCG

        SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Blend SrcAlpha SrcColor
            BlendOp Add
            ColorMask R

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }
    }
}