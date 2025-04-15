using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public Image barEnergy; // UI bar untuk menunjukkan energi pemain
    public float maxEnergy = 100f; // Maksimum energi yang bisa dimiliki pemain
    private float energiSekarang; // Nilai energi saat ini
    public float rateEnergyBerkurangi = 10f; // Kecepatan pengurangan energi per detik saat sprint
    public float rateEnergyBertambah = 5f;   // Kecepatan pemulihan energi saat tidak sprint
    private PlayerMotor playerMotor; // Referensi ke skrip PlayerMotor untuk mengontrol pergerakan

    void Start()
    {
        energiSekarang = maxEnergy; // Set energi awal ke maksimum
        playerMotor = GetComponent<PlayerMotor>(); // Ambil referensi ke PlayerMotor
        UpdateBar(); // Perbarui tampilan UI bar energi
    }

    void Update()
    {
        if (playerMotor != null && playerMotor.IsSprinting()) // Jika pemain sedang sprint
        {
            KurangiEnergi(rateEnergyBerkurangi * Time.deltaTime); // Kurangi energi
        }
        else // Jika pemain tidak sprint
        {
            TambahEnergi(rateEnergyBertambah * Time.deltaTime); // Pulihkan energi
        }

        if (energiSekarang <= 0) // Jika energi habis
        {
            playerMotor.ForceWalk(); // Paksa pemain berjalan (tidak bisa sprint)
        }
    }

    void UpdateBar()
    {
        barEnergy.fillAmount = energiSekarang / maxEnergy; // Perbarui tampilan bar energi
    }

    public void KurangiEnergi(float amount)
    {
        energiSekarang -= amount; // Kurangi energi sebesar amount
        if (energiSekarang < 0) // Jika energi kurang dari 0, set menjadi 0
        {
            energiSekarang = 0;
        }
        UpdateBar(); // Perbarui tampilan bar energi
    }

    public void TambahEnergi(float amount)
    {
        energiSekarang += amount; // Tambah energi sebesar amount
        if (energiSekarang > maxEnergy) // Jika energi melebihi maksimum, set ke maksimum
        {
            energiSekarang = maxEnergy;
        }
        UpdateBar(); // Perbarui tampilan bar energi
    }

    public float GetCurrentEnergy()
    {
        return energiSekarang; // Mengembalikan nilai energi saat ini
    }
}
