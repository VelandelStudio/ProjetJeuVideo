using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIAutoAttackDisplayer : MonoBehaviour
{

    #region Fields
    private AutoAttackBase _autoAttackBase;

    [SerializeField] private Image _autoAttackImgDescription;
    [SerializeField] private Image _autoAttackCD;

    private Text _autoAttackDescription;
    public AutoAttackBase GetAutoAttack()
    {
        return _autoAttackBase;
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
    #endregion
}