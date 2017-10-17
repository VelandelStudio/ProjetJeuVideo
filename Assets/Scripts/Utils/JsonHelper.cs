using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/** JsonHelper public class.
 * This class should be used in a static context in order to convert a simple Array in a json file into a complexe array.
 * As an exemple, we convert arrays like that : [ {"key":"value"}] -> "array":{["key":"value"]}
 **/
public class JsonHelper
{
	/** getJsonArray public static class.
	 * Convert a Json Simple Array to Complex Array with a wrapper instance. 
	 **/
	public static T[] getJsonArray<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
		return wrapper.array;
	}
 
	 /** Wrapper private Serializable class.
	 * This Wrapper class is designed to only serve the JsonHelper class.
	 * It is used as a Wrapper to make a Json Array fit with the await of JsonUtility tool in Unity.
	 **/
	[Serializable]
	private class Wrapper<T>
	{
		public T[] array = null;
	}
}
