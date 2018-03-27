Shader "UltimateWater/Particles/Particles"
{
    Properties
    {
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 5.0

            #include "WaveParticles.cginc"

            ENDCG
        }

        Pass
        {
            Blend One Zero

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 5.0

            #define _DEBUG_PARTICLES 1

            #include "WaveParticles.cginc"

            ENDCG
        }
    }

        SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 3.0

            #include "WaveParticles.cginc"

            ENDCG
        }

        Pass
        {
            Blend One Zero

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 3.0

            #define _DEBUG_PARTICLES 1

            #include "WaveParticles.cginc"

            ENDCG
        }
    }
}