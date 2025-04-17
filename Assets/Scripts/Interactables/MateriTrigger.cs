using UnityEngine;
using UnityEngine.UI;

public class MateriTrigger : MonoBehaviour
{
    public GameObject panelMateri; // Panel UI yang muncul
    private bool isPlayerInside = false; // Menyimpan status apakah player berada di dalam trigger

    // Fungsi ini dipanggil saat objek memasuki area trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Jika yang memasuki trigger adalah player
        {
            isPlayerInside = true; // Menandai bahwa player berada di dalam trigger
            panelMateri.SetActive(true); // Menampilkan panel materi

            // Mengubah status kunci kursor agar bisa bergerak bebas dan terlihat
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Fungsi ini dipanggil saat objek keluar dari area trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Jika yang keluar adalah player
        {
            isPlayerInside = false; // Menandai bahwa player keluar dari trigger
            panelMateri.SetActive(false); // Menyembunyikan panel materi

            // Mengubah status kunci kursor agar kembali terkunci dan tidak terlihat
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}