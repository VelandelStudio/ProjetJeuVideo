using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactReceptacleMechanism : MechanismBase {
    [SerializeField] private GameObject _activableMenus;
    [SerializeField] private DungeonLauncher dungeonLauncher;

    private bool artifactLoaded = false;

    /** ActivateInterractable Method
     * This Method overrides the parent one.
     * It detects if the mechanism as not been activated yet.
     * After activation it launches the dunjon and then Destroyes itself to provide multiple launches.
     * Warning ! Only the script will be Destroyed, not the GameObject
     */
    public override void ActivateInterractable()
    {
        if (!isActivated)
        {
            _activableMenus.SetActive(!_activableMenus.activeSelf);
        }
    }

    public void ArtifactIsLoaded()
    {
        dungeonLauncher.ActivateDungeonLauncher();
    }

    protected void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _activableMenus.SetActive(false);
        }
    }
}
