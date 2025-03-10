using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    [SerializeField]
    private bool alwaysDrawPath;
    [SerializeField]
    private bool drawAsLoop;
    [SerializeField]
    private bool drawNumbers;
    public Color debugColour = Color.white;

#if UNITY_EDITOR    
    public void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }

    public void DrawPath()
    {
        if (waypoints.Count < 2) return; 

        for (int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;  
            labelStyle.normal.textColor = debugColour;

            if (drawNumbers)
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

            if (i > 0) 
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }

        if (drawAsLoop && waypoints.Count > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (!alwaysDrawPath) 
        {
            DrawPath();
        }
    }
#endif
}
