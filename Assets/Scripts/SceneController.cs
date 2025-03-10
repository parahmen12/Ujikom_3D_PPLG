using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject menuCanvas; // Canvas menu utama
    public GameObject currentCanvas; // Canvas yang sedang aktif

    // Menampilkan Menu
    public void EnterMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true); // Aktifkan Menu
            if (currentCanvas != null)
            {
                currentCanvas.SetActive(false); // Sembunyikan Canvas sebelumnya
            }
        }
    }

    // Kembali ke tampilan sebelumnya
    public void BackGame()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false); // Sembunyikan menu
        }

        if (currentCanvas != null)
        {
            currentCanvas.SetActive(true); // Tampilkan kembali canvas sebelumnya
        }
    }

    // Keluar dari Game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Untuk Editor
#else
            Application.Quit(); // Untuk Build Game
#endif
    }
}
