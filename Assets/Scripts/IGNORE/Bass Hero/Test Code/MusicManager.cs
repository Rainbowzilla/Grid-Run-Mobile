using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    bool isStarting;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Start") == 1 && !isStarting)
        {
            GetComponent<AudioSource>().Play();
            isStarting = true;
        }
    }
}
