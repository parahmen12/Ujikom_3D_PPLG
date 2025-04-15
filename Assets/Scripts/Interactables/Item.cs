using UnityEngine;

public class Item : MonoBehaviour
{
    public AudioClip collectSound; // Suara saat item diambil

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position); // Mainkan suara
            Destroy(gameObject); // Hapus item setelah diambil
        }
    }
}
