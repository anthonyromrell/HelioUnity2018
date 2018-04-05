Shader "UltimateWater/Refraction/Collect Light"
{
    SubShader
    {
        Tags{"RenderType" = "Opaque" "Queue" = "Geometry" "CustomType" = "Water"}

        Pass
    {
        Tags{"LightMode" = "ForwardBase"}

        ZWrite Off ZTest Always

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #pragma target 5.0
        #pragma exclude_renderers d3d9

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile_fwdbase

        #define _WATER_OVERLAYS 1
        #include "CollectLight.cginc"

        ENDCG
    }

        Pass
    {
        Tags{"LightMode" = "ForwardAdd"}
        ColorMask RGB
        Blend One One
        BlendOp Add
        Fog{Color(0,0,0,0)} // in additive pass fog should be black
        ZWrite Off ZTest Always

        CGPROGRAM
        #pragma vertex vert_add
        #pragma fragment frag_add

        #pragma target 5.0
        #pragma exclude_renderers d3d9

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile_fwdadd_fullshadows

        #define _WATER_OVERLAYS 1

        #include "CollectLight.cginc"

        ENDCG
    }
    }

        SubShader
    {
        Tags{"RenderType" = "Opaque" "Queue" = "Geometry" "CustomType" = "Water"}

        Pass
    {
        Tags{"LightMode" = "ForwardBase"}

        ZWrite Off ZTest Always

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #pragma target 3.0
        #pragma exclude_renderers d3d9

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile_fwdbase

        #define _WATER_OVERLAYS 1

        #include "CollectLight.cginc"

        ENDCG
    }
    Pass
    {
        Tags{"LightMode" = "ForwardAdd"}
        ColorMask RGB
        Blend One One
        BlendOp Add
        Fog{Color(0,0,0,0)} // in additive pass fog should be black
        ZWrite Off ZTest Always

        CGPROGRAM
        #pragma vertex vert_add
        #pragma fragment frag_add

        #pragma target 3.0
        #pragma exclude_renderers d3d9

        #pragma multi_compile __ _WAVES_FFT
        #pragma multi_compile_fwdadd_fullshadows

        #define _WATER_OVERLAYS 1

        #include "CollectLight.cginc"

        ENDCG
    }
    }
        Fallback "VertexLit"
}