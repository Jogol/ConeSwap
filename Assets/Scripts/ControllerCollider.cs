using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCollider : MonoBehaviour {

    GameObject collidingCone;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cone1" || other.tag == "Cone2" || other.tag == "Cone3" || other.tag == "Cone4" || other.tag == "Cone5")
        {
            collidingCone = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        collidingCone = null;
    }

    public GameObject GetColliding()
    {
        return collidingCone;
    }
}
