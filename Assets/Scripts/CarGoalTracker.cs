using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGoalTracker : MonoBehaviour {

    int score = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Car")
        {
            score++;
            Destroy(other.gameObject);
        }
    }

    public int GetScore ()
    {
        return score;
    }
}
