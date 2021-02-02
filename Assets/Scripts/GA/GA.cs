using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GA : MonoBehaviour
{
    List<Room> population;
    Room bestRoom;
    public GameObject gridSpawner;
    int generation, timesEvolved, generationMax, stepCount;
    float populationFitness, bestRoomFitness;

    public int populationSize, generationStepSize, roomWidth, roomHeight;

    private void Start()
    {
        //Initialize
        generation = 0;
        stepCount = 0;
        timesEvolved = 0;
        generationMax = generationStepSize;
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
        population = SortPopulation();

        //Evolve
        Evolve();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            stepCount++;
            generationMax += generationStepSize;
            Evolve();
        }
    }

    private void Evolve()
    {
        //GA
        while (generation < generationMax)
        {
            //Generate new generation and merge
            population.AddRange(NewGeneration());

            //Calculate fitness
            CalculateFitness();

            //Sort rooms by fitness
            population = SortPopulation();

            //Create new population
            int amountToRemove = population.Count - populationSize;
            population.RemoveRange(populationSize - 1, amountToRemove);
        }
        SpawnRoom(bestRoom);
        UpdateUI();
    }

    private void UpdateUI()
    {
        UserInterface.Instance.SetGeneration(generation);
        UserInterface.Instance.SetPopulation(population.Count);
        UserInterface.Instance.SetFitness(bestRoomFitness);
    }

    private void SpawnRoom(Room room)
    {
        gridSpawner.GetComponent<spawnGrid>().ResetSpawner();
        string toPrint = room.GetRoomString();
        string fileName = "room" + stepCount.ToString() + ".txt";
        filerw.WriteToFile(toPrint, fileName);
        gridSpawner.GetComponent<spawnGrid>().ReadFile(fileName);
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
                best = population[i];
            }
        }
        populationFitness = fitnessSum;
        bestRoom = best;
        bestRoomFitness = best.fitness;
    }

    private List<Room> SortPopulation()
    {
        List<Room> sortedPopulation = population.OrderByDescending(o => o.fitness).ToList();
        return sortedPopulation;
    }

    private Room ChooseParent()
    {
        int randomNumber1 = UnityEngine.Random.Range(0, population.Count - 1);
        int randomNumber2 = UnityEngine.Random.Range(0, population.Count - 1);
        int randomNumber3 = UnityEngine.Random.Range(0, population.Count - 1);
        int bestParent = randomNumber1;
        if (population[randomNumber2].fitness > population[bestParent].fitness) bestParent = randomNumber2;
        if (population[randomNumber3].fitness > population[bestParent].fitness) bestParent = randomNumber2;
        return population[bestParent];
    }
}
