// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#include "../Utility/NoiseLib.cginc"

#ifndef _WAVES_GERSTNER_COUNT
#if SHADER_TARGET >= 50
#define _WAVES_GERSTNER_COUNT 20
#elif SHADER_TARGET == 30
#define _WAVES_GERSTNER_COUNT 20
#else
#define _WAVES_GERSTNER_COUNT 12
#endif
#endif

sampler2D	_GlobalDisplacementMap;
sampler2D	_GlobalDisplacementMap1;
sampler2D	_GlobalDisplacementMap2;
sampler2D	_GlobalDisplacementMap3;
float		_DisplacementsScale;

sampler2D	_LocalDisplacementMap;
sampler2D	_LocalNormalMap;
sampler2D	_DisplacementsMask;

float4		_LocalMapsCoords;
float		_DetailFadeFactor;
float4		_WaterTileSize;
float4		_WaterTileSizeInv;
float4		_WaterTileOffsets;
half3		_WaterTileSizeScales;
float4		_SurfaceOffset;
float4x4	_WaterProjectorVP;

half2		_GerstnerOrigin;
half4		_GrAmp[5];
half4		_GrFrq[5];
half4		_GrOff[5];
half4		_GrAB[5];
half4		_GrCD[5];

#ifndef _VERTICAL_OFFSET
#ifdef _DISPLACED_VOLUME
#define _VERTICAL_OFFSET _SurfaceOffset.y
#else
#define _VERTICAL_OFFSET unity_ObjectToWorld[1].w
#endif
#endif

inline void Gerstner(float2 vertex, half4 amplitudes, half4 k, half4 offset, half4 dirAB, half4 dirCD, half t, inout half3 displacement, inout half2 normal)
{
    half4 dp = k.xyzw * half4(dot(dirAB.xy, vertex), dot(dirAB.zw, vertex), dot(dirCD.xy, vertex), dot(dirCD.zw, vertex));

    half4 c, s;
    sincos(dp + offset, s, c);

    // vertical displacement
    displacement.y += dot(s, amplitudes);

    // horizontal displacement
    half4 ab = amplitudes.xxyy * dirAB.xyzw;
    half4 cd = amplitudes.zzww * dirCD.xyzw;
    displacement.x += dot(c, half4(ab.xz, cd.xz));
    displacement.z += dot(c, half4(ab.yw, cd.yw));

    // normal
    ab *= k.xxyy;
    cd *= k.zzww;

    normal.xy += half2(
        dot(c, half4(ab.xz, cd.xz)),
        dot(c, half4(ab.yw, cd.yw))
        );
}

inline half4 ComputeDistanceMask(float3 posWorld)
{
    #if SHADER_TARGET >= 30
    half3 w = (length(_WorldSpaceCameraPos.xyz - posWorld.xyz) * _WaterTileSizeInv.xyz - 1.0) * _DetailFadeFactor;
    return half4(1.0 - saturate(w), 1.0);
    #else
    return 1;
    #endif
}

#define DistanceMask(input, posWorld) ComputeDistanceMask(posWorld)

inline half ApproximateVerticalDisplacement(float4 fftUV, float4 fftUV2, int iterations = 3)
{
    half2 totalDisplacement = 0;

    for(int i = 0; i < iterations; ++i)
    {
        #if !defined(_WATER_OVERLAYS) || SHADER_TARGET >= 40
        half4 displacement = half4(tex2Dlod(_GlobalDisplacementMap, half4(fftUV.xy, 0.0, 0.0)).xz, tex2Dlod(_GlobalDisplacementMap1, half4(fftUV.zw, 0.0, 0.0)).xz);
        half4 displacement2 = half4(tex2Dlod(_GlobalDisplacementMap2, half4(fftUV2.xy, 0.0, 0.0)).xz, tex2Dlod(_GlobalDisplacementMap3, half4(fftUV2.zw, 0.0, 0.0)).xz);
        displacement += displacement2;
        #else
        half4 displacement = half4(tex2Dlod(_GlobalDisplacementMap, half4(fftUV.xy, 0.0, 0.0)).xz, tex2Dlod(_GlobalDisplacementMap1, half4(fftUV.zw, 0.0, 0.0)).xz);
        #endif

        totalDisplacement = displacement.xy + displacement.zw - totalDisplacement;

        fftUV -= totalDisplacement.xyxy * _WaterTileSizeInv.xxyy;
        fftUV2 -= totalDisplacement.xyxy * _WaterTileSizeInv.zzww;
    }

    #if !defined(_WATER_OVERLAYS) || SHADER_TARGET >= 40
    half4 d = half4(tex2Dlod(_GlobalDisplacementMap, half4(fftUV.xy, 0.0, 0.0)).y, tex2Dlod(_GlobalDisplacementMap1, half4(fftUV.zw, 0.0, 0.0)).y, tex2Dlod(_GlobalDisplacementMap2, half4(fftUV2.xy, 0.0, 0.0)).y, tex2Dlod(_GlobalDisplacementMap3, half4(fftUV2.zw, 0.0, 0.0)).y);
    #else
    half4 d = half4(tex2Dlod(_GlobalDisplacementMap, half4(fftUV.xy, 0.0, 0.0)).y, tex2Dlod(_GlobalDisplacementMap1, half4(fftUV.zw, 0.0, 0.0)).y, 0.0, 0.0);
    #endif

    return dot(d, 1);
}

inline half ApproximateVerticalDisplacement(float2 worldPos)
{
    return ApproximateVerticalDisplacement(worldPos.xyxy * _WaterTileSizeInv.xxyy, worldPos.xyxy * _WaterTileSizeInv.zzww);
}

