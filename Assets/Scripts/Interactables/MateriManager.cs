using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MateriManager : MonoBehaviour
{
    public GameObject[] materiPanels;              // Panel-panel materi
    public bool[] materiDibaca = new bool[3];      // Status baca
    public GameObject selesaiPanel;                // Panel selesai
    public TextMeshProUGUI messageText;            // Pesan notifikasi

    private int materiIndex = 0;

    void Start()
    {
        // Pastikan semua panel materi dimatikan di awal
        foreach (GameObject panel in materiPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (selesaiPanel != null)
            selesaiPanel.SetActive(false);

        // Kunci kursor saat awal scene
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MulaiBacaMateri()
    {
        materiIndex = 0;
        ShowNextMateri();
    }

    public void ClosePanel()
    {
        // Validasi indeks
        if (materiIndex < 0 || materiIndex >= materiPanels.Length)
        {
            Debug.LogWarning("Index materi di luar batas!");
            return;
        }

        // Tutup panel aktif
        if (materiPanels[materiIndex] != null)
            materiPanels[materiIndex].SetActive(false);

        // Tandai sudah dibaca
        materiDibaca[materiIndex] = true;

        Debug.Log("Materi " + (materiIndex + 1) + " sudah dibaca");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Cek semua sudah dibaca
        if (SemuaMateriSudahDibaca())
        {
            TampilkanPanelSelesai();
        }
        else
        {
            materiIndex++;
            ShowNextMateri();
        }
    }

    public void ShowNextMateri()
    {
        // Validasi
        if (materiIndex < materiPanels.Length && materiPanels[materiIndex] != null)
        {
            materiPanels[materiIndex].SetActive(true);
            messageText.text = "Baca materi: " + (materiIndex + 1);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogWarning("Panel materi tidak ditemukan atau indeks salah.");
        }
    }

    private bool SemuaMateriSudahDibaca()
    {
        foreach (bool baca in materiDibaca)
        {
            if (!baca) return false;
        }
        return true;
    }

    public void TampilkanPanelSelesai()
    {
        if (selesaiPanel != null)
            selesaiPanel.SetActive(true);

        messageText.text = "Semua materi telah dibaca!";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LanjutKeLevel1()
    {
        PlayerPrefs.SetInt("Level1Unlocked", 1); // Ganti nama jadi konsisten
        PlayerPrefs.Save(); // Simpan PlayerPrefs secara eksplisit
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}