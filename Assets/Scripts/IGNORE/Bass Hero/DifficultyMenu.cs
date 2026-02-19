using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    public GameObject difficultyMenu;

    private GameObject FUCKYOUBITCH;

    void Start()
    {
        FUCKYOUBITCH = GameObject.Find("Canvas");
    }

    public void DifficultySetting(string levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void DifficultyNumber(int input)
    {
        StaticVariableController.difficulty = input;
    }

    public void DifficultyMenuz()
    {
        difficultyMenu.SetActive(true);
        FUCKYOUBITCH.GetComponent<MenuBassHero>().CloseAll();
    }

    public void Back()
    {
        FUCKYOUBITCH.GetComponent<MenuBassHero>().BackToMainMenu();
        difficultyMenu.SetActive(false);
    }
}
