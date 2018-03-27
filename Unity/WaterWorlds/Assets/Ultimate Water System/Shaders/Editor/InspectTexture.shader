// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Editor/Inspect Texture"
{
    Properties
    {
        _MainTex("", 2D) = "black" {}
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
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float2 _RangeR;
            float2 _RangeG;
            float2 _RangeB;

            float4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);

                color.r = (color.r - _RangeR.x) * (_RangeR.y);
                color.g = (color.g - _RangeG.x) * (_RangeG.y);
                color.b = (color.b - _RangeB.x) * (_RangeB.y);

                return color;
            }
            ENDCG
        }
    }
}