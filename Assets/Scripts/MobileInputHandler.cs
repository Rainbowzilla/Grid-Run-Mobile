using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MobileInputHandler : MonoBehaviour
{
    public static MobileInputHandler Instance;

    public float tiltSpeed; // Adjust this value to control the sensitivity of the tilt

    public float maxXVelocity;

    [HideInInspector] public float xVelocity;

    public bool isForMobile;

    public Transform bikeParent;
    public float tiltMultiplier;

    public bool isTiltMode;

    //public Slider tiltSpeedSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Enable gyroscope
        if (isTiltMode)
        { 
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        if (isTiltMode)
        {
            HandleTiltControls();
        }
        else
        { 
            HandleTouchAndDragControls();
        }


        HandleBikeLean();
    }

    void HandleTiltControls()
    {
        // Get the tilt angle around the x-axis (pitch)
        float tiltAngle = Input.gyro.attitude.eulerAngles.x;

        // Adjust velocity based on tilt
        xVelocity = Input.acceleration.x * tiltSpeed;

        if (xVelocity > maxXVelocity)
        {
            xVelocity = maxXVelocity;
        }
        else if (xVelocity < -maxXVelocity)
        {
            xVelocity = -maxXVelocity;
        }
    }

    void HandleTouchAndDragControls()
    {
        //TODO Grok, fill this in. Instead affecting a rigidbody, simply add or subtract from xVelocity
    }

    void HandleBikeLean()
    {

        float targetZ = xVelocity * tiltMultiplier;
        targetZ = Mathf.Clamp(targetZ, -35f, 35f);

        float t = 10f * Time.deltaTime;          // 10 = speed factor
        t = Mathf.Clamp01(t);                    // prevent overshoot if framerate very low

        Quaternion current = bikeParent.localRotation;
        Quaternion target = Quaternion.Euler(targetZ, 90f, 0f);

        bikeParent.localRotation = Quaternion.Lerp(current, target, t);

        //bikeParent.eulerAngles = new Vector3(xVelocity * tiltMultiplier, xVelocity * tiltMultiplier, xVelocity * tiltMultiplier);
        //bikeParent.Rotate(xVelocity * tiltMultiplier, xVelocity * tiltMultiplier, xVelocity * tiltMultiplier);
    }
}