using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Menggunakan TextMeshPro untuk UI teks

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText; // Variabel untuk menyimpan referensi ke UI teks

    void Awake()
    {
        // Jika promptText belum di-assign di Inspector, coba cari secara otomatis di anak objek
        if (promptText == null)
        {
            promptText = GetComponentInChildren<TextMeshProUGUI>(); // Mencari komponen di anak objek
            if (promptText == null)
            {
                Debug.LogError("PlayerUI: Tidak dapat menemukan komponen TextMeshProUGUI! Pastikan sudah di-assign di Inspector.");
            }
        }
    }

    // Fungsi untuk memperbarui teks di UI
    public void UpdateText(string promptMessage)
    {
        if (promptText == null) // Pastikan promptText sudah terpasang
        {
            Debug.LogError("PlayerUI: promptText belum di-assign! Pastikan diatur di Inspector.");
            return;
        }

        promptText.text = promptMessage; // Mengubah teks di UI
    }
}
