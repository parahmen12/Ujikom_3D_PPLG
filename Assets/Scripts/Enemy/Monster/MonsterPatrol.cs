using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrol : MonoBehaviour
{
    public Path path; // Referensi ke Path yang berisi waypoint
    public float waitTime = 3f; // Waktu berhenti di setiap titik waypoint
    public float walkSpeed = 1f; // Kecepatan monster saat berjalan

    private NavMeshAgent agent; // Komponen NavMeshAgent untuk pergerakan AI
    private int currentPointIndex = 0; // Indeks waypoint saat ini
    private bool isWaiting; // Status apakah monster sedang berhenti
    private Animator animator; // Komponen Animator untuk animasi monster

    void Start()
    {
        // Ambil referensi komponen NavMeshAgent dan Animator
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent tidak ditemukan pada " + gameObject.name);
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent belum ditempatkan di NavMesh pada " + gameObject.name);
            return;
        }

        agent.speed = walkSpeed; // Atur kecepatan berjalan AI
        animator.enabled = true; // Memastikan animator aktif

        // Jika ada path dan waypoint tersedia, mulai bergerak ke titik pertama
        if (path != null && path.waypoints.Count > 0)
        {
            MoveToNextPoint();
        }
    }

    void Update()
    {
        // Pastikan agent sudah ada dan berada di NavMesh sebelum menggunakan remainingDistance
        if (agent == null || !agent.isOnNavMesh)
            return;

        // Jika tidak sedang menunggu dan sudah sampai di tujuan, mulai menunggu sebelum lanjut ke waypoint berikutnya
        if (!isWaiting && agent.remainingDistance > 0 && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            StartCoroutine(WaitAndMove());
        }

        // Mengatur animasi berdasarkan kecepatan agent
        if (isWaiting)
        {
            animator.SetBool("Walking", false); // Matikan animasi berjalan
            animator.Play("Idle"); // Pastikan animasi Idle dimainkan
        }
        else
        {
            animator.SetBool("Walking", agent.velocity.magnitude > 0.1f); // Hidupkan animasi berjalan jika bergerak
        }
    }

    // Coroutine untuk menunggu sebelum bergerak ke waypoint selanjutnya
    IEnumerator WaitAndMove()
    {
        isWaiting = true; // Tandai bahwa AI sedang berhenti
        animator.SetBool("Walking", false); // Matikan animasi berjalan
        animator.Play("Idle"); // Pastikan animasi Idle aktif
        yield return new WaitForSeconds(waitTime); // Tunggu beberapa detik

        MoveToNextPoint(); // Pindah ke waypoint berikutnya
        isWaiting = false; // Tandai bahwa AI sudah siap bergerak lagi
    }

    // Fungsi untuk berpindah ke waypoint berikutnya
    void MoveToNextPoint()
    {
        if (path.waypoints.Count == 0 || agent == null || !agent.isOnNavMesh) return; // Cek sebelum mengatur tujuan baru

        // Berpindah ke waypoint berikutnya (loop kembali ke awal jika sudah sampai di akhir daftar)
        currentPointIndex = (currentPointIndex + 1) % path.waypoints.Count;
        agent.SetDestination(path.waypoints[currentPointIndex].position); // Atur tujuan baru
    }
}
