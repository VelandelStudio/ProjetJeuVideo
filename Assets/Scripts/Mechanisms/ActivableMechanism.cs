using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ActivableMechanism, public abstract class
 * @extends : Mechanism
 * This class represents mechanism that shoudl be activated by the player. These mechanisms can not activate themselves.
 **/
public abstract class ActivableMechanism : Mechanism {

	/** AttributeMechanismObject, protected override void Method
	 * Launched by the base.Start, we instantiate as a child GameObject an ActivableMechanism that has a ActivableMechanismDetector.
	 * We notify the ActivableMechanismDetector that we are the Mechanism associated to it.
	 **/
	protected override void AttributeMechanismObject()
	{
		mechanismObject = (GameObject)Resources.Load("Mechanisms/ActivableMechanism");
		mechanismObject = Instantiate(mechanismObject, transform);
		mechanismObject.GetComponent<ActivableMechanismDetector>().SetInetrractableParent(this);
	}
	
	/** DisplayTextOfInterractable, public override void 
	 * We tell to the player which button to press to activate the mechanism
	 **/
	public override void DisplayTextOfInterractable()
	{
		Debug.Log("Press " + InputsProperties.Activate.ToString() + " to activate.");
	}
	
	/** CancelTextOfInterractable, public override void 
	 * Does nothing currently but will handle the Behaviour of the GUI.
	 **/
	public override void CancelTextOfInterractable(){}
	
	/** ActivateInterractable, public override abstract void 
	 * The behaviour of the ActivableMechanism will be set in the child script.
	 **/
	public override abstract void ActivateInterractable();
}
