using UnityEngine;
using UnityEngine.UI;
using System.IO;

/** GUIChampionDescriptionPanel, public class
 * This class handles the behaviour of the GUIChampionDescriptionPanel.
 * You should notice that this script is called by the ChampionSelectionButton which handles only string elements of a champion.
 * In that way, we need, in that script, to load every json file associated to every component of a champion.
 **/
public class GUIChampionDescriptionPanel : MonoBehaviour {

    private SpellData[] _spellDefinition;
    private PassiveData _passiveDefinition;
    private PassiveBase _passive;

    private AutoAttackData _autoAttackDefinition;
    private string _championName;

    [SerializeField] private GameObject AutoAttackField;
    [SerializeField] private GameObject PassiveField;
    [SerializeField] private GameObject[] SpellFields;

    [SerializeField] private Image _championImage;
    [SerializeField] private Text _championDescription;
    [SerializeField] private Image _championElement;
    [SerializeField] private GameObject _tagBar;

    /** LoadAndDisplayData, public void
     * @param : string, string[], string, string, string[]
     * This method is launched when a ChampionButton is pressed.
     * When launched, we load every eelement from json files (please see other method description).
     **/
    public void LoadAndDisplayData(ChampionSelectionPanel.ChampionData data)
    {
        _championName = data.Name;

        _championDescription.text = "<i>"+string.Join("", data.Description)+"</i>";
        _championElement.sprite = Resources.Load<Sprite>("Images/Elements/" + data.Element);
        Sprite spritechamp = Resources.Load<Sprite>("Images/Champions/" + data.Name + "/" + data.Name);
        _championImage.sprite = spritechamp;

        LoadSpellData(data.ActiveSpells);
        LoadPassiveData(data.Passive);
        LoadAutoAttackData(data.AutoAttack);
        SetTagsSprites(_tagBar, data.Tags);
    }

