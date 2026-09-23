using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerModern : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 7f;
    public float jumpForce = 8f;
    private Vector2 moveInput; 
    private bool facingRight = true;

    [Header("Pulo e Chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Combate")]
    public int maxHealth = 100;
    private int currentHealth;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0 && !facingRight) Flip();
        else if (moveInput.x < 0 && facingRight) Flip();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            if (anim != null) anim.SetTrigger("Jump");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextAttackTime)
        {
            if (anim != null) anim.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("Acertou " + enemy.name);
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (anim != null) anim.SetTrigger("Hurt");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Morreu!");
        if (anim != null) anim.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}