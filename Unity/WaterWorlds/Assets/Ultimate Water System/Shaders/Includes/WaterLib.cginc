#ifndef WATERLIB_INCLUDED
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
#define WATERLIB_INCLUDED

// NOTE: comment this out if you want non-image effect fog on water
#undef FOG_LINEAR
#undef FOG_EXP
#undef FOG_EXP2

#if SHADER_TARGET <= 30
#undef _INCLUDE_SLOPE_VARIANCE
#endif

#if SHADER_TARGET <= 20
#undef _WAVES_FFT
#undef _WATER_OVERLAYS
#undef _INCLUDE_SLOPE_VARIANCE
#undef _PROJECTION_GRID
#undef _WATER_REFRACTION
#undef _ALPHABLEND_ON
#undef _ALPHAPREMULTIPLY_ON
#undef _CUBEMAP_REFLECTIONS
#undef _NORMALMAP
#undef _WATER_FOAM_WS
#undef _WATER_RECEIVE_SHADOWS
#endif

#if defined(_DISPLACED_VOLUME) && !defined(_CLIP_ABOVE)
#define _CLIP_ABOVE 1
#endif

#if defined(_WAVES_FFT) && !defined(_DISPLACED_VOLUME)
#define _WAVES_FFT_NORMAL 1
#endif

#if defined(DEFERRED)
#undef _ALPHABLEND_ON
#undef _WATER_REFRACTION
#undef _WATER_RECEIVE_SHADOWS
#endif

#if defined(TESS_OUTPUT)
#define TESSELATION 1
#endif

#include "../Includes/UnityLightingCommon.cginc"
#include "../Includes/UnityStandardUtils.cginc"
#include "../Includes/WaterDisplace.cginc"

// use differently named refraction tex for water volumes to include previously rendered regular water surfaces in it
#if !_DISPLACED_VOLUME
sampler2D	_RefractionTex;
half2 _RefractionTex_TexelSize;
#if !defined(REFRACTION_TEX)
#define REFRACTION_TEX _RefractionTex
#endif
#else
sampler2D	_RefractionTex2;
half2 _RefractionTex2_TexelSize;
#if !defined(REFRACTION_TEX)
#define REFRACTION_TEX _RefractionTex2
#endif
#endif

CBUFFER_START(Water)
half4		_PlanarReflectionPack;

half3		_AbsorptionColor;
half3		_DepthColor;

half		_RefractionDistortion;
half		_RefractionMaxDepth;

half		_DisplacementNormalsIntensity;

half4		_SubsurfaceScatteringShoreColor;
half4		_WrapSubsurfaceScatteringPack;

half		_EdgeBlendFactorInv;
half4		_FoamParameters;
half		_FoamShoreIntensity;
half2		_FoamTiling;

half3		_ReflectionColor;

half		_FoamNormalScale;
half3		_FoamDiffuseColor;
half4		_FoamSpecularColor;
half		_MaxDisplacement;
half		_LightSmoothnessMul;
half		_UnderwaterLightFadeScale;
half		_Cull;

half		_PlanarReflectionMipBias;
float3		_WaterId;

#if defined(_PROJECTION_GRID)
float4x4	_InvViewMatrix;
#endif

CBUFFER_END

sampler2D	_PlanarReflectionTex;
sampler2D	_GlobalNormalMap;
sampler2D	_GlobalNormalMap1;
sampler2D	_SubtractiveMask;
sampler2D	_AdditiveMask;
sampler3D	_SlopeVariance;
sampler2D	_FoamTex;
sampler2D	_FoamMap;
sampler2D	_FoamNormalMap;
sampler2D	_LocalDebugMap;
sampler2D	_SubsurfaceScattering;
sampler2D	_UnderwaterMask;
sampler2D	_TotalDisplacementMap;
sampler2D	_UnderwaterAbsorptionGradient;
sampler2D   _LocalDiffuseMap;
float2      _LocalDiffuseMap_TexelSize;

sampler2D_float _CameraDepthTexture2;
sampler2D_float _WaterlessDepthTexture;

// I've isolated most of the water data here to not touch Standard shader code structure too much
struct WaterData
{
	half depth;
	half sceneDepth;
	half4 mask;
	half4 totalMask;
	half4 shoreMask4;
	half2 fftUV;
	half2 fftUV2;
	half4 projectorViewPos;
	half4 refractedScreenPos;
	half shoreMask;
};

static WaterData globalWaterData;

#if !defined(UNITY_BRDF_PBS)
#if (SHADER_TARGET < 30) || defined(SHADER_API_PSP2)
#define UNITY_BRDF_PBS BRDF3_Unity_PBS_Water
#elif defined(SHADER_API_MOBILE)
#define UNITY_BRDF_PBS BRDF2_Unity_PBS_Water
#else
#define UNITY_BRDF_PBS BRDF1_Unity_PBS_Water
#endif
#endif

