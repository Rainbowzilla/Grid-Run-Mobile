using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float pushForce;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody _rbgg = hit.collider.attachedRigidbody;

        if (_rbgg != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            _rbgg.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);
        }
    }
}
