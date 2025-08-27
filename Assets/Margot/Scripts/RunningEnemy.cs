using UnityEngine;

namespace Margot
{
    public class RunningEnemy : Enemy
    {
        private Vector2 moveDirection;

        protected override void Start()
        {
            base.Start();

            PickRandomDirection();
        }

        void FixedUpdate()
        {
            if (canAttack)
            {
                rb.linearVelocity = moveDirection * speed;
            }
        }

        void PickRandomDirection()
        {
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;
            moveDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if (collision.gameObject.CompareTag("Wall"))
            {
                Vector2 normal = collision.contacts[0].normal;
                moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
            }
        }
    }
}

