using UnityEngine;
using UnityEngine.UI;

/** GUIChallengePanelBehaviour, public class
 * This class handles the behaviour of the GUI that displays the Challenges elements inside the dungeon.
 **/
public class GUIChallengePanelBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _challengeHandler;

    [SerializeField] private Image _dungeonChallengeImage;
    [SerializeField] private Text _dungeonChallengeText;
    [SerializeField] private Image _roomChallengeImage;
    [SerializeField] private Text _roomChallengeText;

    private float timerFade = 5.0f;
    private bool _menuActivatedByPlayer;

    /** UpdateChallenges, 
     * This method is used when a new challenge is launched.
     * It is just used to displays the gui before it fades.
     **/
    public void UpdateChallenges()
    {
        _challengeHandler.SetActive(true);
        timerFade = 0;
    }

    /** Update, private void method
     * This method is used two ways.
     * First at all we try to detect if the challenge is a fresh one. If it is the case, we start a timer.
     * When the timer reaches 0, the GUI fades. Then it detects if the player presses the abulation key. 
     * When the key is detected, the GUI appears on the screen and slowly fades untile the timer reaches 0 or the player presses Tab again.
     **/
    private void Update()
    {
        if (timerFade < 5.0f)
        {
            timerFade += 1.0f * Time.deltaTime;
        }
        else
        {
            if (_challengeHandler.activeSelf && !_menuActivatedByPlayer)
            {
                _challengeHandler.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _menuActivatedByPlayer = !_menuActivatedByPlayer;
            _challengeHandler.SetActive(_menuActivatedByPlayer);
        }
    }

    /** SetRoomChallenge, public void Method
     *@param : string, Sprite
     * This method is called by the room initilizer trigger. 
     * it is used to set a new challenge inside the dungeon room.
     **/
    public void SetRoomChallenge(string text, Sprite image)
    {
        _dungeonChallengeText.text = text;
        _dungeonChallengeImage.sprite = image;
        UpdateChallenges();
    }

    /** SetDungeonChallenge, public void Method
     *@param : string, Sprite
     * This method is called by the dungeonLauncher. 
     * It is used to set a new challenge inside the dungeon in general.
     **/
    public void SetDungeonChallenge(string text, Sprite image)
    {
        _roomChallengeText.text = text;
        _roomChallengeImage.sprite = image;
        UpdateChallenges();
    }
}
