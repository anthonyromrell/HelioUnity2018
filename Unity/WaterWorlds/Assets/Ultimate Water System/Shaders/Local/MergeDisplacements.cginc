#include "UnityCG.cginc"
#include "../Includes/UnityStandardCore.cginc"

struct VertexOutput2
{
    float4 pos			: SV_POSITION;
    half3 displacement	: TEXCOORD0;
};

VertexOutput2 vert(VertexInput vi)
{
    VertexOutput2 vo;

    float4 posWorld = GET_WORLD_POS(vi.vertex);

    half2 normal;
    float4 fftUV;
    float4 fftUV2;
    float3 displacement;
    float4 projectorViewPos;
    TransformVertex(DistanceMask(vi, posWorld), posWorld, normal, fftUV, fftUV2, displacement, projectorViewPos);

    vo.pos = mul(UNITY_MATRIX_VP, posWorld);
    vo.displacement = displacement;

    return vo;
}

half4 frag(VertexOutput2 vo) : SV_Target
{
    return half4(vo.displacement, 1);
}