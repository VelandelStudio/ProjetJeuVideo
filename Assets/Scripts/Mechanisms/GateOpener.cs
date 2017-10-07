using UnityEngine;
using System.Collections;

/** GateOpener Class
 * @Inherits MechanismBase
 * This Script should only be attached to the RoomGate Prefab.
 **/
public class GateOpener : MechanismBase {

    public Animator animGate1; //The left side of the door with inside room point of view
    public Animator animGate2; //The left side of the door with inside room point of view

    public Animator[] animHandlers; //To move the levers

    /** ActivateInterractable Method
     * This Method overrides the parent one
     * It detects if the mechanism as not been activated yet
     * When it's activated it put down the lever and call OpenDoor() Method
     **/
    public override void ActivateInterractable()
    {
        if (!isActivated)
        {
            base.ActivateInterractable();

            Debug.Log("Open the Door");

            foreach (Animator anim in animHandlers)
                anim.SetTrigger("MoveLever");

            StartCoroutine(CoroutineOneSec());
        }
    }

    /** OpenDoor Method
     * This Method is called during the Coroutine : CoroutineOneSec() below
     * this method open the doors
     * it set the collider to false in order to pass trought the door.
     * It destroy the script also because the door need to be open just once
     **/
    private void OpenDoor()
    {
        animGate1.SetTrigger("OpenLeftDoor");
        animGate2.SetTrigger("OpenRightDoor");

        Destroy(this);
    }

    /** IEnumerator CoroutineOneSec
     *  IEnumerator is needed to be called in a coroutine.
     *  Wait 1 sec and lauch others animations
     **/
    IEnumerator CoroutineOneSec()
    {
        Debug.Log("Start Coroutine");

        yield return new WaitForSeconds(1f);

        OpenDoor();
    }
}
