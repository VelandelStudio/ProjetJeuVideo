using UnityEngine;
using System.IO;

public class Datas : IDisplayable
{
    public string ScriptName { get; protected set; }
    public string Name { get; protected set; }

    public string[] OtherValues { get; protected set; }
    public string[] Description { get; protected set; }
    protected bool _isLoaded = false;
    public bool IsLoaded { get; protected set; }

    private DataBaseLoader _dataBaseLoader;

    public Datas(string dataName) { }
    
    public Datas(string dataName, string json)
    {
        LoadBaseData(dataName, json);
        ScriptName = _dataBaseLoader.ScriptName;
        Name = _dataBaseLoader.Name;
        OtherValues = _dataBaseLoader.OtherValues;
        Description = _dataBaseLoader.Description;
   }   

    private void LoadBaseData(string dataName, string json)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, json);
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            DataBaseLoader[] data = JsonHelper.getJsonArray<DataBaseLoader>(jsonFile);
            foreach (DataBaseLoader dataBaseLoader in data)
            {
                if (dataBaseLoader.ScriptName == dataName)
                {
                    _dataBaseLoader = dataBaseLoader;
                    _isLoaded = true;
                    break;
                }
            }

            if (!_isLoaded)
            {
                DisplayErroDefault(dataName, json, "No Default element ");
            }
        }
        else
        {
            Debug.LogError("Cannot find data on : " + json);
        }
    }

    /** LoadResource, protected virtual GameObject Method
	 * @param : string,
	 * @return : GameObject
	 * This method is used to load a GameObject prefab inside the champion folder.
	 **/
    protected GameObject LoadStatus(string prefabName)
    {
        return (GameObject)Resources.Load("Status/" + prefabName);
    }

    protected GameObject[] AttributeStatus(string[] array)
    {
        GameObject[] status = new GameObject[array.Length];
        for (int i = 0; i < status.Length; i++)
        {
            status[i] = LoadStatus(array[i]);
            if (status[i] == null || !status[i].GetComponent<StatusBase>().PreWarm())
            {
                Debug.Log(array[i] + " can not be loaded. "
                         + "Please Ensure that the Status Name is correct in the SpellData.json file, "
                         + "or that this Status exists as a Prefab with the same Script Name associated to it, "
                         + "or that the Status Name is correct in the StatusData.json file. "
                         + "DefaultStatus substitued");
                status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                status[i].GetComponent<StatusBase>().PreWarm();
            }
        }
        return status;
    }

    /**GetDescriptionGUI, public string Method
     * Return the description of our spell built by the DescriptionBuilder of the StringHelper static class.
     * This method allows to get a dynamic and colored description on the screen.
     **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }

    public void DisplayErroDefault(string elementError, string jsonFileName, string defaultClass)
    {
        Debug.Log("The element " + elementError + " could not be loaded. "
        + "Please check in " + jsonFileName + " If the element name is correct. "
        + defaultClass + " added instead.");
    }


    [System.Serializable]
    protected class DataBaseLoader
    {
        public string ScriptName;
        public string Name;
        public string[] OtherValues;
        public string[] Description;
    }

    #region Serializable Classes
    /** SpellData public Serializable class
    * This class war created to be at the service of the Spell class
    * This class contains all elements to construct a spell from the JSON file.
    **/
    [System.Serializable]
    protected class DataSpellLoader
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public string Type;
        public float CoolDownValue;
        public bool HasGCD;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public int NumberOfStacks;
        public string[] Status;
        public string[] Description;
    }

    /** PassiveData public Serializable class
	 * This class was created to be at the service of the PassiveBase class
	 * This class contains all elements to construct a Passive from the JSON file.
	 **/
    [System.Serializable]
    public class DataPassiveLoader
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public string Type;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public int NumberOfStacks;
        public string[] Status;
        public string[] Description;
    }

    /** AutoAttackData, public Serializable class
     * This Serializable Class is used to get all elements we need to construct an AutoAttack from a Json File.
     **/
    [System.Serializable]
    public class DataAutoAttackLoader
    {
        public string ScriptName;
        public string Name;
        public string Element;
        public string Type;
        public float CoolDownValue;
        public int[] Damages;
        public string[] DamagesType;
        public string[] OtherValues;
        public string[] Status;
        public string[] Description;
    }


    /** ChampionData protected Serializable class.
	 * This class were designed to be at the service of the Champion class.
	 * It is used as a JSON Object to stock every variables read from the JSON file.
	 **/
    [System.Serializable]
    protected class DataChampionLoader
    {
        public string Name;
        public string Passive;
        public string AutoAttack;
        public string[] ActiveSpells;
        public string Element;
        public string[] Description;
        public string[] Tags;
    }

    #endregion
}