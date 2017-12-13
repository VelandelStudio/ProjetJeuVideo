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
        SetElementSprite(spell.Element);
        SetTypeSprite(spell.DamagesType);
    }

    public void DisplayAutoAttackDescription(GUIAutoAttackDisplayer autoAttackDisplayer)
    {
        AutoAttackBase autoAttack = autoAttackDisplayer.GetAutoAttack();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = autoAttack.GetDescriptionGUI();
        SetElementSprite(autoAttack.Element);
        SetTypeSprite(autoAttack.DamagesType);
    }

    public void DisplayPassiveDescription(GUIPassiveDisplayer passiveDisplayer)
    {
        PassiveBase passive = passiveDisplayer.GetPassive();
        _imageZone.enabled = true;
        _textZone.enabled = true;
        _textZone.text = passive.GetDescriptionGUI();
        SetTypeSprite(passive.DamagesType);
    }

    /** CancelDescriptionOnScreen, public static void
     * @Params : Text, Image, string
     * Disables text and images for descriptions on the screen and reset the description inside the text field.
     **/
    public void CancelDescription()
    {
        Start();
    }

    private void SetElementSprite(string element)
    {
        if (element != null)
        {
            _element.sprite = Resources.Load<Sprite>("Images/Elements/" + element);
            _element.enabled = true;
        }
    }

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
