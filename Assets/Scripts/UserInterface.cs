using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    public GameObject generation, population, fitness;
    TextMeshProUGUI generationText, populationText, fitnessText;
    void Start()
    {
        generationText = generation.GetComponent<TextMeshProUGUI>();
        populationText = population.GetComponent<TextMeshProUGUI>();
        fitnessText = fitness.GetComponent<TextMeshProUGUI>();
    }

    public void SetGeneration(int generation)
    {
        generationText.text = generation.ToString();
    }

    public void SetPopulation(int population)
    {
        populationText.text = population.ToString();
    }

    public void SetFitness(float fitness)
    {
        populationText.text = fitness.ToString();
    }
}
