// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utility/ShorelineMaskGenerate"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE
		#include "UnityCG.cginc"
		#include "NoiseLib.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv_heightMap : TEXCOORD1;
		};

		struct v2f_dm
		{
			float4 vertex : SV_POSITION;
			half2 uv : TEXCOORD0;
			half2 uv_1 : TEXCOORD1;
			half2 uv_2 : TEXCOORD2;
			half2 uv_3 : TEXCOORD3;
			half2 uv_4 : TEXCOORD4;
			half2 uv_5 : TEXCOORD5;
			half2 uv_6 : TEXCOORD6;
			half4 uv_7 : TEXCOORD7;
		};

		float2 _ShorelineExtendRange;
		float _TerrainMinPoint;
		float _DepthScale;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			o.uv_heightMap = v.uv * (float2(1.0, 1.0) + _ShorelineExtendRange) - _ShorelineExtendRange * 0.5;
			return o;
		}

		v2f vertFull(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			o.uv_heightMap = v.uv;
			return o;
		}

		sampler2D _MainTex;
		sampler2D _DistanceMap;
		half4 _MainTex_TexelSize;
		float _Offset1;
		float _Offset2;
		float _Steepness;
		float _Lod;
		half _GenerateUnderwaterAreas;

		v2f_dm vertDistanceMapStep(appdata v)
		{
			v2f_dm o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			o.uv_1 = v.uv + float2(_MainTex_TexelSize.x, 0.0);
			o.uv_2 = v.uv + float2(-_MainTex_TexelSize.x, 0.0);
			o.uv_3 = v.uv + float2(0.0, _MainTex_TexelSize.y);
			o.uv_4 = v.uv + float2(0.0, -_MainTex_TexelSize.y);
			o.uv_5 = v.uv + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
			o.uv_6 = v.uv + float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y);
			o.uv_7.xy = v.uv + float2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y);
			o.uv_7.zw = v.uv + float2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y);
			return o;
		}

		float4 fragDistanceMapStart(v2f i) : SV_Target
		{
			float horg = tex2D(_MainTex, i.uv_heightMap).x;
			float h = max(0, -(horg/* + _TerrainMinPoint*/));

			if (i.uv_heightMap.x < 0 || i.uv_heightMap.y < 0 || i.uv_heightMap.x > 1 || i.uv_heightMap.y > 1)
				h = 400;

			return h < 0.02 ? 0 : 1;
		}

		float4 fragDistanceMapStep(v2f_dm i) : SV_Target
		{
			float distance = tex2D(_MainTex, i.uv).x;

			float distance2 = tex2D(_MainTex, i.uv_1).x;
			distance2 = min(distance2, tex2D(_MainTex, i.uv_2).x);
			distance2 = min(distance2, tex2D(_MainTex, i.uv_3).x);
			distance2 = min(distance2, tex2D(_MainTex, i.uv_4).x);

			distance = min(distance, distance2 + _Offset1);

			distance2 = tex2D(_MainTex, i.uv_5).x;
			distance2 = min(distance2, tex2D(_MainTex, i.uv_6).x);
			distance2 = min(distance2, tex2D(_MainTex, i.uv_7.xy).x);
			distance2 = min(distance2, tex2D(_MainTex, i.uv_7.zw).x);

			distance = min(distance, distance2 + _Offset2);

			return distance;
		}

		float4 copyHeightMap(v2f i) : SV_Target
		{
			float horg = tex2D(_MainTex, i.uv_heightMap).x;
			float h = max(0, -(horg/* + _TerrainMinPoint*/));
			float distanceToLand = tex2D(_DistanceMap, i.uv);

			if (i.uv_heightMap.x < 0 || i.uv_heightMap.y < 0 || i.uv_heightMap.x > 1 || i.uv_heightMap.y > 1)
				h = 1000;

			float perlin = 0.0;
			float p = 0.5;
			float f = 1;

			for (int x = 0; x < 5; ++x)
			{
				perlin += Perlin3D(float3(i.uv_heightMap * f, 6.123412)) * p;
				p *= 0.5;
				f *= 2.0;
			}

			if (_GenerateUnderwaterAreas > 0.5)
			{
				h = distanceToLand * _Steepness;
				h += h * min(1, distanceToLand * 3) * perlin;
			}

			return max(0, h * _DepthScale);
		}

		float4 heightMapToMask(v2f i) : SV_Target
		{
			half h = tex2D(_MainTex, i.uv).x;
			half mask = sqrt(tanh(h * 0.01));

			//half box = pow(min(1, length(i.uv * 2 - 1)), 4);
			//mask = lerp(saturate(mask), 1.0, box);

			return mask;
		}
	ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment copyHeightMap

			#pragma target 3.0
			#pragma exclude_renderers opengl

			ENDCG
		}

		Pass
		{
			CGPROGRAM

			#pragma vertex vertFull
			#pragma fragment heightMapToMask

			#pragma target 3.0
			#pragma exclude_renderers opengl

			ENDCG
		}

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment fragDistanceMapStart

			#pragma target 3.0
			#pragma exclude_renderers opengl

			ENDCG
		}

		Pass
		{
			CGPROGRAM

			#pragma vertex vertDistanceMapStep
			#pragma fragment fragDistanceMapStep

			#pragma target 3.0
			#pragma exclude_renderers opengl

			ENDCG
		}
	}
}
