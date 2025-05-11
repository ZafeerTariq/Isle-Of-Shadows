Shader "Unlit/Healthbar" {
	Properties {
		_Health			( "Health", Range( 0, 100 ) ) = 100
		_LowColor		( "Low Health Color", Color ) = ( 1, 0, 0, 1 )
		_FullColor		( "Full Health Color", Color ) = ( 0, 1, 0, 1 )
		_LowThreshold	( "Low Threshold", Range( 0, 50 ) ) = 20
		_FullThreshold	( "Full Threshold", Range( 50, 100 ) ) = 80
		_MainTex		( "Health Shader", 2D ) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _Health;
			float4 _LowColor;
			float4 _FullColor;
			float _LowThreshold;
			float _FullThreshold;
			sampler2D _MainTex;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float inverseLerp( float a, float b, float x ) {
				return ( x - a ) / ( b - a );
			}

			fixed4 frag (v2f i) : SV_Target {
				float healthScalled = _Health / 100;
				float4 color = tex2D( _MainTex, float2( healthScalled, i.uv.y ) );

				float t = inverseLerp( _LowThreshold, _FullThreshold, _Health );
				float4 outColor = lerp( _LowColor, _FullColor, t );

				if( _Health < 20 )
					color *= sin( _Time.y * 15 ) * 0.15 + 1;

				return float4( color.xyz, ( abs( i.uv.x ) < healthScalled ) + 0.2 );
			}
			ENDCG
		}
	}
}