Shader "Masks/CasketUnderHole_Shader"
{
    SubShader
    {
        // regular geometry ���Ŀ� ����ũ�� render ��.
        Tags { "Queue" = "Background" }

        // RGBA ä���� �׸��� �ʰ�, Depth Buffer�θ� ���
        ColorMask 0
        ZWrite On

        Pass {}
    }
}