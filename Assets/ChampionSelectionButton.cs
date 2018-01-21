using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** ChampionSelectionButton public class,
 * This class handles the behaviour of the ChampionSelectionButton. The main goal of this class is to transmit informations
 * from the ChampionSelection Panel to the ChampionDescriptionPanel
 **/
public class ChampionSelectionButton : MonoBehaviour {

    private ChampionSelectionPanel.ChampionData _championData;
    private GameObject _championDescriptionPanel;
    public string ButtonName
    {
        get { return _championData.Name; }
        private set { }
    }

    public ChampionSelectionPanel.ChampionData ChampionData
    {
        get { return _championData; }
        private set { }
    }

    /** AttributeElements, public void method
     * @param : GameObject, ChampionSelectionPanel.ChampionData
     * This method should be called by the GUIChampionSelectionPanel to passe its information to the button associated to the champion.
     **/
    public void AttributeElements(GameObject championDescriptionPanel, ChampionSelectionPanel.ChampionData data)
    {
        _championDescriptionPanel = championDescriptionPanel;
        _championData = data;
    }

    /** DisplayDescription, public void method
     * This method is called by the OnClick event from the bytton.
     * When clicked, we activate the ChampionDescriptionPanel and Load the data of the Champion associated to the button.
     * We obviously activate the panel.
     **/
    public void DisplayDescription()
    {
        _championDescriptionPanel.SetActive(true);
        _championDescriptionPanel.GetComponent<GUIChampionDescriptionPanel>().LoadAndDisplayData(_championData);
    }
}
