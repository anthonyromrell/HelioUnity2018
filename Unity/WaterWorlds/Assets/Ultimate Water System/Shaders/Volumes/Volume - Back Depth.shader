// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Volumes/Back"
{
    Properties
    {
        _WaterStencilId("", Float) = 0
    }

        SubShader
    {
        Tags{ "CustomType" = "WaterVolume" }

        Pass
        {
            Stencil
            {
                Ref[_WaterStencilId]
                Comp Equal
            }

            Cull Front
            ZTest Always
            ZWrite Off
            ColorMask G
            Blend One One
            BlendOp Max

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
                float4 vertex		: SV_POSITION;
                float2 depth		: TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = o.vertex.zw;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(0.0, i.depth.x / i.depth.y, 0.0, 0.0);
            }
            ENDCG
        }
    }
}