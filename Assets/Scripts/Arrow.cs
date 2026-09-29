using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private int coinsLostOnHit = 1;
    [SerializeField] private float arrowGravity = 1f;
    [SerializeField] private float pickupLifetime = 20f;
    
    
    private GameLevelSpawner levelSpawner;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFlying = true;
    private ArrowType arrowType;

    private Collider2D arrowCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        levelSpawner = FindFirstObjectByType<GameLevelSpawner>();
        arrowCollider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        if (!isFlying)
            return;

        Vector2 velocity = rb.linearVelocity;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Initialize(bool facingRight, float gravityDirection, ArrowType type)
    {
        arrowType = type;
        float direction;
        if (facingRight)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        } 

        
        rb.linearVelocity = new Vector2(direction * speed, 0f);
        rb.gravityScale = arrowGravity * gravityDirection;

        spriteRenderer.flipX = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
 
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
            arrowCollider.isTrigger = true;
           Destroy(gameObject, pickupLifetime); 
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger berührt von: " + other.gameObject.name);

        if (isFlying)
            return;

        WeaponHolder weaponHolder =
            other.GetComponentInParent<WeaponHolder>();

        if (weaponHolder != null)
        {
            Debug.Log("WeaponHolder gefunden!");

            bool ammoAdded = weaponHolder.AddBowAmmo(arrowType);

            Debug.Log("Ammo aufgenommen: " + ammoAdded);

            if (ammoAdded)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Kein WeaponHolder gefunden.");
        }
    }
    
    
}