#define LOCAL_MAPS_UV i.pack0.zw

#define WATER_SETUP_PRE(i, s) WaterFragmentSetupPre(i.pack0.xy, i.pack1.zw, posWorld, i.screenPos, i.projectorViewPos);
#define WATER_SETUP_ADD_1(i, s) WaterFragmentSetupPre(i.pack0.xy, i.pack1.zw, posWorld, i.screenPos, i.projectorViewPos);

#ifndef _OBJECT2WORLD
#define _OBJECT2WORLD unity_ObjectToWorld
#endif

#if _PROJECTION_GRID
#ifdef TESS_OUTPUT
#define GET_WORLD_POS(i) i
#else
#define GET_WORLD_POS(i) float4(GetProjectedPosition(i.xy), 1)
#endif
#else
#ifdef TESS_OUTPUT
#define GET_WORLD_POS(i) i
#else
#define GET_WORLD_POS(i) mul(_OBJECT2WORLD, i)
#endif
#endif

// Unity doesn't support shadows for transparent objects, so these macros have to be redefined here
#if _WATER_RECEIVE_SHADOWS
sampler2D _WaterShadowmap;
#define WATER_SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
#define WATER_TRANSFER_SHADOW(a) a._ShadowCoord = ComputeScreenPos(a.pos);
#define WATER_SHADOW_ATTENUATION(a) ShadowAttenuation(a._ShadowCoord)

half ShadowAttenuation(half4 shadowCoord)
{
	fixed shadow = tex2Dproj(_WaterShadowmap, UNITY_PROJ_COORD(shadowCoord)).r;
	return shadow;
}
#else
#define WATER_SHADOW_COORDS(idx1)
#define WATER_TRANSFER_SHADOW(a)
#define WATER_SHADOW_ATTENUATION(a) 1.0
#endif

inline half4 ComputeDistortOffset(half3 normalWorld, half distort)
{
	return half4(normalWorld.xz * distort, 0, 0);
}

inline half LinearEyeDepthHalf(half z)
{
	return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
}

// I've heard that some compilers for some platforms (PS3) don't do this automatically for pow(x, 2)
half  Pow2(half  x) { return x * x; }
half2 Pow2(half2 x) { return x * x; }
half3 Pow2(half3 x) { return x * x; }
half4 Pow2(half4 x) { return x * x; }

// most of the water-specific data used around the shader is stored in a global struct to make future updates to the standard shader easier
inline void WaterFragmentSetupPre(half2 fftUV, half2 fftUV2, float3 worldPos, half4 screenPos, half4 projectorViewPos)
{
	globalWaterData.fftUV = fftUV;
	globalWaterData.fftUV2 = fftUV2;
	globalWaterData.depth = LinearEyeDepthHalf(screenPos.z / screenPos.w);
	globalWaterData.sceneDepth = LinearEyeDepthHalf(SAMPLE_DEPTH_TEXTURE_PROJ(_WaterlessDepthTexture, UNITY_PROJ_COORD(screenPos)));
	globalWaterData.projectorViewPos = projectorViewPos;

	globalWaterData.totalMask = 1;
	//ComputeDistanceMask(worldPos, globalWaterData.mask, globalWaterData.totalMask);

#if _WATER_OVERLAYS
	globalWaterData.shoreMask4 = tex2Dproj(_DisplacementsMask, projectorViewPos);
	globalWaterData.totalMask = globalWaterData.shoreMask4;
	globalWaterData.shoreMask4 = Pow2(max(0.0, sin((globalWaterData.shoreMask4 - 0.1) * 3.5))) * 0.5;
#endif

	globalWaterData.shoreMask = globalWaterData.totalMask.x;
}

inline void WaterFragmentSetupPost(half3 normalWorld, half4 screenPos)
{
	half offset = ComputeDistortOffset(normalWorld, _RefractionDistortion);

#if SHADER_TARGET >= 30
	half centerSceneDepth = LinearEyeDepthHalf(SAMPLE_DEPTH_TEXTURE_PROJ(_WaterlessDepthTexture, screenPos + offset).r) - globalWaterData.depth;
	half distortScale = saturate(centerSceneDepth);
#else
	half distortScale = 1.0;
#endif

#ifndef _DISPLACED_VOLUME
	globalWaterData.refractedScreenPos = UNITY_PROJ_COORD(screenPos + offset * distortScale);
#else
	globalWaterData.refractedScreenPos = UNITY_PROJ_COORD(screenPos);
#endif
}

half2 UnpackScaleNormal2(half4 packednormal, half bumpScale)
{
#if defined(UNITY_NO_DXT5nm)
	return packednormal.xy * 2 - 1;
#else
	half2 normal;
	normal.xy = (packednormal.wy * 2 - 1);
#if (SHADER_TARGET >= 30)
	// SM2.0: instruction count limitation
	// SM2.0: normal scaler is not supported
	normal.xy *= bumpScale;
#endif
	return normal;
#endif
}

