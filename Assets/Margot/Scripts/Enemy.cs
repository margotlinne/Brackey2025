using UnityEngine;
using UnityEngine.EventSystems;

namespace Margot
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyType { Chase, Run, Shoot };
        public static System.Action<Enemy.EnemyType> OnAnyEnemyReturned; 

        [Header("Info")]
        public float maxHealth = 10f;
        public float currentHealth = 0f;
        public EnemyType enemyType;
        public float speed = 3f;
        public bool canAttack = false;

        [Header("Player")]
        protected Transform player = null;

        protected Rigidbody2D rb = null;

        protected virtual void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            currentHealth = maxHealth; 
            
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null)
                {
                    player = p.transform;
                }
                else Debug.LogError("[Enemy] There is no player in the scene!");
            }
           
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerBullet"))
            {
                GettingHit();
            }
        }

        public virtual void OnCollisionEnter2D(Collision2D collision) { }

        public void Dead()
        {
            currentHealth = maxHealth;
            // ! add dying animation or something here
            GameManager.Instance.poolManager.ReturnToPool(enemyType.ToString() + "Enemy", this.gameObject);
            GameManager.Instance.waveManager.spawnedEnemyCount--;
        }

        public void GettingHit()
        {
            Debug.Log("[Enemy] " + enemyType.ToString() + " type enemy got hit");
            currentHealth -= player.gameObject.GetComponent<PlayerAttack>().damage;

            if (currentHealth <= 0f) Dead();

            OnAnyEnemyReturned?.Invoke(enemyType); 
        }
    }

}
