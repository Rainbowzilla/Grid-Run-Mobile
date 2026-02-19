using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToDeath : MonoBehaviour
{
    public GameObject player;
    public CameraSmoothFollow csf1;
    public CameraSmoothFollow csf2;
    public CameraScript cs;
    public PlayerSkadaddle ps;
    public AudioSource fallDeathSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            csf1.enabled = false;
            csf2.enabled = false;
            cs.enabled = false;
            ps.enabled = false;
            fallDeathSound.Play();
            print("Player fell to death");
        }
    }
}
