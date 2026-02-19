using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerForGridRun : MonoBehaviour
{
    public AudioClip normalPhaseMusic; // Background music for normal gameplay
    public AudioClip bossPhaseMusic;   // Music for the boss phase

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (StaticVariableController.statusBool6 == true)
        {
            PlayMusic(normalPhaseMusic);
            AudioListener.pause = false;
        }
        else if (StaticVariableController.statusBool6 == false)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        if (RunnerController.isPlayerDead)
            audioSource.Stop();
    }

    // Method to play a specific music clip
    public void PlayMusic(AudioClip musicClip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop current music
        }

        audioSource.clip = musicClip; // Assign the new music clip
        audioSource.Play(); // Play the new clip
    }

    // Call this method when the boss phase starts
    public void StartBossPhase()
    {
        PlayMusic(bossPhaseMusic);
    }
}
