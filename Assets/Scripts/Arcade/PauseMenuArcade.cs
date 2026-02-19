using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuArcade : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseCanvas;
    public GameObject player;
    public PlayerController playerController;

    void Awake()
    {
        isGamePaused = false;
    }

    void Update()
    {
        PauseGame();
    }
    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.activeInHierarchy == true)
        {
            isGamePaused = !isGamePaused;
            print("The Game has been paused!");
        }

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
            playerController.enabled = false;
        }
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        playerController.enabled = true;
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Player has Quit the Game! :(");
    }
}

