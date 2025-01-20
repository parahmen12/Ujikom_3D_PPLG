using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    void Awake()
    {
        // Inisialisasi PlayerInput dan OnFoot
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        // Referensi Komponen
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        // Aksi Lompat
        onFoot.Jump.performed += ctx => motor.Jump();

        // Aksi Crouch
        onFoot.Crouch.performed += ctx => motor.Crouch();
        // Aksi Sprint
        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    void FixedUpdate()
    {
        // Gerakan pemain
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        // Gerakan kamera
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        // Aktifkan kontrol
        onFoot.Enable();
    }

    private void OnDisable()
    {
        // Nonaktifkan kontrol
        onFoot.Disable();
    }
}
