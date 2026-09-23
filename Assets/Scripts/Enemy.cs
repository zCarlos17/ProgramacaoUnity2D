using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Status do Inimigo")]
    public int maxHealth = 100;
    public float moveSpeed = 3f;
    protected int currentHealth;

    [Header("Combate")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 10;
    public float attackRate = 1f;
    protected float nextAttackTime = 0f;

    protected Rigidbody2D rb;
    protected Animator anim;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
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

    protected virtual void Attack(Collider2D player)
    {
        if (anim != null) anim.SetTrigger("Attack");
        
        Debug.Log(gameObject.name + " atacou " + player.name);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " tomou dano! Vida: " + currentHealth);

        if (anim != null) anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " foi derrotado.");
        if (anim != null) anim.SetBool("IsDead", true);

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
        
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        
        this.enabled = false; 
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
        if(rb != null) rb.velocity = Vector2.zero; 
        
        this.enabled = false; 
        
        }
    }
}
