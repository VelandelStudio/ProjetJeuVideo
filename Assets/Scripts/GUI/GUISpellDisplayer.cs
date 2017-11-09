using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUISpellDisplayer class
 * This script should be associated to GUI Slots that represents Spells on the screen.
 * This script is used to display images, CD, descriptions and every informations we have about spells.
 **/
public class GUISpellDisplayer : MonoBehaviour
{

    /** Fields contains a Spell, associated to the Displayer and two images.
	 * The _CDSpellImage is a rotative image filler that is filled by the CD value of the spell.
	 * The _spellAvailableForGUI is a simple image that shoudkl be activated when the spell is not activable at any conditions (stuns for example)
	 **/
    private Spell _spell;
    private string _spellDescription;
    [SerializeField] private Image _spellImgDescription;
    [SerializeField] private Image _CDSpellImage;
    [SerializeField] private Image _spellAvailableForGUI;
    private Text _spellTextDescription;

    /** Start private void Method
	 * The start method de-activate the _spellAvailableForGUI component by default.
	 * Basically, the spell Image is only CD dependant.
	 **/
    private void Start()
    {
        _spellAvailableForGUI.enabled = false;
        _spellTextDescription = _spellImgDescription.GetComponentInChildren<Text>();
        _spellImgDescription.enabled = false;
        _spellTextDescription.enabled = false;
    }

    /** Update private void Method
	 * We ensure that _spellAvailableForGUI is always equal, to the AvailableForGUI() method in the Spell associated.
	 * We also ensure that the fillAmount of _CDSpellImage is always equal to the current CD of the spell associated. 
	 * Please note that the fillAmount must be [0;1] this is why we divide the currentCD by the SpellCD.
	 **/
    private void Update()
    {
        _spellAvailableForGUI.enabled = !_spell.AvailableForGUI();
        _CDSpellImage.fillAmount = _spell.currentCD / _spell.spellCD;
    }

    /** AttributeSpellToGUI public void Method
	 * @Params : Spell
	 * This public method should only be called by the Character script so far.
	 * In this method, we attribute to the Gui the correct spell.
	 * Then, we try to locate a Sprite associated to the spell in the Image Folder associated to the Character.
	 **/
    public void AttributeSpellToGUI(Spell spell, string description)
    {
        _spell = spell;
        _spellDescription = description;
        Image spellImage = GetComponent<Image>();
        spellImage.sprite = Resources.Load<Sprite>("Images/Spells/" + _spell.GetComponent<Character>().GetType().ToString() + "/" + _spell.GetType());
        if (spellImage.sprite == null)
        {
            spellImage.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }
    }

    public void Enter()
    {
        _spellTextDescription.text = _spellDescription;
        _spellImgDescription.enabled = true;
        _spellTextDescription.enabled = true;
    }

    public void Exit()
    {
        _spellTextDescription.enabled = false;
        _spellImgDescription.enabled = false;
        _spellTextDescription.text = " ";
    }
}