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
    private int _maxRand = 5;
    private Transform _dungeonHolder;
    private List<Coord> _BoardCoord = new List<Coord>();
    private Queue<Coord> _ShuffleBoardCoord;

    public Map map;
    public Transform[,] roomTable;

    public Transform innerRoom;
    public Transform startRoom;
    public Transform endRoom;
    public Transform door;
    public Transform wall;
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

        for (int y = map.MinInnerY; y <= map.MaxInnerY; y += 100)
        {
            for (int x = map.MinInnerX; x <= map.MaxInnerX; x += 100)
            {
                Transform newRoom = PlacementRoom(x, y, innerRoom);
                newRoom.parent = _dungeonHolder;
                roomTable[x / 100 - 1, y / 100 - 1] = newRoom;
            }
        }

        BuildShortestPath(startRoom, endRoom);

        for (int i = 0; i < roomTable.GetLength(0); i++)
        {
            for (int j = 0; j < roomTable.GetLength(1); j++)
            {
                RoomBehaviour roomBehaviour = roomTable[i, j].GetComponent<RoomBehaviour>();

                if (roomBehaviour.RoomSelectedByShortpath && !roomBehaviour.RoomSelectedByExternalAdd)
                {
                    BuildExternalRoom(i, j, roomTable[i, j], _maxRand);
                }
            }
        }

        ClearUnselectedRooms();
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
        Coord randomCoordStart = _ShuffleBoardCoord.Dequeue();
        Coord randomCoordEnd = _ShuffleBoardCoord.Dequeue();

        while (Mathf.Abs(randomCoordStart.x / 100 - randomCoordEnd.x / 100) < 4
        || Mathf.Abs(randomCoordStart.y / 100 - randomCoordEnd.y / 100) < 4)
        {
            randomCoordEnd = _ShuffleBoardCoord.Dequeue();
        }

        startRoom = PlacementRoom(randomCoordStart.x, randomCoordStart.y, startRoom);
        startRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(startRoom);

        endRoom = PlacementRoom(randomCoordEnd.x, randomCoordEnd.y, endRoom);
        endRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(endRoom);
    }

    private void SetRotationBoarderRooms(Transform boarderRoom)
    {
        Quaternion rotation = Quaternion.identity;
        if (boarderRoom.position.x == map.MinBoarderX)
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

        _ShuffleBoardCoord = new Queue<Coord>(DungeonUtility.SuffleArray(_BoardCoord.ToArray(), seed));
    }

    private void BuildShortestPath(Transform roomA, Transform roomB)
    {

        int HorizontalDist = (int)(roomA.position.x / 100 - roomB.position.x / 100);
        int VerticalDist = (int)(roomA.position.z / 100 - roomB.position.z / 100);
        int rand;

        Transform nextRoomA = roomA;
        Transform nextRoomB = roomB;

        if (roomA.transform.position == roomB.transform.position)
        {
            return;
        }

        if (IsBoarder((int)roomA.position.x, (int)roomA.position.z) && IsBoarder((int)roomB.position.x, (int)roomB.position.z))
        {
            nextRoomA = GetAdjacentInnerRoom(roomA);
            nextRoomB = GetAdjacentInnerRoom(roomB);
        }
        else
        {
            if (Mathf.Abs(HorizontalDist) > 0 && Mathf.Abs(VerticalDist) > 0)
            {
                rand = Random.Range(1, 3);
            }
            else
            {
                rand = Mathf.Abs(HorizontalDist) > 0 ? 1 : 2;
            }

            if (rand == 1)
            {
                int modulo = roomA.position.x < roomB.position.x ? 1 : -1;
                nextRoomA = roomTable[CenterOnGrid(roomA.position.x) + modulo, CenterOnGrid(roomA.position.z)];
                AddCorridor(roomA, new Vector3(0, modulo * 90.0f, 0), new Vector3(50f * modulo, 0, 0));
                LinkTwoRooms(roomA, nextRoomA);
            }
            else
            {
                int modulo = roomA.position.z < roomB.position.z ? 1 : -1;
                nextRoomA = roomTable[CenterOnGrid(roomA.position.x), CenterOnGrid(roomA.position.z) + modulo];
                AddCorridor(roomA, new Vector3(0, modulo * 180.0f, 0), new Vector3(0, 0, 50f * modulo));
                LinkTwoRooms(roomA, nextRoomA);
            }
        }

        nextRoomA.GetComponent<RoomBehaviour>().RoomSelectedByShortpath = true;
        nextRoomB.GetComponent<RoomBehaviour>().RoomSelectedByShortpath = true;

        BuildShortestPath(nextRoomA, nextRoomB);
    }

    private void BuildExternalRoom(int i, int j, Transform roomSelected, int maxRand)
    {
        int rand = 0;
        RoomBehaviour selectRoomBehaviour = roomSelected.GetComponent<RoomBehaviour>();
        for (int modulo = -1; modulo <= 1; modulo += 2)
        {
            if ((i > 0 && modulo == -1) || (i < roomTable.GetLength(0) - 1 && modulo == 1))
            {
                if (!roomTable[i + modulo, j].GetComponent<RoomBehaviour>().RoomSelectedByShortpath
                && !roomTable[i + modulo, j].GetComponent<RoomBehaviour>().RoomSelectedByExternalAdd)
                {
                    rand = Random.Range(1, maxRand);

                    if (rand == 1)
                    {
                        roomTable[i + modulo, j].GetComponent<RoomBehaviour>().RoomSelectedByExternalAdd = true;
                        LinkTwoRooms(roomSelected, roomTable[i + modulo, j]);
                        BuildExternalRoom(i + modulo, j, roomTable[i + modulo, j], maxRand + 1);
                        AddCorridor(roomSelected, new Vector3(0, modulo * 90f, 0), new Vector3(50f * modulo, 0, 0));
                    }
                }
            }

            if ((j > 0 && modulo == -1) || (j < roomTable.GetLength(1) - 1 && modulo == 1))
            {
                if (!roomTable[i, j + modulo].GetComponent<RoomBehaviour>().RoomSelectedByShortpath
                    && !roomTable[i, j + modulo].GetComponent<RoomBehaviour>().RoomSelectedByExternalAdd)
                {
                    rand = Random.Range(1, maxRand);

                    if (rand == 1)
                    {
                        roomTable[i, j + modulo].GetComponent<RoomBehaviour>().RoomSelectedByExternalAdd = true;
                        LinkTwoRooms(roomSelected, roomTable[i, j + modulo]);
                        BuildExternalRoom(i, j + modulo, roomTable[i, j + modulo], maxRand + 1);
                        AddCorridor(roomSelected, new Vector3(0, modulo * 180f, 0), new Vector3(0, 0, 50f * modulo));
                    }
                }
            }
        }
        BuildDoorsAndWalls(roomSelected);
    }

    private void LinkTwoRooms(Transform roomA, Transform roomB)
    {
        RoomBase roomBehaviourA = roomA.GetComponent<RoomBase>();
        RoomBase roomBehaviourB = roomB.GetComponent<RoomBase>();

        float horizontalDiff = roomA.transform.position.x - roomB.transform.position.x;
        float verticalDiff = roomA.transform.position.z - roomB.transform.position.z;

        if (horizontalDiff != 0)
        {
            roomBehaviourA.RoomWest = horizontalDiff == 100 ? roomB : roomBehaviourA.RoomWest;
            roomBehaviourA.RoomEast = horizontalDiff == -100 ? roomB : roomBehaviourA.RoomEast;

            roomBehaviourB.RoomEast = horizontalDiff == 100 ? roomA : roomBehaviourB.RoomEast;
            roomBehaviourB.RoomWest = horizontalDiff == -100 ? roomA : roomBehaviourB.RoomWest;
        }

        if (verticalDiff != 0)
        {
            roomBehaviourA.RoomSouth = verticalDiff == 100 ? roomB : roomBehaviourA.RoomSouth;
            roomBehaviourA.RoomNorth = verticalDiff == -100 ? roomB : roomBehaviourA.RoomNorth;

            roomBehaviourB.RoomNorth = verticalDiff == 100 ? roomA : roomBehaviourB.RoomNorth;
            roomBehaviourB.RoomSouth = verticalDiff == -100 ? roomA : roomBehaviourB.RoomSouth;
        }
    }

    private void BuildDoorsAndWalls(Transform room)
    {
        RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();
        Transform objInstance;

        objInstance = roomBehaviour.RoomNorth == null ? wall : door;
        BuildLateralObjects(objInstance, room, new Vector3(0, 0, 0), new Vector3(0, 0, 25));

        objInstance = roomBehaviour.RoomSouth == null ? wall : door;
        BuildLateralObjects(objInstance, room, new Vector3(0, 180, 0), new Vector3(0, 0, -25));

        objInstance = roomBehaviour.RoomEast == null ? wall : door;
        BuildLateralObjects(objInstance, room, new Vector3(0, 90, 0), new Vector3(25, 0, 0));

        objInstance = roomBehaviour.RoomWest == null ? wall : door;
        BuildLateralObjects(objInstance, room, new Vector3(0, -90, 0), new Vector3(-25, 0, 0));
    }

    private void BuildLateralObjects(Transform obj, Transform parent, Vector3 eulerRotation, Vector3 addedPosition)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = eulerRotation;
        Transform newObj = Instantiate(obj, parent.position + addedPosition, rotation) as Transform;
        newObj.position += obj.position;
        newObj.parent = parent;
    }

    private void AddCorridor(Transform parent, Vector3 eulerRotation, Vector3 addedPosition)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = eulerRotation;
        Transform newCorridor = Instantiate(corridor, parent.position + addedPosition, rotation) as Transform;
        newCorridor.parent = parent;
    }

    private Transform GetAdjacentInnerRoom(Transform boarderRoom)
    {
        int moduloX = 0;
        int moduloZ = 0;
        Vector3 rotationNextDoor = Vector3.zero; 
        if (boarderRoom.position.x == map.MinBoarderX || boarderRoom.position.x == map.MaxBoarderX)
        {
            moduloX = boarderRoom.position.x == map.MinBoarderX ? 1 : -1;
        }
        if (boarderRoom.position.z == map.MinBoarderY || boarderRoom.position.z == map.MaxBoarderY)
        {
            moduloZ = boarderRoom.position.z == map.MinBoarderY ? 1 : -1;
        }

        Transform nextRoom = roomTable[CenterOnGrid(boarderRoom.position.x) + moduloX, CenterOnGrid(boarderRoom.position.z) + moduloZ];
        LinkTwoRooms(boarderRoom, nextRoom);

        return nextRoom;
    }

    private void ClearUnselectedRooms()
    {
        for (int i = 0; i < roomTable.GetLength(0); i++)
        {
            for (int j = 0; j < roomTable.GetLength(1); j++)
            {
                RoomBehaviour roomBehaviour = roomTable[i, j].GetComponent<RoomBehaviour>();
                if (!roomBehaviour.RoomSelectedByShortpath && !roomBehaviour.RoomSelectedByExternalAdd)
                {
                    Destroy(roomBehaviour.gameObject);
                }
            }
        }
    }

    private int CenterOnGrid(float position)
    {
        return (int)position / 100 - 1;
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