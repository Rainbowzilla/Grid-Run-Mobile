using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeScore : MonoBehaviour
{
    public static int scoreCount;
    private float scoreCountFloat;
    public static int hiScoreCount;
    public int mutiplier;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    int difficultSettingNumber;

    //With a mutiplier of 15, about every 17 seconds will spawn a new prefab

    void Start()
    {
        scoreCount = 0;
        difficultSettingNumber = StaticVariableController.difficulty;
        UpdateHighScoreText();
    }
    // Update is called once per frame
    void Update()
    {
        SetTimeScore();
    }

    void SetTimeScore()
    {
        if (!RunnerController.isPlayerDead)
        {
            scoreCountFloat += mutiplier * Time.deltaTime;
            scoreCount = Mathf.RoundToInt(scoreCountFloat);
            scoreText.text = scoreCount.ToString();
            CheckHighScore();
        }
    }

    void CheckHighScore()
    {
        if (difficultSettingNumber == 0)
        {
            if (scoreCount > PlayerPrefs.GetInt("HighScoreGridRunEasy", 0) && !RunnerController.isPlayerDead )
            {
                PlayerPrefs.SetInt("HighScoreGridRunEasy", scoreCount);
            }
        }
        if (difficultSettingNumber == 1)
        {
            if (scoreCount > PlayerPrefs.GetInt("HighScoreGridRunMedium", 0) && !RunnerController.isPlayerDead)
            {
                PlayerPrefs.SetInt("HighScoreGridRunMedium", scoreCount);
            }
        }
        if (difficultSettingNumber == 2)
        {
            if (scoreCount > PlayerPrefs.GetInt("HighScoreGridRun", 0) && !RunnerController.isPlayerDead)
            {
                PlayerPrefs.SetInt("HighScoreGridRun", scoreCount);
            }
        }
        if (difficultSettingNumber == 3)
        {
            if (scoreCount > PlayerPrefs.GetInt("HighScoreGridRunInsane", 0) && !RunnerController.isPlayerDead)
            {
                PlayerPrefs.SetInt("HighScoreGridRunInsane", scoreCount);
            }
        }
    }

    void UpdateHighScoreText()
    {
        if (difficultSettingNumber == 0)
            hiScoreText.text = $"{PlayerPrefs.GetInt("HighScoreGridRunEasy", 0)}";
        if (difficultSettingNumber == 1)
            hiScoreText.text = $"{PlayerPrefs.GetInt("HighScoreGridRunMedium", 0)}";
        if (difficultSettingNumber == 2)
            hiScoreText.text = $"{PlayerPrefs.GetInt("HighScoreGridRun", 0)}";
        if (difficultSettingNumber == 3)
            hiScoreText.text = $"{PlayerPrefs.GetInt("HighScoreGridRunInsane", 0)}";
    }
}
