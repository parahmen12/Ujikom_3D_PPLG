using System.Collections;
using UnityEngine;

// Pastikan GameObject memiliki komponen CharacterController
[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller; // Komponen CharacterController untuk mengatur pergerakan karakter
    private Vector3 playerVelocity; // Kecepatan pemain

    [Header("Player Settings")]
    public float gravity = -9.8f; // Gaya gravitasi
    public float walkSpeed = 5f; // Kecepatan berjalan
    public float sprintSpeed = 8f; // Kecepatan sprint
    public float crouchSpeed = 2.5f; // Kecepatan saat crouch
    public float jumpHeight = 3f; // Tinggi lompatan

    [Header("Crouch Settings")]
    public float crouchHeight = 1f; // Tinggi karakter saat crouch
    public float standingHeight = 2f; // Tinggi karakter saat berdiri
    public float crouchTransitionSpeed = 5f; // Kecepatan transisi saat crouch

    private float speed; // Variabel untuk kecepatan pemain
    private bool isGrounded; // Mengecek apakah pemain menyentuh tanah
    private bool isSprinting = false; // Mengecek apakah pemain sedang sprint
    private bool isCrouching = false; // Mengecek apakah pemain sedang crouch
    private PlayerEnergy playerEnergy; // Referensi ke sistem energi pemain

    void Awake()
    {
        // Mengambil komponen CharacterController
        controller = GetComponent<CharacterController>();
        playerEnergy = GetComponent<PlayerEnergy>();

        // Validasi jika komponen tidak ditemukan
        if (controller == null)
        {
            Debug.LogError("CharacterController tidak ditemukan! Pastikan komponen ini ada di GameObject.");
        }
        if (playerEnergy == null)
        {
            Debug.LogError("PlayerEnergy tidak ditemukan! Pastikan komponen ini ada di GameObject.");
        }

        Debug.Log("Awake() dipanggil: Komponen telah diinisialisasi.");
    }

    void Start()
    {
        speed = walkSpeed; // Mengatur kecepatan awal pemain
        Debug.Log("Start() dipanggil: Kecepatan awal diset ke " + speed);
    }

    void Update()
    {
        isGrounded = controller != null && controller.isGrounded; // Mengecek apakah pemain menyentuh tanah
        HandleEnergySystem(); // Mengatur sistem energi saat sprint
    }

    private void HandleEnergySystem()
    {
        if (isSprinting && !isCrouching) // Sprint hanya bisa dilakukan jika tidak crouch
        {
            playerEnergy.KurangiEnergi(10f * Time.deltaTime); // Mengurangi energi saat sprint
            Debug.Log("Sedang Sprint: Energi berkurang.");

            if (playerEnergy.GetCurrentEnergy() <= 0) // Jika energi habis, hentikan sprint
            {
                Debug.Log("Energi habis, berhenti sprint.");
                ForceWalk();
            }
        }
        else
        {
            playerEnergy.TambahEnergi(5f * Time.deltaTime); // Menambah energi saat tidak sprint
            Debug.Log("Tidak sprint: Energi bertambah.");
        }
    }

    // Fungsi untuk sprint
    public void Sprint(bool state)
    {
        if (state && !isCrouching && playerEnergy.GetCurrentEnergy() > 0) // Cek apakah sedang crouch
        {
            isSprinting = true;
            speed = sprintSpeed; // Mengatur kecepatan menjadi sprint
            Debug.Log("Sprint dimulai. Kecepatan: " + speed);
        }
        else
        {
            Debug.Log("Sprint dibatalkan.");
            ForceWalk(); // Jika tidak bisa sprint, kembali ke berjalan
        }
    }

    // Fungsi untuk crouch
    public void Crouch()
    {
        isCrouching = !isCrouching; // Toggle crouch
        Debug.Log(isCrouching ? "Mulai crouch" : "Berdiri kembali");

        if (isCrouching)
        {
            speed = crouchSpeed; // Mengatur kecepatan saat crouch
            StartCoroutine(CrouchTransition(crouchHeight)); // Animasi transisi crouch
        }
        else
        {
            speed = walkSpeed; // Mengembalikan kecepatan normal
            StartCoroutine(CrouchTransition(standingHeight)); // Animasi kembali berdiri
        }
    }

    // Coroutine untuk transisi tinggi karakter saat crouch
    private IEnumerator CrouchTransition(float targetHeight)
    {
        float currentHeight = controller.height; // Simpan tinggi saat ini
        float time = 0f; // Waktu transisi, dimulai dari 0
        Debug.Log("Mulai transisi crouch ke tinggi: " + targetHeight);

        while (time < 1f)
        {
            time += Time.deltaTime * crouchTransitionSpeed; // Meningkatkan waktu transisi berdasarkan kecepatan yang ditentukan
            controller.height = Mathf.Lerp(currentHeight, targetHeight, time); // Smooth transisi tinggi karakter
            yield return null; // Menunggu frame berikutnya sebelum melanjutkan perubahan
        }

        Debug.Log("Transisi crouch selesai.");
    }

    // Fungsi untuk kembali ke mode berjalan normal
    public void ForceWalk()
    {
        isSprinting = false;
        speed = walkSpeed;
        Debug.Log("Kembali ke mode berjalan. Kecepatan: " + speed);
    }

    // Mengecek apakah pemain sedang sprint
    public bool IsSprinting()
    {
        Debug.Log("Cek status sprint: " + isSprinting);
        return isSprinting;
    }

    // Mengatur pergerakan pemain berdasarkan input dari player
    public void ProcessMove(Vector2 input)
    {
        if (controller == null)
        {
            Debug.LogError("CharacterController tidak ditemukan! Pastikan komponen ini ada di GameObject.");
            return;
        }

        Vector3 moveDirection = new Vector3(input.x, 0, input.y); // Mengambil input pergerakan
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime); // Menggerakkan karakter sesuai input
        
        Debug.Log("Pergerakan: X = " + input.x + ", Y = " + input.y);

        if (isGrounded && playerVelocity.y < 0) // Jika pemain di tanah, reset kecepatan jatuh
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime; // Terapkan gravitasi
        controller.Move(playerVelocity * Time.deltaTime); // Terapkan pergerakan akibat gravitasi
    }

    // Fungsi untuk melompat
    public void Jump()
    {
        if (isGrounded && !isCrouching) // Tidak bisa lompat saat crouch
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity); // Hitung kecepatan lompatan
            Debug.Log("Lompat! Kecepatan vertikal: " + playerVelocity.y);
        }
        else
        {
            Debug.Log("Tidak bisa lompat! Pastikan karakter di tanah dan tidak crouch.");
        }
    }
}
