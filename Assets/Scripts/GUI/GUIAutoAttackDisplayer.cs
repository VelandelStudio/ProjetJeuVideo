using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIAutoAttackDisplayer, public class
 * @Implements : IDisplayer
 * This Class is attached to the GUI elements that handles the Behaviour of the AutoAttackBase of the Player.
 **/
public class GUIAutoAttackDisplayer : MonoBehaviour, IDisplayer
{

    #region Fields
    private AutoAttackBase _autoAttackBase;

    [SerializeField] private Image _autoAttackImgDescription;
    [SerializeField] private Image _autoAttackCD;

    private Text _autoAttackDescription;
    public IDisplayable Displayable
    {
        get { return _autoAttackBase; }
        protected set { }
    }
    #endregion

    #region Functionnal Methods	
    /** Start private void Method
	 * The start method de-activate the descriptions component by default.
	 **/
    private void Start()
    {
        _autoAttackDescription = _autoAttackImgDescription.GetComponentInChildren<Text>();
        _autoAttackImgDescription.enabled = false;
        _autoAttackDescription.enabled = false;
    }

    /** Update, private void Method
	 * This method is used to get the CurrentCD/CoolDownValue of the AutoAttack and set the resul to the fillAmount of the Image.
	 **/
    private void Update()
    {
        _autoAttackCD.fillAmount = _autoAttackBase.CurrentCD / _autoAttackBase.CoolDownValue;
    }

    /** AttributeDisplayable public void Method
	 * @Params : IDisplayable
	 * This public method should only be called by the Character script so far.
	 * In this method, we attribute to the Gui the correct autoAttack.
	 * Then, we try to locate a Sprite associated to the autoAttack in the Image Folder associated to the Character.
	 **/
    public void AttributeDisplayable(IDisplayable displayable)
    {
        _autoAttackBase = (AutoAttackBase)displayable;
        Image autoAttackBaseImage = GetComponent<Image>();
        autoAttackBaseImage.sprite = Resources.Load<Sprite>("Images/AutoAttacks/" + _autoAttackBase.GetComponent<Character>().GetType().ToString() + "/" + _autoAttackBase.GetType());
        if (autoAttackBaseImage.sprite == null)
        {
            autoAttackBaseImage.sprite = Resources.Load<Sprite>("Images/Spells/DefaultSpell");
        }
    }
    #endregion
}