    /** LoadAutoAttackData, private void Method
     * @param : string
     * This method is used to load an autoattack from the json data and attribute all elements on the GUI.
     **/
    private void LoadAutoAttackData(string autoAttack)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "AutoAttackData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            AutoAttackData[] data = JsonHelper.getJsonArray<AutoAttackData>(jsonFile);
            foreach (AutoAttackData autoAttackData in data)
            {
                if (autoAttackData.ScriptName == autoAttack)
                {
                    _autoAttackDefinition = autoAttackData;
                    Sprite spriteAutoAttack = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _autoAttackDefinition.ScriptName);
                    Sprite spriteElement = Resources.Load<Sprite>("Images/Elements/" + _autoAttackDefinition.Element);
                    Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_autoAttackDefinition.DamagesType));
                    Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _autoAttackDefinition.Type);

                    SetSpritesToImages(AutoAttackField, spriteAutoAttack, spriteElement, spriteType,typeAttack);
                    WrapperDisplayable wrapper = new WrapperDisplayable(_autoAttackDefinition, _championName);
                    SetDescriptionsToTexts(AutoAttackField, wrapper.GetDescriptionGUI());
                    break;
                }
            }
        }
    }

    /** LoadPassiveData, private void Method
     * @param : string
     * This method is used to load a passive from the json data and attribute all elements on the GUI.
     **/
    private void LoadPassiveData(string passive)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "PassiveData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            PassiveData[] data = JsonHelper.getJsonArray<PassiveData>(jsonFile);
            foreach (PassiveData passiveData in data)
            {
                if (passiveData.ScriptName == passive)
                {
                    _passiveDefinition = passiveData;
                    Sprite spritePassive = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _passiveDefinition.ScriptName);
                    Sprite spriteElement = null;
                    Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_passiveDefinition.DamagesType));
                    Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _passiveDefinition.Type);

                    SetSpritesToImages(PassiveField, spritePassive, spriteElement, spriteType, typeAttack);
                    WrapperDisplayable wrapper = new WrapperDisplayable(_passiveDefinition, _championName);
                    SetDescriptionsToTexts(PassiveField, wrapper.GetDescriptionGUI());
                    break;
                }
            }
        }

    }

    /** LoadPassiveData, private void Method
     * @param : string[]
     * This method is used to load every spells from the json data and attribute all elements on the GUI.
     **/
    private void LoadSpellData(string[] spells)
    {
        _spellDefinition = new SpellData[spells.Length];
        string filePath = Path.Combine(Application.streamingAssetsPath, "SpellData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            SpellData[] data = JsonHelper.getJsonArray<SpellData>(jsonFile);
            foreach (SpellData spell in data)
            {
                for (int i = 0; i < spells.Length; i++)
                {
                    if (spell.ScriptName == spells[i])
                    {
                        _spellDefinition[i] = spell;
                        Sprite spriteSpell = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _spellDefinition[i].ScriptName);
                        Sprite spriteElement = Resources.Load<Sprite>("Images/Elements/" + _spellDefinition[i].Element);
                        Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_spellDefinition[i].DamagesType));
                        Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _spellDefinition[i].Type);
                        SetSpritesToImages(SpellFields[i], spriteSpell, spriteElement, spriteType, typeAttack);
                        WrapperDisplayable wrapper = new WrapperDisplayable(_spellDefinition[i], _championName);
                        SetDescriptionsToTexts(SpellFields[i], wrapper.GetDescriptionGUI());
                        break;
                    }
                }
            }
        }
    }

    /** InvokeChampion, public void
     * This Method is associated to the OnClick event of the Invoke button of the panel.
     * When clicked, we get the old champion and instantiate the new champion (associated to this button).
     * Then, we destroy the oldChampion
     **/
    public void InvokeChampion()
    {
        Champion oldChampion = Camera.main.gameObject.GetComponentInParent<Champion>();
        GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + _championName);
        Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
        Destroy(oldChampion.gameObject);
        gameObject.SetActive(false);
    }

    /** SetSpritesToImages, private void
     * @param : GameObject, Sprite, Sprite, Sprite
     * We attribute every sprites to every elements on the panel such as element, types and Spell images.
     * Please note that if element or type is null (the attack has no element for exemple), no image will be displayabe.
     **/
    private void SetSpritesToImages(GameObject dest, Sprite spellImage, Sprite element, Sprite type, Sprite typeAttack)
    {
        Transform images = dest.transform.Find("Images");
        Sprite defaultSprite = Resources.Load<Sprite>("Images/DefaultSprite");
        Transform componentImageTR = images.Find("ComponentImage");
        Transform typeAttackTR = images.Find("ComponentImage/TypeAttack");
        Transform elementTR = images.Find("TypeImages/Element");
        Transform typeTR = images.Find("TypeImages/Type");

        spellImage = spellImage ?? defaultSprite;
        componentImageTR.GetComponent<Image>().sprite = spellImage;
        elementTR.GetComponent<Image>().sprite = element;
        typeTR.GetComponent<Image>().sprite = type;
        typeAttackTR.GetComponent<Image>().sprite = typeAttack;

        elementTR.gameObject.SetActive(element);
        typeTR.gameObject.SetActive(type); 
        typeAttackTR.gameObject.SetActive(typeAttack);
    }

    /** SetDescriptionsToTexts, private void
     * this method is used to attribute the description text to the text field of the GUI.
     **/
    private void SetDescriptionsToTexts(GameObject dest, string description)
    {
        Transform texts = dest.transform.Find("Texts");
        Transform descriptionText = texts.Find("Description");

        descriptionText.GetComponent<Text>().text = description;
    }

    private void SetTagsSprites(GameObject tagBar, string[] tags)
    {
        GameObject tagObj = (GameObject)Resources.Load("GUI/Tag");
        foreach (Transform child in tagBar.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < tags.Length; i++)
        {
            tagObj.GetComponent<Image>().sprite =  Resources.Load<Sprite>("Images/Types/" + tags[i]);
            Instantiate(tagObj, tagBar.transform);
        }
    }

    /** SpellData public Serializable class
	 * This class war created to be at the service of the Spell class
	 * This class contains all elements to construct a spell from the JSON file.
	 **/
    [System.Serializable]
    public class SpellData
    {
        public string ScriptName;
        public string Name;
        public string Type;
        public string Element;
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
    public class PassiveData
    {
        public string ScriptName;
        public string Name;
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
    public class AutoAttackData
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

    /** WrapperDisplayable, public Serializable class
     * This Serializable Class is used to get all elements we need to construct an WrapperDisplayable from a Data.
     * This Wrapper is used to make the datas of this class feat with a Displayable element. It is like a Fake Displayable class.
     * When transformed to a IDisplayabl element, we can use the FormateDescription method inside the StringHelper static class
     * that uses only IDisplayable.
     **/
    [System.Serializable]
    public class WrapperDisplayable : IDisplayable
    {
        public WrapperDisplayable(PassiveData passive, string championName)
        {

            NumberOfStacks = passive.NumberOfStacks;
            Description = passive.Description;
            Name = passive.Name;
            Type = passive.Type;
            Damages = passive.Damages;
            DamagesType = passive.DamagesType;
            OtherValues = passive.OtherValues;
            NumberOfStacks = passive.NumberOfStacks;
            Description = passive.Description;

            if (passive.Status.Length > 0 && passive.Status[0] != "")
            {
                Status = new GameObject[passive.Status.Length];
                for (int i = 0; i < passive.Status.Length; i++)
                {
                    Status[i] = (GameObject)Resources.Load(championName + "/" + passive.Status[i]);
                    if (Status[i] == null || !Status[i].GetComponent<StatusBase>().PreWarm())
                    {
                        Debug.Log(passive.Status[i] + " can not be loaded. "
                                 + "Please Ensure that the Status Name is correct in the SpellData.json file "
                                 + "or that this Status exists as a Prefab with the same Script Name associated to it. "
                                 + "DefaultStatus substitued");
                        Status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                        Status[i].GetComponent<StatusBase>().PreWarm();
                    }
                }
            }
        }

        public WrapperDisplayable(AutoAttackData autoAttack, string championName)
        {
            Name = autoAttack.Name;
            Element = autoAttack.Element;
            Type = autoAttack.Type;
            CoolDownValue = autoAttack.CoolDownValue;
            Damages = autoAttack.Damages;
            DamagesType = autoAttack.DamagesType;
            OtherValues = autoAttack.OtherValues;
            Description = autoAttack.Description;
            if (autoAttack.Status.Length > 0 && autoAttack.Status[0] != "")
            {
                Status = new GameObject[autoAttack.Status.Length];
                for (int i = 0; i < autoAttack.Status.Length; i++)
                {
                    Status[i] = (GameObject)Resources.Load(championName + "/" + autoAttack.Status[i]);
                    if (Status[i] == null || !Status[i].GetComponent<StatusBase>().PreWarm())
                    {
                        Debug.Log(autoAttack.Status[i] + " can not be loaded. "
                                 + "Please Ensure that the Status Name is correct in the SpellData.json file "
                                 + "or that this Status exists as a Prefab with the same Script Name associated to it. "
                                 + "DefaultStatus substitued");
                        Status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                        Status[i].GetComponent<StatusBase>().PreWarm();
                    }
                }
            }
            
        }

        public WrapperDisplayable(SpellData spellData, string championName)
        {
            Name = spellData.Name;
            Element = spellData.Element;
            Type = spellData.Type;
            CoolDownValue = spellData.CoolDownValue;
            Damages = spellData.Damages;
            DamagesType = spellData.DamagesType;
            OtherValues = spellData.OtherValues;
            NumberOfStacks = spellData.NumberOfStacks;
            Description = spellData.Description;

            if (spellData.Status.Length > 0 && spellData.Status[0] != "")
            {
                Status = new GameObject[spellData.Status.Length];
                for (int i = 0; i < spellData.Status.Length; i++)
                {
                    Status[i] = (GameObject)Resources.Load(championName + "/" + spellData.Status[i]);
                    if (Status[i] == null || !Status[i].GetComponent<StatusBase>().PreWarm())
                    {
                        Debug.Log(spellData.Status[i] + " can not be loaded. "
                                 + "Please Ensure that the Status Name is correct in the SpellData.json file, "
                                 + "or that this Status exists as a Prefab with the same Script Name associated to it, "
                                 + "or that the Status Name is correct in the StatusData.json file. "
                                 + "DefaultStatus substitued");
                        Status[i] = (GameObject)Resources.Load("Default/DefaultStatus");
                        Status[i].GetComponent<StatusBase>().PreWarm();
                    }
                }
            }
        }

        public string Name { get; protected set; }
        public string Element { get; protected set; }
        public string Type { get; protected set; }
        public float CoolDownValue { get; protected set; }
        public int[] Damages { get; protected set; }
        public string[] DamagesType { get; protected set; }
        public string[] OtherValues { get; protected set; }
        public GameObject[] Status { get; protected set; }
        public string[] Description { get; protected set; }
        public int NumberOfStacks { get; protected set; }

        public bool IsLoaded { get; set; }
        public string GetDescriptionGUI()
        {
            return StringHelper.DescriptionBuilder(this);
        }
    }
}
