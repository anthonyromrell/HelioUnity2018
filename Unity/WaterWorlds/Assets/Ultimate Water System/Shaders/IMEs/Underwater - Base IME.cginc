struct VertexInput
{
    float4 vertex	: POSITION;
    float2 uv0		: TEXCOORD0;
};

struct VertexOutput
{
    float4 pos	: SV_POSITION;
    float2 uv	: TEXCOORD0;
    float2 uv1	: TEXCOORD1;
};

sampler2D _MainTex;
sampler2D _UnderwaterMask2;
half2 _MainTex_TexelSize;
half _DistortionIntensity;
half3 _Color;
half3 _TotalDirectionalLightsContribution;
float4x4 UNITY_MATRIX_VP_INVERSE;

sampler2D _DistortionTex;
sampler2D_float _CameraDepthTexture;

#define _VERTICAL_OFFSET _SurfaceOffset.y
#define _UNDERWATER_EFFECT 1

#include "UnityCG.cginc"
#include "../Includes/WaterLib.cginc"
#include "../Includes/Common.cginc"

VertexOutput vert(VertexInput vi)
{
    VertexOutput vo;

    vo.pos = CustomBlitTransform(vi.vertex);
    vo.uv = vi.uv0;
    vo.uv1 = vi.uv0;

    #if UNITY_UV_STARTS_AT_TOP
    if(_MainTex_TexelSize.y < 0)
        vo.uv1.y = 1.0 - vo.uv1.y;
    #endif

    return vo;
}

fixed4 forward(VertexOutput vo)
{
    #if UNITY_UV_STARTS_AT_TOP
    vo.uv.y -= 0.04;
    #else
    vo.uv.y += 0.04;
    #endif

    return tex2D(_MainTex, vo.uv).r;
}

fixed4 FinishMask(VertexOutput vo) : SV_Target
{
    fixed2 c = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(vo.uv)).rg;

    return fixed4(c.r - c.g, 0, 0, 1);
}

float rand(float2 p) { return frac(cos(dot(p, float2(23.14069263277926, 2.665144142690225)))*123456.0); }

half4 ime(VertexOutput vo) : SV_Target
{
    fixed mask = tex2D(_UnderwaterMask, UnityStereoTransformScreenSpaceTex(vo.uv1));
    half4 color = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(vo.uv));
    half4 screenPos = half4(vo.uv1 * 2 - 1, 0, 1);

    half4 pixelWorldSpacePos = mul(UNITY_MATRIX_VP_INVERSE, screenPos);
    pixelWorldSpacePos.xyz /= pixelWorldSpacePos.w;

    half3 ray = pixelWorldSpacePos - _WorldSpaceCameraPos;
    ray.xyz = normalize(ray.xyz) * half3(3, -3, 3);

    float depth = tex2D(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(vo.uv1));
    depth = LinearEyeDepth(depth);

    half3 absorptionColor = tex2D(_UnderwaterAbsorptionGradient, (_SurfaceOffset.y - pixelWorldSpacePos.y + ray.y * depth * 0.5) / 600.0);
    half3 depthColor = _Color * ComputeDepthColor(pixelWorldSpacePos, ray.xyz, half3(0, 1, 0), half3(1, 1, 1), _SurfaceOffset.xyz, 1.0, _TotalDirectionalLightsContribution, depth);

    half3 sceneToAbsorptionLerp = mask * (1.0 - exp(-absorptionColor * depth));

    if(depth >= _ProjectionParams.z * 0.98)		// don't let skybox remain visible at some setups
        sceneToAbsorptionLerp = mask;

    const float ditherFactor = pow(1.0f / (1.0f + length(depthColor)) * 2.0f - 1.0f, 2.0f);
    const float dither = 1.0f + rand(screenPos.xy) * ditherFactor * 0.05f;

    return half4(lerp(color.rgb, depthColor, saturate(sceneToAbsorptionLerp * dither)) , 1);
}

half4 ime2(VertexOutput vo) : SV_Target
{
    fixed mask = tex2D(_UnderwaterMask, UnityStereoTransformScreenSpaceTex(vo.uv1));

    half2 distortion = tex2D(_DistortionTex, vo.uv).xy - 0.75;
    vo.uv += distortion * mask * _DistortionIntensity;

    return tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(vo.uv));
}

//
// Mask IME
//

struct VertexOutput2
{
    float4 pos			: SV_POSITION;
    half2 uv0			: TEXCOORD0;
    half2 uv1			: TEXCOORD1;
    half2 uv2			: TEXCOORD2;
};

VertexOutput2 vertMask(VertexInput vi)
{
    VertexOutput2 vo;

    //vo.pos = mul(UNITY_MATRIX_MVP, vi.vertex);
    vo.pos = vi.vertex;
    vo.uv0.xy = vi.uv0;
    vo.uv1.xy = vi.uv0 - half2(_MainTex_TexelSize.xy * 2);
    vo.uv2.xy = vi.uv0 + half2(_MainTex_TexelSize.xy * 2);

    return vo;
}

fixed4 fragMask(VertexOutput2 vo) : SV_Target
{
    fixed3 mask;
    mask.x = tex2D(_UnderwaterMask2, UnityStereoTransformScreenSpaceTex(vo.uv0));
    mask.y = tex2D(_UnderwaterMask2, UnityStereoTransformScreenSpaceTex(vo.uv1));
    mask.z = tex2D(_UnderwaterMask2, UnityStereoTransformScreenSpaceTex(vo.uv2));

    mask = mask * 255 % 2;

    return dot(mask, 1) < 1.5 ? 0 : 1;
}