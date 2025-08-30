using System;
using UnityEngine;
using UnityEngine.UI;

namespace Margot
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyType { Chase, Run, Shoot };

        [Header("Info")]
        public EnemyType enemyType;
        public float currentHealth = 0f;
        public bool canAttack = false;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float moveSpeed;
        protected Rigidbody2D rb = null;
        public float attackDamage = 3f;

        [Header("Player")]
        protected Transform player = null;

        [Header("Stat")]
        [SerializeField] protected EnemyStats thisEnemyStat = null;

        //[Header("Drop Items")]


        public event Action<GameObject> OnDeath;

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

        void OnEnable()
        {
            // canAttack = true;
            transform.localScale = Vector3.Scale(transform.localScale, GameManager.Instance.resolutionManager.ScaleRatio);
        }


        void OnDisable()
        {
            canAttack = false;
            if (GetComponent<Image>() != null)
            {
                Color color = Color.white;
                GetComponent<Image>().color = color;
            }
        }

        public void UpdateStat()
        {
            Debug.Log("[Enemy] Update enemy stat");
            switch (enemyType)
            {
                case EnemyType.Chase:
                    thisEnemyStat = GameManager.Instance.statManager.chasingEnemyStat;
                    moveSpeed = thisEnemyStat.moveSpeed;
                    maxHealth = thisEnemyStat.maxHealth;
                    attackDamage = thisEnemyStat.attackDamage;  
                    break;
                case EnemyType.Run:
                    thisEnemyStat = GameManager.Instance.statManager.runningEnemyStat;
                    moveSpeed = thisEnemyStat.moveSpeed;
                    maxHealth = thisEnemyStat.maxHealth;
                    attackDamage = thisEnemyStat.attackDamage;
                    break;
                case EnemyType.Shoot:
                    thisEnemyStat = GameManager.Instance.statManager.shootingEnemyStat;
                    moveSpeed = thisEnemyStat.moveSpeed;
                    maxHealth = thisEnemyStat.maxHealth;
                    attackDamage = thisEnemyStat.attackDamage;
                    break;
                default:
                    Debug.LogError("[Enemy] Undefined enemy type!");
                    break;
            }
        }

        public void EnableAttack(bool val)
        {
            canAttack = val;
            GetComponent<BoxCollider2D>().isTrigger = !val;
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("PlayerBullet") && canAttack)
            {
                GettingHit();
            }
        }

        public virtual void OnCollisionEnter2D(Collision2D collision) 
        { 
            if (collision.gameObject.tag == "Player" && canAttack)
            {
                collision.gameObject.GetComponent<Player>().GettingHit(attackDamage);
            }
        }

        public void Dead()
        {
            currentHealth = maxHealth;
            // ! add dying animation or something here
            GameManager.Instance.poolManager.ReturnToPool(enemyType.ToString() + "Enemy", this.gameObject);

            GameObject bloodParticle = GameManager.Instance.poolManager.TakeFromPool("BloodParticle");
            bloodParticle.SetActive(true);
            bloodParticle.transform.position = this.transform.position;

            OnDeath?.Invoke(this.gameObject);

            DropItem();
        }

        public void GettingHit()
        {
            Debug.Log("[Enemy] " + enemyType.ToString() + " type enemy got hit");
            currentHealth -= player.GetComponent<Player>().attackDamage;

            if (currentHealth <= 0f) Dead();

        }

        public void DropItem()
        {
           
        }
    }

}
