using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** ChampionSelectionButton public class,
 * This class handles the behaviour of the ChampionSelectionButton. The maingoal of this class is to transmit informations
 * from the ChampionSelection Panel to the ChampionDescriptionPanel
 **/
public class ChampionSelectionButton : MonoBehaviour {

    private string _championName;
    private string _passive;
    private string _autoAttack;
    private string[] _spells;
    private GameObject _championDescriptionPanel;

    /** AttributeElements, public void method
     * @param : GameObject, string, string, string, string[]
     * This method should be called by the GUIChampionSelectionPanel to passe its information to the button associated to the champion.
     **/
    public void AttributeElements(GameObject championDescriptionPanel, string championName, string passive, string autoAttack, string[] spells)
    {
        _championDescriptionPanel = championDescriptionPanel;
        _championName = championName;
        _passive = passive;
        _autoAttack = autoAttack;
        _spells = spells;
    }

    /** DisplayDescription, public void method
     * This method is called by the OnClick event from the bytton.
     * When clicked, we activate the ChampionDescriptionPanel and Load the data of the Champion associated to the button.
     * We obviously activate the panel.
     **/
    public void DisplayDescription()
    {
        _championDescriptionPanel.SetActive(true);
        _championDescriptionPanel.GetComponent<GUIChampionDescriptionPanel>().LoadAndDisplayData(_championName, _passive, _autoAttack,_spells);
    }
}
