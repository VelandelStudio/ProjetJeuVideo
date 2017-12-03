using UnityEngine;
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
    /** Fields of Spell
     * The Spell class contains a lot of differents fields.
     * Here, you can find fin every component of the SpellData that can be found in the JSON file associated to the spell.
     * All public fields that are set from the SpellDefinition in the Awake method should be used by other scripts.
     * The spellInUse field is use to tell when a spell is starting and when it is ending. For example, a Charge spell has a real duration in time.
     * The CurrentCD field is used to know how much time ypu have to wait until the next use of the spell.
     * The SpellGCD field is used to set a GlobalCooldown to all Spells. Is a Spell is under GCD, the field IsUnderGCD is true.
     **/
    #region Fields
    private SpellData SpellDefinition;
    public string Name;
    public string Element;
    public float CoolDownValue;
    public bool HasGCD;
    public int[] Damages;
    public string[] DamagesType;
    public string[] OtherValues;
    public int NumberOfStacks;
    public GameObject[] Status;
    public string[] Description;

    protected bool spellInUse = false;
    protected Character champion;

    public float CurrentCD
    {
        get;
        protected set;
    }

    protected float spellGCD = 1f;
    public float SpellGCD
    {
        get
        {
            return spellGCD;
        }
        protected set { }
    }

    public bool IsUnderGCD
    {
        get;
        protected set;
    }
    #endregion

    #region Functionnal Methods

    /** Awake protected virtual void Method,
	 * The Awake method is used to create the Spell from the JSON file and attribute every variables.
	 **/
    protected virtual void Awake()
    {
        LoadSpellData("SpellData.json");
        Name = SpellDefinition.Name;
        Element = SpellDefinition.Element;
        CoolDownValue = SpellDefinition.CoolDownValue;
        HasGCD = SpellDefinition.HasGCD;
        Damages = SpellDefinition.Damages;
        DamagesType = SpellDefinition.DamagesType;
        OtherValues = SpellDefinition.OtherValues;
        NumberOfStacks = SpellDefinition.NumberOfStacks;
        Description = SpellDefinition.Description;

        if (SpellDefinition.Status.Length > 0 && SpellDefinition.Status[0] != "")
        {
            Status = new GameObject[SpellDefinition.Status.Length];
            champion = GetComponentInParent<Character>();
            for (int i = 0; i < SpellDefinition.Status.Length; i++)
            {
                Status[i] = (GameObject)Resources.Load(champion.GetType().ToString() + "/" + SpellDefinition.Status[i], typeof(GameObject));
                Status[i].GetComponent<StatusBase>().PreWarm();
            }
        }
    }

    /** Start protected virtual void Method,
	 * The Start method first display the name of the spell whe he is created by the Classe.
	 * Then, it initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        DisplaySpellCreation(this);
        CurrentCD = 0;
    }

    /** Update protected virtual void Method,
	 * If the spell is not Launcheable (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!IsSpellLauncheable())
        {
            CurrentCD = Mathf.Clamp(CurrentCD - Time.deltaTime, 0, CoolDownValue);
        }
    }

    /** LaunchSpell public virtual void Method,
	 * This public Method should always be called by the Classe script. It first checks if the spell is Launcheable.
	 * If it is, it switched the boolean spellInUse to true. This Method should always be used with OnSpellLaunched, to reset the spellInUse to false.
	 **/
    public virtual void LaunchSpell()
    {
        spellInUse = true;
    }

    /** OnSpellLaunched protected virtual void Method,
	 * This method should be called when the spell has reached is final statement.
	 * When it is the case, this method set the current CD to 0 and tells the game that this spells is not in use anymore.
	 **/
    protected virtual void OnSpellLaunched()
    {
        CurrentCD = CoolDownValue;
        spellInUse = false;
    }

    /** IsSpellLauncheable protected virtual bool Method,
	 * This returns if the spell is launcheable or not. In this script, we only check if the spell is under Cooldown or not.
	 * You should overrie this method is you want to not make your spell launcheable in other conditions
	 **/
    public virtual bool IsSpellLauncheable()
    {
        return (CurrentCD == 0);
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
    public void DisplaySpellNotLauncheable()
    {
        Debug.Log(this.GetType().ToString() + " is not available for the moment.");
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
        return StringHelper.SpellDescriptionBuilder(this);
    }

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
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }

    protected GameObject ApplyStatus(GameObject status, Transform tr)
    {
        GameObject objInst = Instantiate(status, tr);
        StatusBase statusInst = objInst.GetComponent<StatusBase>();
        statusInst.StartStatus(status.GetComponent<StatusBase>());
        return objInst;
    }
    #endregion

    #region Ienumerators and Coroutines
    /** LaunchGCD, public virtual IEnumerator Method
	 * This Method should be launched by other scripts in order to activate the Global Cooldown of the spell.
	 * If a spell is stackable and if it has more than 1 stacks, this method can be launched by the spell itself.
	 **/
    public virtual IEnumerator LaunchGCD()
    {
        CurrentCD = SpellGCD;
        IsUnderGCD = true;
        spellInUse = true;

        yield return new WaitForSeconds(SpellGCD);

        spellInUse = false;
        IsUnderGCD = false;
    }
    #endregion

    #region Serializable Classes
    /** SpellData public Serializable class
	 * This class war created to be at the service of the Spell class
	 * This class contains all elements to construct a spell from the JSON file.
	 **/
    [System.Serializable]
    public class SpellData
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public float CoolDownValue;
        public bool HasGCD;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public int NumberOfStacks;
        public string[] Status;
        public string[] Description;
    }
    #endregion
}