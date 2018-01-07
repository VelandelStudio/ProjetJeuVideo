using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIPassiveDisplayer, public class
 * This Class is attached to the GUI elements that handles the Behaviour of the PassiveSpell of the Player.
 **/
public class GUIPassiveDisplayer : MonoBehaviour, IDisplayer
{

    /** Fields of GUIPassiveDisplayer
     * Field contains SerializedFields that are element that must be attached directly via the Editor.
     * You must have An image for the Passive, and a background for the stacks of the passive (if it is stackable).
     * Also, you must have an instance of the Passive your are in charge of.
     **/
    #region Fields

    [SerializeField] private Image _passiveImgDescription;
    [SerializeField] private Text _stackText;
    [SerializeField] private Image _stackBackground;
    private Text _passiveDescription;
    private PassiveBase _passive;

    public IDisplayable Displayable
    {
        get { return _passive; }
        protected set { }
    }
    #endregion

    #region Functionnal methods
    /** Start private void Method
	 * The start method de-activate the description components by default.
	 **/
    private void Start()
    {
        _passiveDescription = _passiveImgDescription.GetComponentInChildren<Text>();
        _passiveDescription.enabled = false;
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

    /** AttributeDisplayable public void Method
	 * @Param : IDisplayable
	 * This method should be only called by the Champion class when the class is constructed.
	 * We get every elements we need to displays informations to the screen (Image, number of stacks, instance of Passive etc..)
	 **/
    public void AttributeDisplayable(IDisplayable displayable)
    {
        _passive = (PassiveBase)displayable;
        _passiveImgDescription.sprite = Resources.Load<Sprite>("Images/Passive/" + _passive.GetComponent<Champion>().GetType().ToString() + "/" + _passive.GetType());
        if (_passiveImgDescription.sprite == null)
        {
            _passiveImgDescription.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }

        if (_passive.NumberOfStacks > 0)
        {
            _stackText.enabled = true;
            _stackBackground.enabled = true;
        }
    }
    #endregion
}
