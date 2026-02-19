using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Activator : MonoBehaviour
{
    MeshRenderer mr;
    public KeyCode key;
    bool isActive = false;
    GameObject note, longNote, starNote, gameManager;
    Color old;
    public bool createMode;
    public GameObject prefabNote;
    public int playerScore;
    AudioSource missedNoteSound;
    private ParticleSystem noteExplosion;

    public static bool canBePressed;

    public bool ifStarNoteIsHere = false;

    //  public float oldRed = 0, oldGreen = 0, oldBlue = 0;
    //  public float newRed = 0, newGreen = 0, newBlue = 0;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        old = mr.material.color;
        gameManager = GameObject.Find("Game Manager");
        missedNoteSound = GetComponent<AudioSource>();
        noteExplosion = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        ParticleSystem.MainModule ne = noteExplosion.main;

        if (GameManagerBassHero.Instance.isPlayerInStarMode == true)
            ne.startColor = Color.cyan;
        else if (GameManagerBassHero.Instance.isPlayerInStarMode == false)
            ne.startColor = old;

        if (createMode)
        {
            if (Input.GetKeyDown(key))
            {
                Instantiate(prefabNote, transform.position, transform.rotation = Quaternion.Euler(0,90,90));
            }
        }
        else
        {
            if (Input.GetKeyDown(key))
            {
                StartCoroutine(PressedDown());
            }
            if (Input.GetKeyDown(key) && isActive && note)
            {
                Destroy(note);
                gameManager.GetComponent<GameManagerBassHero>().AddStreak();
                AddScore();
                isActive = false;
                noteExplosion.Play();
            }
            else if(Input.GetKeyDown(key) && ifStarNoteIsHere && starNote)
            {
                Destroy(starNote);
                gameManager.GetComponent<GameManagerBassHero>().AddStreak();
                gameManager.GetComponent<GameManagerBassHero>().AddStarStreak();
                AddScore();
                isActive = false;
                ifStarNoteIsHere = false;
                noteExplosion.Play();
                //Debug.Log("You are a sack of shit");
            }
            else if (Input.GetKeyDown(key) && !isActive || Input.GetKeyDown(key) && !ifStarNoteIsHere)
            {
                gameManager.GetComponent<GameManagerBassHero>().ResetStreak();
                missedNoteSound.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isActive = true;
        ifStarNoteIsHere = true;
        if (other.gameObject.tag == "Note")
        {
            note = other.gameObject;
            canBePressed = true;
        }
        if(other.gameObject.tag == "WinNote")
        {
            gameManager.GetComponent<GameManagerBassHero>().Win();
        }
        if(other.gameObject.tag == "StarNote" && other.gameObject.GetComponent<StarNoteID>().ID == 0)
        {
            starNote = other.gameObject;
            canBePressed = true;
        }
        if (other.gameObject.tag == "StarNote" && other.gameObject.GetComponent<StarNoteID>().ID == 1)
        {
            starNote = other.gameObject;
            canBePressed = true;
            GameManagerBassHero.Instance.notesToReachStarPower = 8;
            Debug.Log(GameManagerBassHero.starPowerTime);
        }
        if (other.gameObject.tag == "StarNote" && other.gameObject.GetComponent<StarNoteID>().ID == 2)
        {
            starNote = other.gameObject;
            canBePressed = true;
            GameManagerBassHero.Instance.notesToReachStarPower = 14;
            Debug.Log(GameManagerBassHero.starPowerTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LongNote" && Input.GetKey(key) && canBePressed)
        {
            longNote = other.gameObject;
            mr.material.color = new Color(0, 0, 0);
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 10);

            var main = noteExplosion.main;
            main.loop = true;
        }
        if (other.gameObject.tag == "LongNote" && Input.GetKeyUp(key))
        {
            longNote = other.gameObject;
            longNote.gameObject.tag = "Untagged";
            longNote.GetComponent<MeshRenderer>().material.color = Color.gray;
            mr.material.color = old;

            var main = noteExplosion.main;
            main.loop = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Note")
        {
            isActive = false;
            canBePressed = false;
            gameManager.GetComponent<GameManagerBassHero>().ResetStreak();
            gameManager.GetComponent<GameManagerBassHero>().AddTotalNotes();
            missedNoteSound.Play();
        }
        if (other.gameObject.tag == "LongNote")
        {
            isActive = false;
            mr.material.color = old;
            Destroy(longNote);

            var main = noteExplosion.main;
            main.loop = false;
        }
        if(other.gameObject.tag == "StarNote")
        {
            isActive = false;
            ifStarNoteIsHere = false;
            canBePressed = false;
            gameManager.GetComponent<GameManagerBassHero>().ResetStreak();
            gameManager.GetComponent<GameManagerBassHero>().AddTotalNotes();
            missedNoteSound.Play();
        }
    }

    IEnumerator PressedDown()
    {
        mr.material.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(0.05f);
        mr.material.color = old;
    }

    void AddScore()
    {
        if(GameManagerBassHero.Instance.isPlayerInStarMode == false)
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + gameManager.GetComponent<GameManagerBassHero>().GetScore());
        }
        if (GameManagerBassHero.Instance.isPlayerInStarMode == true)
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + gameManager.GetComponent<GameManagerBassHero>().GetStarScore());
            Debug.Log("Star Power has been acivated");
        }
    } 
}
