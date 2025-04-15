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

    private bool isDead = false;

    // Game Over panel
    public GameObject gameOverPanel;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        UpdateHealthUI();
        gameOverPanel.SetActive(false); // Ensure the game over panel is inactive initially
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        HandleDamageOverlay();
        
        // Check if health falls below 20 and show the Game Over panel
        if (health < 20f && !isDead)
        {
            ShowGameOver();
        }
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
        if (isDead) return;

        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        UpdateHealthUI();

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
    }

    void ShowGameOver()
    {
        if (health < 20f && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);  // Show Game Over panel when health is below 20
        }
    }

    void Die()
    {
        FindObjectOfType<GameOver>()?.ShowGameOver(); // Null check
        Debug.Log("Player mati!");
        // Tambahkan animasi mati / disable control jika perlu
    }
}
