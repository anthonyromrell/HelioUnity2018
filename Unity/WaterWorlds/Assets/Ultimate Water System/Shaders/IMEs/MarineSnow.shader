// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Underwater/Marine Snow" 
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
    }

    CGINCLUDE
    struct appdata_t
    {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 texcoord : TEXCOORD0;
    };

    sampler2D _MainTex;
    float4 _MainTex_ST;

    sampler2D _UnderwaterMask;
    fixed4 _TintColor;
    ENDCG

    SubShader 
    {
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            Fog{Mode Off}
            ZWrite On ZTest LEqual Cull Off
            Offset 1, 1

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                
                V2F_SHADOW_CASTER;
                half2 texcoord : TEXCOORD2;
                half4 screenPos : TEXCOORD3;
                half4 color : TEXCOORD4;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)

                o.screenPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(o.screenPos.z);

                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                half4 sampl = tex2D(_MainTex, i.texcoord);
                float mask = tex2Dproj(_UnderwaterMask, i.screenPos);

                if(sampl.a < 0.1f  || mask < 1.0f)
                {
                    discard;
                }
                return 0;
            }

            ENDCG
        }

      
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
                half4 screenPos : TEXCOORD1;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);

                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float mask = tex2Dproj(_UnderwaterMask, i.screenPos);
                half4 texColor = tex2D(_MainTex, i.texcoord);

                if(mask < 1.0f || texColor.a < 0.1f)
                {
                    discard;
                }

               
                return 2.0f * i.color * _TintColor * texColor * texColor.a;
            }
            ENDCG
        }
        
    }

}

