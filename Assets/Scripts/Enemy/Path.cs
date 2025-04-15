using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>(); // List untuk menyimpan waypoint
    [SerializeField]
    private bool alwaysDrawPath; // Apakah path selalu digambar di scene view
    [SerializeField]
    private bool drawAsLoop; // Apakah path berbentuk loop
    [SerializeField]
    private bool drawNumbers; // Apakah nomor waypoint ditampilkan
    public Color debugColour = Color.white; // Warna garis path

#if UNITY_EDITOR    
    // Fungsi ini dipanggil untuk menggambar gizmo di scene view saat objek tidak dipilih
    public void OnDrawGizmos()
    {
        if (alwaysDrawPath) // Jika alwaysDrawPath aktif, gambar path
        {
            DrawPath();
        }
    }

    // Fungsi untuk menggambar path di scene view
    public void DrawPath()
    {
        if (waypoints.Count < 2) return; // Jika waypoint kurang dari 2, keluar dari fungsi

        for (int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;  // Ukuran font untuk angka waypoint
            labelStyle.normal.textColor = debugColour;

            if (drawNumbers) // Jika opsi drawNumbers aktif, tampilkan nomor waypoint
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

            if (i > 0) // Menghubungkan waypoint sebelumnya ke waypoint sekarang
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }

        if (drawAsLoop && waypoints.Count > 1) // Jika path berbentuk loop, hubungkan titik terakhir ke titik pertama
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }

    // Fungsi ini dipanggil untuk menggambar gizmo di scene view saat objek dipilih
    public void OnDrawGizmosSelected()
    {
        if (!alwaysDrawPath) // Jika alwaysDrawPath tidak aktif, gambar path hanya saat dipilih
        {
            DrawPath();
        }
    }
#endif
}
