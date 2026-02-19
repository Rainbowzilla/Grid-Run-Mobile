using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayerTrigger : MonoBehaviour
{
    public bool slowPlayer;
    public bool normalizePlayer;
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && slowPlayer)
        {
            SlowPlayerDown();
        }
        if (other.gameObject.tag == "Player" && normalizePlayer)
        {
            NormalizePlayerSpeed();
        }
    }

    void SlowPlayerDown()
    {
        player.GetComponent<Rigidbody>().drag = 1;
    }
    void NormalizePlayerSpeed()
    {
        player.GetComponent<Rigidbody>().drag = 0;
    }
}
