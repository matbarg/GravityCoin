using UnityEngine;


public class Powerup : MonoBehaviour
{
    [Tooltip("Welches Powerup diese Kugel gibt.")]
    public PowerupType type = PowerupType.GravityFlip;

    [Header("Audio")]
    public AudioClip collectSound;
    public PowerupSpawner spawner; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerupHolder holder = other.GetComponent<PowerupHolder>();

        if (holder != null)
        {
            holder.PickUp(type);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, 3.0f);
            
            if (spawner != null)
                spawner.OnPowerupCollected();
            
            Destroy(gameObject);
        }
    }
}