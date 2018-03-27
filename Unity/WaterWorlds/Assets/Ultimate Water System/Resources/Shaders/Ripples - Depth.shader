// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Dynamic/Depth"
{
    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
    {
        float4 vertex : POSITION;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
        float4 wpos : TEXCOORD0;
    };

    v2f vert(appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.wpos = mul(unity_ObjectToWorld, v.vertex);
        return o;
    }

    float4 Velocity;
    float _WaterHeight;

    float4 frag(v2f i) : SV_Target
    {
        return i.wpos.y;
    }
        ENDCG

        SubShader
    {
        Cull Off
            Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}