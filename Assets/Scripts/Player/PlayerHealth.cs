using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Health Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration = 1f;
    public float fadeSpeed = 2f;
    private float durationTimer;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        UpdateHealthUI();
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        HandleDamageOverlay();
    }

    void HandleDamageOverlay()
    {
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a - (Time.deltaTime * fadeSpeed);
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.Clamp01(tempAlpha));
            }
        }
    }

    public void UpdateHealthUI()
    {
        float hFraction = health / maxHealth;
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime * chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, lerpTimer);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime * chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, lerpTimer);
        }
        else
        {
            lerpTimer = 0;
        }

        healthText.text = Mathf.RoundToInt(health) + " / " + maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (health == 10)
        {
            FindObjectOfType<GameOver>().ShowGameOver();
        }

        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        UpdateHealthUI();
    }
}
