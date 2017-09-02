using UnityEngine;

public class CursorBehaviour : MonoBehaviour {
	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	private void Update() {
		
		if(Input.GetKeyDown(KeyCode.LeftAlt))
			Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;	
	}
}
