using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelManager : MonoBehaviour
{
    public Button level1Button; // Referensi tombol untuk membuka level 1
    public Button level2Button; // Referensi tombol untuk membuka level 2
    public Button level3Button; // Referensi tombol untuk membuka level 3
    public Button resetButton; // Tombol untuk mereset semua level (diisi lewat Inspector)

    void Start()
    {
        UpdateLevelButtons(); // Panggil fungsi untuk mengecek status PlayerPrefs dan update tombol level

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetLevels); // Tambahkan event listener untuk tombol reset
        }
    }

    void UpdateLevelButtons()
    {
        // Cek apakah PlayerPrefs "Level1Unlocked" bernilai 1, jika ya maka tombol level1 aktif
        if (level1Button != null)
            level1Button.interactable = PlayerPrefs.GetInt("Level1Unlocked", 0) == 1;

        // Cek apakah PlayerPrefs "Level2Unlocked" bernilai 1, jika ya maka tombol level2 aktif
        if (level2Button != null)
            level2Button.interactable = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        // Cek apakah PlayerPrefs "Level3Unlocked" bernilai 1, jika ya maka tombol level3 aktif
        if (level3Button != null)
            level3Button.interactable = PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;

        // Debug log untuk melihat status level yang terbuka di console Unity
        Debug.Log("Status Level:");
        Debug.Log("Level 1: " + PlayerPrefs.GetInt("Level1Unlocked", 0));
        Debug.Log("Level 2: " + PlayerPrefs.GetInt("Level2Unlocked", 0));
        Debug.Log("Level 3: " + PlayerPrefs.GetInt("Level3Unlocked", 0));
    }

    public void ResetLevels()
    {
        // Hapus data PlayerPrefs untuk membuka kembali semua level ke status awal (terkunci)
        PlayerPrefs.DeleteKey("Level1Unlocked");
        PlayerPrefs.DeleteKey("Level2Unlocked");
        PlayerPrefs.DeleteKey("Level3Unlocked");
        PlayerPrefs.Save(); // Simpan perubahan PlayerPrefs

        Debug.Log("Semua level telah di-reset!"); // Tampilkan log bahwa level berhasil direset

        UpdateLevelButtons(); // Update tampilan tombol setelah reset
    }
}