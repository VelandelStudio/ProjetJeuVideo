using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/** StatusBase public abstract class
 * Implements IStatus, IStatusDisplayable
 * This class is the mother class of most of the buffs/debuffs/Status in our game.
 * It aims to simplify the creation of Status, using the Template pattern.
 **/
public abstract class StatusBase : MonoBehaviour, IStatus, IStatusDisplayable
{
    public StatusData StatusDefinition { get; protected set; }
    public string Name { get; protected set; }
    public string Element { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public string[] Description { get; protected set; }
    public float Duration { get; protected set; }
    public bool IsTickable { get; protected set; }
    public float[] TicksIntervals { get; protected set; }
    public float[] TickStarts { get; protected set; }

    public bool IsStackable;
    public int NumberOfStacks;

    public float CurrentTimer
    {
        get { return statusDisplayer.CurrentTimerOnScreen; }
        protected set { }
    }

    GameObject statusSection;
    GUIStatusDisplayer statusDisplayer;

    #region Functionnal Methods
    /** Start, protected virtual void
     * Just here to set the local position of the Status to vector3.zero.
     * The main goal of this method is to ensure that every calculation of positions will be based on the zero value. 
     **/
    protected virtual void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    /** PreWarm, public virtual void 
     * This method is called by other scripts (mainly spells)
     * This method is used to load an instance of a Status from a JSON File.
     **/
    public virtual void PreWarm()
    {
        LoadStatusData("StatusData.json");
        Name = StatusDefinition.Name;
        Element = StatusDefinition.Element;
        Duration = StatusDefinition.Duration;
        IsTickable = StatusDefinition.IsTickable;
        TicksIntervals = StatusDefinition.TicksIntervals;
        TickStarts = StatusDefinition.TickStarts;
        Damages = StatusDefinition.Damages;
        DamagesType = StatusDefinition.DamagesType;
        OtherValues = StatusDefinition.OtherValues;
        IsStackable = StatusDefinition.IsStackable;
        NumberOfStacks = StatusDefinition.NumberOfStacks;
        Description = StatusDefinition.Description;
    }

    /** StartStatus, public virtual void 
     * @params : StatusBase
     * This method is called by other scripts (mainly spells)
     * It is used to Start the Status already applied on a target.
     * The Status should be started with another Status instance (because of instanciation of gameobjects on unity)
     * If the param is null, the Status is reloaded (Prewarm) from it's JSON file.
	 * If we detect that the same Status is attached to the GameObject Parent, then we add the Destroy the old Status and add the new one.
	 * Note that the Old Status will be removed only if it was at least one second on the gameObject
     **/
    public virtual void StartStatus(StatusBase status)
    {
        if (status == null)
        {
            PreWarm();
        }

        StatusBase[] statusOnTarget = transform.parent.GetComponentsInChildren<StatusBase>(true);
        for (int i = 0; i < statusOnTarget.Length; i++)
        {
            Debug.Log(statusOnTarget[i]);
            if (statusOnTarget[i].Name == status.Name)
            {
                if (transform.GetComponentInParent<IProjectile>() != null)
                {
                    Destroy(gameObject);
                    return;
                }

                if (transform.parent.gameObject.tag == "Player" && statusOnTarget[i].CurrentTimer > status.Duration - 1f)
                { 
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    statusOnTarget[i].DestroyStatus();
                }
            }
        }

        Name = status.Name;
        Element = status.Element;
        Duration = status.Duration;
        IsTickable = status.IsTickable;
        TicksIntervals = status.TicksIntervals;
        TickStarts = status.TickStarts;
        Damages = status.Damages;
        DamagesType = status.DamagesType;
        OtherValues = status.OtherValues;
        IsStackable = status.IsStackable;
        NumberOfStacks = status.NumberOfStacks;
        Description = status.Description;

        if (transform.parent.gameObject.tag == "Player" && transform.GetComponentInParent<IProjectile>() == null)
        {
            Debug.Log("hello");
            statusSection = GameObject.Find("StatusSection");
            GameObject statusGUIInst = (GameObject)Resources.Load("GUI/StatusGUI", typeof(GameObject));
            statusGUIInst = Instantiate(statusGUIInst, statusSection.transform);
            statusDisplayer = statusGUIInst.GetComponent<GUIStatusDisplayer>();
            statusDisplayer.AttributeDisplayable(this);
        }

        OnStatusApplied();

        if (IsTickable)
        {
            InvokeRepeating("StatusTickBehaviour", TickStarts[0], TicksIntervals[0]);
        }

        Invoke("DestroyStatus", Duration);
    }

    /** OnStatusApplied public abstract void
     * Note that this method is abstract and should always be implemented in the extenders.
     * The objective is to satisfy the Template pattern. In that way, the Start method will automatically call the OnStatusApplied in the inherited script.
     **/
    public abstract void OnStatusApplied();

    /** StatusTickBehaviour public abstract void
     * Note that this method is abstract and should always be implemented in the extenders.
     * The objective is to satisfy the Template pattern. In that way, the Start method will automatically call the StatusTickBehaviour in the inherited script.
     **/
    public abstract void StatusTickBehaviour();

    /** ResetStatus public virtual void
     * This method should be used when we want to Reset the debuff. In this way, we do not need to Destroy an instance of the debuff and then create a new one.
     * In order to do that, we call the OnStatusApplied Method to apply the fresh new inputs from inherited members.
     * Then we cancel and re-Invoke StatusTickBehaviour and DestroyStatus Methods
     **/
    public virtual void ResetStatus()
    {
        if (transform.parent.gameObject.tag == "Player")
        {
            statusDisplayer.ResetGUIStatus();
        }

        OnStatusApplied();
        CancelInvoke("DestroyStatus");
        Invoke("DestroyStatus", Duration);

        if (IsTickable)
        {
            CancelInvoke("StatusTickBehaviour");
            InvokeRepeating("StatusTickBehaviour", TickStarts[0], TicksIntervals[0]);
        }
    }

    /** DestroyStatus public virtual void
     * Instantly Destroy the gameObject that contains the Status
     **/
    public virtual void DestroyStatus()
    {
        if (transform.parent.gameObject.tag == "Player" && transform.GetComponentInParent<IProjectile>() == null)
        {
            statusDisplayer.DestroyGUIStatus();
        }

        Destroy(gameObject);
    }

    /** GetDescriptionGUI, public string method
	 * return a formated string of the description of the Statusthat will be displayed on the screen.
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    #endregion

    /** LoadStatusData, protected void
	 * @Params : string
	 * Loads the JSON StatusDefinition associated to the spell.
	 **/
    protected void LoadStatusData(string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);

            StatusData[] data = JsonHelper.getJsonArray<StatusData>(jsonFile);
            foreach (StatusData status in data)
            {
                if (status.ScriptName == this.GetType().ToString())
                {
                    StatusDefinition = status;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }

    #region Serializable Classes
    /** StatusData public Serializable class
	 * This class was created to be at the service of the StatusBase class
	 * This class contains all elements to construct a Status from the JSON file.
	 **/
    [System.Serializable]
    public class StatusData
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public float Duration;
        public bool IsTickable;
        public float[] TicksIntervals;
        public float[] TickStarts;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public bool IsStackable;
        public int NumberOfStacks;
        public string[] Description;
    }
    #endregion
}