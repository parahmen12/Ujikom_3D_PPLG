using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    // Posisi objek dipegang oleh pemain
    public Transform holdPosition; 

    // Layer objek yang bisa diambil
    public LayerMask grabbableLayer; 

    // Jarak maksimum untuk mengambil objek
    public float grabRange = 2f; 
    
    // Jarak maksimum untuk meletakkan vaksin
    public float placeRange = 2f; 
    
    // Jarak maksimum untuk keluar dari basement
    public float exitRange = 3f; 

    // UI untuk menampilkan pesan interaksi
    public GameObject promptMessage; 
    
    // Panel yang muncul jika misi sukses
    public GameObject missionSuccessPanel; 

    // UI teks untuk menampilkan jarak ke lokasi vaksin
    public Text distanceText; 
    
    // Penanda lokasi tempat meletakkan vaksin
    public Transform targetMarker; 
    
    // UI untuk "Press E untuk menyimpan vaksin"
    public GameObject placePrompt; 
    
    // Titik keluar dari basement
    public Transform outOfBasement; 
    
    // UI teks jarak ke titik keluar
    public Text exitDistanceText; 

    // Objek yang sedang dipegang pemain
    private GameObject grabbedObject; 
    
    // Status apakah pemain sedang memegang objek
    private bool isHolding = false; 
    
    // Status apakah vaksin sudah diletakkan
    private bool vaccinePlaced = false; 

    void Start()
    {
        // Menyembunyikan UI pada awal permainan
        promptMessage.SetActive(false);
        missionSuccessPanel.SetActive(false);
        distanceText.gameObject.SetActive(false);
        placePrompt.SetActive(false);
        exitDistanceText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Mengecek apakah ada objek yang bisa diambil
        CheckForGrabbableObject();

        // Jika pemain menekan tombol "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding)
            {
                // Jika sedang memegang objek, coba letakkan
                TryPlaceObject();
            }
            else
            {
                // Jika tidak memegang objek, coba ambil objek
                GrabObject();
            }
        }

        // Jika pemain sedang memegang objek, tampilkan jarak ke lokasi target
        if (isHolding)
        {
            UpdateDistanceToTarget();
        }

        // Jika vaksin sudah diletakkan, tampilkan jarak ke pintu keluar
        if (vaccinePlaced)
        {
            UpdateDistanceToExit();
        }
    }

    // Memastikan objek selalu berada di posisi tangan saat dipegang
    void LateUpdate()
    {
        if (isHolding && grabbedObject != null)
        {
            grabbedObject.transform.position = holdPosition.position;
            grabbedObject.transform.rotation = holdPosition.rotation;
        }
    }

    // Mengecek apakah ada objek yang bisa diambil dalam radius grabRange
    void CheckForGrabbableObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0 && grabbedObject == null)
        {
            promptMessage.SetActive(true); // Tampilkan UI "Press E"
        }
        else
        {
            promptMessage.SetActive(false); // Sembunyikan UI jika tidak ada objek
        }
    }

    // Mengambil objek dalam jangkauan
    void GrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0)
        {
            grabbedObject = hitColliders[0].gameObject;
            isHolding = true;

            // Menonaktifkan physics agar objek tidak jatuh
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true; 

            // Menjadikan objek sebagai anak dari holdPosition
            grabbedObject.transform.SetParent(holdPosition); 

            // Menyembunyikan UI prompt dan menampilkan jarak ke target
            promptMessage.SetActive(false);
            distanceText.gameObject.SetActive(true);
            distanceText.text = "Letakkan vaksin";
        }
    }

    // Mengecek apakah pemain bisa meletakkan objek
    void TryPlaceObject()
    {
        float distance = Vector3.Distance(transform.position, targetMarker.position);

        if (distance <= placeRange) // Jika dalam jarak placeRange
        {
            PlaceObject(); // Panggil fungsi meletakkan vaksin
        }
    }

    // Meletakkan vaksin di lokasi target
    void PlaceObject()
    {
        // Melepaskan objek dari tangan
        grabbedObject.transform.SetParent(null); 

        // Mengaktifkan physics kembali agar objek tidak diam di udara
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false; 

        // Mengupdate status
        isHolding = false;
        vaccinePlaced = true;

        // Menyembunyikan UI yang tidak diperlukan
        distanceText.gameObject.SetActive(false);
        placePrompt.SetActive(false);
        
        // Menyembunyikan target marker setelah vaksin diletakkan
        targetMarker.gameObject.SetActive(false);

        // Menampilkan UI jarak ke pintu keluar basement
        exitDistanceText.gameObject.SetActive(true);
    }

    // Menampilkan jarak ke lokasi target vaksin
    void UpdateDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetMarker.position);
        distanceText.text = "Letakkan vaksin: " + Mathf.Round(distance) + "m";

        if (distance <= placeRange)
        {
            distanceText.color = Color.green; // Warna hijau jika dekat target
            placePrompt.SetActive(true); // Tampilkan UI "Press E"
        }
        else
        {
            distanceText.color = Color.white; // Warna normal
            placePrompt.SetActive(false); // Sembunyikan UI
        }
    }

    // Menampilkan jarak ke pintu keluar basement
    void UpdateDistanceToExit()
    {
        float exitDistance = Vector3.Distance(transform.position, outOfBasement.position);
        exitDistanceText.text = "Keluar dari Basement: " + Mathf.Round(exitDistance) + "m";

        if (exitDistance <= exitRange)
        {
            // Jika dalam jarak cukup dekat, tampilkan panel Mission Success
            missionSuccessPanel.SetActive(true);
            exitDistanceText.gameObject.SetActive(false); // Sembunyikan teks jarak

            Cursor.lockState = CursorLockMode.None; // Bebaskan kursor
            Cursor.visible = true; // Tampilkan kursor
        }
    }
}
