using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float sensitivity = 0.5f;
    private Vector2 turn;

    public void Look(Vector2 input)
    {
        turn += input * sensitivity;

        turn.y = Mathf.Clamp(turn.y, -90, 90);
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
