using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelManager : MonoBehaviour
{
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        // Cek apakah level 2 sudah terbuka
        if (PlayerPrefs.GetInt("Level2Unlocked", 0) == 1)
        {
            level2Button.interactable = true;
        }
        else
        {
            level2Button.interactable = false;
        }

        // Cek apakah level 3 sudah terbuka
        if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 1)
        {
            level3Button.interactable = true;
        }
        else
        {
            level3Button.interactable = false;
        }
    }
}
