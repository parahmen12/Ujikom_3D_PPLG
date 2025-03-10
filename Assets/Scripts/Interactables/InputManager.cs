using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot; // Tetap private untuk enkapsulasi
    private PlayerMotor motor;
    private PlayerLook look;

    // Getter publik agar bisa diakses dari luar (PlayerInteract.cs)
    public PlayerInput.OnFootActions OnFoot => onFoot;

    void Awake()
    {
        // Inisialisasi PlayerInput dan OnFoot
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        // Referensi Komponen
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        if (motor == null) Debug.LogError("PlayerMotor tidak ditemukan pada " + gameObject.name);
        if (look == null) Debug.LogError("PlayerLook tidak ditemukan pada " + gameObject.name);

        // Aksi Lompat
        onFoot.Jump.performed += ctx => motor?.Jump();
        // Aksi Crouch
        onFoot.Crouch.performed += ctx => motor?.Crouch();
        // Aksi Sprint
        onFoot.Sprint.performed += ctx => motor?.Sprint(true);
        onFoot.Sprint.canceled += ctx => motor?.Sprint(false);


        
    }

    void FixedUpdate()
    {
        motor?.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look?.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerInput.Enable(); 
    }

    private void OnDisable()
    {
        playerInput.Disable(); 
    }
}
