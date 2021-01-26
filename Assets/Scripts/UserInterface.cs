using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    public GameObject generation, population, fitness;
    TextMeshProUGUI generationText, populationText, fitnessText;
    private static UserInterface instance = null;
    public static UserInterface Instance { get { return instance; } }


    public void Awake()
    {
        generationText = generation.GetComponent<TextMeshProUGUI>();
        populationText = population.GetComponent<TextMeshProUGUI>();
        fitnessText = fitness.GetComponent<TextMeshProUGUI>();

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
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
        fitnessText.text = fitness.ToString();
    }
}
