using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public Image barEnergy;
    public float maxEnergy = 100f;
    private float energiSekarang;
    public float rateEnergyBerkurangi = 10f; // Energi berkurang per detik saat sprint
    public float rateEnergyBertambah = 5f;   // Energi bertambah per detik saat tidak sprint
    private PlayerMotor playerMotor; 

    void Start()
    {
        energiSekarang = maxEnergy;
        playerMotor = GetComponent<PlayerMotor>(); // Ambil referensi ke PlayerMotor
        UpdateBar();
    }

    void Update()
    {
        if (playerMotor != null && playerMotor.IsSprinting())
        {
            KurangiEnergi(rateEnergyBerkurangi * Time.deltaTime);
        }
        else
        {
            TambahEnergi(rateEnergyBertambah * Time.deltaTime);
        }

        if (energiSekarang <= 0)
        {
            playerMotor.ForceWalk(); // Paksa pemain berjalan saat energi habis
        }
    }

    void UpdateBar()
    {
        barEnergy.fillAmount = energiSekarang / maxEnergy;
    }

    public void KurangiEnergi(float amount)
    {
        energiSekarang -= amount;
        if (energiSekarang < 0)
        {
            energiSekarang = 0;
        }
        UpdateBar();
    }

    public void TambahEnergi(float amount)
    {
        energiSekarang += amount;
        if (energiSekarang > maxEnergy)
        {
            energiSekarang = maxEnergy;
        }
        UpdateBar();
    }

    public float GetCurrentEnergy()
{
    return energiSekarang;
}

}
