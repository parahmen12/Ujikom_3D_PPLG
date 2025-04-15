using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [Header("Pengaturan Interaksi")]
    public bool useEvents; // Apakah objek ini menggunakan event interaksi
    [SerializeField]
    public string promptMessage; // Pesan yang akan ditampilkan saat pemain melihat objek ini

    [Header("Event yang Akan Dipanggil")]
    public UnityEvent OnInteract; // Event yang akan dieksekusi saat objek diinteraksi

    // Fungsi untuk mengembalikan pesan prompt ketika pemain melihat objek
    public virtual string OnLook()
    {
        return promptMessage;
    }

    // Fungsi dasar untuk memicu interaksi
    public void BaseInteract()
    {
        Interact(); // Panggil fungsi Interact() di kelas turunan
    }

    // Fungsi ini bisa di-override di kelas turunan untuk menambahkan aksi khusus
    protected virtual void Interact()
    {
        // Kosong, akan diimplementasikan dalam script turunan jika diperlukan
    }
}