inline void AddFoam(half4 i_tex, inout half3 specColor, inout half smoothness, inout half3 albedo, inout half refractivity, inout half3 normalWorld)
{
#if _WATER_OVERLAYS || _WATER_FOAM_WS
	half foamIntensity = 0.0;

#if _WATER_FOAM_WS
	half4 uv1 = globalWaterData.fftUV.xyxy * _WaterTileSizeScales.yyzz;

	half4 foamIntensities;
	foamIntensities.x = tex2D(_FoamMap, globalWaterData.fftUV).x;
	foamIntensities.y = tex2D(_FoamMap, globalWaterData.fftUV2).y;
	foamIntensities.z = tex2D(_FoamMap, uv1.xy).z;
	foamIntensities.w = tex2D(_FoamMap, uv1.zw).w;

	foamIntensity += dot(foamIntensities, 1);
#endif

#if _WATER_FOAM_WS && _WATER_OVERLAYS				// currently it's not possible to have both modes (probably there is no reason)
	foamIntensity *= globalWaterData.shoreMask;
#endif

#if _WATER_OVERLAYS
	half2 dynamicFoam = tex2Dproj(_FoamMap, globalWaterData.projectorViewPos);
	foamIntensity += dynamicFoam.x;
#endif

	// 1.2 - changes
	//foamIntensity = tanh(foamIntensity);
	//foamIntensity = 1.0 - exp(-0.05 * foamIntensity);
	half2 foamUV = i_tex.xy * _FoamTiling;

#if _WATER_OVERLAYS
	//foamIntensity += _FoamShoreIntensity * max(0, (1.0 - globalWaterData.shoreMask * _FoamParameters.z));
#endif

// sharp shoreline foam
	float shoreIntensity = _FoamShoreIntensity * ((1.0f + _FoamParameters.z) / pow(1.0f + max(globalWaterData.sceneDepth - globalWaterData.depth, 0.0f), 4.0f));

#if SHADER_TARGET >= 40		// make foam less tileable on modern platforms
	half3 foam = half3(tex2D(_FoamTex, foamUV).x, tex2D(_FoamTex, foamUV * 0.7731).y, 0.9);			// tex2D(_FoamTex, foamUV * 0.437).z
#else
	half3 foam = tex2D(_FoamTex, foamUV);
#endif

	//foam = lerp(half3(0.05, 0.36, 0.7), foam, globalWaterData.totalMask.yxz);			// do this later by blurring mip maps manually and exporting them to dds

	half3 channelIntensities = saturate((foamIntensity + half3(0.0, -0.25, -0.8)) / half3(0.3, 0.55, 0.2));
	foamIntensity *= saturate(dot(foam, channelIntensities));
	foamIntensity += shoreIntensity * foam;

#if _WATER_OVERLAYS
	foamIntensity += dynamicFoam.y;
	foamIntensity = saturate(foamIntensity);
#endif

	albedo = lerp(albedo, _FoamDiffuseColor, foamIntensity);
	specColor = lerp(specColor, _FoamSpecularColor.rgb, foamIntensity);
	smoothness = lerp(smoothness, _FoamSpecularColor.a, foamIntensity);

	float2 uv = globalWaterData.projectorViewPos.xy /= globalWaterData.projectorViewPos.w;

#if UNITY_UV_STARTS_AT_TOP
	if (uv.y < 0)
		uv.y = 1 - uv.y;
#endif

	half4 diffuse = tex2D(_LocalDiffuseMap, uv);
	albedo += diffuse.rgb;
	refractivity *= 1.0f - saturate(length(diffuse.rgb));

	half2 foamNormal = UnpackScaleNormal2(tex2D(_FoamNormalMap, foamUV), foamIntensity * _FoamNormalScale);
	normalWorld = normalize(normalWorld + half3(foamNormal.x, 0, foamNormal.y));

	refractivity *= 1.0 - foamIntensity;
#endif
}

//
// Derived from the paper and accompanying implementation:
// "Real-time Realistic Ocean Lighting using
// Seamless Transitions from Geometry to BRDF"
// Eric Bruneton, Fabrice Neyret, Nicolas Holzschuch
//
inline void ApplySlopeVariance(float3 posWorld, inout float oneMinusRoughness, out half2 dirRoughness)
{
	half Jxx = ddx(posWorld.x);
	half Jxy = ddy(posWorld.x);
	half Jyx = ddx(posWorld.z);
	half Jyy = ddy(posWorld.z);
	half A = Jxx * Jxx + Jyx * Jyx;
	half B = Jxx * Jxy + Jyx * Jyy;
	half C = Jxy * Jxy + Jyy * Jyy;
	half SCALE = 10.0;
	half ua = pow(A / SCALE, 0.25);
	half ub = 0.5 + 0.5 * B / sqrt(A * C);
	half uc = pow(C / SCALE, 0.25);
	half2 sigmaSq = tex3D(_SlopeVariance, half3(ua, ub, uc)).xy;

	half shoreMask = lerp(globalWaterData.totalMask.x, 1.0, 0.25);
	dirRoughness = 1.0 - oneMinusRoughness * (1.0 - sigmaSq * shoreMask);
	oneMinusRoughness = oneMinusRoughness * (1.0 - length(sigmaSq) * shoreMask);
}

