Shader "Custom/DoubleSidedUnlit"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    Category
    {
        Lighting Off
        ZWrite On
        Cull off
        Blend SrcAlpha OneMinusSrcAlpha
        SubShader
        {
            Pass
            {
                Color [_Color]
            }
        }
    }
}