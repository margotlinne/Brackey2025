using UnityEngine;

namespace Margot
{
    public class ChasingEnemy : Enemy
    {
        [Header("Approach")]
        [SerializeField] private float stopDistance = 1.2f;   // 이 거리 이내면 정지
        [SerializeField] private float slowDistance = 2.0f;   // 이 거리부터 서서히 감속

        void FixedUpdate()
        {
            if (player == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (!canAttack)
            {
                rb.linearVelocity = Vector2.zero; // 추적 중지 시 관성 제거
                return;
            }

            Vector2 toPlayer = (player.position - transform.position);
            float dist = toPlayer.magnitude;

            if (dist <= stopDistance)
            {
                rb.linearVelocity = Vector2.zero; // 너무 밀지 않도록 완전 정지
                return;
            }

            // 감속 구간: stopDistance ~ slowDistance 사이에서 속도를 0→1로 보간
            float speedFactor = 1f;
            if (dist < slowDistance)
            {
                float t = Mathf.InverseLerp(stopDistance, slowDistance, dist);
                speedFactor = Mathf.SmoothStep(0f, 1f, t);
            }

            Vector2 dir = toPlayer / dist; // normalized
            rb.linearVelocity = dir * (moveSpeed * speedFactor);
        }
    }
}
