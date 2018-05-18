using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSpawner : MonoBehaviour {

    public GameObject car;
    public GameObject[] spawnPoints;

    public UnityEvent incrementCarsSpawned;

    public float minWaitTime = 5;
    public float maxWaitTime = 5;

    bool spawnCars;
    // Use this for initialization
    void Start () {
        spawnCars = true;
        StartCoroutine("Spawner");
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SetSpawnCarsFalse()
    {
        spawnCars = false;
    }

    IEnumerator Spawner()
    {
        while (spawnCars)
        {
            float randomSleep = Random.Range(minWaitTime, maxWaitTime);
            //bool boolean = (Random.value > 0.5f);
            int rand = Random.Range(0, spawnPoints.Length);
            Instantiate(car, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
            if (incrementCarsSpawned != null)
                incrementCarsSpawned.Invoke();
            yield return new WaitForSeconds(randomSleep);
        }
    }
}
