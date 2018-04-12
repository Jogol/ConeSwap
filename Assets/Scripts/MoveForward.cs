using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

    public float thrust;
    Rigidbody rb;

    Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startRotation = rb.transform.rotation;
    }

    void FixedUpdate()
    {
        //rb.AddForce(transform.forward * thrust);
        Vector3 vel = transform.forward * thrust;
        vel.y += rb.velocity.y;
        rb.velocity = vel;
        rb.transform.rotation = startRotation;
    }
}
