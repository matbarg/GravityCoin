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

    private PlayerMovement movement;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        UpdateHitboxVisual();
    }

    void Update()
    {

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed || isAttacking) return;

     
        if (movement != null && movement.ControlsLocked) return;

       
        if (KingOfCoinManager.Instance != null)
        {
            KingOfCoinPlayer kp = GetComponent<KingOfCoinPlayer>();
            if (kp != null && kp.isCarrier) return;
        }

        isAttacking = true;
        if (audioSource != null && swingSound != null)
        {
            audioSource.PlayOneShot(swingSound);
        }
        animator.SetTrigger("Attack");

        CancelInvoke(nameof(EndAttack));
        Invoke(nameof(EndAttack), maxAttackDuration);
    }

    public void PerformAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

			PlayerMovement targetMovement = hit.GetComponent<PlayerMovement>();

			if (targetMovement != null)
			{
    			targetMovement.TakeHit(transform.position);
			}

            if (KingOfCoinManager.Instance != null)
            {
                KingOfCoinPlayer kp = hit.GetComponent<KingOfCoinPlayer>();
                if (kp != null && kp.isCarrier)
                {
                    KingOfCoinManager.Instance.DropCoin(kp);
                }
            }
            else
            {
           
                PlayerInventory target = hit.GetComponent<PlayerInventory>();
                if (target != null)
                {
                    target.LoseCoins(coinsLostOnHit);
                }
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
        if (hitboxVisual != null) hitboxVisual.SetActive(false);
    }

    private void UpdateHitboxVisual()
    {
        if (hitboxVisual == null) return;

        SpriteRenderer sr = hitboxVisual.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float spriteSize = sr.sprite.bounds.size.x;
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