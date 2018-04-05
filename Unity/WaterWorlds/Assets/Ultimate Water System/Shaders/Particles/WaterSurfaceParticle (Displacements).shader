// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/WaterSurfaceParticle (Displacements)"
{
    Properties
    {
        _Amplitude("Amplitude", Float) = 1
        _FoamTexture("Foam", 2D) = "black" {}
    }
        SubShader
    {
        Pass
        {
            BlendOp Add
            Blend One One

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex			: POSITION;
                half4 color				: COLOR;
                float4 uv				: TEXCOORD0;
                float4 velocityAndSize	: TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex	: SV_POSITION;
                float4 pack0	: TEXCOORD0;
                float2 pack1	: TEXCOORD1;
            };

            struct WaterBufferOutputs
            {
                float4 displacement		: SV_Target0;
                float2 normal			: SV_Target1;
                float4 displacementMask	: SV_Target2;
            };

            sampler2D _FoamTexture;
            float4 _FoamTexture_ST;
            float _Amplitude;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pack0.xy = v.uv.xy * UNITY_PI;
                o.pack0.zw = normalize(v.velocityAndSize.xz);
                o.pack1.x = 1.0 / v.velocityAndSize.w;
                o.pack1.y = v.color.x;
                return o;
            }

            WaterBufferOutputs frag(v2f i)
            {
                half2 uv = i.pack0.xy;
                half2 direction = i.pack0.zw;

                half sizeInv = i.pack1.x;
                half intensity = i.pack1.y;

                half2 s, c;
                sincos(uv.xy, s, c);

                half box = s.x * s.x * s.y * s.y * intensity;
                half2 displacement = box * s.x * c.y * direction * _Amplitude * 2;
                half2 normal = displacement * sizeInv;

                WaterBufferOutputs outputs;
                outputs.displacement = half4(0.0f, _Amplitude * box, 0.0f, 0);
                outputs.normal = half4(0, 0, 0, 0) * box;
                outputs.displacementMask = float4(1.0, 1.0, 1.0, 1.0);
                return outputs;
            }
            ENDCG
        }
    }
}