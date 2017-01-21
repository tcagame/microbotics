Shader "Custom/Pr3T_marker_HA160903" {
	Properties {
		_MainTex( "Texture", 2D ) = "white"{}
		_SubTex( "SubTexture", 2D ) = "white"{}
		_scrollX( "Scroll X", float ) = 0
		_ScrollY( "Scroll Y", float ) = 0
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _SubTex;
		float _ScrollX;
		float _ScrollY;

		void surf( Input IN, inout SurfaceOutput o ) {
			float2 scroll = float2( _ScrollX, _ScrollY ) * _Time.y;
			fixed4 c = tex2D( _MainTex, IN.uv_MainTex ) * tex2D( _SubTex, IN.uv_MainTex + scroll );
			o.Alpha = c.a;
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}