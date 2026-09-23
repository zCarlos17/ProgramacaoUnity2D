using UnityEngine;

public class Slime : Enemy
{
    protected override void Start()
    {
        maxHealth = 30;
        attackDamage = 7;
        attackRate = 1.5f;

        attackRange = 0.3f; 

        moveSpeed = 2f; 

        base.Start(); 
    }  
}