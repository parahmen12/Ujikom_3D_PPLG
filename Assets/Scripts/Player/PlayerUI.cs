using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    void Awake()
    {
        // Jika promptText belum di-assign di Inspector, coba cari secara otomatis
        if (promptText == null)
        {
            promptText = GetComponentInChildren<TextMeshProUGUI>();
            if (promptText == null)
            {
                Debug.LogError("PlayerUI: Tidak dapat menemukan komponen TextMeshProUGUI! Pastikan sudah di-assign di Inspector.");
            }
        }
    }

    public void UpdateText(string promptMessage)
    {
        if (promptText == null)
        {
            Debug.LogError("PlayerUI: promptText belum di-assign! Pastikan diatur di Inspector.");
            return;
        }

        promptText.text = promptMessage;
    }
}
