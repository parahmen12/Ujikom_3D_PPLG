using UnityEngine;
using UnityEngine.AI;

public class MonsterSound : MonoBehaviour
{
    public AudioSource footstepSound; // Suara langkah monster
    public AudioSource roarSound; // Suara auman monster
    private NavMeshAgent agent; // Komponen NavMeshAgent untuk AI monster

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Ambil referensi NavMeshAgent
    }

    void Update()
    {
        // Cek apakah AI sedang bergerak dengan kecepatan lebih dari 0.1 dan masih memiliki tujuan yang belum dicapai
        if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
        {
            // Jika suara langkah belum diputar, mainkan suara langkah
            if (!footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
        else
        {
            // Hentikan suara langkah jika AI berhenti
            footstepSound.Stop();
        }
    }

    // Fungsi untuk memainkan suara Roar
    public void Roar()
    {
        // Jika suara auman belum diputar, mainkan suara auman
        if (!roarSound.isPlaying)
        {
            roarSound.Play();
        }
    }
}
