using UnityEngine;

public class Skeleton : Enemy
{
    [Header("Configurações de Movimento")]
    public float speed = 2f;
    private Transform player;

    [Header("Configurações de Ataque")]
    public float attackRange = 0.5f; 
    public int attackDamage = 15;
    public float attackRate = 1.5f; 
    private float nextAttackTime = 0f;

    protected override void Start()
    {
        maxHealth = 50;
        base.Start();
        
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }
        }

        FlipSprite();
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        Debug.Log("Esqueleto atacou o jogador!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void FlipSprite()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
    }
}