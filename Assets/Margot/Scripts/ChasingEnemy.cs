using UnityEngine;

namespace Margot
{
    public class ChasingEnemy : Enemy
    {
        void FixedUpdate()
        {
            if (player == null) return;
            if (canAttack)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
                
        }
    }
}

