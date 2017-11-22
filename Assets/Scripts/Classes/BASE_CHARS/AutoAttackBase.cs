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
    protected float GCD;
    protected float currentGCD;

    public AutoAttackData AutoAttackDefinition
    {
        get;
        protected set;
    }
    /** Start protected virtual void Method,
	 * The Start method initializes the CD of the auto-attack.
	 **/
    protected virtual void Start()
    {
        LoadAutoAttackData("AutoAttackData.json");
        currentGCD = GCD;
    }

    /** Update protected virtual void Method,
	 * If the auto-attack is not ready (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!AutoAttackIsReady())
        {
            currentGCD = Mathf.Clamp(currentGCD + Time.deltaTime, 0, GCD);
        }
    }

    /** AutoAttackIsReady protected bool Method,
	 * This returns if the auto-attack is launchea&ble or not. In this script, we only check if the auto-attack is under Cooldown or not.
	 **/
    protected bool AutoAttackIsReady()
    {
        return (currentGCD == GCD);
    }

    /** AutoAttack public virtual void Method,
	 * This public Method should always be called by the Classe script. 
	 * When the Auto-attack is launched, the GCD is set to zero.
	 * Please note that, there is no text displayed when the auto-attack is under cooldown if the player tried to launche it.
	 * This is volunteer, because if the player hold the mouse button down, this will spamm the Debug log and next, the player screen with messages.
	 **/
    public virtual void AutoAttack()
    {
        currentGCD = 0;
    }

    /**
	 *
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.AutoAttackDescriptionBuilder(this, getDescriptionVariables());
    }

    /** getDescriptionVariables, protected abstract object[]
	 * Return an array of objects that represents the current variables displayed on the GUI
	**/
    protected abstract object[] getDescriptionVariables();

    /** 
	 *
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
                    AutoAttackDefinition = autoAttack;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /** 
	 * 
	**/
    [System.Serializable]
    public class AutoAttackData
    {
        public string ScriptName;
        public string Name;
        public int[] Damages;
        public string[] OtherValues;
        public string[] Description;
    }
}