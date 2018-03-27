// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Raindrops/Final"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _WaterDropsTex("Water Drops", 2D) = "black" {}
        _NormalSpread("Normal Spread", Float) = 0.01
        _Distortion("Distortion", Float) = 1.0
        _Enviro("Enviro", Float) = 1.0
        _EnviroFalloff("Enviro Falloff", Float) = 1.0
        _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Twirl("Twirl", 2D) = "black" {}
        _TwirlMultiplier("Twirl multiplier", Float) = 1.0
    }
        SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"
            #include "UnityGlobalIllumination.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            UNITY_DECLARE_TEXCUBE(unity_Spec);

            sampler2D _MainTex;
            sampler2D_float _WaterDropsTex;

            float _NormalSpread;
            float _Distortion;
            float _Enviro;
            float _EnviroFalloff;
            sampler2D _Twirl;
            float _TwirlMultiplier;

            float4 _Color;

            float4 frag(v2f i) : SV_Target
            {
                float4 twirl = tex2D(_Twirl, i.uv * 1.0f);

                float2 uv = i.uv + (twirl.xx - 0.5f) * 0.04f * _TwirlMultiplier;
                #if !UNITY_UV_STARTS_AT_TOP
                uv.y = 1.0 - uv.y;
                #endif

                const float center = tex2D(_WaterDropsTex, uv).r;
                if(center <= 0.001f)
                {
                    return tex2D(_MainTex, i.uv);
                }

                float right = tex2D(_WaterDropsTex, uv + float2(_NormalSpread, 0.0f)).r;
                float left = tex2D(_WaterDropsTex, uv - float2(_NormalSpread, 0.0f)).r;
                float top = tex2D(_WaterDropsTex, uv + float2(0.0f, _NormalSpread)).r;
                float bottom = tex2D(_WaterDropsTex, uv - float2(0.0f, _NormalSpread)).r;

                float ndx = (right - left) / _NormalSpread;
                float ndy = (top - bottom) / _NormalSpread;
                float ndz = sqrt(ndx * ndx + ndy * ndy) / sqrt(2);

                float2 lens = float2(ndx, ndy) * _Distortion * 0.001f;

                Unity_GlossyEnvironmentData envData;
                envData.roughness = 0.0f;
                envData.reflUVW = float3(right - left, top - bottom, center * 0.1f);

                float4 value = UNITY_SAMPLE_TEXCUBE_LOD(
                    unity_Spec, envData.reflUVW, 100.0f * UNITY_SPECCUBE_LOD_STEPS
                );

                float4 source = tex2D(_MainTex, i.uv + lens);

                float factor = saturate(1.0f - pow(_EnviroFalloff * center, 0.2)) * _Enviro * length(lens);

                return _Color * source * (1.0f + factor * 0.5f * (value - 0.5f));
            }
            ENDCG
        }
    }
}