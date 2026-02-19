using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public float tiltAngle;
    public float rotationSpeed;
    public static float xRotationOnCameraFollow = 15f;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        Tilt();
        //transform.localRotation = Quaternion.Euler(0, 0, z);

        /*if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 0, z) * Time.deltaTime);
            isCameraTitled = true;
        }
        if (z >= maxTilt)
        {
            z = 15;
        }
        */
    }

    public void Tilt()
    {
        float rotZ = -Input.GetAxis("Horizontal") * tiltAngle;

        Quaternion finalRot = Quaternion.Euler(xRotationOnCameraFollow, 0, rotZ);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, finalRot, rotationSpeed);
    }
}
