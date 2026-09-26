using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Configurações da Zona")]
    public int damageAmount = 20;
    public bool instakill = false; 

    void OnTriggerEnter2D(Collider2D collision)
    {
       
        PlayerControllerModern player = collision.GetComponent<PlayerControllerModern>();

       
        if (player != null)
        {
            if (instakill)
            {
                player.TakeDamage(9999); 
            }
            else
            {
                player.TakeDamage(damageAmount); 
            }
        }
    }
}