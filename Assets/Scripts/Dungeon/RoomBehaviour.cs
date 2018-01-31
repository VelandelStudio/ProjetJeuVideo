using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public List<GameObject> monster = new List<GameObject>();

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

	private void Start()
    {
        GetComponentInParent<DungeonManager>().AttributeNewRoom(this);
		Theme theme = new Theme();
		Debug.Log(theme.Value);
    }
	
    public void InitiateRoom()
    {
        InitiateMonster();
        StoreLevers();
    }

    private void InitiateMonster()
    {
        Debug.Log("Hello");
        Vector3 incVector = transform.position + Vector3.up * 2f;
		for (int i = 0; i < monster.Count; i++)
		{
			monster[i] = Instantiate(monster[i], transform.position + incVector, transform.rotation);
		}
    }

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
