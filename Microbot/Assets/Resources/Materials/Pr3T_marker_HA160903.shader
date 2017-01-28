Shader "Custom/Pr3T_marker_HA160903" {
	Properties {
		_MainTex( "Texture", 2D ) = "white"{}
		_EmissionColor( "EmissionColor", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_ScrollTex1( "EmissionTexture1", 2D ) = "white"{}
		_ScrollTex2( "EmissionTexture2", 2D ) = "white"{}
		_scrollX1( "Scroll X1", float ) = 0
		_ScrollY1( "Scroll Y1", float ) = 0
		_scrollX2( "Scroll X2", float ) = 0
		_ScrollY2( "Scroll Y2", float ) = 0
		_Gloss( "Gloss", Range( 0, 1 ) ) = 1
		_Alpha( "Alpha", Range( 0, 1 ) ) = 1
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
		half4 _EmissionColor;
		sampler2D _ScrollTex1;
		sampler2D _ScrollTex2;
		float _ScrollX1;
		float _ScrollY1;
		float _ScrollX2;
		float _ScrollY2;
		fixed _Alpha;
		fixed _Gloss;

		void surf( Input IN, inout SurfaceOutput o ) {
			float2 scroll1 = float2( _ScrollX1, _ScrollY1 ) * _Time.y;
			float2 scroll2 = float2( _ScrollX2, _ScrollY2 ) * _Time.y;
			float t = ( ( 2 * _SinTime.w * _CosTime.w ) + 1.0 ) * 0.5;
			fixed4 c = tex2D( _MainTex, IN.uv_MainTex ) * tex2D( _ScrollTex1, IN.uv_MainTex + scroll1 ) * tex2D( _ScrollTex2, IN.uv_MainTex + scroll2 );
			o.Alpha = _Alpha;
			o.Gloss = _Gloss;
			o.Albedo = c.rgb;
			o.Emission = _EmissionColor * t;
		}
		ENDCG
	}
	FallBack "Diffuse"
}