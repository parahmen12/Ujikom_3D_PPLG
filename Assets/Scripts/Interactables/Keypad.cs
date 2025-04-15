using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField] private GameObject door; // Objek pintu yang akan terbuka
    [SerializeField] private AudioClip doorOpenSound; // Sound effect saat pintu terbuka
    [SerializeField] private AudioClip doorCloseSound; // Sound effect saat pintu tertutup
    private bool doorOpen = false; // Status apakah pintu terbuka atau tidak
    private AudioSource audioSource; // Komponen AudioSource

    void Start()
    {
        // Menambahkan AudioSource ke objek ini jika belum ada
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Pastikan suara tidak diputar secara looping
        audioSource.loop = false;
    }

    protected override void Interact()
    {
        // Toggle status pintu (buka/tutup)
        doorOpen = !doorOpen;

        // Set animasi pintu
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);

        // Mainkan suara sesuai dengan status pintu
        if (doorOpen && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound); // Mainkan suara pintu terbuka
        }
        else if (!doorOpen && doorCloseSound != null)
        {
            audioSource.PlayOneShot(doorCloseSound); // Mainkan suara pintu tertutup
        }
    }
}
