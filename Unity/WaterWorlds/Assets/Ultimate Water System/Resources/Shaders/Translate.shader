// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Dynamic/Translate"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Offset("Offset", Vector) = (0.0, 0.0, 0.0, 0.0)
    }
        SubShader
        {
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
                };

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float4 _Offset;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    return tex2D(_MainTex, i.uv + _Offset * _MainTex_TexelSize.xy);
                }
                ENDCG
            }
        }
}