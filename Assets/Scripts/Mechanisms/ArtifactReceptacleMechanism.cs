using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ArtifactReceptacleMechanism, public class
 * this script is associated with the ArtifactReceptacle prefab
 * It is used to display the ChampionSelection panel
 **/
public class ArtifactReceptacleMechanism : ActivableMechanism {
    [SerializeField] private GameObject _activableMenus;

    /** ActivateInterractable Method
     * This Method overrides the parent one.
     * It detects if the mechanism as not been activated yet.
     * After activation it launches the dunjon and then Destroyes itself to provide multiple launches.
     * Warning ! Only the script will be Destroyed, not the GameObject
     */
    public override void ActivateInterractable(Collider other)
    {
         _activableMenus.SetActive(!_activableMenus.activeSelf);
    }

    /** OnTriggerExit, protected void method,
     * @param : Collider
     * This trigger is used th close th menu if the player goes too far.
     **/
    protected void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _activableMenus.SetActive(false);
        }
    }
}
