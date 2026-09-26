using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Configurações da Zona")]
    public int damageAmount = 20;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerControllerModern player = collision.GetComponent<PlayerControllerModern>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}