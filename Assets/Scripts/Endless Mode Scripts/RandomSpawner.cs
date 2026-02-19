using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabArray;
    public GameObject gridPrefab;
    public GameObject gateV_ShapePrefab;
    public GameObject gridSpawnPoint;
    public float xAxisSpawn, zAxisSpawn;

    private float _countTimer;
    public float spawnRate; //Easy Mode: .15 //Hard Mode: .05
    public Vector3 cubeSpeed;
    public float maxCubeSpeed;
    public float gridSpeed;
    private PauseGame pg;
    public GameObject gameOverCanvas, scoreCanvas;
    private bool isGateVSpawning, isObstacleSpawning;
    public float zGateSpawnPosition;
    private float standInVar = 0;
    public float SpawnEventPerXScore;
    public GameObject[] gameObjectPrefab_Array;
    public float difficultySpike;
    public AudioSource TV_Head;
    public GameObject explosionSound;
    private float spawnRateCap = 0.0001f;
    public GameObject cameraShaker, player, playerParent;
    GameObject gridBike;

    int difficultySettingNumber;

    void Start()
    {
        _countTimer = spawnRate;
        ObstacleController.speed = cubeSpeed;
        ObstacleController.maxSpeed = maxCubeSpeed;
        GridController.speed = gridSpeed;
        RunnerController.isPlayerDead = false;
        gameOverCanvas.SetActive(false);
        isGateVSpawning = false;
        isObstacleSpawning = true;
        standInVar = standInVar + SpawnEventPerXScore;
        explosionSound.SetActive(false);
        pg = GetComponent<PauseGame>();
        gridBike = GameObject.Find("Grid Bike");
        SetDifficulty();
        CheckStartForStatus();
        gridSpeed = maxCubeSpeed;
        //I'm slowly going insane coding this piece of trash code
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!RunnerController.isPlayerDead)
        {
            SpawnObstacles();
            SpawnTimer();

            if (GridController.didGridCollide == true)
            {
                SpawnNewGrid();
                GridController.didGridCollide = false;
            }
        }
        else if (RunnerController.isPlayerDead)
        {
            pg.enabled = false;
            gameOverCanvas.SetActive(true);
            scoreCanvas.SetActive(true);
            TV_Head.Stop();
            explosionSound.SetActive(true);
            StartCoroutine(ViolentlyShakeUponDeath());
        }
    }
    void Update()
    {
        if (RunnerController.isPlayerDead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Grid Run");
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Grid Run Main Menu 1");
            }
        }
    }
    public void SpawnObstacles()
    {
        if (isObstacleSpawning)
        {
            _countTimer -= Time.deltaTime;

            if (_countTimer <= 0)
            {
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-xAxisSpawn, xAxisSpawn), 1, Random.Range(zAxisSpawn, zAxisSpawn - 25));
                int obstaclePrefabIndex = Random.Range(0, 3);
                Instantiate(obstaclePrefabArray[obstaclePrefabIndex], randomSpawnPosition, Quaternion.identity);
                _countTimer = spawnRate;
            }
        }
    }

    public void SpawnTimer()
    {
        if (TimeScore.scoreCount >= standInVar && !isGateVSpawning)
        {
            isGateVSpawning = true;
            RandomPrefabSpawner();
            TimeScore.scoreCount = 0;
        }
    }
    public void RandomPrefabSpawner()
    {
        int prefabIndex = Random.Range(0, 4);
        Instantiate(gameObjectPrefab_Array[prefabIndex], new Vector3(0, 1, zGateSpawnPosition), Quaternion.identity);
        Debug.Log("Gameprefab " + prefabIndex + " has been spawned");

        standInVar = standInVar + SpawnEventPerXScore;
        ObstacleController.speed = cubeSpeed;
        cubeSpeed = cubeSpeed + new Vector3(0, 0, -difficultySpike);
        maxCubeSpeed = maxCubeSpeed + difficultySpike;
        gridSpeed = maxCubeSpeed;
        spawnRate = spawnRate - 0.005f;
        isGateVSpawning = false;
        Debug.Log("Obstacle Cube's current speed: " + cubeSpeed + " / Spawn Rate: " + spawnRate);
        if (spawnRate < spawnRateCap)
        {
            spawnRate = spawnRateCap;
        }
        StartCoroutine(WaitForObstacleToSpawnAfterPrefab());
    }
    IEnumerator WaitForObstacleToSpawnAfterPrefab()
    {
        isObstacleSpawning = false;
        yield return new WaitForSeconds(2f);
        isObstacleSpawning = true;
    }

    public void SpawnNewGrid()
    {
        Vector3 spawnPosition = new Vector3(gridSpawnPoint.transform.position.x, gridSpawnPoint.transform.position.y, gridSpawnPoint.transform.position.z);
        Instantiate(gridPrefab, spawnPosition, Quaternion.identity);
    }

    void SetDifficulty()
    {
        difficultySettingNumber = StaticVariableController.difficulty;

        if (difficultySettingNumber == 0)
        {
            spawnRate = 0.15f;
            difficultySpike = 10f;
            cubeSpeed += new Vector3(0, 0, -25);
            maxCubeSpeed = 25f;
        }
        if (difficultySettingNumber == 1)
        {
            spawnRate = 0.095f;
            difficultySpike = 20;
            cubeSpeed += new Vector3(0, 0, -35);
            maxCubeSpeed = 35f;
        }
        if (difficultySettingNumber == 2)
        {
            spawnRate = 0.05f;
            difficultySpike = 30;
            cubeSpeed += new Vector3(0, 0, -50);
            maxCubeSpeed = 50f;
        }
        if (difficultySettingNumber == 3)
        {
            spawnRate = 0.035f;
            difficultySpike = 50;
            cubeSpeed += new Vector3(0, 0, -75);
            maxCubeSpeed = 150f;
        }
    }

    public void CheckStartForStatus()
    {
        //Status Bool 1 checks Score Canvas
        if (StaticVariableController.statusBool1)
            scoreCanvas.SetActive(true);
        else if (StaticVariableController.statusBool1 == false)
            scoreCanvas.SetActive(false);

        //Status Bool 2 checks Camera is Shaking
        if (StaticVariableController.statusBool2)
            cameraShaker.GetComponentInChildren<CameraShake>().enabled = true;
        else if (StaticVariableController.statusBool2 == false)
            cameraShaker.GetComponentInChildren<CameraShake>().enabled = false;

        //Status Bool 3 checks if player choose Smooth or Jittery Camera
        if (StaticVariableController.statusBool3 == true)
        {
            playerParent.GetComponent<Animator>().enabled = false;
            player.GetComponent<CameraLean>().enabled = true;
            cameraShaker.GetComponent<CameraFollow>().tiltAngle = 0;
            cameraShaker.GetComponent<CameraFollow>().rotationSpeed = 0;
            cameraShaker.GetComponent<CameraLean>().enabled = true;
            cameraShaker.GetComponent<Animator>().enabled = true;
            player.GetComponentInChildren<GridBikeBehaviour>().tiltAngle = 0;
            player.GetComponentInChildren<GridBikeBehaviour>().rotationSpeed = 0;
        }
        else if (StaticVariableController.statusBool3 == false)
        {
            playerParent.GetComponent<Animator>().enabled = true;
            player.GetComponent<Animator>().enabled = false;
            cameraShaker.GetComponent<CameraFollow>().tiltAngle = 15f;
            cameraShaker.GetComponent<CameraFollow>().rotationSpeed = 10f;
            cameraShaker.GetComponent<CameraLean>().enabled = false;
            cameraShaker.GetComponent<Animator>().enabled = false;
            player.GetComponentInChildren<GridBikeBehaviour>().tiltAngle = 15f;
            player.GetComponentInChildren<GridBikeBehaviour>().rotationSpeed = 10f;
            player.GetComponent<CameraLean>().enabled = false;
        }

        //Status Bool 4 sets First or Third Person Camera
        if (StaticVariableController.statusBool4 == true)
        {
            cameraShaker.GetComponent<Transform>().position = new Vector3(0, 3, -4);
            cameraShaker.GetComponent<Transform>().rotation = new Quaternion(CameraFollow.xRotationOnCameraFollow = 15, 0, 0, 0);
            cameraShaker.GetComponent<Animator>().SetTrigger("play");
            gridBike.SetActive(true);
        }
        else if (StaticVariableController.statusBool4 == false)
        {
            cameraShaker.GetComponent<Transform>().position = new Vector3(0, 1, 0);
            cameraShaker.GetComponent<Transform>().rotation = new Quaternion(CameraFollow.xRotationOnCameraFollow = 0, 0, 0, 0);
            cameraShaker.GetComponent<Animator>().SetTrigger("play zero");
            gridBike.SetActive(false);
        }

        //Status Bool 5 checks Camera Filter
        if (StaticVariableController.statusBool5 == true)
            cameraShaker.GetComponentInChildren<CameraFilterPack_TV_ARCADE>().enabled = true;
        else if (StaticVariableController.statusBool5 == false)
            cameraShaker.GetComponentInChildren<CameraFilterPack_TV_ARCADE>().enabled = false;

        //Status Bool 6 checks Music
        if (StaticVariableController.statusBool6 == true)
        {
            TV_Head.Play();
            AudioListener.pause = false;
        }
        else if (StaticVariableController.statusBool6 == false)
        {
            TV_Head.Pause();
            AudioListener.pause = true;
        }
    }

    IEnumerator ViolentlyShakeUponDeath()
    {
        cameraShaker.GetComponentInChildren<CameraShake>().shakeIntensity = 1f;
        yield return new WaitForSeconds(.25f);
        cameraShaker.GetComponentInChildren<CameraShake>().shakeIntensity = 0;
        cameraShaker.GetComponentInChildren<CameraShake>().enabled = false;
    }
}
