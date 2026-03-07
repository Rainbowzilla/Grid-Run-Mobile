using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class JoystickControls : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBackground;
    public Vector2 joystickVector;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joystickOriginalPos = joystickBackground.transform.position;
        joystickRadius = joystickBackground.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBackground.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        joystickVector = (dragPos - joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if(joystickDist < joystickRadius)
        {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickDist;
        }

        else
        {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickRadius;
        }

    }


    public void PointerUp()
    {
        joystickVector = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBackground.transform.position = joystickOriginalPos;
    }


}
