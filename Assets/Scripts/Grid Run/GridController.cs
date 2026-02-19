using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //private Rigidbody rb;
    public static float speed;
    public static bool didGridCollide = false;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        {
            if (!RunnerController.isPlayerDead)
            {
                /*rb.AddForce(ObstacleController.speed);

                if (rb.velocity.magnitude > ObstacleController.maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * ObstacleController.maxSpeed;
                }
                */
                transform.position += new Vector3(0, 0, -speed) * Time.deltaTime;
            }
            //else if (RunnerController.isPlayerDead)
                //rb.velocity = Vector3.zero;
                //transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            didGridCollide = true;
            Destroy(gameObject);
        }
    }
}
