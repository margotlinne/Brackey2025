using UnityEngine;
using UnityEngine.InputSystem;

namespace Margot
{
    public class PlayerAttack : MonoBehaviour
    {
        public BulletPool bulletPool;

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                GameObject bullet = bulletPool.GetBulletFromPool();
                bullet.GetComponent<BulletBehaviour>().FireBullet(50f);
            }
        }
    }
}

