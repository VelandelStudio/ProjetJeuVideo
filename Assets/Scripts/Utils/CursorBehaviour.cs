using UnityEngine;

/** CursorBehaviour Class
 * This Class is used by Unity to display and Hide the mouse Cursor when the Left Alt button is pressed. 
 * It Also ensures that the camera can not be moved when the player is using the mouse.
 * Please note that, at the begining of the game, the cursor is Locked (hidden).
 **/
public class CursorBehaviour : MonoBehaviour
{
    public static bool CursorIsVisible
    {
        get
        {
            return Cursor.lockState == CursorLockMode.None;
        }
        set { }
    }

    /** Start private void
	 * The method is used to set the cursor locked at the begining of the Game.
	 **/
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /** Start private void
	 * The method is used to set the cursor locked or unlocked when the player is in the game.
	 **/
    private void Update()
    {
        if (Input.GetKeyDown(InputsProperties.SwitchCursorState))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}