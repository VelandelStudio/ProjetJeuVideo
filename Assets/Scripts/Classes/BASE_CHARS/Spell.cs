using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** Spell abstract class.
 * This abstract class is the mother class of all spells in our game. 
 * This class handles the behaviour the CD of all spells. It also contains the LaunchSpell method launched by the Classe.
 **/
public abstract class Spell : MonoBehaviour
{
    public float spellCD
    {
        get;
        protected set;
    }

    public float currentCD
    {
        get;
        protected set;
    }

    public SpellData SpellDefinition
    {
        get;
        protected set;
    }

    protected bool spellInUse = false;

    /** Start protected virtual void Method,
	 * The Start method first display the name of the spell whe he is created by the Classe.
	 * Then, it initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        LoadSpellData("SpellData.json");
        DisplaySpellCreation(this);
        currentCD = 0;
        spellCD = SpellDefinition.CoolDownValue;
    }

    /** Update protected virtual void Method,
	 * If the spell is not Launcheable (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!IsSpellLauncheable())
        {
            currentCD = Mathf.Clamp(currentCD - Time.deltaTime, 0, spellCD);
        }
    }

    /** LaunchSpell public virtual void Method,
	 * This public Method should always be called by the Classe script. It first checks if the spell is Launcheable.
	 * If it is, it switched the boolean spellInUse to true. This Method should always be used with OnSpellLaunched, to reset the spellInUse to false.
	 **/
    public virtual void LaunchSpell()
    {
        if (!IsSpellLauncheable())
        {
            DisplaySpellNotLauncheable(this);
        }
        else
        {
            spellInUse = true;
        }
    }

    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When it is the case, this method set the current CD to 0 and tells the game that this spells is not in use anymore.
	 **/
    protected virtual void OnSpellLaunched()
    {
        currentCD = spellCD;
        spellInUse = false;
    }

    /** IsSpellLauncheable protected virtual bool Method,
	 * This returns if the spell is launcheable or not. In this script, we only check if the spell is under Cooldown or not.
	 * You should overrie this method is you want to not make your spell launcheable in other conditions
	 **/
    protected virtual bool IsSpellLauncheable()
    {
        return (currentCD == 0);
    }

    /** IsSpellInUse public bool Method,
	 * This returns if the spell is currently in use.
	 **/
    public bool IsSpellInUse()
    {
        return spellInUse;
    }

    /** DisplaySpellNotLauncheable protected void Method,
	 * This displays in the logs a message when the spell is not Launchable whereas the player is trying to launche it.
	 * In the future, this method will return a string on the screen of the player.
	 **/
    protected void DisplaySpellNotLauncheable(Spell spell)
    {
        Debug.Log(spell.GetType().ToString() + " is not available for the moment.");
    }

    /** DisplaySpellCreation protected void Method,
	 * This displays in the logs a message when the spell is created at the start of a Classe.
	 * This method is a tool for in-dev feature so far.
	 **/
    protected void DisplaySpellCreation(Spell spell)
    {
        Debug.Log(spell.GetType().ToString() + " created.");
    }

    /** AvailableForGUI public virtual bool Method,
	 * Each spell is associated to a GUI Spell Slot. 
	 * Globally this Slot has an image that represents the remaining CD timer of the Spell. 
	 * If the cooldown is reload, the image is clear. But, sometimes, we have spells that can not be launched in some conditions.
	 * This is what this method does. By Default, every spell is AvailableForGUI, meaning that the image associated is only CD dependant.
	 **/
    public virtual bool AvailableForGUI()
    {
        return true;
    }

    /**GetDescriptionGUI, public string Method
	 * Return the descreiption of our spell built by the SpellDescriptionBuilder of the StringHelper static class.
	 * This method allows to get a dynamic and colored description on the screen.
	**/
    public string GetDescriptionGUI()
    {
        return StringHelper.SpellDescriptionBuilder(this, getDescriptionVariables());
    }

    /** getDescriptionVariables, protected abstract object[]
	 * Return an array of objects that represents the current variables displayed on the GUI
	**/
    protected abstract object[] getDescriptionVariables();

    /** LoadSpellData, protected void
	 * @Params : string
	 * Loads the JSON SpellDefinition associated to the spell.
	**/
    protected void LoadSpellData(string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            SpellData[] data = JsonHelper.getJsonArray<SpellData>(jsonFile);
            foreach (SpellData spell in data)
            {
                if (spell.ScriptName == this.GetType().ToString())
                {
                    SpellDefinition = spell;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /** SpellData public Serializable class
	 * This class war created to be at the service of the Spell class
	 * This class contains all elements to construct a spell from the JSON file.
	**/
    [System.Serializable]
    public class SpellData
    {
        public string ScriptName;
        public string Name;
        public string Type;
        public float CoolDownValue;
        public bool HasGCD;
        public int BaseDamage;
        public int[] AdditionalDamages;
        public IStatus[] AdditionalEffects;
        public string[] OtherValues;
        public bool IsStackable;
        public int NumberOfStack;
        public string[] Description;
    }
}