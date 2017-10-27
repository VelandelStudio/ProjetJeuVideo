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
        return player.position + new Vector3(0f, player.GetComponent<CharacterController>().height * player.localScale.y, 0f);
    }
	
	
	/** GetRightHandTransformOfPlayer static Transform Method
	 * This Static Method is used to get the right hand Transform of the player. 
	 * It is usually used to launch spells.
	 **/
	public static Transform GetRightHandTransformOfPlayer(Transform player)
    {
		Transform[] transformTab = player.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tr in transformTab)
        {
            if (tr.gameObject.name == "mixamorig:RightHand")
            {
                return tr;
            }
        }
		
		return null;
	}
}