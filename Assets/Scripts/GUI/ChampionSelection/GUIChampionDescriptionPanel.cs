using UnityEngine;
using UnityEngine.UI;
using System.IO;

/** GUIChampionDescriptionPanel, public class
 * This class handles the behaviour of the GUIChampionDescriptionPanel.
 * You should notice that this script is called by the GUIChampionSelectionButton which handles only string elements of a champion.
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

    [SerializeField] private GameObject invokeButton;

    /** LoadAndDisplayData, public void
     * @param : string, string[], string, string, string[]
     * This method is launched when a ChampionButton is pressed.
     * When launched, we load every element from json files (please see other method description).
     * Please note that the invokeButton is not interractable if we are playing the champion we are looking at.
     **/
    public void LoadAndDisplayData(ChampionSelectionPanel.ChampionData data)
    {
        _championName = data.Name;
        Champion oldChampion = Camera.main.GetComponentInParent<Champion>();

        invokeButton.GetComponent<Button>().interactable = oldChampion == null || oldChampion.Name != _championName;
  
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
        _autoAttackDefinition = new AutoAttackData(autoAttack);
        Sprite spriteAutoAttack = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _autoAttackDefinition.ScriptName);
        Sprite spriteElement = Resources.Load<Sprite>("Images/Elements/" + _autoAttackDefinition.Element);
        Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_autoAttackDefinition.DamagesType));
        Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _autoAttackDefinition.Type);

        SetSpritesToImages(AutoAttackField, spriteAutoAttack, spriteElement, spriteType,typeAttack);
        SetDescriptionsToTexts(AutoAttackField, _autoAttackDefinition.GetDescriptionGUI());
    }

    /** LoadPassiveData, private void Method
     * @param : string
     * This method is used to load a passive from the json data and attribute all elements on the GUI.
     **/
    private void LoadPassiveData(string passive)
    {
        _passiveDefinition = new PassiveData(passive);
        Sprite spritePassive = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _passiveDefinition.ScriptName);
        Sprite spriteElement = null;
        Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_passiveDefinition.DamagesType));
        Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _passiveDefinition.Type);

        SetSpritesToImages(PassiveField, spritePassive, spriteElement, spriteType, typeAttack);
        SetDescriptionsToTexts(PassiveField, _passiveDefinition.GetDescriptionGUI());
    }

    /** LoadPassiveData, private void Method
     * @param : string[]
     * This method is used to load every spells from the json data and attribute all elements on the GUI.
     **/
    private void LoadSpellData(string[] spells)
    {
        _spellDefinition = new SpellData[spells.Length];
        for (int i = 0; i < spells.Length; i++)
        {
            _spellDefinition[i] = new SpellData(spells[i]);
            Sprite spriteSpell = Resources.Load<Sprite>("Images/Champions/" + _championName + "/" + _spellDefinition[i].ScriptName);
            Sprite spriteElement = Resources.Load<Sprite>("Images/Elements/" + _spellDefinition[i].Element);
            Sprite spriteType = Resources.Load<Sprite>("Images/Types/" + StringHelper.GetDisplayableType(_spellDefinition[i].DamagesType));
            Sprite typeAttack = Resources.Load<Sprite>("Images/Types/" + _spellDefinition[i].Type);
            SetSpritesToImages(SpellFields[i], spriteSpell, spriteElement, spriteType, typeAttack);
            SetDescriptionsToTexts(SpellFields[i], _spellDefinition[i].GetDescriptionGUI());
        }
    }

    /** InvokeChampion, public void
     * This Method is associated to the OnClick event of the Invoke button of the panel.
     * When clicked, we get the old champion and instantiate the new champion (associated to this button).
     * Then, we destroy the oldChampion
     **/
    public void InvokeChampion()
    {
        GameObject oldChampion = Camera.main.GetComponent<CameraController>().playerFollowed.gameObject;
        GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + _championName);
        Camera.main.transform.parent = null;
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
}
