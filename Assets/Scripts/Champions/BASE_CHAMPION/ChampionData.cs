using UnityEngine;
using System.IO;

public class ChampionData : Datas {

    public string Passive { get; protected set; }
    public string AutoAttack { get; protected set; }
    public string[] ActiveSpells { get; protected set; }
    public string Element { get; protected set; }
    public string[] Tags { get; protected set; }

    private DataChampionLoader _dataChampionLoader;

    public ChampionData(string championName) : base(championName)
    {
        LoadChampionData(championName, "ChampionData.json");
        Name = _dataChampionLoader.Name;
        Passive = _dataChampionLoader.Passive;
        AutoAttack = _dataChampionLoader.AutoAttack;
        ActiveSpells = _dataChampionLoader.ActiveSpells;
        Element = _dataChampionLoader.Element;
        Description = _dataChampionLoader.Description;
        Tags = _dataChampionLoader.Tags;
    }

    /** LoadAutoAttackData, protected void Method
    * This Method is launched by the Awake one. Once launched, we try to locate a JSON File associated to this AutoAttack.
    * If we find the AutoAttack in the file, then we build the AutoAttack from the elements indise the JSON and _isLoaded = true.
    **/
    protected void LoadChampionData(string championName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            DataChampionLoader[] data = JsonHelper.getJsonArray<DataChampionLoader>(jsonFile);
            foreach (DataChampionLoader champion in data)
            {
                if (champion.Name == championName)
                {
                    _dataChampionLoader = champion;
                    _isLoaded = true;
                    break;
                }
            }

            if(! _isLoaded)
            {
                string defaultElement = "DefaultChampion";
                DisplayErroDefault(championName, json, defaultElement);
                LoadChampionData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot find data on : " + json);
        }
    }
}
