using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticMove : MonoBehaviour
{
    public Vector3 force;
    public static float speed;
    private Rigidbody rb;
    public float maxSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeed = speed;
        force = new Vector3(0, 0, -speed);
    }

    void FixedUpdate()
    {
        if (!RunnerController.isPlayerDead)
        {
            rb.AddForce(force);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else rb.velocity = Vector3.zero;
    }
}
