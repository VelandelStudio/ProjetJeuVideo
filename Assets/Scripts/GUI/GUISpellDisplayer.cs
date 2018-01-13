using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUISpellDisplayer class
 * @Implements : IDisplayer
 * This script should be associated to GUI Slots that represents Spells on the screen.
 * This script is used to display images, CD, descriptions and every informations we have about spells.
 **/
public class GUISpellDisplayer : MonoBehaviour, IDisplayer
{
    /** Fields contains a Spell, associated to the Displayer and two images.
	 * The _CDSpellImage is a rotative image filler that is filled by the CD value of the spell.
	 * The _spellAvailableForGUI is a simple image that should be activated when the spell is not activable at any conditions (stuns for example)
	 **/
    private Spell _spell;
    [SerializeField] private Image _spellImgDescription;
    [SerializeField] private Image _CDSpellImage;
    [SerializeField] private Image _spellAvailableForGUI;
    [SerializeField] private Text _timerCDText;
    [SerializeField] private Image _stackImage;
    [SerializeField] private Image _backgroundStack;

    [SerializeField] private Text _stackText;
    private Text _spellTextDescription;
    private int _numberOfStacks;

    public IDisplayable Displayable
    {
        get { return _spell; }
        protected set { }
    }

    /** Awake private void Method
	 * The Awake method de-activate the _spellAvailableForGUI component by default.
	 * Basically, the spell Image is only CD dependant.
	 **/
    private void Awake()
    {
        _spellAvailableForGUI.enabled = false;
        _spellTextDescription = _spellImgDescription.GetComponentInChildren<Text>();
        _spellImgDescription.enabled = false;
        _spellTextDescription.enabled = false;
        _timerCDText.enabled = false;
        _stackText = _stackImage.GetComponentInChildren<Text>();
        _stackText.enabled = false;
        _stackImage.enabled = false;
        _backgroundStack.enabled = false;
    }

    /** Update private void Method
	 * We ensure that _spellAvailableForGUI is always equal, to the AvailableForGUI() method in the Spell associated.
	 * We also ensure that the fillAmount of _CDSpellImage is always equal to the current CD of the spell associated. 
	 * Please note that the fillAmount must be [0;1] this is why we divide the currentCD by the CoolDownValue.
	 **/
    private void Update()
    {
        _spellAvailableForGUI.enabled = !_spell.AvailableForGUI();
        if (_spell.IsUnderGCD)
        {
            _CDSpellImage.fillAmount = _spell.CurrentCD / _spell.SpellGCD;
        }
        else
            _CDSpellImage.fillAmount = _spell.CurrentCD / _spell.CoolDownValue;

        _timerCDText.text = ((int)_spell.CurrentCD + 1).ToString();
        _timerCDText.enabled = _CDSpellImage.fillAmount != 0 || _spell.CoolDownValue < _spell.SpellGCD;
        if (_spell is StackableSpell)
        {
            StackableSpell spell = (StackableSpell)_spell;
            _stackText.text = spell.CurrentNumberOfStacks.ToString();
            _stackImage.enabled = spell.CurrentNumberOfStacks > 0;
            _stackImage.fillAmount = spell.CurrentStackCD / spell.StackCD;
        }
    }

    /** AttributeDisplayable public void Method
	 * @Params : IDisplayable
	 * This public method should only be called by the Champion script so far.
	 * In this method, we attribute to the Gui the correct spell.
	 * Then, we try to locate a Sprite associated to the spell in the Image Folder associated to the Champion.
	 **/
    public void AttributeDisplayable(IDisplayable displayable)
    {
        _spell = (Spell)displayable;
        Image spellImage = GetComponent<Image>();
        spellImage.sprite = Resources.Load<Sprite>("Images/Spells/" + _spell.GetComponent<Champion>().GetType().ToString() + "/" + _spell.GetType());
        if (spellImage.sprite == null)
        {
            spellImage.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }

        if (_spell is StackableSpell)
        {
            _stackText.enabled = true;
            _stackImage.enabled = true;
            _backgroundStack.enabled = true;
        }
    }
}