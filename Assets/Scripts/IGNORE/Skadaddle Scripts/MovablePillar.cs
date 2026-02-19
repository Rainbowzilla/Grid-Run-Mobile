using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePillar : MonoBehaviour
{
    public Vector3 force;
   // private Vector3 originalPosition;
    private Rigidbody rb;
    public float maxSpeed = 4;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
       // originalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void FixedUpdate()
    {
        rb.AddForce(force);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "End")
        {
            //gameObject.transform.position = originalPosition;
            Destroy(this.gameObject);
        }
    }
}
