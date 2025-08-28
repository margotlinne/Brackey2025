using UnityEngine;
using System.Collections;

namespace Margot
{
    public class RunningEnemy : Enemy
    {
        private Vector2 moveDirection;
        private bool isDizzy = false;   // 헤롱 상태 플래그

        [SerializeField] private float dizzyTime = 1f; // 멈추는 시간

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
            }
            else
            {
                rb.linearVelocity = Vector2.zero; // 멈춤
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

                // 헤롱 상태 코루틴 시작
                StartCoroutine(DizzyCoroutine());
            }
        }

        private IEnumerator DizzyCoroutine()
        {
            isDizzy = true;
            yield return new WaitForSeconds(dizzyTime); // 1초 기다림
            isDizzy = false;
        }
    }
}
