// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/IME/Water Drops Mask" 
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Fadeout("Fade-out", Float) = 0.98
        _Fadein("Fade-in", Float) = 0.03
    }

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "../Includes/Common.cginc"

    sampler2D _MainTex;
    sampler2D _UnderwaterMask;

    float _Fadeout;
    float _Fadein;

    struct appdata_t
    {
        float4 vertex : POSITION;
        half2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        half2 uv : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        const float previous = tex2D(_MainTex, i.uv).r;
        const float current = tex2D(_UnderwaterMask, i.uv).r;

        const half mask = previous * _Fadeout + current * _Fadein;
        return min(1.0, mask);
    }
    ENDCG

    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

    Fallback Off
}