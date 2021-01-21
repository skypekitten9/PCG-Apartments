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
    float populationFitness, bestRoomFitness;

    public int populationSize, generationsAmount, roomWidth, roomHeight;

    private void Start()
    {
        //Initialize
        generation = 1;
        populationFitness = 0;
        bestRoomFitness = 0;
        population = new List<Room>();
        Instantiate(gridSpawner, gameObject.transform.position, gameObject.transform.rotation);

        //Generate the first population
        for (int i = 0; i < populationSize; i++)
        {
            Room room = new Room(roomWidth, roomHeight, gridSpawner);
            room.GenerateRoom();
            population.Add(room);
        }

        //Calculate fitness
        CalculateFitness();

        //Sort population
        SortPopulation();

        //GA
        while (generation < generationsAmount)
        {
            //Generate new generation and merge
            population.AddRange(NewGeneration());

            //Calculate fitness
            CalculateFitness();

            //Sort rooms by fitness
            SortPopulation();

            //Create new population
            int amountToRemove = population.Count - populationSize;
            population.RemoveRange(populationSize - 1, amountToRemove);
        }
        SpawnRoom(bestRoom);
    }

    private void SpawnRoom(Room room)
    {
        string toPrint = room.GetRoomString();
        filerw.WriteToFile(toPrint, "Assets/Texts/Rooms/test.txt");
        gridSpawner.GetComponent<spawnGrid>().ReadFile("Assets/Texts/Rooms/test.txt");
        StartCoroutine(gridSpawner.GetComponent<spawnGrid>().SpawnGrid());

    }

    private List<Room> NewGeneration()
    {
        if (population.Count <= 0) return null;
        CalculateFitness();
        List<Room> newGeneration = new List<Room>();

        for (int i = 0; i < population.Count; i++)
        {
            Room parentA = ChooseParent();
            Room parentB = ChooseParent();

            Room child = parentA.CrossOver(parentB);
            newGeneration.Add(child);
        }
        generation++;
        return newGeneration;
    }

    private void CalculateFitness()
    {
        float fitnessSum = 0;
        Room best = population[0];
        for (int i = 0; i < population.Count; i++)
        {
            population[i].CalculateFitness();
            fitnessSum += population[i].fitness;

            if (population[i].fitness > best.fitness)
            {

            }
        }
        populationFitness = fitnessSum;
        bestRoomFitness = best.fitness;
        bestRoom = best;
    }

    private void SortPopulation()
    {

    }

    private Room ChooseParent()
    {
        float randomNumber = UnityEngine.Random.Range(0, 1);
        for (int i = 0; i < population.Count; i++)
        {
            if (randomNumber < population[i].fitness)
            {
                return population[i];
            }
            randomNumber -= population[i].fitness;
        }
        return null;
    }
}
