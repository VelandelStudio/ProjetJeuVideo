using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FinalChest public class
 * @extends ActivableMechanism
 * This script is associated to the chest and thet end of the dungeon.
 **/
public class FinalChest : ActivableMechanism
{
    /** ActivateInterractable, public override void 
	 * Handles the behaviour of the chest when it is activated.
     * We Set the Exit portal activable.
	 * Currently, displays a Debug.Log and Detroy the gameObject
	 **/
    public override void ActivateInterractable(Collider other)
    {
        GetComponentInParent<DungeonExit>().SetActivable();
        int bonusChest = GetComponentInParent<DungeonManager>().ChallengeBonus;
        Debug.Log("Chest is Open !");
        if (bonusChest > 0)
        {
            Debug.Log("bonusChest <b><color=yellow>+ " + bonusChest + "%</color></b>");
        }
        Destroy(gameObject);
    }
}
