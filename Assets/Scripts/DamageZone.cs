using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    [Header("Configurações da Zona")]
    public int damageAmount = 20;
    public float damageInterval = 1f; 

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControllerModern player = collision.GetComponent<PlayerControllerModern>();
        
        if (player != null)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealContinuousDamage(player));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealContinuousDamage(PlayerControllerModern player)
    {
        while (true)
        {
            player.TakeDamage(damageAmount);
            Debug.Log("Dano contínuo aplicado pela corrotina!");
            
            yield return new WaitForSeconds(damageInterval);
        }
    }
}