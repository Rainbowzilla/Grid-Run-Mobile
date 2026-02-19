using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    //public AudioSource clickSound;
    public GameObject levelSelectCanvas;
    public GameObject mainMenuCanvas;
    public GameObject controlCanvas;

    public Button[] buttons;
    public GameObject levelButtons;

    void Update()
    {
        Time.timeScale = 1;
    }

    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("Unlocked Level", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void LevelSelectMenu()
    {
        mainMenuCanvas.SetActive(false);
        levelSelectCanvas.SetActive(true);
        //clickSound.Play();
    }

    public void ControlMenu()
    {
        mainMenuCanvas.SetActive(false);
        controlCanvas.SetActive(true);
        //clickSound.Play();
    }

    public void returnToMainMenu()
    {
        controlCanvas.SetActive(false);
        levelSelectCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        //SaveLoadPosition.instance.LoadPosition();
        //clickSound.Play();
    }

    public void returnToMainHub()
    {
        SceneManager.LoadScene(1);
        //clickSound.Play();
    }

    public void LoadLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    public void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).GetComponent<Button>();
        }
    }
}