static const half4 _Weights[7] = { half4(0.0205,0.0205,0.0205,0.0205), half4(0.0855,0.0855,0.0855,0.0855), half4(0.232,0.232,0.232,0.232), half4(0.324,0.324,0.324,0.324), half4(0.232,0.232,0.232,0.232), half4(0.0855,0.0855,0.0855,0.0855), half4(0.0205,0.0205,0.0205,0.0205) };
half4x4 _PlanarReflectionProj;

inline half4 SamplePlanarReflectionHq(sampler2D tex, half4 screenPos, half roughness, half2 dirRoughness)
{
	half4 color = 0;

#if UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2
	dirRoughness = pow(dirRoughness, 3.0 / 4.0);
#endif

	half mip = _PlanarReflectionMipBias + min(dirRoughness.x, dirRoughness.y) * 7;
	half2 step = (clamp(dirRoughness.xy / dirRoughness.yx, 1, 1.006) - 1.0) * 1.5;

	half4 uv = half4(screenPos.xy / screenPos.w, 0, mip);
	uv.xy -= 3 * step;

	for (int i = 0; i < 7; ++i)
	{
#if SHADER_TARGET >= 30
		color += tex2Dlod(tex, uv) * _Weights[i];
#else
		color += tex2D(tex, uv.xy) * _Weights[i];
#endif

		uv.xy += step;
	}

	return color;
}

inline half4 SamplePlanarReflectionSimple(sampler2D tex, half4 screenPos, half roughness, half2 dirRoughness)
{
	half4 color;

#if SHADER_TARGET >= 30
#if UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2
	roughness = pow(roughness, 3.0 / 4.0);
#endif

	half mip = _PlanarReflectionMipBias + roughness * 7;
	half4 uv = half4(screenPos.xy / screenPos.w, 0, mip);
	color = tex2Dlod(tex, uv);
#else
	color = tex2Dproj(tex, UNITY_PROJ_COORD(screenPos));
#endif

	return color;
}

#if _PLANAR_REFLECTIONS_HQ && SHADER_TARGET >= 40
#define SamplePlanarReflection SamplePlanarReflectionHq
#else
#define SamplePlanarReflection SamplePlanarReflectionSimple
#endif

// Used by 'UnityGlobalIllumination' in 'UnityGlobalIllumination.cginc'
inline void PlanarReflection(inout UnityGI gi, half4 screenPos, half roughness, half2 dirRoughness, half3 worldNormal)
{
#if SHADER_API_GLES || SHADER_API_OPENGL || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_PSSL || SHADER_API_PS3 || SHADER_API_PSP2 || SHADER_API_PSM
	screenPos = mul(_PlanarReflectionProj, half4(worldNormal, 0));
#else
	screenPos = mul((half4x3)_PlanarReflectionProj, worldNormal);
#endif

	screenPos.y += (worldNormal.y - 1.0) * _PlanarReflectionPack.z;

#if _CUBEMAP_REFLECTIONS && (_PLANAR_REFLECTIONS || _PLANAR_REFLECTIONS_HQ)

	half4 planarReflection = SamplePlanarReflection(_PlanarReflectionTex, screenPos, roughness, dirRoughness);
	gi.indirect.specular.rgb = lerp(gi.indirect.specular.rgb, planarReflection.rgb, _PlanarReflectionPack.x * planarReflection.a);

#elif _PLANAR_REFLECTIONS || _PLANAR_REFLECTIONS_HQ

	half4 planarReflection = SamplePlanarReflection(_PlanarReflectionTex, screenPos, roughness, dirRoughness);
	gi.indirect.specular.rgb = planarReflection.rgb;

#endif
}

inline float random(float2 p)
{
	float2 r = float2(23.14069263277926, 2.665144142690225);
	return frac(cos(dot(p, r)) * 123.0);
}

inline void ApplyDebugColors(inout half3 c)
{
#if defined(_DEBUG_SHORES)
	c.r += pow(1.0 - tex2Dproj(_LocalNormalMap, globalWaterData.projectorViewPos).a, 2) * 0.8;
#endif

#if defined(_LOCAL_MAPS_DEBUG)
	float particleGroupId = tex2Dproj(_LocalDebugMap, globalWaterData.projectorViewPos).r;
	half3 hash;
	hash.x = random(particleGroupId.xx);
	hash.y = random(hash.xx);
	hash.z = random(hash.yy);

	c.rgb += hash;
#endif
}

