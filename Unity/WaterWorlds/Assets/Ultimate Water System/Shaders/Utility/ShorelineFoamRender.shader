// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Utility/ShorelineFoamRender"
{
	Properties
	{
		_MainTex ("", 2D) = "white" {}
		_FoamRange ("", Float) = 0.3
		_FoamIntensity ("", Vector) = (0.5, 0.5, 1.0, 0.0)
		_FoamIntensityMask ("", 2D) = "white" {}
		_FoamIntensityMaskTiling ("", Float) = 1.0
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
				float4 vertex	: POSITION;
				float2 uv		: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex		: SV_POSITION;
				half2 uv			: TEXCOORD0;
				half2 uv2			: TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _FoamIntensityMask;
			sampler2D _TotalDisplacementMap;
			sampler2D _OcclusionMap;
			half4 _LocalMapsCoords;
			half _FoamRange;
			half3 _FoamIntensity;
			half _FoamIntensityMaskTiling;
			float3 _SurfaceOffset;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv2 = v.uv * _FoamIntensityMaskTiling;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half intensity = 1.0 - tex2D(_MainTex, i.uv).r;
				intensity = (intensity - (1.0 - _FoamRange)) / _FoamRange;
				half uniformFoam = pow(saturate(intensity * _FoamIntensity.x), 2.0);
				half noisyFoam = pow(saturate(intensity * _FoamIntensity.y), 2.0);
				return half4(0.0, (uniformFoam + noisyFoam * saturate(Perlin2D(i.uv2) * Cellular2D(i.uv2))) * _FoamIntensity.z, 0.0, 0.0);
			}
			ENDCG
		}
	}
}
