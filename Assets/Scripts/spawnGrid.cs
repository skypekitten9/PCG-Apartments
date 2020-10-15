﻿
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class spawnGrid : MonoBehaviour
{
    public GameObject[] furnitureArray;
    public GameObject tile;

    int[,] indexArray;
    Direction[,] directionArray;

    int sizeX, sizeZ;
    float spacing = 1;

    public enum Direction
    {
        North,
        East,
        South,
        West,
        Empty,
        Ocupied
    }

    private void Start()
    {
        ReadFile();
        StartCoroutine(SpawnGrid());
    }

    void ReadFile()
    {
        string testRoom = "LivingRoomFurniture 4 4;N0 E0 S0 W0;00 00 00 00;00 00 00 00;00 00 00 00;";
        string[] rows = testRoom.Split(';');
        string[] tiles = rows[0].Split(' ');
        sizeX = int.Parse(tiles[1]);
        sizeZ = int.Parse(tiles[2]);

        indexArray = new int[sizeX, sizeZ];
        directionArray = new Direction[sizeX, sizeZ];


        for (int x = 0; x < sizeX; x++)
        {
            tiles = rows[x+1].Split(' ');
            for (int z = 0; z < sizeZ; z++)
            {
                Debug.Log(tiles[z]);
                if (tiles[z][0] == '0') directionArray[x, z] = Direction.Empty;
                else if (tiles[z][0] == 'X') directionArray[x, z] = Direction.Ocupied;
                else if (tiles[z][0] == 'N') directionArray[x, z] = Direction.North;
                else if (tiles[z][0] == 'S') directionArray[x, z] = Direction.South;
                else if (tiles[z][0] == 'E') directionArray[x, z] = Direction.East;
                else if (tiles[z][0] == 'W') directionArray[x, z] = Direction.West;
                tiles[z] = tiles[z].Substring(1);
                
                indexArray[x, z] = int.Parse(tiles[z]);
            }
        }
    }

    IEnumerator SpawnGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, z * spacing);
                SpawnTile(pos);
                if(directionArray[x,z] != Direction.Empty && directionArray[x,z] != Direction.Ocupied) SpawnObj(pos, directionArray[x, z], indexArray[x, z]);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
    }

    void SpawnObj(Vector3 spawnPosition, Direction direction, int index)
    {
        Vector3 rotation = new Vector3(0, 0, 0);
        switch (direction)
        {
            case Direction.East:
                rotation.y = 90;
                break;
            case Direction.South:
                rotation.y = 180;
                break;
            case Direction.West:
                rotation.y = 270;
                break;
            default:
                break;
        }
        GameObject obj = Instantiate(furnitureArray[index], spawnPosition, Quaternion.Euler(rotation));
    }

    void SpawnTile(Vector3 spawnPosition)
    {
        GameObject obj = Instantiate(tile, spawnPosition, Quaternion.identity);
        obj.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
