using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Soal : MonoBehaviour
{
    [Header("Soal dan Jawaban")]
    public string soal; // Pertanyaan yang akan ditampilkan
    public string jawaban; // Jawaban yang benar

    [Header("UI Elements")]
    public Text teksSoal; // Objek UI untuk menampilkan soal
    public InputField teksJawaban; // Input untuk jawaban
    public GameObject panelPertanyaan; // Panel yang berisi soal dan input jawaban
    public Button submitButton; // Tombol untuk mengirim jawaban
    public Text interactText; // UI untuk menampilkan teks interaksi "Tekan E untuk menjawab"

    [Header("Player & Interaction")]
    public GameObject player; // Referensi ke objek pemain
    public float interactRange = 3f; // Jarak interaksi pemain dengan objek
    private bool dekatDenganPemain; // Menyimpan status apakah pemain dalam jarak interaksi
    private bool soalSudahDijawab = false; // Menandai apakah soal sudah dijawab

    void Start()
    {
        panelPertanyaan.SetActive(false); // Sembunyikan panel pertanyaan di awal
        interactText.gameObject.SetActive(false); // Sembunyikan teks interaksi
        submitButton.onClick.AddListener(CekJawaban); // Tambahkan event klik pada tombol submit
    }

    void Update()
    {
        // Jika soal sudah dijawab, hentikan pengecekan interaksi
        if (soalSudahDijawab) return;

        // Hitung jarak antara pemain dan objek soal
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Jika pemain berada dalam jarak interaksi dan panel pertanyaan belum aktif, tampilkan teks interaksi
        if (distance <= interactRange && !panelPertanyaan.activeSelf)
        {
            dekatDenganPemain = true;
            interactText.gameObject.SetActive(true);
        }
        else
        {
            dekatDenganPemain = false;
            interactText.gameObject.SetActive(false);
        }

        // Jika pemain menekan tombol "E" saat berada dalam jarak interaksi, tampilkan soal
        if (dekatDenganPemain && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TampilkanSoal();
        }
    }

    // Fungsi untuk menampilkan soal ke pemain
    private void TampilkanSoal()
    {
        panelPertanyaan.SetActive(true); // Tampilkan panel pertanyaan
        teksSoal.text = soal; // Tampilkan teks soal
        teksJawaban.text = ""; // Kosongkan input jawaban sebelumnya
        teksJawaban.ActivateInputField(); // Fokuskan ke InputField agar langsung bisa mengetik

        // Sembunyikan teks interaksi
        interactText.gameObject.SetActive(false);

        // Nonaktifkan kontrol pemain agar tidak bisa bergerak saat menjawab soal
        player.GetComponent<PlayerMotor>().enabled = false;

        // Tampilkan kursor agar pemain bisa menggunakan UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Fokuskan input ke input field agar pemain bisa langsung mengetik jawaban
        EventSystem.current.SetSelectedGameObject(teksJawaban.gameObject);
    }

    // Fungsi untuk memeriksa jawaban pemain
    private void CekJawaban()
    {
        // Bandingkan jawaban yang dimasukkan dengan jawaban yang benar (abaikan huruf besar/kecil dan spasi ekstra)
        if (teksJawaban.text.Trim().ToLower() == jawaban.ToLower())
        {
            SelesaikanSoal();
        }
    }

    // Fungsi untuk menyelesaikan soal jika jawaban benar
    private void SelesaikanSoal()
    {
        panelPertanyaan.SetActive(false); // Sembunyikan panel pertanyaan

        // Aktifkan kembali kontrol pemain
        player.GetComponent<PlayerMotor>().enabled = true;

        // Tandai bahwa soal sudah dijawab agar teks interaksi tidak muncul lagi
        soalSudahDijawab = true;
        interactText.gameObject.SetActive(false);

        // Hapus objek soal setelah berhasil dijawab
        Destroy(gameObject);
    }
}
