using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{

    public GameObject savedQuad1;
    public GameObject savedQuad2;

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

    int score;



    // Use this for initialization
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SetSavedQuad();

        //if (carsSpawned >= maxCarsSpawned)
        //{
        //    if (setSpawnCarsFalse != null)
        //        setSpawnCarsFalse.Invoke();
        //}
    }

    public void IncrementScore()
    {
        score++;
    }

    void SetSavedQuad()
    {


        int secondDigit = Modulo(score, 10);

        if (score > 9)
        {
            int firstDigit = score;

            while (firstDigit >= 10)
                firstDigit /= 10;

            savedQuad1.GetComponent<Renderer>().material = IntToMaterial(firstDigit);
            savedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
        }
        else
        {
            savedQuad1.GetComponent<Renderer>().material = IntToMaterial(0);
            savedQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
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
