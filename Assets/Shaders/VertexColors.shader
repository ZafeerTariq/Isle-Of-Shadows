Shader "Unlit/VertexColors" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float3 normal : TEXCOORD1;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos( v.vertex );
				o.normal = UnityObjectToWorldNormal( v.normal );
				o.color = v.color;
				return o;
			}

			float4 frag (v2f i) : SV_Target {
				float3 color =  GammaToLinearSpace( i.color.rgb );

				float3 normal = normalize( i.normal );
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 attenuation = LIGHT_ATTENUATION( i );
				float lambertian = saturate( dot( normal, lightDir ) );
				float3 diffuseLight = lambertian * attenuation * _LightColor0.xyz + 0.2f;

				return float4( color * diffuseLight, i.color.a );
			}
			ENDCG
		}
	}
}
