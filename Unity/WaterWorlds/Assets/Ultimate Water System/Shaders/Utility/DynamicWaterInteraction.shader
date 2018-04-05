// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Utility/DynamicWaterInteraction"
{
    Properties
    {
        _MainTex("", 2D) = "white" {}
        _FoamRange("", Float) = 0.3
        _FoamIntensity("", Vector) = (0.5, 0.5, 1.0, 0.0)
        _FoamIntensityMask("", 2D) = "white" {}
        _FoamIntensityMaskTiling("", Float) = 1.0
    }
        SubShader
        {
            Pass
            {
                Blend One One
                Cull Off
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "../Includes/UnityVersionsCompatibility.cginc"
                #include "../Utility/NoiseLib.cginc"
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 vertex		: SV_POSITION;
                    half2 intensity		: TEXCOORD0;
                    half2 uv			: TEXCOORD1;
                    float4 occlusionUv	: TEXCOORD2;
                };

                sampler2D _FoamIntensityMask;
                sampler2D _TotalDisplacementMap;
                sampler2D _OcclusionMap;
                half4 _LocalMapsCoords;
                half _FoamRange;
                half3 _FoamIntensity;
                half _FoamIntensityMaskTiling;
                float3 _SurfaceOffset;
                float4x4 _OcclusionMapProjection;

                v2f vert(appdata v)
                {
                    v2f o;
                    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                    float3 normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    worldPos.xyz += normal * _FoamRange;

                    o.occlusionUv = mul(_OcclusionMapProjection, worldPos);

                    worldPos.y = 0.0;

                    o.uv = (worldPos.xz + _SurfaceOffset.xz) * _FoamIntensityMaskTiling;

                    half2 localUv = worldPos.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                    float3 displacement = tex2Dlod(_TotalDisplacementMap, half4(localUv, 0.0, 0.0));
                    worldPos.xz -= displacement.xz;

                    o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                    o.intensity = pow(abs(normal.yy), float2(16, 4)) * _FoamIntensity.xy;

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    return half4(0.0, tex2D(_OcclusionMap, i.occlusionUv.xy / i.occlusionUv.w * 0.5 + 0.5).r * saturate(i.intensity.x + Perlin2D(i.uv) * Cellular2D(i.uv) /*tex2D(_FoamIntensityMask, i.uv)*/ * i.intensity.y).r * _FoamIntensity.z, 0.0, 0.0);
                }
                ENDCG
            }

            Pass
            {
                Blend One One
                Cull Off
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "../Includes/UnityVersionsCompatibility.cginc"
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 vertex		: SV_POSITION;
                    half worldPosY		: TEXCOORD0;
                };

                sampler2D _FoamIntensityMask;
                sampler2D _TotalDisplacementMap;
                half4 _LocalMapsCoords;
                half _FoamRange;
                half3 _FoamIntensity;
                half _FoamIntensityMaskTiling;

                v2f vert(appdata v)
                {
                    v2f o;
                    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                    float3 normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    worldPos.xyz += normal * _FoamRange;

                    o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                    o.worldPosY = worldPos.y;

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    return half4(i.worldPosY >= 0.0 ? half2(1.0 / 255.0, 0.0) : half2(0.0, 1.0 / 255.0), 0.0, 0.0);
                }
                ENDCG
            }

            Pass
            {
                Cull Off
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "../Includes/UnityVersionsCompatibility.cginc"
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    half2 uv0 : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex		: SV_POSITION;
                    half2 uv			: TEXCOORD0;
                };

                sampler2D _MainTex;
                half4 _LocalMapsCoords;
                half _FoamRange;
                half3 _FoamIntensity;
                half _FoamIntensityMaskTiling;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv0;

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    return dot(tex2D(_MainTex, i.uv).rg * 255 % 2, 1) > 0 ? 1 : 0;
                }
                ENDCG
            }

            Pass
            {
                Blend One One
                Cull Off
                ZTest Always

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "../Includes/UnityVersionsCompatibility.cginc"
                #include "../Utility/NoiseLib.cginc"
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 vertex		: SV_POSITION;
                    half2 intensity		: TEXCOORD0;
                    half2 uv			: TEXCOORD1;
                    float4 occlusionUv	: TEXCOORD2;
                };

                sampler2D _FoamIntensityMask;
                sampler2D _TotalDisplacementMap;
                sampler2D _OcclusionMap;
                half4 _LocalMapsCoords;
                half _FoamRange;
                half3 _FoamIntensity;
                half _FoamIntensityMaskTiling;
                float3 _SurfaceOffset;
                float4x4 _OcclusionMapProjection;

                v2f vert(appdata v)
                {
                    v2f o;
                    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

                    float3 normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    worldPos.xyz += normal * _FoamRange;

                    o.occlusionUv = mul(_OcclusionMapProjection, worldPos);

                    worldPos.y = 0.0;

                    o.uv = (worldPos.xz + _SurfaceOffset.xz) * _FoamIntensityMaskTiling;

                    half2 localUv = worldPos.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;
                    float3 displacement = tex2Dlod(_TotalDisplacementMap, half4(localUv, 0.0, 0.0));
                    worldPos.xz -= displacement.xz;

                    o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                    o.intensity = pow(abs(normal.yy), float2(16, 4)) * _FoamIntensity.xy;

                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    return half4(0.0, saturate(i.intensity.x + Perlin2D(i.uv) * Cellular2D(i.uv) /*tex2D(_FoamIntensityMask, i.uv)*/ * i.intensity.y).r * _FoamIntensity.z, 0.0, 0.0);
                }
                ENDCG
            }
        }
}
