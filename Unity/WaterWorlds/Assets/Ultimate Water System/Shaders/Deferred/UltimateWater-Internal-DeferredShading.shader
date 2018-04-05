// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Hidden/UltimateWater-Internal-DeferredShading" {
    Properties{
        _LightTexture0("", any) = "" {}
    _LightTextureB0("", 2D) = "" {}
    _ShadowMapTexture("", any) = "" {}
    _SrcBlend("", Float) = 1
        _DstBlend("", Float) = 1
    }
        SubShader{
        // Pass 1: Lighting pass
        //  LDR case - Lighting encoded into a subtractive ARGB8 buffer
        //  HDR case - Lighting additively blended into floating point buffer
        Pass{
        ZWrite Off
        Blend[_SrcBlend][_DstBlend]

        CGPROGRAM
        #pragma target 3.0
        #pragma vertex vert_deferred
        #pragma fragment frag
        #pragma multi_compile_lightpass
        #pragma multi_compile ___ UNITY_HDR_ON

        #pragma exclude_renderers nomrt

        #define _DEFERRED_SHADER 1

        #include "UnityCG.cginc"

        #include "UnityDeferredLibrary.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardUtils.cginc"
        #include "UnityStandardBRDF.cginc"
        #include "../Includes/WaterLib.cginc"
        #include "../Includes/UnityVersionsCompatibility.cginc"

        sampler2D _CameraGBufferTexture0;
    sampler2D _CameraGBufferTexture1;
    sampler2D _CameraGBufferTexture2;
    sampler2D _GlobalWaterLookupTex;
    sampler2D _WaterShadowmap;
    half4	  _MainWaterWrapSubsurfaceScatteringPack;

    half4 BRDF1_Unity_PBS_WaterDeferred(half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness, half refractivity,
        half3 normal, half3 viewDir, half2 uv,
        UnityLight light, UnityIndirect gi, half atten, half refractionDistortion, bool isBack)
    {
        //oneMinusRoughness *= _LightSmoothnessMul;

        half roughness = 1 - oneMinusRoughness;
        half3 halfDir = normalize(light.dir + viewDir);

        half nl = light.ndotl;
        half nh = BlinnTerm(normal, halfDir);
        half nv = DotClamped(normal, viewDir);
        half lv = DotClamped(light.dir, viewDir);
        half lh = DotClamped(light.dir, halfDir);

        #if defined(POINT) || defined(SPOT) || defined(POINT_COOKIE)
        nl = (nl + _MainWaterWrapSubsurfaceScatteringPack.z) * _MainWaterWrapSubsurfaceScatteringPack.w;
        #else
        nl = (nl + _MainWaterWrapSubsurfaceScatteringPack.x) * _MainWaterWrapSubsurfaceScatteringPack.y;
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

        half specularTerm = (V * D) * (1.0 / (4 * UNITY_PI));// Torrance-Sparrow model, Fresnel is applied later (for optimization reasons)
        if(IsGammaSpace())
            specularTerm = sqrt(max(1e-4h, specularTerm));
        specularTerm = max(0, specularTerm * nl);

        #if defined(_SPECULARHIGHLIGHTS_OFF)
        specularTerm = 0.0;
        #endif

        half diffuseTerm = disneyDiffuse * nl;

        half realRoughness = roughness*roughness;		// need to square perceptual roughness
        half surfaceReduction;
        if(IsGammaSpace()) surfaceReduction = 1.0 - 0.28*realRoughness*roughness;		// 1-0.28*x^3 as approximation for (1/(x^4+1))^(1/2.2) on the domain [0;1]
        else surfaceReduction = 1.0 / (realRoughness*realRoughness + 1.0);			// fade \in [0.5;1]

        if(isBack)
            specularTerm = 0;

        half waterSurfaceDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv).r);

        half4 refractOffset = ComputeDistortOffset(normal, refractionDistortion * 0.04);
        half4 refractCoord = half4(uv, 0, 0) + refractOffset;
        half centerSceneDepth = LinearEyeDepth(tex2Dlod(_WaterlessDepthTexture, refractCoord)) - waterSurfaceDepth;
        half distortScale = saturate(centerSceneDepth * 4);

        uv += refractOffset * distortScale;

        half3 depthFade;
        half sceneDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_WaterlessDepthTexture, uv).r);
        depthFade = min(exp(-_AbsorptionColor * (sceneDepth - waterSurfaceDepth)), 1);

        half grazingTerm = saturate(oneMinusRoughness + (1 - oneMinusReflectivity));

        half3 fresnel = WaterFresnelLerp(specColor, grazingTerm, nv, isBack);
        half3 refractedColor = gi.diffuse * (1.0 - fresnel) * (1.0 - depthFade);

        half3 color = lerp(diffColor * light.color * diffuseTerm * atten, refractedColor, refractivity)
            + specularTerm * light.color * WaterFresnelTerm(specColor, lh) * atten
            + surfaceReduction * gi.specular * fresnel;

        return half4(color, 1);
    }

    half4 BRDF1_Unity_PBS_WaterDeferred_Back(half3 diffColor, half3 specColor, half oneMinusReflectivity, half oneMinusRoughness,
        half3 normal, half3 viewDir,
        UnityLight light, UnityIndirect gi, half atten)
    {
        //oneMinusRoughness *= _LightSmoothnessMul;

        half roughness = 1 - oneMinusRoughness;
        half3 halfDir = normalize(light.dir + viewDir);

        half nl = light.ndotl;
        half nh = BlinnTerm(normal, halfDir);
        half nv = DotClamped(normal, viewDir);
        half lv = DotClamped(light.dir, viewDir);
        half lh = DotClamped(light.dir, halfDir);

        #if defined(POINT) || defined(SPOT) || defined(POINT_COOKIE)
        nl = (nl + _MainWaterWrapSubsurfaceScatteringPack.z) * _MainWaterWrapSubsurfaceScatteringPack.w;
        #else
        nl = (nl + _MainWaterWrapSubsurfaceScatteringPack.x) * _MainWaterWrapSubsurfaceScatteringPack.y;
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

        half specularTerm = (V * D) * (1.0 / (4 * UNITY_PI));// Torrance-Sparrow model, Fresnel is applied later (for optimization reasons)
        if(IsGammaSpace())
            specularTerm = sqrt(max(1e-4h, specularTerm));
        specularTerm = max(0, specularTerm * nl);

        #if defined(_SPECULARHIGHLIGHTS_OFF)
        specularTerm = 0.0;
        #endif

        half diffuseTerm = disneyDiffuse * nl;

        half realRoughness = roughness*roughness;		// need to square perceptual roughness
        half surfaceReduction;
        if(IsGammaSpace()) surfaceReduction = 1.0 - 0.28*realRoughness*roughness;		// 1-0.28*x^3 as approximation for (1/(x^4+1))^(1/2.2) on the domain [0;1]
        else surfaceReduction = 1.0 / (realRoughness*realRoughness + 1.0);			// fade \in [0.5;1]

        half3 depthFade;

        half grazingTerm = saturate(oneMinusRoughness + (1 - oneMinusReflectivity));
        half3 color = diffColor * (gi.diffuse + light.color * diffuseTerm);

        return half4(color, 1);
    }

    half4 CalculateLight(unity_v2f_deferred i)
    {
        float3 wpos;
        float2 uv;
        float atten, fadeDist;
        float atten2 = 1.0;
        UnityLight light;
        UNITY_INITIALIZE_OUTPUT(UnityLight, light);
        UnityDeferredCalculateLightParams(i, wpos, uv, light.dir, atten, fadeDist);

        half4 gbuffer0 = tex2D(_CameraGBufferTexture0, uv);
        half4 gbuffer1 = tex2D(_CameraGBufferTexture1, uv);
        half4 gbuffer2 = tex2D(_CameraGBufferTexture2, uv);

        #if defined(DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
        UNITY_BRANCH
            if(abs(_LightPos.y - 6.137) < 0.001)
            {
                atten2 = tex2Dlod(_WaterShadowmap, float4(uv, 0, 0));
            }
        #endif

        light.color = _LightColor.rgb * atten;

        half4 lookup3 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.875, 0, 0));
        half lightSmoothnessMul = lookup3.a;

        #if defined(DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
        half4 lookup2 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.625, 0, 0));
        lightSmoothnessMul *= lookup2.a;
        #endif

        half3 baseColor = half3(lookup3.r * gbuffer0.g / lookup3.g, gbuffer0.gb);
        half3 specColor = gbuffer1.rgb;
        half refractivity = gbuffer0.r;
        half oneMinusRoughness = gbuffer1.a * lightSmoothnessMul;
        half3 normalWorld = gbuffer2.rgb * 2 - 1;
        normalWorld = normalize(normalWorld);
        float3 eyeVec = normalize(wpos - _WorldSpaceCameraPos);
        half oneMinusReflectivity = 1 - SpecularStrength(specColor.rgb);
        light.ndotl = LambertTerm(normalWorld, light.dir);

        bool isBack = gbuffer2.a < 0.5;

        UnityIndirect ind;
        UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
        ind.diffuse = 0;
        ind.specular = 0;

        half4 lookup1 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.375, 0, 0));
        half forwardScatterIntensity = lookup1.a;

        half4 lookup0 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.125, 0, 0));
        half3 absorptionColor = lookup0.rgb;
        half refractionDistortion = lookup0.a;
        ind.diffuse = baseColor * ComputeDepthColorv4(absorptionColor, eyeVec, light.color, light.dir, normalWorld) * forwardScatterIntensity;

        half4 res = BRDF1_Unity_PBS_WaterDeferred(baseColor, specColor, oneMinusReflectivity, oneMinusRoughness, refractivity, normalWorld, -eyeVec, uv, light, ind, atten2, refractionDistortion, isBack);

        return res;
    }

    #ifdef UNITY_HDR_ON
    half4
        #else
    fixed4
        #endif
        frag(unity_v2f_deferred i) : SV_Target
    {
        half4 c = CalculateLight(i);
        #ifdef UNITY_HDR_ON
        return c;
        #else
        return exp2(-c);
        #endif
    }

        ENDCG
    }

        // Pass 2: Final decode pass.
        // Used only with HDR off, to decode the logarithmic buffer into the main RT
        Pass{
        ZTest Always Cull Off ZWrite Off
        Stencil{
        ref[_StencilNonBackground]
        readmask[_StencilNonBackground]
        // Normally just comp would be sufficient, but there's a bug and only front face stencil state is set (case 583207)
        compback equal
        compfront equal
    }

        CGPROGRAM
        #pragma target 3.0
        #pragma vertex vert
        #pragma fragment frag
        #pragma exclude_renderers nomrt

        #include "../Includes/UnityVersionsCompatibility.cginc"

        sampler2D _LightBuffer;
    struct v2f
    {
        float4 vertex : SV_POSITION;
        float2 texcoord : TEXCOORD0;
    };

    v2f vert(float4 vertex : POSITION, float2 texcoord : TEXCOORD0)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(vertex);
        o.texcoord = texcoord.xy;
        return o;
    }

    fixed4 frag(v2f i) : SV_Target
    {
        return -log2(tex2D(_LightBuffer, i.texcoord));
    }
        ENDCG
    }
    }
        Fallback Off
}