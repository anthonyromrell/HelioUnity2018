// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/IME/Water Drops" {
    Properties{
        _MainTex("", 2D) = "" {}
        _NormalMap("Overlay", 2D) = "" {}
        _Intensity("Intensity", Float) = 0.5
    }

        CGINCLUDE

            #include "UnityCG.cginc"
            #include "../Includes/Common.cginc"

            sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _Mask;
        sampler2D _UnderwaterMask;
        sampler2D _SubtractiveMask;

        half _Intensity;

        struct appdata_t
        {
            float4 vertex : POSITION;
            half2 texcoord : TEXCOORD0;
        };

        struct v2f_mask
        {
            float4 vertex : SV_POSITION;
            half2 uv0 : TEXCOORD0;
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
            half2 uv0 : TEXCOORD0;
            half2 uv1 : TEXCOORD1;
        };

        v2f_mask vertMask(appdata_t v)
        {
            v2f_mask o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv0 = v.texcoord.xy;
            return o;
        }

        half4 fragMask(v2f_mask i) : SV_Target
        {
            half mask = tex2D(_MainTex, i.uv0) * 0.98;
            mask += tex2D(_UnderwaterMask, i.uv0) * 0.03;

            return min(1.0, mask);
        }

            v2f vertDrops(appdata_t v)
        {
            v2f o;
            o.vertex = CustomBlitTransform(v.vertex);
            o.uv0 = v.texcoord.xy;
            o.uv1 = v.texcoord.xy;
            o.uv1.y *= max(0.85, _ScreenParams.y / _ScreenParams.x);
            return o;
        }

        fixed4 fragDrops(v2f i) : SV_Target
        {
            half2 normal = UnpackNormal(tex2D(_NormalMap, i.uv1)).xy;
            normal *= tex2D(_Mask, i.uv0).xx * (1.0 - tex2D(_UnderwaterMask, i.uv0).xx) * step(tex2D(_SubtractiveMask, i.uv0).x, 900000);

            return tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(i.uv0 + normal * _Intensity));
        }

            ENDCG

            SubShader
        {
            Pass
            {
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM
                #pragma vertex vertMask
                #pragma fragment fragMask
                ENDCG
            }

                Pass
            {
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM
                #pragma vertex vertDrops
                #pragma fragment fragDrops
                ENDCG
            }
        }

        Fallback Off
}