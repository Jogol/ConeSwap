using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallExplosion : MonoBehaviour {

    public GameObject explosion;
    float startHeight;
    float lastHeight;
    bool goingToExplode = false;
    // Use this for initialization
    void Start () {
        startHeight = transform.position.y;
        lastHeight = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        if ((startHeight - transform.position.y) > 0.4)
            goingToExplode = true;

        float diff = Mathf.Abs(lastHeight - transform.position.y);

        if (diff != 0 && diff < 0.0005 && goingToExplode)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        lastHeight = transform.position.y;
	}
}
