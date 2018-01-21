using UnityEngine;
using UnityEngine.UI;
using System.IO;

/** ChampionSelectionPanel, public class
 * This script handles the behaviour of the ChampionSelectionPanel.
 * It is able to load the ChampionData json file and creates every button associated to each champion.
 **/
public class ChampionSelectionPanel : MonoBehaviour {
    private ChampionData[] _championData;
    [SerializeField] private GameObject _grid;
    [SerializeField] private GameObject _championDescriptionPanel;

    private GameObject _championButton;

    /** Awake private void Method
     * The Awake method is going to read the Json file ChampionData. For each champion encountered, we attribute to the Grid
     * a new button that will display the Champion Informations
     * Once we are done, we disable this Menu because we do not want to see when we launch the game.
     **/
    private void Awake()
    {
        _championButton = (GameObject)Resources.Load("GUI/ChampionSelectionButton");


        string filePath = Path.Combine(Application.streamingAssetsPath, "ChampionData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            _championData = JsonHelper.getJsonArray<ChampionData>(jsonFile);
            for (int i = 0; i < _championData.Length; i++)
            {
                if(_championData[i].Name == "DefaultChampion")
                {
                    continue;
                }

                GameObject objInst = Instantiate(_championButton,_grid.transform);
                
                objInst.GetComponentInChildren<Text>().text = _championData[i].Name;
                GUIChampionSelectionButton button = objInst.GetComponent<GUIChampionSelectionButton>();
                button.AttributeElements(_championDescriptionPanel, _championData[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /** ChampionData protected Serializable class.
	 * This class were designed to be at the service of the ChampionSelectionPanel class.
	 * It is used as a JSON Object to stock every variables read from the JSON file.
	 **/
    [System.Serializable]
    public class ChampionData
    {
        public string Name;
        public string Passive;
        public string AutoAttack;
        public string[] ActiveSpells;
        public string Element;
        public string[] Description;
        public string[] Tags;
    }
}
