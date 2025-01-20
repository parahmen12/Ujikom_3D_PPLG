using System.Collections;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Player Settings")]
    public float gravity = -9.8f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 3f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchDuration = 1f; // Durasi interpolasi crouch

    private float speed;
    private bool isGrounded;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private bool sprinting = false;
    private float crouchTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = walkSpeed; // Default kecepatan jalan
    }

    void Update()
    {
        // Periksa apakah karakter menyentuh tanah
        isGrounded = controller.isGrounded;

        // Logika interpolasi crouch
        HandleCrouchTransition();
    }

    private void HandleCrouchTransition()
    {
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float progress = crouchTimer / crouchDuration;
            progress *= progress; // Membuat transisi lebih halus

            if (crouching)
                controller.height = Mathf.Lerp(controller.height, crouchHeight, progress);
            else
                controller.height = Mathf.Lerp(controller.height, standingHeight, progress);

            if (progress >= 1f)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        lerpCrouch = true;
        crouchTimer = 0f;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        speed = sprinting ? sprintSpeed : walkSpeed; 
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        // Gerakan horizontal
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        // Logika gravitasi
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; 
        }

        playerVelocity.y += gravity * Time.deltaTime; 
        controller.Move(playerVelocity * Time.deltaTime); 
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity); // Rumus lompat
        }
    }
}
