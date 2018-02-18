using UnityEngine;
using System.IO;

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

    /** LoadSpellData, protected void
	 * @Params : string
	 * Loads the JSON _dataSpellLoader associated to the spell.
	 * If the loading is a success, then _isLoaded = true.
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
            Debug.LogError("Cannot find data on : " + json);
        }
    }
}