using UnityEngine;

// PlayerLook mewarisi dari PlayerMotor (kemungkinan untuk menggunakan fitur pergerakan dari PlayerMotor)
public class PlayerLook : PlayerMotor
{
    public Camera cam; // Referensi ke kamera untuk mengontrol pandangan pemain
    public float xRotation = 0f; // Rotasi vertikal kamera

    public float xSensitivity = 30f; // Sensitivitas gerakan horizontal (sumbu X)
    public float ySensitivity = 30f; // Sensitivitas gerakan vertikal (sumbu Y)

    // Fungsi untuk memproses pergerakan kamera berdasarkan input mouse
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x; // Ambil input gerakan mouse ke kiri/kanan
        float mouseY = input.y; // Ambil input gerakan mouse ke atas/bawah

        // Mengatur rotasi vertikal kamera berdasarkan pergerakan mouse
        xRotation += (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Batasi rotasi agar kepala tidak berputar 360 derajat

        // Terapkan rotasi ke kamera (hanya di sumbu X untuk gerakan naik-turun)
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotasi karakter berdasarkan pergerakan mouse ke kiri/kanan
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
