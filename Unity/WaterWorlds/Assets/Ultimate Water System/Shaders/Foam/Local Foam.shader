// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "UltimateWater/Foam/Local"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _FoamParameters("", Vector) = (0, 0, 0, 0)
    }

        CGINCLUDE

            #include "../Includes/UnityVersionsCompatibility.cginc"
            #include "UnityCG.cginc"

            struct VertexInput2
        {
            float4 vertex	: POSITION;
            float2 uv0		: TEXCOORD0;
        };

        struct VertexOutput
        {
            float4 pos				: SV_POSITION;
            float4 screenPos		: TEXCOORD0;		// xy = uv center, zw = world pos
            float2 worldPos			: TEXCOORD1;
            half4 previousScreenPos	: TEXCOORD2;
        };

        half4 _DistortionMapCoords;

        sampler2D _FoamMapPrevious;			// previous foam
        sampler2D _DisplacementMap;
        sampler2D _DisplacementsMask;

        half4	_SampleDir1;
        half4	_FoamParameters;		// x = intensity, y = horizonal displacement scale, z = power, w = fading factor
        float4	_LocalMapsCoords;
        float4	_LocalMapsCoordsPrevious;
        float4	_WaterTileSizeInv;
        float2  _WaterOffsetDelta;
        float3  _SurfaceOffset;
        float4x4 _WaterProjectorPreviousVP;

        sampler2D	_DisplacementDeltaMap;
        sampler2D	_DisplacementDeltaMap1;
        sampler2D	_DisplacementDeltaMap2;
        sampler2D	_DisplacementDeltaMap3;
        float		_DisplacementsScale;

        VertexOutput vert(VertexInput2 vi)
        {
            VertexOutput vo;

            float offset = 0.2;			// 0.25m offset

            vo.pos = UnityObjectToClipPos(vi.vertex);

            float4 worldPos = mul(unity_ObjectToWorld, vi.vertex);
            vi.uv0 = worldPos.xz * _LocalMapsCoords.zz + _LocalMapsCoords.xy;

            float4 packPosHorizontal = worldPos.xzxz + float4(0.2, 0.0, -0.2, 0.0);
            float4 packPosVertical = worldPos.xzxz + float4(0.0, 0.2, 0.0, -0.2);

            vo.screenPos = ComputeNonStereoScreenPos(vo.pos);// -_SampleDir1.zw * 0.000002;		// wind
            vo.worldPos = worldPos.xz + _SurfaceOffset.xz;

            worldPos.xz += _WaterOffsetDelta;
            vo.previousScreenPos = ComputeNonStereoScreenPos(mul(_WaterProjectorPreviousVP, worldPos));
            return vo;
        }

        //inline half2 SampleTex(float4 uvs1, float4 uvs2)
        //{
        //	return tex2D(_GlobalDisplacementMap, uvs1.xy).xz + tex2D(_GlobalDisplacementMap1, uvs1.zw).xz + tex2D(_GlobalDisplacementMap2, uvs2.xy).xz + tex2D(_GlobalDisplacementMap3, uvs2.zw).xz;
        //}

        /*inline half MipLevel(float2 uv)
        {
            float2 dx = ddx(uv * SUB_TEXTURE_SIZE);
            float2 dy = ddy(uv * SUB_TEXTURE_SIZE);
            float d = max(dot(dx, dx), dot(dy, dy));

            // Clamp the value to the max mip level counts
            const float rangeClamp = pow(2, (SUB_TEXTURE_MIPCOUNT - 1) * 2);
            d = clamp(d, 1.0, rangeClamp);

            float mipLevel = 0.5 * log2(d);
            mipLevel = floor(mipLevel);

            return mipLevel;
        }*/

        inline half ComputeFoamGain(float2 samplePosWorld, half4 displacementsMask)
        {
            half4 j = lerp(half4(0.25, 0.0, 0.25, 0.0), tex2D(_DisplacementDeltaMap, samplePosWorld * _WaterTileSizeInv.xx), displacementsMask.x)
                + lerp(half4(0.25, 0.0, 0.25, 0.0), tex2D(_DisplacementDeltaMap1, samplePosWorld * _WaterTileSizeInv.yy), displacementsMask.y)
                + lerp(half4(0.25, 0.0, 0.25, 0.0), tex2D(_DisplacementDeltaMap2, samplePosWorld * _WaterTileSizeInv.zz), displacementsMask.z)
                + lerp(half4(0.25, 0.0, 0.25, 0.0), tex2D(_DisplacementDeltaMap3, samplePosWorld * _WaterTileSizeInv.ww), displacementsMask.w);

            half jacobian = j.y * j.y - j.x * j.z;
            half gain = max(0.0, jacobian + 0.94);

            return gain + j.a * 0.5;
        }

        half4 frag(VertexOutput vo) : SV_Target
        {
            half4 displacementsMask = tex2Dproj(_DisplacementsMask, vo.screenPos);
            half shoreMask = displacementsMask.w;
            half foam;

            if(vo.previousScreenPos.x >= 0.0 && vo.previousScreenPos.x <= vo.previousScreenPos.w && vo.previousScreenPos.y >= 0.0 && vo.previousScreenPos.y <= vo.previousScreenPos.w)
                foam = tex2Dproj(_FoamMapPrevious, vo.previousScreenPos);
            else
                foam = 0.0;

            foam = min(foam, 0.999);
            foam = 20.0 * log(1.0 / (1.0 - foam));

            half foamFade = max(0.0, (shoreMask - 0.06) / 0.94);
            foam *= lerp(lerp(_FoamParameters.w, 1.0, 0.75), _FoamParameters.w, foamFade * foamFade);

            half foamGain = ComputeFoamGain(vo.worldPos, displacementsMask) * _FoamParameters.x * shoreMask * shoreMask;
            foam += foamGain;

            foam = 1.0 - exp(-0.05 * foam);

            half2 fade = 1.0 - pow(vo.screenPos.xy / vo.screenPos.ww * 2.0 - 1.0, 8);		// precompute?

            return half4(max(0, foam * min(fade.x, fade.y)), 0, 0, 0);
        }

            /*half4 fragInit(VertexOutput vo) : SV_Target
            {
                half4 prev = tex2D(_FoamMapPrevious, vo.uvPreviousFrame);
                return half4(0, 0, (1.0 - prev.a * 5.0) * 0.15, 0);				// / 0.2
            }*/

            ENDCG

            SubShader
        {
            Pass
            {
                ZTest Always Cull Off ZWrite Off
                ColorMask R

                CGPROGRAM

                #pragma target 3.0

                #pragma vertex vert
                #pragma fragment frag

                ENDCG
            }

                /*Pass
                {
                    ZTest Always Cull Off ZWrite Off
                    ColorMask B

                    CGPROGRAM

                    #pragma target 2.0

                    #pragma vertex vert
                    #pragma fragment fragInit

                    ENDCG
                }*/
        }
}