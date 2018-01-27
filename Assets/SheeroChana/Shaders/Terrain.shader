// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Terrain Shader with splat maps for GPDQ9 V2.1
// (c) 2014-2016 Franck Sauer
//
// This shader requires an RGBA splatmap and 5 tileable textures.
// UV0 original unrelaxed(projected) UVs from WM for splatmap
// UV1 relaxed UV for tiles mapping and lightmapping

//updates
//2.1 added _LightmapHDRFactor to compensate for lack of HDR lightmaps on android

Shader "Custom/Terrain"
{
	Properties
	{
		[NoScaleOffset] _SplatMap ("SplatMap", 2D) = "" {}
		[NoScaleOffset] _ShadeMap ("ShadeMap", 2D) = "" {}
		_ShadeScale("ShadeScale",Range(1,1.5)) = 1.25

		_Tex_Red ("Tex_Red", 2D) = "" {}
		_Tex_Green ("Tex_Green", 2D) = "" {}
		_Tex_Blue ("Tex_Blue", 2D) = "" {}
		_Tex_Alpha ("Tex_Alpha", 2D) = "" {}
		_Tex_Black ("Tex_Black", 2D) = "" {}
	}
	SubShader
	{
		Tags { "Queue" = "Geometry-10" "DisableBatching" = "True"}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _SplatMap, _ShadeMap, _Tex_Red, _Tex_Green, _Tex_Blue, _Tex_Alpha, _Tex_Black;
			half3 _SunDir, _SunColor, _FogColor;
			float _FogMinHeight, _FogMaxHeight, _FogDistance, _LightmapHDRFactor;
			half _ShadeScale;
			float2 _Tex_Red_ST,_Tex_Green_ST,_Tex_Blue_ST,_Tex_Alpha_ST,_Tex_Black_ST;

			struct appData
			{
				float4 vertex	:	POSITION;
				float2 texcoord	:	TEXCOORD0;				
				float2 texcoord1:	TEXCOORD1;				
			};

			struct v2f
			{
				float4 pos		:	SV_POSITION;
				float4 UV		:	ATTR0;
				float2 fog		:	ATTR1;
			};

			v2f vert(appData v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.UV.xy = v.texcoord;
				o.UV.zw = v.texcoord1;
				o.fog.x = saturate(_FogDistance*length(v.vertex.xyz-_WorldSpaceCameraPos));
				o.fog.x *= saturate(_FogMaxHeight*(_FogMinHeight-v.vertex.y));
				o.fog.x *= o.fog.x;
				o.fog.y = dot(-_SunDir,normalize(v.vertex.xyz-_WorldSpaceCameraPos))*.5+.5;
				return o;
			}
			
			half4 frag(v2f i) : SV_Target
			{
				half4 o;
				half4 splat = tex2D(_SplatMap, i.UV.xy);
				half3 shadeMap = tex2D(_ShadeMap, i.UV.xy).rgb*_ShadeScale;
				float2 lightmapUV = i.UV.zw * unity_LightmapST.xy + unity_LightmapST.zw;
				half3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, lightmapUV)) *_LightmapHDRFactor;
				half4 Tex_Black = tex2D(_Tex_Black, i.UV.zw * _Tex_Black_ST).rgba;
				half4 Tex_Alpha = tex2D(_Tex_Alpha, i.UV.zw * _Tex_Alpha_ST ).rgba;
				half4 Tex_Blue = tex2D(_Tex_Blue, i.UV.zw * _Tex_Blue_ST ).rgba;
				half4 Tex_Green = tex2D(_Tex_Green, i.UV.zw * _Tex_Green_ST ).rgba;
				half4 Tex_Red = tex2D(_Tex_Red, i.UV.zw * _Tex_Red_ST ).rgba;
				
				//note: splat map is pre-normalized in world-machine
				half3 diff = Tex_Black.rgb * saturate(1-(dot(splat,half4(1,1,1,1))));
				diff += Tex_Alpha.rgb * splat.a;
				diff += Tex_Blue.rgb * splat.b;
				diff += Tex_Green.rgb * splat.g;
				diff += Tex_Red.rgb * splat.r;
				
				half3 result = diff * lightmap * shadeMap;
				
				o.rgb = lerp(result, lerp(_FogColor,_SunColor,(half)i.fog.y), (half)i.fog.x);
				
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
	} 
}
