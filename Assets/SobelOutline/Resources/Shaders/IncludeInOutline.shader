 Shader  "VertexFragment/IncludeInOutline"{

	Properties{
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0 
	//	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

		SubShader{
		//Tags {"Queue" = "Transparent-20" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		//LOD 200
		//ColorMask 0
		//ZWrite On

		Tags {  "RenderType" = "Opaque" "IgnoreProjector" = "True" "DisableOutlines" = "False" }

		LOD 200

//		ColorMask 0
//		ZWrite On
//			Lighting Off
//			Fog { Mode Off }

		//	Blend SrcAlpha OneMinusSrcAlpha
		//Pass { }
		CGPROGRAM
	//	#pragma surface surf Lambert alpha
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
		float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
		o.Alpha = 0;//c.a;            // Albedo comes from a texture tinted by color

		}
		ENDCG
	}

		Fallback "Transparent/VertexLit"
}
		//Fallback "Transparent/VertexLit"

/*
		SubShader{

		
		Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" }

		LOD 200
		ColorMask 1

		ZWrite On
			Lighting Off
			Fog { Mode Off }

			Blend SrcAlpha OneMinusSrcAlpha
		Pass { }
		CGPROGRAM
		
		#pragma surface surf Lambert alpha
       // #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

		sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
		fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
        /*
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
	
        ENDCG
} 

            */

		//Fallback "Transparent/VertexLit"
			