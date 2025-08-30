using UnityEngine;

namespace Margot
{
    public class StatManager : MonoBehaviour
    {
        [Header("Enemy")]
        public EnemyStats enemyStat;

        [Header("Player")]
        public PlayerStats playerStat;
        public Player player;

        private void Start()
        {
            InitStats();
        }

        private void InitStats() // initial stats
        {
            // set values in inspector
            //enemyStat.maxHealth = 10f;
            //enemyStat.attackDamage = 3f;
            //enemyStat.moveSpeed = 3f;
            //enemyStat.attackSpeedSPS = 0.5f;
            ////enemyStat.bulletsPerShot = 1;
          

            //playerStat.maxHealth = 30f;
            //playerStat.attackDamage = 5f;
            //playerStat.moveSpeed = 5f;
            //playerStat.attackSpeedSPS = 0.5f;
            //// playerStat.bulletsPerShot = 1;

            player.UpdateStat();
            GameManager.Instance.uiManager.UpdateStatUIText();
        }
    }
}

