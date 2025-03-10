using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : MonoBehaviour
{
    public Path path; // Referensi ke Path yang berisi waypoint
    public float waitTime = 2f; // Waktu berhenti di setiap titik
    public float walkSpeed = 2f;

    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private bool isWaiting;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        animator.enabled = true; // Memastikan animator aktif

        if (path != null && path.waypoints.Count > 0)
        {
            MoveToNextPoint();
        }
    }

    void Update()
    {
        if (!isWaiting && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            StartCoroutine(WaitAndMove());
        }

        // Memastikan animasi berjalan saat bergerak
        animator.SetBool("Walking", agent.velocity.magnitude > 0.1f);
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        animator.SetBool("Walking", false); // Berhenti saat menunggu
        yield return new WaitForSeconds(waitTime);

        MoveToNextPoint();
        isWaiting = false;
    }

    void MoveToNextPoint()
    {
        if (path.waypoints.Count == 0) return;

        currentPointIndex = (currentPointIndex + 1) % path.waypoints.Count;
        agent.SetDestination(path.waypoints[currentPointIndex].position);
    }
}
