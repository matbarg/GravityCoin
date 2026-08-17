using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = false;
    private float gravityDirection = 1f;
    private bool isGrounded;

    private float speedMultiplier = 1f;

    private bool controlsLocked = false;
    public bool ControlsLocked => controlsLocked;

    [Header("Gravity Settings")]
    public bool requireGroundToSwitch = true;
    public bool useCooldownToSwitch = false;
    public float gravityCooldownTime = 1.5f;
    private float nextGravitySwitchTime = 0f;
    private bool hasTouchedGroundSinceSwitch = true;

	private bool isStaggered = false;
	[SerializeField] private float staggerDuration = 0.2f;
	[SerializeField] private float knockbackForce = 10f;

    [Header("Treffer-Aufblinken")]
    [Tooltip("Farbe, in der der Spieler bei einem Treffer kurz aufblinkt.")]
    public Color hitFlashColor = new Color(1f, 0.3f, 0.3f);
    [Tooltip("Wie lange das Aufblinken dauert (Sekunden).")]
    public float hitFlashDuration = 0.1f;

    [Header("Treffer-Partikel")]
    [Tooltip("Partikel-Prefab, das bei einem Treffer kurz aufplatzt.")]
    public GameObject hitParticlePrefab;
    [Tooltip("Versatz vom Spieler-Mittelpunkt (z.B. leicht nach oben oder Z nach vorne).")]
    public Vector3 hitParticleOffset = Vector3.zero;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip jumpSound;

    private Animator animator;

    private SpriteRenderer[] sprites;
    private Color[] originalColors;

    private bool InputBlocked => isStaggered || controlsLocked;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        sprites = GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
            originalColors[i] = sprites[i].color;
    }

    void Update()
    {
		if (InputBlocked) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsGrounded", isGrounded);

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
    	if (InputBlocked) return;

        rb.linearVelocity = new Vector2(horizontal * speed * speedMultiplier, rb.linearVelocity.y);
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            hasTouchedGroundSinceSwitch = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (InputBlocked) return;
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * gravityDirection);
            animator.SetTrigger("Jump");

            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound, 0.1f);
            }
        }

        if (context.canceled && rb.linearVelocity.y * gravityDirection > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        Vector2 direction = Vector2.down * gravityDirection;
        Vector2 checkPosition = (Vector2)groundCheck.position + direction * 0.15f;

        return Physics2D.OverlapBox(
            checkPosition,
            new Vector2(0.8f, 0.25f),
            0f,
            groundLayer
        );
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void FlipVertical()
    {
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
		if (InputBlocked) { horizontal = 0f; return; }
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void SwitchGravity(InputAction.CallbackContext context)
    {
        if (InputBlocked) return;
        if (context.performed)
        {
            if (requireGroundToSwitch && !hasTouchedGroundSinceSwitch)
            {
                return;
            }
        if (useCooldownToSwitch && Time.time < nextGravitySwitchTime)
            {
                return;
            }

            gravityDirection *= -1f;
            rb.gravityScale *= -1f;

            FlipVertical();

            animator.SetTrigger("Jump");
            hasTouchedGroundSinceSwitch = false;
            nextGravitySwitchTime = Time.time + gravityCooldownTime;
        }
    }
    //für Gravity Flip-Powerup. erzwingt Gravity switch
    public void ForceGravityFlip()
    {
        gravityDirection *= -1f;
        rb.gravityScale *= -1f;

        FlipVertical();

        animator.SetTrigger("Jump");

// sperren von switch
        hasTouchedGroundSinceSwitch = false;
        nextGravitySwitchTime = Time.time + gravityCooldownTime;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsLocked = !enabled;

        if (controlsLocked && rb != null)
        {
            horizontal = 0f;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

	public void TakeHit(Vector2 hitSourcePosition)
	{
    	if (isStaggered) return;

    	isStaggered = true;

    	animator.SetTrigger("Hit");

    	Vector2 direction = (Vector2)(transform.position - (Vector3)hitSourcePosition).normalized;
    	rb.linearVelocity = Vector2.zero;
    	rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

    	// Treffer-Aufblinken
    	StartCoroutine(HitFlash());

    	// Treffer-Partikel - Kopie SOFORT aktivieren, egal wie das Prefab
    	// gespeichert ist (sonst spielt ein inaktives Prefab nichts ab)
    	if (hitParticlePrefab != null)
    	{
        	GameObject fx = Instantiate(
            	hitParticlePrefab,
            	transform.position + hitParticleOffset,
            	Quaternion.identity
        	);
        	fx.SetActive(true);
    	}

    	Invoke(nameof(EndStagger), staggerDuration);
	}

	private IEnumerator HitFlash()
	{
    	for (int i = 0; i < sprites.Length; i++)
        	if (sprites[i] != null) sprites[i].color = hitFlashColor;

    	yield return new WaitForSeconds(hitFlashDuration);

    	for (int i = 0; i < sprites.Length; i++)
        	if (sprites[i] != null) sprites[i].color = originalColors[i];
	}

	private void EndStagger()
	{
    	isStaggered = false;
	}
}