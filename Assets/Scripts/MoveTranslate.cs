using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTranslate : MonoBehaviour
{

    void FixedUpdate()
    {
        if (!RunnerController.isPlayerDead)
            gameObject.transform.Translate(0, 0, Time.fixedDeltaTime + 0.5f);
    }
}
