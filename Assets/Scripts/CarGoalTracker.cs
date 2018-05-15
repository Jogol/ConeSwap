using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarGoalTracker : MonoBehaviour {

    public UnityEvent incrementCarsSaved;

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
            if (incrementCarsSaved != null)
                incrementCarsSaved.Invoke();
            Destroy(other.gameObject);
        }
    }
}
