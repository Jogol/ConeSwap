using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryManager : MonoBehaviour {

    public TextMesh current;
    public TextMesh target;
    public TextMesh result;
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;

    ClosestNum num1;
    ClosestNum num2;
    ClosestNum num3;
    ClosestNum num4;
    ClosestNum num5;

    int targetNum;
    int displayFrames = 0;
    // Use this for initialization
    void Start () {
        num1 = box1.GetComponent<ClosestNum>();
        num2 = box2.GetComponent<ClosestNum>();
        num3 = box3.GetComponent<ClosestNum>();
        num4 = box4.GetComponent<ClosestNum>();
        num5 = box5.GetComponent<ClosestNum>();

        targetNum = GetRand();
        target.text = targetNum.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (displayFrames == 0)
        {
            result.text = "";
        }

        string numbers = "" + num1.GetNum() + num2.GetNum() + num3.GetNum() + num4.GetNum() + num5.GetNum();
        int b = Convert.ToInt32(numbers, 2);
        current.text = b.ToString();
        if (targetNum == b)
        {
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
        return UnityEngine.Random.Range(0, 32);
    }
}
