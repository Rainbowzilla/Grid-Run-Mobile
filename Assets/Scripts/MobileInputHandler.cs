using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // Required for Unity UI Toggle
using TMPro;

public class MobileInputHandler : MonoBehaviour
{
    public static MobileInputHandler Instance;

    //[Header("Mode Settings")]
    //[SerializeField] private bool startInTiltMode = false;  // Initial state – can be changed in Inspector

    [HideInInspector] public bool isTiltMode;

    [Header("Physics / Movement")]
    public float tiltSpeed;
    public float maxXVelocity;
    [HideInInspector] public float xVelocity;
    public bool isForMobile;
    public Transform bikeParent;
    public Transform bikeParent2;
    public float tiltMultiplier;

    [Header("UI - Tilt Mode Toggle")]
    //[SerializeField] private Toggle tiltModeToggle;         // ← Drag your UI Toggle here in Inspector

    [Header("Touch/Drag Controls")]
    public float dragSensitivity = 2.0f;
    public float dragReleaseDamping = 7.5f;

    // Private drag state
    private Vector2 dragStartPosition;
    private bool isDragging;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set initial mode
        //isTiltMode = startInTiltMode;
    }

    void Start()
    {
        if(PlayerPrefs.GetInt("TiltMode", 1) == 1)
        {
            isTiltMode = true;
        }
        else
        {
            isTiltMode = false;
        }

        tiltMultiplier = PlayerPrefs.GetFloat("TiltSensitivity", 13f);
        dragSensitivity = PlayerPrefs.GetFloat("DragSensitivity", 0.35f);
        // Sync toggle UI with starting mode

        // Enable gyroscope only if starting in tilt mode
        Input.gyro.enabled = isTiltMode;
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

    // Called whenever the UI Toggle changes state
    private void OnTiltModeToggleChanged(bool isOn)
    {
        isTiltMode = isOn;
        Input.gyro.enabled = isOn;

        // Reset velocity when switching modes (prevents sudden jumps)
        xVelocity = 0f;

        Debug.Log($"Control mode switched to: {(isOn ? "Tilt / Gyro" : "Touch + Drag")}");
    }

    void HandleTiltControls()
    {
        // Using acceleration.x is usually better for left/right bike steering than gyro attitude
        xVelocity = Input.acceleration.x * tiltSpeed;
        xVelocity = Mathf.Clamp(xVelocity, -maxXVelocity, maxXVelocity);
    }

    void HandleTouchAndDragControls()
    {
        Vector2 currentPos = Vector2.zero;
        bool pointerDown = false;

        if (Input.touchCount > 0)
        {
            currentPos = Input.GetTouch(0).position;
            pointerDown = true;
        }
        else if (Input.GetMouseButton(0)) // mouse support for editor testing
        {
            currentPos = Input.mousePosition;
            pointerDown = true;
        }

        if (pointerDown)
        {
            if (!isDragging)
            {
                isDragging = true;
                dragStartPosition = currentPos;
            }

            float horizontalDragDistance = currentPos.x - dragStartPosition.x;
            float normalizedDrag = horizontalDragDistance / Screen.width;
            xVelocity = normalizedDrag * dragSensitivity * maxXVelocity;
            xVelocity = Mathf.Clamp(xVelocity, -maxXVelocity, maxXVelocity);
        }
        else
        {
            if (isDragging)
            {
                isDragging = false;
            }

            // Smooth return to zero
            xVelocity = Mathf.MoveTowards(
                xVelocity,
                0f,
                dragReleaseDamping * Time.deltaTime * 55f
            );
        }
    }

    void HandleBikeLean()
    {
        float targetZ = xVelocity * tiltMultiplier;
        targetZ = Mathf.Clamp(targetZ, -35f, 35f);

        float t = 10f * Time.deltaTime;
        t = Mathf.Clamp01(t);

        Quaternion current = bikeParent.localRotation;
        Quaternion target = Quaternion.Euler(targetZ, 90f, 0f);
        bikeParent.localRotation = Quaternion.Lerp(current, target, t);

        Quaternion current2 = bikeParent2.localRotation;
        Quaternion target2 = Quaternion.Euler(0f, 0f, -targetZ);
        bikeParent2.localRotation = Quaternion.Lerp(current2, target2, t);
    }

    // Optional: public method to force mode change from other scripts
    // public void SetTiltMode(bool enableTilt)
    // {
    //     if (tiltModeToggle != null)
    //     {
    //         tiltModeToggle.isOn = enableTilt;
    //     }
    //     else
    //     {
    //         isTiltMode = enableTilt;
    //         Input.gyro.enabled = enableTilt;
    //         xVelocity = 0f;
    //     }
    // }
}