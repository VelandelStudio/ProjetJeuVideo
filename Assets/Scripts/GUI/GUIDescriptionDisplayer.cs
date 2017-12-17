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

    /** Start, private void
	 * The start Method is used to disableevery graphic elements when there are not used.
	 **/
    private void Start()
    {
        _imageZone.enabled = false;
        _textZone.enabled = false;
        _element.enabled = false;
        _type.enabled = false;
    }

    /** DisplaySpellDescription, public void
	 * Enable graphic elements and display the description of the spell inside the spellDisplayer.
	 * Also enable the element and type images.
	 **/
    public void DisplaySpellDescription(GUISpellDisplayer spellDisplayer)
    {
        Spell spell = spellDisplayer.GetSpell();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = spell.GetDescriptionGUI();
        SetElementSprite(spell.Element);
        SetTypeSprite(spell.DamagesType);
    }

    /** DisplayAutoAttackDescription, public void
	 * Enable graphic elements and display the description of the autoAttack inside the autoAttackDisplayer.
	 * Also enable the element and type images.
	 **/
    public void DisplayAutoAttackDescription(GUIAutoAttackDisplayer autoAttackDisplayer)
    {
        AutoAttackBase autoAttack = autoAttackDisplayer.GetAutoAttack();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = autoAttack.GetDescriptionGUI();
        SetElementSprite(autoAttack.Element);
        SetTypeSprite(autoAttack.DamagesType);
    }

    /** DisplayPassiveDescription, public void
	 * Enable graphic elements and display the description of the passive inside the passiveDisplayer.
	 * Also enable the element and type images.
	 **/
    public void DisplayPassiveDescription(GUIPassiveDisplayer passiveDisplayer)
    {
        PassiveBase passive = passiveDisplayer.GetPassive();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = passive.GetDescriptionGUI();
        SetTypeSprite(passive.DamagesType);
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
        if (element != null)
        {
            _element.sprite = Resources.Load<Sprite>("Images/Elements/" + element);
            _element.enabled = true;
        }
    }

    /** SetTypeSprite, private static void
     * @Params : string
     * Get the string element name of a Displayer element inside parameters and attributes the corresponding sprite on the screen.
     **/
    private void SetTypeSprite(string[] type)
    {
        if (type.Length > 0)
        {
            string prevType = type[0];
            foreach (string t in type)
            {
                if (t != prevType)
                {
                    prevType = "Hybride";
                    break;
                }
            }

            if (prevType == "m") { prevType = "Magical"; }
            if (prevType == "p") { prevType = "Physical"; }

            _type.sprite = Resources.Load<Sprite>("Images/Types/" + prevType);
            _type.enabled = true;
        }
    }
}