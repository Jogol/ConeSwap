using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundExplosion : MonoBehaviour {

    public GameObject explosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            Debug.Log("Boom!");
            Instantiate(explosion, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
