using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** DungeonManager public class,
 * This is A Singleton class. We use this class two handles all elements that are inside the dungeon
 * This is used at anytime, how many time we passed insiode the dungeons, stats, room clean etc...
 **/
public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject _dungeonTimer;
    private MapGenerator _mapGenerator;

    private static DungeonManager instance;

    private List<RoomBehaviour> _roomList = new List<RoomBehaviour>();
    private float timerDungeon;
    private bool dungeonStarted;

    private GUIChallengePanelBehaviour _GUIChallengePanelBehaviour;
    /** ChallengeBonus, public int property
     * This property is used to get and set the bonus of the final chest.
     * This bonus is upgraded with the challenges succeeded.
     **/
    public int ChallengeBonus
    {
        get { return _challengeBonus; }
        private set { _challengeBonus = value; }
    }
    private int _challengeBonus;

    public DungeonManager Instance
    {
        get
        {
            return instance;
        }
    }

    /** Awake, private void Method 
     * Used to Destroy another Instance because of the Singleton pattern.
     **/
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /** Start, private void Method 
     * Used to generate the Map and teleport the player inside the dungeon when it is done.
     **/
    private void Start()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.GenerationMap();

        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = GameObject.FindWithTag("StartRoom").transform.position;
        _GUIChallengePanelBehaviour = GameObject.Find("Canvas").GetComponentInChildren<GUIChallengePanelBehaviour>(true);

        ChallengeBonus = 0;
        Debug.Log("ChallengeBonus = " + ChallengeBonus);
    }

    /** AttributeNewRoom, public void 
     * @param : RoomBehaviour
     * Called by roomBehaviours to add themselves to the room list
     **/
    public void AttributeNewRoom(RoomBehaviour newRoom)
    {
        _roomList.Add(newRoom);
    }

    /** StartDungeonTimer, public void
     * Launches the dungeon timer when the first door is opened
     **/
    public void StartDungeonTimer()
    {
        dungeonStarted = true;

        _dungeonTimer.SetActive(true);
    }

    /** EndDungeonTimer, public void
     * Ends the dungeon timer when the last door is opened
     **/
    public void EndDungeonTimer()
    {
        dungeonStarted = false;
    }

    /** Update, private void
     * We use the Update Method to update the Timer of the dungeon.
     **/
    private void Update()
    {
        if (dungeonStarted && _dungeonTimer)
        {
            timerDungeon += Time.deltaTime;
            _dungeonTimer.GetComponent<Text>().text = StringHelper.FormateFloatToClock(timerDungeon);
        }
    }

    /** AddChallengeBonus, public void
     * @param : int
     * This method is called each time a challenge is succeeded.
     * It adds x% bonus to the final chest.
     **/
    public void AddChallengeBonus(int bonus)
    {
        ChallengeBonus += bonus;
        Debug.Log("ChallengeBonus = " + ChallengeBonus);
    }

    public void SetRoomChallenge(string text, Sprite image)
    {
        _GUIChallengePanelBehaviour.SetRoomChallenge(text, image);
    }
}