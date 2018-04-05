// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "UltimateWater/Raindrops/PreciseParticle"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Blend SrcAlpha One
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                // UNITY_VERTEX_INPUT_INSTANCE_ID
             };

             v2f vert(appdata v)
             {
                 v2f o;

                 // UNITY_SETUP_INSTANCE_ID(v);
                 // UNITY_TRANSFER_INSTANCE_ID(v, o);

                  o.vertex = mul(unity_ObjectToWorld, v.vertex);
                  o.uv = v.uv;

                  return o;
              }

              sampler2D _MainTex;

              float4 frag(v2f i) : SV_Target
              {
                  float modifier = tex2D(_MainTex, i.uv);
                  float value = 1.0f - saturate(length((i.uv - 0.5f) * 2.0f));

                  float strength = (modifier) * pow(sin(3.1415 * value * 0.5f), 8.0f) * 0.1f;
                  return strength;
              }
              ENDCG
          }
    }
}