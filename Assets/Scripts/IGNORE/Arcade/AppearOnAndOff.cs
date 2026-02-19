using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnAndOff : MonoBehaviour
{
    public GameObject[] gameObjectName;

    private void OnTriggerEnter(Collider other)
    {
        gameObjectName[0].SetActive(true);
        gameObjectName[1].SetActive(true);
        gameObjectName[2].SetActive(true);
        gameObjectName[3].SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObjectName[0].SetActive(false);
        gameObjectName[1].SetActive(false);
        gameObjectName[2].SetActive(false);
        gameObjectName[3].SetActive(false);
    }
}
