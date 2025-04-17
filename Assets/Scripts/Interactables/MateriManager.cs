using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MateriManager : MonoBehaviour
{
    public GameObject[] materiPanels; // Panel-panel materi yang akan ditampilkan secara bergantian
    public bool[] materiDibaca = new bool[3]; // Status apakah materi sudah dibaca oleh pemain
    public GameObject selesaiPanel; // Panel yang muncul setelah semua materi dibaca
    public TextMeshProUGUI messageText; // Teks untuk memberi notifikasi ke pemain

    private int materiIndex = 0; // Menyimpan indeks materi yang sedang dibaca

    // Fungsi ini dipanggil saat game dimulai (dalam hal ini tidak ada tampilan langsung)
    void Start()
    {
        // Tidak ada aksi yang dilakukan saat Start, karena ini Materi manager untuk mengatur fungsi panel materi
        
    }

    // Fungsi ini dipanggil saat pemain mulai membaca materi (misalnya, lewat trigger atau tombol interaksi)
    public void MulaiBacaMateri()
    {
        materiIndex = 0; // Setel indeks materi ke 0 (materi pertama)
        ShowNextMateri(); // Tampilkan materi pertama
    }

    // Fungsi untuk menutup panel materi dan menandainya sebagai telah dibaca
    public void ClosePanel()
    {
        // Tutup panel materi yang sedang ditampilkan
        if (materiIndex < materiPanels.Length)
        {
            materiPanels[materiIndex].SetActive(false); // Menyembunyikan panel materi yang sedang aktif
        }

        // Mengubah kunci kursor untuk kembali ke mode terkunci
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Tandai materi sebagai sudah dibaca
        materiDibaca[materiIndex] = true;

        // Debugging - menampilkan log di konsol
        Debug.Log("Materi " + (materiIndex + 1) + " sudah dibaca");

        // Cek apakah semua materi sudah dibaca
        if (SemuaMateriSudahDibaca())
        {
            TampilkanPanelSelesai(); // Jika semua materi sudah dibaca, tampilkan panel selesai
        }
        else
        {
            materiIndex++; // Pindah ke materi berikutnya
            ShowNextMateri(); // Tampilkan materi berikutnya
        }
    }

    // Fungsi untuk menampilkan materi berikutnya
    public void ShowNextMateri()
    {
        // Cek apakah ada materi yang masih tersisa untuk ditampilkan
        if (materiIndex < materiPanels.Length)
        {
            materiPanels[materiIndex].SetActive(true); // Menampilkan panel materi berdasarkan indeks
            messageText.text = "Baca materi: " + (materiIndex + 1); // Menampilkan pesan notifikasi
            Cursor.lockState = CursorLockMode.None; // Mengubah status kunci kursor agar bisa bebas bergerak
            Cursor.visible = true; // Membuat kursor terlihat
        }
    }

    // Fungsi untuk memeriksa apakah semua materi sudah dibaca
    private bool SemuaMateriSudahDibaca()
    {
        foreach (bool baca in materiDibaca) // Memeriksa status semua materi
        {
            if (!baca) return false; // Jika ada materi yang belum dibaca, kembalikan false
        }
        return true; // Semua materi sudah dibaca
    }

    // Fungsi untuk menampilkan panel selesai setelah semua materi dibaca
    public void TampilkanPanelSelesai()
    {
        selesaiPanel.SetActive(true); // Menampilkan panel selesai
        messageText.text = "Semua materi telah dibaca!"; // Menampilkan pesan selesai
        Cursor.lockState = CursorLockMode.None; // Membuat kursor bebas bergerak
        Cursor.visible = true; // Membuat kursor terlihat
    }

    // Fungsi untuk melanjutkan ke level berikutnya (Level 1)
    public void LanjutKeLevel1()
    {
        PlayerPrefs.SetInt("Level1_Unlock", 1); // Menyimpan status level 1 terbuka
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home"); // Memuat scene "Home" setelah materi selesai
    }
}