// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utilities/Blur (Max)"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Offset("Offset", Float) = (0, 0, 0, 0)
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
            half2 uv0	: TEXCOORD0;
            half2 uv1	: TEXCOORD1;
            half2 uv2	: TEXCOORD2;
            half2 uv3	: TEXCOORD3;
            half2 uv4	: TEXCOORD4;
            half2 uv5	: TEXCOORD5;
            half2 uv6	: TEXCOORD6;
        };

        sampler2D _MainTex;
        sampler2D _OriginalTex;
        half2 _Offset;

        static const half4 _Weights[4] = {half4(0.0205,0.0205,0.0205,0.0205), half4(0.0855,0.0855,0.0855,0.0855), half4(0.232,0.232,0.232,0.232), half4(0.324,0.324,0.324,0.324)};

        inline void SetUV(out half2 uv, inout half2 current)
        {
            uv = current;
            current += _Offset;
        }

        VertexOutput vert(VertexInput vi)
        {
            VertexOutput vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);

            half2 uv = vi.uv0 - _Offset * 3.0;

            SetUV(vo.uv0, uv);
            SetUV(vo.uv1, uv);
            SetUV(vo.uv2, uv);
            SetUV(vo.uv3, uv);
            SetUV(vo.uv4, uv);
            SetUV(vo.uv5, uv);
            SetUV(vo.uv6, uv);

            return vo;
        }

        void AddSampleAt(half2 uv, inout half4 color, int index)
        {
            color += tex2D(_MainTex, uv) * _Weights[index];
        }

        half4 frag(VertexOutput vo) : SV_Target
        {
            half4 color = 0;

            AddSampleAt(vo.uv0, color, 0);
            AddSampleAt(vo.uv1, color, 1);
            AddSampleAt(vo.uv2, color, 2);
            AddSampleAt(vo.uv3, color, 3);
            AddSampleAt(vo.uv4, color, 2);
            AddSampleAt(vo.uv5, color, 1);
            AddSampleAt(vo.uv6, color, 0);

            return half4(min(tex2D(_OriginalTex, vo.uv3).x, color.x), color.yz, 1.0);
        }

            ENDCG

            SubShader
        {
            Pass
            {
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment frag

                ENDCG
            }
        }
}