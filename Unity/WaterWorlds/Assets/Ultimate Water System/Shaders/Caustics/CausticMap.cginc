#include "UnityCG.cginc"
#include "../Includes/UnityStandardCore.cginc"

struct v2f
{
    float4 vertex : SV_POSITION;
    half2 uv : TEXCOORD0;
    half intensity : TEXCOORD1;
};

sampler2D_float _WorldPosMap;
half _CausticLightIntensity;

VertexInput vert(VertexInput v)
{
    return v;
}

v2f vert2(VertexInput v)
{
    v2f o;
    float4 rayPosWorld = v.vertex;
    half2 normal;
    float4 fftUV, fftUV2;
    float3 totalDisplacement;
    float4 projectorViewPos;

    TransformVertex(DistanceMask(v, rayPosWorld), rayPosWorld, /*out*/ normal, /*out*/ fftUV, /*out*/ fftUV2, /*out*/ totalDisplacement, /*out*/ projectorViewPos, false);

    rayPosWorld = v.vertex;

    half4 mask = tex2Dlod(_DisplacementsMask, half4(projectorViewPos.xy / projectorViewPos.w, 0, 0));
    normal = tex2Dlod(_GlobalNormalMap, float4(fftUV.xy, 0, 0)).xy * mask.x;
    normal += tex2Dlod(_GlobalNormalMap, float4(fftUV.zw, 0, 0)).zw * mask.y;
    normal += tex2Dlod(_GlobalNormalMap1, float4(fftUV2.xy, 0, 0)).xy * mask.z;
    normal += tex2Dlod(_GlobalNormalMap1, float4(fftUV2.zw, 0, 0)).zw * mask.w;
    normal *= _DisplacementNormalsIntensity;

    half4 overlayNormal = tex2Dlod(_LocalNormalMap, half4(projectorViewPos.xy / projectorViewPos.w, 0, 0));
    normal.xy += overlayNormal.xy;

    //float3 refractedDir = -normalize(float3(normal.x, 50.4, normal.y));
    half3 normal3 = normalize(float3(normal.x, 0.05, normal.y));
    half3 refractedDir = normalize(refract(_CausticLightDir, normal3, 1.33333));

    half nl = LambertTerm(normal3, -_CausticLightDir);

    float4 uv;
    float3 geometryPosWorld;

    o.intensity = _CausticLightIntensity * nl;

    rayPosWorld.xyz += refractedDir;

    for(int i = 0; i < 4; ++i)
    {
        half4 projectedRay = mul(UNITY_MATRIX_VP, rayPosWorld);
        half sceneDepth = tex2Dlod(_WorldPosMap, half4(projectedRay.xy * half2(0.5, -0.5) + 0.5, 0, 0));
        half delta = min(10, LinearEyeDepthHalf(sceneDepth) - LinearEyeDepthHalf(projectedRay.z));
        rayPosWorld.xyz += refractedDir * delta;
    }

    o.vertex = rayPosWorld;
    return o;
}

half4 frag(v2f i) : SV_Target
{
    half2 s = sin(i.uv.xy);
    //return 0.25;
    return s.x * s.y * i.intensity;
}

void RenderPoint(VertexInput px, half pointSize, inout TriangleStream<v2f> outStream)
{
    v2f p = vert2(px);
    p.vertex = mul(UNITY_MATRIX_VP, p.vertex);

    //const half pointSize = 0.015;
    half pointSizeHalf = pointSize * 0.5;

    p.vertex.x += pointSizeHalf;
    p.vertex.y -= pointSizeHalf;
    p.uv = half2(3.14159, 0);
    outStream.Append(p);
    p.vertex.y += pointSize;
    p.uv = half2(3.14159, 3.14159);
    outStream.Append(p);
    p.vertex.x -= pointSize;
    p.vertex.y -= pointSize;
    p.uv = half2(0, 0);
    outStream.Append(p);
    p.vertex.y += pointSize;
    p.uv = half2(0, 3.14159);
    outStream.Append(p);

    outStream.RestartStrip();
}

[maxvertexcount(4)]
void geom(triangle VertexInput input[3], inout TriangleStream<v2f> outStream)
{
    VertexInput p = input[0];

    if(dot(input[1].vertex.xyz - input[0].vertex.xyz, input[2].vertex.xyz - input[0].vertex.xyz) < 0.01)
        p.vertex = (input[1].vertex + input[2].vertex) * 0.5;
    else if(dot(input[0].vertex.xyz - input[1].vertex.xyz, input[2].vertex.xyz - input[1].vertex.xyz) < 0.01)
        p.vertex = (input[0].vertex + input[2].vertex) * 0.5;
    else
        p.vertex = (input[1].vertex + input[0].vertex) * 0.5;

    RenderPoint(p, 0.035, outStream);
}