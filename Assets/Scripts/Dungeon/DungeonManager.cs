using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
 {
    [SerializeField] private Text _dungeonTimer;
    private MapGenerator _mapGenerator;

	private static DungeonManager instance;
	public DungeonManager Instance
	{
		get 
		{
			return instance;
		}
	}

	private void Awake()
	{
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

    private void Start()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.GenerationMap();
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = GameObject.FindWithTag("StartRoom").transform.position;
    }

    private List<RoomBehaviour> _roomList = new List<RoomBehaviour>();
	private float timerDungeon;
	private bool dungeonStarted;
	
	public void AttributeNewRoom(RoomBehaviour newRoom)
	{
		_roomList.Add(newRoom);
	}
	
	public void StartDungeon()
	{
		dungeonStarted = true;

        _dungeonTimer.gameObject.SetActive(true);
    }
	
	public void EndDungeon()
	{
		dungeonStarted = false;
	}
		
	private void Update()
	{
		if(dungeonStarted && _dungeonTimer)
		{
			timerDungeon += Time.deltaTime;
            _dungeonTimer.text = StringHelper.FormateFloatToClock(timerDungeon);
        }
	}
}