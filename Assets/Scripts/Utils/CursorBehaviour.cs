﻿using UnityEngine;

/** CursorBehaviour Class
 * This Class is used by Unity to display and Hide the mouse Cursor when the Left Alt button is pressed. 
 * It Also ensures that the camera can not be moved when the player is using the mouse.
 * Please note that, at the begining of the game, the cursor is Locked (hidden).
 **/
public class CursorBehaviour : MonoBehaviour
{

    private CameraController playerCameraController;

    /** Start private void
	 * The method is used to set the cursor locked at the begining of the Game.
	 * It also gets the the CameraController of the player.
	 **/
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCameraController = GetComponentInChildren<Camera>().GetComponent<CameraController>();
        playerCameraController.CameraControlled = true;
    }

    /** Start private void
	 * The method is used to set the cursor locked or unlocked when the player is in the game.
	 * It also lock or unlock the player Camera to make an easy use of GUI elements.
	 **/
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
            playerCameraController.CameraControlled = !playerCameraController.CameraControlled;
        }
    }
}
