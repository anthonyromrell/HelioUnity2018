Shader "UltimateWater/Deferred/FinalColorMix"
{
    CGINCLUDE
        #define REFRACTION_TEX _WaterColorTex
        #define _WAVES_FFT 1

        #include "UnityCG.cginc"
        #include "../Includes/WaterLib.cginc"

        struct appdata
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        half4 uv : TEXCOORD0;
    };

    v2f vert(appdata v)
    {
        v2f o;
        o.vertex = v.vertex;
        o.uv = half4(v.uv, 0, 0);
        return o;
    }

    //sampler2D _WaterlessDepthTexture;
    sampler2D _CameraGBufferTexture0;
    sampler2D _CameraGBufferTexture1;
    sampler2D _CameraGBufferTexture2;
    sampler2D _WaterDepthTexture;
    sampler2D _CameraDepthTexture;
    sampler2D _WaterColorTex;
    sampler2D _GlobalWaterLookupTex;
    half4x4 UNITY_MATRIX_VP_INVERSE;

    inline half3 GetRefractedColor(float waterSurfaceDepth, half sceneDepth1, half4 uv, half3 posWorld, half3 eyeVec, half3 worldNormal, half3 absorptionColor, half3 baseColor, half3 offset, half refractionDistortion, half displacementScale, bool isBack)
    {
        //half centerSceneDepth = sceneDepth1 - waterSurfaceDepth;
        half4 refractOffset = ComputeDistortOffset(worldNormal, refractionDistortion * 0.04);
        half4 refractCoord = uv + refractOffset;
        half centerSceneDepth = LinearEyeDepth(tex2Dlod(_WaterlessDepthTexture, refractCoord)) - waterSurfaceDepth;
        half distortScale = saturate(centerSceneDepth * 4);

        refractCoord = uv + refractOffset * distortScale;
        half3 sceneColor = tex2Dlod(_RefractionTex, refractCoord).rgb;

        float sceneDepth = LinearEyeDepth(tex2Dlod(_WaterlessDepthTexture, refractCoord).x) - waterSurfaceDepth;
        half3 depthFade = exp(-absorptionColor * sceneDepth);
        //depthFade *= saturate(sceneDepth - _RefractionMaxDepth);			// fade out refraction for objects placed closer than the water

        if(sceneDepth < 0)
            depthFade = 0;

        half w = saturate((waterSurfaceDepth - _WaterTileSize.x) * 3.0 * _DetailFadeFactor / _WaterTileSize.x);
        globalWaterData.totalMask.x = 1.0 - w;
        half3 depthColor = 0;

        //if(!isBack)
        //	depthColor = baseColor * ComputeDepthColor2(posWorld, eyeVec, 1, offset, 1.0) * 0.05;

        return isBack ? sceneColor : lerp(depthColor, sceneColor, depthFade);
    }

    half4 frag(v2f i) : SV_Target
    {
        half4 uv = UnityStereoTransformScreenSpaceTex(i.uv);

        half waterDepthClip = tex2Dlod(_CameraDepthTexture, uv);
        half waterlessDepth = tex2Dlod(_WaterlessDepthTexture, uv);

#if defined(UNITY_REVERSED_Z)
        UNITY_BRANCH
        if(waterlessDepth < waterDepthClip)
#else
        UNITY_BRANCH
        if(waterlessDepth > waterDepthClip)
#endif
        {
            half4 gbuffer0 = tex2Dlod(_CameraGBufferTexture0, uv);
            half4 gbuffer1 = tex2Dlod(_CameraGBufferTexture1, uv);
            half4 gbuffer2 = tex2Dlod(_CameraGBufferTexture2, uv);

            half refractivity = gbuffer0.r;
            half4 lookup0 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.125, 0, 0));
            half3 absorptionColor = lookup0.rgb;
            half refractionDistortion = lookup0.a;

            half4 lookup1 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.375, 0, 0));
            half4 lookup2 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.625, 0, 0));
            half3 offset = lookup2.xyz;
            half displacementScale = lookup2.w;

            half3 worldNormal = gbuffer2.xyz * 2.0 - 1.0;
            half4 screenPos = half4(uv.xy * 2 - 1, waterDepthClip, 1);
            screenPos.y = -screenPos.y;
            half4 pixelWorldSpacePos = mul(UNITY_MATRIX_VP_INVERSE, screenPos);
            pixelWorldSpacePos.xyz /= pixelWorldSpacePos.w;

            float waterDepth = LinearEyeDepth(waterDepthClip);
            waterlessDepth = LinearEyeDepthHalf(waterlessDepth);

            half3 ray = normalize(_WorldSpaceCameraPos - pixelWorldSpacePos);

            half3 depthFade;
            globalWaterData.depth = waterDepth;
            globalWaterData.refractedScreenPos = half4(uv.xyz, 1);

            half3 baseColor = half3(lookup0.r * gbuffer0.g / lookup0.g, gbuffer0.gb);

            bool isBack = gbuffer2.a < 0.5;

            half3 refractedColor = GetRefractedColor(waterDepth, waterlessDepth, uv, pixelWorldSpacePos.xyz, ray.xyz, worldNormal, absorptionColor, baseColor, offset, refractionDistortion, displacementScale, isBack);
            half3 waterColor = tex2Dlod(_WaterColorTex, uv).rgb;

            half oneMinusRoughness = gbuffer1.a;
            half roughness = 1 - oneMinusRoughness;

            half nv = DotClamped(worldNormal, ray.xyz);
            half3 totalRefraction = refractedColor * (1.0 - WaterFresnelLerp(gbuffer1.rgb, 1.0, nv, isBack));

            // blend edges
            half edgeBlendFactor = _EdgeBlendFactorInv;//lerp(10.0, _EdgeBlendFactorInv, refractivity);
            half blendEdgesFactor = saturate(0.6f * edgeBlendFactor * abs(waterlessDepth - waterDepth));

            half4 sceneColor = tex2Dlod(_RefractionTex, uv);

            //blendEdgesFactor = lerp(1.0, blendEdgesFactor, 0.3 + refractivity / 0.7);
            return half4(lerp(sceneColor.rgb, waterColor + totalRefraction * refractivity,  blendEdgesFactor), 1.0);
        }
        else
        {
            half4 sceneColor = tex2Dlod(_RefractionTex, uv);
            return sceneColor;
        }
    }
        ENDCG

        SubShader
    {
        Cull Off ZWrite Off ZTest Always
            //Blend One SrcAlpha

            Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 4.0

            ENDCG
        }
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
            //Blend One SrcAlpha

            Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 3.0

            ENDCG
        }
    }
}
