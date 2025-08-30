using UnityEngine;

namespace Margot
{
    public class EnemyBullet : Bullet
    {

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (collision.CompareTag("Player"))
            {
                Debug.Log("[BulletBehaviour] enemy bullet collided with player");
                collision.GetComponent<Player>().GettingHit(GameManager.Instance.statManager.enemyStat.attackDamage);
                ReturnBullet();
            }
        }

        protected override void ReturnBullet()
        {
            base.ReturnBullet();

            // add particle or other effect here
            GameManager.Instance.poolManager.ReturnToPool("EnemyBullet", this.gameObject);
        }
    }

}
