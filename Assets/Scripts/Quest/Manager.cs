using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Intro & UI")]
    public string[] textIntro;  // Array berisi teks intro yang akan ditampilkan pada UI
    public Text textUI;  // Referensi ke komponen Text untuk menampilkan teks intro
    private int index = 0;  // Indeks untuk melacak teks intro yang sedang ditampilkan
    public GameObject panelMisi;  // Panel misi yang menampilkan teks intro

    [Header("Player & Controls")]
    public GameObject ScoreText;  // Referensi ke objek yang menampilkan skor
    public GameObject PlayerMove;  // Referensi ke objek yang mengendalikan pergerakan pemain
    public GameObject PlayerLook;  // Referensi ke objek yang mengendalikan pandangan pemain
    public float typingSpeed = 0.02f;  // Kecepatan pengetikan untuk teks intro
    private Coroutine typingCoroutine;  // Menyimpan referensi ke coroutine untuk pengetikan teks

    [Header("Pause Menu")]
    public GameObject pauseMenu;  // Referensi ke menu pause

    void Start()
    {
        // Menonaktifkan tampilan skor pada awal permainan
        if (ScoreText != null) ScoreText.SetActive(false);

        // Nonaktifkan kontrol player saat panel misi aktif
        if (PlayerMove != null)
        {
            var motor = PlayerMove.GetComponent<PlayerMotor>();  // Mendapatkan komponen PlayerMotor
            var look = PlayerMove.GetComponent<PlayerLook>();  // Mendapatkan komponen PlayerLook
            var energy = PlayerMove.GetComponent<PlayerEnergy>();  // Mendapatkan komponen PlayerEnergy

            // Menonaktifkan komponen-komponen kontrol player
            if (motor != null) motor.enabled = false;
            if (look != null) look.enabled = false;
            if (energy != null) energy.enabled = false;
        }

        // Menampilkan panel misi dan memulai pengetikan teks intro
        panelMisi.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(textIntro[index]));

        // Mengatur kursor agar tidak terkunci dan terlihat
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Jika panel misi aktif, tunggu input Enter untuk melanjutkan atau hentikan pengetikan
        if (panelMisi.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))  // Jika tombol Enter ditekan
            {
                // Jika ada coroutine pengetikan yang berjalan, hentikan
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    textUI.text = textIntro[index];  // Tampilkan teks lengkap tanpa pengetikan
                    typingCoroutine = null;
                }
                else
                {
                    // Lanjutkan ke teks berikutnya
                    NextText();
                }
            }
            return;
        }

        // Jika tombol P ditekan, tampilkan menu pause
        if (Input.GetKeyDown(KeyCode.P) && pauseMenu != null)
        {
            pauseMenu.GetComponent<PauseMenu>().PauseGame();  // Panggil fungsi untuk pause game
        }
    }

    public void NextText()
    {
        // Jika ada coroutine pengetikan, hentikan
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        index++;  // Pindah ke teks intro berikutnya

        // Jika masih ada teks berikutnya, mulai pengetikan teks
        if (index < textIntro.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(textIntro[index]));
        }
        else
        {
            // Jika sudah selesai dengan semua teks intro, aktifkan tampilan skor dan nonaktifkan panel misi
            if (ScoreText != null) ScoreText.SetActive(true);
            panelMisi.SetActive(false);

            // Aktifkan kembali kontrol player
            if (PlayerMove != null)
            {
                var motor = PlayerMove.GetComponent<PlayerMotor>();
                var look = PlayerMove.GetComponent<PlayerLook>();
                var energy = PlayerMove.GetComponent<PlayerEnergy>();

                if (motor != null) motor.enabled = true;
                if (look != null) look.enabled = true;
                if (energy != null) energy.enabled = true;
            }

            // Kunci kursor di tengah layar dan sembunyikan
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Fungsi untuk mengetikkan teks satu karakter pada satu waktu
    IEnumerator TypeText(string text)
    {
        textUI.text = "";  // Mulai dengan teks kosong
        foreach (char letter in text.ToCharArray())  // Iterasi setiap karakter dalam teks
        {
            textUI.text += letter;  // Tambahkan karakter satu per satu
            yield return new WaitForSecondsRealtime(typingSpeed);  // Tunggu beberapa detik antara karakter
        }
    }
}