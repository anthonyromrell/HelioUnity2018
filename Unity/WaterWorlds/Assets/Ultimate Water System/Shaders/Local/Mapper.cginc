#include "UnityCG.cginc"
#include "../Includes/UnityStandardCore.cginc"

struct VertexOutput
{
    float4 pos			: SV_POSITION;
    half2 heights		: TEXCOORD0;
    half2 screenPos		: TEXCOORD1;
    half4 fftUV			: TEXCOORD2;
};

VertexOutput vert(VertexInput vi)
{
    VertexOutput vo;

    float4 posWorld = GET_WORLD_POS(vi.vertex);
    half neutralY = posWorld.y;

    half2 normal;
    float4 fftUV;
    float4 fftUV2;
    float3 displacement;
    float4 projectorViewPos;
    TransformVertex(DistanceMask(vi, posWorld), posWorld, normal, fftUV, fftUV2, displacement, projectorViewPos);

    vo.pos = mul(UNITY_MATRIX_VP, posWorld);
    vo.heights = half2(neutralY, posWorld.y);
    vo.screenPos = vo.pos.xy;			// it's ortographic projection, so two components are enough
    vo.fftUV = fftUV;

    return vo;
}

half4 frag(VertexOutput vo) : SV_Target
{
    // fade near edges
    half2 k = abs(vo.screenPos);
    //half4 result = lerp(vo.heights.x, vo.heights.y, max(0, 1.0 - pow(max(k.x, k.y), 6)));
    half2 result;
    result.x = lerp(vo.heights.y, vo.heights.x, max(0, pow(max(k.x, k.y), 6)));
    //result.x = vo.heights.y;

    half jacobian = tex2D(_GlobalDisplacementMap, vo.fftUV.xy).w;
    jacobian = max(jacobian, tex2D(_GlobalDisplacementMap1, vo.fftUV.zw).w);
    jacobian = max(jacobian, tex2D(_GlobalDisplacementMap2, vo.fftUV.xy * _WaterTileSize.x / _WaterTileSize.z).w);
    jacobian = max(jacobian, tex2D(_GlobalDisplacementMap3, vo.fftUV.xy * _WaterTileSize.x / _WaterTileSize.w).w);
    result.y = jacobian;

    return half4(result, 0, 1);
}