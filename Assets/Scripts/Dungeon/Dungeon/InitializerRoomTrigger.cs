using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** InitializerRoomTrigger, public class
 * This script is associated to the BoxTrigger in the Corridor between two rooms.
 * When a player goes through this trigger, it opens the next room and initialize it.
 **/
public class InitializerRoomTrigger : MonoBehaviour 
{
	RoomBehaviour roomBehaviour;
	GateOpener gateOpener;
	
	/** OnTriggerEnter private void
	 * @param : Collider
	 * When a player enters in this collider, we try to get the RoomBehaviour and the GateOpener associated to it. 
	 * At this point, different things can Occur
	 * If the room is not clean, that means, if it is the first time someone enters in it, then we, Initiate the room.
	 * If we get a GateOpener and if the Gate is not open, then we open the door. 
	 * Finally we destroy the gameObject.
	 **/
	private void OnTriggerEnter(Collider other)
	{
		roomBehaviour = GetComponentInParent<RoomBehaviour>();
		gateOpener = transform.parent.GetComponentInChildren<GateOpener>();
		if(other.tag == "Player")
		{
			if(roomBehaviour && !roomBehaviour.IsClean)
			{
				roomBehaviour.InitiateRoom();
			}
			
			if(gateOpener && !gateOpener.IsOpen)
			{
				gateOpener.OpenDoor();

                if (gameObject.tag == "EndRoom")
                {
                    GetComponentInParent<DungeonManager>().EndDungeon();
                } 
			}
			
			Destroy(gameObject);
		}
	}
}
