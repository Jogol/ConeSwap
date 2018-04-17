using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Takes two gameobject and checks which one is closer to the object this script is applied to
public class ClosestNum : MonoBehaviour {

    public GameObject num0;
    public GameObject num1;

    int maximumDistance = 3;
    float minimumDifDistance = 0.1f;
    GameObject closestObject;
    int currentNumber = 0;

	// Use this for initialization
	void Start () {
		if (num0 == null || num1 == null)
        {
            Debug.Log("Error, num0 or num1 not assigned.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        float distTo0 = Vector3.Distance(num0.transform.position, transform.position);
        float distTo1 = Vector3.Distance(num1.transform.position, transform.position);

        if (Mathf.Abs(distTo0 - distTo1) > minimumDifDistance && Mathf.Min(distTo0, distTo1) < maximumDistance)
        {
            if (distTo0 <= distTo1)
            {
                currentNumber = 0;
            } else
            {
                currentNumber = 1;
            }
        }

    }

    public int GetNum ()
    {
        return currentNumber;
    }
}
