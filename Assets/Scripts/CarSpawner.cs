using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    public GameObject car;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;

    int minWaitTime = 4;
    int maxWaitTime = 7;
    // Use this for initialization
    void Start () {
        StartCoroutine("Spawner");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Spawner()
    {
        while (true)
        {
            int randomNumber = Random.Range(minWaitTime, maxWaitTime);
            //bool boolean = (Random.value > 0.5f);

            if (Random.value > 0.5f)
            {
                Instantiate(car, spawnPoint1.transform.position, spawnPoint1.transform.rotation);
            } else
            {
                Instantiate(car, spawnPoint2.transform.position, spawnPoint2.transform.rotation);
            }
            yield return new WaitForSeconds(randomNumber);
        }
    }
}
