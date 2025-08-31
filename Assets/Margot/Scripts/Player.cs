using System.Collections;
using UnityEngine;
using static Margot.Enemy;

namespace Margot
{
    public class Player : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip clip;
        [Header("Info")]
        public float maxHealth = 30f;
        public float currentHealth = 30f;

        [Header("Movement")]
        public bool cantMove = false;
        public float moveSpeed; 
        [HideInInspector] public Rigidbody2D rb;

        [Header("Shooting")]
        public float attackSpeedSPS = 0.5f;
        [HideInInspector] public float fireCooldown = 0f;
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
            attackSpeedSPS = PlayerAttackInterval(GameManager.Instance.statManager.playerStat.attackSpeedSPS);
            attackDamage = GameManager.Instance.statManager.playerStat.attackDamage;
            maxHealth = GameManager.Instance.statManager.playerStat.maxHealth;
            bulletPerShot = GameManager.Instance.statManager.playerStat.bulletsPerShot;
        }

        float PlayerAttackInterval(float attackSpeed)
        {
            float minInterval = 0.1f; 
            float maxInterval = 1.0f;
            float decayRate = 0.85f;  
            return Mathf.Max(minInterval, maxInterval * Mathf.Pow(decayRate, attackSpeed - 1));
        }

        public void GettingHit(float damage)
        {
            if (detectAttackedCoroutine == null)
            {
                detectAttackedCoroutine = StartCoroutine(DetectAttacked(damage));
            }       
        }

     IEnumerator DetectAttacked(float damage)
{
    Debug.Log("[Player] player got hit");
    currentHealth -= damage;

    // Play hit animation
    GetComponent<Animator>().SetTrigger("GettingHit");

    // Play hit sound
    if (audioSource != null && clip != null)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

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
