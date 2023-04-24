// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "tintImageEffectShader"
{
   Properties
   {
      _MainTex ("Source", 2D) = "white" {}
      _Color("Tint", Color) = (1,1,1,1)
      _Intensity("Intensity", Float) = 0
      _VignetteRadius("VignetteRadius", Float) = 1.0
      _VignetteSmoothing("VignetteSmoothing", Float) = 0.5
   }
   SubShader
   {
      Cull Off 
      ZWrite Off 
      ZTest Always
      
      Tags
      {
      "RenderType" = "Overlay"
      "Queue" = "Overlay"
      }
      
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
            float4 vertex : TEXCOORD1;
            float4 position : SV_POSITION;
         };

         vertexOutput vertexShader(vertexInput i)
         {
            vertexOutput o;
            o.vertex = i.vertex;
            o.position = UnityObjectToClipPos(i.vertex);
            o.texcoord = i.texcoord;
            return o;
         }
			
         sampler2D _MainTex;
         float4 _MainTex_ST;
         float4 _Color;
         float _Intensity;
         float _VignetteRadius;
         float _VignetteSmoothing;

         float4 fragmentShader(vertexOutput i) : COLOR
         {
            //grab color from base screen. This VVV here is completely useless.
            float4 color = tex2D(_MainTex,UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST));	
            
            ///Would like this to be scaled by distance from player rather than screen width/height
            float2 uvCentered = i.texcoord * 2 - 1;
            float distance = length(uvCentered);
            
            //Apply those tuning values, smoothstep clamps it already
            float vignette = smoothstep(_VignetteRadius - _VignetteSmoothing, _VignetteRadius, distance);

            //Tinted based on vignette strength and effect intensity
            return color * (lerp(1, _Color, _Intensity * vignette) );
         }
         ENDCG
      }
   }
   Fallback Off
}