using UnityEngine;
using System.IO;

public class PassiveData : Datas, ISpellDisplayable
{
    public string Element { get; protected set; }
    public string Type { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public int NumberOfStacks { get; protected set; }

    private DataPassiveLoader _dataPassiveLoader;

    public PassiveData(string passiveName) : base(passiveName)
    {
        LoadPassiveData(passiveName, "PassiveData.json");
        ScriptName = _dataPassiveLoader.ScriptName;
        Name = _dataPassiveLoader.Name;
        Element = _dataPassiveLoader.Element;
        Type = _dataPassiveLoader.Type;
        Damages = _dataPassiveLoader.Damages;
        DamagesType = _dataPassiveLoader.DamagesType;
        OtherValues = _dataPassiveLoader.OtherValues;
        NumberOfStacks = _dataPassiveLoader.NumberOfStacks;
        Description = _dataPassiveLoader.Description;

        if (_dataPassiveLoader.Status.Length > 0 && _dataPassiveLoader.Status[0] != "")
        {
            Status = AttributeStatus(_dataPassiveLoader.Status);
        }
    }

    /** LoadSpellData, protected void
      * @Params : string
      * Loads the JSON _dataSpellLoader associated to the spell.
      * If the loading is a success, then _isLoaded = true.
      **/
    protected void LoadPassiveData(string passiveName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            DataPassiveLoader[] data = JsonHelper.getJsonArray<DataPassiveLoader>(jsonFile);
            foreach (DataPassiveLoader passive in data)
            {
                if (passive.ScriptName == passiveName)
                {
                    _dataPassiveLoader = passive;
                    _isLoaded = true;
                    break;
                }
            }

            if (!_isLoaded)
            {
                string defaultElement = "DefaultPassive";
                DisplayErroDefault(passiveName, json, defaultElement);
                LoadPassiveData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }
}
