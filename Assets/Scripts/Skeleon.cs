using UnityEngine;
public class Skeleton : Enemy 
{
    //private bool facingRight = true;
    protected override void Start()
    {
        maxHealth = 50;
        attackDamage = 15;
        attackRate = 1.2f;

        attackRange = 0.8f; 

        moveSpeed = 4.5f; 

        base.Start(); 
        //Flip();
    }
    
}