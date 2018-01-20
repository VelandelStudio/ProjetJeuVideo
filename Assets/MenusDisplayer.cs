using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** MenusDisplayer, public class
 * This script is associated to the gameObject ActivableMenus in the Canvas. 
 * This GameObject should contain every Menus element that can be displayed or not if the player press a Key or a button on the screen
 **/
public class MenusDisplayer : MonoBehaviour {

    [SerializeField] private GameObject _championSelectionMenu;

    /** Update private void 
     * If one of the Key associated with a menu is pressed, then we Activate or De-activate the GameObject associated.
     **/
    private void Update ()
    {
		if(Input.GetKeyDown(KeyCode.K))
        {
            _championSelectionMenu.SetActive(!_championSelectionMenu.activeSelf);
        }
	}
}
