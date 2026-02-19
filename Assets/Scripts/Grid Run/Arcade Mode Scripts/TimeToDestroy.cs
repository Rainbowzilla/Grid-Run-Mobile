using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDestroy : MonoBehaviour
{
    public float lifeTime = 1;

    void Update()
    {
        StartCoroutine(DestroyGameObject(lifeTime));
    }

    IEnumerator DestroyGameObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
