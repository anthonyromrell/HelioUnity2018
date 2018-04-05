// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltimateWater/Dynamic/Simulation"
{
    Properties
    {
        MatrixT2("Matrix t-2", 2D) = "black" {}
        MatrixT1("Matrix t-1", 2D) = "black" {}

        DepthT1("Depth t-1", 2D) = "black" {}
        Depth("Depth", 2D) = "black" {}
        StaticDepth("Static Depth", 2D) = "black" {}

        Propagation("Propagation", Float) = 1.0
        Damping("Damping", Float) = 1.0

        Gain("Gain", Float) = 0.05

        HeightOffset("Height offset", Float) = 2.0
        HeightGain("Height gain", Float) = 2.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                // Wave simulation textures
                sampler2D MatrixT2;
                sampler2D MatrixT1;
                float4 MatrixT1_TexelSize;

                // Depth data
                sampler2D DepthT1;
                sampler2D Depth;
                sampler2D StaticDepth;

                // Settings
                uniform float Propagation = 1.0f;
                uniform float Damping = 0.2f;

                uniform float Gain = 0.05f;
                uniform float HeightOffset = 2.0f;
                uniform float HeightGain = 2.0f;

                uniform float _WaterHeight = 0.0f;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    const float2 scale = float2(1.0f / (MatrixT1_TexelSize.z - 1.0f), 1.0f / (MatrixT1_TexelSize.w - 1.0f));

                    // calculate sampling points
                    const float2 left_idx   = i.uv + float2(-scale.x, 0.0f);
                    const float2 right_idx  = i.uv + float2(+scale.x, 0.0f);
                    const float2 bottom_idx = i.uv + float2(0.0f, -scale.y);
                    const float2 top_idx    = i.uv + float2(0.0f, +scale.y);

                    // sample simulation height
                    const float center_height = tex2D(MatrixT1, i.uv).r;
                    const float left_height   = tex2D(MatrixT1, left_idx).r;
                    const float right_height  = tex2D(MatrixT1, right_idx).r;
                    const float top_height    = tex2D(MatrixT1, top_idx).r;
                    const float bottom_height = tex2D(MatrixT1, bottom_idx).r;

                    //handle static calculations
                    const float static_height = tex2D(StaticDepth, i.uv);
                    if(static_height > _WaterHeight && static_height != 0.0f)
                    {
                        const float left_sample   = left_height   * (tex2D(StaticDepth, left_idx).r   == 0.0f);
                        const float right_sample  = right_height  * (tex2D(StaticDepth, right_idx).r  == 0.0f);
                        const float top_sample    = top_height    * (tex2D(StaticDepth, top_idx).r    == 0.0f);
                        const float bottom_sample = bottom_height * (tex2D(StaticDepth, bottom_idx).r == 0.0f);
                        return max(max(max(left_sample, right_sample), top_sample), bottom_sample);
                    }

                    const float previous_value = tex2D(MatrixT2, i.uv).r;

                    // sample velocity/depth texture
                    const float4 object_data       = tex2D(Depth, i.uv);
                    const float4 previous_object_data = tex2D(DepthT1, i.uv);

                    const float3 object_velocity   = object_data.rgb;
                    const float  object_height     = object_data.a;

                    const float previous_object_height = previous_object_data.a;

                    // calculations
                    const float velocity_magnitude = length(object_velocity);
                    const float direction = sign(sign(object_velocity.y) * 2 + 1);


                    const float height          = object_height - _WaterHeight;
                    const float previous_height = previous_object_height - _WaterHeight;

                    const float height_delta = object_height - previous_object_height;

                    const float left   = lerp(center_height, left_height, Propagation);
                    const float right  = lerp(center_height, right_height, Propagation);
                    const float top    = lerp(center_height, top_height, Propagation);
                    const float bottom = lerp(center_height, bottom_height, Propagation);

                    const float height_gain = abs(height * HeightGain) + abs(HeightOffset);
                    const float gain = Gain * velocity_magnitude * direction / pow(height_gain + 1.0f, 5.0f);

                    float value = 0.5f * (left + right + top + bottom) - previous_value;
                    value -= value * Damping;
                    value += clamp(gain, -5.0f, 5.0f);
                    return value;
                }
                ENDCG
            }
        }
}