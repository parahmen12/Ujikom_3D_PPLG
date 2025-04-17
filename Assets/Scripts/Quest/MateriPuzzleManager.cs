using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Script utama untuk mengelola sistem puzzle edukasi
public class MateriPuzzleManager : MonoBehaviour
{
    // Kelas data untuk menyimpan informasi per puzzle/kotak materi
    [System.Serializable]
    public class PuzzleData
    {
        public string namaPuzzle;                          // Nama Panel tiap puzzle
        public TextMeshProUGUI[] wordSlots;                // Slot tempat pemain menyusun kata
        public Button[] wordButtons;                       // Tombol-tombol berisi kata acak
        public string[] correctWords;                      // Jawaban benar dalam urutan
        public TextMeshProUGUI messageText;                // Pesan status untuk pemain

        [HideInInspector] public string[] shuffledWords;   // Kata acak yang ditampilkan (acak dari correctWords)
        [HideInInspector] public bool puzzleSelesai = false; // Menandai apakah puzzle ini sudah diselesaikan
    }

    [Header("List Puzzle")]
    public PuzzleData[] puzzles; // Daftar semua puzzle yang ada di scene

    [Header("UI Panel")]
    public GameObject selesaiPanel; // Panel muncul jika semua puzzle berhasil
    public GameObject salahPanel;   // Panel muncul jika pemain salah menjawab

    // Fungsi dijalankan saat game dimulai
    void Start()
    {
        // Untuk setiap puzzle dalam list...
        foreach (var puzzle in puzzles)
        {
            InitPuzzle(puzzle); // Inisialisasi puzzle (acak kata, set ulang UI)

            // Tampilkan pesan awal pada setiap puzzle
            if (puzzle.messageText != null)
                puzzle.messageText.text = "Pecahkan puzzle materi ini!";
        }

        // Pastikan panel selesai dan salah tidak aktif di awal
        if (selesaiPanel != null) selesaiPanel.SetActive(false);
        if (salahPanel != null) salahPanel.SetActive(false);
    }

    // Fungsi untuk inisialisasi ulang puzzle
    void InitPuzzle(PuzzleData puzzle)
    {
    // Salin isi correctWords ke shuffledWords (kata jawaban yang akan diacak)
    puzzle.shuffledWords = (string[])puzzle.correctWords.Clone();

    // Acak urutan kata di shuffledWords
    ShuffleWords(puzzle.shuffledWords); // Panggil fungsi untuk mengacak kata

    // Reset semua slot jawaban jadi kosong dan ubah warnanya ke putih
    for (int i = 0; i < puzzle.wordSlots.Length; i++)
    {
        puzzle.wordSlots[i].text = "";           // Kosongkan teks di slot
        puzzle.wordSlots[i].color = Color.white; // Warnai slot jadi putih (neutral)
    }

    // Siapkan ulang semua tombol kata (wordButtons)
    for (int i = 0; i < puzzle.wordButtons.Length; i++)
    {
        // Kalau index tombol masih dalam jumlah kata acak
        if (i < puzzle.shuffledWords.Length)
        {
            int index = i; // Simpan indeks sebagai variabel lokal agar bisa dipakai dalam listener

            puzzle.wordButtons[i].gameObject.SetActive(true);  // Aktifkan tombolnya (tampil di UI)
            puzzle.wordButtons[i].interactable = true;         // Buat tombol bisa diklik

            // Tampilkan kata acak ke dalam teks tombol
            puzzle.wordButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = puzzle.shuffledWords[i];

            // Hapus semua fungsi klik sebelumnya agar tidak dobel
            puzzle.wordButtons[i].onClick.RemoveAllListeners();

            // Tambahkan listener baru: saat diklik, jalankan PlaceWordInSlot()
            puzzle.wordButtons[i].onClick.AddListener(() => PlaceWordInSlot(puzzle, index));
        }
        else
        {
            // Kalau tidak ada kata untuk tombol ini, sembunyikan
            puzzle.wordButtons[i].gameObject.SetActive(false);
        }
    }
}

