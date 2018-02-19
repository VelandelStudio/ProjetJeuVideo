using UnityEngine;
using System.IO;

/** SpellData public class
 * @extends : Datas,
 * @implements : ISpellDisplayable
 * This Data class is specific to Spells. We handle here every method to correctly launch a Spells from the JSON file.
 **/
public class SpellData : Datas, ISpellDisplayable
{
    public string Element { get; protected set; }
    public string Type { get; protected set; }
    public float CoolDownValue { get; protected set; }
    public int[] Damages { get; protected set; }
    public string[] DamagesType { get; protected set; }
    public GameObject[] Status { get; protected set; }
    public int NumberOfStacks { get; protected set; }

    public bool HasGCD;

    private DataSpellLoader _dataSpellLoader;

    /** SpellData, public constructor
	 * @param : string 
	 * This Constructor is designed to get a Spell name as a parameter and load this one from the Json
	 * Once loading is done, we attribute each variable to the corresponding property.
	 **/
    public SpellData(string spellName) : base(spellName)
    {
        LoadSpellData(spellName, "SpellData.json");
        ScriptName = _dataSpellLoader.ScriptName;
        Name = _dataSpellLoader.Name;
        Element = _dataSpellLoader.Element;
        Type = _dataSpellLoader.Type;
        CoolDownValue = _dataSpellLoader.CoolDownValue;
        HasGCD = _dataSpellLoader.HasGCD;
        Damages = _dataSpellLoader.Damages;
        DamagesType = _dataSpellLoader.DamagesType;
        OtherValues = _dataSpellLoader.OtherValues;
        NumberOfStacks = _dataSpellLoader.NumberOfStacks;
        Description = _dataSpellLoader.Description;

        if (_dataSpellLoader.Status.Length > 0 && _dataSpellLoader.Status[0] != "")
        {
            Status = AttributeStatus(_dataSpellLoader.Status);
        }
    }

    /** LoadSpellData, protected void Method
    * This Method is launched by the contructor. Once launched, we try to locate a JSON File associated to this Spell.
    * If we find the Spell in the file, then we build the Spell from the elements inside the JSON and _isLoaded = true.
    **/
    protected void LoadSpellData(string spellName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            DataSpellLoader[] data = JsonHelper.getJsonArray<DataSpellLoader>(jsonFile);
            foreach (DataSpellLoader spell in data)
            {
                if (spell.ScriptName == spellName)
                {
                    _dataSpellLoader = spell;
                    _isLoaded = true;
                    break;
                }
            }

            if (!_isLoaded)
            {
                string defaultElement = "DefaultSpell";
                DisplayErroDefault(spellName, json, defaultElement);
                LoadSpellData(defaultElement, json);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data on : " + this.GetType().ToString());
        }
    }
}