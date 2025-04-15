using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Canvas References")]
    public CanvasGroup splashScreenCanvas; // Canvas Splash Screen
    public CanvasGroup homeMenuCanvas; // Canvas Home Menu
    public GameObject menuCanvas; // Canvas menu utama
    public GameObject currentCanvas; // Canvas yang sedang aktif
    public GameObject optionCanvas; // Canvas menu option

    private GameObject lastActiveCanvas; // Menyimpan canvas terakhir sebelum masuk ke Option

    [Header("Splash Screen Settings")]
    public float fadeDuration = 1.5f; // Durasi animasi fade in/out
    public float displayTime = 2f; // Waktu tampil sebelum masuk menu

    private void Start()
    {
        StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        // Set awal: SplashScreen tampak, HomeMenu transparan
        splashScreenCanvas.alpha = 1f;
        homeMenuCanvas.alpha = 0f;
        homeMenuCanvas.gameObject.SetActive(false);

        // Biarkan Splash Screen tampil selama beberapa detik
        yield return new WaitForSeconds(displayTime);

        // Fade out Splash Screen
        yield return StartCoroutine(FadeCanvas(splashScreenCanvas, 0f));

        // Aktifkan Home Menu dan fade in
        homeMenuCanvas.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvas(homeMenuCanvas, 1f));

        // Nonaktifkan SplashScreen agar tidak mengganggu
        splashScreenCanvas.gameObject.SetActive(false);
    }

    IEnumerator FadeCanvas(CanvasGroup canvas, float targetAlpha)
    {
        float startAlpha = canvas.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvas.alpha = targetAlpha;
    }

    // 🔹 Fungsi Navigasi ke Option Menu
    private GameObject GetActiveCanvas()
    {
        if (menuCanvas != null && menuCanvas.activeSelf) return menuCanvas;
        if (currentCanvas != null && currentCanvas.activeSelf) return currentCanvas;
        if (homeMenuCanvas != null && homeMenuCanvas.gameObject.activeSelf) return homeMenuCanvas.gameObject;
        return null;
    }
    public void OptionMenu()
    {
        if (optionCanvas != null) // Pastikan ada panel opsi
        {
            lastActiveCanvas = GetActiveCanvas(); // Simpan panel yang sedang aktif sebelum masuk ke Option Menu
            optionCanvas.SetActive(true); // Tampilkan panel opsi
        }

        if (menuCanvas != null) // Pastikan ada menu utama
        {
            menuCanvas.SetActive(false); // Sembunyikan menu utama
        }
    }

    // 🔹 Fungsi untuk Masuk ke Level Menu
    public void EnterMenu()
{
    if (menuCanvas != null)
    {
        menuCanvas.SetActive(true); // Tampilkan Level Menu
        if (currentCanvas != null)
        {
            currentCanvas.SetActive(false); // Sembunyikan menu sebelumnya
        }
    }
}

    // 🔹 Fungsi Kembali ke Menu Sebelumnya
public void BackGame()
{
    // Jika sedang di Option Menu, kembali ke menu sebelumnya
    if (optionCanvas != null && optionCanvas.activeSelf)
    {
        optionCanvas.SetActive(false); // Tutup Option Menu

        if (lastActiveCanvas != null) 
        {
            lastActiveCanvas.SetActive(true); // Kembali ke menu sebelumnya
        }
        else if (homeMenuCanvas != null)
        {
            homeMenuCanvas.gameObject.SetActive(true); // Jika tidak ada menu sebelumnya, kembali ke Home
        }
        return; // Hentikan fungsi agar tidak lanjut ke bawah
    }

    // Jika sedang di Level Menu (menuCanvas), kembali ke Home Menu
    if (menuCanvas != null && menuCanvas.activeSelf)
    {
        menuCanvas.SetActive(false); // Sembunyikan Level Menu
        if (homeMenuCanvas != null)
        {
            homeMenuCanvas.gameObject.SetActive(true); // Kembali ke Home Menu
        }
        return;
    }

    // Jika tidak sedang di Option Menu atau Level Menu, langsung kembali ke Home Menu
    if (homeMenuCanvas != null)
    {
        homeMenuCanvas.gameObject.SetActive(true);
    }
}


    // 🔹 Fungsi Load Scene dengan Loading Screen
    public void LoadGameScene()
    {
        PlayerPrefs.SetString("NextScene", "Level 1");
        SceneManager.LoadScene("Loading");
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene("Home");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
