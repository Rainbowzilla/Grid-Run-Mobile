using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeIntensity = 0.02f;
    private Vector3 initialPos;

    void Awake()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = initialPos + Random.insideUnitSphere * shakeIntensity;
    }
}
