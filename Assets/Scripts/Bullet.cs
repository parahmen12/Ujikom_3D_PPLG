using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Hancurkan otomatis setelah waktu tertentu
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        // Cek apakah objek yang kena adalah Boss
        BossMutant boss = collision.gameObject.GetComponent<BossMutant>();
        if (boss != null)
        {
            Debug.Log("Hit Boss! Dealing damage: " + damage);
            boss.TakeDamage((int)damage);
        }

        // Hancurkan peluru setelah tabrakan
        Destroy(gameObject);
    }
}
