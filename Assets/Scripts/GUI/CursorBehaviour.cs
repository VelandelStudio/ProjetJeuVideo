using UnityEngine;
using UnityEngine.UI;

/** CursorBehaviour Class
 * This Class is used by Unity to display and Hide the mouse Cursor when the Left Alt button is pressed. 
 * It Also ensures that the camera can not be moved when the player is using the mouse.
 * Please note that, at the begining of the game, the cursor is Locked (hidden).
 **/
public class CursorBehaviour : MonoBehaviour
{
    private static GameObject _tooltip;

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
        _tooltip = GameObject.Find("TooltipStatus");
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

        if (_tooltip.activeSelf && Cursor.lockState == CursorLockMode.Locked)
        {
            CancelTooltip();
        }
    }

    public static void DisplayTooltip(string description)
    {
        _tooltip.GetComponent<Image>().enabled = true;
        _tooltip.GetComponentInChildren<Text>().text = description;
        _tooltip.transform.position = Input.mousePosition;
    }

    public static void CancelTooltip()
    {
        _tooltip.GetComponent<Image>().enabled = false;
        _tooltip.GetComponentInChildren<Text>().text = "";
    }
}