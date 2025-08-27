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

        void Update()
        {
            if (player == null) return;
            if (canAttack)
            {
                Vector2 direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                if (Time.time >= nextFireTime)
                {
                    GameObject bullet = GameManager.Instance.poolManager.bulletPool.GetBulletFromPool(BulletPool.BulletType.e_bullet, firePoint);
                    bullet.GetComponent<Bullet>().Shoot(direction.normalized, bulletForce);
                    nextFireTime = Time.time + fireRate;
                }
            }
    
        }

    }

}
