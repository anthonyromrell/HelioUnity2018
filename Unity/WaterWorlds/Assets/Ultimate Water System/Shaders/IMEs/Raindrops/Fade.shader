// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Raindrops/Fade"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

        [Header(Clear strength)]
        _Value("Value", Float) = 1.0

        [Header(Modulation Parameters)]
        _Modulation("Modulation", 2D) = "black"{}
        _ModulationStrength("Strength", Float) = 0
        _ModulationScale("Scale", Float) = 1
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

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float2 mod_uv : TEXCOORD1;
                };

                sampler2D _UnderwaterMask;

                sampler2D_float _MainTex;
                sampler2D _Modulation;
                float4 _Modulation_ST;

                float _ModulationStrength;
                float _ModulationScale;
                float _Value;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.mod_uv = v.uv * _ModulationScale;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    const float underwater = tex2D(_UnderwaterMask, i.uv).r;

                    const float value = tex2D(_MainTex, i.uv);
                    const float modulation = pow(tex2D(_Modulation, i.mod_uv), 2.0f);

                    return (1.0f - underwater) * value * pow(_Value - modulation * _ModulationStrength, 1.0f + 2.0f * value);
                }
                ENDCG
            }
        }
}