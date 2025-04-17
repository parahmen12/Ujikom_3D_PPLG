using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Panel Puzzle yang akan ditampilkan")]
    public GameObject puzzlePanel; // Panel puzzle yang akan muncul ketika player masuk trigger

    [Header("Hanya bisa aktif satu kali?")]
    public bool triggerSekaliSaja = false; // Jika true, puzzle hanya bisa diaktifkan sekali saja

    private bool sudahMasuk = false; // Penanda apakah player sudah pernah masuk trigger

    private void OnTriggerEnter(Collider other)
    {
        // Saat player masuk area trigger dan belum pernah masuk sebelumnya
        if (other.CompareTag("Player") && !sudahMasuk)
        {
            if (puzzlePanel != null)
            {
                puzzlePanel.SetActive(true); // Aktifkan panel puzzle
                Cursor.lockState = CursorLockMode.None; // Bebaskan kursor agar bisa klik UI
                Cursor.visible = true; // Tampilkan kursor

                if (triggerSekaliSaja)
                    sudahMasuk = true; // Tandai bahwa trigger sudah pernah diaktifkan
            }
            else
            {
                Debug.LogWarning("Puzzle Panel belum diset di Inspector!"); // Warning jika belum di-assign
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Jika player keluar dari trigger dan trigger boleh diulang (bukan sekali saja)
        if (other.CompareTag("Player") && !triggerSekaliSaja)
        {
            if (puzzlePanel != null)
            {
                puzzlePanel.SetActive(false); // Sembunyikan panel puzzle
                Cursor.lockState = CursorLockMode.Locked; // Kunci kembali kursor
                Cursor.visible = false; // Sembunyikan kursor
            }
        }
    }
}