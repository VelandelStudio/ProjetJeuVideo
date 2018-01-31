using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
 {
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
	}
	
	public void EndDungeon()
	{
		dungeonStarted = false;
	}
		
	private void Update()
	{
		if(dungeonStarted)
		{
			timerDungeon += Time.deltaTime;
			Debug.Log(StringHelper.SecToMinConverter(timerDungeon));
		}
	}
}