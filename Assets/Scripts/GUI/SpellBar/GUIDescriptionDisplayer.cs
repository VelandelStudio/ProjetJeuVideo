using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIDescriptionDisplayer, public class
 * This class contains the methods that enable and disable the GUI elements for descriptions.
 **/
public class GUIDescriptionDisplayer : MonoBehaviour
{
    [SerializeField] private Image _imageZone;
    [SerializeField] private Text _textZone;
    [SerializeField] private Image _element;
    [SerializeField] private Image _type;
    [SerializeField] private Image _typeAttack;

    /** Start, private void
	 * The start Method is used to disableevery graphic elements when there are not used.
	 **/
    private void Start()
    {
        _imageZone.enabled = false;
        _textZone.enabled = false;
        _element.enabled = false;
        _type.enabled = false;
        _typeAttack.enabled = false;
    }

    /** DisplayDescription, public void
	 * Enable graphic elements and display the description of the displayable.
	 * Also enable the element and type images.
	 **/
    public void DisplayDescription(Behaviour obj)
    {
        if (obj is IDisplayer)
        {
            IDisplayer displayer = (IDisplayer)obj;
            _imageZone.enabled = true;
            _textZone.enabled = true;
            _textZone.text = displayer.Displayable.GetDescriptionGUI();
            SetElementSprite(displayer.Displayable.Element);
            SetTypeSprite(displayer.Displayable.DamagesType);
            SetTypeAttack(displayer.Displayable.Type);
        }
    }

    /** CancelDescriptionOnScreen, public void
     * @Params : Text, Image, string
     * Disables text and images for descriptions on the screen and reset the description inside the text field.
     **/
    public void CancelDescription()
    {
        Start();
    }

    /** SetElementSprite,private void
     * @Params : string
     * Get the string element name of a Displayer element inside parameters and attributes the corresponding sprite on the screen.
     **/
    private void SetElementSprite(string element)
    { 
        if (element != null && element != "")
        {
            _element.sprite = Resources.Load<Sprite>("Images/Elements/" + element);
            _element.enabled = true;
        }
    }

    /** SetTypeAttack,private void
     * @Params : string
     * Get the string element name of a Displayer element inside parameters and attributes the corresponding sprite on the screen.
     **/
    private void SetTypeAttack(string typeAttack)
    {
        if (typeAttack != null && typeAttack !="")
        {
            _typeAttack.sprite = Resources.Load<Sprite>("Images/Types/" + typeAttack);
            _typeAttack.enabled = true;
        }
    }

    /** SetTypeSprite, private static void
     * @Params : string
     * Get the string element name of a Displayer element inside parameters and attributes the corresponding sprite on the screen.
     **/
    private void SetTypeSprite(string[] type)
    {
        string typeToLoad = StringHelper.GetDisplayableType(type);
        if (typeToLoad != "")
        {
            _type.sprite = Resources.Load<Sprite>("Images/Types/" + typeToLoad);
            _type.enabled = true;
        }
    }
}