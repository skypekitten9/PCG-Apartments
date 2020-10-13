using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGrid : MonoBehaviour
{
    public GameObject tile;
    public int xSize, zSize;
    public float spacing;

    private void Start()
    {
        StartCoroutine(SpawnGrid());
    }

    IEnumerator SpawnGrid()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
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
