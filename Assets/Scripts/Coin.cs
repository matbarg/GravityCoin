using UnityEngine;
using System.Collections;
 
public class Coin : MonoBehaviour
{
    public CoinSpawner spawner;
    private Rigidbody2D rb;
    private bool canBePickedUp;
    public bool regularSpawn = false;
 
 
    public bool kingMode = false;
 
    [SerializeField] private float pickupDelay = 0.5f;
 
    [Header("Schweben")]
    [Tooltip("Wie stark der Coin nach dem Wegfliegen ausbremst. Hoeher = stoppt schneller.")]
    [SerializeField] private float floatDamping = 0.8f;
 
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
    }
 
    private void InitializePhysics()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;                 
        rb.linearDamping = floatDamping;     
        rb.angularDamping = floatDamping;
    }
 
    public void AddImpulse(Vector2 force)
    {
       
        rb.WakeUp();
        rb.AddForce(force, ForceMode2D.Impulse);
    }
 
    private IEnumerator EnablePickup()
    {
        yield return new WaitForSeconds(pickupDelay);
        canBePickedUp = true;
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canBePickedUp) return;
        
        if (kingMode && KingOfCoinManager.Instance != null)
        {
            if (other.GetComponent<PlayerMovement>() != null)
            {
                KingOfCoinManager.Instance.OnCoinPickedUp(other.gameObject);
 
                if (collectSound != null)
                    AudioSource.PlayClipAtPoint(collectSound, transform.position,  3.0f);
 
                Destroy(gameObject);
            }
            return;
        }
        
        PlayerInventory inv = other.GetComponent<PlayerInventory>();
 
        if (inv != null)
        {
            inv.AddCoin();
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, 3.0f);
            }
 
            if (regularSpawn)
            {
                spawner.OnCoinCollected();
            }
            Destroy(gameObject);
        }
    }
}