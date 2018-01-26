using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** SearchFieldBehaviour, public class 
 * This script is associated with the Search InputField of the ChampionSelectionPanel.
 * We handle here everything we need to search and find a champion inside the list of champions.
 **/
public class SearchFieldBehaviour : MonoBehaviour {

    [SerializeField] private GameObject _gridOfChampions;
    [SerializeField] private Text _inputFieldText;
    [SerializeField] private GameObject _advencedSearchPanel;

    private List<string> _elements = new List<string>();
    private List<string> _roles = new List<string>();
    private List<string> _types = new List<string>();

    private string textInput;
    private bool _advancedPanelOpen = false;

    /** Update, private void method
     * This method is used to check if every GUIChampionSelectionButton fit to the Criterias of the search.
     * At any moment, if the player changes one value of the search panel, the Champion list will be automatically updated
     **/
    private void Update()
    {
        textInput = _inputFieldText.text;
        GUIChampionSelectionButton[] buttons = _gridOfChampions.GetComponentsInChildren<GUIChampionSelectionButton>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(ButtonCorrespondsToCriterias(buttons[i]));
        }
    }

    /** LaunchAdvancedOptionSearch public void,
     * @param : text
     * This method is launched by the OnClick Method associted to the AdvancedSearch button.
     * When clicked, we display or hide the advanced panel.
     * * Please note that the text of the button is swapped if the panel is open or not.
     **/
    public void LaunchAdvancedOptionSearch(Text advancedText)
    {
        _advancedPanelOpen = !_advancedPanelOpen;
        _advencedSearchPanel.SetActive(_advancedPanelOpen);

        advancedText.text = _advancedPanelOpen ? "Advanced Search <" : "Advanced Search >";
    }

    /** AddAdvancedSearchElement public void,
     * @param : Toggle
     * This method is launched when we toggle a checkbox on the AdvnacedSearchPanel.
     * When launched, we try to catch which checkbox was checked or unchecked and we update our lists 
     * These lists contains every element of the research such as Elements, Types or Roles.
     * If no checkboxes are checked, all these lists are empty.
     **/
    public void AddAdvancedSearchElement(Toggle checkBox)
    {
        GameObject parent = checkBox.GetComponentInParent<Transform>().parent.gameObject;
        if (parent.name != "Roles" && parent.name != "Types")
        {
            if (checkBox.isOn)
            {
                _elements.Add(checkBox.gameObject.name);
            }
            else
            {
                _elements.Remove(checkBox.gameObject.name);
            }
        }
        if (parent.name == "Roles")
        {
            if (checkBox.isOn)
            {
                _roles.Add(checkBox.gameObject.name);
            }
            else
            {
                _roles.Remove(checkBox.gameObject.name);
            }
        }
        if (parent.name == "Types")
        {
            if (checkBox.isOn)
            {
                _types.Add(checkBox.gameObject.name);
            }
            else
            {
                _types.Remove(checkBox.gameObject.name);
            }
        }
    }

    /** ButtonCorrespondsToCriterias public bool,
     * @param : GUIChampionSelectionButton
     * This method is launched by the Update method of this script.
     * We try to check each frame if something in the search panel moved in order to re-organize the list of champions.
     * Please note that we will display only champion that are fill theses conditions :
     * - The champion name is a part of the SearchTextField or the SearchTextField is empty.
     * - The champion has at least one Type in common with the search or the Types list is empty.
     * - The champion has at least one Roles in common with the search or the Roles list is empty.
     * - The champion has at least one Elements in common with the search or the Elements list is empty.
     *  If we Select 1 Role and 1 Element for example, you will get only champions that have both of them.
     * However, if you chose 2 Roles, you will see all champions that belongs to one or an other role.
     **/
    private bool ButtonCorrespondsToCriterias (GUIChampionSelectionButton button)
    {
        bool isSearchedInTextField = button.ButtonName.Contains(textInput) || textInput == "";
        bool hasCorrectElement = _elements.Count == 0;
        bool hasCorrectRole = _roles.Count == 0;
        bool hasCorrectType = _types.Count == 0;

        if (_elements.Count > 0)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                if (_elements[i].Contains(button.ChampionData.Element) && button.ChampionData.Element != "")
                {
                    hasCorrectElement = true;
                    break;
                }
            }
        }

        if (_roles.Count > 0)
        {
            for (int i = 0; i < button.ChampionData.Tags.Length; i++)
            {
                if (_roles.Contains(button.ChampionData.Tags[i]))
                {
                    hasCorrectRole = true;
                    break;
                }
            }
        }

        if (_types.Count > 0)
        {
            for (int i = 0; i < button.ChampionData.Tags.Length; i++)
            {
                if (_types.Contains(button.ChampionData.Tags[i]))
                {
                    hasCorrectType = true;
                    break;
                }
            }
        }
        return isSearchedInTextField && hasCorrectElement && hasCorrectType && hasCorrectRole;
    }
}
