using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour {

    public GameObject target;
    public float duration = 1.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, duration);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, duration);
    }
}
