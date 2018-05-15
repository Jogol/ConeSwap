using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreHolder : MonoBehaviour {

    public GameObject savedQuad1;
    public GameObject savedQuad2;
    public GameObject slashQuad;
    public GameObject spawnedQuad1;
    public GameObject spawnedQuad2;

    public Material mat0;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Material mat5;
    public Material mat6;
    public Material mat7;
    public Material mat8;
    public Material mat9;

    public int maxCarsSpawned;
    public UnityEvent setSpawnCarsFalse;
    
    int carsSpawned;
    int carsSaved;

    

    // Use this for initialization
    void Start () {
        carsSpawned = 0;
        carsSaved = 0;
	}
	
	// Update is called once per frame
	void Update () {
        SetSavedQuad();
        SetSpawnedQuad();

        if (carsSpawned >= maxCarsSpawned)
        {
            if (setSpawnCarsFalse != null)
                setSpawnCarsFalse.Invoke();
        }
	}

    public void IncrementCarsSpawned()
    {
        carsSpawned++;
    }

    public void IncrementCarsSaved()
    {
        carsSaved++;
    }

    void SetSavedQuad()
    {
        

        int secondDigit = Modulo(carsSaved, 10);

        if (carsSaved > 9)
        {
            int firstDigit = carsSaved;

            while (firstDigit >= 10)
                firstDigit /= 10;

            savedQuad1.GetComponent<Renderer>().material = IntToMaterial(firstDigit);
            savedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
        } else
        {
            savedQuad1.GetComponent<Renderer>().material = IntToMaterial(0);
            savedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
        }
        
    }

    void SetSpawnedQuad()
    {
        int secondDigit = Modulo(carsSpawned, 10);

        if (carsSpawned > 9)
        {
            int firstDigit = carsSpawned;

            while (firstDigit >= 10)
                firstDigit /= 10;

            spawnedQuad1.GetComponent<Renderer>().material = IntToMaterial(firstDigit);
            spawnedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
        }
        else
        {
            spawnedQuad1.GetComponent<Renderer>().material = IntToMaterial(0);
            spawnedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
        }
    }

    private int Modulo(int a, int b)
    {
        return a - (int)((double)a / b) * b;
    }

    Material IntToMaterial(int num)
    {
        switch (num)
        {
            case 0:
                return mat0;
            case 1:
                return mat1;
            case 2:
                return mat2;
            case 3:
                return mat3;
            case 4:
                return mat4;
            case 5:
                return mat5;
            case 6:
                return mat6;
            case 7:
                return mat7;
            case 8:
                return mat8;
            case 9:
                return mat9;
            default:
                return null;
        }
    }
}
