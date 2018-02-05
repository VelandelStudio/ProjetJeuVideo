using UnityEngine;

/** RoomBase, public abstract class
 * Mother class of all RoomBehaviours inside the Dungeon. 
 * This class contains property used to know if a room is linked to another and by which side.
 **/
public abstract class RoomBase : MonoBehaviour {

    public Transform RoomNorth;
    public Transform RoomSouth;
    public Transform RoomEast;
    public Transform RoomWest;
}