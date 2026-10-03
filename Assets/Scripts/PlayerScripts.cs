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

    [Header("Dano e Knockback")]
    public float damageCooldown = 0.5f;
    public float knockbackForce = 6f;       // impulso horizontal
    public float knockbackUpForce = 3f;     // pulinho para cima
    public float knockbackDuration = 0.2f;  // tempo sem controle
    private float lastDamageTime = -999f;
    private float knockbackEndTime;

    private Rigidbody2D rb;
    private Animator anim;

    private bool InKnockback => Time.time < knockbackEndTime;

    void Start(){
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    currentHealth = maxHealth;

        if (HealthBar.instance != null)
        HealthBar.instance.AtualizarVida(currentHealth, maxHealth);
    }

    void Update()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        }
    }

    void FixedUpdate()
    {
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }

        // Durante o knockback, não sobrescreve a velocidade nem vira o sprite
        if (InKnockback) return;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0 && !facingRight) Flip();
        else if (moveInput.x < 0 && facingRight) Flip();
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
        if (context.performed && isGrounded && !InKnockback)
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
    // Dano com origem: aplica knockback (inimigos, espinhos etc.)
    public void TakeDamage(int damage, Vector2 sourcePosition)
    {
        if (!ApplyDamage(damage)) return; // cooldown ativo ou morreu: sem empurrão

        float dir = Mathf.Sign(transform.position.x - sourcePosition.x);
        if (dir == 0) dir = facingRight ? -1f : 1f;

        rb.linearVelocity = new Vector2(dir * knockbackForce, knockbackUpForce);
        knockbackEndTime = Time.time + knockbackDuration;
    }

    // Dano simples, sem knockback (ex.: água)
    public void TakeDamage(int damage)
    {
        ApplyDamage(damage);
    }

    // Retorna true se o dano foi aceito e o player continua vivo
    private bool ApplyDamage(int damage)
    {
        if (Time.time < lastDamageTime + damageCooldown) return false;
        lastDamageTime = Time.time;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log("Player tomou dano! Vida restante: " + currentHealth);

        // Atualiza a barra ANTES de checar a morte
        if (HealthBar.instance != null)
            HealthBar.instance.AtualizarVida(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return false;
        }

        if (anim != null) anim.SetTrigger("Hurt");
        return true;
    }

    void Die(){
        Debug.Log("Personagem Morreu!");

        if (anim != null) anim.SetTrigger("Die");

        if (rb != null){
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
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