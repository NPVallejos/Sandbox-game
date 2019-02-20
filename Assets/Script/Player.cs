using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    private Vector3 respawnPoint;
    private float jumpCount;
    private float jumpTimer;
    public float walkSpeed;
    public float numJumps;
    public float modifyJumpHeightTimeWindow;
    public float fallOffJumpHeight;
    public float gravity = -20;
    public float jumpForce = 8.5f;
    public float secondJumpModifier;
    public static Vector3 velocity;
    public static bool isGrounded;
    public static bool isFloating;

    Controller2D controller;
    //Builder builder;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        CheckIfGrounded();
        isFloating = false;
        jumpCount = 0;

        respawnPoint = transform.position;
    }

    void Update()
    {
        CheckIfGrounded();
        ManageJump();
        ManageMovement();
        ResetPosition();
    }

    void CheckIfGrounded()
    {
        if (controller.collisions.below)
        {
            isGrounded = true;
            isFloating = false;
            jumpCount = 0;
            jumpTimer = 0;
            velocity.y = 0f;
        }
        else if (controller.collisions.above)
        {
            isGrounded = false;
            velocity.y = 0f; // The player will fall as soon as he collides with the ceiling
        }
        else
        {
            isGrounded = false; // The player is in mid-air
        }
    }

    void ManageJump()
    {
        if (Input.GetKeyDown("w") && controller.collisions.below)
        {
            // So if the player presses 'w' AND the player object is standing on something
            velocity.y = jumpForce;
            jumpCount++;
        }
        else if (Input.GetKeyDown("w") && !controller.collisions.below && jumpCount < numJumps)
        {
            velocity.y = (jumpForce - (jumpCount / jumpForce)) * secondJumpModifier; //* 0.75f;
            jumpCount++;
            jumpTimer = 0;
        }
        jumpTimer += Time.deltaTime;
        if (Input.GetKeyUp("w") && !controller.collisions.below && velocity.y > 0)
        {
            //if(jumpTimer < modifyJumpHeightTimeWindow) {
            velocity.y = fallOffJumpHeight;
            //}
        }
    }

    void ManageMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        velocity.x = input.x * walkSpeed;
        if (!isFloating)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    void ResetPosition()
    {
        if (Input.GetButtonDown("Restart"))
            transform.position = respawnPoint;
    }
}
