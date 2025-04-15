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
            scoreText.gameObject.SetActive(false);
        }

        // Ambil poin dari PlayerItemCollector jika ada
        PlayerItemCollector itemCollector = FindObjectOfType<PlayerItemCollector>();
        if (itemCollector != null)
        {
            totalPoints = itemCollector.GetTotalPoints();
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
            scoreText.gameObject.SetActive(true);

            // Pindahkan ke tengah
            RectTransform rt = scoreText.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = Vector2.zero;
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
            }
        }
    }

    // Fungsi untuk lanjut ke level berikutnya
    public void ContinueGame()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        // Unlock level selanjutnya sebelum load scene
        UnlockNextLevel(currentScene);

        if (currentScene == "Level 1")
        {
            nextScene = "Level 2";
        }
        else if (currentScene == "Level 2")
        {
            nextScene = "Level 3";
        }
        else
        {
            nextScene = "Home";
        }

        SceneManager.LoadScene(nextScene);
    }

    // Unlock level berdasarkan level saat ini
    void UnlockNextLevel(string currentLevel)
    {
        if (currentLevel == "Level 1")
        {
            PlayerPrefs.SetInt("Level2Unlocked", 1);
        }
        else if (currentLevel == "Level 2")
        {
            PlayerPrefs.SetInt("Level3Unlocked", 1);
        }

        PlayerPrefs.Save();
    }

    // Kembali ke menu utama
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Home");
    }
}
