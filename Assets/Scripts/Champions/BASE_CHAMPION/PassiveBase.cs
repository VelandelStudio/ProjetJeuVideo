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
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public string[] Description { get; protected set; }

    private Boolean _isLoaded = false;

    protected Champion champion;

    public int NumberOfStacks;
    #endregion

    #region Functional methods
    /** Awake protected void Method
	 * The Awake ensure a good construction of the passive from the JSON by calling the LoadSpellData.
	 * After that, we transmit all values to public fields that will be used by the other scripts.
	 **/
    protected void Awake()
    {
        LoadSpellData("PassiveData.json");
        NumberOfStacks = PassiveDefinition.NumberOfStacks;
        Description = PassiveDefinition.Description;
        Name = PassiveDefinition.Name;
        Damages = PassiveDefinition.Damages;
        DamagesType = PassiveDefinition.DamagesType;
        OtherValues = PassiveDefinition.OtherValues;
        NumberOfStacks = PassiveDefinition.NumberOfStacks;
        Description = PassiveDefinition.Description;

        champion = GetComponentInParent<Champion>();
        if (PassiveDefinition.Status.Length > 0 && PassiveDefinition.Status[0] != "")
        {
            Status = new GameObject[PassiveDefinition.Status.Length];
            for (int i = 0; i < PassiveDefinition.Status.Length; i++)
            {
                Status[i] = LoadResource(PassiveDefinition.Status[i]);
                Status[i].GetComponent<StatusBase>().PreWarm();
            }
        }
    }

    /** LoadSpellData, protected void
	 * @Params : string
	 * Loads the JSON PassiveData associated to the Passive.
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

            if (!_isLoaded)
            {
                LoadDeafaultPassiv(json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }

    protected void LoadDeafaultPassiv(string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            PassiveData[] data = JsonHelper.getJsonArray<PassiveData>(jsonFile);
            foreach (PassiveData passive in data)
            {
                if (passive.ScriptName == "DefaultPassive")
                {
                    _passiveDefinition = passive;
                    break;
                }
            }
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
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public int NumberOfStacks;
        public string[] Status;
        public string[] Description;
    }
    #endregion
}