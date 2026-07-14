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

    [Header("Kollision")]
    [Tooltip("Der SOLIDE Collider des Coins (der zum Abprallen an Waenden). " +
             "Dieser wird gegen alle Spieler-Collider ignoriert, damit der Coin " +
             "nicht vom Koerper/Kopf weggestossen wird. NICHT der Trigger-Collider!")]
    [SerializeField] private Collider2D solidCollider;

    [Header("Audio")]
    public AudioClip collectSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        InitializePhysics();
        IgnoreAllPlayerColliders();
        StartCoroutine(EnablePickup());
    }

    private void InitializePhysics()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.linearDamping = floatDamping;
        rb.angularDamping = floatDamping;
    }

    // Sorgt dafuer, dass der SOLIDE Coin-Collider keine Spieler wegstoesst.
    // Erfasst ALLE Collider jedes Spielers - auch zusaetzliche wie den Kopf-Collider.
    private void IgnoreAllPlayerColliders()
    {
        if (solidCollider == null) return;

        var players = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            // alle Collider des Spielers (Body, Kopf, usw.) einsammeln
            var playerColliders = player.GetComponentsInChildren<Collider2D>();
            foreach (var col in playerColliders)
            {
                if (col != null)
                    Physics2D.IgnoreCollision(solidCollider, col, true);
            }
        }
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
                    AudioSource.PlayClipAtPoint(collectSound, transform.position, 3.0f);

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