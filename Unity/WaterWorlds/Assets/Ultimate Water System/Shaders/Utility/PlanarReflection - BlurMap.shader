// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



/*
 * Renders map of depth-based blur sizes for planar reflection effect.
 */
Shader "UltimateWater/Utilities/PlanarReflectionBlurMap" {
    Properties{ }

        CGINCLUDE
        #include "UnityCG.cginc"

        struct v2f
    {
        float4 pos : SV_POSITION;
        half2 depth : TEXCOORD0;
    };

    float4x4 _NormalProjectionMatrix;			// UNITY_MATRIX_MVP is set as an oblique matrix and is useless for depth mapping
    half _MaxBlurSizeDepthInv;

    v2f vert(float4 vertex : POSITION)
    {
        v2f o;
        o.pos = mul(_NormalProjectionMatrix, mul(UNITY_MATRIX_MV, vertex));
        o.depth = o.pos.zw;
        o.pos = UnityObjectToClipPos(vertex);
        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        half depth = i.depth.x / i.depth.y;
        half linearDepth = LinearEyeDepth(depth);
        half blurSize = linearDepth * _MaxBlurSizeDepthInv;

        return min(1, blurSize);
    }
        ENDCG

        SubShader
    {
        Tags{"RenderType" = "Opaque"}

            Pass
        {
            Fog { Mode Off }

            CGPROGRAM
                #pragma target 2.0

                #pragma vertex vert
                #pragma fragment frag
            ENDCG
        }
    }
}