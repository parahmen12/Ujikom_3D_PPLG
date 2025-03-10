using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Tambahkan ini

public class Soal : MonoBehaviour
{
    public string soal;
    public string jawaban;

    public Text teksSoal;
    public InputField teksJawaban;

    private bool dekatDenganPemain;
    public GameObject player;
    public GameObject panelPertanyaan;

    void Start()
    {
        panelPertanyaan.SetActive(false);
    }

    void Update()
    {
        // Gunakan Input System baru
        if (dekatDenganPemain && Keyboard.current.eKey.wasPressedThisFrame)
        {
            panelPertanyaan.SetActive(true);
            teksSoal.text = soal;
            player.GetComponent<PlayerMotor>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dekatDenganPemain = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dekatDenganPemain = false;
        }
    }

    public void CekJawaban()
    {
        if (teksJawaban.text.Trim().ToLower() == jawaban.ToLower()) 
        {
            panelPertanyaan.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<PlayerMotor>().enabled = true;
            Destroy(gameObject);
        }
    }
}
