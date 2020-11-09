using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Room
{
    public string[,] furniture { get; private set; }
    int width, height;
    GameObject gridSpawner;
    public Room(int width, int height, GameObject gridSpawner)
    {
        furniture = new string[height , width];
        this.width = width;
        this.height = height;
        this.gridSpawner = gridSpawner;
    }

    public float CalculateFitness(int index)
    {
        return 0f;
    }

    public void GenerateRoom()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                switch (UnityEngine.Random.Range(0, 6))
                {
                    case 0:
                        furniture[i, j] = "N";
                        break;
                    case 1:
                        furniture[i, j] = "N";
                        break;
                    case 2:
                        furniture[i, j] = "N";
                        break;
                    case 3:
                        furniture[i, j] = "N";
                        break;
                    case 4:
                        furniture[i, j] = "0";
                        break;
                    case 5:
                        furniture[i, j] = "X";
                        break;
                    default:
                        break;
                }
                furniture[i, j] += UnityEngine.Random.Range(0, gridSpawner.GetComponent<spawnGrid>().furnitureArray.Length);
            }
        }
        FixCollisions();
    }

    void FixCollisions()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (furniture[i, j].Substring(0, 1) != "X" && furniture[i, j].Substring(0, 1) != "0")
                {
                    if (IsFurnitureMultipleTiles(Int32.Parse(furniture[i, j].Substring(1,1)))) SolveCollision(i, j, Int32.Parse(furniture[i, j].Substring(1, 1)));
                }
            }
        }
    }

    void SolveCollision(int i, int j, int furnitureIndex)
    {
        int furnitureHeight = gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().height;
        int furnitureWidth = gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().width;
        if (furniture[i, j].Substring(0, 1) == "N")
        {
            if (i + furnitureHeight >= height || j + furnitureWidth >= width)
            {
                furniture[i, j] = "00";
                return;
            }

            for (int m = 1; m == furnitureHeight; m++)
            {
                for (int n = 1; n == furnitureWidth; n++)
                {
                    furniture[i + m, j + n] = "X" + furnitureIndex;
                }
            }
        }
        else if (furniture[i, j].Substring(0, 1) == "E")
        {
            if (i + furnitureWidth > height || j + furnitureHeight > width)
            {
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = 0; n < furnitureHeight; n++)
                {
                    furniture[i + m, j + n] = "00";
                }
            }
        }
        else if (furniture[i, j].Substring(0, 1) == "S")
        {
            if (i - furnitureHeight < 0 || j - furnitureWidth < 0)
            {
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureHeight; m++)
            {
                for (int n = 0; n < furnitureWidth; n++)
                {
                    furniture[i - m, j - n] = "00";
                }
            }
        }
        else if (furniture[i, j].Substring(0, 1) == "W")
        {
            if (i - furnitureWidth < 0 || j - furnitureHeight < 0)
            {
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = 0; n < furnitureHeight; n++)
                {
                    furniture[i - m, j - n] = "00";
                }
            }
        }
        else return;

        
    }

    bool IsFurnitureMultipleTiles(int furnitureIndex)
    {
        if (gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().width > 1) return true;
        if (gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().height > 1) return true;
        return false;
    }

    public Room CrossOver(Room parent) //EJ IMPLEMENTERAD KORREKT ÄN
    {
        Room child = new Room(width, height, gridSpawner);
        for (int i = 0; i < furniture.Length; i++)
        {
            if (i < furniture.Length/2)
            {
                child.furniture[i, i] = furniture[i, i];
            }
            else
            {
                child.furniture[i, i] = parent.furniture[i, i];
            }
        }
        return child;
    }

    public string GetRoomString()
    {
        string result = gridSpawner.name + " " + height + " " + width + ";\n";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j != 0) result += " ";
                result += furniture[i, j];
            }
            result += ";\n";
        }
        return result;
    }
}
