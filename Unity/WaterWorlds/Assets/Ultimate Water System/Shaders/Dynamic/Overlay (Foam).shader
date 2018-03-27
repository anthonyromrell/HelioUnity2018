// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Water/Overlay/Foam"
{
    Properties
    {
        _MainTex("Simulation Matrix", 2D) = "black" {}
        _Amplitude("Amplitude", Float) = 1
    }
        SubShader
        {
            Pass
            {
                Blend SrcAlpha One

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

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
                    float4 nuv    : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;

                float _Amplitude;
                float _Spread;
                float _Multiplier;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.nuv = v.uv;
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    const float value = saturate(tex2D(_MainTex, i.uv - float2(0.0f, 0.0f * _Time.x * 2.4f)).r);
                    return  _Amplitude * value * (1.0f - sin(2.0f * abs(i.nuv.y - 0.5f)));
                  }
                  ENDCG
              }
        }
}