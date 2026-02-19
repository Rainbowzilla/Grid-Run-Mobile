using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerSkadaddle;

public class BallEnemy : MonoBehaviour
{
    public Vector3 force;
    private Rigidbody rb;
    public float maxSpeed = 4;
    public GameObject player;

    public bool ball;
    public bool spikes;

    public AudioSource crushSound;
    public AudioSource spikeSound;

    public SkadaddlePlayerData playerData;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(force);
        //print("Speed Up");
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerDied();
        }
    }

    void PlayerDied()
    {
            player.GetComponent<PlayerSkadaddle>().enabled = false;
            player.layer = LayerMask.NameToLayer("Wall");
            player.GetComponent<PlayerSkadaddle>().isPlayerDead = true;
            player.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            player.GetComponentInChildren<Animator>().SetBool("isDead", true);
            GameManager.instance.SkadaddlePlayerDied();
            if (ball)
                {
                    crushSound.Play();
                }
            if (spikes)
            {
                spikeSound.Play();
            }

    }

}
