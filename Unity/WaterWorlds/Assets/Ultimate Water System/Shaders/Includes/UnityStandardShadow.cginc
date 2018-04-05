// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef UNITY_STANDARD_SHADOW_INCLUDED
#define UNITY_STANDARD_SHADOW_INCLUDED

// NOTE: had to split shadow functions into separate file,
// otherwise compiler gives trouble with LIGHTING_COORDS macro (in UnityStandardCore.cginc)

#include "../Includes/UnityVersionsCompatibility.cginc"
#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"
#include "../Includes/UnityStandardConfig.cginc"
#include "../Includes/WaterLib.cginc"

// Do dithering for alpha blended shadows on SM3+/desktop;
// on lesser systems do simple alpha-tested shadows
//#if defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
//	#if !((SHADER_TARGET < 30) || defined (SHADER_API_MOBILE) || defined(SHADER_API_D3D11_9X) || defined (SHADER_API_PSP2) || defined (SHADER_API_PSM))
//	#define UNITY_STANDARD_USE_DITHER_MASK 1
//	#endif
//#endif

// Need to output UVs in shadow caster, since we need to sample texture and do clip/dithering based on it
//#if defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
//#define UNITY_STANDARD_USE_SHADOW_UVS 1
//#endif

// Has a non-empty shadow caster output struct (it's an error to have empty structs on some platforms...)
#if !defined(V2F_SHADOW_CASTER_NOPOS_IS_EMPTY) || defined(UNITY_STANDARD_USE_SHADOW_UVS)
#define UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT 1
#endif

half4		_Color;
half		_Cutoff;
sampler2D	_MainTex;
float4		_MainTex_ST;
#ifdef UNITY_STANDARD_USE_DITHER_MASK
sampler3D	_DitherMaskLOD;
#endif

struct VertexInput
{
    float4 vertex	: POSITION;
    float3 normal	: NORMAL;
    float2 uv0		: TEXCOORD0;
};

//#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
struct VertexOutputShadowCaster
{
    float4 pos	: SV_POSITION;
    V2F_SHADOW_CASTER_NOPOS
        #if defined(UNITY_STANDARD_USE_SHADOW_UVS)
        float2 tex : TEXCOORD1;
    #endif
};
//#endif

struct VertexOutputDepth
{
    float4 pos		: SV_POSITION;
    //#if defined(UNITY_MIGHT_NOT_HAVE_DEPTH_TEXTURE)
    float2 depth	: TEXCOORD0;
    //#endif
};

float4 UnityClipSpaceShadowCasterPosWS(float3 wPos, float3 normal)
{
    float4 clipPos;

    // Important to match MVP transform precision exactly while rendering
    // into the depth texture, so branch on normal bias being zero.
    if(unity_LightShadowBias.z != 0.0)
    {
        float3 wNormal = UnityObjectToWorldNormal(normal);
        float3 wLight = normalize(UnityWorldSpaceLightDir(wPos));

        // apply normal offset bias (inset position along the normal)
        // bias needs to be scaled by sine between normal and light direction
        // (http://the-witness.net/news/2013/09/shadow-mapping-summary-part-1/)
        //
        // unity_LightShadowBias.z contains user-specified normal offset amount
        // scaled by world space texel size.

        float shadowCos = dot(wNormal, wLight);
        float shadowSine = sqrt(1 - shadowCos*shadowCos);
        float normalBias = unity_LightShadowBias.z * shadowSine;

        wPos -= wNormal * normalBias;

        clipPos = mul(UNITY_MATRIX_VP, float4(wPos, 1));
    }
    else
    {
        clipPos = mul(UNITY_MATRIX_VP, float4(wPos, 1));
    }
    return clipPos;
}

// We have to do these dances of outputting SV_POSITION separately from the vertex shader,
// and inputting VPOS in the pixel shader, since they both map to "POSITION" semantic on
// some platforms, and then things don't go well.

