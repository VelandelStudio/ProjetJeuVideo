// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Fast ocean shader for GPDQ9 Version 2.1
// (c) 2016 Franck Sauer

// Uses hemispheric env map + normal

// updates
// UV speed can now be configured by user using Offset values in the interface

Shader "Custom/Ocean"
{
	Properties
	{
		[NoScaleOffset] _EnvTex ("_EnvTex", 2D) = "white" {} //requires an hemispheric env map
		_NormTex ("_NormTex", 2D) = "white" {} //small and tileable
	}

	SubShader
	{
		Tags { "Queue" = "Geometry-5" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _EnvTex, _NormTex, _LightMapTex;
			half3 _SunColor, _Sky, _FogColor,_SunDir;
			float _FogMinHeight, _FogMaxHeight, _FogDistance; 
			float4 _NormTex_ST;

			struct appData
			{
				float4 vertex		:	POSITION;
				float4 texcoord0	:	TEXCOORD0;
			};

			struct v2f
			{
				float4 pos		:	SV_POSITION;
				float4 UV_fog	:	TEXCOORD0;
				float3 wpos		:	TEXCOORD1;
			};
			
			v2f vert(appData i)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (i.vertex);
				
				float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
				o.wpos = i.vertex.xyz;
				
				o.UV_fog.xy = i.texcoord0 * _NormTex_ST.xy;
				
				float3 dist = i.vertex.xyz-_WorldSpaceCameraPos;
				float3 normDist = normalize(dist);
				o.UV_fog.z = saturate(_FogDistance*length(dist)); //_FogDistance = 1/_FogDistance (1/1214)
				o.UV_fog.z *= saturate(_FogMaxHeight*(_FogMinHeight-i.vertex.y)); //_FogMaxHeight = 1/_FogMaxHeight (1/286)
				o.UV_fog.w = dot(-_SunDir,normDist)*.5+.5;
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				half4 o;
				half3 V = normalize(i.wpos-_WorldSpaceCameraPos);
				
				float2 disp = _Time.x * _NormTex_ST.zw;
				half2 Nxy = tex2D(_NormTex, i.UV_fog.xy + disp).ag*2-1;
				Nxy += tex2D(_NormTex, i.UV_fog.xy *.7 + disp *.25).ag*2-1;
				half3 N = normalize(half3(Nxy,2));
				half3 R = reflect(-V,N);
							
				half3 envTexture = tex2D(_EnvTex, R.xz *.5 +.5 ); //hemispheric projection
				
				half3 result = envTexture;

				o.rgb = lerp(result, lerp(_FogColor,_SunColor,(half)i.UV_fog.w), (half)i.UV_fog.z);
				
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
	} 
}

