
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class spawnGrid : MonoBehaviour
{
    
    public bool generateTiles, coloredTiles;
    public bool delay;
    public float secondsToDelay;
    public GameObject[] furnitureArray;

    int[,] indexArray;
    Direction[,] directionArray;

    int sizeX, sizeZ;
    int originX = 0;
    int originZ = 0;
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
        //ReadFile("Assets/Texts/Rooms/test.txt");
        //StartCoroutine(SpawnGrid());
    }

    public void SetOrigin(int x, int z)
    {
        originX = x;
        originZ = z;
    }

    public void ReadFile(string fileName)
    {
        string content = filerw.FileToString(fileName);
        Debug.Log(content);

        string[] rows = content.Split(';');
        string[] tiles = rows[0].Split(' ');
        sizeX = int.Parse(tiles[1]);
        sizeZ = int.Parse(tiles[2]);

        indexArray = new int[sizeX, sizeZ];
        directionArray = new Direction[sizeX, sizeZ];


        for (int x = 0; x < sizeX; x++)
        {
            tiles = rows[x + 1].Split(' ');
            for (int z = 0; z < sizeZ; z++)
            {
                tiles[z] = tiles[z].Trim();
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

    public IEnumerator SpawnGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Vector3 pos = new Vector3(originX + x * spacing, 0, originZ + z * spacing);
                if(generateTiles) SpawnTile(pos);
                if (directionArray[x,z] != Direction.Empty && directionArray[x,z] != Direction.Ocupied) SpawnObj(pos, directionArray[x, z], indexArray[x, z]);
                if(delay) yield return new WaitForSeconds(secondsToDelay);
            }
        }
        yield return new WaitForSeconds(0.0f);
        yield return new WaitForEndOfFrame();
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
        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tile.transform.position = spawnPosition;
        tile.transform.localScale = new Vector3(1, 0.2f, 1);
        if(coloredTiles) tile.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        tile.gameObject.tag = "Tile";
    }

    public void ResetSpawner()
    {
        GameObject[] furnitures = GameObject.FindGameObjectsWithTag("Furniture");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject f in furnitures)
            Destroy(f);

        foreach (GameObject t in tiles)
            Destroy(t);
    }
}
