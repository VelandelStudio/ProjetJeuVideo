using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

/** MapGenerator : public Class
 * This class is used to build a dungeon from scratch.
 * Note that building a dungeon can take time because we are parsing a lot of elements. We are going in this class to 
 * build a main path and external rooms randomly. Set up a Start and an End room.
 *  After that we are going to place every elements attached to the room such as Doors, Walls and Corridors.
 **/
public class MapGenerator : MonoBehaviour
{

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

    /** Awake private void;
	 * On Aake, we try to check if another Dungeon already exists. If that's the case, we Destroy it and load a new instance of the DungeonHolder.
	 * The DungeonHolder is a gameObject that holds every rooms of the dungeon. We use this intermediate GameObject between the Dungeon and Rooms to make
	 * the creation and the Destruction of the gameObject easier.
	 **/
    private void Awake()
    {
        if (transform.Find(holderName))
        {
            Destroy(transform.Find(holderName).gameObject);
        }

        _dungeonHolder = new GameObject(holderName).transform;
        _dungeonHolder.parent = transform;
    }

    /** GenerationMap, public void Method
     * This Method is launched by the DungeonManager. It is used to create all the dungeon.
	 * The main goals of building a dungeon are : 
	 * First : Create a grid of Coords that will represents a room table
	 * Second : Place one StartRoom and one EndRoom randomly on the boarder of the map and attribute them the correct rotation.
	 * Third : Generate empty rooms (only floors), on the scene that fits with the grid created at the first step.
	 * Fourth : Build a random path between the StartRoom and the EndRoom
	 * Fifth : Add random ExternalRooms that are not on the main path from start to end, but add complexity to the dungeon
     **/
    public void GenerationMap()
    {
        seed = (int)DateTime.Now.Ticks;
        map.Initialize();
        roomTable = new Transform[map.mapSize.x, map.mapSize.y];

        StoreAndSuffleBoardPositions();
        PlaceStartEnd();
        PlaceAllRoomsOnGrid();

        BuildShortestPath(startRoom, endRoom);
        BuildAllExternalRooms();

        ClearUnselectedRooms();
    }

    /** StoreAndSuffleBoardPositions, private void Method
	 * Here, we construct a list of boarder positions available to place a Start and an end room.
	 * When the List is built, we create a Queue that will be shuffle to always have a different configuration.
	 **/
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

    /** PlaceStartEnd, private void Method
	 * This method is used to set up a Start and an end room randomly.
	 * First of all we Attribute new coords (obtained by "EVERYDAY I'M SHUFFLING") to a new Start and EndRoom
	 * We do that whil the distance (Horizontal and Vertical) is less that 4. That ensure that the dungeon will contain, at least 8 rooms.
	 * Once it is done, we place the rooms in the Space world and Set the rotation to them.
	 **/
    private void PlaceStartEnd()
    {
        Coord randomCoordStart = _ShuffleBoardCoord.Dequeue();
        Coord randomCoordEnd = _ShuffleBoardCoord.Dequeue();

        while (Mathf.Abs(GetDistanceOnGrid(randomCoordStart.x, randomCoordEnd.x)) < 4 || Mathf.Abs(GetDistanceOnGrid(randomCoordStart.y, randomCoordEnd.y)) < 4)
        {
            randomCoordEnd = _ShuffleBoardCoord.Dequeue();
        }

        startRoom = RoomPlacement(randomCoordStart.x, randomCoordStart.y, startRoom);
        startRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(startRoom);

        endRoom = RoomPlacement(randomCoordEnd.x, randomCoordEnd.y, endRoom);
        endRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(endRoom);
    }

    /** PlaceAllRoomsOnGrid, private void Method
	 * This method is used to parse the InnerMap coords and place a new empty room on each coord.
	 * At each step, we also attribute the transform of the fresh room to the RoomTable.
	 **/
    private void PlaceAllRoomsOnGrid()
    {
        for (int x = map.MinInnerX; x <= map.MaxInnerX; x += 100)
        {
            for (int y = map.MinInnerY; y <= map.MaxInnerY; y += 100)
            {
                Transform newRoom = RoomPlacement(x, y, innerRoom);
                newRoom.parent = _dungeonHolder;
                roomTable[CenterOnGrid(x), CenterOnGrid(y)] = newRoom;
            }
        }
    }

    /** SetRotationBoarderRooms, private void Method
	 * This Method is used to set the Correct rotation of Start and End Rooms.
	 * We check on which side we are on the boarder and we rotate the room in the correct way.
	 **/
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

