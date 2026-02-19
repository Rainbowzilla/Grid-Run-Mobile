using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;


    [Header("Movement Settings")]
    public CharacterController characterController;
    public float speed = 6f;
    public float sprintMultiplier = 1f;

    private float gravity = 9.87f;
    private float verticalSpeed = 0;

    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float upLimit = -50f;
    public float downLimit = 50;

    private Rigidbody rb;
    private Vector3 velocity;

    public float jump = 10f;
    public float Gravity = -9.8f;

    //private bool isWalking = false;
    //private bool isJumping = false;



    //[Header("Sounds", order = 1)]
    //public AudioSource walk;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

       /* if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
       */
    }
    void Update()
    {
        Move();
        Rotate();
        Jump();
        Time.timeScale = 1;
    }

    private void Move()
    {
        float sprintValue = 1f;

        if (Input.GetButton("Sprint"))
        {
            sprintValue = sprintMultiplier;
            print("Player is Sprinting");
        }

        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;

        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move * sprintValue);

       // if (horizontalMove != 0 && !isJumping || verticalMove != 0 && !isJumping)
      //  {
      //      isWalking = true;
      //  }
      //  else
      //  {
     //      isWalking = false;
      //  }

    //    if (isWalking && !walk.isPlaying)
    //    {
    //        walk.PlayOneShot(walk.clip);
    //    }
    }
    private void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation * mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && transform.position.y < 5.5f)
        // (-0.5) change this value according to your character y position + 1
        {
            velocity.y = jump;
            print("Space Was Pressed");
            //isJumping = true;
        }
        else
        {
            velocity.y += Gravity * Time.deltaTime;
            //isJumping = false;
        }
        characterController.Move(velocity * Time.deltaTime);
    }
    public void SavePlayer()
    {
        //SaveSystem.SavePlayer(this);
        print("Saved the game!");
    }

    /*public void LoadPlayer()
    {
        // data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1] + 4;
        position.z = data.position[2];
        transform.position = position;

        Time.timeScale = 1;
    }*/
}

