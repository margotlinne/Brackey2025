using System.Collections;
using UnityEngine;

namespace Margot
{
    public class ShootingEnemy : Enemy
    {
        [Header("Specific Info")]
        public Transform firePoint;
        public float fireRate = 2f;
        public float bulletForce = 50f;

        private float nextFireTime = 0f;

        [Header("Sprites")]
        public Sprite[] up;
        public Sprite[] down;
        public Sprite[] left;
        public Sprite[] right;
        public Sprite[] upLeft;
        public Sprite[] upRight;
        public Sprite[] downLeft;
        public Sprite[] downRight;

        Coroutine animationCoroutine = null;


        protected override void Update()
        {
            base.Update();

            if (player == null) return;

            Vector2 direction = player.position - transform.position;

            // 스프라이트 방향 갱신
            UpdateSprite(direction.normalized);

            if (canAttack)
            {
                // 총알 발사 처리
                if (enemyStat != null)
                {
                    if (Time.time >= nextFireTime)
                    {            
                        anim.SetTrigger("Shoot");
                        nextFireTime = Time.time + enemyStat.AttackInterval;
                    }
                }

            }
        }

        /// <summary> call this when shoot animation is done </summary>
        public void Shoot()
        {
            Vector2 direction = player.position - firePoint.position;
            GameObject bullet = GameManager.Instance.poolManager.bulletPool
                            .GetBulletFromPool(BulletPool.BulletType.e_bullet, firePoint);
            bullet.GetComponent<Bullet>().Shoot(direction.normalized, bulletForce);
        }


        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void UpdateSprite(Vector2 dir)
        {
            base.UpdateSprite(dir);

            if (sr == null) return;

            if (dir.x > 0.3f && dir.y > 0.3f)
            {
                sr.sprite = upRight[0];
            }
            else if (dir.x < -0.3f && dir.y > 0.3f)
            {
                sr.sprite = upLeft[0];
            }
            else if (dir.x > 0.3f && dir.y < -0.3f)
            {
                sr.sprite = downRight[0];
            }
            else if (dir.x < -0.3f && dir.y < -0.3f)
            {
                sr.sprite = downLeft[0];
            }
            else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    sr.sprite = right[0];
                }
                else
                {
                    sr.sprite = left[0];
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    sr.sprite = up[0];
                }
                else
                {
                    sr.sprite = down[0];
                }
            }
        }
    }
}
