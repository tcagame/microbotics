Shader "Custom/Pr3T_marker_HA160903" {
	Properties {
		_MainTex( "Texture", 2D ) = "white"{}
		_SubTex1( "SubTexture1", 2D ) = "white"{}
		_SubTex2( "SubTexture2", 2D ) = "white"{}
		_scrollX1( "Scroll X1", float ) = 0
		_ScrollY1( "Scroll Y1", float ) = 0
		_scrollX2( "Scroll X2", float ) = 0
		_ScrollY2( "Scroll Y2", float ) = 0
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
		sampler2D _SubTex1;
		sampler2D _SubTex2;
		float _ScrollX1;
		float _ScrollY1;
		float _ScrollX2;
		float _ScrollY2;
		fixed _Alpha;

		void surf( Input IN, inout SurfaceOutput o ) {
			float2 scroll1 = float2( _ScrollX1, _ScrollY1 ) * _Time.y;
			float2 scroll2 = float2( _ScrollX2, _ScrollY2 ) * _Time.y;
			fixed4 c = tex2D( _MainTex, IN.uv_MainTex ) * tex2D( _SubTex1, IN.uv_MainTex + scroll1 ) * tex2D( _SubTex2, IN.uv_MainTex + scroll2 );
			o.Alpha = _Alpha;
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}