    /** BuildShortestPath, private void RECURSIVE Method
	 * @param : Transform, Transform
	 * This mehod is used to create the whortest path possible between the Start and End Room.
	 * Every time we go inside this method, we check if the Two rooms passed as parameters is the same instance of the room (i.e. you are checking the same room).
	 * If it is the first time we get into this method (i.e. if we are checking the Start and EndRoom), the we are going to find the nearest inner room.
	 * The nearest inner rooms will become the new start and end and we recall this method.
	 * If we are not on a border (Second time in the method or more), then we try to get the Horizontal and Vertical Distance. 
	 * If both of the distance are > 0 then we rand on which side we are going to work, else we work on the only distance which is > 0 
	 * (Remember that if Distances = 0 you are checking the same room so the method already returned).
	 * No matter on which side we are working, we are going to get the next adjacente room. Once done, we create a Corridor between the two rooms.
	 * The next RoomA will be markeded as RoomSelectedByShortpath in the Monobehaviour.
	 * We recall the method with the new RoomA and RoomB 
	 * Please note that the nextRoomB is only modified in the first occurence of the method (when we check the nearest inner room of EndRoom).
	 **/
    private void BuildShortestPath(Transform roomA, Transform roomB)
    {
        if (roomA.transform.position == roomB.transform.position)
        {
            return;
        }

        int HorizontalDist = GetDistanceOnGrid(roomA.position.x, roomB.position.x);
        int VerticalDist = GetDistanceOnGrid(roomA.position.z, roomB.position.z);
        int rand;

        Transform nextRoomA = roomA;
        Transform nextRoomB = roomB;

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

    /** BuildAllExternalRooms, private void
	 * This method is used to parse the roomTable and get their RoomBehaviour.
	 * If the room was selected by the shortest path method but not already selected then we try to build an external adjacente room
	 **/
    private void BuildAllExternalRooms()
    {
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
    }

    /** BuildExternalRoom, private void RECURSIVE Method
	 * @param : int, int, Transform, int
	 * This method acts as the BuildShortestPath method. For the room selected passed as parameter, 
	 * we are going to check on each side if we find a room that is not alread selected by the short path and not selected by the external path.
	 * If we find that room, then we rand from [0,maxRand+1]. As we are recursive, you can note that the MaxRand value take +1 at each time, meaning building External rooms becomes very harder each time.
	 * Each time we construct an external room, we recall this method.
	 * If no external room were constructed then we return.
	 * Please note that, for each room, if we build an adjacente room, then we add a Corridor between the two rooms.
	 * At the end of the Method, we will call the BuildDoorsAndWalls on the room we are working on.
	 *
	 * The room built here ar also called PrestatairesRooms. They can not park in the proBTP parking and calso have to pay 
	 * the shitty food of the cantine two euros plus cher than fucking internes. This is bullshit and this is why they are working so hard in the P P PJV !
	 **/
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

    /** GetAdjacentInnerRoom, private Transform Method
	 * @param : Transform
	 * This method is used to get the Adjacente inner rooms from a board room (Start or End) and link them.
	 * We check if the link is Left-Right or Up-Down and we link the two rooms before returning the Adjacent room to the Start/EndRoom
	 **/
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

    /** ClearUnselectedRooms, private void
	 * We are going to parse all the RoomTable and check if rooms are selected or not.
	 * If theyr are not, we Destroy them.
	 **/
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

    /** LinkTwoRooms, private void
	 * @param: Transform, Transform
	 * This method is used to Link two rooms with their RoomBehaviour.
	 * Fist at all, we Get the RoomBehaviours of the two rooms passed and we calculate the Distance between them. 
	 * Note that you should always call this method with two adjacente rooms.
	 * Then, we check if the link is Left-Right or Up-Down (with the Distance value) and attribute the room to each other
	 **/
    private void LinkTwoRooms(Transform roomA, Transform roomB)
    {
        RoomBase roomBehaviourA = roomA.GetComponent<RoomBase>();
        RoomBase roomBehaviourB = roomB.GetComponent<RoomBase>();

        float horizontalDiff = GetDistanceOnGrid(roomA.position.x, roomB.position.x);
        float verticalDiff = GetDistanceOnGrid(roomA.position.z, roomB.position.z);

        if (horizontalDiff != 0)
        {
            roomBehaviourA.RoomWest = horizontalDiff == 1 ? roomB : roomBehaviourA.RoomWest;
            roomBehaviourA.RoomEast = horizontalDiff == -1 ? roomB : roomBehaviourA.RoomEast;

            roomBehaviourB.RoomEast = horizontalDiff == 1 ? roomA : roomBehaviourB.RoomEast;
            roomBehaviourB.RoomWest = horizontalDiff == -1 ? roomA : roomBehaviourB.RoomWest;
        }

        if (verticalDiff != 0)
        {
            roomBehaviourA.RoomSouth = verticalDiff == 1 ? roomB : roomBehaviourA.RoomSouth;
            roomBehaviourA.RoomNorth = verticalDiff == -1 ? roomB : roomBehaviourA.RoomNorth;

            roomBehaviourB.RoomNorth = verticalDiff == 1 ? roomA : roomBehaviourB.RoomNorth;
            roomBehaviourB.RoomSouth = verticalDiff == -1 ? roomA : roomBehaviourB.RoomSouth;
        }
    }

    /** RoomPlacement, Private Transform Method
	 * @param : int, int, Transform
     *  This method is used to instantiate a roomTransform at x,0,z position.
	 * We return the Transform of the new room instanciated.
     **/
    private Transform RoomPlacement(int x, int y, Transform roomTransform)
    {
        Vector3 roomPosition = new Vector3(x, 0, y);
        Transform newRoom = Instantiate(roomTransform, roomPosition, Quaternion.identity) as Transform;

        return newRoom;
    }

    /** BuildDoorsAndWalls, private void
	 * @param:Transform
	 * This Method is called to Build the Lateral objects of every rooms. (mainly doors and walls).
	 * First of all, we get the RoomBahviour of the Transform passed.
	 * Then we check if this room is linked to another one on each side. 
	 * If there is a link, we called the BuildLateralObjects(with the correct position and rotation) with a Door.
	 * If they are no links, we call it with a Wall.
	 **/
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

    /** BuildLateralObjects, private void
	 * @param : Transform, Transform, Vector3, Vector3
	 * This method is used to Add a GameObject prefab on a side of a room.
	 *Most of the time, the object is a Door or a Wall. 
	 * The object will be instanciated with an added position and a specific rotation calculated before.
	 * Then we set the parent of the new object which is the Transform passed as the second parameter.
	 **/
    private void BuildLateralObjects(Transform obj, Transform parent, Vector3 eulerRotation, Vector3 addedPosition)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = eulerRotation;
        Transform newObj = Instantiate(obj, parent.position + addedPosition, rotation) as Transform;
        newObj.position += obj.position;
        newObj.parent = parent;
    }

