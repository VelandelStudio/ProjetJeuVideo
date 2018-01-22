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
public abstract class AutoAttackBase : MonoBehaviour, IDisplayable
{

    #region Fields
    public AutoAttackData AutoAttackDefinition { get; protected set; }
    public string Name { get; protected set; }
    public string Element { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public string[] Description { get; protected set; }

    private bool _isLoaded = false;
    public bool IsLoaded { get; protected set; }

    public float CurrentCD;
    protected Champion champion;
    #endregion

    #region Functionnal Methods

    /** Awake protected virtual void Method,
	 * The Awake method is used to create the AutoAttackBase from the JSON file and attribute every variables.
     * You should notice that the Status table contains Status GameObject with an instance of StatusBase attached to it.
     * We try to pre-warm the StatusBase attached in order to display descriptions and maybe modify the instance.
	 * If we do not find a Status prefab that correspond to the information in the AutoAttackData.json file or if we are not able to pre-warm the Status, 
	 * then, the Status is substitued by a DefaultStatus.
	 **/
    protected void Awake()
    {
        champion = GetComponentInParent<Champion>();

        LoadAutoAttackData("AutoAttackData.json");
        if (_isLoaded)
        {
            Name = AutoAttackDefinition.Name;
            Element = AutoAttackDefinition.Element;
            CoolDownValue = AutoAttackDefinition.CoolDownValue;
            Damages = AutoAttackDefinition.Damages;
            DamagesType = AutoAttackDefinition.DamagesType;
            OtherValues = AutoAttackDefinition.OtherValues;

            if (AutoAttackDefinition.Status.Length > 0 && AutoAttackDefinition.Status[0] != "")
            {
                Status = new GameObject[AutoAttackDefinition.Status.Length];
                for (int i = 0; i < AutoAttackDefinition.Status.Length; i++)
                {
                    Status[i] = LoadResource(AutoAttackDefinition.Status[i]);
                    if (Status[i] == null || !Status[i].GetComponent<StatusBase>().PreWarm())
                    {
                        Debug.Log(AutoAttackDefinition.Status[i] + " can not be loaded. "
                                 + "Please Ensure that the Status Name is correct in the SpellData.json file "
                                 + "or that this Status exists as a Prefab with the same Script Name associated to it. "
                                 + "DefaultStatus substitued");
                        Status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                        Status[i].GetComponent<StatusBase>().PreWarm();
                    }
                }
            }
            Description = AutoAttackDefinition.Description;
        }
    }

    /** Start protected virtual void Method,
	 * Before everything, we check if the AutoAttack was correctly loaded from the JSON file. If it is not the case, we notify the Champion class to replace the broken AutoAttack by a DefaultAutoAttack.
	 * Then, we initialize the CD of the spell.
	 **/
    protected virtual void Start()
    {
        if (!_isLoaded)
        {
            champion.ReplaceByDefaultDisplayable(this);
            Debug.Log("Error when loading the json data of : " + this.GetType().Name + ". Please, check your AutoAttackData.Json. DefaultAutoAttack was substitued.");
        }

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

    /** LoadAutoAttackData, protected void Method
	 * This Method is launched by the Awake one. Once launched, we try to locate a JSON File associated to this AutoAttack.
	 * If we find the AutoAttack in the file, then we build the AutoAttack from the elements indise the JSON and _isLoaded = true.
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
                    _isLoaded = true;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load Auto-attack data!");
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
        public string[] Status;
        public string[] Description;
    }
    #endregion
}