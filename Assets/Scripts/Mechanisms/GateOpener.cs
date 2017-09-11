using UnityEngine;
using System.Collections;

/** GateOpener Class
 * @Inherits MechanismBase
 * This Script should only be attached to the RoomGate Prefab.
 **/
[RequireComponent(typeof(Collider))]
public class GateOpener : MechanismBase {

    public Animator animGate1; //The left side of the door with inside room point of view
    public Animator animGate2; //The left side of the door with inside room point of view
    public Animator animHandler; //To move the lever

    Collider col; // The attached boxCollider of the assembly object

    /** Start Method
     * Just here to grab the boxCollider of the gameobject
     **/
    private void Start()
    {
        col = GetComponent<Collider>();
    }

    /** ActivateMechanism Method
     * This Method overrides the parent one
     * It detects if the mechanism as not been activated yet
     * When it's activated it open the doors
     * it set the collider to false in order to pass trought the door.
     * It destroy the script also because the door need to be open just once
     **/
    public override void ActivateMechanism()
    {
        if (!isActivated)
        {
            Debug.Log("Open the Door");
            animHandler.SetTrigger("MoveLever");

            StartCoroutine(CoroutineOneSec());
        }
    }

    private void OpenDoor()
    {
        animGate1.SetTrigger("OpenLeftDoor");
        animGate2.SetTrigger("OpenRightDoor");

        col.enabled = false;
        base.ActivateMechanism();

        Destroy(this);
    }

    IEnumerator CoroutineOneSec()
    {
        Debug.Log("Start Coroutine");

        yield return new WaitForSeconds(1f);

        OpenDoor();
    }
}
