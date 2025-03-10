using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        gameOverPanel.SetActive(false); // Sembunyikan panel di awal
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Pause game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Kembalikan waktu normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Muat ulang scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Kembalikan waktu normal sebelum keluar
        Application.Quit(); // Keluar dari game
    }
}
