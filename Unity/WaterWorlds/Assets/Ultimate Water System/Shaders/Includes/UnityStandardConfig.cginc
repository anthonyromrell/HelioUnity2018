#ifndef UNITY_STANDARD_CONFIG_INCLUDED
#define UNITY_STANDARD_CONFIG_INCLUDED

// Define Specular cubemap constants
#define UNITY_SPECCUBE_LOD_EXPONENT (1.5)
#define UNITY_SPECCUBE_LOD_STEPS (7) // TODO: proper fix for different cubemap resolution needed. My assumptions were actually wrong!

// Energy conservation for Specular workflow is Monochrome. For instance: Red metal will make diffuse Black not Cyan
#define UNITY_CONSERVE_ENERGY 0
#define UNITY_CONSERVE_ENERGY_MONOCHROME 1

// "platform caps" defines that were moved to editor, so they are set automatically when compiling shader
// UNITY_SPECCUBE_BOX_PROJECTION
// UNITY_SPECCUBE_BLENDING

// still add safe net for low shader models, otherwise we might end up with shaders failing to compile
// the only exception is WebGL in 5.3 - it will be built with shader target 2.0 but we want it to get rid of constraints, as it is effectively desktop
#if SHADER_TARGET < 30 && !UNITY_53_SPECIFIC_TARGET_WEBGL
#undef UNITY_SPECCUBE_BOX_PROJECTION
#define UNITY_SPECCUBE_BOX_PROJECTION 0
#undef UNITY_SPECCUBE_BLENDING
#define UNITY_SPECCUBE_BLENDING 0
#endif

#ifndef UNITY_SAMPLE_FULL_SH_PER_PIXEL
#define UNITY_SAMPLE_FULL_SH_PER_PIXEL 0
#endif

#define UNITY_GLOSS_MATCHES_MARMOSET_TOOLBAG2 1
#define UNITY_BRDF_GGX 0

// Orthnormalize Tangent Space basis per-pixel
// Necessary to support high-quality normal-maps. Compatible with Maya and Marmoset.
// However xNormal expects oldschool non-orthnormalized basis - essentially preventing good looking normal-maps :(
// Due to the fact that xNormal is probably _the most used tool to bake out normal-maps today_ we have to stick to old ways for now.
//
// Disabled by default, until xNormal has an option to bake proper normal-maps.
#define UNITY_TANGENT_ORTHONORMALIZE 0

#endif // UNITY_STANDARD_CONFIG_INCLUDED