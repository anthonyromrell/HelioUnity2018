// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Utilities/FFT Utilities"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _ColorMask("", Float) = 1
    }

        CGINCLUDE

            #include "UnityCG.cginc"

            struct VertexInput
        {
            float4 vertex	: POSITION;
            float2 uv0		: TEXCOORD0;
        };

        struct VertexInput2
        {
            float4 vertex	: POSITION;
            float2 uv0		: TEXCOORD0;
            float4 uv1		: TEXCOORD1;
            float4 uv2		: TEXCOORD2;
        };

        struct VertexOutput
        {
            float4 pos	: SV_POSITION;
            half2 uv	: TEXCOORD0;
            half2 uv1	: TEXCOORD1;
            half2 uv2	: TEXCOORD2;
            half2 uv3	: TEXCOORD3;
            half2 uv4	: TEXCOORD4;
        };

        struct VertexOutput2
        {
            float4 pos	: SV_POSITION;
            half4 uv	: TEXCOORD0;
        };

        struct VertexOutput5
        {
            float4 pos	: SV_POSITION;
            half2 uv1	: TEXCOORD1;
            half2 uv2	: TEXCOORD2;
            half2 uv3	: TEXCOORD3;
            half2 uv4	: TEXCOORD4;
        };

        sampler2D _MainTex;
        float _MainTex_Texel_Size;
        float4 _Offset;
        sampler2D _SecondTex;
        half _Intensity1;
        half _Intensity2;
        half _JacobianScale;

        //
        // Displacement map to normal map
        //
        VertexOutput5 vert(VertexInput vi)
        {
            VertexOutput5 vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv1 = vi.uv0 + half2(_MainTex_Texel_Size, 0.0);
            vo.uv2 = vi.uv0 + half2(0.0, _MainTex_Texel_Size);
            vo.uv3 = vi.uv0 - half2(_MainTex_Texel_Size, 0.0);
            vo.uv4 = vi.uv0 - half2(0.0, _MainTex_Texel_Size);

            return vo;
        }

        inline half2 GetNormal(VertexOutput5 vo, sampler2D tex, half intensity)
        {
            half h10 = tex2D(tex, vo.uv1).y;
            half h01 = tex2D(tex, vo.uv2).y;
            half h20 = tex2D(tex, vo.uv3).y;
            half h02 = tex2D(tex, vo.uv4).y;

            return half2(h20 - h10, h02 - h01) * intensity;
        }

        half4 fragHeight2Normal(VertexOutput5 vo) : SV_Target
        {
            half2 normal1 = GetNormal(vo, _MainTex, _Intensity1);
            half2 normal2 = GetNormal(vo, _SecondTex, _Intensity2);

            return half4(normal1, normal2);
        }

        half _MipIndex;

        //
        // Copy FFT result to final map
        //
        VertexOutput2 vertCopy(VertexInput vi)
        {
            VertexOutput2 vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv = half4(vi.uv0 * 0.5 + _Offset.xy, 0, 0);

            return vo;
        }

        half4 fragCopy(VertexOutput2 vo) : SV_Target
        {
            //return tex2Dlod(_MainTex, vo.uv);
            return 0;
        }

        sampler2D _HeightTex;
        sampler2D _DisplacementTex;
        half2 _DisplacementTex_TexelSize;
        half _HorizontalDisplacementScale;

        half4 copyDisplacements(VertexOutput2 In) : SV_Target
        {
            half tex0 = tex2D(_HeightTex, In.uv);
            half2 tex1 = tex2D(_DisplacementTex, In.uv) * _HorizontalDisplacementScale;

            return half4(tex1.x, tex0, tex1.y, 0.0);
        }

            //
            // Copy normal to final map
            //
            struct VertexOutput4
        {
            float4 pos	: SV_POSITION;
            half2 uv1	: TEXCOORD0;
            half2 uv2	: TEXCOORD1;
        };

        VertexOutput4 vertCopyNormal(VertexInput vi)
        {
            VertexOutput4 vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv1 = vi.uv0 * 0.5 + _Offset.xy;
            vo.uv2 = vi.uv0 * 0.5 + _Offset.zw;

            return vo;
        }

        half4 fragCopyNormal(VertexOutput4 In) : SV_Target
        {
            return half4(tex2D(_MainTex, In.uv1.xy).rg, tex2D(_MainTex, In.uv2.xy).rg);
        }

            //
            // Copy displacement FFT to final map and compute jacobian into alpha channel
            //
            VertexOutput vertJacobian(VertexInput2 vi)
        {
            VertexOutput vo;

            #if UNITY_UV_STARTS_AT_TOP
            vi.uv0.y = 1.0 - vi.uv0.y;
            vi.uv1.yw = 1.0 - vi.uv1.yw;
            vi.uv2.yw = 1.0 - vi.uv2.yw;
            #endif

            vo.pos = vi.vertex;
            vo.uv = vi.uv0.xy * 0.5 + _Offset.xy;
            vo.uv1 = vi.uv1.xy * 0.5 + _Offset.xy;
            vo.uv2 = vi.uv1.zw * 0.5 + _Offset.xy;
            vo.uv3 = vi.uv2.xy * 0.5 + _Offset.xy;
            vo.uv4 = vi.uv2.zw * 0.5 + _Offset.xy;

            return vo;
        }

        half4 copyJacobian(VertexOutput In) : SV_Target
        {
            half tex0 = tex2D(_HeightTex, In.uv);
            half2 d00 = tex2D(_DisplacementTex, In.uv).xy;
            half2 d10 = tex2D(_DisplacementTex, In.uv1).xy;
            half2 d01 = tex2D(_DisplacementTex, In.uv2).xy;
            half2 d20 = tex2D(_DisplacementTex, In.uv3).xy;
            half2 d02 = tex2D(_DisplacementTex, In.uv4).xy;

            half4 diff = half4(d10 - d20, d01 - d02);
            half3 j = half3(diff.x, diff.w, diff.y) * _JacobianScale;
            j.xy += 1.0;
            half jacobian = -(j.x * j.y - j.z * j.z);

            d00 *= _HorizontalDisplacementScale;

            return half4(d00.x, tex0, d00.y, jacobian);
        }

            //
            // Displace Heightmap
            //
            struct VertexOutput3
        {
            float4 pos			: SV_POSITION;
            half height			: TEXCOORD0;
        };

        float		_WorldToPixelSpace;

        VertexOutput3 vertDisplace(VertexInput vi)
        {
            VertexOutput3 vo;

            vi.vertex.xz = vi.vertex.xz * 1.5;

            float2 fftUV = vi.vertex.xz * 0.5 + 0.5;
            float3 displacement = tex2Dlod(_MainTex, float4(fftUV, 0, 0)).xyz;

            vi.vertex.xz += displacement.xz * _WorldToPixelSpace;

            #if UNITY_UV_STARTS_AT_TOP
            vi.vertex.z = -vi.vertex.z;
            #endif

            vo.pos = float4(vi.vertex.xz, 0.5, 1);
            vo.height = displacement.y;

            return vo;
        }

        half4 fragDisplace(VertexOutput3 vo) : SV_Target
        {
            return vo.height;
        }

            //
            // Blend two textures
            //
        sampler2D _Texture1;
        sampler2D _Texture2;
        float _BlendFactor;

        VertexOutput2 vertBlend(VertexInput vi)
        {
            VertexOutput2 vo;

            vo.pos = UnityObjectToClipPos(vi.vertex);
            vo.uv = half4(vi.uv0, 0, 0);

            return vo;
        }

        half4 fragBlend(VertexOutput2 vo) : SV_Target
        {
    #if SHADER_TARGET >= 40
            half4 pix1 = tex2Dlod(_Texture1, vo.uv);
            half4 pix2 = tex2Dlod(_Texture2, vo.uv);
    #else
            half4 pix1 = tex2D(_Texture1, vo.uv);
            half4 pix2 = tex2D(_Texture2, vo.uv);
    #endif

            return lerp(pix1, pix2, _BlendFactor);
        }

            ENDCG

            SubShader
        {
            Pass
            {
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment fragHeight2Normal

                ENDCG
            }

                Pass
            {
                Name "CopyDisplacements"

                ZTest Always Cull Off ZWrite Off
                ColorMask RGB

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vertCopy
                #pragma fragment copyDisplacements

                ENDCG
            }

                // unused
                Pass
            {
                Name "CopyRB"

                ZTest Always Cull Off ZWrite Off
                ColorMask RB

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vertCopy
                #pragma fragment copyDisplacements

                ENDCG
            }

                Pass
            {
                Name "CopyRG"

                ZTest Always Cull Off ZWrite Off
                ColorMask RGBA

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vertCopyNormal
                #pragma fragment fragCopyNormal

                ENDCG
            }

                Pass
            {
                Name "CopyRBAJacobian"

                ZTest Always Cull Off ZWrite Off
                ColorMask RGBA

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vertJacobian
                #pragma fragment copyJacobian

                ENDCG
            }

                Pass
            {
                Name "DisplaceHeightmap"

                ZTest Always Cull Off ZWrite Off
                ColorMask[_ColorMask]

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vertDisplace
                #pragma fragment fragDisplace

                ENDCG
            }

                /*Pass
                {
                    Name "Copy"

                    ZTest Always Cull Off ZWrite Off
                    ColorMask RGBA

                    CGPROGRAM

                    #pragma target 2.0

                    #pragma vertex vertCopy
                    #pragma fragment fragCopy

                    ENDCG
                }*/

                Pass
            {
                Name "Blend"

                ZTest Always Cull Off ZWrite Off

                CGPROGRAM

                #pragma target 2.0

                #pragma vertex vertBlend
                #pragma fragment fragBlend

                ENDCG
            }
        }
}