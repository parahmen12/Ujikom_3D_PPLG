using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Tambahkan ini

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel; // Panel UI Game Over
    public Button restartButton; // Tombol untuk restart game
    public Button quitButton; // Tombol untuk keluar dari game
    public GameObject player; // Referensi ke Player
    public GameObject ScoreText;
    public GameObject[] enemies; // Referensi ke semua musuh di dalam game

    private bool gameOverTriggered = false; // Mencegah pemanggilan lebih dari sekali

    void Start()
    {
        gameOverPanel.SetActive(false); // Sembunyikan panel game over saat awal game

        // Pastikan tombol bisa diklik
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Menampilkan layar Game Over dan menghentikan permainan.
    /// </summary>
    public void ShowGameOver()
    {
        ScoreText.SetActive(false);
        
        if (gameOverTriggered) return; // Cegah pemanggilan ulang

        gameOverTriggered = true;
        gameOverPanel.SetActive(true); // Tampilkan panel Game Over

        // Aktifkan kursor agar pemain bisa menekan tombol
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // **PAUSE GAME TAPI BIARKAN UI TETAP BISA DIKLIK**
        Time.timeScale = 0f;

        // **Hindari bug UI yang tidak responsif**
        EventSystem.current.SetSelectedGameObject(null);

        // Nonaktifkan kontrol player agar tidak bisa bergerak
        if (player != null)
        {
            if (player.GetComponent<PlayerMotor>() != null)
                player.GetComponent<PlayerMotor>().enabled = false;

            if (player.GetComponent<PlayerLook>() != null)
                player.GetComponent<PlayerLook>().enabled = false;
        }

        // Hentikan semua musuh jika ada
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<NavMeshAgent>() != null)
                    enemy.GetComponent<NavMeshAgent>().isStopped = true;

                if (enemy.GetComponent<MonsterAI>() != null)
                    enemy.GetComponent<MonsterAI>().enabled = false;
            }
        }

        // **Pastikan tombol UI tetap bisa berfungsi saat game pause**
        foreach (Button btn in gameOverPanel.GetComponentsInChildren<Button>())
        {
            btn.interactable = true;
        }
    }

    /// <summary>
    /// Mengulang kembali game dengan memuat ulang scene saat ini.
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Tombol Retry Ditekan"); // Cek apakah tombol ditekan
        Time.timeScale = 1f; // Kembalikan waktu normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang scene saat ini
    }

    /// <summary>
    /// Menutup game dan keluar dari aplikasi.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Tombol Quit Ditekan"); // Cek apakah tombol ditekan
        Time.timeScale = 1f; // Pastikan waktu kembali normal sebelum keluar
        Application.Quit(); // Keluar dari game (hanya berfungsi di build, tidak di editor)
    }
}
