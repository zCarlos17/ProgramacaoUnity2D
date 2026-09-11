using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth; 

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " tomou dano! Vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " foi derrotado.");
        Destroy(gameObject);
    }
}