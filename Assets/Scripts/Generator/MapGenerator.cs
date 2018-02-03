using System;
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
    public Transform[,] roomTable;

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
        seed = (int)DateTime.Now.Ticks;
        map.Initialize();
        roomTable = new Transform[map.mapSize.x, map.mapSize.y];
        StoreAndSuffleBoardPositions();
        PlaceStartEnd();

        for (int x = map.MinInnerX; x <= map.MaxInnerX; x += 100)
        {
            for (int y = map.MinInnerY; y <= map.MaxInnerX; y += 100)
            {
                Transform newRoom = PlacementRoom(x, y, innerRoom);
                newRoom.parent = _dungeonHolder;
                roomTable[x / 100 - 1, y / 100 - 1] = newRoom;
            }
        }

        GetShortestDistance(startRoom, endRoom);
    }

    /** placementRoom Method
     *  Here to place the good prefab at the good place
     **/
    private Transform PlacementRoom(int x, int y, Transform roomTransform)
    {
        Vector3 roomPosition = new Vector3(x, 0, y);
        Transform newRoom = Instantiate(roomTransform, roomPosition, Quaternion.identity) as Transform;

        return newRoom;
    }

    private void PlaceStartEnd()
    {
        Coord randomCoord = _ShuffleBoardCoord.Dequeue();
        //startRoom = PlacementRoom(randomCoord.x, randomCoord.y, startRoom);
        startRoom = PlacementRoom(0, 200, startRoom);
        startRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(startRoom);

        randomCoord = _ShuffleBoardCoord.Dequeue();
        //endRoom = PlacementRoom(randomCoord.x, randomCoord.y, endRoom);
        endRoom = PlacementRoom(600, 200, endRoom);
        endRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(endRoom);
    }

    private void SetRotationBoarderRooms(Transform boarderRoom)
    {
        Quaternion rotation = Quaternion.identity;
        if(boarderRoom.position.x == map.MinBoarderX)
        {
            rotation.eulerAngles = new Vector3(0, 90, 0);
        }

        if (boarderRoom.position.x == map.MaxBoarderX)
        {
            rotation.eulerAngles = new Vector3(0, -90, 0);
        }

        if (boarderRoom.position.z == map.MaxBoarderY)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0);
        }
        boarderRoom.transform.rotation = rotation;
    }

    private void StoreAndSuffleBoardPositions()
    {
        for (int x = map.MinBoarderX; x <= map.MaxBoarderX; x += 100)
        {
            for (int y = map.MinBoarderY; y <= map.MaxBoarderY; y += 100)
            {
                if (IsBoarder(x, y) && !IsCorner(x, y))
                {
                    _BoardCoord.Add(new Coord(x, y));
                }
            }
        }

        _ShuffleBoardCoord= new Queue<Coord> (DungeonUtility.SuffleArray(_BoardCoord.ToArray(), seed));
    }

    private void StoreInnerPositions()
    {
        for (int x = map.MinInnerX; x <= map.MaxInnerX; x += 100)
        {
            for (int y = map.MinInnerY; y <= map.MaxInnerY; y += 100)
            {
                _innerRooms.Add(new Coord(x, y));
            }
        }
    }

    private void GetShortestDistance(Transform roomA, Transform roomB)
    {
        int HorizontalDist = (int)(roomA.position.x / 100 - roomB.position.x / 100);
        int VerticalDist = (int)(roomA.position.z / 100 - roomB.position.z / 100);

        int rand;
        Transform nextRoom = roomA;

        if (Mathf.Abs(HorizontalDist) == 0 && Math.Abs(VerticalDist) == 0)
        {
            return;
        }

        if(Mathf.Abs(HorizontalDist) > 0 && Mathf.Abs(VerticalDist) > 0)
        {
            rand = Random.Range(1, 2);
        }
        else
        {
            rand = Mathf.Abs(HorizontalDist) > 0 ? 1:2;
        }

        if(rand == 1)
        {
            if (roomA.position.x == map.MinBoarderX)
            {
                nextRoom = roomTable[0, (int)roomA.position.z / 100 -1];
            }

            if(roomA.position.x == map.MaxBoarderX)
            {
                nextRoom = roomTable[((int)roomA.position.x / 100)-1, (int)roomA.position.z / 100];
            }

            if(roomA.position.x != map.MinBoarderX && roomA.position.x != map.MaxBoarderX)
            {
                int modulo = HorizontalDist > 0 ? 1 : -1;
                Debug.Log(VerticalDist);
                nextRoom = roomTable[((int)roomA.position.x/100)+modulo, (int)roomA.position.z / 100];
            }
        }
        else
        {

        }
        nextRoom.GetComponent<RoomBehaviour>().RoomSelected = true;
        GetShortestDistance(nextRoom,roomB);
    }

    private bool IsBoarder(int x, int y)
    {
        return (x == map.MinBoarderX || x == map.MaxBoarderX || y == map.MinBoarderY || y == map.MaxBoarderY);
    }

    private bool IsCorner(int x, int y)
    {
        return (x == map.MinBoarderX && y == map.MinBoarderY) || (x == map.MinBoarderX && y == map.MaxBoarderY) ||
               (x == map.MaxBoarderX && y == map.MinBoarderY) || (x == map.MaxBoarderX && y == map.MaxBoarderX);
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

        public int MinInnerX = 100;
        public int MinInnerY = 100;
        public int MaxInnerX;
        public int MaxInnerY;

        public int MinBoarderX = 0;
        public int MinBoarderY = 0;
        public int MaxBoarderX;
        public int MaxBoarderY;

        public void Initialize()
        {
            MaxInnerX = mapSize.x * 100;
            MaxInnerY = mapSize.y * 100;
            MaxBoarderX = MaxInnerX + 100;
            MaxBoarderY = MaxInnerY + 100;
        }
    }

    #endregion innerClassAndStruct

}