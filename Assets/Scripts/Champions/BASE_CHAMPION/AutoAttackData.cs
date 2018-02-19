using UnityEngine;
using System.IO;

/** AutoAttackData public class
 * @extends : Datas,
 * @implements : ISpellDisplayable
 * This Data class is specific to AutoAttacks. We handle here every method to correctly launch an AutoAttack from the JSON file.
 **/
public class AutoAttackData : Datas, ISpellDisplayable
{
    public string Element { get; protected set; }
    public string Type { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public GameObject[] Status { get; protected set; }

    private DataAutoAttackLoader _dataAutoAttackLoader;

    /** AutoAttackData, public constructor
	 * @param : string 
	 * This Constructor is designed to get an auto attack name as a parameter and load this one from the Json
	 * Once loading is done, we attribute each variable to the corresponding property.
	 **/
    public AutoAttackData(string autoAttackName) : base(autoAttackName)
    {
        LoadAutoAttackData(autoAttackName, "AutoAttackData.json");
        ScriptName = _dataAutoAttackLoader.ScriptName;
        Name = _dataAutoAttackLoader.Name;
        Element = _dataAutoAttackLoader.Element;
        Type = _dataAutoAttackLoader.Type;
        Damages = _dataAutoAttackLoader.Damages;
        CoolDownValue = _dataAutoAttackLoader.CoolDownValue;
        DamagesType = _dataAutoAttackLoader.DamagesType;
        OtherValues = _dataAutoAttackLoader.OtherValues;
        Description = _dataAutoAttackLoader.Description;

        if (_dataAutoAttackLoader.Status.Length > 0 && _dataAutoAttackLoader.Status[0] != "")
        {
            Status = AttributeStatus(_dataAutoAttackLoader.Status);
        }
    }

    /** LoadAutoAttackData, protected void Method
    * This Method is launched by the contructor. Once launched, we try to locate a JSON File associated to this AutoAttack.
    * If we find the AutoAttack in the file, then we build the AutoAttack from the elements inside the JSON and _isLoaded = true.
    **/
    protected void LoadAutoAttackData(string autoAttackName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            DataAutoAttackLoader[] data = JsonHelper.getJsonArray<DataAutoAttackLoader>(jsonFile);
            foreach (DataAutoAttackLoader autoAttack in data)
            {
                if (autoAttack.ScriptName == autoAttackName)
                {
                    _dataAutoAttackLoader = autoAttack;
                    _isLoaded = true;
                    break;
                }
            }

            if (!_isLoaded)
            {
                string defaultElement = "DefaultAutoAttack";
                DisplayErroDefault(autoAttackName, json, defaultElement);
                LoadAutoAttackData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }
}