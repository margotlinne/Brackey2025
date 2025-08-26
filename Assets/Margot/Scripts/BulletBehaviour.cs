using UnityEngine;


namespace Margot
{
    public class BulletBehaviour : MonoBehaviour
    {
        Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();   
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Boundary"))
            {
                Debug.Log("[BulletBehaviour] bullet collided with boundary");
                // add particle or other effect here
                GameManager.Instance.poolManager.ReturnToPool("Bullet", this.gameObject);
            }
        }

        public void FireBullet(float force)
        {
            gameObject.SetActive(true);

            rb.linearVelocity = Vector2.zero;  
            rb.angularVelocity = 0f;

            rb.AddForce(-transform.right * force, ForceMode2D.Impulse);
        }

    }

}
