using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Status do Inimigo")]
    public int maxHealth = 100;
    public float moveSpeed = 3f;
    protected int currentHealth;

    [Header("Detecção")]
    public float detectionRange = 6f;
    public float stopDistance = 0.7f;

    [Header("Combate")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 10;
    public float attackRate = 1f;
    protected float nextAttackTime = 0f;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Transform player;
    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("Nenhum objeto com a Tag 'Player' encontrado!");
    }

    protected virtual void Update()
    {
        if (isDead) return;

        // Ataque
        if (Time.time >= nextAttackTime && attackPoint != null)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer != null)
            {
                Attack(hitPlayer);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && distance > stopDistance)
        {
            float dir = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            Flip(dir);
            if (anim != null) anim.SetBool("IsWalking", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (anim != null) anim.SetBool("IsWalking", false);
        }
    }

    protected void Flip(float dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    protected virtual void Attack(Collider2D playerCol)
    {
        if (anim != null) anim.SetTrigger("Attack");
        Debug.Log(gameObject.name + " atacou " + playerCol.name);

        PlayerControllerModern playerScript = playerCol.GetComponent<PlayerControllerModern>();
if (playerScript != null) playerScript.TakeDamage(attackDamage, transform.position);    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        Debug.Log(gameObject.name + " tomou dano! Vida: " + currentHealth);

        if (anim != null) anim.SetTrigger("Hurt");
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " foi derrotado.");
        if (anim != null) anim.SetBool("IsDead", true);

        foreach (Collider2D col in GetComponents<Collider2D>())
            col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}