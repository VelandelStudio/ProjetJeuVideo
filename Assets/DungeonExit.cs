using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** DungeonExit, public class
 * @extends : ActivableMechanism
 *  This activable mechanism should always be attached to the DungeonExit prefab.
 **/
public class DungeonExit : ActivableMechanism
{
    /** Start, protected void method
     * By default, we set this mechanism not activable
     **/
    protected void Start()
    {
        IsActivable = false;
    }

    /** SetActivable, public void
     * This method should be launched by the final chest only.
     * We set the mechanism activable and we start playing thbe particle system.
     **/
    public void SetActivable()
    {
        isActivable = true;
        GetComponentInChildren<ParticleSystem>().Play();
    }

    /** ActivateInterractable, public override void
     * @param : Collider
     * When activated, we set the ChampionDestroyable because he is not in the dungeon anymore.
     * Then we teleports him to the domain before destroying the dungeon;
     **/
    public override void ActivateInterractable(Collider other)
    {
        other.GetComponent<Champion>().ChampionDestroyable = true;
        other.transform.position = GameObject.Find("DungeonEnter").transform.position;
        Destroy(GetComponentInParent<DungeonManager>().gameObject);
    }
}
