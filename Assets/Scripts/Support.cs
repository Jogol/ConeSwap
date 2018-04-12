using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour {

    string cone1 = "Cone1";
    string cone2 = "Cone2";
    string cone3 = "Cone3";
    string cone4 = "Cone4";
    string cone5 = "Cone5";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool tagIsCone(string tag)
    {
        if (tag == "Cone1" || tag == "Cone2" || tag == "Cone3" || tag == "Cone4" || tag == "Cone5")
        {
            return true;
        }

        return false;
    }

    public int getConeNumberFromTag(string tag)
    {
        switch (tag)
        {
            case "Cone1":
                return 1;
            case "Cone2":
                return 2;
            case "Cone3":
                return 3;
            case "Cone4":
                return 4;
            case "Cone5":
                return 5;
            default:
                return -1;
        }
    }

    public bool IsSortedAscending(int[] arr)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] > arr[i])
            {
                return false;
            }
        }
        return true;
    }

    public bool IsSortedDescending(int[] arr)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] < arr[i])
            {
                return false;
            }
        }
        return true;
    }

    public void DebugLogArray(int[] arr)
    {
        string output = "";
        for (int i = 0; i < arr.Length; i++)
        {
            output = output + arr[i] + " ";
        }
        Debug.Log(output);
    }
}
