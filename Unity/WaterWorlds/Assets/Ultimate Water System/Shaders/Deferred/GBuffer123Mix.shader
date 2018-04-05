Shader "UltimateWater/Deferred/GBuffer123Mix"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		half4 uv : TEXCOORD0;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = v.vertex;
#if UNITY_UV_STARTS_AT_TOP
		o.vertex.y = -o.vertex.y;
#endif
		o.uv = half4(v.uv, 0, 0);
		return o;
	}

	sampler2D _CameraGBufferTexture0;
	sampler2D _CameraGBufferTexture1;
	sampler2D _CameraGBufferTexture2;
	sampler2D _CameraReflectionsTexture;
	sampler2D _CameraGBufferTextureOriginal0;
	sampler2D _CameraGBufferTextureOriginal1;
	sampler2D _CameraGBufferTextureOriginal2;
	sampler2D _CameraGBufferTextureOriginal3;
	sampler2D _WaterlessDepthTexture;
	sampler2D _WaterDepthTexture;
	sampler2D _CameraDepthTexture;
	sampler2D _GlobalWaterLookupTex;
	half _DepthClipMultiplier;

	void frag(
		v2f i,
		//out half4 outDiffuse : SV_Target0,			// RT0: diffuse color (rgb), occlusion (a)
		out half4 outSpecSmoothness : SV_Target0,	// RT1: spec color (rgb), smoothness (a)
		out half4 outNormal : SV_Target1,			// RT2: normal (rgb), --unused, very low precision-- (a) 
		out half4 outEmission : SV_Target2)			// RT3: emission (rgb), --unused-- (a)) : SV_Target
	{
		half4 uv = UnityStereoTransformScreenSpaceTex(i.uv);
		half waterDepth = tex2Dlod(_CameraDepthTexture, uv);
		half waterlessDepth = tex2Dlod(_WaterlessDepthTexture, uv);

#if defined(UNITY_REVERSED_Z)
		//clip(waterlessDepth - waterDepth + 0.00001);
#else
		//clip((waterlessDepth - waterDepth) * _DepthClipMultiplier - 0.000001);
#endif

		//outDiffuse = tex2Dlod(_CameraGBufferTextureOriginal0, uv);
		outSpecSmoothness = tex2Dlod(_CameraGBufferTextureOriginal1, uv);
		outNormal = tex2Dlod(_CameraGBufferTextureOriginal2, uv);
		outEmission = tex2Dlod(_CameraGBufferTextureOriginal3, uv);

		half blendEdgesFactor = saturate(5 * (LinearEyeDepth(waterlessDepth) - LinearEyeDepth(waterDepth)));
		outSpecSmoothness.a = blendEdgesFactor;
		outNormal.a = blendEdgesFactor;
		outEmission.a = blendEdgesFactor;
	}
ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Blend OneMinusSrcAlpha SrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma target 3.0
			
			ENDCG
		}
	}
}
