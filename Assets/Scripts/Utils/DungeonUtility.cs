using System;
using System.Collections;
using System.Collections.Generic;

public static class DungeonUtility {

    public static T[] SuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = prng.Next(i, array.Length);
            T temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }

        return array;
    }
}
