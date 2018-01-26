using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FogAndLightSetup : MonoBehaviour {

	public Transform MainLight;

	[Range(1.0f,4.0f)]	public float LightmapHDRFactor = 1.0f;

	public Color FogColor;
	[Range(0.0f,500f)]	public float FogMinHeight = 0.0f;
	[Range(0.0f,500f)]	public float FogMaxHeight = 400.0f;
	[Range(0.0f,500f)]	public float FogDistance = 50.0f;
	[Range(0.0f,8000f)]	public float SkyFogMinHeight = 0.0f;
	[Range(0.0f,8000f)]	public float SkyFogMaxHeight = 4000.0f;
	[Range(0.0f,8000f)]	public float SkyFogDistance = 1000.0f;
	

	void Start()
	{
		updateLight();
	}

	#if UNITY_EDITOR
	//lighting and fog patametters will only be updated in-editor
	//they will be static in-game

	void Update ()
	{
		updateLight();
	}
	#endif

	void updateLight()
	{
		Shader.SetGlobalFloat ("_LightmapHDRFactor", LightmapHDRFactor);
		Shader.SetGlobalColor("_FogColor", FogColor);
		Shader.SetGlobalFloat("_FogMinHeight", FogMinHeight);
		Shader.SetGlobalFloat("_FogMaxHeight", 1/FogMaxHeight);
		Shader.SetGlobalFloat("_FogDistance", 1/FogDistance);

		Shader.SetGlobalFloat("_SkyFogMinHeight", SkyFogMinHeight);
		Shader.SetGlobalFloat("_SkyFogMaxHeight", 1/SkyFogMaxHeight);
		Shader.SetGlobalFloat("_SkyFogDistance", 1/SkyFogDistance);
				
		Shader.SetGlobalColor("_SunColor", MainLight.GetComponent<Light>().color);
		Shader.SetGlobalFloat("_SunIntensity", MainLight.GetComponent<Light>().intensity);

		Shader.SetGlobalVector("_SunDir", MainLight.transform.forward);
	}
}