inline half4 GetOcclusionDir(half3 partialDir)
{
    return half4(partialDir.xyz, 1.0 - dot(partialDir, half3(1, 1, 1)));
}

inline half4 approxTanh(half4 x)
{
    return x / sqrt(1.0 + x * x);
}

inline void TransformVertex(half4 mask, inout float4 posWorld, out half2 normal, out float4 fftUV, out float4 fftUV2, out float3 totalDisplacement, out float4 projectorViewPos, bool useMask = true)
{
    projectorViewPos = ComputeNonStereoScreenPos(mul(_WaterProjectorVP, posWorld));

    float2 samplePos = posWorld.xz + _SurfaceOffset.xz;

    totalDisplacement = float3(0, 0, 0);
    normal = half2(0, 0);

    #if _WAVES_GERSTNER && !_DISPLACED_VOLUME			// putting it there solves temporary registers shortage problem on SM 2.0
    float2 samplePosGerstner = -samplePos;

    for(int i = 0; i < (_WAVES_GERSTNER_COUNT / 4); ++i)
        Gerstner(samplePosGerstner, _GrAmp[i], _GrFrq[i], _GrOff[i], _GrAB[i], _GrCD[i], _Time.y, /*out*/ totalDisplacement, /*out*/ normal);

    totalDisplacement.xz *= -_DisplacementsScale;
    #endif

    fftUV = samplePos.xyxy * _WaterTileSizeInv.xxyy;
    fftUV2 = samplePos.xyxy * _WaterTileSizeInv.zzww;

    #if _DISPLACED_VOLUME
    return;
    #endif

    #if _WAVES_FFT
    float3 lod = 1.0 / mask.xyz - 1.0;

    #if defined(_WATER_OVERLAYS)
    half4 shoreDisplacementMask = tex2Dlod(_DisplacementsMask, half4(projectorViewPos.xy / projectorViewPos.w, 0, 0));
    mask *= shoreDisplacementMask;
    #endif

    if(!useMask)
        mask = 1.0 - pow(1.0 - mask, 3);

    #if defined(_FORCE_FULL_DISPLACEMENT)
    lod = 0.0;
    mask = 1.0;
    #endif

    float3 displacement = 0;
    UNITY_BRANCH
        if(mask.x >= 0.05)
            displacement = tex2Dlod(_GlobalDisplacementMap, float4(fftUV.xy, 0, lod.x)).xyz * mask.x;

    #if !defined(_WATER_OVERLAYS) || SHADER_TARGET >= 40 || !defined(_WAVES_ALIGN)
    UNITY_BRANCH
        if(mask.y >= 0.05)
            displacement += tex2Dlod(_GlobalDisplacementMap1, float4(fftUV.zw, 0, lod.y)).xyz * mask.y;
    #endif

    #if (!defined(_WATER_OVERLAYS) && !defined(_WAVES_ALIGN)) || SHADER_TARGET >= 40
    displacement += tex2Dlod(_GlobalDisplacementMap2, float4(fftUV2.xy, 0, lod.z)).xyz * mask.z;
    displacement += tex2Dlod(_GlobalDisplacementMap3, float4(fftUV2.zw, 0, 0)).xyz * mask.w;
    #endif

    #if _WAVES_ALIGN
    displacement = float3(0.0, ApproximateVerticalDisplacement(fftUV, fftUV2), 0.0);
    #endif

    totalDisplacement += displacement;
    #endif

    totalDisplacement *= _SurfaceOffset.w;

    #if defined(_WATER_OVERLAYS)
    totalDisplacement += tex2Dlod(_LocalDisplacementMap, half4(projectorViewPos.xy / projectorViewPos.w, 0, 0)) * half3(_DisplacementsScale, 1.0, _DisplacementsScale);
    #endif

    posWorld.xyz += totalDisplacement;

    // experimentally compensate for earth's curvature (causes major precision issues)
    //float _PlanetRadius = 6371000.0;
    //float4 cosxz = cos(float4(posWorld.xz / _PlanetRadius, posWorld.xz * 0.5 / _PlanetRadius));
    //posWorld.xz *= cosxz.z * cosxz.w;
    //posWorld.y += (cosxz.x * cosxz.y - 1.0) * _PlanetRadius;
}

inline half3 GetWaterDisplacement(float2 samplePos)
{
    samplePos += _SurfaceOffset.xz;

    half3 displacement = half3(0.0, 0.0, 0.0);

    #if _WAVES_FFT
    float4 fftUV = samplePos.xyxy * _WaterTileSizeInv.xxyy;
    float4 fftUV2 = samplePos.xyxy * _WaterTileSizeInv.zzww;
    half4 mask = tex2D(_DisplacementsMask, samplePos * _LocalMapsCoords.zz + _LocalMapsCoords.xy);

    displacement = tex2D(_GlobalDisplacementMap, fftUV.xy).xyz * mask.x;
    displacement += tex2D(_GlobalDisplacementMap1, fftUV.zw).xyz * mask.y;
    #if !defined(_WATER_OVERLAYS) || SHADER_TARGET >= 40
    displacement += tex2D(_GlobalDisplacementMap2, fftUV2.xy).xyz * mask.z;
    displacement += tex2D(_GlobalDisplacementMap3, fftUV2.zw).xyz * mask.w;
    #endif
    #endif

    half2 normal = half2(0.0, 0.0);			// will be compiled out

    #if _WAVES_GERSTNER
    for(int i = 0; i < (_WAVES_GERSTNER_COUNT / 4); ++i)
        Gerstner(-samplePos, _GrAmp[i], _GrFrq[i], _GrOff[i], _GrAB[i], _GrCD[i], _Time.y, /*out*/ displacement, /*out*/ normal);
    #endif

    return displacement;
}