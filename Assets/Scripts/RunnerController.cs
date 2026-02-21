using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    public float speed = 5.0f;
    public static bool isPlayerDead = false;
    public ParticleSystem explosion;
    private Rigidbody rb;
    public PowerUps Power_Ups;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal;

        if (MobileInputHandler.Instance != null && MobileInputHandler.Instance.isForMobile)
        {
            moveHorizontal = MobileInputHandler.Instance.xVelocity;
        }
        else
        { 
            moveHorizontal = Input.GetAxis("Horizontal");
        }

        rb.linearVelocity = new Vector3(moveHorizontal * (speed * Time.timeScale), 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            isPlayerDead = true;
            gameObject.SetActive(false);
            Debug.Log("The Runner has died");
            explosion.Play();
        }
        if (other.gameObject.tag == "Slow Down")
        {
            Power_Ups.isSlowMotionInEffect = true;
            Power_Ups.slowDownSwoosh.Play();
            Destroy(other.gameObject);
            Debug.Log("Is Slow Motion in effect = " + Power_Ups.isSlowMotionInEffect);
        }
        if (other.gameObject.tag == "Auto")
        {
            Power_Ups._fireModeID = 2;
            Power_Ups.fullAutoPowerUpSound.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Time")
        {
            TimeCountdown.countTimer += Power_Ups.addExtraTime;
            Power_Ups.addTimeDing.Play();
            Destroy(other.gameObject);
            StartCoroutine(DestroyAfterTime(1));
        }
        if (other.gameObject.tag == "Mutiplier")
        {
            GridRunArcadeModeGameManager.isDoubleTime = true;
            Power_Ups.doublePointRise.Play();
            Destroy(other.gameObject);
        }
    }

    IEnumerator DestroyAfterTime(float x)
    {
        Transform child = transform.Find("+3");
        child.gameObject.SetActive(true);
        yield return new WaitForSeconds(x);
        child.gameObject.SetActive(false);
    }
}