inline half BlendEdges(float4 screenPos)
{
#if defined(_ALPHABLEND_ON) || defined(DEFERRED)
	float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_WaterlessDepthTexture, UNITY_PROJ_COORD(screenPos));
	depth = LinearEyeDepth(depth);
	return saturate(_EdgeBlendFactorInv * (depth - screenPos.w));
#else
	return 1.0;
#endif
}

inline void MaskWater(out half alpha, float4 screenPos, float3 worldPos)
{
#if !defined(_DISPLACED_VOLUME) && !defined(_DEPTH) && defined(_WATER_REFRACTION)
	alpha = BlendEdges(screenPos);
#else
	alpha = 1.0;
#endif

	float depth = screenPos.z / screenPos.w;

#ifndef _CLIP_ABOVE
	float4 subMask = tex2Dproj(_SubtractiveMask, UNITY_PROJ_COORD(screenPos));

#if _DISPLACED_VOLUME
	alpha *= subMask.w;
#else

#if !defined(UNITY_REVERSED_Z)
	if (depth <= subMask.y && depth >= subMask.z && fmod(subMask.x, _WaterId.y) >= _WaterId.x)
#else
	if (depth <= subMask.z && depth >= subMask.y && fmod(subMask.x, _WaterId.y) >= _WaterId.x)
#endif
		alpha *= subMask.w;
#endif
#endif

#if _BOUNDED_WATER && !_DISPLACED_VOLUME
	float4 addMask = tex2Dproj(_AdditiveMask, UNITY_PROJ_COORD(screenPos));

#if !defined(UNITY_REVERSED_Z)
	if (depth < addMask.z || depth > addMask.y || fmod(addMask.x, _WaterId.y) < _WaterId.x)
#else
	if (depth < addMask.y || depth > addMask.z || fmod(addMask.x, _WaterId.y) < _WaterId.x)
#endif
		alpha = 0.0;
#endif

#if _CLIP_ABOVE
	alpha = step(worldPos.y, _VERTICAL_OFFSET + ApproximateVerticalDisplacement(worldPos.xz + _SurfaceOffset.xz) + 0.04);
	//alpha = step(worldPos.y, GetDisplacedHeight4(worldPos.xz + _SurfaceOffset.xz) + 0.04);			// temporarily disabled
#endif
}

inline void UnderwaterClip(half4 screenPos)
{
#if _WATER_BACK && SHADER_TARGET >= 30				// on sm 2.0, there is no support for underwater effect masking
	fixed mask = tex2Dproj(_UnderwaterMask, UNITY_PROJ_COORD(screenPos));
	clip(-0.001 + mask);
#endif
}

sampler2D _CausticsMap;
float4x4  _CausticsMapProj;
float4	  _CausticsOffsetScale;
half3	  _CausticLightDir;

inline half3 ComputeDepthColor(half3 posWorld, half3 eyeVec, half3 lightDir, half3 lightColor, half3 offset, half displacementScale, half3 totalDirectionalLightsContribution, float maxDepth = 0)
{
	half4 samplePoint = half4(posWorld + half3(offset.x, 0.0, offset.z), 1.0);
	half step = 0.75f;

	half3 depthColor = 0;
	half distance = 0;
	half fade = 1;
	half4 localUv = half4(0, 0, 0, 3);

	for (int i = 0; i < 10; ++i)
	{
		samplePoint.xyz -= eyeVec * step;
		distance += step;
		step *= 1.5f;

		localUv.xy = samplePoint.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
		half waterHeight = tex2D(_TotalDisplacementMap, localUv).y;
		half3 scattering = tex2Dlod(_SubsurfaceScattering, localUv);
		//scattering += totalDirectionalLightsContribution;
		half depth = samplePoint.y - waterHeight;

		half4 absorptionColor2 = tex2D(_UnderwaterAbsorptionGradient, depth / -600);
		scattering *= absorptionColor2.a;

		//half backFade = saturate(1.0 - (distance - maxDepth) / step);
		//half geometryScatteringInv = min(1.0, max(0.2, (maxDepth - distance) * 0.4 / step) * max(1, i * 0.25));
		//half scatteringScale = backFade / geometryScatteringInv;
		//scattering *= lerp(1.0, scatteringScale, 0.5);

		half3 gain = scattering * step * exp(absorptionColor2.rgb * (min(depth * _UnderwaterLightFadeScale, 0) - distance));
		depthColor += gain;
	}

	return depthColor;
}

