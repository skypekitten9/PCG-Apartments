using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Room
{
    public string[,] furniture { get; private set; }
    int width, height;
    static int roomCount;
    int ID;
    GameObject gridSpawner;
    int sofaCount, lampCount, tableCount, tvCount, chairCount;
    public float fitness;
    public Room(int width, int height, GameObject gridSpawner)
    {
        ID = roomCount;
        roomCount++;
        furniture = new string[height , width];
        this.width = width;
        this.height = height;
        this.gridSpawner = gridSpawner;
    }

    public int CountFurniture(int furnitureIndex)
    {
        int amount = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(Int32.Parse(furniture[i, j].Substring(1)) == furnitureIndex && furniture[i, j].Substring(0, 1) != "X"  && furniture[i, j].Substring(0, 1) != "0")
                {
                    amount++;
                }
            }
        }
        return amount;
    }

    private float EvaluateFamily(int i, int j)
    {
        if (furniture[i, j].Substring(0, 1) == "X" || furniture[i, j].Substring(0, 1) == "0") return 0;
        FamilyType familyType = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().familyType;
        switch (familyType)
        {
            case FamilyType.Surrounding:
                //return SurroundingFamily(i, j, 1, 1);
                return 0;
            case FamilyType.Directional:
                return DirectionalFamily(i, j, familyType);
            default:
                return 0f;
        }
    }

    private float DirectionalFamily(int i, int j, FamilyType familyType)
    {
        float result = 0f;
        string direction = furniture[i, j].Substring(0, 1);
        int furnitureWidth = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().width;
        int prefferedDistance = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().prefferedDistance;
        FurnitureType likes = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().likes;

        //North
        if (direction == "N")
        {
            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = i - 1; n >= 0; n--)
                {
                    if (IsFurnitureType(n, j + m, likes))
                    {
                        if (Mathf.Abs(n - i) == prefferedDistance) result += 1;
                        break;
                    }
                }
            }
        }
        //East
        else if (direction == "E")
        {
            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = j + 1; n < width; n++)
                {
                    if (IsFurnitureType(i + m, n, likes))
                    {
                        if (Mathf.Abs(n - j) == prefferedDistance) result += 1;
                        break;
                    }
                }
            }
        }
        //South
        else if (direction == "S")
        {
            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = i + 1; n < height; n++)
                {
                    if (IsFurnitureType(n, j + m, likes))
                    {
                        if (Mathf.Abs(n - i) == prefferedDistance) result += 1;
                        break;
                    }
                }
            }
        }
        //West
        else if (direction == "W")
        {
            for (int m = 0; m < furnitureWidth; m++)
            {
                for (int n = j - 1; n >= 0; n--)
                {
                    if (IsFurnitureType(i - m, n, likes))
                    {
                        if (Mathf.Abs(n - j) == prefferedDistance) result += 1;
                        break;
                    }
                }
            }
        }
        else result -= 0.5f;
        return result;

    }

    private bool IsFurnitureType(int i, int j, FurnitureType furnitureType)
    {
        if (i >= height || i < 0) return false;
        if (j >= width || j < 0) return false;
        if (furniture[i, j].Substring(0, 1) == "0") return false;

        if (gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().furnitureType == furnitureType)
        {
            return true;
        }
        else return false;
    }

    private float SurroundingFamily(int i, int j, int range, float scoreChange)
    {       
        if (furniture[i, j].Substring(0, 1) == "X" || furniture[i, j].Substring(0, 1) == "0") return 0;

        float result = 0;
        FamilyType familyType = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().familyType;
        int startX = i - range;
        int startY = j - range;
        if (startX < 0) startX = 0;
        if (startY < 0) startY = 0;

        for (int x = startX; x <= i + range; x++)
        {
            if (x >= height) break;
            for (int z = startY; z <= j + range; z++)
            {
                if (z >= width) break;
                result += EvaluateNeighbour(x, z, familyType, scoreChange);
            }
        }

        return result;
    }

    //Returns the change in score from a neighbour with index i, j with concideration on a FamilyType
    public float EvaluateNeighbour(int i, int j, FamilyType type, float scoreChange)
    {
        if (furniture[i, j].Substring(0, 1) == "X" || furniture[i, j].Substring(0, 1) == "0") return 0;

        FamilyType familyType = gridSpawner.GetComponent<spawnGrid>().furnitureArray[Int32.Parse(furniture[i, j].Substring(1, 1))].GetComponent<Furniture>().familyType;
        if (familyType == type) return scoreChange;
        else if (familyType == FamilyType.None) return 0;
        else return (scoreChange * -1);
    }

    public float CalculateFitness()
    {
        fitness = 0f;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                fitness += EvaluateFamily(i, j);
            }
        }
        //for (int i = 0; i < CountFurniture(3); i++)
        //{
        //    fitness += 0.05f;
        //}
        //if (sofaCount > 1)
        //{
        //    fitness -= 0.2f;          
        //}

        //if (tableCount > 1)
        //{
        //    fitness -= 0.1f;
        //}

        //if (lampCount < 1)
        //{
        //    fitness -= 0.1f;
        //}

        //if (lampCount > 3)
        //{
        //    fitness -= 0.1f;
        //}

        //if (tvCount > 1)
        //{
        //    fitness -= 0.2f;
        //}

        //if (tableCount > 0)
        //{
        //    if (chairCount > 2)
        //    {
        //        fitness += 0.3f;
        //    }
        //}

        return fitness; //debug
    }

    #region CreatingRooms
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
        CalculateFitness();
    }

    public void FixCollisions()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (furniture[i, j].Substring(0, 1) == "X")
                {
                    furniture[i, j] = "00";
                }
            }
        }
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
    }

    #endregion

    bool IsFurnitureMultipleTiles(int furnitureIndex)
    {
        if (gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().width > 1) return true;
        if (gridSpawner.GetComponent<spawnGrid>().furnitureArray[furnitureIndex].GetComponent<Furniture>().height > 1) return true;
        return false;
    }

    public Room CrossOver(Room parent, int mutationRate, int populationSize)
    {
        Room child = new Room(width, height, gridSpawner);
        for (int i = 0; i < furniture.GetLength(0); i++)
        {
            for (int j = 0; j < furniture.GetLength(1); j++)
            {
                //if (2 % (i + 2) == 0) child.furniture[i, j] = furniture[i, j];
                //else child.furniture[i, j] = parent.furniture[i, j];

                if (i < (height / 3))
                {
                    child.furniture[i, j] = furniture[i, j];
                }
                else if (i >= (height / 3) && i < (height / 3) * 2)
                {
                    child.furniture[i, j] = parent.furniture[i, j];
                }
                else
                {
                    child.furniture[i, j] = furniture[i, j];
                }
            }
        }
        child.Mutate(mutationRate, populationSize);
        child.FixCollisions();
        child.CalculateFitness();
        return child;
    }

    public void Mutate(int mutationRate, int populationSize)
    {
        if (mutationRate == 0) return;
        int randomNumber = UnityEngine.Random.Range(0, populationSize - 1);
        if(randomNumber <= mutationRate)
        {
            int i = UnityEngine.Random.Range(0, height - 1);
            int j = UnityEngine.Random.Range(0, width - 1);
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
