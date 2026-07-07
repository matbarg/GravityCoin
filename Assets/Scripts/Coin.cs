using UnityEngine;
using System.Collections;
 
public class Coin : MonoBehaviour
{
    public CoinSpawner spawner;
    private Rigidbody2D rb;
    private bool canBePickedUp;
    public bool regularSpawn = false;
 
    // NEU: Wird vom KingOfCoinManager gesetzt. Wenn true, macht das Aufheben
    // den Spieler zum Coin-Traeger, statt eine Muenze gutzuschreiben.
    public bool kingMode = false;
 
    [SerializeField] private float pickupDelay = 0.5f;
    [SerializeField] private float freezeDelay = 0.05f;
 
    [Header("Audio")]
    public AudioClip collectSound;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    void Start()
    {
        InitializePhysics();
        StartCoroutine(EnablePickup());
        StartCoroutine(FreezeAfterDelay());
    }
 
    private void InitializePhysics()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = 3f;
        rb.angularDamping = 3f;
    }
 
    public void AddImpulse(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
 
    private IEnumerator EnablePickup()
    {
        yield return new WaitForSeconds(pickupDelay);
        canBePickedUp = true;
    }
 
    private IEnumerator FreezeAfterDelay()
    {
        yield return new WaitForSeconds(freezeDelay);
 
        // stop motion
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
 
        // fully disable physics simulation
        rb.Sleep();
 
        // optional alternative (more “hard stop”):
        // rb.bodyType = RigidbodyType2D.Kinematic;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBePickedUp) return;
        
        Debug.Log("coin collision");
 
        // --- King of the Coin: Aufheben macht den Spieler zum Traeger ---
        if (kingMode && KingOfCoinManager.Instance != null)
        {
            // Nur echte Spieler aufheben lassen
            if (other.GetComponent<PlayerMovement>() != null)
            {
                KingOfCoinManager.Instance.OnCoinPickedUp(other.gameObject);
 
                if (collectSound != null)
                    AudioSource.PlayClipAtPoint(collectSound, transform.position);
 
                Destroy(gameObject);
            }
            return;
        }
 
        // --- Klassischer Modus ---
        PlayerInventory inv = other.GetComponent<PlayerInventory>();
 
        if (inv != null)
        {
            inv.AddCoin();
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
 
            if (regularSpawn)
            {
                spawner.OnCoinCollected();
            }
            Destroy(gameObject);
        }
    }
}
 