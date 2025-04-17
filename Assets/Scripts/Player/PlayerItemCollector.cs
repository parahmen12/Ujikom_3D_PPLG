using UnityEngine;
using UnityEngine.UI;

public class PlayerItemCollector : MonoBehaviour
{
    public Text scoreText; // UI untuk menampilkan skor

    public int itemCount { get; private set; } = 0; // Skor atau jumlah item yang dikumpulkan

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("⚠ scoreText belum diatur di Inspector!"); // Mengecek apakah scoreText sudah diassign di Inspector
            return;
        }
        UpdateScoreText(); // Memperbarui teks skor saat permainan dimulai
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) // Jika player menabrak objek yang memiliki tag "Item"
        {
            itemCount++; // Menambah jumlah item yang dikumpulkan
            Debug.Log("✅ Item diambil! Total: " + itemCount); // Menampilkan log setiap kali item diambil

            Destroy(other.gameObject); // Menghancurkan objek item yang diambil
            UpdateScoreText(); // Memperbarui teks skor
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Item Collected: " + itemCount; // Memperbarui tampilan jumlah item yang dikumpulkan
        }
    }

    // Tambahkan getter agar bisa diakses dari luar (misal GameSuccess)
    public int GetTotalPoints()
    {
        return itemCount; // Mengembalikan total jumlah item yang dikumpulkan
    }
}