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

    private bool _wallMoving = true;
    private Vector3 origin;
    /** Start private void method
	 * The start method is used to get the StormWallSpell associated to this wall.
	 * Then we detach the gameObjet of its parent.
	 **/
    private void Start()
    {
        _parentSpell = GetComponentInParent<StormWallSpell>();
        origin = transform.position;
        transform.parent = null;
        transform.position += new Vector3(0, transform.lossyScale.y/2, 0);
    }

    private void Update()
    {
        if (_wallMoving)
        {
            transform.position += transform.forward * Time.deltaTime * 5;
            if (Vector3.Distance(transform.position, origin) >= 30f)
            {
                _wallMoving = false;
            }
        }
    }

    /** OnTriggerEnter private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerEnter(Collider other)
    {
        _parentSpell.ApplyEffect(other);
        Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
        rb.AddForceAtPosition(other.transform.position * 100, other.transform.position);
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