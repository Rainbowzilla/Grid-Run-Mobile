using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBassHero : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject quickPlayMenu;
    public GameObject multiplayerMenu;

    public void QuickPlayMenu()
    {
        quickPlayMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void MultiplayerMenu()
    {

    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        quickPlayMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseAll()
    {
        mainMenu.SetActive(false);
        quickPlayMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
    }
}
