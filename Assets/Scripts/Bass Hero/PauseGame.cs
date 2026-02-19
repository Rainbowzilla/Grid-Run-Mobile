using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    private bool isGamePaused;
    public GameObject pauseMenu;
    string sceneName = "Bass Hero Main Menu";

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Literally what makes the game paused
        {
            isGamePaused = !isGamePaused;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseMenu.SetActive(true);

            if (!isGamePaused)
            {
                Time.timeScale = 1f;
                AudioListener.pause = false;
                pauseMenu.SetActive(false);
            }
        }
    }

    public void ResumeButton()
    {
        isGamePaused = false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
    }

    public void RestartButton(string SceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneName);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene(sceneName);
    }
}
