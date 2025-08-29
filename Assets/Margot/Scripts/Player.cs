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
        public float moveSpeed = 6f; 
        [HideInInspector] public Rigidbody2D rb;

        [Header("Shooting")]
        public float? attackSpeedSPS = 0.5f;
        [HideInInspector] public float? fireCooldown = 0f;
        public float attackDamage = 5f;
        public int? bulletPerShot = 1;

        // [Header("Health")]
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
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

        public void GettingHit(float damange)
        {
            Debug.Log("[Player] player got hit");
            currentHealth -= damange;

            if (currentHealth <= 0f) Dead();

        }

        public void Dead()
        {
            GameObject bloodParticle = GameManager.Instance.poolManager.TakeFromPool("BloodParticle");
            bloodParticle.SetActive(true);
            bloodParticle.transform.position = this.transform.position;
        }

    }

}
