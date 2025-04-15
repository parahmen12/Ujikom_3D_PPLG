using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Panel utama untuk menu pause
    public GameObject settingsPanel; // Panel untuk menu pengaturan
    public GameObject blurPanel; // Panel untuk efek blur saat game dijeda
    public Slider bgmSlider; // Slider untuk mengatur volume musik latar
    public Slider sfxSlider; // Slider untuk mengatur volume efek suara
    public AudioSource backgroundMusic; // AudioSource yang digunakan untuk musik latar

    private bool isPaused = false; // Mengecek apakah game sedang dijeda
    private const string BgmVolumeKey = "BGMVolume";
    private const string SfxVolumeKey = "SFXVolume";

    void Start()
    {
        // Sembunyikan semua panel saat game dimulai
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        blurPanel.SetActive(false);

        // Ambil nilai volume yang tersimpan atau gunakan default 1.0f
        float savedBgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 1.0f);
        float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1.0f);

        // Set nilai awal slider
        bgmSlider.value = savedBgmVolume;
        sfxSlider.value = savedSfxVolume;

        // Terapkan volume yang tersimpan
        backgroundMusic.volume = savedBgmVolume;
        AudioListener.volume = savedSfxVolume; // Untuk efek suara secara keseluruhan

        // Tambahkan listener agar saat slider digerakkan, volume berubah
        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    void Update()
    {
        // Jika tombol "P" ditekan, maka game akan di-pause atau dilanjutkan
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame(); // Lanjutkan game jika sudah dalam keadaan pause
            else
                PauseGame(); // Jeda game jika belum dalam keadaan pause
        }
    }

    public void PauseGame()
    {
        // Aktifkan panel pause dan efek blur
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        blurPanel.SetActive(true);

        // Hentikan waktu dalam game (pause)
        Time.timeScale = 0;
        isPaused = true;

        // Tampilkan kursor agar pemain bisa mengklik menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        // Sembunyikan semua panel saat game dilanjutkan
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        blurPanel.SetActive(false);

        // Kembalikan waktu normal dalam game
        Time.timeScale = 1;
        isPaused = false;

        // Sembunyikan kembali kursor agar tidak mengganggu gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        // Tampilkan panel settings dan sembunyikan panel pause
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void CloseSettings()
    {
        // Sembunyikan panel settings dan kembali ke panel pause
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ChangeBGMVolume(float volume)
    {
        backgroundMusic.volume = volume; // Atur volume musik latar
        PlayerPrefs.SetFloat(BgmVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioListener.volume = volume; // Atur volume efek suara
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void GoToMainMenu()
    {
        // Pastikan waktu kembali normal sebelum berpindah ke scene menu utama
        Time.timeScale = 1;
        SceneManager.LoadScene("Home"); // Ganti "Home" dengan nama scene menu utama yang sesuai
    }
}
