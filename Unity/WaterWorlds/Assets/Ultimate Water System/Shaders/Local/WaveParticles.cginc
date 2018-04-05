#include "UnityCG.cginc"

struct appdata
{
    float4 vertex		: POSITION;
    float2 uv			: TEXCOORD0;
    #if defined(_DEBUG_PARTICLES)
    float3 normal		: NORMAL;
    #endif
    float4 tangent		: TANGENT;
};

struct v2f
{
    float4 vertex		: SV_POSITION;
    half4 uv			: TEXCOORD0;
    half amplitude		: TEXCOORD1;
    half4 dir			: TEXCOORD2;
    half2 pack			: TEXCOORD3;			// x = k, y = speed
    half2 cosUV			: TEXCOORD4;
    half shoaling		: TEXCOORD5;
    #if defined(_DEBUG_PARTICLES)
    half debug			: TEXCOORD6;			// x = group id
    #endif
};

struct PsOutput
{
    half4 displacement	: SV_Target0;
    half4 normal		: SV_Target1;
    #if defined(_DEBUG_PARTICLES)
    half4 debug			: SV_Target2;
    #endif
};

struct ParticleData
{
    float3 position;
    float3 velocity;
    float2 lifetime;
    float offset;
    float maxIntensity;
};

sampler2D _MainTex;
float4 _MainTex_TexelSize;
float4 _LocalMapsCoords;
float _WaterScale;
float4x4 _ParticlesVP;

AppendStructuredBuffer<ParticleData> particles : register(u3);

inline float random(float2 p)
{
    float2 r = float2(23.14069263277926, 2.665144142690225);
    return frac(cos(dot(p, r)) * 123.0);
}

inline float gauss(float2 p)
{
    return sqrt(-2.0f * log(random(p))) * sin(UNITY_PI * 2.0 * random(p * -0.3241241));
}

inline float halfGauss(float2 p)
{
    return abs(sqrt(-2.0f * log(random(p))) * sin(UNITY_PI * 2.0 * random(p * -0.3241241)));
}

v2f vert(appdata vi)
{
    v2f vo;

    float2 forward = vi.tangent.xy * 0.5;
    float2 right = float2(forward.y, -forward.x);
    float k = 2.0 * UNITY_PI / length(vi.tangent.xy);
    float shoaling = (1.0 - exp(vi.tangent.z * -1000)) * 4;
    float speed = vi.tangent.w;
    float2 worldPos = vi.vertex.xy + forward * (vi.uv.y - 0.5) + right * (vi.uv.x - 0.5);
    float amplitude = vi.vertex.z;

    // remove lines below
    shoaling = min(2.0, shoaling);
    amplitude *= 2.0;

    vo.vertex = mul(_ParticlesVP, half4(worldPos.x, 0, worldPos.y, 1));
    //vo.vertex.y = -vo.vertex.y;
    vo.uv = half4(vi.uv * UNITY_PI, worldPos.xy);
    //vo.cosUV = vi.uv * UNITY_PI - tanh(shoaling * 40) * 0.5;// *0.7;
    vo.cosUV = vi.uv * UNITY_PI - shoaling * 0.5;
    vo.amplitude = min(amplitude, 0.18 / k);			// 0.165
    vo.dir = half4(normalize(vi.tangent.xy), vi.tangent.xy);
    vo.pack.x = k;
    vo.pack.y = speed;
    vo.shoaling = shoaling;

    #if defined(_DEBUG_PARTICLES)
    vo.debug = vi.normal.x;
    #endif
    return vo;
}

PsOutput frag(v2f vo)
{
    half k = vo.pack.x;
    half speed = vo.pack.y;

    half2 s, c;
    s = sin(vo.uv.xy);
    c = cos(vo.cosUV.xy);
    //sincos(vo.uv.xy, s, c);

    //half fade = max(0, 1.0 - pow(max(abs(vo.uv.z), abs(vo.uv.w)), 4));
    half fade = 1.0;
    half box = s.x * s.y;

    half height = box * (s.x - sin(vo.uv.x * 2 - UNITY_PI * 0.5) * 0.9) * vo.amplitude;						// * s.x ensures that neighbouring waves will sum to 1 (sin^2(x) + cos^2(x) = 1)
    half2 displacement = box * s.x * c.y * vo.dir.xy * vo.amplitude;
    half2 normal = displacement * k * 0.4;
    half foam = max(0, box * s.x * (s.y - 0.7) * 0.5) * 0.5 * (1.0 + vo.shoaling) * vo.amplitude / k;

    half r = random(vo.uv.zw);

    #if SHADER_TARGET >= 50
    UNITY_BRANCH
        if(vo.shoaling > 0.55 && r > 0.9992 && box > 0.9 && vo.uv.y > 0.6)
        {
            float2 worldPos = vo.uv.zw + vo.dir.xy * (0.5 * vo.shoaling + 0.3) / k;
            worldPos += vo.dir.yx * half2(-1.0, 1.0) * (random(vo.uv.zw * _SinTime.xy) - 0.5) * 50.0 * _LocalMapsCoords.ww / _ScreenParams.xx;

            half spawnRate = foam + 2.0;
            half intensity = log(spawnRate + 1) * (0.25 + halfGauss(displacement.yx) * 0.75);

            ParticleData particle;
            particle.position = float3(worldPos.x, -10000 + 1, worldPos.y);
            particle.velocity.xz = vo.dir.xy * (speed + 4);
            particle.velocity.y = 0.5 + random(vo.uv.xy) * 3.5;

            half paramsw = 1.0;

            particle.lifetime = 2.0 * intensity * paramsw;
            particle.offset = r * 2;
            particle.maxIntensity = saturate(intensity) * paramsw;
            particles.Append(particle);
        }
    #endif

    PsOutput po;
    po.displacement = half4(displacement.x, height, displacement.y, 0) * fade;			// a = foam
    po.normal = half4(-normal.x, -normal.y, 0, 0) * fade;

    //half t = saturate(vo.cosUV.y - 1.95) * saturate(3.0 - vo.cosUV.y);
    //po.utility = half4(foam * 100 * vo.dir.xy * t, 0, 0) * fade;

    #if defined(_DEBUG_PARTICLES)
    po.debug = half4(vo.debug, 0, 0, 0);
    #endif

    return po;
}