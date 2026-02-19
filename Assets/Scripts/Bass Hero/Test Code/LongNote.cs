using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : MonoBehaviour
{
    public static bool canBePressed, canBeReleased;
    GameObject acti;

    void Update()
    {
        if (canBePressed)
        {

        }
    }

    IEnumerator PressingDown()
    {
        yield return new WaitForSeconds(1f);
    }
}
