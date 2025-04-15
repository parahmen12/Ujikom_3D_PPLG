using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Menentukan apakah objek ini menggunakan event saat berinteraksi
    public bool useEvents;

    // Pesan yang akan ditampilkan saat pemain melihat objek ini
    [SerializeField]
    public string promptMessage;

    // Fungsi yang mengembalikan pesan saat pemain melihat objek ini
    public virtual string OnLook()
    {
        return promptMessage; // Mengembalikan teks yang ditentukan di Inspector
    }

    // Fungsi dasar untuk memproses interaksi
    public void BaseInteract()
    {
        // Jika `useEvents` aktif, maka panggil event interaksi dari `InteractionEvent`
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();

        // Memanggil fungsi `Interact()` yang akan di-override di subclass
        Interact();
    }

    // Fungsi virtual yang akan di-override oleh class turunannya
    protected virtual void Interact()
    {
        // ini adalah fungsi untuk interaksi
    }
}