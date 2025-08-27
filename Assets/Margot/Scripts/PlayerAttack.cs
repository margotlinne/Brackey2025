using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private float fireRate = 0.5f; 
        private float fireCooldown = 0f;
        public Transform firePoint;

        [SerializeField] private float bulletForce = 50f; 

        public float damage = 10f; // ! temporary, set this value from player stat in the future

        private void Update()
        {
            if (fireCooldown > 0f)
                fireCooldown -= Time.deltaTime;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && fireCooldown <= 0f)
            {
                GameObject bullet = GameManager.Instance.poolManager.bulletPool.GetBulletFromPool(BulletPool.BulletType.p_bullet, firePoint);
                bullet.GetComponent<Bullet>().Shoot(-bullet.transform.right, bulletForce);

                fireCooldown = fireRate; 
            }
        }
    }
}

