using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSpawner : MonoBehaviour {

    public GameObject car;
    public GameObject[] spawnPoints;
    public TextMesh result;
    public UnityEvent incrementCarsSpawned;

    public float minWaitTime = 5;
    public float maxWaitTime = 5;

    public int maxCarsSpawned = 3;

    int carsSpawned = 0;
    bool spawnCars = true;
    // Use this for initialization
    void Start () {
        spawnCars = true;
        StartCoroutine("Spawner");
    }
	
	// Update is called once per frame
	void Update () {
        if (carsSpawned == maxCarsSpawned)
        {
            spawnCars = false;
            result.text = "Last car!";
        }

        //If the last car is gone
        if (!spawnCars && GameObject.FindGameObjectsWithTag("Car").Length == 0)
        {
            result.text = "Game Over!";
        }
            
	}

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(5);
        while (spawnCars)
        {
            float randomSleep = Random.Range(minWaitTime, maxWaitTime);
            //bool boolean = (Random.value > 0.5f);
            int rand = Random.Range(0, spawnPoints.Length);
            Instantiate(car, spawnPoints[rand].transform.position, spawnPoints[rand].transform.rotation);
            carsSpawned++;
            if (incrementCarsSpawned != null)
                incrementCarsSpawned.Invoke();
            yield return new WaitForSeconds(randomSleep);
        }
    }
}
