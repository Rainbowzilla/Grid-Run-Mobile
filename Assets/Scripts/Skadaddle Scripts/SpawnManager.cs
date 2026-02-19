using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject objectPrefab;
    public float x, y, z;
    public float spawnRate, spawnTime;
    private float countTimer, countTime;

    // Start is called before the first frame update
    void Start()
    {
        countTime = spawnTime;
        countTimer = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        countTime -= Time.deltaTime;
        if (countTime <= 0)
        {
            StartTime();
        }
    }
    void StartTime()
    {
        countTimer -= Time.deltaTime;
        if (countTimer <= 0)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 SpawnPosition = new Vector3(x, y, z);
        Instantiate(objectPrefab, SpawnPosition, Quaternion.identity);
        countTimer = spawnRate;
    }
}
