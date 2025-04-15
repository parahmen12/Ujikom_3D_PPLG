using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossMutant : MonoBehaviour
{
    [Header("Boss UI")]
    public GameObject bossUI;
    public Image bossHealthFill;
    public Text bossNameText;
    public string bossName = "Boss Mutant";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip roarClip;
    private bool isWalkingSoundPlaying = false;

    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float chaseRange = 15f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    [Header("Movement Settings")]
    public float chaseSpeed = 4f;

    [Header("References")]
    public Animator animator;
    private Transform player;
    private NavMeshAgent agent;
    public GameObject dropItem;

    private bool isActive = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = chaseSpeed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene!");
        }

        currentHealth = maxHealth;

        if (bossNameText != null)
            bossNameText.text = bossName;

        if (bossUI != null)
            bossUI.SetActive(false); // Jangan tampilkan UI di awal sebelum diaktifkan

        // Uncomment ini jika boss harus aktif dari awal
        // ActivateBoss();
    }

    void Update()
    {
        if (!isActive || isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Aktifkan UI Boss jika sudah dekat dengan player
        if (distance <= chaseRange && bossUI != null && !bossUI.activeSelf)
        {
            bossUI.SetActive(true);
        }

        if (distance <= attackRange)
        {
            HandleAttack();
        }
        else if (distance <= chaseRange)
        {
            HandleChase();
        }
        else
        {
            HandleIdle();
        }

        if (animator != null && agent != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void HandleChase()
{
    if (agent == null || animator == null) return;

    agent.isStopped = false;
    agent.speed = chaseSpeed;
    agent.SetDestination(player.position);

    animator.SetBool("isWalking", true);
    animator.SetBool("isAttacking", false);

    if (!isWalkingSoundPlaying && footstepClip != null && audioSource != null)
    {
        audioSource.clip = footstepClip;
        audioSource.loop = true;
        audioSource.Play();
        isWalkingSoundPlaying = true;
    }

    Debug.Log("Boss chasing player..."); // Debugging Chase
}

    void HandleAttack()
{
    if (agent == null || animator == null) return;

    agent.isStopped = true;
    agent.velocity = Vector3.zero;

    animator.SetBool("isWalking", false);
    animator.SetBool("isAttacking", true);

    Vector3 direction = (player.position - transform.position).normalized;
    direction.y = 0;
    if (direction != Vector3.zero)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    if (Time.time >= lastAttackTime + attackCooldown)
    {
        lastAttackTime = Time.time;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20f);
            Debug.Log("Boss attacking player!");  // Debugging Attack
        }
    }

    if (isWalkingSoundPlaying && audioSource != null)
    {
        audioSource.Stop();
        isWalkingSoundPlaying = false;
    }
}

    void HandleIdle()
    {
        if (agent == null || animator == null) return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        if (isWalkingSoundPlaying && audioSource != null)
        {
            audioSource.Stop();
            isWalkingSoundPlaying = false;
        }
    }

    public void TakeDamage(int damage)
{
    if (isDead) return;

    currentHealth -= damage;
    UpdateBossHealthUI();

    Debug.Log("Boss took damage! Current HP: " + currentHealth);

    if (currentHealth <= 0)
    {
        Die();
    }
}

    void UpdateBossHealthUI()
    {
        if (bossHealthFill != null)
        {
            bossHealthFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }

        if (audioSource != null && isWalkingSoundPlaying)
        {
            audioSource.Stop();
            isWalkingSoundPlaying = false;
        }

        Invoke(nameof(SpawnDropItem), 2f);
    }

    void SpawnDropItem()
    {
        if (dropItem != null)
        {
            Instantiate(dropItem, transform.position + Vector3.up, Quaternion.identity);
        }

        Destroy(gameObject, 3f);
    }

    // Dipanggil dari luar (GameManager) untuk mulai mengaktifkan Boss
    public void ActivateBoss()
    {
        isActive = true;
        currentHealth = maxHealth;
        UpdateBossHealthUI();

        if (bossNameText != null)
            bossNameText.text = bossName;

        if (bossUI != null)
            bossUI.SetActive(true);
    }
}