inline half3 ComputeDepthColor2(half3 posWorld, half3 eyeVec, half3 lightColor, half3 offset, half displacementScale, float maxDepth = 0)
{
	half4 samplePoint = half4(posWorld + half3(offset.x, 0.0, offset.z), 1.0);
	half step = 0.5f;

	half3 depthColor = 0;
	half distance = 0;
	half fade = 1;
	half4 localUv = half4(0, 0, 0, 3);

	for (int i = 0; i < 4; ++i)
	{
		samplePoint.xyz -= eyeVec * step;
		distance += step;
		step *= 1.35f;

		localUv.xy = samplePoint.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
		half waterHeight = tex2D(_TotalDisplacementMap, localUv).y;
		half3 scattering = tex2D(_SubsurfaceScattering, localUv);
		half depth = samplePoint.y - waterHeight;

		half3 gain = scattering * step * exp(_AbsorptionColor * (min(depth, 0) - distance));
		depthColor += gain;
	}

	return depthColor;
}

inline half3 ComputeDepthColor3(half3 posWorld, half3 eyeVec, half3 lightColor, half3 absorptionColor, half3 lightDir, half3 offset, float maxDepth = 0)
{
	half4 samplePoint = half4(posWorld + half3(offset.x, 0.0, offset.z), 1.0);
	half step = 0.5f;

	half3 depthColor = 0;
	half distance = 0;
	half fade = 1;
	half4 localUv = half4(0, 0, 0, 3);

	for (int i = 0; i < 4; ++i)
	{
		samplePoint.xyz -= eyeVec * step;
		distance += step;
		step *= 1.35f;

		localUv.xy = samplePoint.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
		half waterHeight = tex2D(_TotalDisplacementMap, localUv).y;
		half depth = samplePoint.y - waterHeight;

		half3 gain = step * exp(absorptionColor * (min(depth, 0) - distance));
		depthColor += gain;
	}

	half dp = dot(lightDir, -eyeVec);

	if (dp < 0) dp *= -0.25;
	dp = 0.333333 + dp * dp * 0.666666;

	return depthColor * dp * lightColor;
}

inline half3 ComputeDepthColorv4(half3 absorptionColor, half3 eyeVec, half3 lightColor, half3 lightDir, half3 normalWorld)
{
	half dp = dot(lightDir, eyeVec);

	if (dp < 0) dp *= -0.25;
	dp = 0.14 + dp * dp * dp * 0.86;

	dp *= 1.225 + abs(dot(normalWorld, lightDir)) * 0.525;

	return exp(absorptionColor * -2.0) * dp * lightColor;
}

// used by lighting functions at the bottom of this file
inline half3 ComputeRefractionColor(half3 normalWorld, half3 eyeVec, half3 posWorld, half3 lightDir, half3 lightColor, WaterData waterData, out half3 depthFade)
{
#if defined(DEFERRED)
	depthFade = 0;
	return 0;
#else
	half waterSurfaceDepth = waterData.depth;

#if !defined(_WATER_BACK)
	half sceneDepth = LinearEyeDepthHalf(SAMPLE_DEPTH_TEXTURE_PROJ(_WaterlessDepthTexture, waterData.refractedScreenPos).r) - waterSurfaceDepth;
	depthFade = min(exp(-_AbsorptionColor * sceneDepth), 1);
#else
	depthFade = 1;
#endif

#if _WATER_REFRACTION
#if UNITY_UV_STARTS_AT_TOP
	if (_ProjectionParams.x >= 0)
		waterData.refractedScreenPos.y = waterData.refractedScreenPos.w - waterData.refractedScreenPos.y;
#endif

	//half3 depthColor = ComputeDepthColor2(posWorld, eyeVec, lightDir, lightColor, _SurfaceOffset.xyz, 1.0);
	//half3 depthColor = 0;
	return tex2Dproj(REFRACTION_TEX, waterData.refractedScreenPos).rgb * depthFade;
#else
	depthFade = 0;
	return _DepthColor;
#endif
#endif
}

#if defined(_PROJECTION_GRID)
// grid projection
float3 GetScreenRay(float2 screenPos)
{
	return mul((float3x3)_InvViewMatrix, float3(screenPos.xy, -UNITY_MATRIX_P[0].x));
}

float3 GetProjectedPosition(float2 vertex)
{
	float screenScale = 1.2;
	float focal = UNITY_MATRIX_P[0].x;
	float aspect = UNITY_MATRIX_P[1].y;

	float2 screenPos = float2((vertex.x - 0.5) * screenScale * aspect, (vertex.y - 0.5) * screenScale * focal);

	float3 ray = GetScreenRay(screenPos);

	if (ray.y == 0) ray.y = 0.001;

	float d = _WorldSpaceCameraPos.y / -ray.y;

	float3 pos;

	if (d >= 0.0)
		pos = _WorldSpaceCameraPos.xyz + ray * d;
	else
		pos = float3(_WorldSpaceCameraPos.x, 0.0, _WorldSpaceCameraPos.z) + normalize(float3(ray.x, 0.0, ray.z)) * _ProjectionParams.z * (1.0 + -1.0 / d);

	pos -= normalize(float3(_InvViewMatrix[0].x, 0, _InvViewMatrix[0].z)) * (vertex.x - 0.5) * 2 * _MaxDisplacement;
	pos -= normalize(float3(_InvViewMatrix[2].x, 0, _InvViewMatrix[2].z)) * (vertex.y - 0.5) * 2 * _MaxDisplacement;

	return pos;
}
#endif

