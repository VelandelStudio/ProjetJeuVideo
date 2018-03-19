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
    public StatusData _statusData { get; protected set; }

    public string Name { get { return _statusData.Name; } protected set { } }
    public string Element { get { return _statusData.Element; } protected set { } }
    public int[] Damages
    {
        get
        {
            if (!launcherCharacs)
            {
                return _statusData.Damages;
            }

            int[] DamagesCalc = new int[_statusData.Damages.Length];
            for (int i = 0; i < DamagesCalc.Length; i++)
            {
                DamagesCalc[i] = (int)(_statusData.Damages[i] * launcherCharacs.DamageFactor);
            }
            return DamagesCalc;
        }
        protected set { }
    }

    public string[] DamagesType { get { return _statusData.DamagesType; } protected set { } }
    public string[] OtherValues { get { return _statusData.OtherValues; } protected set { } }
    public string[] Description { get { return _statusData.Description; } protected set { } }
    public float Duration { get { return _statusData.Duration; } protected set { } }
    public bool IsTickable { get { return _statusData.IsTickable; } protected set { } }
    public float[] TicksIntervals { get { return _statusData.TicksIntervals; } protected set { } }
    public float[] TickStarts { get { return _statusData.TickStarts; } protected set { } }
    public bool IsStackable { get { return _statusData.IsStackable; } protected set { } }
    public int NumberOfStacks { get { return _statusData.NumberOfStacks; } protected set { } }
    public bool IsLoaded { get { return _statusData.IsLoaded; } protected set { } }

    public float CurrentTimer
    {
        get { return statusDisplayer.CurrentTimerOnScreen; }
        protected set { }
    }

    GameObject statusSection;
    GUIStatusDisplayer statusDisplayer;

    public float CoolDownValue { get { return _statusData.CoolDownValue; } protected set { } }
    public GameObject[] Status { get { return _statusData.Status; } protected set { } }
    public string Type { get { return _statusData.Type; } protected set { } }

    protected Characteristics launcherCharacsInstance;
    protected Characteristics launcherCharacs;
    protected Characteristics receiverCharacs;

    #region Functionnal Methods
    /** Start, protected virtual void
     * Just here to set the local position of the Status to vector3.zero.
     * The main goal of this method is to ensure that every calculation of positions will be based on the zero value. 
     **/
    protected virtual void Start()
    {
        transform.localPosition = Vector3.zero;
        receiverCharacs.GetComponentInParent<Characteristics>();
        StartStatus();
    }

    /** PreWarm, public virtual bool 
     * This method is called by other scripts (mainly spells)
     * This method is used to load an instance of a Status from a JSON File.
	 * We return true is the Status is correctly loaded.
     **/
    public virtual bool PreWarm()
    {
        _statusData = new StatusData(this.GetType().ToString());
        return _statusData.IsLoaded;
    }

    public void AttributeCharacteristics(Characteristics characteristics)
    {
        launcherCharacsInstance = characteristics;
        receiverCharacs = new Characteristics();
        receiverCharacs = launcherCharacsInstance;
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
    public virtual void StartStatus()
    {
        if (transform.parent.gameObject.tag == "Player")
        {
            AttributeStatusToPlayerGUI();
        }

        OnStatusApplied();

        if (IsTickable)
        {
            InvokeRepeating("StatusTickBehaviour", TickStarts[0], TicksIntervals[0]);
        }

        if (Duration != Mathf.Infinity)
        {
            Invoke("DestroyStatus", Duration);
        }
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

        if (Duration != Mathf.Infinity)
        {
            CancelInvoke("DestroyStatus");
            Invoke("DestroyStatus", Duration);
        }
    }

    public virtual void RefreshStatus()
    {
        ResetStatus();
        receiverCharacs = new Characteristics();
        receiverCharacs = launcherCharacsInstance;
        OnStatusApplied();
    }

    /** DestroyStatus public virtual void
     * Instantly Destroy the gameObject that contains the Status
     **/
    public virtual void DestroyStatus()
    {
        if (transform.parent.gameObject.tag == "Player")
        {
            statusDisplayer.DestroyGUIStatus();
        }

        Destroy(gameObject);
    }

    private void AttributeStatusToPlayerGUI()
    {
        statusSection = GameObject.Find("StatusSection");
        GameObject statusGUIInst = (GameObject)Resources.Load("GUI/StatusGUI", typeof(GameObject));
        statusGUIInst = Instantiate(statusGUIInst, statusSection.transform);
        statusDisplayer = statusGUIInst.GetComponent<GUIStatusDisplayer>();
        statusDisplayer.AttributeDisplayable(this);
    }

    /** GetDescriptionGUI, public string method
	 * return a formated string of the description of the Statusthat will be displayed on the screen.
	 **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    #endregion
}