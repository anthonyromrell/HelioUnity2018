#include "UnityCG.cginc"
#include "../Includes/UnityStandardCore.cginc"

struct v2f
{
    float4 vertex : SV_POSITION;
    half4 fftUV : TEXCOORD0;
    half4 fftUV2 : TEXCOORD1;
};

sampler2D_float _WorldPosMap;
half _CausticLightIntensity;

v2f vert(VertexInput v)
{
    v2f o;
    float4 posWorld = v.vertex;
    half2 normal;
    float4 fftUV, fftUV2;
    float3 totalDisplacement;
    float4 projectorViewPos;

    TransformVertex(DistanceMask(v, posWorld), posWorld, /*out*/ normal, /*out*/ fftUV, /*out*/ fftUV2, /*out*/ totalDisplacement, /*out*/ projectorViewPos, false);

    o.vertex = mul(UNITY_MATRIX_VP, posWorld);
    o.fftUV = fftUV;
    o.fftUV2 = fftUV2;

    return o;
}

half4 frag(v2f i) : SV_Target
{
    half2 normals = tex2D(_GlobalNormalMap, i.fftUV.xy).xy + tex2D(_GlobalNormalMap, i.fftUV.zw).zw + tex2D(_GlobalNormalMap1, i.fftUV2.xy).xy + tex2D(_GlobalNormalMap1, i.fftUV2.zw).zw;
    return half4(normals, 0.0, 0.0);
}