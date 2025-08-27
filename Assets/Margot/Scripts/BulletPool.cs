using UnityEngine;

namespace Margot {
    public class BulletPool : MonoBehaviour
    {
        public enum BulletType { e_bullet, p_bullet }

        public GameObject player_bulletPrefab;
        public GameObject enemy_bulletPrefab;

        void Start()
        {
            GameManager.Instance.poolManager.InitiatePool(player_bulletPrefab, 100, "PlayerBullet");
            GameManager.Instance.poolManager.InitiatePool(enemy_bulletPrefab, 100, "EnemyBullet");
        }


        public GameObject GetBulletFromPool(BulletType type, Transform spawnPoint)
        {
            GameObject bullet = null;

            if (type == BulletType.e_bullet)
            {
                Debug.Log("[BulletPool] get enemy bullet from pool");
                bullet = GameManager.Instance.poolManager.TakeFromPool("EnemyBullet");
            }
            else if (type == BulletType.p_bullet)
            {
                Debug.Log("[BulletPool] get player bullet from pool");
                bullet = GameManager.Instance.poolManager.TakeFromPool("PlayerBullet");
            }

            bullet.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            return bullet;
        }
    }
}