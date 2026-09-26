using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Configurações da Zona")]
    public int damageAmount = 20;
    public float damageInterval = 0.5f; 
    private float nextDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerControllerModern player = collision.GetComponent<PlayerControllerModern>();
        
        if (player != null)
        {
            if (Time.time >= nextDamageTime)
            {
                player.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nextDamageTime = 0f;
        }
    }
}