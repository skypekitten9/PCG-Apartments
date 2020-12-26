using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA : MonoBehaviour
{
    List<Room> population;
    Room bestRoom;
    public GameObject gridSpawner;
    int generation;
    float fitness, bestFitness;

    public int populationSize, generationsAmount, roomWidth, roomHeight;

    private void Start()
    {
        generation = 1;
        fitness = 0;
        bestFitness = 0;
        population = new List<Room>();
        Instantiate(gridSpawner, gameObject.transform.position, gameObject.transform.rotation);
        for (int i = 0; i < populationSize; i++)
        {
            Room room = new Room(roomWidth, roomHeight, gridSpawner);
            room.GenerateRoom();
            population.Add(room);
            
        }
        SpawnRooms();
        CalculateFitness();
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
            Room parentA = ChooseParent();
            Room parentB = ChooseParent();

            Room child = parentA.CrossOver(parentB);
            newPopulation.Add(child);
        }
        population = newPopulation;
        generation++;
    }

    private void CalculateFitness()
    {
        float fitnessSum = 0;
        Room best = population[0];
        for (int i = 0; i < population.Count; i++)
        {
            population[i].CalculateFitness();
            fitnessSum += population[i].score;
            Debug.Log(population[i].score);

            if (population[i].score > best.score)
            {

            }
        }
        fitness = fitnessSum;
        bestFitness = best.score;
        bestRoom = best;
    }

    private Room ChooseParent()
    {
        float randomNumber = UnityEngine.Random.Range(0, 1);
        for (int i = 0; i < population.Count; i++)
        {
            if (randomNumber < population[i].score)
            {
                return population[i];
            }
            randomNumber -= population[i].score;
        }
        return null;
    }
}
