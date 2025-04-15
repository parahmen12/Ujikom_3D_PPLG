using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public GameObject doorLockedUI;
    public GameObject doorOpenedUI;

    public Transform doorTransform;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public float openSpeed = 2f;

    private bool isOpen = false;
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null && playerInventory.hasAccessCode)
            {
                OpenDoor();
            }
            else
            {
                ShowLockedMessage();
            }
        }

        if (isOpen && doorTransform != null)
        {
            doorTransform.rotation = Quaternion.Lerp(
                doorTransform.rotation,
                Quaternion.Euler(openRotation),
                Time.deltaTime * openSpeed
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            HideMessages();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        HideMessages();
        doorOpenedUI?.SetActive(true);
    }

    void ShowLockedMessage()
    {
        doorLockedUI?.SetActive(true);
    }

    void HideMessages()
    {
        doorLockedUI?.SetActive(false);
        doorOpenedUI?.SetActive(false);
    }
}
