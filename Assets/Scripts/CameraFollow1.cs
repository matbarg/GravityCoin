using UnityEngine;
using System.Collections.Generic;

// "Folge allen Spielern"-Kamera mit optionalen Grenzen (Clamping).
// Auf die Main Camera legen. Kamera muss Orthographic sein (2D).
//
// Grenzen: Zwei leere GameObjects in die untere-linke und obere-rechte
// Ecke der Map ziehen und unten zuweisen. Dann faehrt die Kamera nie
// ueber den Level-Rand hinaus. Bleiben die Felder leer -> nur folgen.
public class CameraFollow1 : MonoBehaviour
{
    [Tooltip("Wie traege die Kamera nachzieht. Groesser = weicher.")]
    public float smoothTime = 0.25f;

    [Header("Grenzen (optional)")]
    [Tooltip("Leeres GameObject in der UNTEREN-LINKEN Map-Ecke.")]
    public Transform boundsMin;
    [Tooltip("Leeres GameObject in der OBEREN-RECHTEN Map-Ecke.")]
    public Transform boundsMax;

    private List<PlayerMovement> players = new List<PlayerMovement>();
    private Vector3 velocity = Vector3.zero;
    private float fixedZ;
    private Camera cam;

    void Start()
    {
        fixedZ = transform.position.z;
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        RefreshPlayersIfNeeded();
        if (players.Count == 0) return;

        // Mittelpunkt aller Spieler
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
        float targetX = center.x;
        float targetY = center.y;

        // --- Clamping: Kamera-Mitte so begrenzen, dass der Bildrand in der Map bleibt ---
        if (cam != null && boundsMin != null && boundsMax != null)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            float minX = boundsMin.position.x + halfWidth;
            float maxX = boundsMax.position.x - halfWidth;
            float minY = boundsMin.position.y + halfHeight;
            float maxY = boundsMax.position.y - halfHeight;

            // Map schmaler als Sichtfeld? -> auf Map-Mitte zentrieren
            if (minX > maxX)
                targetX = (boundsMin.position.x + boundsMax.position.x) * 0.5f;
            else
                targetX = Mathf.Clamp(targetX, minX, maxX);

            if (minY > maxY)
                targetY = (boundsMin.position.y + boundsMax.position.y) * 0.5f;
            else
                targetY = Mathf.Clamp(targetY, minY, maxY);
        }

        Vector3 target = new Vector3(targetX, targetY, fixedZ);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

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

    // Zeigt die Grenzen als Rechteck in der Scene-Ansicht (zum Ausrichten).
    private void OnDrawGizmosSelected()
    {
        if (boundsMin == null || boundsMax == null) return;
        Gizmos.color = Color.yellow;
        Vector3 bl = boundsMin.position;
        Vector3 tr = boundsMax.position;
        Vector3 br = new Vector3(tr.x, bl.y, 0f);
        Vector3 tl = new Vector3(bl.x, tr.y, 0f);
        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);
    }
}