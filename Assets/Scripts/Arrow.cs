using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int coinsLostOnHit = 1;

    private GameLevelSpawner levelSpawner;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFlying = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        levelSpawner = FindFirstObjectByType<GameLevelSpawner>();
    }

    public void Initialize(bool facingRight)
    {
        float direction = facingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * speed, 0f);

        spriteRenderer.flipX = !facingRight;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFlying)
            return;
        PlayerMovement player = collision.collider.GetComponentInParent<PlayerMovement>();

        if (player != null)
        {
            Debug.Log("Pfeil hat einen Spieler getroffen: " + player.gameObject.name);
            player.TakeHit(transform.position);
            PlayerInventory inventory = collision.collider.GetComponentInParent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.LoseCoins(coinsLostOnHit);
            }

            if (levelSpawner != null)
            {
                levelSpawner.RespawnAtRandomPointDelayed(player.gameObject, 0.7f);
            }
            isFlying = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Pfeil hat die Umgebung getroffen: " + collision.gameObject.name);
            isFlying = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Debug.Log("Pfeil ist jetzt: " + rb.bodyType);
        }
    }
}