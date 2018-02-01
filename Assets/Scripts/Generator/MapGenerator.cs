using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

/** MapGenerator : Class
 *  @Inherits : Monobehavior
 *  This script is here to generate un dungeon whitch is sqarre or rectangle.
 **/
public class MapGenerator : MonoBehaviour {

    private string holderName = "Generated Dungeon";
    private Transform _dungeonHolder;
    private List<Coord> _BoardCoord = new List<Coord>();
    private List<Coord> _innerRooms = new List<Coord>();
    private Queue<Coord> _ShuffleBoardCoord;

    public Map map;
    public List<GameObject> rooms = new List<GameObject>();

    public Transform innerRoom;
    public Transform startRoom;
    public Transform endRoom;
    public Transform door;
    public Transform Wall;
    public Transform corridor;

    public int seed;

    #region methods

    private void Awake()
    {
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        _dungeonHolder = new GameObject(holderName).transform;
        _dungeonHolder.parent = transform;
    }

    /** generation map Method
     *  the idea is to place the rooms
     **/
    public void GenerationMap()
    {
        StoreAndSuffleBoardPositions();
        PlaceStartEnd();

        for (int x = 0; x < map.mapSize.x * 100 -100; x += 100)
        {
            for (int y = 0; y < map.mapSize.y * 100 -100; y += 100)
            {
                if ((x != 0 && x != map.mapSize.x) && (y != 0 && y != map.mapSize.y))
                {
                    PlacementRoom(x, y, innerRoom).parent = _dungeonHolder;
                }
            }
        }
    }

    /** placementRoom Method
     *  Here to place the good prefab at the good place
     **/
    private Transform PlacementRoom(int x, int y, Transform roomTransform)
    {
        Vector3 roomPosition = new Vector3(-(map.mapSize.x *100) / 2 + 100f + x, 0, -(map.mapSize.y *100) / 2 + 100f + y);
        Transform newRoom = Instantiate(roomTransform, roomPosition, Quaternion.identity) as Transform;

        return newRoom;
    }

    private void PlaceStartEnd()
    {
        Coord randomCoord = _ShuffleBoardCoord.Dequeue();
        PlacementRoom(randomCoord.x, randomCoord.y, startRoom).parent = _dungeonHolder;
        randomCoord = _ShuffleBoardCoord.Dequeue();
        PlacementRoom(randomCoord.x, randomCoord.y, endRoom).parent = _dungeonHolder;
    }

    private void StoreAndSuffleBoardPositions()
    {
        for (int x = 0; x < map.mapSize.x * 100; x += 100)
        {
            for (int y = 0; y < map.mapSize.y * 100; y += 100)
            {
                if ( IsBoarder(x, y) && !IsCorner(x, y))
                {
                    _BoardCoord.Add(new Coord(x, y));
                }
            }
        }

        _ShuffleBoardCoord= new Queue<Coord> (DungeonUtility.SuffleArray(_BoardCoord.ToArray(), seed));
    }

    private void StoreInnerPositions()
    {
        for (int x = 0; x < map.mapSize.x * 100; x += 100)
        {
            for (int y = 0; y < map.mapSize.y * 100; y += 100)
            {
                if ( (x != 0 && x != map.mapSize.x) && (x != 0 && x != map.mapSize.x)) 
                {
                    _innerRooms.Add(new Coord(x, y));
                }
            }
        }
    }

    private bool IsBoarder(int x, int y)
    {
        return (x == 0 || x == map.mapSize.x * 100 || y == 0 || y == map.mapSize.y * 100);
    }

    private bool IsCorner(int x, int y)
    {
        return (x == 0 && y == 0) || (x == 0 && y == map.mapSize.y * 100) ||
               (x == map.mapSize.x * 100 && y == 0) || (x == map.mapSize.x * 100 && y == map.mapSize.y * 100);
    }

    #endregion methods

    #region innerClassAndStruct

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord (int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    [System.Serializable]
    public class Map
    {
        public Coord mapSize;
    }

    #endregion innerClassAndStruct

}