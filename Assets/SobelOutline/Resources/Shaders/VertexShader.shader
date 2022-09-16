Shader "Unlit/VertexShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
	// no Properties block this time!
	SubShader
	{

       
			Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" }
                
			CGPROGRAM
			
            //ZWrite On
            //ColorMask 0

			#pragma surface surf Standard 
			#pragma target 3.0
            // include file that contains UnityObjectToWorldNormal helper function
            //#include "UnityCG.cginc"

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
           // o.Albedo = c.rgb;
            o.Alpha = c.a;
            // Metallic and smoothness come from slider variables
          //  o.Metallic = _Metallic;
          //  o.Smoothness = _Glossiness;
        }
            ENDCG
		} 

}