using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerSkadaddle;
using static UnityEngine.GraphicsBuffer;

public class TriggerManager : MonoBehaviour
{
    public GameObject _object;
    public GameObject shutDoor;
    public FadingScript fs;
    public bool activateObject;
    public bool deactivateObject;
    public bool boulderActivate;
    public bool shutDoorEvent;
    public bool cameraStopFollowEvent;
    public bool finishGame;
    [Header ("Type Scene Name To Load")]
    public string sceneName = "Default";
    [Header("Camera Fixed Position")]
    public Vector3 cameraTransformPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && activateObject)
        {
            ActivateObject();
        }
        if (other.gameObject.tag == "Player" && deactivateObject)
        {
            DeactivateObject();
        }
        if (other.gameObject.tag == "Player" && boulderActivate)
        {
            ActivateBallOfDeath();
        }
        if (other.gameObject.tag == "Player" && shutDoorEvent)
        {
            ShutDownDoor();
        }
        if (other.gameObject.tag == "Player" && cameraStopFollowEvent)
        {
            GameObject camera = GameObject.Find("Main Camera");
            camera.GetComponent<CameraSmoothFollow>().enabled = false;
            camera.GetComponent<Transform>().position = cameraTransformPosition;
        }
        if (other.gameObject.tag == "Player" && finishGame)
        {
            UnlockNewLevel();
            GameManager.instance.saveData.Coins = other.gameObject.GetComponent<PlayerSkadaddle>().coins;
            GameManager.instance.SaveGame();
            StartCoroutine(RespawnDelay());
        }
    }

    public void ActivateObject()
    {
        _object.SetActive(true);
            print("Player activated ActivateObject Trigger");
    }

    public void DeactivateObject()
    {
        _object.SetActive(false);
        print("Player activated DeactivateObject Trigger");
    }

    public void FinishGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Unlocked Level", PlayerPrefs.GetInt("Unlocked Level", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void ActivateBallOfDeath()
    {
        _object.GetComponent<BallEnemy>().force = new Vector3(0, 0, -25);
        _object.GetComponent<Rigidbody>().isKinematic = false;
        _object.GetComponentInChildren<AudioSource>().Play();
        _object.GetComponentInChildren<Rotate>().enabled = true;
    }

    public void ShutDownDoor()
    {
        shutDoor.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;

        StartCoroutine(FreezeDoor(shutDoor, 1.5f));
    }

    private IEnumerator FreezeDoor(GameObject door, float delay)
    {
        yield return new WaitForSeconds(delay);

        door.GetComponent<Rigidbody>().isKinematic = true;
    }

    IEnumerator RespawnDelay()
    {
        fs.FadeOut();
        yield return new WaitForSeconds(2);
        FinishGame();
    }
}
