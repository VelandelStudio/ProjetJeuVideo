using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIAutoAttackDisplayer : MonoBehaviour {

    private AutoAttackBase _autoAttackBase;

    [SerializeField] private Image _autoAttackImgDescription;
    private Text _autoAttackDescription;
	
	/** Start private void Method
	 * The start method de-activate the descriptions component by default.
	 **/
    private void Start()
    {
        _autoAttackDescription = _autoAttackImgDescription.GetComponentInChildren<Text>();
        _autoAttackImgDescription.enabled = false;
		_autoAttackDescription.enabled = false;
	}
	
	/** AttributeAutoAttackToGUI public void Method
	 * @Params : AutoAttackBase
	 * This public method should only be called by the Character script so far.
	 * In this method, we attribute to the Gui the correct autoAttack.
	 * Then, we try to locate a Sprite associated to the autoAttack in the Image Folder associated to the Character.
	 **/
    public void AttributeAutoAttackToGUI(AutoAttackBase autoAttackBase)
    {
        _autoAttackBase = autoAttackBase;
        Image autoAttackBaseImage = GetComponent<Image>();
        autoAttackBaseImage.sprite = Resources.Load<Sprite>("Images/AutoAttacks/" + _autoAttackBase.GetComponent<Character>().GetType().ToString() + "/" + _autoAttackBase.GetType());
        if (autoAttackBaseImage.sprite == null)
        {
            autoAttackBaseImage.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }
    }
	
	/** MouseEnter, public void Method
	 * This Method is launched with an event trigger when the mouse enters the autoAttack icon on the screen
	**/
    public void MouseEnter()
	{
		GUIDescriptionDisplayer.DisplayDescriptionOnScreen(_autoAttackDescription,_autoAttackImgDescription, _autoAttackBase.GetDescriptionGUI());
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the autoAttack icon on the screen
	**/
    public void MouseExit()
    {
		GUIDescriptionDisplayer.CancelDescriptionOnScreen(_autoAttackDescription, _autoAttackImgDescription);
    }
}