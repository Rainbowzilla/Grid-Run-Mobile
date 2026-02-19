using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    public float rotateX, RotateY, RotateZ;

    void Update()
    {
        Rotating();
    }
    void Rotating()
    {
        transform.Rotate(new Vector3(rotateX, RotateY, RotateZ) * Time.deltaTime);
    }
 }
