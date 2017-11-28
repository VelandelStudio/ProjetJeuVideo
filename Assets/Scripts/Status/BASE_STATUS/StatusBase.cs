using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/** StatusBase public abstract class
 * Implements IStatus.
 * This class is the mother class of most of the buffs/debuffs/Status in our game.
 * It aims to simplify the creation of Status, using the Template pattern.
 **/
public abstract class StatusBase : MonoBehaviour, IStatus
{
    private StatusData StatusDefinition;
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

    private bool ExternalLoading = false;
    /** Awake, protected virtual void
     *  By default, a Status should be initialized at the LocalPosition of 0,0,0.
     **/
    protected virtual void Awake()
    {
        Debug.Log("ExternalLoading");
        if (ExternalLoading)
        {
            return;
        }

        transform.localPosition = Vector3.zero;
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
        ExternalLoading = true;
    }

    /** Start protected virtual void
     * The method is here to launch the method OnStatusApplied.
     * Then, it lanches an InvokeRepeating on StatusTickBehaviour to make the status tick every tickInterval.
	 * Please note that, by default, a Status is considered as tickable.
     * Finally, it lanches an Invoke on DestroyStatus that will occurs in maxDuration seconds.
     **/
    protected virtual void Start()
    {
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
        Debug.Log("Reset Status");

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
        Destroy(gameObject);
    }

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

    public void PreloadStatus()
    {
        Awake();
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