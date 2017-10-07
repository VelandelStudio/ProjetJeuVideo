using UnityEngine;

/** PosHelper static Class
 * This Static class should be used to add methods that helps us to get some important positions.
 **/
public static class PosHelper
{

    /** GetOriginOfDetector static Vector3 Method
	 * This Static Method is used to get the OriginPoint of the GameObjectDetector in front of the player.
	 * It is called for exemple, to launch spell projectiles.
	 **/
    public static Vector3 GetOriginOfDetector(Transform player)
    {
        return player.gameObject.GetComponentInChildren<Camera>().GetComponent<GameObjectDetector>().OriginPoint;
    }
}