    /** AddCorridor, private void
	 * @param : Transform, Vector3, Vector3
	 * This method is used to Add a Corridor prefab between two rooms. 
	 * The Corridor will be instanciated with an added position and a specific rotation calculated before.
	 * Then we set the parent of the new corridor which is the Transform passed as the first parameter.
	 **/
    private void AddCorridor(Transform parent, Vector3 eulerRotation, Vector3 addedPosition)
    {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = eulerRotation;
        Transform newCorridor = Instantiate(corridor, parent.position + addedPosition, rotation) as Transform;
        newCorridor.parent = parent;
    }

    /** private int CenterOnGrid
	 * @param : float
	 * This method is used to return the position of a room inside the grid. 
	 * Please note that the best way to use this method is to pass as argument a position.x or a position.z in the worldSpace.
	 * For example, the First room is located at transform.position.x = 100 and transform.position.z = 100.
	 * If we center these positions on the grid, we will have get the [0,0] indexes of the RoomTable
	 **/
    private int CenterOnGrid(float position)
    {
        return (int)position / 100 - 1;
    }

    /** private bool IsBoarder
	 * @param:int, int
	 * This method is used to know if the Coords passed in argument are on a boarder of the map.
	 **/
    private bool IsBoarder(int x, int y)
    {
        return (x == map.MinBoarderX || x == map.MaxBoarderX || y == map.MinBoarderY || y == map.MaxBoarderY);
    }

    /** private bool IsCorner
	 * @param:int, int
	 * This method is used to know if the Coords passed in argument are in a Corner of the boarder.
	 **/
    private bool IsCorner(int x, int y)
    {
        return (x == map.MinBoarderX && y == map.MinBoarderY) || (x == map.MinBoarderX && y == map.MaxBoarderY) ||
               (x == map.MaxBoarderX && y == map.MinBoarderY) || (x == map.MaxBoarderX && y == map.MaxBoarderX);
    }

    /** private int GetDistanceOnGrid
	 * @param:float, float
	 * This method is used to get the Distance between two position floats related to the map grid.
	 * Please note that the Distance returned is the Distance centered on the grid and not in the world space
	 * Example : posA = 200, PosB = 100, Distance = 1.
	 **/
    private int GetDistanceOnGrid(float posA, float posB)
    {
        return (int)(CenterOnGrid(posA) - CenterOnGrid(posB));
    }
    #endregion methods

    #region innerClassAndStruct

    /** Coord, Public Inner Struc
	 * This struc is used to get the coord of a room with x and y.
	 **/
    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    /** Map, Public Inner Class
	 * this ineer class was made to Serve the MapGenerator. 
	 * It has a bunch of fields that are used to know the limit of the map.
	 * Also has a method to intitialize these fields at the begining.
	 **/
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