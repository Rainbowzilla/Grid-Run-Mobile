using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLean : MonoBehaviour
{
    public Animator cameraAnim, bikeAnim;
    public bool animState;


    void Update()
    {
        if (MobileInputHandler.Instance.isForMobile)
        {
            HandleMobileInput();
        }
        else
        { 
            HandleKeyPresses();
        }
    }

    void HandleKeyPresses()
    {
        if (animState)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                cameraAnim.ResetTrigger("idle");
                cameraAnim.ResetTrigger("right");
                cameraAnim.SetTrigger("left");
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                cameraAnim.ResetTrigger("idle");
                cameraAnim.ResetTrigger("left");
                cameraAnim.SetTrigger("right");
            }
            else
            {
                cameraAnim.ResetTrigger("right");
                cameraAnim.ResetTrigger("left");
                cameraAnim.SetTrigger("idle");
            }
        }
        if (!animState)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                bikeAnim.ResetTrigger("idle");
                bikeAnim.ResetTrigger("right");
                bikeAnim.SetTrigger("left");
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                bikeAnim.ResetTrigger("idle");
                bikeAnim.ResetTrigger("left");
                bikeAnim.SetTrigger("right");
            }
            else
            {
                bikeAnim.ResetTrigger("right");
                bikeAnim.ResetTrigger("left");
                bikeAnim.SetTrigger("idle");
            }
        }
    }

    void HandleMobileInput()
    {
        if (animState)
        {
            if (MobileInputHandler.Instance.xVelocity < MobileInputHandler.Instance.minXForCameraTilt)
            {
                cameraAnim.ResetTrigger("idle");
                cameraAnim.ResetTrigger("right");
                cameraAnim.SetTrigger("left");
            }
            else if (MobileInputHandler.Instance.xVelocity > MobileInputHandler.Instance.minXForCameraTilt)
            {
                cameraAnim.ResetTrigger("idle");
                cameraAnim.ResetTrigger("left");
                cameraAnim.SetTrigger("right");
            }
            else
            {
                cameraAnim.ResetTrigger("right");
                cameraAnim.ResetTrigger("left");
                cameraAnim.SetTrigger("idle");
            }
        }
        if (!animState)
        {
            if (MobileInputHandler.Instance.xVelocity < MobileInputHandler.Instance.minXForCameraTilt)
            {
                bikeAnim.ResetTrigger("idle");
                bikeAnim.ResetTrigger("right");
                bikeAnim.SetTrigger("left");
            }
            else if (MobileInputHandler.Instance.xVelocity > MobileInputHandler.Instance.minXForCameraTilt)
            {
                bikeAnim.ResetTrigger("idle");
                bikeAnim.ResetTrigger("left");
                bikeAnim.SetTrigger("right");
            }
            else
            {
                bikeAnim.ResetTrigger("right");
                bikeAnim.ResetTrigger("left");
                bikeAnim.SetTrigger("idle");
            }
        }
    }
}
