using UnityEngine;

public class FootSteps : MonoBehaviour
{
    // Komponen AudioSource untuk efek suara langkah kaki, sprint, dan lompatan
    public AudioSource footstepsSound, sprintSound, jumpSound;

    // Komponen CharacterController untuk mendeteksi pergerakan karakter
    public CharacterController characterController;

    void Update()
    {
        // Cek apakah karakter sedang bergerak dengan menekan tombol WASD
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Jika karakter bergerak dan berada di tanah (isGrounded)
        if (isMoving && characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift)) // Jika menekan Shift, berarti sprint
            {
                if (!sprintSound.isPlaying) // Jika suara sprint belum diputar, mainkan
                {
                    sprintSound.Play();
                    footstepsSound.Stop(); // Hentikan suara berjalan biasa
                }
            }
            else // Jika hanya berjalan biasa (tanpa menekan Shift)
            {
                if (!footstepsSound.isPlaying) // Jika suara langkah belum diputar, mainkan
                {
                    footstepsSound.Play();
                    sprintSound.Stop(); // Hentikan suara sprint
                }
            }
        }
        else // Jika karakter tidak bergerak atau sedang melayang di udara
        {
            footstepsSound.Stop(); // Hentikan suara langkah kaki
            sprintSound.Stop(); // Hentikan suara sprint
        }

        // Jika pemain menekan tombol lompat (Space) dan sedang di tanah
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            jumpSound.Play(); // Putar suara lompatan
        }
    }
}
