using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA : MonoBehaviour
{
    List<Room> population;
    public GameObject gridSpawner;
    int generation;

    public int populationSize, generationsAmount, roomWidth, roomHeight;

    private void Start()
    {
        generation = 1;
        population = new List<Room>();
        GameObject.Instantiate(gridSpawner);
        for (int i = 0; i < populationSize; i++)
        {
            Room room = new Room(roomWidth, roomHeight, gridSpawner);
            room.GenerateRoom();
            population.Add(room);
            
        }
        SpawnRooms();
    }

    private void SpawnRooms()
    {
        string toPrint = population[0].GetRoomString();
        filerw.WriteToFile(toPrint, "Assets/Texts/Rooms/test.txt");
        gridSpawner.GetComponent<spawnGrid>().ReadFile("Assets/Texts/Rooms/test.txt");
        StartCoroutine(gridSpawner.GetComponent<spawnGrid>().SpawnGrid());

    }
}
