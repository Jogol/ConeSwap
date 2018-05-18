using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCounter : MonoBehaviour
{

    public GameObject numQuad1;
    public GameObject numQuad2;
    public GameObject numQuad3;

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

    int timeLeft;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetTimeQuad();

        //if (carsSpawned >= maxCarsSpawned)
        //{
        //    if (setSpawnCarsFalse != null)
        //        setSpawnCarsFalse.Invoke();
        //}
    }

    public void SetTimeLeft(int time)
    {
        timeLeft = time;
    }

    //void SetTimeQuad()
    //{
    //    if (timeLeft > 999)
    //    {
    //        Debug.LogError("Time left too high: " + timeLeft);
    //        return;
    //    }

    //    int firstDigit = timeLeft;
    //    while (firstDigit >= 100)
    //        firstDigit /= 100;

    //    int secondDigit = timeLeft;
    //    while (secondDigit >= 10)
    //        secondDigit /= 10;

    //    int thirdDigit = Modulo(timeLeft, 10);


    //    if (timeLeft < 10)
    //    {
    //        firstDigit = 0;
    //        secondDigit = 0;

    //    } else if (9 < timeLeft && timeLeft < 100)
    //    {
    //        firstDigit = 0;
    //    }

    //    numQuad1.GetComponent<Renderer>().material = IntToMaterial(firstDigit);
    //    numQuad2.GetComponent<Renderer>().material = IntToMaterial(secondDigit);
    //    numQuad3.GetComponent<Renderer>().material = IntToMaterial(thirdDigit);

    //}

    void SetTimeQuad()
    {
        string stringTimeLeft = timeLeft.ToString();

        if (stringTimeLeft.Length == 1)
        {
            stringTimeLeft = "00" + stringTimeLeft;
        } else if (stringTimeLeft.Length == 2)
        {
            stringTimeLeft = "0" + stringTimeLeft;
        }

        int dig1;
        System.Int32.TryParse(stringTimeLeft[0].ToString(), out dig1);
        int dig2;
        System.Int32.TryParse(stringTimeLeft[1].ToString(), out dig2);
        int dig3;
        System.Int32.TryParse(stringTimeLeft[2].ToString(), out dig3);
        numQuad1.GetComponent<Renderer>().material = IntToMaterial(dig1);
        numQuad2.GetComponent<Renderer>().material = IntToMaterial(dig2);
        numQuad3.GetComponent<Renderer>().material = IntToMaterial(dig3);

    }

    void TestTimeQuad(int time)
    {
        string stringTimeLeft = time.ToString();

        if (stringTimeLeft.Length == 1)
        {
            stringTimeLeft = "00" + stringTimeLeft;
        }
        else if (stringTimeLeft.Length == 2)
        {
            stringTimeLeft = "0" + stringTimeLeft;
        }

        Debug.Log(stringTimeLeft[0] + " " + stringTimeLeft[1] + " " + stringTimeLeft[2]);
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
