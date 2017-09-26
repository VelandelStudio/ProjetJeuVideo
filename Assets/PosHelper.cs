using UnityEngine;

public static class PosHelper {

	public static Vector3 GetOriginOfDetector(Transform player) {
	
		return player.gameObject.GetComponentInChildren<Camera>().GetComponent<GameObjectDetector>().OriginPoint;
	}
}
