using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Shooting")]
        public Transform firePoint;
        [SerializeField] private float bulletForce = 50f;

        Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (player.fireCooldown > 0f)
                player.fireCooldown -= Time.deltaTime;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && player.fireCooldown <= 0f)
            {
                GameObject bullet = GameManager.Instance.poolManager.bulletPool.GetBulletFromPool(BulletPool.BulletType.p_bullet, firePoint);
                bullet.GetComponent<Bullet>().Shoot(-bullet.transform.right, bulletForce);

                player.fireCooldown = player.attackSpeedSPS; 
            }
        }
    }
}

