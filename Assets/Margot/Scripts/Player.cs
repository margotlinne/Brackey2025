using UnityEngine;

namespace Margot
{
    public class Player : MonoBehaviour
    {
        [Header("Info")]
        public float maxHealth = 30f;
        public float currentHealth = 10f;

        [Header("Movement")]
        public float moveSpeed = 6f; 
        [HideInInspector] public Rigidbody2D rb;

        [Header("Shooting")]
        public float? attackSpeedSPS = 0.5f;
        [HideInInspector] public float? fireCooldown = 0f;
        public float attackDamage = 5f;
        public int? bulletPerShot = 1;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
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



    }

}
