using UnityEngine;


namespace Margot
{
    public class Bullet : MonoBehaviour
    {
        protected Rigidbody2D rb;
        

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();   
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

        protected virtual void ReturnBullet() { }

        public virtual void Shoot(Vector2 dir, float speed)
        {
            gameObject.SetActive(true);

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.AddForce(dir * speed, ForceMode2D.Impulse);
        }

    }

}
