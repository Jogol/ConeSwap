using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : MonoBehaviour {

    public GameObject dotCube;
    public GameObject starCube;
    public GameObject triCube;

    int currentDotUp = 0;
    int currentStarUp = 0;
    int currentTriUp = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentDotUp = dotCube.GetComponent<Orientator>().GetNum();
        currentStarUp = starCube.GetComponent<Orientator>().GetNum();
        currentTriUp = triCube.GetComponent<Orientator>().GetNum();
    }
}
