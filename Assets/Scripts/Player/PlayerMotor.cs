using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;

    [Header("Player Settings")]
    public float gravity = -9.8f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f; // Kecepatan saat crouch
    public float jumpHeight = 3f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f; // Tinggi saat crouch
    public float standingHeight = 2f; // Tinggi normal
    public float crouchTransitionSpeed = 5f; // Kecepatan transisi crouch

    private float speed;
    private bool isGrounded;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private PlayerEnergy playerEnergy;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerEnergy = GetComponent<PlayerEnergy>();

        if (controller == null)
        {
            Debug.LogError("CharacterController tidak ditemukan! Pastikan komponen ini ada di GameObject.");
        }
        if (playerEnergy == null)
        {
            Debug.LogError("PlayerEnergy tidak ditemukan! Pastikan komponen ini ada di GameObject.");
        }
    }

    void Start()
    {
        speed = walkSpeed;
    }

    void Update()
    {
        isGrounded = controller != null && controller.isGrounded;
        HandleEnergySystem();
    }

    private void HandleEnergySystem()
    {
        if (isSprinting && !isCrouching) // Sprint hanya bisa dilakukan jika tidak crouch
        {
            playerEnergy.KurangiEnergi(10f * Time.deltaTime);

            if (playerEnergy.GetCurrentEnergy() <= 0)
            {
                ForceWalk();
            }
        }
        else
        {
            playerEnergy.TambahEnergi(5f * Time.deltaTime);
        }
    }

    public void Sprint(bool state)
    {
        if (state && !isCrouching && playerEnergy.GetCurrentEnergy() > 0) // Cek apakah sedang crouch
        {
            isSprinting = true;
            speed = sprintSpeed;
        }
        else
        {
            ForceWalk();
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching; // Toggle crouch

        if (isCrouching)
        {
            speed = crouchSpeed;
            StartCoroutine(CrouchTransition(crouchHeight));
        }
        else
        {
            speed = walkSpeed;
            StartCoroutine(CrouchTransition(standingHeight));
        }
    }

    private IEnumerator CrouchTransition(float targetHeight)
    {
        float currentHeight = controller.height;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * crouchTransitionSpeed;
            controller.height = Mathf.Lerp(currentHeight, targetHeight, time);
            yield return null;
        }
    }

    public void ForceWalk()
    {
        isSprinting = false;
        speed = walkSpeed;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public void ProcessMove(Vector2 input)
    {
        if (controller == null)
        {
            Debug.LogError("CharacterController tidak ditemukan! Pastikan komponen ini ada di GameObject.");
            return;
        }

        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded && !isCrouching) // Tidak bisa lompat saat crouch
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }
}
