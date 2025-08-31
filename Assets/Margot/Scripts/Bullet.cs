using UnityEngine;


namespace Margot
{
    public class Bullet : SoundPlayer
    {
        protected Rigidbody2D rb;
        

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();   
        }

        void OnEnable()
        {
            transform.localScale = Vector3.Scale(transform.localScale, GameManager.Instance.resolutionManager.ScaleRatio);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Wall"))
            {
                Debug.Log("[BulletBehaviour] bullet collided with wall");
                ReturnBullet();
            }

        }

        protected virtual void OnCollisionEnter2D(Collision2D collision) { }

        protected virtual void ReturnBullet()
        {
            GameObject bulletParticle = GameManager.Instance.poolManager.TakeFromPool("BulletParticle");
            bulletParticle.SetActive(true);
            bulletParticle.transform.position = this.transform.position;
        }

        public virtual void Shoot(Vector2 dir, float speed)
        {
            gameObject.SetActive(true);

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(dir * speed, ForceMode2D.Impulse);
        }

    }

}