VertexOutputShadowCaster vertShadowCaster(VertexInput v)
{
    VertexOutputShadowCaster o;

    float4 posWorld = GET_WORLD_POS(v.vertex);

    half2 normal;
    float4 fftUV;
    float4 fftUV2;
    float3 displacement;
    float4 projectorViewPos;
    TransformVertex(DistanceMask(v, posWorld), posWorld, normal, fftUV, fftUV2, displacement, projectorViewPos);

    half3 worldNormal = normalize(half3(normal.x, 1.0, normal.y));

    #ifdef SHADOWS_CUBE
    o.vec = posWorld.xyz - _LightPositionRange.xyz; o.pos = mul(UNITY_MATRIX_VP, posWorld);
    #else
    // Rendering into directional or spot light shadows
    #if defined(UNITY_MIGHT_NOT_HAVE_DEPTH_TEXTURE)
    o.pos = UnityClipSpaceShadowCasterPosWS(posWorld.xyz, worldNormal);
    o.pos = UnityApplyLinearShadowBias(o.pos);
    o.hpos = o.pos;
    #else
    o.pos = UnityClipSpaceShadowCasterPosWS(posWorld.xyz, worldNormal);
    o.pos = UnityApplyLinearShadowBias(o.pos);
    #endif
    #endif

    //TRANSFER_SHADOW_CASTER_NOPOS(o,o.pos)
    #if defined(UNITY_STANDARD_USE_SHADOW_UVS)
    o.tex = TRANSFORM_TEX(v.uv0, _MainTex);
    #endif

    return o;
}

half4 fragShadowCaster(
    VertexOutputShadowCaster i
    #ifdef UNITY_STANDARD_USE_DITHER_MASK
    , UNITY_VPOS_TYPE vpos : VPOS
    #endif
) : SV_Target
{
    #if defined(UNITY_STANDARD_USE_SHADOW_UVS)
        half alpha = tex2D(_MainTex, i.tex).a * _Color.a;
        #if defined(_ALPHATEST_ON)
            clip(alpha - _Cutoff);
        #endif
        #if defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
            #if defined(UNITY_STANDARD_USE_DITHER_MASK)
            // Use dither mask for alpha blended shadows, based on pixel position xy
            // and alpha level. Our dither texture is 4x4x16.
            half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
            clip(alphaRef - 0.01);
        #else
            clip(alpha - _Cutoff);
        #endif
    #endif
#endif // #if defined(UNITY_STANDARD_USE_SHADOW_UVS)

SHADOW_CASTER_FRAGMENT(i)
}

VertexOutputDepth vertDepth(VertexInput v)
{
    VertexOutputDepth o;

    float4 posWorld = GET_WORLD_POS(v.vertex);

    half2 normal;
    float4 fftUV;
    float4 fftUV2;
    float3 displacement;
    float4 projectorViewPos;
    TransformVertex(DistanceMask(v, posWorld), posWorld, normal, fftUV, fftUV2, displacement, projectorViewPos);

    o.pos = mul(UNITY_MATRIX_VP, posWorld);
    o.depth = o.pos.zw;

    return o;
}

half4 fragDepth(VertexOutputDepth i) : SV_Target
{
    UNITY_OUTPUT_DEPTH(i.depth);
//return i.depth.x / i.depth.y;
}

float4x4 _NonJitteredVP;
float4x4 _PreviousVP;
float4x4 _PreviousM;
bool _HasLastPositionData;
float _MotionVectorDepthBias;

struct MotionVectorData
{
    float4 transferPos : TEXCOORD0;
    float4 transferPosOld : TEXCOORD1;
    float4 pos : SV_POSITION;
};

struct MotionVertexInput
{
    float4 vertex : POSITION;
    float3 oldPos : NORMAL;
};

MotionVectorData VertMotionVectors(VertexInput v)
{
    MotionVectorData o;
    o.pos = UnityObjectToClipPos(v.vertex);

    // this works around an issue with dynamic batching
    // potentially remove in 5.4 when we use instancing
    #if defined(UNITY_REVERSED_Z)
    o.pos.z -= _MotionVectorDepthBias * o.pos.w;
    #else
    o.pos.z += _MotionVectorDepthBias * o.pos.w;
    #endif
    o.transferPos = mul(_NonJitteredVP, mul(unity_ObjectToWorld, v.vertex));
    o.transferPosOld = mul(_PreviousVP, mul(_PreviousM, _HasLastPositionData ? float4(v.normal, 1) : v.vertex));
    return o;
}

half4 FragMotionVectors(MotionVectorData i) : SV_Target
{
    float3 hPos = (i.transferPos.xyz / i.transferPos.w);
    float3 hPosOld = (i.transferPosOld.xyz / i.transferPosOld.w);

    // V is the viewport position at this pixel in the range 0 to 1.
    float2 vPos = (hPos.xy + 1.0f) / 2.0f;
    float2 vPosOld = (hPosOld.xy + 1.0f) / 2.0f;

#if UNITY_UV_STARTS_AT_TOP
    vPos.y = 1.0 - vPos.y;
    vPosOld.y = 1.0 - vPosOld.y;
#endif
    half2 uvDiff = vPos - vPosOld;
    return half4(uvDiff, 0, 1);
}

#endif // UNITY_STANDARD_SHADOW_INCLUDED