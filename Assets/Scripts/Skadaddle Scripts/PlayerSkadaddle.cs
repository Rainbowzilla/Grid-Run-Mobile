using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class PlayerSkadaddle : MonoBehaviour
{
    public class SkadaddlePlayerData
    {
        public int Coins = 0;
        public int Lives = 3;
    }

    public GameObject player;

    public float MovementSpeed = 5.0f;

    private Rigidbody rb;
    //private int count;

    public float jump = 10f;
    public float gravity = -10f;
    public bool isPlayerOnTheGround = true;
    public bool isPlayerInWater = false;

    public int coins = 0;
    public TMP_Text coinText;
    public TMP_Text livesText;

    private float slowSpeed = 3f;

    public AudioSource jumpSound;
    public AudioSource collectSound;

    [HideInInspector] public bool isPlayerDead;

    private Animator m_Animator;

    [SerializeField] private Transform shadowObject;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float maxShadowDistance = 20f;


    void Awake()
    {
        //This will load the Data at the start of the Scene
        SaveSystem.Load(out SkadaddlePlayerData LoadedData);

        coins = LoadedData.Coins;       
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        SetCountText();        
        player.layer = LayerMask.NameToLayer("Default");
        isPlayerDead = false;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Jump();
        ShadowFollow();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isPlayerOnTheGround = true; // Turns on Booleon if the Player is touching the floor
            m_Animator.SetBool("IsGrounded", true);
            //shadowObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Collect(); // Does collect function
            coinText.text = "" + coins;
            GameManager.instance.DebugCheckSave();
        }

        if (other.gameObject.tag == "Life")
        {
            GameManager.instance.AddLives();
            livesText.text = "" + GameManager.instance.saveData.Lives;
            //collectLifeSound.Play();
            //GameManager.instance.DebugCheckSave();
        }

        if (other.gameObject.tag == "Water")
        {
            isPlayerInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            isPlayerInWater = false;
        }
    }

    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

       if (moveDirection.magnitude >= 0.1f)
        {
            rb.MovePosition(rb.position + moveDirection * MovementSpeed * Time.fixedDeltaTime);

            // Rotate player toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));

            // Apply movement
            rb.MovePosition(rb.position + moveDirection * MovementSpeed * Time.fixedDeltaTime);
        }

        bool isMoving = Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f;
        m_Animator.SetBool("isRunning", isMoving);


        if (isPlayerInWater == true)
        {
            MovementSpeed = slowSpeed;
            jump = 11f;
        }
        else if (isPlayerInWater == false)
        {
            MovementSpeed = 5f;
            jump = 14f;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isPlayerOnTheGround)
        {
            isPlayerOnTheGround = false; // Turns off booleon when Player is in the air
            rb.AddForce(new Vector3(0 , jump, 0), ForceMode.Impulse); // Adds force in the air
            jumpSound.Play();
            m_Animator.SetBool("IsGrounded", false);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Impulse);
        }
        if(!isPlayerOnTheGround)
        {
            shadowObject.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else shadowObject.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Collect()
    {
        coins++; // Adds 1 to coin counter
        collectSound.Play();
        //print("Player Collected a Coin!");
    }

    public void SetCountText()
    {
        coinText.text = "" + GameManager.instance.saveData.Coins; // Displays the coin counter in UI
        livesText.text = "" + GameManager.instance.saveData.Lives;
    }

    public void ShadowFollow()
    {
        RaycastHit hit;

        // Raycast straight down from player
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxShadowDistance, groundLayers))
        {
            // Position shadow at hit point
            shadowObject.position = hit.point + Vector3.up * 0.01f; // Slight offset to avoid z-fighting
        }
    }
}

