// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Deferred/GBuffer0Mix"
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

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = half4(v.uv, 0, 0);
		return o;
	}

	half4 frag(v2f i) : SV_Target
	{
		half4 uv = UnityStereoTransformScreenSpaceTex(i.uv);
		half waterDepth = tex2Dlod(_CameraDepthTexture, uv);
		half waterlessDepth = tex2Dlod(_WaterlessDepthTexture, uv);

		UNITY_BRANCH
#if defined(UNITY_REVERSED_Z)
		if (waterDepth > waterlessDepth)
#else
		if (waterDepth < waterlessDepth)
#endif
		{
			half4 gbuffer0 = tex2Dlod(_CameraGBufferTexture0, uv);
			half3 pack0 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.875, 0, 0));
			gbuffer0.r = pack0.r * gbuffer0.g / pack0.g;
			gbuffer0.a = 1.0;
			return gbuffer0;
		}
		else
			return tex2Dlod(_CameraGBufferTextureOriginal0, uv);

		/*half blendEdgesFactor = saturate(1000 * (waterDepth - waterlessDepth));
		half4 gbuffer0 = tex2Dlod(_CameraGBufferTexture0, uv);
		half3 pack0 = tex2Dlod(_GlobalWaterLookupTex, half4(gbuffer0.a, 0.875, 0, 0));
		gbuffer0.r = pack0.r * gbuffer0.g / pack0.g;
		gbuffer0.a = 1.0;
		return lerp(tex2Dlod(_CameraGBufferTextureOriginal0, uv), gbuffer0, blendEdgesFactor);*/
	}
ENDCG

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma target 3.0
			
			ENDCG
		}
	}
}
