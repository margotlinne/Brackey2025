using UnityEngine;
using UnityEngine.Rendering;

namespace Margot
{
    public class PlayerBullet : Bullet
    {
        public AudioSource audioSource;
        public AudioClip clip;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);


            if (collision.gameObject.layer == 6 && collision.gameObject.GetComponent<Enemy>().canAttack) // enemy layer
            {
                Debug.Log("[BulletBehaviour] player bullet collided with enemy");
                GameManager.Instance.shaking.Shake();
                ReturnBullet();
                collision.GetComponent<Enemy>().GettingHit();
                audioSource.clip = clip;
                audioSource.PlayOneShot(clip);
            }
        }
        protected override void ReturnBullet()
        {
            base.ReturnBullet();

            // add particle or other effect here
            GameManager.Instance.poolManager.ReturnToPool("PlayerBullet", this.gameObject);
        }
    }

}
