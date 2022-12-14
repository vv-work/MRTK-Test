/*
* This is a slightly modified version of "Alpha-Diffuse.shader" from the Built-in shaders
* pack for Unity3D version 4.1.5 downloaded from http://unity3d.com/unity/download/archive
* The only modification is the change to the "Queue" tag, making objects using this shader
* render before other transparent objects.
*/

Shader "TransparentBlock2" {//Before Adding Outine
	Properties{
	_Color("Main Color", Color) = (1,1,1,1)
	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

		SubShader{
		Tags {"Queue" = "Transparent-20" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 200
		//ZWrite On
		//ZTest LEqual


		 // Tags { "Queue" = "Geometry-1" }
		ColorMask 0
		ZWrite On
		Pass { }


		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
		float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		}
		ENDCG
	}

		Fallback "Transparent/VertexLit"
}