#include "UnityCG.cginc"
#include "../Includes/Random.cginc"
#include "WaveParticlesCommon.cginc"

struct VertexInput
{
    float4 vertex		: POSITION;
    float2 uv0			: TEXCOORD0;
    float4 particleData	: TEXCOORD1;
    float2 direction	: TEXCOORD2;

    #if defined(TRAILS)
    float trailIntensity : TEXCOORD3;
    #elif defined(FOAM)
    float2 uvOffset		: TEXCOORD3;
    float foam : TEXCOORD4;
    #endif
};

struct FragmentInput
{
    float4 vertex		: SV_POSITION;
    half4 uv			: TEXCOORD0;
    half4 particleData	: TEXCOORD1;
    half2 direction		: TEXCOORD2;
    half k				: TEXCOORD3;
    half3 worldPos		: TEXCOORD4;
    #if defined(TRAILS)
    half trailIntensity	: TEXCOORD5;
    #endif
};

struct FragmentOutput
{
    half4 displacement	: SV_Target0;
    half4 normal		: SV_Target1;
};

struct SprayParticleData
{
    float3 position;
    float3 velocity;
    float2 lifetime;
    float offset;
    float maxIntensity;
};

float4x4 _ParticlesVP;

AppendStructuredBuffer<SprayParticleData> spray : register(u3);
sampler2D _FoamAtlas;
sampler2D _FoamOverlayTexture;
float2 _FoamAtlasParams;

FragmentInput vert(VertexInput vi)
{
    FragmentInput vo;

    vo.vertex = mul(_ParticlesVP, vi.vertex);
    vo.uv.xy = vi.uv0 * 3.1415;			// don't use UNITY_PI here, as it may produce < 0 for sin
    vo.uv.zw = vi.uv0;
    vo.particleData = vi.particleData;
    vo.direction = vi.direction;
    vo.k = 2.0 * UNITY_PI / vi.particleData.x;
    vo.worldPos = vi.vertex;

    // lifetime to fade
    half initialLifetime = vi.particleData.w;
    half lifetime = vi.particleData.z;

    #if defined(FOAM)
    vo.uv.zw = (vi.uvOffset + (vi.uv0 /*+ half2(0.0, 0.3)*/) * _FoamAtlasParams);
    vo.particleData.z = vi.foam * 0.35 * saturate((initialLifetime - lifetime) * 3) * pow(saturate(lifetime / initialLifetime), 4);
    #else
    vo.particleData.z = saturate(initialLifetime - lifetime) * saturate(lifetime / initialLifetime);
    #endif

    #if defined(TRAILS)
    float wavelength = vi.particleData.x;
    float trailLength = max(0.0, initialLifetime - vi.particleData.z - 1.0) * length(vi.direction);
    vo.uv.w = 1.0 - vo.uv.w;
    vo.uv.w *= (1.0 - lifetime / initialLifetime) * trailLength / wavelength;
    vo.trailIntensity = vi.trailIntensity;
    #endif

    return vo;
}

FragmentOutput frag(FragmentInput input)
{
    half amplitude = input.particleData.y;
    half fade = input.particleData.z;

    half2 s, c;
    s = sin(input.uv.xy);
    c = cos(input.uv.xy);

    half box = s.x * s.x * s.y * s.y;
    half2 displacement = box * s.x * c.y * normalize(input.direction) * amplitude * 2;
    half2 normal = displacement * input.k;

    FragmentOutput po;
    po.displacement = half4(displacement.x, amplitude * box, displacement.y, 0) * fade;
    po.normal = half4(-normal.xy, 0, 0) * box * fade;

    /*half r = random(s.xy);

    UNITY_BRANCH
    if (r > 0.999 && box > 0.8)
    {
        half intensity = 0.6;

        SprayParticleData particle;
        particle.position = input.worldPos + float3(0.0, 4.0, 0.0);
        particle.velocity.xz = input.direction.xy;
        particle.velocity.y = 0.5 + random(input.uv.xy) * 3.5;

        half paramsw = 1.0;

        particle.lifetime = 2.0 * intensity * paramsw;
        particle.offset = r * 2;
        particle.maxIntensity = saturate(intensity) * paramsw;
        spray.Append(particle);
    }*/

    return po;
}

half4 fragFoam(FragmentInput input) : SV_Target
{
    half amplitude = input.particleData.x;
    half fade = input.particleData.z;

    return tex2D(_FoamAtlas, input.uv.zw).g /* tex2D(_FoamOverlayTexture, input.uv.zw * 4.0 + half2(0, _Time.x * 3.5))*/ * fade;
}

