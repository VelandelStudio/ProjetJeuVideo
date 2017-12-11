using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIDescriptionDisplayer, public class
 * This class contains the methods that enable and disable the GUI elements for descriptions.
 **/
public class GUIDescriptionDisplayer : MonoBehaviour {
    [SerializeField] private Image _imageZone;
    [SerializeField] private Text _textZone;
    [SerializeField] private Image _element;
    [SerializeField] private Image _type;

    private void Start()
    {
        _imageZone.enabled = false;
        _textZone.enabled = false;
        _element.enabled = false;
        _type.enabled = false;
    }

    public void DisplayDescription(string description)
    {
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = description;
        _element.enabled = false;
        _type.enabled = false;
    }

    public void DisplaySpellDescription(GUISpellDisplayer spellDisplayer)
    {
        Spell spell = spellDisplayer.GetSpell();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = spell.GetDescriptionGUI();
        _element.enabled = spell.Element != null;
        _type.enabled = spell.DamagesType.Length > 0;
    }

    public void DisplayAutoAttackDescription(GUIAutoAttackDisplayer autoAttackDisplayer)
    {
        AutoAttackBase autoAttack = autoAttackDisplayer.GetAutoAttack();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = autoAttack.GetDescriptionGUI();
        _element.enabled = autoAttack.Element != null;
        _type.enabled = autoAttack.DamagesType.Length > 0;
    }

    public void DisplayPassiveDescription(GUIPassiveDisplayer passiveDisplayer)
    {
        PassiveBase passive = passiveDisplayer.GetPassive();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = passive.GetDescriptionGUI();
        _type.enabled = passive.DamagesType.Length > 0;
    }

    /** CancelDescriptionOnScreen, public static void
     * @Params : Text, Image, string
     * Disables text and images for descriptions on the screen and reset the description inside the text field.
     **/
    public void CancelDescription()
	{
        Start();
	}
}
