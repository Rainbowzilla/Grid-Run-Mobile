using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR;

public class GameManagerBassHero : MonoBehaviour
{
    public static GameManagerBassHero Instance;
    public int mutiplier = 1;
    public int starPowerMultiplier = 2;
    int noteStreak = 0;
    bool didPlayerLose = false;
    bool canPlayerTurnOnStarMode;
    public bool isPlayerInStarMode = false;
    public static float countTimer;
    public static float starPowerTime;
    public int notesToReachStarPower;
    public int[] starPowerSectionIncrements = new int[3];
    //public int maxIncrementsStarPower;
    public int incrementsStarPower;
    //[SerializeField] private GameObject[] RegisteredObjects;

    int starPowerNoteStreak;
    GameObject rm;

    public int difficultySetting = 0;
    public GameObject easy, medium, hard, expert;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("RockMeter", 50);
        PlayerPrefs.SetInt("NoteStreak", 0);
        PlayerPrefs.SetInt("High Streak", 0);
        PlayerPrefs.SetInt("Multiplier", 1);
        PlayerPrefs.SetInt("NotesHit", 0);
        PlayerPrefs.SetInt("Start", 1);
        PlayerPrefs.SetInt("TotalNotes", 0);
        starPowerTime = 0;

        rm = GameObject.Find("Rock Meter Bar");

        countTimer = starPowerTime;

        difficultySetting = StaticVariableController.difficulty;

        if (difficultySetting == 1)
        {
            easy.SetActive(true);
        }
        if (difficultySetting == 2)
        {
            medium.SetActive(true);
        }
        if (difficultySetting == 3)
        {
            hard.SetActive(true);
        }
        if (difficultySetting == 4)
        {
            expert.SetActive(true);
        }
    }

    void Update()
    {
        StarPower();
    }

    public int GetScore()
    {
        return 100 * mutiplier;
    }

    public int GetStarScore()
    {
        return 100 * mutiplier * starPowerMultiplier;
    }

    public void AddStreak()
    {
        if (PlayerPrefs.GetInt("RockMeter") + 1 <= 100)
            PlayerPrefs.SetInt("RockMeter", PlayerPrefs.GetInt("RockMeter") + mutiplier);
        else if (isPlayerInStarMode)
                PlayerPrefs.SetInt("RockMeter", PlayerPrefs.GetInt("RockMeter") + mutiplier * starPowerMultiplier);

        noteStreak++;
        if (noteStreak >= 24)
            mutiplier = 4;
        else if (noteStreak >= 16)
            mutiplier = 3;
        else if (noteStreak >= 8)
            mutiplier = 2;
        else
            mutiplier = 1;

        if (noteStreak > PlayerPrefs.GetInt("High Streak"))
            PlayerPrefs.SetInt("High Streak" ,noteStreak);

        PlayerPrefs.SetInt("NotesHit", PlayerPrefs.GetInt("NotesHit") + 1);
        AddTotalNotes();
        UpdateUI();
        rm.GetComponent<RockMeterUI>().ChangeFillColor();
    }

    public void AddStarStreak()
    {
        starPowerNoteStreak++;
        if (starPowerNoteStreak >= notesToReachStarPower)
        {
            starPowerTime += starPowerSectionIncrements[0];
            incrementsStarPower++;
            starPowerNoteStreak = 0;
            //canPlayerTurnOnStarMode = true;
        }

        /*if(Activator.starNote.GetComponent<StarNoteID>().ID == 0)
        {
            notesToReachStarPower[0] = 8;
            Debug.Log("This is working cuz the number is: " + notesToReachStarPower[0]);
        }

        if (RegisteredObjects[1].GetComponent<StarNoteID>().ID == 2)
        {
            notesToReachStarPower[0] = 16;
            Debug.Log(notesToReachStarPower);
        }*/

        if (incrementsStarPower >= 2)
        {
            canPlayerTurnOnStarMode = true;
        }
        else if (incrementsStarPower <= 1)
        {
            canPlayerTurnOnStarMode = false;
        }
        if (incrementsStarPower > 4)
        {
            incrementsStarPower = 4;
        }
    }

    public void ResetStreak()
    {
        if (!didPlayerLose)
        {
            PlayerPrefs.SetInt("RockMeter", PlayerPrefs.GetInt("RockMeter") - 2);
            if (PlayerPrefs.GetInt("RockMeter") < 0)
                Lose();
            noteStreak = 0;
            mutiplier = 1;
            UpdateUI();
            starPowerNoteStreak = 0;
            rm.GetComponent<RockMeterUI>().ChangeFillColor();
        }
    }

    void UpdateUI()
    {
        PlayerPrefs.SetInt("NoteStreak", noteStreak);
        PlayerPrefs.SetInt("Multiplier", mutiplier);
        if(isPlayerInStarMode)
            PlayerPrefs.SetInt("Multiplier", mutiplier * starPowerMultiplier);
    }

    public void Lose()
    {
        PlayerPrefs.SetInt("Start", 0);
        Debug.Log("You Lose!");
        SceneManager.LoadScene(11);
        didPlayerLose = true;
    }

    public void Win()
    {
        PlayerPrefs.SetInt("Start", 0);
        SceneManager.LoadScene(10);
        if (PlayerPrefs.GetInt("HighScore") < PlayerPrefs.GetInt("Score"))
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
    }

    public void AddTotalNotes()
    {
        PlayerPrefs.SetInt("TotalNotes", PlayerPrefs.GetInt("TotalNotes") + 1);
    }
    public void goToScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        var currentSceneName = currentScene.name;
        Debug.Log(currentSceneName);
    }

    public void StarPower()
    {
        if (canPlayerTurnOnStarMode && incrementsStarPower >= 2)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canPlayerTurnOnStarMode)
            {
                isPlayerInStarMode = true;
                Debug.Log("Star Power ACTIVATE");
                countTimer = starPowerTime;
            }

            if (isPlayerInStarMode && countTimer >= 0)
            {
                countTimer -= Time.deltaTime; //Starts countdown for Star Power
                Debug.Log(countTimer);

                if (countTimer <= 0)
                {
                    isPlayerInStarMode = false; //Turns off Star Power
                                                //canPlayerTurnOnStarMode = false;
                    countTimer = starPowerTime;
                    starPowerTime = 0;
                    incrementsStarPower = 0;
                }
            }
        }
    }
}
