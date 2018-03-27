#define LIGHTMAP_OFF 1
#define _CUBEMAP_REFLECTIONS 1
#define _COLLECT_LIGHT_PASS 1

#include "../Includes/UnityStandardCore.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex				: SV_POSITION;
    half4 projectorViewPos		: TEXCOORD0;
    half2 normal				: TEXCOORD1;
    half2 localMapsUv			: TEXCOORD2;
    half4 fftUV					: TEXCOORD3;
    float3 posWorld				: TEXCOORD4;
    #if UNITY_SHOULD_SAMPLE_SH
    half3 ambientOrLightmapUV	: TEXCOORD5;
    #endif
};

float3 _WorldSpaceOriginalCameraPos;
half2 _ScatteringParams;

v2f vert(VertexInput v)
{
    v2f o;
    float4 posWorld = GET_WORLD_POS(v.vertex);
    half2 uv2 = posWorld.xz / 100.0;
    half2 normal;
    float4 fftUV, fftUV2;
    float3 totalDisplacement;

    TransformVertex(DistanceMask(v, posWorld), posWorld, /*out*/ normal, /*out*/ fftUV, /*out*/ fftUV2, /*out*/ totalDisplacement, /*out*/ o.projectorViewPos);

    o.vertex = mul(UNITY_MATRIX_VP, posWorld);
    o.normal = normal;
    o.localMapsUv = posWorld.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
    o.fftUV = fftUV;
    o.posWorld = posWorld;

    half3 normalWorld = normalize(half3(normal.x, 1.0, normal.y));

    #if UNITY_SHOULD_SAMPLE_SH
    #if UNITY_SAMPLE_FULL_SH_PER_PIXEL
    o.ambientOrLightmapUV.rgb = 0;
    #elif (SHADER_TARGET < 30)
    o.ambientOrLightmapUV.rgb = ShadeSH9(half4(normalWorld, 1.0));
    #else
    // Optimization: L2 per-vertex, L0..L1 per-pixel
    o.ambientOrLightmapUV.rgb = ShadeSH3Order(half4(normalWorld, 1.0));
    #endif
    // Add approximated illumination from non-important point lights
    #ifdef VERTEXLIGHT_ON
    o.ambientOrLightmapUV.rgb += Shade4PointLights(
        unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
        unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
        unity_4LightAtten0, posWorld, normalWorld);
    #endif
    #endif

    return o;
}

half4 frag(v2f i) : SV_Target
{
    globalWaterData.totalMask = half4(1.0, 1.0, 1.0, 1.0);
    globalWaterData.fftUV = i.fftUV.xy;
    globalWaterData.fftUV2 = i.fftUV.zw;
    globalWaterData.projectorViewPos = i.projectorViewPos;

    half2 normalFlat = NormalInTangentSpace(i.fftUV.xyxy, i.normal);
    half3 normalWorld = normalize(half3(normalFlat.x, 1.0, normalFlat.y));

    UnityLight light = MainLight(normalWorld * half3(-1, 1, -1));
    FragmentCommonData data = SpecularSetup(i.fftUV.xyxy, normalWorld);

    half3 viewDir = i.posWorld - _WorldSpaceCameraPos;
    half3 normal = normalWorld;
    half oneMinusRoughness = data.oneMinusRoughness;
    oneMinusRoughness *= _LightSmoothnessMul;

    half roughness = 1 - oneMinusRoughness;
    half3 halfDir = normalize(light.dir + viewDir);

    half nl = light.ndotl;
    half nh = BlinnTerm(normal, halfDir);
    half nv = DotClamped(normal, viewDir);
    half lv = DotClamped(light.dir, viewDir);
    half lh = DotClamped(light.dir, halfDir);

    half nlPow5 = Pow5(1 - nl);
    half nvPow5 = 0.1;		 Pow5(1 - nv);
    half Fd90 = 0.5 + 2 * lh * lh * roughness;
    half disneyDiffuse = (1 + (Fd90 - 1) * nlPow5) * (1 + (Fd90 - 1) * nvPow5);
    half diffuseTerm = disneyDiffuse * nl;

    half2 dirRoughness = 1.0 - oneMinusRoughness;
    #if UNITY_SHOULD_SAMPLE_SH
    UnityGI gi = FragmentGI(i.posWorld, 1.0, half4(i.ambientOrLightmapUV, 1.0), 1.0, oneMinusRoughness, normalWorld, viewDir, light, 0, dirRoughness);
    #else
    UnityGI gi = FragmentGI(i.posWorld, 1.0, half4(0.0, 0.0, 0.0, 1.0), 1.0, oneMinusRoughness, normalWorld, viewDir, light, 0, dirRoughness);
    #endif

    half3 viewDir2 = normalize(i.posWorld - _WorldSpaceOriginalCameraPos);
    half nv2 = max(0.0, dot(-normal, viewDir2));
    half lv2 = max(0.0, dot(light.dir, viewDir2));

    if(dot(_LightColor0.rgb, _LightColor0.rgb) < 0.01)
        lv2 = 1.0;

    half ndotl1 = DotClamped(normal, light.dir);
    half ndotl2 = DotClamped(normal * half3(-1, 1, -1), light.dir);

    ndotl1 = max(ndotl1 * 0.9, ndotl2 * 0.5);

    half ambientDot = length(viewDir2.xz);

    half foamIntensity = tex2Dproj(_FoamMap, globalWaterData.projectorViewPos);
    foamIntensity = 1.0 - exp(-0.02 * foamIntensity);
    ndotl1 *= saturate(1.0 - foamIntensity);

    half scatterIntensity = 1;
    return half4(
        gi.indirect.diffuse * nv2 * (0.3 + ambientDot * 0.7) * _ScatteringParams.x +
        light.color * lerp(1, (1.0 + Pow5(1.0 - ndotl1)) * ndotl1, _ScatteringParams.y),
        scatterIntensity);
}

