using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackAndFourth : MonoBehaviour
{
    [SerializeField] float distanceToCoverX;
    [SerializeField] float distanceToCoverY;
    [SerializeField] float distanceToCoverZ;
    [SerializeField] float speed;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        Vector3 position = startingPosition;
        position.x += distanceToCoverX * Mathf.Sin(Time.time * speed);
        position.y += distanceToCoverY * Mathf.Sin(Time.time * speed);
        position.z += distanceToCoverZ * Mathf.Sin(Time.time * speed);
        transform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
