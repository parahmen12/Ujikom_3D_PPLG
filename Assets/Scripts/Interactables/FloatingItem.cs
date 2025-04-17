using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatSpeed = 0.5f; // Kecepatan melayang
    public float floatHeight = 0.3f; // Seberapa tinggi item melayang

    private Vector3 startPosition; // Posisi awal item

    void Start()
    {
        startPosition = transform.position; // Simpan posisi awal
    }

    void Update()
    {
        // Gerakan naik-turun menggunakan fungsi sinus agar lembut
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        // Menghitung posisi Y baru menggunakan fungsi sinus untuk membuat efek naik-turun (melayang) secara halus.
        // Time.time membuat gerakan terus berubah seiring waktu.
        // floatSpeed menentukan kecepatan gerakan naik-turun.
        // floatHeight menentukan seberapa tinggi gerakan naik-turunnya.
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        // Mengatur posisi objek saat ini dengan nilai Y yang baru,
        // sementara nilai X dan Z tetap sama (agar hanya bergerak secara vertikal).
    }
}
