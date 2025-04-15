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
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
