// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Utility/Wetness Update (Near Camera)"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "CustomType" = "Wettable" }
        Cull Off
        ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
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
                half2 localUv		: TEXCOORD0;
                half2 localUvPrev	: TEXCOORD1;
                half worldPosY		: TEXCOORD2;
                half2 uv			: TEXCOORD3;
            };

            sampler2D _WetnessMapPrevious;
            sampler2D _TotalDisplacementMap;
            float4 _LocalMapsCoords;
            float4 _LocalMapsCoordsPrevious;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPosY = worldPos.y;
                o.localUv = worldPos.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                o.localUvPrev = worldPos.xz * _LocalMapsCoordsPrevious.zz + _LocalMapsCoordsPrevious.xy;
                o.uv = o.vertex.xy;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half previous = tex2D(_WetnessMapPrevious, i.localUvPrev);
                half waterPosY = tex2D(_TotalDisplacementMap, i.localUv).y;
                return half4((previous * 0.995 + saturate((waterPosY - i.worldPosY - 0.015) * 4.0)) * (1.0 - pow(min(1.0, length(i.uv) * 1.05), 4)), 0.0, 0.0, 0.0);
            }
            ENDCG
        }
    }
}
