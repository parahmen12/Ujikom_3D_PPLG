using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingBar;
    public Text loadingText;

    [Header("Loading Settings")]
    public float fakeLoadingSpeed = 0.5f;

    private float targetProgress = 0f;
    private bool isProgressStarted = false;

    void Start()
    {
        loadingBar.gameObject.SetActive(false); // Hide loading bar at start
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        string nextScene = PlayerPrefs.GetString("NextScene", "Level 1");
        loadingText.text = "Loading...";

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f); // Optional delay

        while (!operation.isDone)
        {
            // Unity async load progress usually goes from 0 to 0.9 before allowing activation
            targetProgress = operation.progress < 0.9f ? operation.progress : 1f;

            // Show loading bar once loading starts
            if (!isProgressStarted && operation.progress > 0.1f)
            {
                isProgressStarted = true;
                loadingBar.gameObject.SetActive(true);
            }

            // Lerp for smooth progress bar filling
            loadingBar.value = Mathf.MoveTowards(loadingBar.value, targetProgress, fakeLoadingSpeed * Time.deltaTime);

            // Update loading text
            if (isProgressStarted)
            {
                loadingText.text = "Rendering: " + Mathf.RoundToInt(loadingBar.value * 100f) + "%";
            }

            // When loading complete
            if (loadingBar.value >= 0.99f && targetProgress >= 1f)
            {
                loadingText.text = "Tekan tombol apa saja untuk melanjutkan...";
                loadingBar.gameObject.SetActive(false);

                yield return new WaitUntil(() => Input.anyKeyDown);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
