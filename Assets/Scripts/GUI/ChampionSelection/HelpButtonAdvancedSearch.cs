using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** HelpButtonAdvancedSearch, public class
 * This script is associated to the Help Button on the Advanced search panel
 * It is used to display more information on the panel.
 **/
public class HelpButtonAdvancedSearch : MonoBehaviour {

    [SerializeField] private Text _tankIcon;
    [SerializeField] private Text _healIcon;
    [SerializeField] private Text _DPSIcon;

    [SerializeField] private Text _meleeIcon;
    [SerializeField] private Text _rangedIcon;
    [SerializeField] private Text _monoTargetIcon;
    [SerializeField] private Text _multiTargetIcon;

    [SerializeField] private Text _fireIcon;
    [SerializeField] private Text _waterIcon;
    [SerializeField] private Text _stormIcon;
    [SerializeField] private Text _natureIcon;
    [SerializeField] private Text _voidIcon;
    [SerializeField] private Text _arcanesIcon;
    [SerializeField] private Text _lightIcon;

    [SerializeField] private Text _roles;
    [SerializeField] private Text _types;
    [SerializeField] private Text _elements;

    /** SwapHelpInformations, public void
     * this Method is launched by the OnClick method on the button.
     * When launched we swap the button color and informations on the panel.
     * If we click again on the button then this method is launched again and the panel goes back to its original state.
     **/
    public void SwapHelpInformations()
    {
        Button button = GetComponent<Button>();
        ColorBlock cb = button.colors;
        Color colorTmp = cb.normalColor;
        cb.normalColor = button.colors.pressedColor;
        cb.highlightedColor = cb.normalColor;
        cb.pressedColor = colorTmp;
        button.colors = cb;

        _roles.enabled = !_roles.enabled;
        _types.enabled = !_types.enabled;
        _elements.enabled = !_elements.enabled;

        _tankIcon.enabled = !_tankIcon.enabled;
        _healIcon.enabled = !_healIcon.enabled;
        _DPSIcon.enabled = !_DPSIcon.enabled;

        _meleeIcon.enabled = !_meleeIcon.enabled;
        _rangedIcon.enabled = !_rangedIcon.enabled;
        _monoTargetIcon.enabled = !_monoTargetIcon.enabled;
        _multiTargetIcon.enabled = !_multiTargetIcon.enabled;

        _fireIcon.enabled = !_fireIcon.enabled;
        _waterIcon.enabled = !_waterIcon.enabled;
        _stormIcon.enabled = !_stormIcon.enabled;
        _natureIcon.enabled = !_natureIcon.enabled;
        _voidIcon.enabled = !_voidIcon.enabled;
        _arcanesIcon.enabled = !_arcanesIcon.enabled;
        _lightIcon.enabled = !_lightIcon.enabled;
    }
}
