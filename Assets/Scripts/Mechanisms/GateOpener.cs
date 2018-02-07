using UnityEngine;
using System.Collections;

/** GateOpener Class
 * @Inherits MechanismBase
 * This Script should only be attached to the RoomGate Prefab.
 **/
public class GateOpener : ActivableMechanism
{
    public Animator animGate1; //The left side of the door with inside room point of view
    public Animator animGate2; //The left side of the door with inside room point of view

    private bool _isOpen;
    public bool IsOpen
    {
        get { return _isOpen; }
        protected set { }
    }

    public Animator[] animHandlers; //To move the levers

    protected RoomBehaviour roomBehaviour;

    /** IsActivable, public override bool Property
	 * The GateOpener is not activable only if a room is not clean and if Players are inside this room.
	 **/
    public override bool IsActivable
    {
        get
        {
            if (roomBehaviour)
            {
                return !(!roomBehaviour.IsClean && roomBehaviour.PlayerAreInside);
            }
            else
            {
                return true;
            }
        }
        protected set { }
    }

    /** Start, protected void method
	 * We try to get here the RoomBehaviour in our parent room.
	 **/
    protected void Start()
    {
        roomBehaviour = GetComponentInParent<RoomBehaviour>();
    }

    /** ActivateInterractable Method
     * This Method overrides the parent one
     * It detects if the mechanism as not been activated yet
     * When it's activated it put down the lever and call OpenDoor() Method
     **/
    public override void ActivateInterractable(Collider other)
    {
        foreach (Animator anim in animHandlers)
        {
            anim.SetTrigger("MoveLever");
        }

        if (transform.parent.tag == "StartRoom")
        {
            GetComponentInParent<DungeonManager>().Instance.StartDungeonTimer();
            Debug.Log("Hello");
        }

        OpenDoor();
        InitializerRoomTrigger intializerRoomTrigger = GetComponentInChildren<InitializerRoomTrigger>();
        if (intializerRoomTrigger)
        {
            Destroy(intializerRoomTrigger.gameObject);
        }
        Destroy(mechanismObject);
        Destroy(this);
    }

    /** OpenDoor, public void Method
     * This method opens the doors
     * It sets the collider to false in order to pass trough the door.
     **/
    public void OpenDoor()
    {
        animGate1.SetTrigger("OpenLeftDoor");
        animGate2.SetTrigger("OpenRightDoor");
        _isOpen = true;
    }

    /** CloseDoor, public void Method
     * This method closes the doors
     * It sets the collider to false.
     **/
    public void CloseDoor()
    {
        animGate1.SetTrigger("CloseLeftDoor");
        animGate2.SetTrigger("CloseRightDoor");
        _isOpen = false;
    }
}
