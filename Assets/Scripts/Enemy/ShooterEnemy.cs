using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;                                                                                                                            
    public NavMeshAgent Agent { get => agent; }
    
    private GameObject player;  // Deklarasi variabel Player
    public GameObject Player { get => player; }
    
    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }

    [SerializeField] public string currentState;
    public Path path;
    public GameObject debugsphere;
    private Vector3 lastKnowPos;

    [Header("Sight Values")]
    public float sightdistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player tidak ditemukan! Pastikan ada objek dengan tag 'Player'.");
        }

        if (gunBarrel == null)
        {
            Debug.LogWarning("GunBarrel belum diassign di Inspector! Pastikan sudah diassign.");
        }
    }

    void Update()
    {
        if (CanSeePlayer() || DetectSphere())
        {
            Debug.Log("Musuh melihat sesuatu, tetapi tetap bergerak.");
            Patrol();
        }

        currentState = stateMachine.activeState?.ToString();
        if (debugsphere != null)
        {
            debugsphere.transform.position = lastKnowPos;
        }
    }

    public bool CanSeePlayer()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < sightdistance)
        {
            Vector3 targetDirection = (player.transform.position - transform.position) - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, sightdistance))
                {
                    if (hitInfo.transform.gameObject == player)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool DetectSphere()
    {
        if (debugsphere == null)
            return false;

        float distanceToSphere = Vector3.Distance(transform.position, debugsphere.transform.position);
        return distanceToSphere < sightdistance;
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 1f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }
}
