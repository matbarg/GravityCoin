using UnityEngine;
using System.Collections.Generic;

// Einfache "folge allen Spielern"-Kamera: schiebt sich sanft zum
// Mittelpunkt aller Spieler. FESTER Zoom (kein Rein-/Rauszoomen).
// Auf die Main Camera legen. Kamera sollte Orthographic sein (2D).
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Wie traege die Kamera nachzieht. Groesser = langsamer/weicher.")]
    public float smoothTime = 0.25f;

    [Tooltip("Feste Hoehe der Kamera ueber dem Mittelpunkt (Z bleibt erhalten).")]
    public bool keepStartZ = true;

    private List<PlayerMovement> players = new List<PlayerMovement>();
    private Vector3 velocity = Vector3.zero;
    private float fixedZ;

    void Start()
    {
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        RefreshPlayersIfNeeded();
        if (players.Count == 0) return;

        // Mittelpunkt aller Spieler berechnen
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var p in players)
        {
            if (p == null) continue;
            sum += p.transform.position;
            count++;
        }
        if (count == 0) return;

        Vector3 center = sum / count;

        // Ziel: Mittelpunkt, aber die Kamera-Tiefe (Z) beibehalten
        Vector3 target = new Vector3(center.x, center.y, keepStartZ ? fixedZ : transform.position.z);

        // sanft dorthin gleiten
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

    // Holt die Spieler, solange die Liste leer ist oder jemand fehlt.
    private void RefreshPlayersIfNeeded()
    {
        bool needsRefresh = players.Count == 0;
        if (!needsRefresh)
        {
            foreach (var p in players)
                if (p == null) { needsRefresh = true; break; }
        }

        if (needsRefresh)
        {
            players.Clear();
            players.AddRange(Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None));
        }
    }
}