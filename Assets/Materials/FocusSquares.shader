Shader "Custom/Effects/FocusSquares"
{
    Properties
    {
        _Intensity("Intensity", Float) = 0
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Tags { 
        "RenderType"="Transparent"
        "Queue" = "Transparent"}

        Pass
        {
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 color : COLOR;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainTex_ST;
            float4 _Color;
            float _Intensity;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                return lerp(0, i.color, _Intensity);
            }
            ENDCG
        }
    }
}
