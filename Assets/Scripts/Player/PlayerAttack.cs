using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 30f;
    public float fireRate = 0.5f;

    [Header("Audio")]
    public AudioSource gunshotAudio;

    private float lastFireTime;

    void Update()
    {
        bool isFiring = Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"); 
        // "Fire1" default-nya juga mendukung gamepad trigger kiri (joystick)

        if (isFiring && Time.time >= lastFireTime + fireRate)
        {
            if (bulletPrefab == null || firePoint == null)
            {
                Debug.LogWarning("BulletPrefab atau FirePoint belum diisi di Inspector!");
                return;
            }

            Shoot();
            lastFireTime = Time.time;
        }
    }

    void Shoot()
    {
        Debug.Log("Shoot function called");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab tidak memiliki Rigidbody!");
        }

        if (gunshotAudio != null)
        {
            gunshotAudio.Play();
        }
    }
}
