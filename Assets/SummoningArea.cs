using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** SummoningArea, public class
 * This class is associated to the SummoningArea Prefab
 * It is use to define areas in the Domain where summoning is allowed.
 * When you leave this area, the player comes back to its Summoner state
 **/
public class SummoningArea : MonoBehaviour {

    [SerializeField] private DungeonLauncher dungeonLauncher;

    /** OnTriggerExit, private void
     * @param : Collider
     * This method is used to detect if a player leaves the area.
     * it that is the case, we try to get a Chmapion instance on this player. 
     * If we catch one, we destroy it and intantiate a instance of Summoner prefab.
     **/
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Champion champion = other.GetComponent<Champion>();
            if(champion)
            {
                champion.DestroyChampion();
                dungeonLauncher.DeactivateDungeonLauncher();
            }
        }
    }
}
