using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    public GameObject car;
    public GameObject[] spawnPoints;

    public int minWaitTime = 4;
    public int maxWaitTime = 7;
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
            int rand = Random.Range(0, spawnPoints.Length);
            Instantiate(car, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
            yield return new WaitForSeconds(randomNumber);
        }
    }
}
