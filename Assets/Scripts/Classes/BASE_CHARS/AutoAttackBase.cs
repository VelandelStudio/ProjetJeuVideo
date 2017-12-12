using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** AutoAttackBase abstract class.
 * This abstract class is the mother class of all AutoAttack in our game. 
 * This class handles the behaviour the CD of all AutoAttacks. It also contains the AutoAttack method launched by the Classe.
 **/
public abstract class AutoAttackBase : MonoBehaviour
{

    #region Fields
    private AutoAttackData _autoAttackDefinition;
    public string Name;
    public string Element;
    public float CoolDownValue;
    public int[] Damages;
    public string[] DamagesType;
    public string[] OtherValues;
    public string[] Description;

    public float CurrentCD;
    #endregion

    #region Functionnal Methods

    /** Awake, Protected void Method
	 * This method is used to launch the Loading of Data from a JSON File.
	 * If the loading is a success, we set all the public fields with the elements we have found in the JSON. 
	 * These fields must be used by other scripts.
	 **/
    protected void Awake()
    {
        LoadAutoAttackData("AutoAttackData.json");
        Debug.Log(_autoAttackDefinition.Name);
        Name = _autoAttackDefinition.Name;
        Element = _autoAttackDefinition.Element;
        CoolDownValue = _autoAttackDefinition.CoolDownValue;
        Damages = _autoAttackDefinition.Damages;
        DamagesType = _autoAttackDefinition.DamagesType;
        OtherValues = _autoAttackDefinition.OtherValues;
        Description = _autoAttackDefinition.Description;
    }

    /** Start protected virtual void Method,
	 * The Start method initializes the CD of the auto-attack.
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

    /** AutoAttackIsReady protected bool Method,
	 * This returns if the auto-attack is launchea&ble or not. In this script, we only check if the auto-attack is under Cooldown or not.
	 **/
    protected bool AutoAttackIsReady()
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
        return StringHelper.AutoAttackDescriptionBuilder(this);
    }

    /** LoadAutoAttackData, protected void Method
	 * This Method is launched by the Awake one. Once launched, we try to locate a JSON File associated to this AutoAttack.
	 * If we find the AutoAttack in the file, then we build the AutoAttack from the elements indise the JSON.
	 **/
    protected void LoadAutoAttackData(string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            AutoAttackData[] data = JsonHelper.getJsonArray<AutoAttackData>(jsonFile);
            foreach (AutoAttackData autoAttack in data)
            {
                if (autoAttack.ScriptName == this.GetType().ToString())
                {
                    _autoAttackDefinition = autoAttack;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /** ApplyEffect, protected void method
     *  This method is usually called by a prefab attach to th character who autoAttack to apply Damages and Effects
     *  In the mother Abstract class the method is empty in the case of nothing is apply to a prefab
     **/
    public virtual void ApplyEffect(EntityLivingBase hit)
    {

    }

    #endregion

    #region Serializable Classes
    /** AutoAttackData, public Serializable class
	 * This Serializable Class is used to get all elements we need to construct an AutoAttack from a Json File.
	 **/
    [System.Serializable]
    public class AutoAttackData
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public float CoolDownValue;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public string[] Description;
    }
    #endregion
}