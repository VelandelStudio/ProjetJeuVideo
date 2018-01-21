// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Skydome shader for GPDQ9 Version 1.0
// (c) 2016 Franck Sauer

Shader "Custom/SkyDome"
{

	Properties
	{
		[NoScaleOffset] _MainTex ("Base (RGB)", 2D) = "" {}
		_Exposure("Exposure",Range(1,4)) = 2
	}

	SubShader
	{
		Tags { "Queue" = "Geometry-5" }
		Pass
		{
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;

			half3 _SunColor, _SkyColor, _FogColor, _SunDir;
			half _SkyFogMinHeight, _SkyFogMaxHeight, _SkyFogDistance, _Exposure;
			
			struct appData
			{
				float4 vertex	:	POSITION;
				float4 texcoord	:	TEXCOORD0;				
			};

			struct v2f
			{
				float4 pos		:	SV_POSITION;
				float4 UV_fog	:	TEXCOORD0;
			};
			
			v2f vert(appData i)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (i.vertex);
				o.UV_fog.xy = i.texcoord.xy;
				o.UV_fog.z = saturate(_SkyFogDistance*length(i.vertex.xyz-_WorldSpaceCameraPos)); //_SkyFogDistance = 1/_SkyFogDistance
				o.UV_fog.z *= saturate(_SkyFogMaxHeight*(_SkyFogMinHeight-i.vertex.y)); //_SkyFogMaxHeight = 1/_SkyFogMaxHeight
				o.UV_fog.z *= o.UV_fog.z;
				o.UV_fog.w = dot(-_SunDir,normalize(i.vertex.xyz-_WorldSpaceCameraPos))*.5+.5;
				return o;
			}
			
			half4 frag(v2f i) : SV_Target
			{
				half4 o;
				half3 result = tex2D(_MainTex, i.UV_fog.xy) * _Exposure;
			
				o.rgb = lerp(result, lerp(_FogColor,_SunColor,(half)i.UV_fog.w), (half)i.UV_fog.z);
				
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
	} 
}

