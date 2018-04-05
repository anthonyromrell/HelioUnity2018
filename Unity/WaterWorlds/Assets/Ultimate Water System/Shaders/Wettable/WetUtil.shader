Shader "UltimateWater/Utility/Wetness Update"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "CustomType" = "Wettable" }
        LOD 100
        Blend One SrcAlpha
        Cull Off

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
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 localUv : TEXCOORD0;
                half worldPosY : TEXCOORD1;
            };

            sampler2D _TotalDisplacementMap;
            float4 _LocalMapsCoords;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = float4(v.uv2 * 2.0 - 1.0, 0.0, 1.0);
                o.vertex.y = -o.vertex.y;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldPosY = worldPos.y;
                o.localUv = worldPos.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half waterPosY = tex2D(_TotalDisplacementMap, i.localUv).y;
                return half4(saturate((waterPosY - i.worldPosY - 0.015) * 4.0), 0.0, 0.0, 0.995);
            }
            ENDCG
        }
    }
}
