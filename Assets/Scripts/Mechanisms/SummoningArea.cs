using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** SummoningArea, public class
 * This class is associated to the SummoningArea Prefab
 * It is use to define areas in the Domain where summoning is allowed.
 * When you leave this area, the player comes back to its Summoner state
 **/
public class SummoningArea : MonoBehaviour
{

    [SerializeField] private DungeonLauncher dungeonLauncher;
    private bool championLoaded = false;
    /** OnTriggerExit, private void
     * @param : Collider
     * This method is used to detect if a player leaves the area.
     * it that is the case, we try to get a Chmapion instance on this player. 
     * If we catch one, we destroy it and intantiate a instance of Summoner prefab.
     **/
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Champion champion = other.GetComponent<Champion>();
            if (champion && champion.ChampionDestroyable)
            {
                champion.DestroyChampion();
            }
            championLoaded = false;
            dungeonLauncher.DeactivateDungeonLauncher();
            EntityHelper.ClearAllStatus(other.gameObject);
        }

        if (other.gameObject.GetComponent<IProjectile>() != null)
        {
            Destroy(other.gameObject);
        }
    }

    /** OnTriggerStay, private void
     * @param : Collider
     * While players are inside the Area, we try to catch the frame when they choos to summon a champion.
     * Once it is done, we notify the dungeonLauncher to activate it.
     **/
    private void OnTriggerStay(Collider other)
    {
        if (!championLoaded && other.tag == "Player" && other.GetComponent<Champion>())
        {
            championLoaded = true;
            dungeonLauncher.ActivateDungeonLauncher();
        }
    }
}