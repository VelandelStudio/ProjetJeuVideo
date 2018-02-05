using System.Collections.Generic;
using UnityEngine;

/** RoomBehaviour, public Class
 * @extends : RoomBase
 * This script shoudl be associated to every inner rooms inside  Dungeon.
 * We handle here to behaviour and the life cycle of a room.
 **/
public class RoomBehaviour : RoomBase
{

    public List<GameObject> monster = new List<GameObject>();
    public bool RoomSelectedByShortpath = false;
    public bool RoomSelectedByExternalAdd = false;

    private bool _playerAreInside;

    /** PlayerAreInside, public bool Property
	 * We use this property to tell if players are inside this room or not.
	 * This is set by the RoomCollider so far.
	 **/
    public bool PlayerAreInside
    {
        get { return _playerAreInside; }
        set { _playerAreInside = value; }
    }

    /** IsClean, public bool Property
	 * We use this property to tell if a room was already clean or not.
	 * We consider that a room is clean if there are no monsters inside anymore.
	 **/
    public bool IsClean
    {
        get { return monster.Count == 0; }
        protected set { }
    }

    /// <summary>
    /// Start Method
    /// Contact the Dungeon manager and attribute a theme to itself
    /// </summary>
	private void Start()
    {
        GetComponentInParent<DungeonManager>().AttributeNewRoom(this);
        Theme theme = new Theme();
        InitiateMonster();
    }

    /// <summary>
    /// InitiateRoom method
    /// This method is called by the InitializerRoomTrigger
    /// Call methods to fullfill the room
    /// Get the levers to say when it can activate it !
    /// </summary>
    public void InitiateRoom()
    {
        for (int i = 0; i < monster.Count; i++)
        {
            monster[i].SetActive(true);
        }
    }

    /// <summary>
    /// InitiateMonster method
    /// this method is called by the InitiateRoom
    /// place the monster inside the room.
    /// </summary>
    private void InitiateMonster()
    {
        for (int i = 0; i < monster.Count; i++)
        {
            monster[i] = Instantiate(monster[i], transform.position + Vector3.up * 2f, transform.rotation);
            monster[i].transform.parent = transform;
            monster[i].SetActive(false);
        }
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
            Theme[] themes = new Theme[] { Fire, Water, Nature, Storm };
            Value = themes[Random.Range(0, themes.Length)].Value;
        }
    }
}