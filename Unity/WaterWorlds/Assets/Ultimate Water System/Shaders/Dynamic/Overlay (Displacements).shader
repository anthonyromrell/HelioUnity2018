// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Water/Overlay/Displacements"
{
    Properties
    {
        _SimulationMatrix("Simulation Matrix", 2D) = "black" {}
        _Amplitude("Amplitude", Float) = 1

        _Spread("Spread", Float) = 0.01
        _Multiplier("Multiplier", Float) = 2.0

        [MaterialToggle] _Fadeout("Fadeout", Float) = 0
        _FadePower("Fade power", Float) = 2.0
    }
        SubShader
        {
            Pass
            {
                Blend One One

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #pragma multi_compile _ _FADEOUT_ON

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    half4 color   : COLOR;
                    float4 uv     : TEXCOORD0;
                    float4 velocityAndSize : TEXCOORD1;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    float2 uv    : TEXCOORD0;
                };

                struct WaterBufferOutputs
                {
                    float4 displacement     : SV_Target0;
                    float4 normal           : SV_Target1;
                    float4 displacementMask : SV_Target2;
                };

                sampler2D _SimulationMatrix;
                float4 _SimulationMatrix_TexelSize;

                uniform float _Amplitude;
                uniform float _Spread;
                uniform float _Multiplier;

                uniform float _Fadeout;
                uniform float _FadePower;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                WaterBufferOutputs frag(v2f i)
                {
                    const float value = tex2D(_SimulationMatrix, i.uv).r;

                    const float left = tex2D(_SimulationMatrix, i.uv + float2(-_Spread, 0.0f)).r;
                    const float right = tex2D(_SimulationMatrix, i.uv + float2(+_Spread, 0.0f)).r;
                    const float top = tex2D(_SimulationMatrix, i.uv + float2(0.0f, +_Spread)).r;
                    const float bottom = tex2D(_SimulationMatrix, i.uv + float2(0.0f, -_Spread)).r;

                    WaterBufferOutputs outputs;
                    outputs.displacement = half4(0.0f, _Amplitude * value, 0.0f, 0);
                    outputs.normal = half4(right - left, top - bottom, 0, 0) * _Multiplier;
                    outputs.displacementMask = 0.0f;

                    if(_Fadeout)
                    {
                        float len = 1.0f - pow(saturate(2.0f * length(i.uv - float2(0.5f, 0.5f))), _FadePower);
                        outputs.displacement *= len;
                        outputs.normal *= len;
                    }

                    return outputs;
                  }
                  ENDCG
              }
        }
}