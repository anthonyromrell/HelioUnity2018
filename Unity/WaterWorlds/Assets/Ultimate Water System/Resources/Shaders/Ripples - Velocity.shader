// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Dynamic/Velocity"
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
        float3 velocity : TEXCOORD1;
    };

    float4x4 _PreviousWorld;
    float4x4 _CurrentWorld;

    float  _Data; // [multiplier / deltaTime]
    float _WaterHeight;

    v2f vert(appdata v)
    {
        float4 previous = mul(_PreviousWorld, v.vertex);
        float4 current = mul(_CurrentWorld, v.vertex);

        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.wpos = current;
        o.velocity = (current.xyz - previous.xyz) * _Data;

        return o;
    }

    float4 frag(v2f i) : SV_Target
    {
        if(i.wpos.y > _WaterHeight + 0.02f)
        {
            discard;
        }
        return float4(i.velocity, i.wpos.y);
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