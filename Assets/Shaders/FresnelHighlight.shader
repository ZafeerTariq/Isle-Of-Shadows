Shader "Unlit/FresnelHighlight" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Power ( "Power", float ) = 2
		_Color ( "Color", Color ) = ( 1, 1, 1, 1 )
		_Frequency ( "Glow Frequency", float ) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float3 normal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Power;
			float _Frequency;
			float4 _Color;

			v2f vert( appdata v ) {
				v2f o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.uv = TRANSFORM_TEX( v.uv, _MainTex );
				o.normal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = mul( unity_ObjectToWorld, v.vertex );
				return o;
			}

			float4 frag( v2f i ) : SV_Target {
				float3 viewAngle = normalize( _WorldSpaceCameraPos - i.worldPos );
				float fresnel = pow( 1.0 - saturate( dot( viewAngle, i.normal) ), _Power );

				float pulse = sin( _Time.y * _Frequency );

				float4 color = tex2D( _MainTex, i.uv );
				color.rgb += fresnel * _Color.rgb * pulse;
                return color;
			}
			ENDCG
		}
	}
}
