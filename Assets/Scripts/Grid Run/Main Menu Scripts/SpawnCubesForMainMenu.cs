using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubesForMainMenu : MonoBehaviour
{
    public GameObject[] obstaclePrefabArray;
    private float _countTimer;
    private float spawnRate = .08f;
    public Vector3 cubeSpeed = new Vector3 (0, 0, -18);
    public float maxCubeSpeed = 18;


    // Start is called before the first frame update
    void Start()
    {
        AudioListener.pause = false;
        _countTimer = spawnRate;
        ObstacleController.speed = cubeSpeed;
        ObstacleController.maxSpeed = maxCubeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        _countTimer -= Time.deltaTime;

        if (_countTimer <= 0)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-14, 14), 1, Random.Range(150, 125));
            Quaternion newRoation = new Quaternion(0, 90, 0, 0);
            int obstaclePrefabIndex = Random.Range(0, 3);
            Instantiate(obstaclePrefabArray[obstaclePrefabIndex], randomSpawnPosition, newRoation);
            _countTimer = spawnRate;
        }
    }
}
