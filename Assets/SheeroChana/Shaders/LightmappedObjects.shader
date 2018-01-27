// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Light-mapped objects shader for GPD-Q9 V2.4
// (c) 2016 Franck Sauer
//
// RGB_nX -> RGB base color, normal X
// MGC_nY -> Metalness, Glossiness, Cavity, normal Y

//updates
// 2.1 small fixes and tweaks
// 2.2 replaced custom cubemaps with Unity's Reflection Probes.
// 2.3 added directionnality param for bidirectionnal diffuse.
// 2.4 added _LightmapHDRFactor to compensate for lack of HDR lightmaps on android

Shader "Custom/LightmappedObjects"
{
	Properties
	{
		[NoScaleOffset] _RGB_nX ("RGB_nX", 2D) = "" {}
		[NoScaleOffset] _MGC_nY ("MGC_nY", 2D) = "" {}
		_Directionnality ("Directionnality", Range (0.0,1.0)) = 0.5
	}
	SubShader
	{
		Tags { "Queue" = "Geometry" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _RGB_nX, _MGC_nY;
			
			half3 _SunColor, _FogColor, _SunDir;
			float _FogMinHeight, _FogMaxHeight, _FogDistance, _Directionnality, _LightmapHDRFactor;

			struct appData
			{
				float4 vertex	:	POSITION;
				half3 normal	:	NORMAL;
				float4 tangent	:	TANGENT;
				float2 texcoord	:	TEXCOORD0;
				float2 texcoord1:	TEXCOORD1;				
			};

			struct v2f
			{
				float4 pos		:	SV_POSITION;
				float4 UV		:	TEXCOORD0;
				float2 fog		:	TEXCOORD1;
				float4 T_wPosX	:	TEXCOORD2;
				float4 B_wPosY	:	TEXCOORD3;
				float4 N_wPosZ	:	TEXCOORD4;
			};
			
			v2f vert(appData v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.T_wPosX.w = wPos.x;
				o.B_wPosY.w = wPos.y;
				o.N_wPosZ.w = wPos.z;
				
				o.N_wPosZ.xyz = mul(unity_ObjectToWorld, half4(v.normal,0)).xyz;
				o.T_wPosX.xyz = mul(unity_ObjectToWorld, half4(v.tangent.xyz,0)).xyz;
				o.B_wPosY.xyz = cross(o.N_wPosZ.xyz, o.T_wPosX.xyz) * v.tangent.w;
				
				o.UV.xy = v.texcoord.xy;
				o.UV.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				
				o.fog.x = saturate(_FogDistance*length(wPos-_WorldSpaceCameraPos)); //_FogDistance = 1/FogDistance
				o.fog.x *= saturate(_FogMaxHeight*(_FogMinHeight-wPos.y)); //_FogMaxHeight = 1/FogMaxHeight
				o.fog.x *= o.fog.x;
				o.fog.y = dot(-_SunDir,normalize(wPos-_WorldSpaceCameraPos))*.5+.5;
				return o;
			}
			
			half4 frag(v2f i) : SV_Target
			{
				half4 o;
				half4 RGB_nX = tex2D(_RGB_nX,i.UV).rgba;
				half4 MGC_nY = tex2D(_MGC_nY,i.UV).rgba;
				half2 Nxy = half2(RGB_nX.w, MGC_nY.w)*2-1;
				half3 N = normalize(i.B_wPosY.xyz * Nxy.y + (i.T_wPosX.xyz * Nxy.x + i.N_wPosZ.xyz));
				half3 wPos = float3(i.T_wPosX.w,i.B_wPosY.w,i.N_wPosZ.w);
				half3 L = -_SunDir;
				half3 V = normalize(_WorldSpaceCameraPos-wPos);
				half3 H = normalize(V+L);
				half3 R = reflect(-V,N);
				half NdotL = dot(N,L);
				half NdotH = saturate(dot(N,H));
				half NdotV = saturate(dot(N,V));
				half3 baseColor = RGB_nX.rgb;
				half gloss = MGC_nY.g;
				half e = exp2(gloss*12);
				half metalness = MGC_nY.r;
				half cavity = MGC_nY.b;
				half3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.UV.zw)) * _LightmapHDRFactor;
				
				//diffuse term
				half3 diffuse = (abs(NdotL*_Directionnality)+cavity*(1-_Directionnality)) * baseColor * (1-metalness); //soft double direction for non-directional lightmapped objects

				//analytic spec
				half specDistrib = saturate(NdotL) * pow(NdotH,e) * (e *.125 + 1);

				half3 minSpec = float3(.1,.1,.1);
				half3 F0 = metalness * ( baseColor - minSpec) + minSpec;
				half3 invF0 = 1-F0;
				half fCurve = 1-NdotV; //omni fresnel for simplification
				fCurve *= fCurve;
				fCurve *= fCurve;
				half3 fresnel = fCurve * (invF0) + F0;
				
				//env spec
				half mipOffset = 1-saturate(gloss+fCurve);
				half4 envData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, R,  mipOffset*7);
                half3 env = DecodeHDR (envData, unity_SpecCube0_HDR); 				
				
				//geovis
				half3 geoVis = NdotV * (1-gloss) + gloss;
				geoVis *= fresnel;

				//spec term
				half3 specular = specDistrib * geoVis;
				
				//ambient specular term
				half3 ambSpec = env * geoVis * cavity;
				
				//final compo
				half3 result = (diffuse + specular) * lightmap + ambSpec;
				
				o.rgb = lerp(result, lerp(_FogColor,_SunColor,(half)i.fog.y), (half)i.fog.x);
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
	} 
}

