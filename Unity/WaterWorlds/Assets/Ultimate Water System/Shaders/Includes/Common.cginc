// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



inline float4 CustomBlitTransform(float4 vertex)
{
    #if UNITY_SINGLE_PASS_STEREO

    #if UNITY_UV_STARTS_AT_TOP
    vertex.y = -vertex.y;
    #endif

    return vertex;
    #else
    return UnityObjectToClipPos(vertex);
    #endif
}