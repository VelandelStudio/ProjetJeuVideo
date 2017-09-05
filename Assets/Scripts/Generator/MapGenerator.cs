using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

/** MapGenerator : Class
 *  @Inherits : Monobehavior
 *  This script is here to generate un dungeon whitch is sqarre or rectangle.
 **/
public class MapGenerator : MonoBehaviour {

    public Transform[] rooms;

    // room[0] need to be with 4 corridors
    // room[1] need to be with 3 corridors
    // room[2] need to be with 2 corridors in an Elbow shape

    [SerializeField]
    private int seed = 0;        // for the random generation

    [SerializeField]
    private int min = 2;         // define the shape of the dungeon with a minimum for a 

    [SerializeField]
    private int max = 5;

    private Vector2 sizeMap;

    //gets and sets
    public int Seed { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }

    /** Simple constructor of MapGenerator
     *  This is to set the seed and the number of rooms
     **/
    public MapGenerator (int seed, int min, int max)
    {
        this.seed = seed;
        this.min = min;
        this.max = max;
    }

    private void Start()
    {
        GenerationMap();
    }

    /** generation map Method
     *  the idea is to place the rooms
     **/
    public void GenerationMap()
    {
        randSizeDungeon(seed);

        // to build th dungeon inside a empty GameObject
        string holderName = "Generated Dungeon";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform dungeonHolder = new GameObject(holderName).transform;
        dungeonHolder.parent = transform;

        // The grid to place the rooms
        for (int x = 0; x < sizeMap.x; x+=100)
        {
            for (int y = 0; y < sizeMap.y; y+=100)
            {
                Transform newRoom = placementRoom(x, y);
                // And make the new room being a child of dungeonHolder
                newRoom.parent = dungeonHolder;
            }
        }
    }

    /** randSizeDungeon Method
     *  This is the random size of the map
     *  We can multiply by 100 cause of the size of our prefab rooms
     **/
    private void randSizeDungeon (int seed)
    {
        System.Random prng = new System.Random(seed);

        sizeMap.x = prng.Next(min, max) * 100;
        sizeMap.y = prng.Next(min, max) * 100;
    }

    /** placementRoom Method
     *  Here to place the good prefab at the good place
     **/
    private Transform placementRoom(int x, int y)
    {
        Vector3 roomPosition = new Vector3(-sizeMap.x / 2 + 100f + x, 0, -sizeMap.y / 2 + 100f + y);
        Transform newRoom = null ;

        // Check all the possibility of a room placement

        if (x == 0 && y == 0)
            return newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
        else if (x == sizeMap.x - 100 && y == sizeMap.y - 100)
            return newRoom = Instantiate(rooms[2], roomPosition, Quaternion.identity) as Transform;
        else if (x == 0 && y == sizeMap.y - 100)
            return newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
        else if (x == sizeMap.x - 100 && y == 0)
            return newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
        else if ((x != 0 && x != sizeMap.x - 100) && y == 0)
            return newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
        else if ((x != 0 && x != sizeMap.x - 100) && y == sizeMap.y - 100)
            return newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
        else if (x == 0  && (y != 0 && y != sizeMap.y - 100))
            return newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
        else if (x == sizeMap.x - 100 && (y != 0 && y != sizeMap.y - 100))
            return newRoom = Instantiate(rooms[1], roomPosition, Quaternion.identity) as Transform;
        else if (x != 0 && y != 0 && y != sizeMap.y - 100 && x != 0 && x != sizeMap.x)
            return newRoom = Instantiate(rooms[0], roomPosition, Quaternion.identity) as Transform;

        return newRoom;
    }
}