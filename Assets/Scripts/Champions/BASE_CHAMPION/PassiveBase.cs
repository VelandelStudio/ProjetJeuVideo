using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** PassiveBase, public abstract class
 * This class should be the mother class of all Passives of our game.
 * The main goal of this script is to create a passive from its JSON file.
 **/
public abstract class PassiveBase : MonoBehaviour, IDisplayable
{

    /** Fields of PassiveBase
     * The Passive base is constructed with a PassiveData that comes from the JSON File
     * Other fields are public field initialized in the Awake Method with the values of the JSON.
     * We should always work with these public fields and never with the raw data of the JSON.
     **/
    #region Fields  	
    public PassiveData PassiveDefinition { get; protected set; }
    public string Name { get; protected set; }
    public string Element { get; protected set; }
    public string Type { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public string[] Description { get; protected set; }

    private bool _isLoaded = false;
    public bool IsLoaded { get; protected set; }

    protected Champion champion;

    public int NumberOfStacks;
    #endregion

    #region Functional methods
    /** Awake protected virtual void Method,
	 * The Awake method is used to create the PassiveBase from the JSON file and attribute every variables.
     * You should notice that the Status table contains Status GameObject with an instance of StatusBase attached to it.
     * We try to pre-warm the StatusBase attached in order to display descriptions and maybe modify the instance.
	 * If we do not find a Status prefab that correspond to the information in the PassiveData.json file or if we are not able to pre-warm the Status, 
	 * then, the Status is substitued by a DefaultStatus.
	 **/
    protected void Awake()
    {
        champion = GetComponentInParent<Champion>();

        LoadSpellData("PassiveData.json");
        if (_isLoaded)
        {
            NumberOfStacks = PassiveDefinition.NumberOfStacks;
            Description = PassiveDefinition.Description;
            Name = PassiveDefinition.Name;
            Type = PassiveDefinition.Type;
            Damages = PassiveDefinition.Damages;
            DamagesType = PassiveDefinition.DamagesType;
            OtherValues = PassiveDefinition.OtherValues;
            NumberOfStacks = PassiveDefinition.NumberOfStacks;
            Description = PassiveDefinition.Description;

            if (PassiveDefinition.Status.Length > 0 && PassiveDefinition.Status[0] != "")
            {
                Status = new GameObject[PassiveDefinition.Status.Length];
                for (int i = 0; i < PassiveDefinition.Status.Length; i++)
                {
                    Status[i] = LoadResource(PassiveDefinition.Status[i]);
                    if (Status[i] == null || !Status[i].GetComponent<StatusBase>().PreWarm())
                    {
                        Debug.Log(PassiveDefinition.Status[i] + " can not be loaded. "
                                 + "Please Ensure that the Status Name is correct in the SpellData.json file "
                                 + "or that this Status exists as a Prefab with the same Script Name associated to it. "
                                 + "DefaultStatus substitued");
                        Status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                        Status[i].GetComponent<StatusBase>().PreWarm();
                    }
                }
            }
        }
    }

    /** Start protected virtual void Method,
	 * Before everything, we check if the Passive was correctly loaded from the JSON file. If it is not the case, we notify the Champion class to replace the broken Passive by a DefaultPassive.
	 **/
    protected virtual void Start()
    {
        if (!_isLoaded)
        {
            champion.ReplaceByDefaultDisplayable(this);
            Debug.Log("Error when loading the json data of : " + this.GetType().Name + ". Please, check your PassiveData.Json. DefaultPassive was substitued.");
        }
    }

    /** LoadSpellData, protected void
	 * @Params : string
	 * Loads the JSON PassiveData associated to the Passive.
	 * If the loading is a success, then _isLoaded = true.
	 **/
    protected void LoadSpellData(string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            PassiveData[] data = JsonHelper.getJsonArray<PassiveData>(jsonFile);
            foreach (PassiveData passive in data)
            {
                if (passive.ScriptName == this.GetType().ToString())
                {
                    PassiveDefinition = passive;
                    _isLoaded = true;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
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

    /**GetDescriptionGUI, public string Method
	 * Return the description of our passive built by the PassiveDescriptionBuilder of the StringHelper static class.
	 * This method allows to get a dynamic and colored description on the screen.
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    /** ApplyStatus, protected virtual GameObject
     * @Params : GameObject, Transform
     * @Returns: GameObject
     * This method should be called by Passive that are able to apply a Status on their targets.
     * The first param (GameObject status) should be a GameObject that has a StatusBase Script attached. 
     * Most of the time, this gameObject is contained in the SpellDefinition.Status Table, and the Transform is the target one.
     * The method will instantiate a new Status and attach to it the status that is already attached on the first parameter.
     * When we instantiate an object, the StatusBase element if reseted, so we need to attach this instance of the StatusBase because of previous modifications,
     * such as damages or CDReduction of the Status of the player. 
     * Then, we return the fresh GameObject constructed if we want to use it later.
     **/
    protected virtual GameObject ApplyStatus(GameObject status, Transform tr)
    {
        GameObject objInst = Instantiate(status, tr);
        StatusBase statusInst = objInst.GetComponent<StatusBase>();
        statusInst.StartStatus(status.GetComponent<StatusBase>());
        return objInst;
    }
    #endregion

    #region Serializable Classes
    /** PassiveData public Serializable class
	 * This class was created to be at the service of the PassiveBase class
	 * This class contains all elements to construct a Passive from the JSON file.
	 **/
    [System.Serializable]
    public class PassiveData
    {
        public string ScriptName;
        public string Name;
        public string Type;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public int NumberOfStacks;
        public string[] Status;
        public string[] Description;
    }
    #endregion
}