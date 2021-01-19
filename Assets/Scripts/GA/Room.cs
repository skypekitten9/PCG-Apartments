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
    int sofaCount, lampCount, tableCount, tvCount, chairCount;
    public float score;
    public Room(int width, int height, GameObject gridSpawner)
    {
        furniture = new string[height , width];
        this.width = width;
        this.height = height;
        this.gridSpawner = gridSpawner;
    }

    public void CountFurniture()
    {
        foreach (string f in furniture)
        {

            if (Int32.Parse(f.Substring(1)) == 0)
            {
                tvCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 1)
            {
                tableCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 2)
            {
                tableCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 3)
            {
                sofaCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 4)
            {
                lampCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 5)
            {
                chairCount++;
            }

            if (Int32.Parse(f.Substring(1)) == 6)
            {
                chairCount++;
            }
        }

    }

    public float CalculateFitness()
    {
        score = 1f;
        CountFurniture();

        if (sofaCount > 1)
        {
            score -= 0.2f;          
        }

        if (tableCount > 1)
        {
            score -= 0.1f;
        }

        if (lampCount < 1)
        {
            score -= 0.1f;
        }

        if (lampCount > 3)
        {
            score -= 0.1f;
        }

        if (tvCount > 1)
        {
            score -= 0.2f;
        }

        if (tableCount > 0)
        {
            if (chairCount > 2)
            {
                score += 0.3f;
            }
        }

        return score;
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
                        furniture[i, j] = "E";
                        break;
                    case 2:
                        furniture[i, j] = "S";
                        break;
                    case 3:
                        furniture[i, j] = "W";
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
        //North
        if (furniture[i, j].Substring(0, 1) == "N")
        {
            Debug.Log("Solving north!" + i + " " + j);
            if (i + furnitureHeight > height || j + furnitureWidth > width)
            {
                Debug.Log("Out of bounds");
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureHeight; m++)
            {
                for (int n = 0; n < furnitureWidth; n++)
                {
                    if (m == 0 && n == 0) continue;
                    if(furniture[i + m, j + n].Substring(0, 1) == "X")
                    {
                        furniture[i, j] = "00";
                        return;
                    }
                    Debug.Log("Removed: "  + (i + m) + " " + (j + n));
                    furniture[i + m, j + n] = "X" + furnitureIndex;
                }
            }
        }

        //East
        if (furniture[i, j].Substring(0, 1) == "E")
        {
            Debug.Log("Solving East!" + i + " " + j);

            if (i + furnitureWidth > height || j + furnitureHeight > width)
            {
                Debug.Log("Out of bounds");
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = 0; n < furnitureHeight; n++)
                {
                    if (m == 0 && n == 0) continue;
                    if (furniture[i + m, j + n].Substring(0, 1) == "X")
                    {
                        furniture[i, j] = "00";
                        return;
                    }
                    Debug.Log("Removed: " + (i + m) + " " + (j + n));
                    furniture[i + m, j + n] = "X" + furnitureIndex;
                }
            }
        }

        //South
        if (furniture[i, j].Substring(0, 1) == "S")
        {
            Debug.Log("Solving south!" + i + " " + j);
            if (i - furnitureHeight + 1 < 0 || j - furnitureWidth + 1 < 0)
            {
                Debug.Log("Out of bounds");
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureHeight; m++)
            {
                for (int n = 0; n < furnitureWidth; n++)
                {
                    if (m == 0 && n == 0) continue;
                    if (furniture[i - m, j - n].Substring(0, 1) == "X")
                    {
                        furniture[i, j] = "00";
                        return;
                    }
                    Debug.Log("Removed: " + (i - m) + " " + (j - n));
                    furniture[i - m, j - n] = "X" + furnitureIndex;
                }
            }
        }

        //West
        if (furniture[i, j].Substring(0, 1) == "W")
        {
            Debug.Log("Solving west!" + i + " " + j);
            if (i - furnitureWidth + 1 < 0 || j - furnitureHeight + 1 < 0)
            {
                Debug.Log("Out of bounds");
                furniture[i, j] = "00";
                return;
            }

            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = 0; n < furnitureHeight; n++)
                {
                    if (m == 0 && n == 0) continue;
                    if (furniture[i - m, j - n].Substring(0, 1) == "X")
                    {
                        furniture[i, j] = "00";
                        return;
                    }
                    Debug.Log("Removed: " + (i - m) + " " + (j - n));
                    furniture[i - m, j - n] = "X" + furnitureIndex;
                }
            }
        }

        //else if (furniture[i, j].Substring(0, 1) == "E")
        //{
        //    if (i + furnitureWidth > height || j + furnitureHeight > width)
        //    {
        //        furniture[i, j] = "00";
        //        return;
        //    }

        //    for (int m = 0; m < furnitureWidth; m++)
        //    {
        //        for (int n = 0; n < furnitureHeight; n++)
        //        {
        //            furniture[i + m, j + n] = "00";
        //        }
        //    }
        //}
        //else if (furniture[i, j].Substring(0, 1) == "S")
        //{
        //    if (i - furnitureHeight < 0 || j - furnitureWidth < 0)
        //    {
        //        furniture[i, j] = "00";
        //        return;
        //    }

        //    for (int m = 0; m < furnitureHeight; m++)
        //    {
        //        for (int n = 0; n < furnitureWidth; n++)
        //        {
        //            furniture[i - m, j - n] = "00";
        //        }
        //    }
        //}
        //else if (furniture[i, j].Substring(0, 1) == "W")
        //{
        //    if (i - furnitureWidth < 0 || j - furnitureHeight < 0)
        //    {
        //        furniture[i, j] = "00";
        //        return;
        //    }

        //    for (int m = 0; m < furnitureWidth; m++)
        //    {
        //        for (int n = 0; n < furnitureHeight; n++)
        //        {
        //            furniture[i - m, j - n] = "00";
        //        }
        //    }
        //}
        //else return;


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
        for (int i = 0; i < furniture.GetLength(0); i++)
        {
            for (int j = 0; j < furniture.GetLength(1); j++)
            {
                int random = UnityEngine.Random.Range(0, 10);
                if (random > 5)
                {
                    child.furniture[i, j] = furniture[i, j];
                }
                else
                {
                    child.furniture[i, j] = parent.furniture[i, j];
                }
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
