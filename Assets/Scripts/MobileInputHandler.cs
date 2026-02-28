using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobileInputHandler : MonoBehaviour
{
    public static MobileInputHandler Instance;

    public float tiltSpeed = 5f; // Adjust this value to control the sensitivity of the tilt

    public float maxXVelocity;

    [HideInInspector] public float xVelocity;

    public bool isForMobile;

    //public float minXForCameraTilt;

    public Transform bikeParent;
    public float tiltMultiplier;

    public TextMeshProUGUI debugText;

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
        Input.gyro.enabled = true;
    }

    void Update()
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

        HandleBikeLean();
    }

    void HandleBikeLean()
    {
        debugText.SetText("X Veloctiy: " + xVelocity);

        //bikeParent.eulerAngles = new Vector3(xVelocity * tiltMultiplier, xVelocity * tiltMultiplier, xVelocity * tiltMultiplier);
        bikeParent.Rotate(xVelocity * tiltMultiplier, xVelocity * tiltMultiplier, xVelocity * tiltMultiplier);
    }
}