inline half SimpleFresnel(half dp)
{
	half t = 1.0 - dp;

	return t * t;
}

#include "UnityStandardBRDF.cginc"

inline half3 WaterFresnelFast(half cosA)
{
	return exp2(-8.35 * cosA);
}

inline half3 WaterFresnelTerm(half3 F0, half cosA)
{
	return F0 + (1 - F0) * Pow5(1.0 - cosA);
}

inline half3 WaterFresnelLerp(half3 F0, half3 F90, half cosA, bool back = false)
{
	if (back)
	{
		F0 = 0.0204;
		half inv_eta = 1.333333;
		half SinT2 = inv_eta * inv_eta * (1.0 - cosA * cosA);

		if (SinT2 > 1.0f)
			return 1.0;			// total internal reflection

		cosA = sqrt(1.0f - SinT2);
	}

	return lerp(F0, F90, Pow5(1.0 - cosA));	// ala Schlick interpoliation
}

half4 BRDF1_Unity_PBS_Water(half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness, half refractivity,
	half3 normal, half3 viewDir, half3 posWorld,
	UnityLight light, UnityIndirect gi, half atten)
{
	oneMinusRoughness *= _LightSmoothnessMul;

	half roughness = 1 - oneMinusRoughness;
	half3 halfDir = normalize(light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm(normal, halfDir);
	half nv = DotClamped(normal, viewDir);
	half lv = DotClamped(light.dir, viewDir);
	half lh = DotClamped(light.dir, halfDir);

#if defined(POINT) || defined(SPOT) || defined(POINT_COOKIE)
	nl = (nl + _WrapSubsurfaceScatteringPack.z) * _WrapSubsurfaceScatteringPack.w;
#else
	nl = (nl + _WrapSubsurfaceScatteringPack.x) * _WrapSubsurfaceScatteringPack.y;
#endif

#if 0 // UNITY_BRDF_GGX - I'm not sure when it's set, but we don't want this in the case of water
	half V = SmithGGXVisibilityTerm(nl, nv, roughness);
	half D = GGXTerm(nh, roughness);
#else
	half V = SmithBeckmannVisibilityTerm(nl, nv, roughness);
	half D = NDFBlinnPhongNormalizedTerm(nh, RoughnessToSpecPower(roughness));
#endif

	half nlPow5 = Pow5(1 - nl);
	half nvPow5 = Pow5(1 - nv);
	half Fd90 = 0.5 + 2 * lh * lh * roughness;
	half disneyDiffuse = (1 + (Fd90 - 1) * nlPow5) * (1 + (Fd90 - 1) * nvPow5);

	// HACK: theoretically we should divide by Pi diffuseTerm and not multiply specularTerm!
	// BUT 1) that will make shader look significantly darker than Legacy ones
	// and 2) on engine side "Non-important" lights have to be divided by Pi to in cases when they are injected into ambient SH
	// NOTE: multiplication by Pi is part of single constant together with 1/4 now

	half specularTerm = (V * D) * (4 * UNITY_PI);// Torrance-Sparrow model, Fresnel is applied later (for optimization reasons)
	if (IsGammaSpace())
		specularTerm = sqrt(max(1e-4h, specularTerm));
	specularTerm = max(0, specularTerm * nl);

#if defined(_SPECULARHIGHLIGHTS_OFF) || (!defined(POINT) && !defined(SPOT) && !defined(POINT_COOKIE) && defined(_WATER_BACK))
	specularTerm = 0.0;
#endif

	half diffuseTerm = disneyDiffuse * nl;

	half realRoughness = roughness * roughness;		// need to square perceptual roughness
	half surfaceReduction;
	if (IsGammaSpace()) surfaceReduction = 1.0 - 0.28*realRoughness*roughness;		// 1-0.28*x^3 as approximation for (1/(x^4+1))^(1/2.2) on the domain [0;1]
	else surfaceReduction = 1.0 / (realRoughness*realRoughness + 1.0);			// fade \in [0.5;1]

	half grazingTerm = saturate(oneMinusRoughness + (1 - oneMinusReflectivity));

	half3 depthFade;
	half3 refraction = ComputeRefractionColor(normal, viewDir, posWorld, light.dir, light.color, globalWaterData, depthFade);

#if defined(_WATER_BACK)
	half3 fresnel = WaterFresnelLerp(specColor, grazingTerm, nv, true);
	half3 refractedColor = refraction * (1.0 - fresnel);
#else
	half3 fresnel = WaterFresnelLerp(specColor, grazingTerm, nv, false);
	half3 refractedColor = (diffColor * gi.diffuse * (1.0 - depthFade) + refraction) * (1.0 - fresnel);
	//half3 refractedColor = (gi.diffuse * (1.0 - depthFade) + refraction) * (1.0 - fresnel);
#endif

	half3 color = lerp(diffColor * light.color * diffuseTerm * atten, refractedColor, refractivity)
		+ specularTerm * light.color * WaterFresnelTerm(specColor, lh) * atten
		+ surfaceReduction * gi.specular * fresnel;

	return half4(color, 1);
}

half4 BRDF2_Unity_PBS_Water(half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness, half refractivity,
	half3 normal, half3 viewDir, half3 posWorld,
	UnityLight light, UnityIndirect gi, half atten)
{
	half3 halfDir = normalize(light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm(normal, halfDir);
	half nv = DotClamped(normal, viewDir);
	half lh = DotClamped(light.dir, halfDir);

	half roughness = 1 - oneMinusRoughness;
	half specularPower = RoughnessToSpecPower(roughness);
	// Modified with approximate Visibility function that takes roughness into account
	// Original ((n+1)*N.H^n) / (8*Pi * L.H^3) didn't take into account roughness
	// and produced extremely bright specular at grazing angles

	// HACK: theoretically we should divide by Pi diffuseTerm and not multiply specularTerm!
	// BUT 1) that will make shader look significantly darker than Legacy ones
	// and 2) on engine side "Non-important" lights have to be divided by Pi to in cases when they are injected into ambient SH
	// NOTE: multiplication by Pi is cancelled with Pi in denominator

	half invV = lh * lh * oneMinusRoughness + roughness * roughness; // approx ModifiedKelemenVisibilityTerm(lh, 1-oneMinusRoughness);
	half invF = lh;
	half specular = ((specularPower + 1) * pow(nh, specularPower)) / (unity_LightGammaCorrectionConsts_8 * invV * invF + 1e-4f); // @TODO: might still need saturate(nl*specular) on Adreno/Mali

	half fresnelTerm = WaterFresnelFast(nv);

	half grazingTerm = saturate(oneMinusRoughness + (1 - oneMinusReflectivity));
	half3 color = specular * light.color * nl * atten
		+ gi.specular * lerp(specColor, grazingTerm, fresnelTerm);

	half3 depthFade;
	half3 refraction = ComputeRefractionColor(normal, viewDir, posWorld, light.dir, light.color, globalWaterData, depthFade);
	color = lerp(color, refraction, (1.0 - fresnelTerm) * refractivity);

	return half4(color, 1);
}

half4 BRDF3_Unity_PBS_Water(half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness, half refractivity,
	half3 normal, half3 viewDir, half3 posWorld,
	UnityLight light, UnityIndirect gi, half atten)
{
	half LUT_RANGE = 16.0; // must match range in NHxRoughness() function in GeneratedTextures.cpp

	half3 reflDir = reflect(viewDir, normal);
	half3 halfDir = normalize(light.dir + viewDir);

	half nl = light.ndotl;
	half nh = BlinnTerm(normal, halfDir);
	half nv = DotClamped(normal, viewDir);
	half rl = dot(reflDir, light.dir);

	// Vectorize Pow4 to save instructions
	half rlPow4 = Pow4(rl); // power exponent must match kHorizontalWarpExp in NHxRoughness() function in GeneratedTextures.cpp
	half fresnelTerm = SimpleFresnel(nv);

	half3 depthFade;
	half3 refraction = ComputeRefractionColor(normal, viewDir, posWorld, light.dir, light.color, globalWaterData, depthFade);

#if 1 // Lookup texture to save instructions
	half specular = tex2D(unity_NHxRoughness, half2(rlPow4, 0.5 * (1 - oneMinusRoughness))).UNITY_ATTEN_CHANNEL * LUT_RANGE;
#else
	half roughness = 1 - oneMinusRoughness;
	half n = RoughnessToSpecPower(roughness) * .25;
	half specular = (n + 2.0) / (2.0 * UNITY_PI * UNITY_PI) * pow(dot(reflDir, light.dir), n) * nl;// / unity_LightGammaCorrectionConsts_PI;
																								   //half specular = (1.0/(UNITY_PI*roughness*roughness)) * pow(dot(reflDir, light.dir), n) * nl;// / unity_LightGammaCorrectionConsts_PI;
#endif
																							   //half grazingTerm = saturate(oneMinusRoughness + (1-oneMinusReflectivity));
	half grazingTerm = oneMinusRoughness + (1 - oneMinusReflectivity);

	half3 color = specular * light.color * nl
		+ gi.specular * lerp(specColor, grazingTerm, fresnelTerm);

	color = lerp(color, refraction, (1.0 - fresnelTerm) * refractivity);

	return half4(color, 1);
}

#endif