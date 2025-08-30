using System.Collections;
using UnityEngine;
using static Margot.Enemy;

namespace Margot
{
    public class Player : MonoBehaviour
    {
        [Header("Info")]
        public float maxHealth = 30f;
        public float currentHealth = 30f;

        [Header("Movement")]
        public bool cantMove = false;
        public float moveSpeed; 
        [HideInInspector] public Rigidbody2D rb;

        [Header("Shooting")]
        public float? attackSpeedSPS = 0.5f;
        [HideInInspector] public float? fireCooldown = 0f;
        public float attackDamage;
        public int? bulletPerShot;
        Coroutine detectAttackedCoroutine = null;

        // [Header("Health")]
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        void Update()
        {
            cantMove = GameManager.Instance.uiManager.isCanvasOn;
        }

        public void UpdateStat()
        {
            Debug.Log("[Player] Update player stat");

            moveSpeed = GameManager.Instance.statManager.playerStat.moveSpeed;
            attackSpeedSPS = GameManager.Instance.statManager.playerStat.attackSpeedSPS;
            attackDamage = GameManager.Instance.statManager.playerStat.attackDamage;
            maxHealth = GameManager.Instance.statManager.playerStat.maxHealth;
            bulletPerShot = GameManager.Instance.statManager.playerStat.bulletsPerShot;
        }

        public void GettingHit(float damage)
        {
            if (detectAttackedCoroutine == null)
            {
                StartCoroutine(DetectAttacked(damage));
            }       
        }

        IEnumerator DetectAttacked(float damage)
        {
            Debug.Log("[Player] player got hit");
            currentHealth -= damage;

            if (currentHealth <= 0) Dead();

            yield return new WaitForSeconds(0.3f);
            detectAttackedCoroutine = null;
            yield break;
        }



        public void Dead()
        {
            cantMove = true;
            GameObject bloodParticle = GameManager.Instance.poolManager.TakeFromPool("BloodParticle");
            bloodParticle.SetActive(true);
            bloodParticle.transform.position = this.transform.position;

            GameManager.Instance.uiManager.OpenCanvas(UIManager.CanvasType.gameover);
            gameObject.SetActive(false);
        }

    }

}
