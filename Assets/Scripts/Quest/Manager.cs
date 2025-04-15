using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Intro & UI")]
    public string[] textIntro;
    public Text textUI;
    private int index = 0;
    public GameObject panelMisi;

    [Header("Player & Controls")]
    public GameObject ScoreText;
    public GameObject PlayerMove;
    public GameObject PlayerLook;
    public float typingSpeed = 0.02f;
    private Coroutine typingCoroutine;

    [Header("Player Attack")]
    public GameObject playerAttack;
    public BossMutant bossMutant; // ← Drag BossMutant di Inspector

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    void Start()
    {
        if (ScoreText != null) ScoreText.SetActive(false);

        // Nonaktifkan kontrol player saat panel misi aktif
        if (PlayerMove != null)
        {
            var motor = PlayerMove.GetComponent<PlayerMotor>();
            var look = PlayerMove.GetComponent<PlayerLook>();
            var energy = PlayerMove.GetComponent<PlayerEnergy>();

            if (motor != null) motor.enabled = false;
            if (look != null) look.enabled = false;
            if (energy != null) energy.enabled = false;
        }

        // Nonaktifkan player attack
        if (playerAttack != null)
        {
            var attack = playerAttack.GetComponent<PlayerAttack>();
            if (attack != null) attack.enabled = false;
        }

        panelMisi.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(textIntro[index]));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (panelMisi.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    textUI.text = textIntro[index];
                    typingCoroutine = null;
                }
                else
                {
                    NextText();
                }
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.P) && pauseMenu != null)
        {
            pauseMenu.GetComponent<PauseMenu>().PauseGame();
        }
    }

    public void NextText()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        index++;
        if (index < textIntro.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(textIntro[index]));
        }
        else
        {
            if (ScoreText != null) ScoreText.SetActive(true);
            panelMisi.SetActive(false);

            // Aktifkan kontrol player
            if (PlayerMove != null)
            {
                var motor = PlayerMove.GetComponent<PlayerMotor>();
                var look = PlayerMove.GetComponent<PlayerLook>();
                var energy = PlayerMove.GetComponent<PlayerEnergy>();

                if (motor != null) motor.enabled = true;
                if (look != null) look.enabled = true;
                if (energy != null) energy.enabled = true;
            }

            // Aktifkan player attack
            if (playerAttack != null)
            {
                var attack = playerAttack.GetComponent<PlayerAttack>();
                if (attack != null)
                {
                    attack.enabled = true;
                    Debug.Log("PlayerAttack berhasil diaktifkan");
                }
            }

            // Aktifkan boss
            if (bossMutant != null)
            {
                bossMutant.ActivateBoss();
            }

            // Kunci cursor ke tengah & sembunyikan
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    IEnumerator TypeText(string text)
    {
        textUI.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textUI.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
