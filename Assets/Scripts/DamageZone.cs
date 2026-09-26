using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Configurações da Zona")]
    public int damageAmount = 20;
    public float damageInterval = 1f; // Tempo em segundos entre cada dano contínuo
    private float nextDamageTime;

    // Assim que entra, dá o primeiro dano imediatamente
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    // Enquanto continua dentro da água, aplica dano respeitando o intervalo de tempo
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time >= nextDamageTime)
        {
            TryDamage(collision);
            nextDamageTime = Time.time + damageInterval; // Atualiza o cronômetro para o próximo dano
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reseta o temporizador quando o jogador sai da água
        if (collision.CompareTag("Player"))
        {
            nextDamageTime = 0f;
        }
    }

    private void TryDamage(Collider2D collision)
    {
        PlayerControllerModern player = collision.GetComponent<PlayerControllerModern>();
        
        if (player != null)
        {
            player.TakeDamage(damageAmount);
            Debug.Log("Dano constante aplicado pela água!");
        }
    }
}