using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera closeCamera;
    public Camera topCamera;

    private Camera[] cameras;
    private int currentCameraIndex = 0;
    private Camera currentCamera;

    void Start()
    {
        cameras = new Camera[] { mainCamera, closeCamera, topCamera };
        currentCamera = mainCamera;
        ChangeView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentCameraIndex++;
            if (currentCameraIndex > cameras.Length-1)
                currentCameraIndex = 0;
            ChangeView();
        }
    }

    void ChangeView()
    {
        currentCamera.enabled = false;
        currentCamera.GetComponent<AudioListener>().enabled = false;
        currentCamera = cameras[currentCameraIndex];
        currentCamera.enabled = true;
        currentCamera.GetComponent<AudioListener>().enabled = true;
    }
}
