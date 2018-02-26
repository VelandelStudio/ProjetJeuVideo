using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** AutoAttackBase abstract class.
 * @implements : ISpellDisplayable, 
 * This abstract class is the mother class of all AutoAttack in our game. 
 * This class handles the behaviour of the CD of all AutoAttacks. 
 * It also contains the AutoAttack method launched by the Champion.
 **/
public abstract class AutoAttackBase : MonoBehaviour, ISpellDisplayable
{
    #region Fields
    public AutoAttackData _autoAttackData { get; protected set; }
    public string Name { get { return _autoAttackData.Name; } protected set { } }
    public string Element { get { return _autoAttackData.Element; } protected set { } }
    public string Type { get { return _autoAttackData.Type; } protected set { } }
    public float CoolDownValue { get { return _autoAttackData.CoolDownValue; } protected set { } }
    public int[] Damages { get { return _autoAttackData.Damages; } protected set { } }
    public string[] DamagesType { get { return _autoAttackData.DamagesType; } protected set { } }
    public string[] OtherValues { get { return _autoAttackData.OtherValues; } protected set { } }
    public GameObject[] Status { get { return _autoAttackData.Status; } protected set { } }
    public string[] Description { get { return _autoAttackData.Description; } protected set { } }
    public bool IsLoaded { get { return _autoAttackData.IsLoaded; } protected set { } }


    public float CurrentCD;
    protected Champion champion;
    #endregion

    #region Functionnal Methods

    /** Awake protected virtual void Method,
	 * The Awake method is used to create an instance of AutoAttackData which is responsible of the JSON Loading.
	 **/
    protected void Awake()
    {
        champion = GetComponentInParent<Champion>();
        _autoAttackData = new AutoAttackData(GetType().ToString());
    }

    /** Start protected virtual void Method,
	 * Initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        CurrentCD = 0;
    }

    /** Update protected virtual void Method,
	 * If the auto-attack is not ready (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!AutoAttackIsReady())
        {
            CurrentCD = Mathf.Clamp(CurrentCD - Time.deltaTime, 0, CoolDownValue);
        }
    }

    /** LoadResource, protected virtual GameObject Method
	 * @param : string,
	 * @return : GameObject
	 * This method is used to load a GameObject prefab inside the champion folder.
	 **/
    protected virtual GameObject LoadResource(string prefabName)
    {
        return (GameObject)Resources.Load(champion.Name + "/" + prefabName);
    }

    /** AutoAttackIsReady protected bool Method,
	 * This returns if the auto-attack is launchea&ble or not. In this script, we only check if the auto-attack is under Cooldown or not.
	 **/
    protected virtual bool AutoAttackIsReady()
    {
        return (CurrentCD == 0);
    }

    /** AutoAttack public virtual void Method,
	 * This public Method should always be called by the Classe script. 
	 * When the Auto-attack is launched, the GCD is set to zero.
	 * Please note that, there is no text displayed when the auto-attack is under cooldown if the player tried to launche it.
	 * This is volunteer, because if the player hold the mouse button down, this will spamm the Debug log and next, the player screen with messages.
	 **/
    public virtual void AutoAttack()
    {
        CurrentCD = CoolDownValue;
    }

    /** GetDescriptionGUI, public string Method
	 * Called by the GuiAutoAttackDisplayer to get a formated string to display on the screen.
	 * The string contains a Dynamic description of the AutoAttack.
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    /** ApplyEffect, protected void method
     *  This method is usually called by a prefab attach to th character who autoAttack to apply Damages and Effects
     *  In the mother Abstract class the method is empty in the case of nothing is apply to a prefab
     **/
    public virtual void ApplyEffect(EntityLivingBase hit) { }

    #endregion
}