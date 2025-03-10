using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    public Transform holdPosition; // Posisi di mana objek dipegang
    public LayerMask grabbableLayer; // Layer untuk objek yang bisa diambil
    public float grabRange = 2f; // Jarak interaksi
    public float placeRange = 2f; // Jarak untuk meletakkan vaksin
    public float exitRange = 3f; // Jarak untuk keluar dari basement
    public GameObject promptMessage; // UI Prompt Message
    public GameObject missionSuccessPanel; // Panel mission success
    public Text distanceText; // UI teks untuk menampilkan jarak ke tempat letakkan vaksin
    public Transform targetMarker; // Target lokasi untuk meletakkan vaksin
    public GameObject placePrompt; // UI untuk "Press E untuk menyimpan vaksin"
    public Transform outOfBasement; // Titik keluar dari basement
    public Text exitDistanceText; // UI teks jarak ke titik keluar

    private GameObject grabbedObject;
    private bool isHolding = false;
    private bool vaccinePlaced = false; // Menyimpan status apakah vaksin sudah diletakkan

    void Start()
    {
        promptMessage.SetActive(false);
        missionSuccessPanel.SetActive(false);
        distanceText.gameObject.SetActive(false);
        placePrompt.SetActive(false);
        exitDistanceText.gameObject.SetActive(false); // Sembunyikan jarak keluar sampai vaksin diletakkan
    }

    void Update()
    {
        CheckForGrabbableObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding)
            {
                TryPlaceObject();
            }
            else
            {
                GrabObject();
            }
        }

        if (isHolding)
        {
            UpdateDistanceToTarget();
        }

        if (vaccinePlaced)
        {
            UpdateDistanceToExit();
        }
    }

    void CheckForGrabbableObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0 && grabbedObject == null)
        {
            promptMessage.SetActive(true);
        }
        else
        {
            promptMessage.SetActive(false);
        }
    }

    void GrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grabRange, grabbableLayer);

        if (hitColliders.Length > 0)
        {
            grabbedObject = hitColliders[0].gameObject;
            grabbedObject.transform.position = holdPosition.position;
            grabbedObject.transform.parent = holdPosition;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            isHolding = true;
            promptMessage.SetActive(false);

            // Ubah teks menjadi "Letakkan vaksin" dan tampilkan jarak ke targetMarker
            distanceText.gameObject.SetActive(true);
            distanceText.text = "Letakkan vaksin";
        }
    }

    void TryPlaceObject()
    {
        float distance = Vector3.Distance(transform.position, targetMarker.position);

        if (distance <= placeRange) // Jika dalam jarak yang bisa meletakkan vaksin
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        isHolding = false;
        vaccinePlaced = true; // Tandai bahwa vaksin telah diletakkan

        // Sembunyikan teks jarak setelah vaksin diletakkan
        distanceText.gameObject.SetActive(false);
        placePrompt.SetActive(false); // Sembunyikan prompt "Press E"

        // Langsung tampilkan teks jarak keluar basement
        exitDistanceText.gameObject.SetActive(true);

        // Tampilkan panel mission success (jika ada kondisi tambahan bisa ditambahkan di sini)
    }

    void UpdateDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetMarker.position);
        distanceText.text = "Letakkan vaksin: " + Mathf.Round(distance) + "m";

        if (distance <= placeRange)
        {
            distanceText.color = Color.green;
            placePrompt.SetActive(true);
        }
        else
        {
            distanceText.color = Color.white;
            placePrompt.SetActive(false);
        }
    }

    void UpdateDistanceToExit()
    {
        float exitDistance = Vector3.Distance(transform.position, outOfBasement.position);
        exitDistanceText.text = "Keluar dari Basement: " + Mathf.Round(exitDistance) + "m";

        if (exitDistance <= exitRange)
        {
            // Jika dalam jarak yang cukup dekat, langsung tampilkan panel Mission Success
            missionSuccessPanel.SetActive(true);
            exitDistanceText.gameObject.SetActive(false); // Sembunyikan teks jarak karena sudah keluar
        }
    }
}
