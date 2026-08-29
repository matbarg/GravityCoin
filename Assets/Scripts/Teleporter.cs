using UnityEngine;

public class Teleporter : MonoBehaviour
{ 
    public TeleporterSpawner spawner;
    public Transform exitPoint;

        void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D rb = other.attachedRigidbody;

            if (rb != null)
            {
                rb.position = exitPoint.position;
            }
            spawner.PortalUsed();
        }
}


