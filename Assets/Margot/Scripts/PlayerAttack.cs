using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class PlayerAttack : Sounds
    {

        [Header("Shooting")]
        public Transform firePoint;
        [SerializeField] private float bulletForce = 50f;
        public bool canShoot = false;

        Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (GameManager.Instance.uiManager.rouletteCanvas.activeSelf)
            {
                if (canShoot) canShoot = false; 
            }
            else
            {
                if (!canShoot) canShoot = true;
            }
            
            if (player.fireCooldown > 0f)
                player.fireCooldown -= Time.deltaTime;
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!canShoot) return;
            if (context.phase == InputActionPhase.Started && player.fireCooldown <= 0f)
            {
                GameObject bullet = GameManager.Instance.poolManager.bulletPool.GetBulletFromPool(BulletPool.BulletType.p_bullet, firePoint);
                bullet.GetComponent<Bullet>().Shoot(-bullet.transform.right, bulletForce);
                PlaySound(0);

                player.fireCooldown = player.attackSpeedSPS; 
            }
        }
    }
}

