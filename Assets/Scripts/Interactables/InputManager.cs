using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput; // Input system Unity baru
    private PlayerInput.OnFootActions onFoot; // Aksi input untuk pergerakan

    private PlayerMotor motor; // Skrip kontrol pergerakan pemain
    private PlayerLook look; // Skrip kontrol kamera pemain

    // Getter publik agar bisa diakses dari luar (misalnya PlayerInteract.cs)
    public PlayerInput.OnFootActions OnFoot => onFoot;

    void Awake()
    {
        // Inisialisasi PlayerInput dan OnFoot Actions
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        // Ambil referensi komponen pada pemain
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        // Debug jika komponen tidak ditemukan
        if (motor == null) Debug.LogError("PlayerMotor tidak ditemukan pada " + gameObject.name);
        if (look == null) Debug.LogError("PlayerLook tidak ditemukan pada " + gameObject.name);

        // Bind input untuk berbagai aksi
        onFoot.Jump.performed += ctx => motor?.Jump(); // Lompat
        onFoot.Crouch.performed += ctx => motor?.Crouch(); // Crouch
        onFoot.Sprint.performed += ctx => motor?.Sprint(true); // Sprint mulai
        onFoot.Sprint.canceled += ctx => motor?.Sprint(false); // Sprint berhenti
    }

    void FixedUpdate()
    {
        // Panggil fungsi pergerakan pemain berdasarkan input
        motor?.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        // Panggil fungsi untuk menggerakkan kamera berdasarkan input mouse
        look?.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerInput.Enable(); // Aktifkan input saat script aktif
    }

    private void OnDisable()
    {
        playerInput.Disable(); // Nonaktifkan input saat script dinonaktifkan
    }
}
