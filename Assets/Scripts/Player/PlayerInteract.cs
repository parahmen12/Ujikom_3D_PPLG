using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam; // Referensi ke kamera pemain untuk menentukan arah interaksi
    [SerializeField]
    private float distance = 3f; // Jarak maksimum interaksi
    [SerializeField]
    private LayerMask mask; // Layer yang bisa berinteraksi

    private PlayerUI playerUI; // Referensi ke UI pemain untuk menampilkan teks interaksi
    private InputManager inputManager; // Referensi ke Input Manager untuk mendeteksi input dari pemain

    // Start dipanggil saat pertama kali objek aktif di scene
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam; // Ambil kamera dari komponen PlayerLook
        playerUI = GetComponent<PlayerUI>(); // Ambil referensi UI dari komponen PlayerUI
        inputManager = GetComponent<InputManager>(); // Ambil referensi dari InputManager
    }

    // Update dipanggil setiap frame
    void Update()
    {
        // Reset teks interaksi setiap frame agar tidak tersisa jika tidak ada objek di depan
        playerUI.UpdateText(string.Empty);

        // Buat ray (garis tak terlihat) dari posisi kamera ke arah depan
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance); // Menampilkan ray di editor untuk debugging

        RaycastHit hitInfo; // Variabel untuk menyimpan informasi hasil raycast

        // Jika ray mengenai objek dalam jarak "distance" dan sesuai layer "mask"
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // Cek apakah objek yang terkena memiliki skrip "Interactable"
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>(); // Ambil komponen Interactable
                playerUI.UpdateText(interactable.promptMessage); // Tampilkan teks interaksi di UI

                // Jika tombol interaksi ditekan
                if (inputManager.OnFoot.Interact.triggered)
                {
                    interactable.BaseInteract(); // Panggil fungsi interaksi utama dari objek
                }
            }
        }
    }
}
