// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "tintImageEffectShader"
{
   Properties
   {
      _MainTex ("Source", 2D) = "white" {}
      _Color("Tint", Color) = (1,1,1,1)
      _Intensity("Intensity", Float) = 0
   }
   SubShader
   {
      Cull Off 
      ZWrite Off 
      ZTest Always

      Pass
      {
         CGPROGRAM
         #pragma vertex vertexShader
         #pragma fragment fragmentShader
			
         #include "UnityCG.cginc"

         struct vertexInput
         {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
         };

         struct vertexOutput
         {
            float2 texcoord : TEXCOORD0;
            float4 position : SV_POSITION;
         };

         vertexOutput vertexShader(vertexInput i)
         {
            vertexOutput o;
            o.position = UnityObjectToClipPos(i.vertex);
            o.texcoord = i.texcoord;
            return o;
         }
			
         sampler2D _MainTex;
         float4 _MainTex_ST;
         float4 _Color;
         float _Intensity;

         float4 fragmentShader(vertexOutput i) : COLOR
         {
            float4 color = tex2D(_MainTex, 
               UnityStereoScreenSpaceUVAdjust(
               i.texcoord, _MainTex_ST));		
            //return float4(_Intensity, 0, 0, 1);
            //return color * _Color;
            return color * lerp(1, _Color, _Intensity);
         }
         ENDCG
      }
   }
   Fallback Off
}