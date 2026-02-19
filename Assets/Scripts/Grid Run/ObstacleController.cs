using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public static Vector3 speed;
    private Rigidbody rb;
    public static float maxSpeed;
    public ParticleSystem explosion;
    public float health = 1;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (explosion == null)
        {
            Debug.Log("Explosion is missing!");
        }
    }

    void Update()
    {
        if (!RunnerController.isPlayerDead)
        {
            rb.AddForce(speed);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else if (RunnerController.isPlayerDead)
            rb.velocity = Vector3.zero;

        if (health <= 0)
        {
            Instantiate(explosion, transform.position = new Vector3 (gameObject.transform.position.x, 1, gameObject.transform.position.z), new Quaternion (-90, 0, 0, 0), null);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.GetComponent<BulletBehaviour>().AddScore(GridRunArcadeModeGameManager.Points_Per_Hit);
            Destroy(other.gameObject);
            health--;
        }
    }
}
