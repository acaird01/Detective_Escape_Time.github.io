Shader "Masks/CasketUnderHole_Shader"
{
    SubShader
    {
        // regular geometry 이후에 마스크를 render 함.
        Tags { "Queue" = "Background" }

        // RGBA 채널을 그리지 않고, Depth Buffer로만 사용
        ColorMask 0
        ZWrite On

        Pass {}
    }
}