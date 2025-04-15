using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasAccessCode = false;

    // Fungsi ini bisa dipanggil dari item yang dikoleksi
    public void GiveAccessCode()
    {
        hasAccessCode = true;
        Debug.Log("Player now has the access code!");
    }
}
