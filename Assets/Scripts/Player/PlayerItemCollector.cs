using UnityEngine;
using UnityEngine.UI;

public class PlayerItemCollector : MonoBehaviour
{
    public Text scoreText; // UI untuk menampilkan skor

    public int itemCount { get; private set; } = 0; // Skor atau jumlah item

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("⚠ scoreText belum diatur di Inspector!");
            return;
        }
        UpdateScoreText(); // Update saat start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemCount++;
            Debug.Log("✅ Item diambil! Total: " + itemCount);

            Destroy(other.gameObject);
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Item Collected: " + itemCount;
        }
    }

    // Tambahkan getter agar bisa diakses dari luar (misal GameSuccess)
    public int GetTotalPoints()
    {
        return itemCount;
    }
}
