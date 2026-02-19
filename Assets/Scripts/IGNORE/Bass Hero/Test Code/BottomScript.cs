using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Activator")
        {
            Activator.canBePressed = true;
            Debug.Log("Bottom Script Can BE Pressed = true");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Activator")
        {
            Activator.canBePressed = false;
            Debug.Log("Bottom Script CANNOT BE Pressed");
        }
    }
}
