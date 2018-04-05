// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Volumes/Front"
{
    Properties
    {
        _WaterId("", Vector) = (2, 1, 0, 0)
        _WaterStencilId("", Float) = 0
        _WaterStencilIdInv("", Float) = 0
    }

        SubShader
    {
        Tags{ "CustomType" = "WaterVolume" }

        Pass
        {
            Stencil
            {
                Ref[_WaterStencilIdInv]
                Comp Always
                WriteMask 255
                Pass Replace
                Fail Invert
                ZFail Invert
            }

            Cull Back
            ZTest Less
            ZWrite On
            ColorMask RB

            CGPROGRAM
            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex	: SV_POSITION;
                float2 depth	: TEXCOORD0;
            };

            float2 _WaterId;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = o.vertex.zw;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(_WaterId.x, 0.0, i.depth.x / i.depth.y, 0.0);
            }
            ENDCG
        }

        Pass
        {
            Stencil
            {
                Ref[_WaterStencilId]
                Comp Equal			// if 128 bit is not set
                ReadMask 128
                WriteMask 255
                Pass Replace
                Fail Invert
                ZFail Invert
            }

            Cull Front
            ZTest Always
            ZWrite Off
            ColorMask RB

            CGPROGRAM
            #pragma target 2.0

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex	: SV_POSITION;
            };

            float2 _WaterId;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
#if UNITY_VERSION >= 550
                return float4(_WaterId.x, 0.0, 1.0, 0.0);
#else
                return float4(_WaterId.x, 0.0, 0.0, 0.0);
#endif
            }
            ENDCG
        }
    }
}