using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform[] rooms;
    public int seed = 0;
    public int min = 2;
    public int max = 5;

    private Vector2 sizeMap;

    private void Start()
    {
        GenerationMap();
    }

    public void GenerationMap()
    {
        randSizeDungeon(seed);

        string holderName = "Generated Dungeon";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform dungeonHolder = new GameObject(holderName).transform;
        dungeonHolder.parent = transform;

        for (int x = 0; x < sizeMap.x; x+=100)
        {
            for (int y = 0; y < sizeMap.y; y+=100)
            {
                Transform newRoom = placementRoom(x, y);
                newRoom.parent = dungeonHolder;
            }
        }
    }

    private void randSizeDungeon (int seed)
    {
        System.Random prng = new System.Random(seed);

        sizeMap.x = prng.Next(min, max) * 100;
        sizeMap.y = prng.Next(min, max) * 100;
    }

    private Transform placementRoom(int x, int y)
    {
        Vector3 roomPosition = new Vector3(-sizeMap.x / 2 + 100f + x, 0, -sizeMap.y / 2 + 100f + y);
        Transform newRoom = null ;

        if (x == 0 && y == 0)
            newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
        else if (x == sizeMap.x - 100 && y == sizeMap.y - 100)
            newRoom = Instantiate(rooms[2], roomPosition, Quaternion.identity) as Transform;
        else if (x == 0 && y == sizeMap.y - 100)
            newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
        else if (x == sizeMap.x - 100 && y == 0)
            newRoom = Instantiate(rooms[2], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
        else if ((x != 0 && x != sizeMap.x - 100) && y == 0)
            newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * 90)) as Transform;
        else if ((x != 0 && x != sizeMap.x - 100) && y == sizeMap.y - 100)
            newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * -90)) as Transform;
        else if (x == 0  && (y != 0 && y != sizeMap.y - 100))
            newRoom = Instantiate(rooms[1], roomPosition, Quaternion.Euler(Vector3.up * 180)) as Transform;
        else if (x == sizeMap.x - 100 && (y != 0 && y != sizeMap.y - 100))
            newRoom = Instantiate(rooms[1], roomPosition, Quaternion.identity) as Transform;
        else if (x != 0 && y != 0 && y != sizeMap.y - 100 && x != 0 && x != sizeMap.x)
            newRoom = Instantiate(rooms[0], roomPosition, Quaternion.identity) as Transform;

        return newRoom;
    }
}