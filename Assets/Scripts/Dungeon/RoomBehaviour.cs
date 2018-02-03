using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RoomBehaviour Class
/// This script define the cycle of life of a room in the dungeon.
/// </summary>
public class RoomBehaviour : MonoBehaviour {

    public List<GameObject> monster = new List<GameObject>();
    public bool RoomSelectedByShortpath = false;
    public bool RoomSelectedByExternalAdd = false;

    private GateOpener[] _insideLevers;
    private bool _playerAreInside;

    public bool PlayerAreInside
	{
		get {return _playerAreInside;}
		set{_playerAreInside = value;}
	}
	
	public bool IsClean
	{
		get {return monster.Count == 0;}
		protected set{}
	}

    /// <summary>
    /// Start Method
    /// Contact the Dungeon manager and attribute a theme to itself
    /// </summary>
	private void Start()
    {
        GetComponentInParent<DungeonManager>().AttributeNewRoom(this);
		Theme theme = new Theme();
		Debug.Log(theme.Value);
    }

    /// <summary>
    /// InitiateRoom method
    /// This method is called by the InitializerRoomTrigger
    /// Call methods to fullfill the room
    /// Get the levers to say when it can activate it !
    /// </summary>
    public void InitiateRoom()
    {
        InitiateMonster();
        StoreLevers();
    }

    /// <summary>
    /// InitiateMonster method
    /// this method is called by the InitiateRoom
    /// place the monster inside the room.
    /// </summary>
    private void InitiateMonster()
    {
        Debug.Log("Hello");
        Vector3 incVector = transform.position + Vector3.up * 2f;
		for (int i = 0; i < monster.Count; i++)
		{
			monster[i] = Instantiate(monster[i], transform.position + incVector, transform.rotation);
		}
    }

    /// <summary>
    /// StoreLevers method
    /// called by the InitiateRoom method
    /// get all the levers scripts inside the the room.
    /// </summary>
    private void StoreLevers()
    {
		_insideLevers = GetComponentsInChildren<GateOpener>();
    }
	
	/** Update, private void method
	 * The methods check if the Monster array has dead monsters inside. If that's the case, we remove all instances that are dead.
	 **/
	private void Update()
	{
		monster.RemoveAll(Monster => Monster.GetComponent<EntityLivingBase>().IsDead);
	}
	
	/** Theme, private inner class
	 * This class contains static Theme instances that corresponds to all themes that a room can take.
	 * When a Theme instance is created, we rand a Theme inside all Themes possibilities and returns it.
	 **/
	[System.Serializable]
	private class Theme
	{
		public Theme() 
		{ 
			RandTheme();
		}
		
		private Theme(string value) { Value = value; }
		public string Value { get; set; }

		public static Theme Fire { get { return new Theme("Fire"); } }
		public static Theme Water { get { return new Theme("Water"); } }
		public static Theme Nature { get { return new Theme("Nature"); } }
		public static Theme Storm { get { return new Theme("Storm"); } }
		
		public void RandTheme()
		{
			Theme[] themes = new Theme[]{Fire, Water, Nature, Storm};
			Value = themes[Random.Range(0,themes.Length-1)].Value;
		}
	}
}
