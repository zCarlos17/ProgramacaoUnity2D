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

    [Header("Combate e Vida")]
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

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }

        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
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

            if (attackPoint != null)
            {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Acertou " + enemy.name);

                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(attackDamage);
                    }
                }
            }

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private float lastDamageTime;
    public float damageCooldown = 0.5f;

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;
        lastDamageTime = Time.time;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log("Player tomou dano! Vida restante: " + currentHealth);

        if (HealthBar.instance != null)
            HealthBar.instance.AtualizarVida(currentHealth, maxHealth);

        if (DamageVignette.instance != null)
            DamageVignette.instance.Piscar();

        if (anim != null) anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Personagem Morreu!");

        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;

            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
        }
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (HealthBar.instance != null)
            HealthBar.instance.AtualizarVida(currentHealth, maxHealth);
    }
}
