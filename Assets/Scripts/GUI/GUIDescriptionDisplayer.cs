using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIDescriptionDisplayer, public class
 * This class contains the methods that enable and disable the GUI elements for descriptions.
 **/
public class GUIDescriptionDisplayer : MonoBehaviour {

    /** DisplayDescriptionOnScreen, public static void
     * @Params : Text, Image, string
     * Enables text and images for descriptions on the screen and places the description inside the text field.
     **/
    public static void DisplayDescriptionOnScreen(Text textZone, Image imageZone, string rawDescription)
	{
		textZone.text = rawDescription;
		imageZone.enabled = true;
        textZone.enabled = true;
	}
    /** CancelDescriptionOnScreen, public static void
     * @Params : Text, Image, string
     * Disables text and images for descriptions on the screen and reset the description inside the text field.
     **/
    public static void CancelDescriptionOnScreen(Text textZone, Image imageZone)
	{
		textZone.enabled = false;
        imageZone.enabled = false;
        textZone.text = "";
	}
}
