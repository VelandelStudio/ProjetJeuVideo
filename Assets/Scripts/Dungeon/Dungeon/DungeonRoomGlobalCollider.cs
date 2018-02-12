using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** DungeonRoomGlobalCollider, public class
 * This script is associated on the collider that encapsulate all the room. 
 * It is used to detect if players are inside a room.
 **/
public class DungeonRoomGlobalCollider : MonoBehaviour {
		
	RoomBehaviour roomBehaviour;
	
	/** OnTriggerEnter, private void
	 *@param Collider
	 * First at all, we get the roomBehaviour associated to the room.
	 * Then, we chck if this room is not clean and if it is, we ensure that all doors of the room are closed.
	 **/
	private void OnTriggerEnter(Collider other)
	{
		roomBehaviour = GetComponentInParent<RoomBehaviour>();

		if(other.tag == "Player" && !roomBehaviour.IsClean)
		{
			roomBehaviour.PlayerAreInside = true;
			GateOpener[] gateOpeners = transform.parent.GetComponentsInChildren<GateOpener>();
			
			for(int i = 0; i < gateOpeners.Length; i++)
			{
				if(gateOpeners[i].IsOpen)
				{
					gateOpeners[i].CloseDoor();
				}
			}
		}
	}
}
