// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Spray/Spray To Foam"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

        CGINCLUDE
        #define postGeomVert vert
        #define vertexInputType VertexOutput2

        #include "UnityCG.cginc"
        #include "UnityStandardCore.cginc"

        struct VertexOutput2
    {
        float4 pos				: SV_POSITION;
        half2 uv				: TEXCOORD0;
        half particleData		: TEXCOORD1;
    };

    float4 _LocalMapsCoords;

    VertexOutput2 vert(VertexInput vi)
    {
        VertexOutput2 vo;

        vo.pos.xy = (vi.vertex.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy) * 2.0 - 1.0;
        vo.pos.zw = float2(0.75, 1.0);
        vo.pos.y = -vo.pos.y;
        vo.uv = vi.uv0;
        vo.particleData = vi.particleData.x * 0.001;

        return vo;
    }

    half4 frag(VertexOutput2 vo) : SV_Target
    {
        return vo.particleData * max(0, sin(length(vo.uv.xy) * 3.14159));
    }

        struct VertexOutputReprojection
    {
        float4 pos				: SV_POSITION;
        half2 uv				: TEXCOORD0;
    };

    VertexOutputReprojection vertReprojection(VertexInput vi)
    {
        VertexOutputReprojection vo;

        vo.pos = UnityObjectToClipPos(vi.vertex);
        vo.uv = vi.uv0;

        return vo;
    }

    half4 fragReprojection(VertexOutputReprojection vo) : SV_Target
    {
        return tex2D(_MainTex, vo.uv);
    }
        ENDCG

        SubShader
    {
        Pass
        {
            Blend One One
            ZWrite Off ZTest Off Cull Off

            CGPROGRAM
            #pragma target 5.0

            #pragma vertex geomVert
            #pragma geometry geom
            #pragma fragment frag

            #define _FOAM_GAIN_MAP 1

            #include "ParticleGeometry.cginc"

            ENDCG
        }

            Pass
        {
            Blend One One
            ZWrite Off ZTest Off Cull Off

            CGPROGRAM
            #pragma target 5.0

            #pragma vertex vertReprojection
            #pragma fragment fragReprojection

            ENDCG
        }
    }
}