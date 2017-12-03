using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/** GUIPassiveDisplayer, public class
 * This Class is attached to the GUI elements that handles the Behaviour of the PassiveSpell of the Player.
 **/
public class GUIPassiveDisplayer : MonoBehaviour
{

    /** Fields of GUIPassiveDisplayer
     * Field contains SerializedFields that are element that must be attached directly via the Editor.
     * You must have An image for the Passive, and a background for the stacks of the passive (if it is stackable).
     * Also, you must have an instance of the Passive your are in charge of.
     **/
    #region Fields

    [SerializeField] private Image _PassiveImgDescription;
    [SerializeField] private Text _stackText;
    [SerializeField] private Image _stackBackground;
    private Text _PassiveDescription;
    private PassiveBase _passive;

    #endregion

    #region Functionnal methods
    /** Start private void Method
	 * The start method de-activate the description components by default.
	 **/
    private void Start()
    {
        _PassiveDescription = _PassiveImgDescription.GetComponentInChildren<Text>();
        _PassiveImgDescription.enabled = false;
        _PassiveDescription.enabled = false;
        _stackBackground.enabled = false;
        _stackText.enabled = false;
    }

    /** Update private void Method
	 * This Method only concerns Passive that have a Stackable Behaviour. 
	 * It displays the number of stacks on the passive Image.
	 **/
    private void Update()
    {
        if (_passive.NumberOfStacks > 0)
        {
            _stackText.text = _passive.NumberOfStacks.ToString();
            _stackBackground.enabled = true;
            _stackText.enabled = true;
        }
        else
        {
            _stackText.enabled = false;
            _stackBackground.enabled = false;
        }
    }

    /** AttributeSpellToGUI public void Method
	 * This method should be only called by the Character class when the class is constructed.
	 * We get every elements we need to displays informations to the screen (Image, number of stacks, instance of Passive etc..)
	 **/
    public void AttributePassiveToGUI(PassiveBase passive)
    {
        _passive = passive;
        Image passiveImage = GetComponent<Image>();
        passiveImage.sprite = Resources.Load<Sprite>("Images/Passive/" + _passive.GetComponent<Character>().GetType().ToString() + "/" + _passive.GetType());
        if (passiveImage.sprite == null)
        {
            passiveImage.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }

        if (passive.NumberOfStacks > 0)
        {
            _stackText.enabled = true;
            _stackBackground.enabled = true;
        }
    }
    #endregion

    #region TriggerEvents	
    /** MouseEnter, public void Method
	 * This Method is launched with an event trigger when the mouse enters the Passive icon on the screen
	**/
    public void MouseEnter()
    {
        GUIDescriptionDisplayer.DisplayDescriptionOnScreen(_PassiveDescription, _PassiveImgDescription, _passive.GetDescriptionGUI());
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the Passive icon on the screen
	**/
    public void MouseExit()
    {
        GUIDescriptionDisplayer.CancelDescriptionOnScreen(_PassiveDescription, _PassiveImgDescription);
    }
    #endregion
}
