using UnityEngine;
using System.Collections;

namespace Margot
{
    public class RunningEnemy : Enemy
    {
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private bool isDizzy = false;  

        [SerializeField] private float dizzyTime = 1f;
        private bool setAnim = false;

        protected override void Start()
        {
            base.Start();
            PickRandomDirection();
        }

        void FixedUpdate()
        {
            if (canAttack && !isDizzy)
            {
                rb.linearVelocity = moveDirection * moveSpeed;
                anim.SetBool("isMoving", true);
            }
            else
            {
                rb.linearVelocity = Vector2.zero; 
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
                setAnim = false;
                anim.SetBool("isMoving", false);
                anim.SetBool("isDizzy", true);
                Vector2 normal = collision.contacts[0].normal;
                moveDirection = Vector2.Reflect(moveDirection, normal).normalized;

                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(DizzyCoroutine());
                }
            }
        }

        private IEnumerator DizzyCoroutine()
        {
            isDizzy = true;
            yield return new WaitForSeconds(dizzyTime); 
            isDizzy = false;
            anim.SetBool("isDizzy", false);
            if (!setAnim)
            {
                anim.SetTrigger("Move");
                setAnim = true;
            }
        }


        protected override void OnDisable()
        {
            base.OnDisable();

            isDizzy = false;
        }
    }
}
