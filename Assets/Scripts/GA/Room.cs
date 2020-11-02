using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Room
{
    public string[] furniture { get; private set; }
    int width, height;
    public GameObject gridSpawner;
    public Room(int size, int width, int height)
    {
        furniture = new string[size];
        this.width = width;
        this.height = height;
    }

    public float CalculateFitness(int index)
    {
        return 0f;
    }

    public void GenerateRoom(int[] allowedFurniture)
    {
        for (int i = 0; i < furniture.Length; i++)
        {
            switch (UnityEngine.Random.Range(0,6))
            {
                case 0:
                    furniture[i] = "N";
                    break;
                case 1:
                    furniture[i] = "E";
                    break;
                case 2:
                    furniture[i] = "S";
                    break;
                case 3:
                    furniture[i] = "W";
                    break;
                case 4:
                    furniture[i] = "0";
                    break;
                case 5:
                    furniture[i] = "X";
                    break;
                default:
                    break;
            }
            furniture[i] += allowedFurniture[UnityEngine.Random.Range(0, allowedFurniture.Length)];
        }
    }

    public Room CrossOver(Room parent)
    {
        Room child = new Room(furniture.Length, width, height);
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

    public void GenerateRoom()
    {

    }

    
}
