// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "UltimateWater/Utility/HeightMapperAlt"
{
    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
    {
        float4 vertex	: POSITION;
    };

    struct v2f
    {
        float4 vertex	: SV_POSITION;
        float worldPosY : TEXCOORD0;
    };

    v2f vertBottom(appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.worldPosY = mul(unity_ObjectToWorld, v.vertex).y;
        o.vertex.z = -o.worldPosY * _ProjectionParams.w;
        return o;
    }

    v2f vertTop(appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.worldPosY = mul(unity_ObjectToWorld, v.vertex).y;
        o.vertex.z = o.worldPosY * _ProjectionParams.w;
        return o;
    }

    float4 fragBottom(v2f i) : SV_Target
    {
        return -100;
    }

        float4 fragTop(v2f i) : SV_Target
    {
        return i.worldPosY;
    }
        ENDCG

        SubShader
    {
        Tags{"RenderType" = "Opaque"}

            Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            ColorMask 0

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vertBottom
            #pragma fragment fragBottom

            ENDCG
        }

            Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            BlendOp Max
            Blend One One

            Stencil
            {
                Ref 1
                Comp Equal
            }

            CGPROGRAM
            #pragma vertex vertTop
            #pragma fragment fragTop

            ENDCG
        }

            Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off
            ColorMask 0

            Stencil
            {
                Comp Always
                Pass Zero
            }

            CGPROGRAM
            #pragma vertex vertBottom
            #pragma fragment fragBottom

            ENDCG
        }
    }
}