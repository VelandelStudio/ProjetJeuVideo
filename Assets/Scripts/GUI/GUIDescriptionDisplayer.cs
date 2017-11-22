using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDescriptionDisplayer : MonoBehaviour {

	public static void DisplayDescriptionOnScreen(Text textZone, Image imageZone, string rawDescription)
	{
		textZone.text = rawDescription;
		imageZone.enabled = true;
        textZone.enabled = true;
	}
	
	public static void CancelDescriptionOnScreen(Text textZone, Image imageZone)
	{
		textZone.enabled = false;
        imageZone.enabled = false;
        textZone.text = " ";
	}
}
