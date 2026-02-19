using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBikeBehaviour : MonoBehaviour
{
    public GameObject tire;
    public float tireRotationSpeed = 90f;
    public float tiltAngle;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        tire.transform.Rotate(new Vector3(0, 0, tireRotationSpeed) * Time.deltaTime);
        Tilt();
    }

    public void Tilt()
    {
        float rotX = Input.GetAxis("Horizontal") * tiltAngle;

        Quaternion finalRot = Quaternion.Euler(rotX, 90, 0);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, finalRot, rotationSpeed * Time.timeScale);
    }
}
