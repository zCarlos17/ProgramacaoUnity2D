using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    
    void Update()
    {
       float moveInput = 0f;
       if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
       moveInput = -1f;
       else if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
       moveInput = 1f;
       rb.linearVelocity = new Vector2(moveInput*moveSpeed, rb.linearVelocity.y);
    }

}
public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackpoint;
    public float attackRange = 1.0f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void update()
    {
        if (nextAttackTime.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown("0"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Cutucada");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackDamageRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackpoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jogador tomou " + damage + " de dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER!");
        
        Destroy(gameObject);
    }
}