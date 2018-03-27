// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Spectrum/Water Spectrum"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _TileSizeLookup("", 2D) = "" {}
        _Gravity("", Float) = 9.81
        _PlaneSizeInv("", Float) = 0.01
        _TargetResolution("", Vector) = (256, 256, 256, 256)
        _LoopDuration("", Float) = 10.0
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
            float4 pos	: SV_POSITION;
            float2 uv	: TEXCOORD0;
            float2 uv1  : TEXCOORD1;
        };

        struct PSOutput
        {
            float4 height		: SV_Target;
            float4 displacement	: SV_Target1;
        };

        struct PSOutput2
        {
            float4 height					: SV_Target;
            float4 normal					: SV_Target1;
            float4 displacement				: SV_Target2;
        };

        sampler2D _MainTex;
        sampler2D _TileSizeLookup;
        float _RenderTime;
        float _Gravity;
        float2 _TargetResolution;
        float4 _MainTex_TexelSize;
        float _Weight;
        float _LoopDuration;

        VertexOutput vert(VertexInput vi)
        {
            VertexOutput vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv = (vi.uv0 - _MainTex_TexelSize.xy * 0.5) * 2.0;
            vo.uv1 = vi.uv0;

            return vo;
        }

        float2 Spectrum(float2 uv1, float2 uv2, float t)
        {
            float2 s1 = tex2D(_MainTex, uv1);
            float2 s2 = tex2D(_MainTex, uv2);

            float s, c;
            sincos(t, s, c);

            return float2((s1.x + s2.x) * c - (s1.y + s2.y) * s, (s1.x - s2.x) * s + (s1.y - s2.y) * c);
        }

        float2 Spectrum(float2 uv, float2 uv1, out float2 k)
        {
            float2 selector = step(float2(0.5, 0.5), uv1.xy);
            float3 tileSize = tex2D(_TileSizeLookup, selector);
            float pix2 = 6.2831853;
            float2 centeredUV = -0.5 + frac(uv + 0.5);
            k = pix2 * _TargetResolution.xy * tileSize.z * centeredUV;
            float t = _RenderTime * sqrt(_Gravity * length(k));

            #if defined(_LOOPING)
            float omega0 = pix2 / _LoopDuration;
            float omega = sqrt(_Gravity * length(k));
            float omegaQ = round(omega / omega0) * omega0;

            if(omegaQ == 0.0f)
                return 0.0f;

            t = _RenderTime * omegaQ;
            #endif

            float2 uv2 = tileSize.xy - uv1 + _MainTex_TexelSize.xy;
            float2 selector2 = floor(uv2.xy * 2.0);

            if(selector.x != selector2.x)
                uv2.x = uv1.x;

            if(selector.y != selector2.y)
                uv2.y = uv1.y;

            return Spectrum(uv1, uv2, t);
        }

        float4 animate(VertexOutput vo) : SV_Target0
        {
            float2 k;
            return float4(Spectrum(vo.uv, vo.uv1, k), 0, 1);
        }

            PSOutput animateDisplacements(VertexOutput vo)
        {
            float2 k;
            float2 s = Spectrum(vo.uv, vo.uv1, k);

            PSOutput po;
            po.height = float4(s, 1, 1);

            k = normalize(k);
            if(length(frac(vo.uv)) < 0.0002) k = 0.707107;

            po.displacement.x = s.y * k.x;
            po.displacement.y = -s.x * k.x;
            po.displacement.z = s.y * k.y;
            po.displacement.w = -s.x * k.y;

            return po;
        }

        PSOutput2 animatex3(VertexOutput vo)
        {
            float2 k;
            float2 s = Spectrum(vo.uv, vo.uv1, k);

            PSOutput2 po;
            po.height = float4(s, 1, 1);

            po.normal.x = -s.y * k.x;
            po.normal.y = s.x * k.x;
            po.normal.z = -s.y * k.y;
            po.normal.w = s.x * k.y;

            k = normalize(k);
            if(length(frac(vo.uv)) < 0.0002) k = 0.707107;

            po.displacement.x = s.y * k.x;
            po.displacement.y = -s.x * k.x;
            po.displacement.z = s.y * k.y;
            po.displacement.w = -s.x * k.y;

            return po;
        }

        float4 animateNormal(VertexOutput vo) : SV_Target
        {
            float2 k;
            float2 s = Spectrum(vo.uv, vo.uv1, k);

            float4 normal;
            normal.x = -s.y * k.x;
            normal.y = s.x * k.x;
            normal.z = -s.y * k.y;
            normal.w = s.x * k.y;

            return normal;
        }

        float2 _WindDirection;
        float _Directionality;
        float3 _WeatherSystemOffset;
        float2 _WeatherSystemRadius;

        /// Converts omnidirectional spectrum to a directional one
        float4 directionalSpectrumComplex(VertexOutput vo) : SV_Target
        {
            float3 spectrum = tex2D(_MainTex, vo.uv1.xy);
            float tileSizeInv = tex2D(_TileSizeLookup, step(float2(0.5, 0.5), vo.uv1.xy)).z;

            float pix2 = 6.2831853;
            float2 centeredUV = -0.5 + frac(vo.uv.xy + 0.5);
            float2 k = pix2 * _TargetResolution.xy * tileSizeInv * centeredUV;

            float2 nk = normalize(k);
            if(length(frac(vo.uv)) < 0.0002)
            {
                //nk = _WindDirection.xy;
                return float4(0.0, 0.0, 0.0, 1.0);
            }

            float dp = _WindDirection.x * nk.x + _WindDirection.y * nk.y;
            float directionalFactor = sqrt(1.0f + spectrum.z * (2.0f * dp * dp - 1.0f));

            if(dp < 0)
                directionalFactor *= _Directionality;

            // -- distant weather systems
            float distance = _WeatherSystemOffset.z;		// z == sqrt(x*x + y*y)
            float U10 = 14.0f;
            float omegac = 0.84f * pow(tanh(pow(distance / 22000.0f, 0.4f)), -0.75f);
            float kp = _Gravity * pow(omegac / U10, 2);

            float ks = length(k);
            float b = -2.0f * nk.x * _WeatherSystemOffset.x + -2.0f * nk.y * _WeatherSystemOffset.y;
            float c = _WeatherSystemOffset.x * _WeatherSystemOffset.x + _WeatherSystemOffset.y * _WeatherSystemOffset.y - _WeatherSystemRadius.y;

            float sqrtarg = b * b - 4.0f * c;

            if(sqrtarg < 0.0f)
            {
                // if that wave wouldn't reach this place
                return float4(0.0, 0.0, 0.0, 1.0);
            }

            float sqrt1 = sqrt(sqrtarg);
            float t1 = (sqrt1 - b) * 0.5f;
            float t2 = (-sqrt1 - b) * 0.5f;

            if(t1 > 0.0f && t2 > 0.0f)
            {
                // if it's in a wrong direction
                return float4(0.0, 0.0, 0.0, 1.0);
            }

            float2 intersection1 = nk.xy * t1;
            float2 intersection2 = nk.xy * t2;
            float angularFactor = length(intersection1 - intersection2) / _WeatherSystemRadius.x;

            float dist = min(-t1, -t2);

            if(t1 * t2 <= 0.0)
                dist = 0.0;

            float dissipationFactor = exp(-1e-6 * dist * pow(ks / kp, 2));

            return float4(spectrum.xy * directionalFactor * angularFactor * dissipationFactor * _Weight, 0.0, 1.0);
        }

            float4 directionalSpectrumSimple(VertexOutput vo) : SV_Target
        {
            float3 spectrum = tex2D(_MainTex, vo.uv1.xy);
            float tileSizeInv = tex2D(_TileSizeLookup, step(float2(0.5, 0.5), vo.uv1.xy)).z;

            float pix2 = 6.2831853;
            float2 centeredUV = -0.5 + frac(vo.uv.xy + 0.5);
            float2 k = pix2 * _TargetResolution.xy * tileSizeInv * centeredUV;

            k = normalize(k);
            if(length(frac(vo.uv)) < 0.0002) k = _WindDirection.xy;

            float dp = _WindDirection.x * k.x + _WindDirection.y * k.y;
            float directionalFactor = sqrt(1.0f + spectrum.z * (2.0f * dp * dp - 1.0f));

            if(dp < 0)
                directionalFactor *= _Directionality;

            return float4(spectrum.xy * directionalFactor, 0.0, 1.0);
        }

            struct VertexOutput2
        {
            float4 pos	: SV_POSITION;
            float2 uv	: TEXCOORD0;
        };

        VertexOutput2 vertSimple(VertexInput vi)
        {
            VertexOutput2 vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv = vi.uv0;

            return vo;
        }

        float4 addSpectrum(VertexOutput2 vo) : SV_Target
        {
            return tex2D(_MainTex, vo.uv.xy) * _Weight;
        }

            /*
             * Add low res spectrum.
             */

            struct VertexInputLowRes
        {
            float4 vertex	: POSITION;
            float3 normal	: NORMAL;
            float2 uv0		: TEXCOORD0;
        };

        half _ResolutionRatio;

        VertexOutput2 vertAddLowResSpectrum(VertexInputLowRes vi)
        {
            VertexOutput2 vo;

            float3 vertex = vi.vertex;
            float3 origin = vi.normal;

            //vo.pos = mul(UNITY_MATRIX_MVP, float4(lerp(vertex, origin, _ResolutionRatio), 1.0));
            vo.pos = float4(lerp(origin, vertex, _ResolutionRatio), 1.0);
            vo.uv = vi.uv0;

            //vo.pos = float4(vertex, 0.1);

            #if UNITY_UV_STARTS_AT_TOP
            vo.uv.y = 1.0 - vo.uv.y;
            #endif

            return vo;
        }

        float4 fragAddLowResSpectrum(VertexOutput2 vo) : SV_Target
        {
            return tex2D(_MainTex, vo.uv.xy);
        }

            ENDCG

            SubShader
        {
            Cull Off ZWrite Off ZTest Always

                Pass
            {
                Name "Spectrum"

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment animate

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Animate Normal"

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment animateNormal

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Spectrumx3"

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vert
                #pragma fragment animatex3

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Directional Spectrum (Simple)"

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment directionalSpectrumSimple

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Add Spectrum"

                Blend One One

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vertSimple
                #pragma fragment addSpectrum

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Animate Displacements"

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vert
                #pragma fragment animateDisplacements

                #pragma multi_compile __ _LOOPING

                ENDCG
            }

                Pass
            {
                Name "Directional Spectrum (Complex)"
                Blend One One
                BlendOp Add
                Cull Off

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vert
                #pragma fragment directionalSpectrumComplex

                #pragma multi_compile __ _LOOPING

                ENDCG
            }
        }
}