struct v2f_add
{
    float4 pos					: SV_POSITION;
    half4 projectorViewPos		: TEXCOORD0;
    half2 normal				: TEXCOORD1;
    half2 localMapsUv			: TEXCOORD2;
    half4 fftUV					: TEXCOORD3;
    half3 posWorld				: TEXCOORD4;
    half3 lightDir				: TEXCOORD5;
    LIGHTING_COORDS(6, 7)
};

v2f_add vert_add(VertexInput v)
{
    v2f_add o;
    float4 posWorld = GET_WORLD_POS(v.vertex);
    half2 uv2 = posWorld.xz / 100.0;
    half2 normal;
    float4 fftUV, fftUV2;
    float3 totalDisplacement;

    TransformVertex(DistanceMask(v, posWorld), posWorld, /*out*/ normal, /*out*/ fftUV, /*out*/ fftUV2, /*out*/ totalDisplacement, /*out*/ o.projectorViewPos);

    o.pos = mul(UNITY_MATRIX_VP, posWorld);
    o.normal = normal;
    o.localMapsUv = posWorld.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
    o.fftUV = fftUV;
    o.posWorld = posWorld;
    half3 lightDir = _WorldSpaceLightPos0.xyz - posWorld.xyz * _WorldSpaceLightPos0.w;
    #ifndef USING_DIRECTIONAL_LIGHT
    lightDir = NormalizePerVertexNormal(lightDir);
    #endif
    o.lightDir.xyz = lightDir;

    //We need this for shadow receving
    TRANSFER_VERTEX_TO_FRAGMENT(o);

    return o;
}

half4 frag_add(v2f_add i) : SV_Target
{
    globalWaterData.totalMask = half4(1.0, 1.0, 1.0, 1.0);
    globalWaterData.fftUV = i.fftUV.xy;
    globalWaterData.fftUV2 = i.fftUV.zw;
    globalWaterData.projectorViewPos = i.projectorViewPos;

    half2 normalFlat = -NormalInTangentSpace(i.fftUV.xyxy, i.normal);
    half3 normalWorld = normalize(half3(normalFlat.x, 1.0, normalFlat.y));

    UnityLight light = AdditiveLight(normalWorld, IN_LIGHTDIR_FWDADD(i), LIGHT_ATTENUATION(i));
    FragmentCommonData data = SpecularSetup(i.fftUV.xyxy, normalWorld);

    half3 viewDir = i.posWorld - _WorldSpaceCameraPos;
    half3 normal = normalWorld;
    half oneMinusRoughness = data.oneMinusRoughness;
    oneMinusRoughness *= _LightSmoothnessMul;

    half roughness = 1 - oneMinusRoughness;
    half3 halfDir = normalize(light.dir + viewDir);

    half nl = light.ndotl;
    half nh = BlinnTerm(normal, halfDir);
    half nv = DotClamped(normal, viewDir);
    half lv = DotClamped(light.dir, viewDir);
    half lh = DotClamped(light.dir, halfDir);

    half nlPow5 = Pow5(1 - nl);
    half nvPow5 = 0.1;		// Pow5(1 - nv);
    half Fd90 = 0.5 + 2 * lh * lh * roughness;
    half disneyDiffuse = (1 + (Fd90 - 1) * nlPow5) * (1 + (Fd90 - 1) * nvPow5);
    half diffuseTerm = disneyDiffuse * nl;

    half2 dirRoughness = 1.0 - oneMinusRoughness;

    return half4(light.color * diffuseTerm, abs(normalWorld.y));			// i.posWorld.y
}
