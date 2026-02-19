using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject bike, cube;

    void Update()
    {
        if (GridRunMenu.menuID == 0 || GridRunMenu.menuID == 1)
        {
            bike.SetActive(false);
            cube.SetActive(true);
        }
        else if (GridRunMenu.menuID == 2)
        {
            bike.SetActive(true);
            cube.SetActive(false);
        }
    }
}
