using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSuccess : MonoBehaviour
{
    [Header("UI References")]
    public Text scoreText; // Di-assign dari luar (drag dari UI)

    private int totalPoints;

    void Start()
    {
        // Sembunyikan scoreText di awal
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false); // Menyembunyikan scoreText pada awal permainan
        }

        // Ambil poin dari PlayerItemCollector jika ada
        PlayerItemCollector itemCollector = FindObjectOfType<PlayerItemCollector>(); // Mencari komponen PlayerItemCollector di scene
        if (itemCollector != null)
        {
            totalPoints = itemCollector.GetTotalPoints(); // Mengambil total poin yang dikumpulkan
        }
        else
        {
            totalPoints = 0;
            Debug.Log("ℹ Tidak ada PlayerItemCollector di scene ini.");
        }
    }

    // Dipanggil saat panel Game Success muncul
    public void ShowSuccess()
    {
        if (scoreText != null)
        {
            // Tampilkan skor
            scoreText.text = "Total Poin: " + totalPoints + " item dikumpulkan!";
            scoreText.gameObject.SetActive(true); // Menampilkan scoreText

            // Pindahkan ke tengah
            RectTransform rt = scoreText.GetComponent<RectTransform>(); // Mengambil RectTransform dari scoreText
            if (rt != null)
            {
                rt.anchoredPosition = Vector2.zero; // Menempatkan teks di tengah
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
            }
        }
    }

    // Fungsi untuk lanjut ke level berikutnya
    public void ContinueGame()
    {
        string currentScene = SceneManager.GetActiveScene().name; // Mendapatkan nama scene saat ini
        string nextScene = ""; // Menyimpan nama scene berikutnya

        // Unlock level selanjutnya sebelum load scene
        UnlockNextLevel(currentScene); // Fungsi untuk membuka level berikutnya

        if (currentScene == "Level 1")
        {
            nextScene = "Level 2"; // Jika level saat ini adalah Level 1, lanjut ke Level 2
        }
        else if (currentScene == "Level 2")
        {
            nextScene = "Level 3"; // Jika level saat ini adalah Level 2, lanjut ke Level 3
        }
        else
        {
            nextScene = "Home"; // Jika level saat ini adalah Level 3, kembali ke menu utama
        }

        SceneManager.LoadScene(nextScene); // Memuat scene berikutnya
    }

    // Unlock level berdasarkan level saat ini
    void UnlockNextLevel(string currentLevel)
    {
        if (currentLevel == "Level 1")
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1); // Membuka Level 2 setelah selesai dengan Level 1
        }
        else if (currentLevel == "Level 2")
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1); // Membuka Level 3 setelah selesai dengan Level 2
        }

        PlayerPrefs.Save(); // Menyimpan perubahan pada PlayerPrefs
    }

    // Kembali ke menu utama
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Home"); // Memuat scene "Home" untuk kembali ke menu utama
    }
}