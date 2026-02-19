using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseCanvas;
    public GameObject playerCanvas;
    public GameObject deathCanvas;
    public GameObject player;
    public FadingScript fs;

    void Awake()
    {
        isGamePaused = false;
    }

    void Start()
    {
        fs.FadeIn();
    }
    void Update()
    {
        //image.GetComponent<FadeTransition>().FadeIn();
        PauseGame();
        if (player.GetComponent<PlayerSkadaddle>().isPlayerDead == true)
        {
            StartCoroutine(RespawnDelay());
            //deathCanvas.SetActive(true);
        }
    }
    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.GetComponent<PlayerSkadaddle>().isPlayerDead == false)
        {
            isGamePaused = !isGamePaused;
            print("The Game has been paused!");
        }

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
            playerCanvas.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void RestartGame()
    {
        pauseCanvas.SetActive(false);
        deathCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(3);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Player has Quit the Game! :(");
    }

    IEnumerator RespawnDelay()
    {
        fs.FadeOut();
        yield return new WaitForSeconds(2);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
