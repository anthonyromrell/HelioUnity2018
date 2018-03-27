Shader "UltimateWater/Particles/GPU_Render"
{
    Properties
    {
        _FoamAtlasParams("", Vector) = (1, 1, 0, 0)
        _FoamAtlas("", 2D) = "white" {}
        _FoamOverlayTexture("", 2D) = "white" {}
    }

        SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Displacement and normal map"
            Blend One One

            CGPROGRAM
            #pragma vertex geomVert
            #pragma geometry geom
            #pragma fragment frag

            #pragma target 5.0

            #include "WaveParticlesGPU_Render.cginc"

            ENDCG
        }

        Pass
        {
            Name "Foam"
            ColorMask G
            Blend One One

            CGPROGRAM
            #pragma vertex geomVert
            #pragma geometry geom
            #pragma fragment fragFoam

            #pragma target 5.0

            #define FOAM 1

            #include "WaveParticlesGPU_Render.cginc"

            ENDCG
        }

        Pass
        {
            Name "Displacement Mask Trail"
            Blend DstColor Zero
            BlendOp Add

            CGPROGRAM
            #pragma vertex geomVert
            #pragma geometry geom
            #pragma fragment fragDisplacementsMask

            #pragma target 5.0

            #define TRAILS 1

            #include "WaveParticlesGPU_Render.cginc"

            ENDCG
        }

        Pass
        {
            Name "Foam Trail"
            ColorMask G
            Blend One One

            CGPROGRAM
            #pragma vertex geomVert
            #pragma geometry geom
            #pragma fragment fragFoamTrail

            #pragma target 5.0

            #define TRAILS 1
            #define TRAILS_FOAM 1

            #include "WaveParticlesGPU_Render.cginc"

            ENDCG
        }
    }
}