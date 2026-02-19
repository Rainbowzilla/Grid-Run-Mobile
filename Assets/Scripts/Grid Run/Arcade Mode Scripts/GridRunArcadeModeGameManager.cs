using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

public class GridRunArcadeModeGameManager : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 20f;
    public float spawnRateForEnemies;
    public float spawnRateForEnemyPattern = 10;
    public float spawnRateForPowerUps;
    private float _countTimerForEnemies;
    private float _countTimerForPowerUps;
    private float _countTimerForEnemyPattern;
    private float _countTimerForDifficultyInterval;
    public float subtractSpawnRate = 0.005f;
    public float increaseSpeedDifficulty = 5f;
    public float difficultyInterval = 10f;
    public static bool isGamePaused;

    [Header("Score Settings")]
    public int pointsPerHit = 100;
    public float doubleTimeDuration;
    private float _countDoubleTime;
    public static bool isDoubleTime = false;
    public static int Points_Per_Hit;
    public static int static_score;
    public static int hiScoreCount;

    [Header("Secret Boss Settings")]
    public int bossHealth;
    public int bossSpeed = 50;
    public float bossMaxSpeed;
    public float bulletSpeed = 30f;
    public float fireRate = 10f;
    public float shootingDuration = 3f;
    public float coolDownDuration = 5f;
    public float rotationGunSpeed = 10f;
    [SerializeField] public static int Minimum_Score_So_Unlock_Boss = 50000;

    [Header("Components")]
    public GameObject gridPrefab;
    public GameObject enemyPrefab;
    public GameObject[] powerUpArray;
    public GameObject[] enemyPatternArray;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    [SerializeField] TextMeshProUGUI countDownText;
    public PowerUps pu;
    public Image doublePoints;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject mainCamera;
    public GameObject explosion;


    void Start()
    {
        Cursor.visible = false;
        RunnerController.isPlayerDead = false;
        ObstacleController.speed = new Vector3 (0,0, -speed - 10);
        ObstacleController.maxSpeed = speed;
        GridController.speed = speed;
        StaticMove.speed = speed;
        BossAI.health = bossHealth;
        BossAI.speed = new Vector3 (0,0, -bossSpeed - 10);
        BossAI.maxSpeed = bossMaxSpeed;
        BossAI.bulletSpeed = bulletSpeed;
        BossAI.fireRate = fireRate;
        BossAI.automaticFireDuration = shootingDuration;
        BossAI.coolDownFireDuration = coolDownDuration;
        BossAI.rotationSpeed = rotationGunSpeed;
        Points_Per_Hit = pointsPerHit;
        _countTimerForEnemies = spawnRateForEnemies;
        _countTimerForPowerUps = spawnRateForPowerUps;
        _countDoubleTime = doubleTimeDuration;
        _countTimerForDifficultyInterval = difficultyInterval;
        static_score = 0;
        UpdateHighScoreText();
        AudioListener.pause = false;
        doublePoints.enabled = false;
        isGamePaused = false;
        pauseCanvas.SetActive(false);
        explosion.SetActive(false);
        gameOverCanvas.SetActive(false);
        isDoubleTime = false;
        if (StaticVariableController.statusBool5 == true)
            mainCamera.GetComponentInChildren<CameraFilterPack_TV_ARCADE>().enabled = true;
        else if (StaticVariableController.statusBool5 == false)
            mainCamera.GetComponentInChildren<CameraFilterPack_TV_ARCADE>().enabled = false;
    }

    void Update()
    {
        Debug.Log("Is Player Dead = " + RunnerController.isPlayerDead);
        if (GridController.didGridCollide == true)
        {
            SpawnNewGrid();
            GridController.didGridCollide = false;
        }

        if (!RunnerController.isPlayerDead)
        {
            SpawnEnemies();
            SpawnPowerUps();
            RandomPrefabSpawner();
            Pause();
            IncreaseDifficulty();
            gameOverCanvas.SetActive(false);
        }
        else if (RunnerController.isPlayerDead)
        {
            pu._fireModeID = 3;
            speed = 0;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            gameOverCanvas.SetActive(true);
            Cursor.visible = true;
            explosion.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene("Grid Run Arcade Mode");
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("Grid Run Main Menu 1");
        }
        CheckScore();

        if (isDoubleTime == true)
        {
            DoubleScore();
            doublePoints.enabled = true;
        }
        else if (isDoubleTime == false)
        {
            Points_Per_Hit = pointsPerHit;
            doublePoints.enabled = false;
        }
    }

    public void SpawnNewGrid()
    {
        Vector3 spawnPosition = new Vector3(0, 3, 220);
        Instantiate(gridPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnEnemies()
    {
        _countTimerForEnemies -= Time.deltaTime;
        
        if (_countTimerForEnemies <= 0)
        {
            Vector3 RandomSpawnPosition = new Vector3(Random.Range(-14, 14), 1, Random.Range(175, 150));
            Quaternion rotatePosition = new Quaternion(0, 180, 0, 0);
            Instantiate(enemyPrefab, RandomSpawnPosition, rotatePosition);
            _countTimerForEnemies = spawnRateForEnemies;
        }
    }

    public void RandomPrefabSpawner()
    {
        _countTimerForEnemyPattern -= Time.deltaTime;

        if (_countTimerForEnemyPattern <= 0)
        {
            int prefabIndex = Random.Range(0, 3);
            Instantiate(enemyPatternArray[prefabIndex], new Vector3(0, 1, 175), Quaternion.identity);
            _countTimerForEnemyPattern = spawnRateForEnemyPattern;
        }
    }

    public void SpawnPowerUps()
    {
        _countTimerForPowerUps -= Time.deltaTime;

        if (_countTimerForPowerUps <= 0)
        {
            int prefabIndex = Random.Range(0, 4);
            Instantiate(powerUpArray[prefabIndex], new Vector3(Random.Range(-14, 14), 1, Random.Range(175, 150)), Quaternion.identity);
            _countTimerForPowerUps = spawnRateForPowerUps;
        }
    }

    public void IncreaseDifficulty()
    {
        _countTimerForDifficultyInterval -= Time.deltaTime;

        if (_countTimerForDifficultyInterval <= 0)
        {
            speed += increaseSpeedDifficulty;
            spawnRateForEnemies -= subtractSpawnRate;
            spawnRateForPowerUps -= subtractSpawnRate;

            if (spawnRateForPowerUps <= 0)
            {
                spawnRateForPowerUps = 0.05f;
            }    

            if (spawnRateForEnemies <= 0)
            {
                spawnRateForEnemies = 0.05f;
            }
            _countTimerForDifficultyInterval = difficultyInterval;
        }
    }

    #region Score_Related_Code

    public void DoubleScore()
    {
        Points_Per_Hit = 200;
        _countDoubleTime -= Time.deltaTime;

        if (_countDoubleTime <= 0)
        {
            isDoubleTime = false;
            _countDoubleTime = doubleTimeDuration;
        }
        Debug.Log(_countDoubleTime);
    }

    public void CheckScore()
    {
        scoreText.text = static_score.ToString();

        if (static_score > PlayerPrefs.GetInt("HighScoreGridRunArcade", 0) && !RunnerController.isPlayerDead)
        {
            PlayerPrefs.SetInt("HighScoreGridRunArcade", static_score);
        }
    }

    public void UpdateHighScoreText()
    {
        hiScoreText.text = $"{PlayerPrefs.GetInt("HighScoreGridRunArcade", 0)}";
    }

    #endregion

    #region Resume/Pause
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameOverCanvas.activeInHierarchy == false)
        {
            isGamePaused = !isGamePaused;
            AudioListener.pause = true;
            pauseCanvas.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = false;

            if (!isGamePaused)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
                pauseCanvas.SetActive(false);
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pauseCanvas.SetActive(false);
        isGamePaused = false;
        Cursor.visible = false;
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f;
        AudioListener.pause = true;
        isGamePaused = false;
        Cursor.visible = true;
    }
    #endregion
}
