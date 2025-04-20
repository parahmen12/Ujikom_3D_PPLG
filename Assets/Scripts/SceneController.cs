using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Canvas References")]
    public CanvasGroup splashScreenCanvas;   // Splash screen canvas
    public CanvasGroup homeMenuCanvas;       // Home menu canvas
    public GameObject menuCanvas;            // Level selection menu
    public GameObject currentCanvas;         // Currently active canvas
    public GameObject optionCanvas;          // Option menu canvas

    private GameObject lastActiveCanvas;     // Stores last active canvas (used when returning from option menu)

    [Header("Splash Screen Settings")]
    public float fadeDuration = 1.5f;
    public float displayTime = 2f;

    void Start()
    {
        StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        // Setup splash screen visibility
        splashScreenCanvas.alpha = 1f;
        homeMenuCanvas.alpha = 0f;
        homeMenuCanvas.gameObject.SetActive(false);

        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeCanvas(splashScreenCanvas, 0f));

        homeMenuCanvas.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvas(homeMenuCanvas, 1f));

        splashScreenCanvas.gameObject.SetActive(false);
    }

    IEnumerator FadeCanvas(CanvasGroup canvas, float targetAlpha)
    {
        float startAlpha = canvas.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            yield return null;
        }

        canvas.alpha = targetAlpha;
    }

    // Show Option Menu
    public void OptionMenu()
    {
        if (optionCanvas != null)
        {
            lastActiveCanvas = GetActiveCanvas(); // Save last active menu
            optionCanvas.SetActive(true);
        }

        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
    }

    // Enter Level Menu
    public void EnterMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }

        if (currentCanvas != null)
        {
            currentCanvas.SetActive(false);
        }
    }

    // Back Button Logic
    public void BackGame()
    {
        // If in Option Menu
        if (optionCanvas != null && optionCanvas.activeSelf)
        {
            optionCanvas.SetActive(false);

            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.SetActive(true);
            }
            else if (homeMenuCanvas != null)
            {
                homeMenuCanvas.gameObject.SetActive(true);
            }
            return;
        }

        // If in Level Menu
        if (menuCanvas != null && menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);
            if (homeMenuCanvas != null)
            {
                homeMenuCanvas.gameObject.SetActive(true);
            }
            return;
        }

        // Default fallback
        if (homeMenuCanvas != null)
        {
            homeMenuCanvas.gameObject.SetActive(true);
        }
    }

    // -----------------------------
    // Scene Loading Functions
    // -----------------------------

    public void LoadGameScene()
    {
        PlayerPrefs.SetString("NextScene", "Level 1");
        SceneManager.LoadScene("Loading");
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene("Home");
    }

    public void LevelPrologue()
    {
        Debug.Log("🔁 Loading Materi Scene...");
        SceneManager.LoadScene("Materi");
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

    // Helper: Get currently active canvas
    private GameObject GetActiveCanvas()
    {
        if (menuCanvas != null && menuCanvas.activeSelf) return menuCanvas;
        if (currentCanvas != null && currentCanvas.activeSelf) return currentCanvas;
        if (homeMenuCanvas != null && homeMenuCanvas.gameObject.activeSelf) return homeMenuCanvas.gameObject;
        return null;
    }
}