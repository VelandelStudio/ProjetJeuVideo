using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPassiveDisplayer : MonoBehaviour {

	[SerializeField] private Image _PassiveImgDescription;
    private Text _PassiveDescription;
	
	/** Start private void Method
	 * The start method de-activate the descriptions component by default.
	 **/
    private void Start()
    {
        _PassiveDescription = _PassiveImgDescription.GetComponentInChildren<Text>();
        _PassiveImgDescription.enabled = false;
		_PassiveDescription.enabled = false;
	}
	
	/** MouseEnter, public void Method
	 * This Method is launched with an event trigger when the mouse enters the Passive icon on the screen
	**/
    public void MouseEnter()
	{
		GUIDescriptionDisplayer.DisplayDescriptionOnScreen(_PassiveDescription,_PassiveImgDescription, "hello world!"); //::_spell.GetDescriptionGUI());
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the Passive icon on the screen
	**/
    public void MouseExit()
    {
		GUIDescriptionDisplayer.CancelDescriptionOnScreen(_PassiveDescription, _PassiveImgDescription);
    }
}
