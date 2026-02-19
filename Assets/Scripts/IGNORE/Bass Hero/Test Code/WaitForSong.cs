using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitForSong : MonoBehaviour
{
    public AudioSource music;

    public float countTimer, countTime;

    [SerializeField] TextMeshProUGUI countDownText;

    public bool isPlaying = false;

    // Use this for initialization
    void Start()
    {
        countTimer = countTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            countTimer -= Time.deltaTime;
            countDownText.text = countTimer.ToString("0");
        }
        if (countTimer <= 0 && !isPlaying)
        {
            isPlaying = true;
            countDownText.enabled = false;
            music.Play();
            countTimer = 0;
        }
    }
}
