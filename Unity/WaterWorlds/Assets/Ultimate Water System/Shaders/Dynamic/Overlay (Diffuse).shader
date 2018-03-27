// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Water/Overlay/Diffuse"
{
    Properties
    {
        _MainTex("Texture", 2D) = "black" {}
        _Intensity("Intensity", Float) = 1.0
        _Tint("Tint", Color) = (1.0, 1.0, 1.0, 1.0)
    }
        SubShader
        {
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha

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
                };

                sampler2D _MainTex;
                float _Intensity;
                float4 _Tint;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float4 color = tex2D(_MainTex, i.uv);
                    return color * _Intensity * _Tint;
                }
                ENDCG
            }
        }
}