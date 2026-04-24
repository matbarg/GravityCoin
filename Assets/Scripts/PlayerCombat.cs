using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private bool isAttacking = false;
    private Animator animator;
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public int coinsLostOnHit = 2;
    [SerializeField] private GameObject hitboxVisual;
	private bool isHitStopped = false;
    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip swingSound;

    public float maxAttackDuration = 0.5f;

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
        if (audioSource != null && swingSound != null)
        {
            audioSource.PlayOneShot(swingSound);
        } 
        Debug.Log("Attack!");
        animator.SetTrigger("Attack");

        // --- NEU: Das Sicherheitsnetz ---
        // Stoppt alte Timer, falls vorhanden
        CancelInvoke(nameof(EndAttack)); 
        // Erzwingt den Reset, falls das Animation Event übersprungen wird
        Invoke(nameof(EndAttack), maxAttackDuration); 
    }
    
    public void PerformAttack()
    {
        //hitboxVisual.SetActive(true);
        
        Debug.Log("Start hitbox");
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

			PlayerMovement movement = hit.GetComponent<PlayerMovement>();

			if (movement != null)
			{
    			movement.TakeHit(transform.position);
			}

            PlayerInventory target = hit.GetComponent<PlayerInventory>();

            if (target != null)
            {
                Debug.Log("Target hit.");
                target.LoseCoins(coinsLostOnHit);
            }
			
			TriggerHitStop(0.12f);
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

	public void TriggerHitStop(float duration)
	{
		if (isHitStopped) return;

    	isHitStopped = true;
    	Time.timeScale = 0f;

    	StartCoroutine(HitStopCoroutine(duration));
	}

	IEnumerator HitStopCoroutine(float duration)
	{
    	yield return new WaitForSecondsRealtime(duration);

    	Time.timeScale = 1f;
    	isHitStopped = false;
	}
}