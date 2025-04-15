using UnityEngine; 

public class BacksoundManager : MonoBehaviour
{
    public AudioSource audioSource; // Komponen AudioSource untuk memutar musik
    public AudioClip backgroundMusic; // File AudioClip yang akan diputar sebagai backsound

    void Start()
    {
        // Jika audioSource belum diatur di Inspector, tambahkan komponen AudioSource secara otomatis
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Mengatur musik latar belakang yang akan diputar
        audioSource.clip = backgroundMusic;

        // Mengaktifkan looping agar musik diputar terus-menerus tanpa henti
        audioSource.loop = true;

        // Musik akan langsung mulai diputar ketika game berjalan
        audioSource.playOnAwake = true;

        // Menyesuaikan volume audio (nilai antara 0.0 hingga 1.0)
        audioSource.volume = 0.5f;

        // Memulai pemutaran musik
        audioSource.Play();
    }
}
