Shader "UltimateWater/Utilities/WaterNormalMap"
{
    Properties
    {
        _MainTex("", 2D) = "bump"
        _Period("Period", Float) = (8, 8, 8, 8)
        _Offset("Offset", Float) = (0, 0, 0, 0)
    }

        CGINCLUDE

        #define NOISE_FUNC Perlin3D(coord1.xyz)
        #define NOISE_FUNC_2 Perlin3D(coord2.xyz + 100)
        #define NOISE_FUNC_3 Perlin3D(coord3.xyz - 100)

        #include "UnityCG.cginc"
        #include "NoiseLib.cginc"

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

    sampler2D _MainTex;

    float4 _Period;
    float4 _Offset;
    float _Param;

    VertexOutput vert(VertexInput vi)
    {
        VertexOutput vo;

        vo.pos = float4(vi.uv0 * 2 - 1, 0, 1);
        vo.uv = vi.uv0;

        return vo;
    }

    float4 PackNormal(float4 normal)
    {
        return float4(0.0, normal.y, 0.0, normal.x);
    }

    float4 fragNormal(VertexOutput vo) : SV_Target
    {
        float3 source = UnpackNormal(tex2D(_MainTex, vo.uv));

        float4 uv = float4(vo.uv, 0, 0) + _Offset;
        float4 coord1 = uv * _Period.x;
        float4 coord2 = uv * _Period.y;
        float4 val = float4(NOISE_FUNC, NOISE_FUNC_2, _Param, 1);

        val.xyz = normalize(source * normalize(val.xyz));
        val.xyz = val.xyz * 0.5 + 0.5;

        return PackNormal(val);
    }

        ENDCG

        SubShader
    {
        Cull Off

            Pass
        {
            Name "Perlin NM"

            CGPROGRAM

            #pragma target 3.0

            #pragma vertex vert
            #pragma fragment fragNormal
            //#define NOISE_FUNC Perlin3D(coord.xyz)

            ENDCG
        }
    }
}