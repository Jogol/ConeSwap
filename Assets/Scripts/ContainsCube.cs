using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainsCube : MonoBehaviour {

    bool containsDotCube = false;
    bool containsStarCube = false;
    bool containsTriCube = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "DotCube")
        {
            containsDotCube = true;
        } else if (col.gameObject.tag == "StarCube")
        {
            containsStarCube = true;
        } else if (col.gameObject.tag == "TriCube")
        {
            containsTriCube = true;
        } else
        {
            Debug.LogError("Something weird entered!");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "DotCube")
        {
            containsDotCube = false;
        }
        else if (col.gameObject.tag == "StarCube")
        {
            containsStarCube = false;
        }
        else if (col.gameObject.tag == "TriCube")
        {
            containsTriCube = false;
        }
        else
        {
            Debug.LogError("Something weird exited!");
        }
    }

    public bool ContainsDotCube()
    {
        return containsDotCube;
    }

    public bool ContainsStarCube()
    {
        return containsStarCube;
    }

    public bool ContainsTriCube()
    {
        return containsTriCube;
    }
}
