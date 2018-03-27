// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Spray/Generator (Local)"
{
    Properties
    {
        _Lambda("", Float) = 1
    }

        CGINCLUDE

        #include "UnityCG.cginc"

        struct VertexInput
    {
        float4 vertex	: POSITION;
        float2 uv0		: TEXCOORD0;
    };

    struct VertexOutput
    {
        float4 pos		: SV_POSITION;
        half2 uv_a		: TEXCOORD0;		// right
        half2 uv_b		: TEXCOORD1;		// up
        half2 uv_c		: TEXCOORD2;		// left
        half2 uv_d		: TEXCOORD3;		// down
    };

    struct ParticleData
    {
        float3 position;
        float3 velocity;
        float2 lifetime;
        float offset;
        float maxIntensity;
    };

    half4 _Params;			// x - lambda, y - spawn rate, z - horizontal displacement scale / tile size, w - scale

    sampler2D	_TotalDisplacementMap;
    float4		_TotalDisplacementMap_TexelSize;
    sampler2D	_LocalNormalMap;
    sampler2D	_LocalUtilityMap;
    float2		_SurfaceOffset;
    float4		_LocalMapsCoords;
    int			_Iterations;

    AppendStructuredBuffer<ParticleData> particles : register(u1);

    VertexOutput vert(VertexInput vi)
    {
        VertexOutput vo;

        half offset = 0.25 * _LocalMapsCoords.zz;

        vo.pos = UnityObjectToClipPos(vi.vertex);

        vo.uv_a = vi.uv0 + float2(offset, 0.0);
        vo.uv_b = vi.uv0 + float2(0.0, offset);
        vo.uv_c = vi.uv0 + float2(-offset, 0.0);
        vo.uv_d = vi.uv0 + float2(0.0, -offset);

        return vo;
    }

    inline float random(float2 p)
    {
        float2 r = float2(23.14069263277926, 2.665144142690225);
        return frac(cos(dot(p, r)) * 123.0);
    }

    inline float gauss(float2 p)
    {
        return sqrt(-2.0f * log(random(p))) * sin(3.14159 * 2.0 * random(p * -0.3241241));
    }

    inline float halfGauss(float2 p)
    {
        return abs(sqrt(-2.0f * log(random(p))) * sin(3.14159 * 2.0 * random(p * -0.3241241)));
    }

    inline half2 SampleHorizontalDisplacement(half2 uv1)
    {
        return tex2D(_TotalDisplacementMap, uv1.xy).xz;
    }

    inline half3 SampleFullDisplacement(half2 uv1)
    {
        return tex2Dlod(_TotalDisplacementMap, half4(uv1.xy, 0, 0));
    }

    fixed4 frag(VertexOutput vo) : SV_Target
    {
        half2 h10 = SampleHorizontalDisplacement(vo.uv_a);
        half2 h01 = SampleHorizontalDisplacement(vo.uv_b);
        half2 h20 = SampleHorizontalDisplacement(vo.uv_c);
        half2 h02 = SampleHorizontalDisplacement(vo.uv_d);

        half4 diff = half4(h20 - h10, h02 - h01) * -0.7;
        half3 j = half3(diff.x, diff.w, diff.y) * _Params.x;

        j.xy += 1.0;

        half2 eigenvalue = ((j.x + j.y) + half2(1, -1) * sqrt(pow(j.x - j.y, 2) + 4.0 * j.z * j.z)) * 0.5;
        half2 q = (eigenvalue.xy - j.xx) / (j.z == 0 ? 0.00001 : j.z);

        half spawnRate = (0.94 - eigenvalue.y) * 18.0;
        half r = random(diff.xw);

        half shoreMask = tex2D(_LocalNormalMap, half2(vo.uv_b.x, vo.uv_a.y)).a;
        half2 localSpray = tex2D(_LocalUtilityMap, half2(vo.uv_b.x, vo.uv_a.y)).xy;

        half localSpawnRate = tanh(length(localSpray)) * 3.0;
        spawnRate = lerp(localSpawnRate, spawnRate, shoreMask);

        if(spawnRate < 0.45)
            spawnRate = 0.0;

        half2 eigenvector = lerp(localSpray, half2(1.0, q.y), shoreMask * shoreMask);

        //spawnRate += tex2D(_LocalNormalMap, half2(vo.uv_b.x, vo.uv_a.y)).b * 0.1;

        UNITY_BRANCH
        if(r * spawnRate > pow(_Params.y, 1024 / _TotalDisplacementMap_TexelSize.z))
        {
            half3 displacement = SampleFullDisplacement(half2(vo.uv_b.x, vo.uv_a.y));
            spawnRate = spawnRate * lerp(4.5, 1.0, shoreMask) + 2.0;

            ParticleData particle;
            particle.velocity.xz = spawnRate * normalize(eigenvector) * lerp(3.0, 2.0, shoreMask);
            particle.velocity.y = spawnRate * 0.2;

            for(int i = 0; i < _Iterations; ++i)
            {
                float2 r2 = float2(random(displacement.xz), random(displacement.zx));

                float2 worldPos = (float2(vo.uv_b.x, vo.uv_a.y) - _LocalMapsCoords.xy) * _LocalMapsCoords.ww;
                worldPos += (r2 - 0.5) * _TotalDisplacementMap_TexelSize.xy * _LocalMapsCoords.ww * 20;

                half intensity = log(spawnRate + 1) * (0.25 + halfGauss(displacement.zx) * 0.75);

                particle.position = float3(worldPos.x + _SurfaceOffset.x, displacement.y - 0.2, worldPos.y + _SurfaceOffset.y);
                particle.lifetime = 2.0 * intensity * _Params.w;
                particle.lifetime.y += 0.25;
                particle.offset = r2.x * 2;
                particle.maxIntensity = saturate(intensity) * _Params.w;
                particles.Append(particle);

                displacement.xz += 0.05;
            }
        }

        return 0;
    }

        ENDCG

        SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM

            #pragma target 5.0

            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }
    }
}