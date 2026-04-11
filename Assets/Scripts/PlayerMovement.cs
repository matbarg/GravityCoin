using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = false;
    private float gravityDirection = 1f; // 1 = normal, -1 = inverted
    private bool isGrounded;

    [Header("Gravity Settings")]
    public bool requireGroundToSwitch = true;
    public bool useCooldownToSwitch = false;  
    public float gravityCooldownTime = 1.5f;
    private float nextGravitySwitchTime = 0f;
    private bool hasTouchedGroundSinceSwitch = true;
    

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
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
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        isGrounded = IsGrounded();

        if (isGrounded)
        {
            hasTouchedGroundSinceSwitch = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * gravityDirection);
            animator.SetTrigger("Jump");
        }

        if (context.canceled && rb.linearVelocity.y * gravityDirection > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        Vector2 direction = Vector2.down * gravityDirection; 
        Vector2 checkPosition = (Vector2)groundCheck.position + direction * 0.1f; 
        return Physics2D.OverlapCircle(checkPosition, 0.2f, groundLayer);
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
        horizontal = context.ReadValue<Vector2>().x;
    }
    
    public void SwitchGravity(InputAction.CallbackContext context)
    {
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
}