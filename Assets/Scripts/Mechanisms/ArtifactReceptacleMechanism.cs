using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** ArtifactReceptacleMechanism, public class
 * this script is associated with the ArtifactReceptacle prefab
 * It is used to display the ChampionSelection panel
 **/
public class ArtifactReceptacleMechanism : ActivableMechanism
{
    [SerializeField] private GameObject _activableMenus;

    /** ActivateInterractable Method
     * This Method overrides the parent one.
     * It detects if the mechanism as not been activated yet.
     * After activation it launches the dunjon and then Destroyes itself to provide multiple launches.
     * Warning ! Only the script will be Destroyed, not the GameObject
     */
    public override void ActivateInterractable(Collider other)
    {
        _activableMenus.SetActive(true);
    }

    public override void CancelTextOfInterractable()
    {
        _activableMenus.SetActive(false);
    }
}
