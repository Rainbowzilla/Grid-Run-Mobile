using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotateX, RotateY, RotateZ;

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        transform.Rotate (new Vector3 (rotateX, RotateY ,RotateZ) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
