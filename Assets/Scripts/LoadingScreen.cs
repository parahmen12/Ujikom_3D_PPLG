using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingBar; // Slider untuk menampilkan progress loading
    public Text loadingText; // Teks untuk menampilkan status loading
    public float fakeLoadingSpeed = 0.5f; // Kecepatan loading palsu agar lebih smooth

    private float targetProgress = 0f; // Menyimpan target progress untuk loading bar
    private bool isProgressStarted = false; // Menandakan apakah progress loading sudah dimulai

    void Start()
    {
        // Sembunyikan slider di awal agar tidak langsung terlihat
        loadingBar.gameObject.SetActive(false);

        // Memulai proses loading scene berikutnya secara asinkron
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Mengambil nama scene berikutnya dari PlayerPrefs
        // Jika tidak ada yang tersimpan, gunakan "Level 1" sebagai default
        string nextScene = PlayerPrefs.HasKey("NextScene") ? PlayerPrefs.GetString("NextScene") : "Level 1";

        // Menampilkan teks awal "Loading..." sebelum progress dimulai
        loadingText.text = "Loading...";

        // Memuat scene secara asinkron, tetapi belum langsung masuk ke scene tersebut
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false; // Mencegah scene langsung berubah sebelum mencapai 100%

        // Delay 1 detik agar efek loading lebih terasa nyata
        yield return new WaitForSeconds(1f);

        // Looping untuk mengupdate progress hingga scene selesai dimuat
        while (!operation.isDone)
        {
            // Menentukan target progress berdasarkan progress asli dari operasi asinkron
            // AsyncOperation biasanya hanya mencapai 90%, sisanya dikendalikan manual
            targetProgress = operation.progress < 0.9f ? operation.progress : 1f;

            // Jika progress sudah mulai (di atas 10%), maka tampilkan loading bar
            if (!isProgressStarted && operation.progress > 0.1f)
            {
                isProgressStarted = true;
                loadingBar.gameObject.SetActive(true); // Munculkan loading bar
            }

            // Memperbarui nilai loading bar agar terlihat lebih smooth dengan Lerp
            loadingBar.value = Mathf.Lerp(loadingBar.value, targetProgress, fakeLoadingSpeed * Time.deltaTime);

            // Jika progress sudah mulai, ubah teks menjadi "Rendering: [persentase]%"
            if (isProgressStarted)
            {
                loadingText.text = "Rendering: " + (loadingBar.value * 100).ToString("F0") + "%";
            }

            // Jika progress sudah mencapai 100%, beri instruksi untuk melanjutkan
            if (loadingBar.value >= 0.99f)
            {
                loadingText.text = "Tekan tombol apa saja untuk melanjutkan...";
                loadingBar.gameObject.SetActive(false); // Sembunyikan loading bar

                // Tunggu hingga pemain menekan tombol sebelum melanjutkan ke scene berikutnya
                yield return new WaitUntil(() => Input.anyKeyDown);
                operation.allowSceneActivation = true; // Masuk ke scene tujuan
            }

            yield return null; // Tunggu frame berikutnya sebelum melanjutkan looping
        }
    }
}
