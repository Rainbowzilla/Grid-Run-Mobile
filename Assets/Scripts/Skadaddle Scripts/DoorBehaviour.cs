using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public AudioSource explosionSound;
    public ParticleSystem explosionEffect_1, explosionEffect_2, explosionEffect_3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball of Death")
        {
            explosionSound.Play();
            explosionEffect_1.Play();
            explosionEffect_2.Play();
            explosionEffect_3.Play();
            Destroy(collision.gameObject);
            StartCoroutine(CameraShakeDuration());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<Transform>().position = new Vector3(0, 1.5f, -262);
        }
    }

    IEnumerator CameraShakeDuration()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraShakeAlt>().enabled = true;

        yield return new WaitForSeconds(1.5f);

        camera.GetComponent<CameraShakeAlt>().enabled = false;
    }
}
