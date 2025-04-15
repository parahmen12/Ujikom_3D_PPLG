using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player; // Referensi ke pemain yang akan dikejar
    public float chaseRange = 10f; // Jarak mulai mengejar pemain
    public float attackRange = 2f; // Jarak untuk menangkap pemain
    public float runSpeed = 5f; // Kecepatan monster saat mengejar
    public GameObject gameOverPanel; // Panel Game Over
    public GameObject playerController; // Kontrol pemain

    private Animator animator;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private bool gameOverTriggered = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (gameOverTriggered) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.Log("Jarak ke pemain: " + distanceToPlayer); // Debugging

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Pemain tertangkap! Menampilkan Game Over.");
            TriggerGameOver();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            ReturnToPatrol();
        }
    }

    void ChasePlayer()
    {
        if (gameOverTriggered) return;

        isChasing = true;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("Walking", true);
    }

    void ReturnToPatrol()
    {
        if (isChasing)
        {
            isChasing = false;
            animator.SetBool("Walking", false);
        }
    }

    void TriggerGameOver()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;

        agent.isStopped = true;
        agent.ResetPath();
        agent.enabled = false;

        animator.SetBool("Walking", false);
        animator.Play("Idle");

        if (gameOverPanel != null)
        {
            Debug.Log("Menampilkan panel Game Over.");
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Game Over Panel tidak di-assign di Inspector!");
        }

        if (playerController != null)
        {
            Debug.Log("Menonaktifkan kontrol pemain.");
            playerController.SetActive(false);
        }

        // Tampilkan kursor agar bisa tekan tombol
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause game
        Time.timeScale = 0f;
    }
}
