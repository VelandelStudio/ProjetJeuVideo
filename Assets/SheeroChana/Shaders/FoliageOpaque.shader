// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Folliage opaque shader for GPD-Q9 2.1
// (c) Franck Sauer 2016

//vertex color is used to control foliage movement: R->along normal vector, G->along tangent vector, B->not used, A->not used
//use this shader for trunc, branches or dense folliage.

//update
//2.1 added _LightmapHDRFactor to compensate for lack of HDR lightmaps on android

Shader "Custom/FolliageOpaque"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Diffuse Texture", 2D) = "" {}
		_windScale ("WindScale", Range(0,1)) = 0.2
		_windFreq ("WindFreq", Vector) = (100,80,1,0) //normal freq, binormal freq, phase Y pos scaler, phase XZ scaler
	}
	SubShader
	{
		Tags {"Queue" = "Geometry" }
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			
			half4  _windFreq;
			half3 _SunColor, _FogColor, _SunDir;
			half _FogMinHeight, _FogMaxHeight, _FogDistance, _windScale, _LightmapHDRFactor;

			struct appData
			{
				float4 vertex	:	POSITION;
				half3 normal	:	NORMAL;
				float4 tangent	:	TANGENT;
				float2 texcoord	:	TEXCOORD0;
				float2 texcoord1:	TEXCOORD1;				
				float4 color	:	COLOR;
			};

			struct v2f
			{
				float4 pos		:	SV_POSITION;
				float4 UV		:	ATTR0;
				float2 fog		:	ATTR1;
				half3 N			:	ATTR2;
			};
			
			v2f vert(appData v)
			{
				v2f o;
				v.color *= _windScale;
				half phase = v.vertex.y * _windFreq.z + (v.vertex.x + v.vertex.z) * _windFreq.w;
				v.vertex.xyz += v.normal.xyz * cos(_Time.x * _windFreq.x + phase ) * v.color.r;
				v.vertex.xyz += v.tangent.xyz * cos(_Time.x * _windFreq.y + phase ) * v.color.g;

				o.pos = UnityObjectToClipPos(v.vertex);
				half3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				o.N = mul(unity_ObjectToWorld, half4(v.normal,0)).xyz;

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
				
				half3 N = normalize(i.N);
				half3 L = -_SunDir;
				
				half3 baseColor = tex2D(_MainTex, i.UV.xy).rgb;
				half3 lightmap = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.UV.zw))*_LightmapHDRFactor;

				half3 result = baseColor.rgb * lightmap;
				
				o.rgb = lerp(result, lerp(_FogColor,_SunColor,(half)i.fog.y), (half)i.fog.x);
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
	} 
}

