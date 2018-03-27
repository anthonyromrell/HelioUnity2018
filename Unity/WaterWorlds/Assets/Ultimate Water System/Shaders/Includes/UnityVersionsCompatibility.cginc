// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



/*
 * Ensures that water shaders will work on all Unity versions.
 */

#ifndef UNITY_VERSIONS_COMPATIBILITY_INCLUDED
#define UNITY_VERSIONS_COMPATIBILITY_INCLUDED

#if UNITY_VERSION <= 540

#define api_updater_trick(a, b) a##b
#define unity_ObjectToWorld api_updater_trick(_Object, 2World)
#define unity_WorldToObject api_updater_trick(_World, 2Object)
#define unity_CameraToWorld api_updater_trick(_Camera, ToWorld)

#define UnityObjectToClipPos(pos) UnityObjectToClipPos(float4(pos.xyz, 1.0))
#endif

#if UNITY_VERSION >= 550
#include "UnityCG.cginc"
#include "../Includes/UnityLightingCommon.cginc"
#include "../Includes/UnityStandardConfig.cginc"
#include "UnityStandardBRDF.cginc"
#include "UnityImageBasedLighting.cginc"
#endif

#endif