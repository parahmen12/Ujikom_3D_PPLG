using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] textIntro;
    public Text textUI;
    private int index = 0;
    public GameObject panelMisi;
    public GameObject PlayerMove;
    public float typingSpeed = 0.02f; // Lebih cepat dan smooth
    private Coroutine typingCoroutine;

    void Start()
    {
        panelMisi.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(textIntro[index]));
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
            panelMisi.SetActive(false);
            PlayerMove.GetComponent<PlayerMotor>().enabled = true;
            PlayerMove.GetComponent<PlayerLook>().enabled = true;
            PlayerMove.GetComponent<PlayerEnergy>().enabled = true;
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
