Shader "Custom/Pr3T_marker_HA160903" {
	Properties {
		_Color ( "Color", Color ) = ( 1, 1, 1, 1 )
		_Color2 ( "Color2", Color ) = ( 1, 1, 1, 1 )
		_MainTex( "Texture", 2D ) = "white"{}
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		struct Input {
			float3 worldPos;
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed4 _Color2;
		sampler2D _MainTex;

		fixed3 radar( Input IN, float period, float fade ) {
			float distFromCam = length( IN.worldPos );
			float x = distFromCam / period - _Time.y;
			float sawtooth = ( x - floor( x ) ) * period;
			fixed v = saturate( ( sawtooth - ( period - fade ) ) / fade ) / ( 1 + distFromCam );
			return fixed3( v, v, v );
		}

		void surf( Input IN, inout SurfaceOutput o ) {
			float3 worldNormal = WorldNormalVector( IN, o.Normal );
			fixed3 thick = 1;
			fixed3 thin = 1;
			fixed3 strength = radar( IN, 8.0f, 7.0f );
			fixed3 ruleResult = _Color * ( 6.0f * thick * thick + 6.0f * thin * thin ) * strength;

			fixed3 radarResult = _Color2 * ( IN, 8.0f, 1.0f );
			o.Emission = ruleResult + radarResult;
			o.Alpha = 1;
			o.Albedo = tex2D( _MainTex, IN.uv_MainTex );
		}
		ENDCG
	}
	FallBack "Diffuse"
}