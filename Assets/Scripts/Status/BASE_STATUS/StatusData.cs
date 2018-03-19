using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StatusData : Datas, IStatusDisplayable
{

    public StatusData StatusDefinition { get; protected set; }
    public string Name { get; protected set; }
    public string Element { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public string[] Description { get; protected set; }
    public float Duration { get; protected set; }
    public bool IsTickable { get; protected set; }
    public float[] TicksIntervals { get; protected set; }
    public float[] TickStarts { get; protected set; }
    public bool IsStackable { get; protected set; }
    public int NumberOfStacks { get; protected set; }

    private DataStatusLoader _dataStatusLoader;

    /** Wrapper for IStatusDisplayable **/
    public string Type { get; }
    public float CoolDownValue { get; }
    public GameObject[] Status { get; }
    /**          End Wrapper           **/

    public StatusData(string statusName) : base(statusName)
    {
        LoadStatusData(statusName, "SpellData.json");
        ScriptName = _dataStatusLoader.ScriptName;
        Name = _dataStatusLoader.Name;
        Element = _dataStatusLoader.Element;
        Damages = _dataStatusLoader.Damages;
        DamagesType = _dataStatusLoader.DamagesType;
        OtherValues = _dataStatusLoader.OtherValues;
        NumberOfStacks = _dataStatusLoader.NumberOfStacks;
        Description = _dataStatusLoader.Description;
        Duration = _dataStatusLoader.Duration;
        IsTickable = _dataStatusLoader.IsTickable;
        TicksIntervals = _dataStatusLoader.TicksIntervals;
        TickStarts = _dataStatusLoader.TickStarts;
        IsStackable = _dataStatusLoader.IsStackable;
        NumberOfStacks = _dataStatusLoader.NumberOfStacks;
    }

    /** LoadStatusData, protected void
	* @Params : string
	* Loads the JSON StatusDefinition associated to the spell.
	* If the loading is a success, then _isLoaded = true.
	**/
    protected void LoadStatusData(string statusName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);

            DataStatusLoader[] data = JsonHelper.getJsonArray<DataStatusLoader>(jsonFile);
            foreach (DataStatusLoader status in data)
            {
                if (status.ScriptName == statusName)
                {
                    _dataStatusLoader = status;
                    _isLoaded = true;
                    break;
                }
            }

            if (!_isLoaded)
            {
                string defaultElement = "DefaultStatus";
                DisplayErroDefault(statusName, json, defaultElement);
                LoadStatusData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }
}