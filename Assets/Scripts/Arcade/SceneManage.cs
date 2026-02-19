using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public bool isTriggered = false;
    public bool skadadale = false;
    public bool scaryMaze = false;
    public bool bassHero = false;
    public bool initalR = false;
    public bool shooter = false;
    public bool gridRun = false;
    public string stringText;
    public Text arcadeText;
    public GameObject arcadeTextGameObject;

    public void Start()
    {
        arcadeTextGameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown("e") && isTriggered && scaryMaze)
        {
            SceneManager.LoadScene(2); //This sends the player to the menu of (Scary Maze)
        }
        if (Input.GetKeyDown("e") && isTriggered && skadadale)
        {
            SceneManager.LoadScene(3); //This sends the player to the menu of (Skadaddle)
        }
        if (Input.GetKeyDown("e") && isTriggered && bassHero)
        {
            SceneManager.LoadScene(9); //This sends the player to the menu of (Bass Hero)
        }
        if (Input.GetKeyDown("e") && isTriggered && gridRun)
        {
            SceneManager.LoadScene(13); //This sends the player to the menu of (Grid Run)
        }
        stringText = gameObject.name; //This sets the text to any arcade machine name when a player goes through it.
    }

    public void SetStringText()
    {
        arcadeText.text = "Press E to play " + stringText;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
         {
            arcadeTextGameObject.SetActive(true);
            isTriggered = true;
            SetStringText();
            print("Player Has Entered The " + gameObject.name + " Trigger Box");
            //This all turns on the UI text when I player is inside the hitbox
         }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTriggered = false;
            arcadeTextGameObject.SetActive(false);
            print("Player Has Exited The " + gameObject.name + " Trigger Box");
            //This all turns off the UI text when I player is inside the hitbox
        }

    }
}
