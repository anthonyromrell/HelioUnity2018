// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utilities/Water Mask As Underwater"
{
	Properties
	{
		_IntensityInv("Intensity Invert", Float) = 0.0
		_WaterId ("", Float) = 128
	}

	CGINCLUDE
	
	#include "UnityCG.cginc"

	struct VertexInput
	{
		float4 vertex	: POSITION;
	};

	struct VertexOutput
	{
		float4 pos	: SV_POSITION;
	};

	half _IntensityInv;
	float _WaterId;

	VertexOutput vert (VertexInput vi)
	{
		VertexOutput vo;
		vo.pos = UnityObjectToClipPos(vi.vertex);

		return vo;
	}

	float4 frag(VertexOutput vo) : SV_Target
	{
#if defined(UNITY_REVERSED_Z)
		return float4(_WaterId, 0, 800000, _IntensityInv);
#else
		return float4(_WaterId, 800000, 0, _IntensityInv);
#endif
	}

	ENDCG

	SubShader
	{
		Tags { "RenderType"="Transparent" "PerformanceChecks"="False" "Queue"="Transparent" }

		Pass
		{
			ZTest Always Cull Off ZWrite On
			Blend Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
	}
}