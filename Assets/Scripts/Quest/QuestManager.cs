using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public int syringeCount = 0;  // Pastikan ini public
    public int vaccineCount = 0;  // Pastikan ini ada dan public
    public int totalSyringes = 3; // Jumlah suntikan yang harus diambil
    public Text syringeText; // UI untuk suntikan
    public Text vaccineText; // UI untuk vaccine
    public GameObject missionSuccessPanel; // Panel Mission Success
    public Transform targetMarker; // Tempat meletakkan vaccine

    void Start()
    {
        UpdateQuestUI();
        missionSuccessPanel.SetActive(false);
    }

    public void PickUpSyringe()
    {
        syringeCount++;
        UpdateQuestUI();
    }

    public void PickUpVaccine()
    {
        if (syringeCount >= totalSyringes) // Pastikan sudah mengambil semua suntikan dulu
        {
            vaccineCount++;
            UpdateQuestUI();
        }
    }

    void UpdateQuestUI()
    {
        if (syringeText != null)
            syringeText.text = "Syringe: " + syringeCount + " / " + totalSyringes;

        if (vaccineText != null)
            vaccineText.text = "Vaccine: " + vaccineCount;
    }

    public bool CanPlaceVaccine(Vector3 playerPosition)
    {
        return Vector3.Distance(playerPosition, targetMarker.position) < 2f;
    }

    public void CompleteMission()
    {
        missionSuccessPanel.SetActive(true);
    }
}
