using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinaryManager : MonoBehaviour {

    public TextMesh current;
    public TextMesh target;
    public TextMesh result;
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;

    public GameObject timeCounter;

    public int playTime = 300; //5 minutes

    public string path = "";

    public UnityEvent incrementScore;

    ClosestNum num1;
    ClosestNum num2;
    ClosestNum num3;
    ClosestNum num4;

    int score = 0;
    List<float> clearTimes;
    float lastClearTime;
    float timerStartTime = 0;
    float timeLeft;
    float timeSinceStartTime = 0;

    int targetNum;
    int displayFrames = 0;

    
    // Use this for initialization
    void Start () {
        num1 = box1.GetComponent<ClosestNum>();
        num2 = box2.GetComponent<ClosestNum>();
        num3 = box3.GetComponent<ClosestNum>();
        num4 = box4.GetComponent<ClosestNum>();

        targetNum = GetRand();
        target.text = targetNum.ToString();

        clearTimes = new List<float>();

        if (path.Equals(""))
        {
            path = "Assets/Resources/Binary/" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (timerStartTime != 0)
        {
            timeSinceStartTime = Time.time - timerStartTime;
        }
        timeLeft = Mathf.Max(playTime - (timeSinceStartTime), 0); //Stops at 0
        timeCounter.GetComponent<TimeCounter>().SetTimeLeft((int)timeLeft);
        if (displayFrames == 0)
        {
            result.text = "";
        }

        if (timeLeft < 1)
        {
            result.text = "Times up!";
        }

        string stringNumber = "" + num1.GetNum() + num2.GetNum() + num3.GetNum() + num4.GetNum();
        int number = Convert.ToInt32(stringNumber, 2);
        current.text = number.ToString();
        if (targetNum == number)
        {
            if (score == 0)
            {
                //Start timing now
                lastClearTime = Time.time;
                timerStartTime = lastClearTime;
            }
            else
            {
                float currentTime = Time.time;
                float diff = currentTime - lastClearTime;
                clearTimes.Add(diff);
                WriteString(diff.ToString());
                //Debug.Log(currentTime - lastClearTime);
                lastClearTime = currentTime;

            }

            score++;
            incrementScore.Invoke();
            result.text = "Correct!";
            displayFrames = 200;
            targetNum = GetRand();
            target.text = targetNum.ToString();
        } else
        {
            displayFrames--;
        }
	}

    int GetRand()
    {
        return UnityEngine.Random.Range(0, 16);
    }

    void WriteString(string line)
    {
        if (timeLeft == 0)
            return;
        //Write some text to the test.txt file
        System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true);
        writer.WriteLine(line);
        writer.Close();
    }

    private int Modulo(int a, int b)
    {
        return a - (int)((double)a / b) * b;
    }

    int SecondsLeft(int time)
    {
        return Modulo(time, 60);
    }

    int MinutesLeft(float time)
    {
        return (int) time / 60;
    }
}
