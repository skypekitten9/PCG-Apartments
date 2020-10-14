
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGrid : MonoBehaviour
{
    public GameObject[] furnitureArray;
    public int[] allowedFurniture;
    public GameObject tile;

    int sizeX, sizeZ;
    float spacing = 1;

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    private void Start()
    {
        ReadFile();
        StartCoroutine(SpawnGrid());
    }

    void ReadFile()
    {
        string testRoom = "LivingRoomFurniture 4 4;N1 E3 N2 00;00 E3 X2 00;00 00 00 00;00 00 00 00;";
        string[] rows = testRoom.Split(';');
        string[] tiles = rows[0].Split(' ');
        sizeX = int.Parse(tiles[1]);
        sizeZ = int.Parse(tiles[2]);
        for (int i = 0; i < sizeX; i++)
        {

        }
    }

    IEnumerator SpawnGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, z * spacing);
                SpawnObj(pos, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
    }

    void SpawnObj(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject obj = Instantiate(tile, spawnPosition, spawnRotation);
        obj.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
