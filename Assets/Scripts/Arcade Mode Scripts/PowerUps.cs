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
    float countSlowMoTimer, countAutoFireTimer, countGhostTimer, countAPRoundsTimer;
    [HideInInspector] public bool isSlowMotionInEffect = false;

    [Header("Add Time Settings")]
    public float addExtraTime = 3f;

    [Header("Ghost Settings")]
    public float ghostDuration = 10f;
    [HideInInspector] public bool isGhostActivated = false;
    public Color originalColor;
    public Color ghostColor;
    public float transistionFadeDuration = 3f;
    [HideInInspector] public float elapsedLerpTimer = 0f;

    [Header("AP Rounds Setting")]
    public float AP_Rounds_Duration = 5f;
    public Color apRoundColor;
    public Color bulletColor;
    private Gradient apRoundTrail;
    private Gradient originalBulletTrail;
    public static bool isAPRoundsActive = false;

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
    public Material bodyMaterial;
    public Image fullAuto, slowMo, ghost, apRounds, fullAutoPortrait, slowMoPortrait, ghostPortrait, apRoundsPortrait;
    public AudioSource laserShot;
    public AudioSource laserShotHigherPitch;
    public AudioSource fullAutoPowerUpSound;
    public AudioSource slowDownSwoosh;
    public AudioSource doublePointRise;
    public AudioSource addTimeDing;
    public AudioSource ghostWhoosh;
    public AudioSource apRoundsClangSound;


    void Start()
    {
        countSlowMoTimer = slowMoDuration;
        countAutoFireTimer = automaticFireDuration;
        countGhostTimer = ghostDuration;
        countAPRoundsTimer = AP_Rounds_Duration;
        _fireModeID = 1;
        slowMo.enabled = false;
        fullAuto.enabled = false;
        ghost.enabled = false;
        apRounds.enabled = false;
        isGhostActivated = false;
        bodyMaterial.color = originalColor;

        apRoundTrail = new Gradient();
        originalBulletTrail = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(apRoundColor, 0.0f);
        colorKeys[1] = new GradientColorKey(apRoundColor, 0.6f);
        colorKeys[2] = new GradientColorKey(Color.white, 1.0f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(1.0f, 0.0f);

        apRoundTrail.SetKeys(colorKeys, alphaKeys);

        GradientColorKey[] originalColorKeys = new GradientColorKey[3];
        originalColorKeys[0] = new GradientColorKey(originalColor, 0.0f);
        originalColorKeys[1] = new GradientColorKey(originalColor, 0.6f);
        originalColorKeys[2] = new GradientColorKey(Color.white, 1.0f);

        GradientAlphaKey[] originalAlphaKeys = new GradientAlphaKey[2];
        originalAlphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
        originalAlphaKeys[1] = new GradientAlphaKey(1.0f, 0.0f);

        originalBulletTrail.SetKeys(originalColorKeys, originalAlphaKeys);

        bulletPrefabLeft.GetComponent<TrailRenderer>().colorGradient = originalBulletTrail;
    }

    void Update()
    {
        DoSlowMotion();
        SwitchFireMode();
        DoGhostPowerUp();
        ActivateAPRounds();
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
            laserShotHigherPitch.pitch = 0.75f;

            if (countSlowMoTimer <= 0)
            {
                isSlowMotionInEffect = false;
                slowDownSwoosh.Stop();
                laserShot.pitch = 1;
                laserShotHigherPitch.pitch = 1;
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
                if (GridRunArcadeModeGameManager.isInLandscapeMode)
                    fullAuto.enabled = false;
                else if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                    fullAutoPortrait.enabled = false;
                break;

            case FiringModes.Auto:
                Auto();
                if (GridRunArcadeModeGameManager.isInLandscapeMode)
                    fullAuto.enabled = true;
                else if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                    fullAutoPortrait.enabled = true;
                break;
        }
    }

    public void Semi()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            sparksLeft.Play();
            sparksRight.Play();
            if (isAPRoundsActive)
                laserShotHigherPitch.Play();
            else
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

                if (isAPRoundsActive)
                    laserShotHigherPitch.Play();
                else
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

    public void DoGhostPowerUp()
    {
        if (isGhostActivated)
        {
            countGhostTimer -= Time.deltaTime; //Counts for how long the duration will last
            elapsedLerpTimer += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedLerpTimer / transistionFadeDuration); //Transistion Fade Duration is based on how long the fade will take instead of fade speed

            bodyMaterial.color = Color.Lerp(originalColor, ghostColor, t);

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true); //This one line makes the player invincible by ignoring collision

            if (GridRunArcadeModeGameManager.isInLandscapeMode)
                ghost.enabled = true;
            if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                ghostPortrait.enabled = true;

            if (countGhostTimer <= 0)
            {
                isGhostActivated = false;
                countGhostTimer = ghostDuration;
                bodyMaterial.color = originalColor;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            }
        }
        else
        {
            if (GridRunArcadeModeGameManager.isInLandscapeMode)
                ghost.enabled = false;
            if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                ghostPortrait.enabled = false;
        }
    }

    public void ActivateAPRounds()
    {
        if (isAPRoundsActive)
        {
            countAPRoundsTimer -= Time.deltaTime;

            bulletPrefabLeft.GetComponent<TrailRenderer>().colorGradient = apRoundTrail;

            if (GridRunArcadeModeGameManager.isInLandscapeMode)
                apRounds.enabled = true;
            if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                apRoundsPortrait.enabled = true;

            if (countAPRoundsTimer <= 0)
            {
                countAPRoundsTimer = AP_Rounds_Duration;
                bulletPrefabLeft.GetComponent<TrailRenderer>().colorGradient = originalBulletTrail;
                isAPRoundsActive = false;
            }
        }
        else
        {
            if (GridRunArcadeModeGameManager.isInLandscapeMode)
                apRounds.enabled = false;
            if (!GridRunArcadeModeGameManager.isInLandscapeMode)
                apRoundsPortrait.enabled = false;
        }
    }

    #endregion
}
