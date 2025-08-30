using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        public float attackSpeed = 3f;
        protected Animator anim = null;
        protected SpriteRenderer sr = null;

        [Header("Player")]
        protected Transform player = null;

        [Header("Stat")]
        [SerializeField] protected EnemyStats enemyStat = null;


        //[Header("Drop Items")]


        public event Action<GameObject> OnDeath;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
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

            EnableAttack(false);
        }

        protected virtual void OnEnable()
        {
            // canAttack = true;
            transform.localScale = Vector3.Scale(transform.localScale, GameManager.Instance.resolutionManager.ScaleRatio);
        }


        protected virtual void OnDisable()
        {
            canAttack = false; 

            if (GetComponent<SpriteRenderer>() != null)
            {
                Color color = Color.white;
                GetComponent<SpriteRenderer>().color = color;
            }
        }

        protected virtual void Update()
        {
            if (GameManager.Instance.isGameOver) Dead(true);
        }

        public void UpdateStat()
        {
            Debug.Log("[Enemy] Update enemy stat");
            #region Commented out
            //switch (enemyType)
            //{
            //    case EnemyType.Chase:
            //        thisEnemyStat = GameManager.Instance.statManager.chasingEnemyStat;
            //        moveSpeed = thisEnemyStat.moveSpeed;
            //        maxHealth = thisEnemyStat.maxHealth;
            //        attackDamage = thisEnemyStat.attackDamage;  
            //        break;
            //    case EnemyType.Run:
            //        thisEnemyStat = GameManager.Instance.statManager.runningEnemyStat;
            //        moveSpeed = thisEnemyStat.moveSpeed;
            //        maxHealth = thisEnemyStat.maxHealth;
            //        attackDamage = thisEnemyStat.attackDamage;
            //        break;
            //    case EnemyType.Shoot:
            //        thisEnemyStat = GameManager.Instance.statManager.shootingEnemyStat;
            //        moveSpeed = thisEnemyStat.moveSpeed;
            //        maxHealth = thisEnemyStat.maxHealth;
            //        attackDamage = thisEnemyStat.attackDamage;
            //        break;
            //    default:
            //        Debug.LogError("[Enemy] Undefined enemy type!");
            //        break;
            //}
            #endregion

            enemyStat = GameManager.Instance.statManager.enemyStat;
            moveSpeed = enemyStat.moveSpeed;
            maxHealth = enemyStat.maxHealth;
            attackDamage = enemyStat.attackDamage;
            attackSpeed = EnemyAttackInterval(enemyStat.attackSpeedSPS);
        }


        float EnemyAttackInterval(float attackSpeed)
        {
            float baseInterval = 6.0f; 
            float growthRate = 0.7f;    

            return baseInterval * Mathf.Pow(growthRate, attackSpeed - 1);
        }


        public void EnableAttack(bool val)
        {
            canAttack = val;
            // GetComponent<BoxCollider2D>().isTrigger = !val;
            if (val) anim.SetTrigger("Ready");
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player" && canAttack)
            {
                Debug.Log("[Enemy] Hit Player");
                collision.gameObject.GetComponent<Player>().GettingHit(attackDamage);
            }
        }

        public virtual void OnCollisionEnter2D(Collision2D collision) { }

        protected virtual void UpdateSprite(Vector2 dir) { }

        public void Dead(bool gameover)
        {
            Debug.Log("[Enemy] Dead");
            currentHealth = maxHealth;

            anim.Rebind();
            anim.Update(0f); 

            // ! add dying animation or something here
            GameManager.Instance.poolManager.ReturnToPool(enemyType.ToString() + "Enemy", this.gameObject);

            GameObject bloodParticle = GameManager.Instance.poolManager.TakeFromPool("BloodParticle");
            bloodParticle.SetActive(true);
            bloodParticle.transform.position = this.transform.position;

            if (!gameover) OnDeath?.Invoke(this.gameObject);


            DropItem();
        }

        public void GettingHit()
        {
            if (canAttack)
            {
                Debug.Log("[Enemy] " + enemyType.ToString() + " type enemy got hit");
                currentHealth -= player.GetComponent<Player>().attackDamage;
                anim.SetTrigger("GettingHit");

                if (currentHealth <= 0f) Dead(false);
            }
        }

        public void DropItem()
        {
           
        }
    }

}
