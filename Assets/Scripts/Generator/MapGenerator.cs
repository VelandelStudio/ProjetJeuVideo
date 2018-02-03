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
    [SerializeField]public Transform[,] roomTable;

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
        // Action here !
        /* IL MECLAIRE QUAND IL BRILLE DANS LA NUIIIIIIIIIIIIIIIIIT
         * On a à présent une tableau de transform roomTable qui contient les transform des rooms.
         * On a bouclé sur l'ensemble de tableau pour trouver un chemin valide et court.
         * Toutes les salles selectionnées dans ce chemin se sont vu attribuée un booleen = true dans leur RoomBehaviour
         * Il s'agit du RoomSelected qui vaut true à présent.
         * La méthode suivante ClearUnselectedRooms parcourt ce tableau et supprime les GameObjects qui portent un RoomSelected = false.
         * Le but est d'essayer d'ajouter juste ici une methode qui permette de parcourir le tableau et d'ajouter de manière plus ou moins aléatoires
         * des salles dont la valeur de RoomSelected seriant = true  pour éviter qu'elles ne soient détruite. Attention, il faudra que ces salles
         * soient adjacentes aux salles qui ont deja été selectionnées.
         **/
        //ClearUnselectedRooms();
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
        startRoom = PlacementRoom(randomCoord.x, randomCoord.y, startRoom);
        startRoom.parent = _dungeonHolder;
        SetRotationBoarderRooms(startRoom);

        randomCoord = _ShuffleBoardCoord.Dequeue();
        endRoom = PlacementRoom(randomCoord.x, randomCoord.y, endRoom);
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

    private void BuildShortestPath(Transform roomA, Transform roomB)
    {

        int HorizontalDist = (int)(roomA.position.x / 100 - roomB.position.x / 100);
        int VerticalDist = (int)(roomA.position.z / 100 - roomB.position.z / 100);
        int rand;

        Transform nextRoomA = roomA;
        Transform nextRoomB = roomB;

        if(roomA.transform.position == roomB.transform.position)
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
                nextRoomA = roomTable[CenterOnGrid(roomA.position.x) +modulo, CenterOnGrid(roomA.position.z)];
            }
            else
            {
                int modulo = roomA.position.z < roomB.position.z ? 1 : -1;
                nextRoomA = roomTable[CenterOnGrid(roomA.position.x), CenterOnGrid(roomA.position.z) +modulo];
            }
        }

        nextRoomA.GetComponent<RoomBehaviour>().RoomSelected = true;
        nextRoomB.GetComponent<RoomBehaviour>().RoomSelected = true;

        BuildShortestPath(nextRoomA,nextRoomB);
    }

    private Transform GetAdjacentInnerRoom(Transform boarderRoom)
    {
        int moduloX = 0;
        int moduloZ = 0;
        if (boarderRoom.position.x == map.MinBoarderX || boarderRoom.position.x == map.MaxBoarderX)
        {
            moduloX = boarderRoom.position.x == map.MinBoarderX ? 1 : -1;
        }
        if (boarderRoom.position.z == map.MinBoarderY || boarderRoom.position.z == map.MaxBoarderY)
        {
            moduloZ = boarderRoom.position.z == map.MinBoarderY ? 1 : -1;
        }

        return roomTable[CenterOnGrid(boarderRoom.position.x) + moduloX, CenterOnGrid(boarderRoom.position.z) + moduloZ];
    }

    private void ClearUnselectedRooms()
    {
        for (int i = 0; i < roomTable.GetLength(0); i++)
        {
            for (int j = 0; j < roomTable.GetLength(1); j++)
            {
                RoomBehaviour rb = roomTable[i, j].GetComponent<RoomBehaviour>();
                if (!rb.RoomSelected)
                {
                    Destroy(rb.gameObject);
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