#ifndef UNITY_STANDARD_INPUT_INCLUDED
#define UNITY_STANDARD_INPUT_INCLUDED

#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityStandardConfig.cginc"
#include "UnityPBSLighting.cginc" // TBD: remove
#include "UnityStandardUtils.cginc"

//---------------------------------------
// Directional lightmaps & Parallax require tangent space too
#if (_NORMALMAP || !DIRLIGHTMAP_OFF || _PARALLAXMAP)
#define _TANGENT_TO_WORLD 1
#endif

#if (_DETAIL_MULX2 || _DETAIL_MUL || _DETAIL_ADD || _DETAIL_LERP)
#define _DETAIL 1
#endif

//---------------------------------------
half4		_Color;
half		_Cutoff;

sampler2D	_MainTex;
float4		_MainTex_ST;

sampler2D	_DetailAlbedoMap;
float4		_DetailAlbedoMap_ST;

sampler2D	_BumpMap;
half		_BumpScale;

sampler2D	_SubtractiveMask;
sampler2D	_UnderwaterMask;

// smooth particles
sampler2D_float _CameraDepthTexture;
float _InvFade;

sampler2D	_DetailMask;
sampler2D	_DetailNormalMap;
half		_DetailNormalMapScale;

sampler2D	_SpecGlossMap;
sampler2D	_MetallicGlossMap;
half		_Metallic;
half		_Glossiness;

sampler2D	_OcclusionMap;
half		_OcclusionStrength;

sampler2D	_ParallaxMap;
half		_Parallax;
half		_UVSec;

half4 		_EmissionColor;
sampler2D	_EmissionMap;

//-------------------------------------------------------------------------------------
// Input functions

struct VertexInput
{
    float4 vertex	: POSITION;
    half3 normal	: NORMAL;
    float2 uv0		: TEXCOORD0;
    float2 uv1		: TEXCOORD1;
    float2 particleData	: TEXCOORD2;
    #ifdef _TANGENT_TO_WORLD
    half4 tangent	: TANGENT;
    #endif
};

float4 TexCoords(VertexInput v)
{
    float4 texcoord;
    texcoord.xy = TRANSFORM_TEX(v.uv0, _MainTex); // Always source from uv0
    texcoord.zw = TRANSFORM_TEX(v.uv1, _DetailAlbedoMap);
    return texcoord;
}

half DetailMask(float2 uv)
{
    return tex2D(_DetailMask, uv).a;
}

half3 Albedo(float4 texcoords, float blend)
{
    half3 albedo = tex2D(_MainTex, texcoords.xy).rgb;
    half3 albedo2 = tex2D(_MainTex, texcoords.zw).rgb;

    return _Color.rgb * lerp(albedo, albedo2, blend);
}

half Alpha(float4 uv, float blend)
{
    half alpha = tex2D(_MainTex, uv.xy).a;
    half alpha2 = tex2D(_MainTex, uv.zw).a;

    return _Color.a * lerp(alpha, alpha2, blend);
}

half Occlusion(float2 uv)
{
    #if (SHADER_TARGET < 30)
    // SM20: instruction count limitation
    // SM20: simpler occlusion
    return tex2D(_OcclusionMap, uv).g;
    #else
    half occ = tex2D(_OcclusionMap, uv).g;
    return LerpOneTo(occ, _OcclusionStrength);
    #endif
}

half4 SpecularGloss(float2 uv)
{
    half4 sg;
    #ifdef _SPECGLOSSMAP
    sg = tex2D(_SpecGlossMap, uv.xy);
    #else
    sg = half4(_SpecColor.rgb, _Glossiness);
    #endif
    return sg;
}

half2 MetallicGloss(float2 uv)
{
    half2 mg;
    #ifdef _METALLICGLOSSMAP
    mg = tex2D(_MetallicGlossMap, uv.xy).ra;
    #else
    mg = half2(_Metallic, _Glossiness);
    #endif
    return mg;
}

half3 Emission(float2 uv)
{
    #ifndef _EMISSION
    return 0;
    #else
    return tex2D(_EmissionMap, uv).rgb * _EmissionColor.rgb;
    #endif
}

#ifdef _NORMALMAP
half3 NormalInTangentSpace(float4 texcoords, float blend)
{
    half3 normalTangent = UnpackScaleNormal(tex2D(_BumpMap, texcoords.xy), _BumpScale);
    half3 normalTangent2 = UnpackScaleNormal(tex2D(_BumpMap, texcoords.zw), _BumpScale);
    return lerp(normalTangent, normalTangent2, blend);
}
#endif

float4 Parallax(float4 texcoords, half3 viewDir)
{
    #if !defined(_PARALLAXMAP) || (SHADER_TARGET < 30)
    // SM20: instruction count limitation
    // SM20: no parallax
    return texcoords;
    #else
    half h = tex2D(_ParallaxMap, texcoords.xy).g;
    float2 offset = ParallaxOffset1Step(h, _Parallax, viewDir);
    return float4(texcoords.xy + offset, texcoords.zw + offset);
    #endif
}

#endif // UNITY_STANDARD_INPUT_INCLUDED