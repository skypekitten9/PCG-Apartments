using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string[] furniture { get; private set; }
    Random rand;
    public Room(int size)
    {
        furniture = new string[size];
        rand = new Random();
    }

    public float CalculateFitness()
    {
        return 0f;
    }

    public void GenerateRoom(int[] allowedFurniture)
    {

    }

    public Room CrossOver(Room parent)
    {
        Room child = new Room(furniture.Length);
        for (int i = 0; i < furniture.Length; i++)
        {
            if (i < furniture.Length/2)
            {
                child.furniture[i] = furniture[i];
            }
            else
            {
                child.furniture[i] = parent.furniture[i];
            }
        }
        return child;
    }

    
}
