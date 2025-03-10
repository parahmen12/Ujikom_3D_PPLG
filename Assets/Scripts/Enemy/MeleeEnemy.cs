using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;

    private GameObject player;
    public GameObject Player { get => player; }

    [Header("Sight Values")]
    public float sightDistance = 15f;
    public float fieldOfView = 90f;
    public float attackRange = 2f;

    [Header("Combat Values")]
    public float attackCooldown = 1.5f;
    private float attackTimer;
    public int damage = 20;
    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask playerLayer;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player tidak ditemukan! Pastikan ada objek dengan tag 'Player'.");
        }

        stateMachine.Initialise();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < sightDistance)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfView * 0.5f)
            {
                Ray ray = new Ray(transform.position, directionToPlayer);
                if (Physics.Raycast(ray, out RaycastHit hit, sightDistance))
                {
                    return hit.transform.gameObject == player;
                }
            }
        }
        return false;
    }

    void ChasePlayer()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.transform.position);

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        if (attackTimer < attackCooldown) return;

        Debug.Log("Musuh menyerang dengan pedang!");
        attackTimer = 0;

        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
