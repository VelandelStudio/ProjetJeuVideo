using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** StormWallBehaviour, public class
 * This script should always be associated with the StormWall prefab which is one of the prefabs created by the Windiator Champion
 * The main object of the screen is to get the associated spell from the parent and contact it when something passes through the wall.
 * Please note that the prefab has a collider trigger.
 **/
public class StormWallBehaviour : MonoBehaviour
{

    private StormWallSpell _parentSpell;

    /** Start private void method
	 * The start method is used to get the StormWallSpell associated to this wall.
	 * Then we detach the gameObjet of its parent.
	 **/
    private void Start()
    {
        _parentSpell = GetComponentInParent<StormWallSpell>();
        transform.parent = null;
    }

    /** OnTriggerEnter private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerEnter(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }


    /** OnTriggerStay private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerStay(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }

    /** OnTriggerStay private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerExit(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }
}