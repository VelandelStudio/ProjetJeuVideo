using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** PassiveBase, public abstract class
 * This class should be the mother class of all Passives of our game.
 * The main goal of this script is to create a passive from its JSON file.
 **/
public abstract class PassiveBase : MonoBehaviour, ISpellDisplayable
{
    /** Fields of PassiveBase
     * The Passive base is constructed with a PassiveData that comes from the JSON File
     * Other fields are public field initialized in the Awake Method with the values of the JSON.
     * We should always work with these public fields and never with the raw data of the JSON.
     **/
    #region Fields  	
    public PassiveData _passiveData { get; protected set; }
    public string Name { get { return _passiveData.Name; } protected set { } }
    public string Element { get { return _passiveData.Element; } protected set { } }
    public string Type { get { return _passiveData.Type; } protected set { } }
    public float CoolDownValue { get { return _passiveData.CoolDownValue; } protected set { } }
    public int[] Damages { get { return _passiveData.Damages; } protected set { } }
    public string[] DamagesType { get { return _passiveData.DamagesType; } protected set { } }
    public string[] OtherValues { get { return _passiveData.OtherValues; } protected set { } }
    public GameObject[] Status { get { return _passiveData.Status; } protected set { } }
    public string[] Description { get { return _passiveData.Description; } protected set { } }
    public int NumberOfStacks { get { return _passiveData.NumberOfStacks; } protected set { } }
    public bool IsLoaded { get { return _passiveData.IsLoaded; } protected set { } }

    protected Champion champion;

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
        _passiveData = new PassiveData(GetType().ToString());
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
}