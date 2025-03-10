using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic; // Audio Source untuk musik latar
    public Slider volumeSlider; // UI Slider untuk volume

    void Start()
    {
        // Pastikan volume diatur ke nilai yang disimpan sebelumnya
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        backgroundMusic.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Tambahkan listener ke slider untuk mengubah volume saat digeser
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Simpan pengaturan volume
    }
}
