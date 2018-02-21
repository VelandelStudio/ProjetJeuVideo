using UnityEngine;
using System.IO;

/** ChampionData public class
 * @extends : Datas,
 * This Data class is specific to Champions. We handle here every method to correctly launch a Champion from the JSON file.
 **/
public class ChampionData : Datas
{

    public string Passive { get; protected set; }
    public string AutoAttack { get; protected set; }
    public string[] ActiveSpells { get; protected set; }
    public string Element { get; protected set; }
    public string[] Tags { get; protected set; }

    private DataChampionLoader _dataChampionLoader;

    /** ChampionData, public constructor
	 * @param : string 
	 * This Constructor is designed to get a Champion name as a parameter and load this one from the Json
	 * Once loading is done, we attribute each variable to the corresponding property.
	 **/
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

    /** LoadChampionData, protected void Method
	* @param : string, string
    * This Method is launched by the Constructor one. Once launched, we try to locate a JSON File associated to this AutoAttack.
    * If we find the Champion in the file, then we build the Champion from the elements inside the JSON and _isLoaded = true.
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

            if (!_isLoaded)
            {
                string defaultElement = "DefaultChampion";
                DisplayErroDefault(championName, json, defaultElement);
                LoadChampionData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }
}