using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    /*
     small: probability = 72  | neighbor threshold : 55
     medium: probability = 75 | neighbor Threshold : 51
     large: probability = 75 | neighbor Threshold : 51
    */
    public bool small; // This option won't do anything
    public bool medium;
    public bool large;

    private int[,] map;
    private int maxX;
    private int maxY;

    //[Range(25, 100)]
    private int probability;
    //[Range(25, 100)]
    private int neighborThreshold;

    public GameObject[] prefabList;

    void Awake()
    {
        maxX = 50;
        maxY = 50;
        if (large)
        {
            probability = 75;
            neighborThreshold = 51;
            maxX *= 4;
            maxY *= 4;
        }
        else if(medium)
        {
            probability = 75;
            neighborThreshold = 51;
            maxX *= (int)(2.5 + 0.5);
            maxY *= (int)(2.5 + 0.5);
        }
        else
        {
            probability = 72;
            neighborThreshold = 55;
        }
        map = new int[maxX, maxY];
        initializeMap();
        smoothMap();
        generateMap();
    }

    void initializeMap()
    {
        for(int x = 0; x < maxX; x++)
        {
            for(int y = 0; y < maxY; y++)
            {
                if (Random.Range(25f, 100f) <= probability || x == 0 || x == maxX-1 || y == 0)
                    map[x, y] = 1;
                else
                    map[x, y] = 0;
            }
        }
    }

    void smoothMap()
    {
        for (int x = 1; x < maxX - 1; x++)
        {
            for (int y = 1; y < maxY - 1; y++)
            {
                float numNeighbors = 0;
                if (map[x - 1, y - 1] == 1)
                    numNeighbors++;
                if (map[x, y - 1] == 1)
                    numNeighbors++;
                if (map[x + 1, y - 1] == 1)
                    numNeighbors++;
                if (map[x - 1, y] == 1)
                    numNeighbors++;
                if (map[x + 1, y] == 1)
                    numNeighbors++;
                if (map[x - 1, y + 1] == 1)
                    numNeighbors++;
                if (map[x, y + 1] == 1)
                    numNeighbors++;
                if (map[x + 1, y + 1] == 1)
                    numNeighbors++;

                numNeighbors *= 12.5f;

                if (numNeighbors >= neighborThreshold)
                    map[x, y] = 1;
                else
                    map[x, y] = 0;
            }
        }
    }

    void generateMap()
    {
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if(map[x,y] == 1)
                {
                    Object.Instantiate(prefabList[0], new Vector3(x, y, 90f), new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }
}
