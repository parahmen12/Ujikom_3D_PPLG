using UnityEngine;
using UnityEngine.UI; // Untuk UI Slider

public class SoundSlider : MonoBehaviour
{
    public Slider volumeSlider; // Slider untuk mengatur volume musik
    public AudioSource backgroundMusic; // Musik latar belakang

    private const string VolumePrefKey = "MusicVolume"; // Key untuk menyimpan volume

    void Start()
    {
        // Pastikan ada AudioSource dan Slider yang dihubungkan di Inspector
        if (backgroundMusic == null || volumeSlider == null)
        {
            Debug.LogError("Background Music atau Volume Slider belum diatur di Inspector!");
            return;
        }

        // Ambil volume yang tersimpan, jika tidak ada gunakan default (1.0f)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1.0f);
        volumeSlider.value = savedVolume; // Set nilai slider
        backgroundMusic.volume = savedVolume; // Terapkan ke musik

        // Tambahkan listener agar saat slider digerakkan, volume berubah
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float volume)
    {
        backgroundMusic.volume = volume; // Ubah volume musik
        AudioListener.volume = volume; // Ubah volume keseluruhan game (opsional)

        // Simpan pengaturan volume ke PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}
