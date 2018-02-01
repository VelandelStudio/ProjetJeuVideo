using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldGeneratorMap : MonoBehaviour {

    public int Seed { get; set; }                               // for the random generation Map
    public List<GameObject> rooms = new List<GameObject>();     // List of all rooms in the game
    public Transform[] roomTypes;

    // room[0] need to be with 4 corridors
    // room[1] need to be with 3 corridors
    // room[2] need to be with 2 corridors in an Elbow shape

    [SerializeField]
    private int min = 2;            // define the shape of the dungeon with a minimum number of rows and columns
    [SerializeField]
    private int max = 5;            // define the shape of the dungeon with a minimum number of rows and columns    
    private Vector2 sizeMap;        // Vector2 that define the width and length of the dungeon

    /** generation map Method
     *  the idea is to place the rooms
     **/
    public void GenerationMapOld()
    {
        RandSizeDungeon(Seed);

        // to build th dungeon inside a empty GameObject
        string holderName = "Generated Dungeon";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform dungeonHolder = new GameObject(holderName).transform;
        dungeonHolder.parent = transform;

        // The grid to place the rooms
        for (int x = 0; x < sizeMap.x; x += 100)
        {
            for (int y = 0; y < sizeMap.y; y += 100)
            {
                Transform newRoom = PlacementRoom(x, y);
                rooms.Add(newRoom.gameObject);

                // And make the new room being a child of dungeonHolder
                newRoom.parent = dungeonHolder;
            }
        }

        Debug.Log(rooms.Count);
    }

    /** randSizeDungeon Method
     *  This is the random size of the map
     *  We can multiply by 100 cause of the size of our prefab rooms
     **/
    private void RandSizeDungeon(int seed)
    {
        System.Random prng = new System.Random(seed);

        sizeMap.x = prng.Next(min, max) * 100;
        sizeMap.y = prng.Next(min, max) * 100;
    }

    /** placementRoom Method
     *  Here to place the good prefab at the good place
     **/
    private Transform PlacementRoom(int x, int y)
    {
        Vector3 roomPosition = new Vector3(-sizeMap.x / 2 + 100f + x, 0, -sizeMap.y / 2 + 100f + y);
        Transform newRoom = null;

        // Check all the possibility of a room placement

        var line = y == 0 ? 0 : (y == sizeMap.y - 100 ? 2 : 1);
        var col = x == 0 ? 0 : (x == sizeMap.x - 100 ? 2 : 1);

        switch (line + " | " + col)
        {
            case "0 | 0":
                newRoom = Instantiate(roomTypes[2], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
                break;
            case "0 | 1":
                newRoom = Instantiate(roomTypes[1], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
                break;
            case "0 | 2":
                newRoom = Instantiate(roomTypes[2], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
                break;
            case "1 | 0":
                newRoom = Instantiate(roomTypes[1], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
                break;
            case "1 | 1":
                newRoom = Instantiate(roomTypes[0], roomPosition, Quaternion.identity) as Transform;
                break;
            case "1 | 2":
                newRoom = Instantiate(roomTypes[1], roomPosition, Quaternion.identity) as Transform;
                break;
            case "2 | 0":
                newRoom = Instantiate(roomTypes[2], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
                break;
            case "2 | 1":
                newRoom = Instantiate(roomTypes[1], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
                break;
            case "2 | 2":
                newRoom = Instantiate(roomTypes[2], roomPosition, Quaternion.identity) as Transform;
                break;
            default:
                newRoom = null;
                break;
        }

        return newRoom;
    }
}
