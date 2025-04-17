using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Tambahkan ini agar bisa mengatur EventSystem untuk UI

public class GameOver : MonoBehaviour
{
    // Panel UI yang muncul saat game over
    public GameObject gameOverPanel; // Panel UI Game Over
    // Tombol untuk merestart permainan
    public Button restartButton; // Tombol untuk restart game
    // Tombol untuk keluar dari game
    public Button quitButton; // Tombol untuk keluar dari game
    // Referensi ke objek Player
    public GameObject player; // Referensi ke Player
    // Referensi ke UI teks skor
    public GameObject ScoreText;
    // Referensi ke semua musuh yang ada di dalam game
    public GameObject[] enemies; // Referensi ke semua musuh di dalam game

    // Variabel untuk memastikan Game Over hanya dipanggil sekali
    private bool gameOverTriggered = false; // Mencegah pemanggilan lebih dari sekali

    void Start()
    {
        gameOverPanel.SetActive(false); // Sembunyikan panel game over saat awal game

        // Menambahkan listener ke tombol restart dan quit agar bisa berfungsi
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    /// <summary>
    /// Menampilkan layar Game Over dan menghentikan permainan.
    /// </summary>
    public void ShowGameOver()
    {
        ScoreText.SetActive(false); // Sembunyikan teks skor saat game over

        if (gameOverTriggered) return; // Pastikan hanya dipanggil sekali

        gameOverTriggered = true;
        gameOverPanel.SetActive(true); // Menampilkan panel Game Over

        // Mengaktifkan kursor untuk memungkinkan pemain menekan tombol UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // **PAUSE GAME TAPI TETAP MEMBIARKAN UI TETAP BISA DIKLIK**
        Time.timeScale = 0f; // Menghentikan permainan sementara tanpa menghentikan UI

        // **Hindari bug UI yang tidak responsif**: Memastikan tombol UI tetap aktif saat game dipause
        EventSystem.current.SetSelectedGameObject(null);

        // Menonaktifkan kontrol player agar tidak bisa bergerak saat game over
        if (player != null) // Mengecek apakah objek player tidak null, artinya objek player ada di scene.
        {
            if (player.GetComponent<PlayerMotor>() != null) // Mengecek apakah komponen PlayerMotor ada pada objek player
                player.GetComponent<PlayerMotor>().enabled = false; // Menonaktifkan komponen PlayerMotor, yang berfungsi untuk menggerakkan player

            if (player.GetComponent<PlayerLook>() != null) // Mengecek apakah komponen PlayerLook ada pada objek player
                player.GetComponent<PlayerLook>().enabled = false; // Menonaktifkan komponen PlayerLook, yang berfungsi untuk mengontrol arah pandangan player
        }


        // Menonaktifkan semua musuh agar berhenti bergerak
        foreach (GameObject enemy in enemies) // Iterasi melalui semua objek musuh yang ada di dalam array 'enemies'
        {
            if (enemy != null) // Mengecek apakah objek musuh tidak null (ada di scene)
            {
                if (enemy.GetComponent<NavMeshAgent>() != null) // Mengecek apakah musuh memiliki komponen NavMeshAgent
                    enemy.GetComponent<NavMeshAgent>().isStopped = true; // Menonaktifkan pergerakan musuh dengan menghentikan NavMeshAgent

                if (enemy.GetComponent<MonsterAI>() != null) // Mengecek apakah musuh memiliki komponen MonsterAI
                    enemy.GetComponent<MonsterAI>().enabled = false; // Menonaktifkan AI musuh sehingga mereka tidak akan bergerak atau bereaksi terhadap player
            }
        }


        // **Pastikan tombol UI tetap bisa berfungsi saat game pause**
        foreach (Button btn in gameOverPanel.GetComponentsInChildren<Button>())
        {
            btn.interactable = true; // Pastikan tombol UI tetap dapat diklik
        }
    }

    /// <summary>
    /// Mengulang kembali game dengan memuat ulang scene saat ini.
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Tombol Retry Ditekan"); // Debug log untuk memverifikasi tombol ditekan
        Time.timeScale = 1f; // Kembalikan waktu normal agar game bisa dilanjutkan
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang scene saat ini
    }

    /// <summary>
    /// Menutup game dan keluar dari aplikasi.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Tombol Quit Ditekan"); // Debug log untuk memverifikasi tombol quit ditekan
        Time.timeScale = 1f; // Pastikan waktu kembali normal sebelum keluar
        Application.Quit(); // Keluar dari game (hanya berfungsi di build, tidak di editor)
    }
}