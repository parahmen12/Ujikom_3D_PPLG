using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float runSpeed = 5f;
    public GameObject gameOverPanel;
    public GameObject playerController; // Objek untuk player movement

    private Animator animator;
    private NavMeshAgent agent;
    private bool isChasing = false;
    private MonsterPatrol patrolScript;
    private bool gameOverTriggered = false; // Mencegah Game Over berulang

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        patrolScript = GetComponent<MonsterPatrol>();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (gameOverTriggered) return; // Jika game over, hentikan update

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
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
        if (gameOverTriggered) return; // Jangan kejar jika game over

        isChasing = true;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("Walking", true);

        if (patrolScript != null)
        {
            patrolScript.enabled = false;
        }
    }

    void ReturnToPatrol()
    {
        if (isChasing)
        {
            isChasing = false;
            animator.SetBool("Walking", false);

            if (patrolScript != null)
            {
                patrolScript.enabled = true;
            }
        }
    }

    void TriggerGameOver()
    {
        if (gameOverTriggered) return;

        gameOverTriggered = true;
        agent.ResetPath(); // Hentikan pergerakan NavMeshAgent
        agent.isStopped = true; // Stop agent
        agent.enabled = false; // Matikan NavMeshAgent agar tidak mencari jalan lagi

        animator.SetBool("Walking", false);
        animator.SetBool("Turning", false);
        animator.Play("Idle"); // Pastikan animasi diam dimainkan

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.SetActive(false); // Matikan kontrol player
        }
    }
}
