using UnityEngine;

public class AccessCodeItem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pickupPromptUI;
    public GameObject accessCodePanel;

    [Header("Player Settings")]
    public string playerTag = "Player";

    private bool playerInRange = false;
    private bool isCollected = false;

    private PlayerInventory playerInventory;

    void Update()
    {
        if (playerInRange && !isCollected && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            pickupPromptUI?.SetActive(true);

            // Cari komponen inventory di player
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            pickupPromptUI?.SetActive(false);
        }
    }

    void CollectItem()
    {
        isCollected = true;
        pickupPromptUI?.SetActive(false);
        accessCodePanel?.SetActive(true);

        // Beri akses ke player melalui inventory
        if (playerInventory != null)
        {
            playerInventory.GiveAccessCode();
        }

        Destroy(gameObject);
    }
}
