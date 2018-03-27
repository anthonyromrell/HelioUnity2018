// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/WaterSurfaceParticle (Foam)"
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
            Blend One One
            BlendOp Add
            ColorMask G

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

            half4 frag(v2f i) : SV_Target
            {
                half2 uv = i.pack0.xy;
                return tex2D(_FoamTexture, uv);
            }
            ENDCG
        }
    }
}