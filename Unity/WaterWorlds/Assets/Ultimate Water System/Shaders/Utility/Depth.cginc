#include "UnityCG.cginc"
#include "../Includes/UnityStandardCore.cginc"

struct v2f
{
    float4 pos			: SV_POSITION;
    float4 screenPos		: TEXCOORD0;
    #if _CLIP_ABOVE
    float3 worldPos		: TEXCOORD1;
    #endif
};

v2f vert(VertexInput vi)
{
    v2f o;

    float4 posWorld = GET_WORLD_POS(vi.vertex);

    half2 normal;
    float4 fftUV;
    float4 fftUV2;
    float3 displacement;
    float4 projectorViewPos;
    TransformVertex(DistanceMask(vi, posWorld), posWorld, /*out*/ normal, /*out*/ fftUV, /*out*/ fftUV2, /*out*/ displacement, /*out*/ projectorViewPos);

    o.pos = mul(UNITY_MATRIX_VP, posWorld);
    o.screenPos = ComputeScreenPos(o.pos);
    o.screenPos.zw = o.pos.zw;

    #if _CLIP_ABOVE
    o.worldPos = posWorld.xyz;
    #endif

    return o;
}

float4 frag(v2f i) : SV_Target
{
    half alpha;

#if _CLIP_ABOVE
    MaskWater(alpha, i.screenPos, i.worldPos);
#else
    MaskWater(alpha, i.screenPos, 0);
#endif

    clip(alpha - 0.006);

    //#if defined(UNITY_MIGHT_NOT_HAVE_DEPTH_TEXTURE)
        return i.screenPos.z / i.screenPos.w;
        //#else
        //	return 0;
        //#endif
}