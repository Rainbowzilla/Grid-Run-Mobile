using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    [Header("Slow Mo Settings")]

    public float TimeScale = 0.05f;
    public float TimeDelta = 0.02f;

    public float slowMoDuration;
    float countSlowMoTimer, countAutoFireTimer;
    [HideInInspector] public bool isSlowMotionInEffect = false;

    [Header("Add Time Settings")]

    public float addExtraTime = 3f;

    [Header("Gun Settings")]

    public float bulletSpeed = 30;
    public float lifeTime = 3;
    public float fireRate = 1f;
    public float automaticFireDuration;
    private float TimeBeforeShooting;
    [HideInInspector] public int _fireModeID = 1;

    public enum FiringModes
    {
        Semi = 1,
        Auto = 2,
        Nothing = 3
    }

    [HideInInspector] public FiringModes _CurrentFiringMode = FiringModes.Semi;

    [Header("Components")]

    public GameObject bulletPrefabLeft;
    public GameObject bulletPrefabRight;
    public Transform bulletSpawnLeft, bulletSpawnRight;
    public ParticleSystem sparksLeft, sparksRight;
    public GameObject player;
    public Image fullAuto, slowMo;
    public AudioSource laserShot;
    public AudioSource fullAutoPowerUpSound;
    public AudioSource slowDownSwoosh;
    public AudioSource doublePointRise;
    public AudioSource addTimeDing;

    void Start()
    {
        countSlowMoTimer = slowMoDuration;
        countAutoFireTimer = automaticFireDuration;
        _fireModeID = 1;
        slowMo.enabled = false;
        fullAuto.enabled = false;
    }

    void Update()
    {
        DoSlowMotion();
        SwitchFireMode();
    }

    #region Monobehaviour API

    public void DoSlowMotion()
    {
        if (isSlowMotionInEffect)
        {
            Time.timeScale = TimeScale;
            Time.fixedDeltaTime = TimeDelta * Time.timeScale;

            countSlowMoTimer -= Time.deltaTime;

            player.GetComponent<RunnerController>().speed = 75f;

            slowMo.enabled = true;

            laserShot.pitch = 0.75f;

            if (countSlowMoTimer <= 0)
            {
                isSlowMotionInEffect = false;
                slowDownSwoosh.Stop();
                laserShot.pitch = 1;
                countSlowMoTimer = slowMoDuration;
            }
            //Debug.Log(countTimer);
        }
        else
        {
            isSlowMotionInEffect = false;
            player.GetComponent<RunnerController>().speed = 30f;
            slowMo.enabled = false;
        }
        if (GridRunArcadeModeGameManager.isGamePaused)
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0f;
        }
        else if (GridRunArcadeModeGameManager.isGamePaused == false && !isSlowMotionInEffect)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = TimeDelta;
            player.GetComponent<RunnerController>().speed = 30f;
        }
            
    }

    public void SwitchFireMode()
    {
        if (_fireModeID > 3)
        {
            _fireModeID = 1;
        }

        _CurrentFiringMode = (FiringModes)_fireModeID;

        switch (_CurrentFiringMode)
        {
            case FiringModes.Semi:
                Semi();
                fullAuto.enabled = false;
                break;

            case FiringModes.Auto:
                Auto();
                fullAuto.enabled = true;
                break;
        }
    }

    public void Semi()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            sparksLeft.Play();
            sparksRight.Play();
            laserShot.Play();

            GameObject bulletLeft = Instantiate(bulletPrefabLeft);
            GameObject bulletRight = Instantiate(bulletPrefabRight);

            Physics.IgnoreCollision(bulletLeft.GetComponent<Collider>(),
                bulletSpawnLeft.parent.GetComponent<Collider>());

            bulletLeft.transform.position = bulletSpawnLeft.position;

            Vector3 rotationLeft = bulletLeft.transform.rotation.eulerAngles;

            bulletLeft.transform.rotation = Quaternion.Euler(rotationLeft.x, transform.eulerAngles.y, rotationLeft.z);

            bulletLeft.GetComponent<Rigidbody>().AddForce(bulletSpawnLeft.forward * bulletSpeed, ForceMode.Impulse);

            Physics.IgnoreCollision(bulletRight.GetComponent<Collider>(),
                bulletSpawnRight.parent.GetComponent<Collider>());

            bulletRight.transform.position = bulletSpawnRight.position;

            Vector3 rotationRight = bulletRight.transform.rotation.eulerAngles;

            bulletRight.transform.rotation = Quaternion.Euler(rotationRight.x, transform.eulerAngles.y, rotationRight.z);

            bulletRight.GetComponent<Rigidbody>().AddForce(bulletSpawnRight.forward * bulletSpeed, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(bulletLeft, bulletRight, lifeTime));
        }
    }

    public void Auto()
    {
        countAutoFireTimer -= Time.deltaTime;


        if (countAutoFireTimer <= 0)
        {
            _fireModeID = 1;
            countAutoFireTimer = automaticFireDuration;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (TimeBeforeShooting <= 0f)
            {
                GameObject bulletLeft = Instantiate(bulletPrefabLeft);
                GameObject bulletRight = Instantiate(bulletPrefabRight);

                Physics.IgnoreCollision(bulletLeft.GetComponent<Collider>(),
                    bulletSpawnLeft.parent.GetComponent<Collider>());

                bulletLeft.transform.position = bulletSpawnLeft.position;

                Vector3 rotationLeft = bulletLeft.transform.rotation.eulerAngles;

                bulletLeft.transform.rotation = Quaternion.Euler(rotationLeft.x, transform.eulerAngles.y, rotationLeft.z);

                bulletLeft.GetComponent<Rigidbody>().AddForce(bulletSpawnLeft.forward * bulletSpeed, ForceMode.Impulse);

                Physics.IgnoreCollision(bulletRight.GetComponent<Collider>(),
                    bulletSpawnRight.parent.GetComponent<Collider>());

                bulletRight.transform.position = bulletSpawnRight.position;

                Vector3 rotationRight = bulletRight.transform.rotation.eulerAngles;

                bulletRight.transform.rotation = Quaternion.Euler(rotationRight.x, transform.eulerAngles.y, rotationRight.z);

                bulletRight.GetComponent<Rigidbody>().AddForce(bulletSpawnRight.forward * bulletSpeed, ForceMode.Impulse);

                StartCoroutine(DestroyBulletAfterTime(bulletLeft, bulletRight, lifeTime));

                sparksLeft.Play();
                sparksRight.Play();
                laserShot.Play();

                TimeBeforeShooting = 1 / fireRate;
            }
            else
            {
                TimeBeforeShooting -= Time.deltaTime;
            }
        }
        else
        {
            TimeBeforeShooting = 0f;
        }
    }

    public void Nothing()
    {

    }
    private IEnumerator DestroyBulletAfterTime(GameObject bulletLeft, GameObject bulletRight, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bulletLeft);
        Destroy(bulletRight);
    }

    #endregion
}
