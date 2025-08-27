using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Shooting Settings")]
        [SerializeField] private float fireRate = 0.5f; 
        private float fireCooldown = 0f;

        [SerializeField] private float bulletForce = 50f; 
        public BulletPool bulletPool;

        private void Update()
        {
            if (fireCooldown > 0f)
                fireCooldown -= Time.deltaTime;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && fireCooldown <= 0f)
            {
                GameObject bullet = bulletPool.GetBulletFromPool();
                bullet.GetComponent<BulletBehaviour>().FireBullet(bulletForce);

                fireCooldown = fireRate; 
            }
        }
    }
}

