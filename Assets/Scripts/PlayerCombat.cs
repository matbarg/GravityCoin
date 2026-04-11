using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private bool isAttacking = false;
    private Animator animator;
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public int coinsLostOnHit = 2;
    [SerializeField] private GameObject hitboxVisual;


    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHitboxVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking) return;
        
        isAttacking = true;
        
        Debug.Log("Attack!");
        animator.SetTrigger("Attack");
    }
    
    public void PerformAttack()
    {
        hitboxVisual.SetActive(true);
        
        Debug.Log("Start hitbox");
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            PlayerInventory target = hit.GetComponent<PlayerInventory>();

            if (target != null)
            {
                Debug.Log("Target hit.");
                target.LoseCoins(coinsLostOnHit);
            }
        }
        
        Invoke(nameof(HideHitbox), 0.1f);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
    
    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    
    private void HideHitbox()
    {
        hitboxVisual.SetActive(false);
    }
    
    private void UpdateHitboxVisual()
    {
        SpriteRenderer sr = hitboxVisual.GetComponent<SpriteRenderer>();
    
        float spriteSize = sr.sprite.bounds.size.x; // LOCAL size of sprite
        float diameter = attackRadius * 2f;

        float scale = diameter / spriteSize;

        hitboxVisual.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
