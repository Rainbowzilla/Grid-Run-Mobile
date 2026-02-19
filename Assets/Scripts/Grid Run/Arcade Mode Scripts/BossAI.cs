using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Search;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public static Vector3 speed;
    private Rigidbody rb;
    public static float maxSpeed;
    public static float health = 100;
    public static bool isBossDead = false;

    [Header("Gun Settings")]

    public static float bulletSpeed = 30;
    public float lifeTime = 2;
    public static float fireRate = 1f;
    public static float automaticFireDuration;
    public static float coolDownFireDuration;
    private float shootTimer;
    private float delayTimer;
    private float TimeBeforeShooting;
    private bool canShoot = true;
    public static float rotationSpeed = 10f; // Speed of rotation
    private Vector3 targetRotation;  // Target rotation angles (X, Y)

    [Header("Components")]
    public GameObject explosion;
    public GameObject bulletBossPrefab;
    public Transform bulletBossSpawn;
    public ParticleSystem muzzleFlash;
    public AudioSource bulletSound;
    public AudioSource explosionSound;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        shootTimer = automaticFireDuration;
        delayTimer = coolDownFireDuration;
        SetRandomTargetRotation();

        if (explosion == null)
        {
            Debug.Log("Explosion is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!RunnerController.isPlayerDead)
        {
            rb.AddForce(speed);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            if (health <= 0)
            {
                explosion.SetActive(true);
                explosionSound.Play();
                isBossDead = true;
                Destroy(gameObject);
            }

            gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(gun.transform.rotation, Quaternion.Euler(targetRotation)) < 0.1f)
            {
                SetRandomTargetRotation();
            }
            Auto();
            CoolDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.GetComponent<BulletBehaviour>().AddScore(GridRunArcadeModeGameManager.Points_Per_Hit);
            Destroy(other.gameObject);
            health--;
        }
    }

    public void Auto()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer > 0 && canShoot)
        {
            if (TimeBeforeShooting <= 0f)
            {
                GameObject bulletboss = Instantiate(bulletBossPrefab);

                Physics.IgnoreCollision(bulletboss.GetComponent<Collider>(),
                    bulletBossSpawn.parent.GetComponent<Collider>());

                bulletboss.transform.position = bulletBossSpawn.position;

                Vector3 rotationLeft = bulletboss.transform.rotation.eulerAngles;

                bulletboss.transform.rotation = Quaternion.Euler(rotationLeft.x, transform.eulerAngles.y, rotationLeft.z);

                bulletboss.GetComponent<Rigidbody>().AddForce(bulletBossSpawn.forward * bulletSpeed, ForceMode.Impulse);

                StartCoroutine(DestroyBulletAfterTime(bulletboss, lifeTime));

                muzzleFlash.Play();
                bulletSound.Play();

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

    public void CoolDown()
    {

        if (shootTimer <= 0)
        {
            canShoot = false;

            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                shootTimer = automaticFireDuration;
                delayTimer = coolDownFireDuration;
                canShoot = true;
            }
        }
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bulletBoss, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(bulletBoss);
    }

    private void SetRandomTargetRotation()
    {
        // Randomize the X and Y rotation while keeping Z constant (or you can also randomize Z)
        targetRotation = new Vector3(Random.Range(-5f, -8f), Random.Range(-15f, 15f), transform.rotation.eulerAngles.z);
    }
}
