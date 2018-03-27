// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utility/ShorelineMaskRender"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveDampingThreshold ("", Float) = 0.0
	}
	SubShader
	{
		Tags { "CustomType"="WaterInteraction" }

		Pass
		{
			//ColorMask A
			Blend DstColor Zero
			Cull Off
			//BlendOp Min

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Includes/UnityVersionsCompatibility.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			half _WaveDampingThreshold;
			half4 _TileSizesInv;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half mask = tex2D(_MainTex, i.uv);
				half4 t = half4(min(1.0, _TileSizesInv * _WaveDampingThreshold * 0));
				return lerp(mask, min(1.0, mask * 80), t);
			}
			ENDCG
		}
	}

	SubShader
	{
		Tags { "CustomType"="GlobalWaterInteraction" }

		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Includes/UnityVersionsCompatibility.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _OffsetScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = mul(unity_ObjectToWorld, v.vertex).xz * _OffsetScale.zw + _OffsetScale.xy;
				//o.uv.y = 1.0 - o.uv.y;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}

	SubShader
	{
		Tags { "CustomType"="WaterInteractionDynamic" }

		Pass
		{
			//ColorMask A
			Blend DstColor Zero
			//BlendOp Min

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "../Includes/UnityVersionsCompatibility.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half worldPosY : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPosY = mul(unity_ObjectToWorld, v.vertex).y;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				//return 1.0 - min(1.0, exp(i.worldPosY * 0.01));
				return 1.0 - min(1.0, i.worldPosY * half4(0.5, 1.0, 0.1, 0.04));
			}
			ENDCG
		}
	}
}