#if defined(TRAILS)
half4 fragDisplacementsMask(FragmentInput input) : SV_Target
{
    half fade = input.particleData.z;

    half2 s, c;
    s = sin(input.uv.xy);
    c = cos(input.uv.xy);

    half box = saturate(s.x * s.y);
    half t = sqrt(box) * fade;

    return 1.0 - t * half4(0.3, 0.2, 0.01, 0.0) * input.trailIntensity;
}

half4 fragFoamTrail(FragmentInput input) : SV_Target
{
    half fade = input.particleData.z;

    half2 s, c;
    s = sin(input.uv.xy);
    c = cos(input.uv.xy);

    half box = saturate(s.x * s.y);
    half t = sqrt(box) * fade;

    return t * input.trailIntensity * tex2D(_FoamOverlayTexture, input.uv.zw);
}
#endif

/* GEOMETRY */

StructuredBuffer<ParticleData> _Particles : register(t0);

struct GeometryInput
{
    float4 pos			: SV_POSITION;
    float4 particleData	: TEXCOORD0;
    float2 direction	: TEXCOORD1;

    #if defined(TRAILS)
    half trailIntensity	: TEXCOORD2;
    #elif defined(FOAM)
    half2 uvOffset		: TEXCOORD2;
    half foam			: TEXCOORD3;
    #endif
};

struct vs_in
{
    float4 vertex : POSITION;
    uint id : SV_VertexID;
};

GeometryInput geomVert(vs_in vi)			// vs_out geomVert (uint id : SV_VertexID)
{
    uint particleId = vi.id;

    ParticleData particle = _Particles[particleId];

    GeometryInput o;
    o.pos = particle.position.xyxy;
    o.particleData = float4(
        particle.wavelength,
        particle.amplitude,
        particle.lifetime,
        particle.initialLifetime
        );

    #if defined(TRAILS)
    #if !defined(TRAILS_FOAM)
    o.trailIntensity = particle.trailCalming;
    #else
    o.trailIntensity = particle.trailFoam;
    #endif
    #elif defined(FOAM)
    float t = frac(particle.uvOffsetPack);
    o.uvOffset = float2(t, (particle.uvOffsetPack - t) / 4.0);
    o.foam = particle.foam;
    #endif

    o.direction = particle.direction;
    return o;
}

void MakeInput(inout VertexInput vi, float3 vertex, half2 uv)
{
    vi.vertex = float4(vertex, 1.0);
    vi.uv0 = uv;
}

[maxvertexcount(4)]
void geom(point GeometryInput input[1], inout TriangleStream<FragmentInput> outStream)
{
    float2 position2d = input[0].pos.xy;
    float4 particleData = input[0].particleData;
    float2 direction = input[0].direction;

    float initialLifetime = particleData.w;
    float lifetime = particleData.z;

    VertexInput vi;
    vi.particleData = particleData;
    vi.direction = direction;
    #if defined(FOAM)
    vi.uvOffset = input[0].uvOffset;
    vi.foam = input[0].foam * (0.6 + saturate((initialLifetime - lifetime) * 3) * 0.4);
    #endif

    #if defined(TRAILS)
    vi.trailIntensity = input[0].trailIntensity;
    #endif

    float wavelength = particleData.x;
    float3 position = float3(position2d.x, 0.0, position2d.y);

    float2 forward = normalize(direction);
    float2 right = float2(forward.y, -forward.x);

    float2 t = (forward + right) * wavelength;
    float3 diagonal1 = float3(t.x, 0.0, t.y);

    t = (forward - right) * wavelength;
    float3 diagonal2 = float3(t.x, 0.0, t.y);

    float3 trail1 = 0;
    float3 trail2 = 0;

    #if defined(TRAILS)
    t = right * wavelength;
    diagonal1 = float3(t.x, 0.0, t.y);
    t = -right * wavelength;
    diagonal2 = float3(t.x, 0.0, t.y);

    float trailLength = max(0.0, initialLifetime - particleData.z - 1.0) * length(direction);

    //trail1 = -float3(forward.x, 0.0, forward.y) * wavelength * 0.5;
    trail2 = -float3(forward.x, 0.0, forward.y) * trailLength + trail1;
    #endif

    MakeInput(vi, position + diagonal1 + trail1, half2(1.0, 1.0));
    outStream.Append(vert(vi));

    MakeInput(vi, position - diagonal2 + trail2, half2(1.0, 0.0));
    outStream.Append(vert(vi));

    MakeInput(vi, position + diagonal2 + trail1, half2(0.0, 1.0));
    outStream.Append(vert(vi));

    MakeInput(vi, position - diagonal1 + trail2, half2(0.0, 0.0));
    outStream.Append(vert(vi));

    outStream.RestartStrip();
}
