using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeAlt : MonoBehaviour
{
    public float shakeIntensity = 0.02f;
    private Vector3 basePos;

    void LateUpdate()
    {
        basePos = transform.localPosition;
        transform.localPosition = basePos + Random.insideUnitSphere * shakeIntensity;
    }
}