    // Fungsi untuk mengacak urutan kata menggunakan algoritma Fisher-Yates
    void ShuffleWords(string[] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            int rand = Random.Range(i, words.Length); // Ambil indeks acak
            (words[i], words[rand]) = (words[rand], words[i]); // Tukar posisi
        }
    }

    // Fungsi ketika tombol kata ditekan, lalu dimasukkan ke slot
    void PlaceWordInSlot(PuzzleData puzzle, int index)
    {
        if (puzzle.puzzleSelesai) return; // Jika sudah selesai, jangan izinkan input lagi

        // Ambil kata dari tombol
        string selected = puzzle.wordButtons[index].GetComponentInChildren<TextMeshProUGUI>().text;

        // Masukkan kata ke slot pertama yang kosong
        for (int i = 0; i < puzzle.wordSlots.Length; i++)
        {
            if (string.IsNullOrEmpty(puzzle.wordSlots[i].text))
            {
                puzzle.wordSlots[i].text = selected;
                puzzle.wordSlots[i].color = Color.white; // Reset warna
                puzzle.wordButtons[index].interactable = false; // Disable tombol
                break;
            }
        }

        // Setelah isi, langsung cek apakah susunan benar
        CheckPuzzle(puzzle);
    }

    // Fungsi untuk memeriksa apakah susunan jawaban benar
    void CheckPuzzle(PuzzleData puzzle)
    {
        // Jika masih ada slot kosong, jangan periksa dulu
        foreach (var slot in puzzle.wordSlots)
        {
            if (string.IsNullOrEmpty(slot.text)) return;
        }

        bool benar = true;

        // Cek tiap kata di slot apakah sesuai jawaban
        for (int i = 0; i < puzzle.wordSlots.Length; i++)
        {
            if (puzzle.wordSlots[i].text != puzzle.correctWords[i])
            {
                puzzle.wordSlots[i].color = Color.red; // Salah -> merah
                benar = false;
            }
            else
            {
                puzzle.wordSlots[i].color = Color.green; // Benar -> hijau
            }
        }

        if (benar)
        {
            // Jika semua susunan kata benar...

            puzzle.puzzleSelesai = true; // Tandai bahwa puzzle ini sudah berhasil diselesaikan (tidak bisa diubah lagi)

            if (puzzle.messageText != null) // Jika ada objek teks untuk menampilkan pesan...
                puzzle.messageText.text = "Benar!"; // Tampilkan pesan ke pemain bahwa jawabannya benar

            CekSemuaPuzzle(); // Cek apakah semua puzzle sudah selesai
        }
        else
        {

            // Kalau salah, tampilkan panel salah dan reset otomatis
            if (puzzle.messageText != null)
                puzzle.messageText.text = "Salah, coba lagi!";
            if (salahPanel != null) salahPanel.SetActive(true);
            Debug.Log("Game Over");


            StartCoroutine(ResetPuzzleCoroutine(puzzle)); // Jalankan reset otomatis
        }
    }

    // Coroutine untuk mereset puzzle setelah 2 detik
    IEnumerator ResetPuzzleCoroutine(PuzzleData puzzle)
    {
        yield return new WaitForSeconds(2f); // Tunggu 2 detik

        InitPuzzle(puzzle); // Reset ulang puzzle

        if (salahPanel != null)
            salahPanel.SetActive(false); // Sembunyikan panel salah

        if (puzzle.messageText != null)
            puzzle.messageText.text = "Pecahkan puzzle materi ini!"; // Reset pesan
    }

    // Fungsi untuk memeriksa apakah semua puzzle sudah diselesaikan
    void CekSemuaPuzzle()
    {
        foreach (var puzzle in puzzles)
        {
            if (!puzzle.puzzleSelesai) return; // Jika masih ada yang belum, keluar dulu
        }

        // Jika semua sudah selesai, tampilkan panel selesai
        if (selesaiPanel != null)
            selesaiPanel.SetActive(true);
    }

    // Fungsi tambahan untuk reset manual puzzle tertentu dari tombol
    public void ResetPuzzleManual(int index)
    {
        if (index >= 0 && index < puzzles.Length)
        {
            InitPuzzle(puzzles[index]); // Reset ulang

            if (puzzles[index].messageText != null)
                puzzles[index].messageText.text = "Puzzle direset";
        }
    }
    // Fungsi untuk menutup panel UI (dari tombol close)
    public void ClosePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // Fungsi ketika player ingin lanjut ke Level 1 setelah menyelesaikan semua puzzle
    public void LanjutKeLevel1()
    {
        PlayerPrefs.SetInt("Level1Unlocked", 1); // Simpan bahwa level 1 sudah terbuka
        PlayerPrefs.Save(); // Simpan ke storage
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home"); // Pindah ke scene Home
    }
}