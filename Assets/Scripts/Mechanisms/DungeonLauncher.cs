using UnityEngine;

/** DungeonLauncher Class
 * @Inherits MechanismBase
 * This Mecanism allows the player to launch a dunjon.
 * This Script should only be attached to the DungeonLauncher Prefab.
 **/
public class DungeonLauncher : MechanismBase
{

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
            Debug.Log("Launching new Dunjon");
            base.ActivateInterractable();
        }
        Destroy(this);
    }
}
