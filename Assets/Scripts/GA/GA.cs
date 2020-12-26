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
        Instantiate(gridSpawner, gameObject.transform.position, gameObject.transform.rotation);
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

    private void NewGeneration()
    {
        if (population.Count <= 0) return;
        CalculateFitness();
        List<Room> newPopulation = new List<Room>();

        for (int i = 0; i < population.Count; i++)
        {
            Room parentA = null;
            Room parentB = null;

            Room child = parentA.CrossOver(parentB);
            newPopulation.Add(child);
        }
        population = newPopulation;
        generation++;
    }

    private void CalculateFitness()
    {
        foreach (Room p in population)
        {
            p.CalculateFitness();
            Debug.Log(p.score);
        }
    }
}
