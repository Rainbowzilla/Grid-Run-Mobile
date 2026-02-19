using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float forwardSpeed = 5f;
    public float backwardSpeed = 3f;
    public float strafeSpeed = 4f;
    private bool isGrounded;
    bool crouching = false;
    bool sprinting = false;
    bool lerpCrouch = false;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    float crouchTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    //Recieve the input for out InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        // Forward / backward
        if (input.y > 0)
            moveDirection.z = input.y * forwardSpeed;
        else
            moveDirection.z = input.y * backwardSpeed;

        // Strafing
        moveDirection.x = input.x * strafeSpeed;

        // Move character
        controller.Move(transform.TransformDirection(moveDirection) * Time.deltaTime);

        // Gravity (same as before)
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }


    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            forwardSpeed = 8;
        else
            forwardSpeed = 5;
    }
}
/*
    private void CalculateMovement()
    {
    var verticalSpeed = playerSettings.WalkingForwardSpeed * input_Movement.y * Time.deltaTime;
    var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

    var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
    newMovementSpeed = transform.TransformDirection(newMovementSpeed)

        characterController.Move(newMovementSpeed);
    }
*/