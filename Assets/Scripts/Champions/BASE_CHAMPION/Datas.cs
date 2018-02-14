using UnityEngine;

public class Datas
{

    public string ScriptName { get; protected set; }
    public string Name { get; protected set; }

    protected bool _isLoaded = false;
    public bool IsLoaded { get; protected set; }

    /** LoadResource, protected virtual GameObject Method
	 * @param : string,
	 * @return : GameObject
	 * This method is used to load a GameObject prefab inside the champion folder.
	 **/
    protected GameObject LoadStatus(string prefabName)
    {
        Debug.Log("Status/" + prefabName);
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
    #endregion
}