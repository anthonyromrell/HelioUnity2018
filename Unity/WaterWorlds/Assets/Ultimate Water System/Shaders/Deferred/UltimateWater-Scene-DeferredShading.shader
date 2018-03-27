// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Hidden/UltimateWater-Scene-DeferredShading" {
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
        #pragma multi_compile ____ WATER_BUFFERS_ENABLED

        #pragma exclude_renderers nomrt

        #include "UnityCG.cginc"
        #include "UnityDeferredLibrary.cginc"
        #include "UnityPBSLighting.cginc"
        #include "UnityStandardUtils.cginc"
        #include "UnityStandardBRDF.cginc"

        sampler2D _CameraGBufferTexture0;
    sampler2D _CameraGBufferTexture1;
    sampler2D _CameraGBufferTexture2;
    sampler2D _CausticsMap;
    sampler2D _CausticsDistortionMap;
    sampler2D _UnderwaterMask;
    sampler2D _WaterlessDepthTexture;
    float	  _CausticsMapKey;
    float	  _CausticsMultiplier;
    float4x4  _CausticsMapProj;
    float4	  _CausticsOffsetScale;
    float4	  _CausticsOffsetScale2;
    half4	  _AbsorptionColor;
    sampler2D _TotalDisplacementMap;
    float4	  _LocalMapsCoords;

    #if UNITY_VERSION >= 550 && SHADER_TARGET >= 40
    #define conditionalTex2D tex2D
    #else
    #define conditionalTex2D tex2Dlod
    #endif

    float3 ApplyWaterInfluence(float2 uv, float3 wpos)
    {
        #if defined(WATER_BUFFERS_ENABLED) && (defined(DIRECTIONAL) || defined (DIRECTIONAL_COOKIE))
        UNITY_BRANCH
            if(abs(_LightPos.y - 6.137) < 0.001)
            {
                // apply directional light caustics
                float4 uvlod = float4(uv, 0.0, 0.0);
                float waterDepth = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, uvlod);
                float depth = SAMPLE_DEPTH_TEXTURE_LOD(_WaterlessDepthTexture, uvlod);
                half underwaterMask = tex2Dlod(_UnderwaterMask, uvlod).r;

                #if UNITY_VERSION >= 550
                waterDepth = lerp(waterDepth, 1.0, underwaterMask);
                if(waterDepth > depth)
                    #else
                waterDepth *= 1.0 - underwaterMask;
                if(waterDepth < depth)
                    #endif
                {
                    half2 localUv = uv * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                    half waterElevation = tex2Dlod(_TotalDisplacementMap, half4(localUv, 0, 0)).y;

                    float4 uvx = mul(_CausticsMapProj, float4(wpos, 1));
                    uvx.xy = (uvx.xy * float2(0.5, -0.5)) + 0.5;
                    uvx.zw = 0;

                    half2 distort = tex2Dlod(_CausticsDistortionMap, uvx);
                    float4 uvx2 = uvx;
                    uvx2.xy += distort * _CausticsOffsetScale.ww;
                    uvx2.xy = uvx2.xy * _CausticsOffsetScale.zz + _CausticsOffsetScale.xy;
                    half caustic = conditionalTex2D(_CausticsMap, uvx2).r;

                    uvx2.xy = uvx.xy;
                    uvx2.xy += distort.yx * _CausticsOffsetScale2.ww;
                    uvx2.xy = uvx2.xy * _CausticsOffsetScale2.zz + _CausticsOffsetScale2.xy;
                    caustic += conditionalTex2D(_CausticsMap, uvx2).r;

                    half depthDelta = LinearEyeDepth(depth) - LinearEyeDepth(waterDepth);

                    return lerp(1, caustic * (1.0f + _CausticsMultiplier) * exp(_AbsorptionColor * 0.15 * min(wpos.y - waterElevation, 0.0)), saturate(depthDelta * 0.1));
                }
            }
            else
            {
                // apply light fade under water
                float4 uvlod = float4(uv, 0.0, 0.0);
                float waterDepth = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, uvlod);
                float depth = SAMPLE_DEPTH_TEXTURE_LOD(_WaterlessDepthTexture, uvlod);
                half underwaterMask = tex2Dlod(_UnderwaterMask, uvlod).r;

                #if UNITY_VERSION >= 550
                waterDepth = lerp(waterDepth, 1.0, underwaterMask);
                if(waterDepth > depth)
                    #else
                waterDepth *= 1.0 - underwaterMask;
                if(waterDepth < depth)
                    #endif
                {
                    half2 localUv = uv * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                    half waterElevation = tex2Dlod(_TotalDisplacementMap, half4(localUv, 0, 0)).y;

                    half depthDelta = LinearEyeDepth(depth) - LinearEyeDepth(waterDepth);

                    return (1.0f + _CausticsMultiplier) * lerp(1, exp(_AbsorptionColor * 0.15 * min(wpos.y - waterElevation, 0.0)), saturate(depthDelta * 0.1));
                }
            }
        #endif

        return 1;
    }

    half4 CalculateLight(unity_v2f_deferred i)
    {
        float3 wpos;
        float2 uv;
        float atten, fadeDist;
        UnityLight light;
        UNITY_INITIALIZE_OUTPUT(UnityLight, light);
        UnityDeferredCalculateLightParams(i, wpos, uv, light.dir, atten, fadeDist);

        half4 gbuffer0 = tex2D(_CameraGBufferTexture0, uv);
        half4 gbuffer1 = tex2D(_CameraGBufferTexture1, uv);
        half4 gbuffer2 = tex2D(_CameraGBufferTexture2, uv);

        float3 atten3 = atten * ApplyWaterInfluence(uv, wpos);

        light.color = _LightColor.rgb * atten3;
        half3 baseColor = gbuffer0.rgb;
        half3 specColor = gbuffer1.rgb;
        half oneMinusRoughness = gbuffer1.a;
        half3 normalWorld = gbuffer2.rgb * 2 - 1;
        normalWorld = normalize(normalWorld);
        float3 eyeVec = normalize(wpos - _WorldSpaceCameraPos);
        half oneMinusReflectivity = 1 - SpecularStrength(specColor.rgb);
        light.ndotl = LambertTerm(normalWorld, light.dir);

        UnityIndirect ind;
        UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
        ind.diffuse = 0;
        ind.specular = 0;

        half4 res = UNITY_BRDF_PBS(baseColor, specColor, oneMinusReflectivity, oneMinusRoughness, normalWorld, -eyeVec, light, ind);

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

        #define UnityObjectToClipPos(pos) UnityObjectToClipPos(float4(pos.xyz, 1.0))

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